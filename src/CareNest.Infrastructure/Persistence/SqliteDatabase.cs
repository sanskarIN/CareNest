using CareNest.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using SQLite;

namespace CareNest.Infrastructure.Persistence;

public sealed class SqliteDatabase : IAsyncDisposable
{
    private readonly CareNestStorageOptions _options;
    private readonly ILogger<SqliteDatabase> _logger;
    private readonly SemaphoreSlim _initGate = new(1, 1);
    private SQLiteAsyncConnection? _connection;
    private bool _initialized;

    public SqliteDatabase(CareNestStorageOptions options, ILogger<SqliteDatabase> logger)
    {
        _options = options;
        _logger = logger;
        _options.EnsureDirectories();
    }

    public string DatabasePath => _options.DatabasePath;
    public string DocumentDirectory => _options.DocumentDirectory;
    public string WorkingDirectory => _options.WorkingDirectory;

    public SQLiteAsyncConnection Connection =>
        _connection ?? throw new InvalidOperationException("Database is not initialized.");

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (_initialized)
        {
            return;
        }

        await _initGate.WaitAsync(cancellationToken);
        try
        {
            if (_initialized)
            {
                return;
            }

            _connection = CreateConnection();
            await ConfigureConnectionAsync(_connection);

            await ApplyMigrationsAsync(cancellationToken);
            _initialized = true;
        }
        finally
        {
            _initGate.Release();
        }
    }

    public async Task<int> GetSchemaVersionAsync(CancellationToken cancellationToken = default)
    {
        await InitializeAsync(cancellationToken);
        var rows = await Connection.QueryAsync<SchemaInfo>("SELECT Version, AppliedUtc FROM SchemaInfo ORDER BY Version DESC LIMIT 1");
        return rows.Count == 0 ? 0 : rows[0].Version;
    }

    public async Task CreateSnapshotAsync(string destinationPath, CancellationToken cancellationToken = default)
    {
        await InitializeAsync(cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();

        if (File.Exists(destinationPath))
        {
            File.Delete(destinationPath);
        }

        var escaped = destinationPath.Replace("'", "''", StringComparison.Ordinal);

        // wal_checkpoint returns a result row (busy/log/checkpointed). Consume the
        // first scalar rather than executing it as a non-query; sqlite-net otherwise
        // surfaces SQLITE_ROW as a misleading "not an error" exception.
        _ = await Connection.ExecuteScalarAsync<int>("PRAGMA wal_checkpoint(FULL);");
        await Connection.ExecuteAsync($"VACUUM INTO '{escaped}';");
    }

    public async Task ReplaceDatabaseAsync(string validatedDatabasePath, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _initGate.WaitAsync(cancellationToken);
        try
        {
            if (_connection is not null)
            {
                await _connection.CloseAsync();
                _connection = null;
            }

            var backupPath = DatabasePath + ".pre-restore";
            if (File.Exists(backupPath))
            {
                File.Delete(backupPath);
            }

            if (File.Exists(DatabasePath))
            {
                File.Move(DatabasePath, backupPath);
            }

            try
            {
                File.Copy(validatedDatabasePath, DatabasePath, overwrite: true);
                DeleteSidecars(DatabasePath);
                _connection = CreateConnection();
                await ConfigureConnectionAsync(_connection);
                _initialized = false;
                await ApplyMigrationsAsync(cancellationToken);
                _initialized = true;

                if (File.Exists(backupPath))
                {
                    File.Delete(backupPath);
                }
            }
            catch
            {
                if (_connection is not null)
                {
                    await _connection.CloseAsync();
                    _connection = null;
                }

                if (File.Exists(DatabasePath))
                {
                    File.Delete(DatabasePath);
                }

                if (File.Exists(backupPath))
                {
                    File.Move(backupPath, DatabasePath);
                }

                _connection = CreateConnection();
                await ConfigureConnectionAsync(_connection);
                _initialized = false;
                await ApplyMigrationsAsync(CancellationToken.None);
                _initialized = true;
                throw;
            }
        }
        finally
        {
            _initGate.Release();
        }
    }

    private SQLiteAsyncConnection CreateConnection() =>
        new(DatabasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);

    private static async Task ConfigureConnectionAsync(SQLiteAsyncConnection connection)
    {
        await connection.ExecuteAsync("PRAGMA foreign_keys = ON;");

        // journal_mode and busy_timeout are result-producing pragmas. Reading
        // their returned values avoids sqlite-net treating SQLITE_ROW as a
        // non-query failure on native providers used by CI and device builds.
        var journalMode = await connection.ExecuteScalarAsync<string>("PRAGMA journal_mode = WAL;");
        if (!string.Equals(journalMode, "wal", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("CareNest could not enable SQLite WAL journal mode.");
        }

        await connection.ExecuteAsync("PRAGMA synchronous = NORMAL;");

        var busyTimeout = await connection.ExecuteScalarAsync<int>("PRAGMA busy_timeout = 5000;");
        if (busyTimeout < 5000)
        {
            throw new InvalidOperationException("CareNest could not configure the SQLite busy timeout.");
        }
    }

    private async Task ApplyMigrationsAsync(CancellationToken cancellationToken)
    {
        if (_connection is null)
        {
            throw new InvalidOperationException("Connection is unavailable.");
        }

        await _connection.ExecuteAsync("""
            CREATE TABLE IF NOT EXISTS SchemaInfo (
                Version INTEGER PRIMARY KEY NOT NULL,
                AppliedUtc TEXT NOT NULL
            );
            """);

        var current = await GetCurrentVersionWithoutInitializeAsync();

        if (current < 1)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach (var statement in Migration1Statements)
            {
                await _connection.ExecuteAsync(statement);
            }
            await RecordVersionAsync(1);
            current = 1;
        }

        if (current < 2)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach (var statement in Migration2Statements)
            {
                await _connection.ExecuteAsync(statement);
            }
            await RecordVersionAsync(2);
            current = 2;
        }

        if (current < 3)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach (var statement in Migration3Statements)
            {
                await _connection.ExecuteAsync(statement);
            }
            await RecordVersionAsync(3);
            current = 3;
        }

        if (current < 4)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach (var statement in Migration4Statements)
            {
                await _connection.ExecuteAsync(statement);
            }
            await RecordVersionAsync(4);
            current = 4;
        }

        if (current < 5)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach (var statement in Migration5Statements)
            {
                await _connection.ExecuteAsync(statement);
            }
            await RecordVersionAsync(5);
        }

        var integrity = await _connection.ExecuteScalarAsync<string>("PRAGMA integrity_check;");
        if (!string.Equals(integrity, "ok", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogError("SQLite integrity check failed during initialization.");
            throw new InvalidDataException("The local CareNest database failed an integrity check.");
        }
    }

    private async Task<int> GetCurrentVersionWithoutInitializeAsync()
    {
        var rows = await _connection!.QueryAsync<SchemaInfo>("SELECT Version, AppliedUtc FROM SchemaInfo ORDER BY Version DESC LIMIT 1");
        return rows.Count == 0 ? 0 : rows[0].Version;
    }

    private Task<int> RecordVersionAsync(int version) =>
        _connection!.ExecuteAsync("INSERT OR REPLACE INTO SchemaInfo (Version, AppliedUtc) VALUES (?, ?)", version, DateTime.UtcNow);

    private static void DeleteSidecars(string databasePath)
    {
        foreach (var suffix in new[] { "-wal", "-shm" })
        {
            var sidecar = databasePath + suffix;
            if (File.Exists(sidecar))
            {
                File.Delete(sidecar);
            }
        }
    }

    private static readonly string[] Migration1Statements =
    [
        """
        CREATE TABLE IF NOT EXISTS PersonProfile (
            Id TEXT PRIMARY KEY NOT NULL, CreatedUtc TEXT NOT NULL, UpdatedUtc TEXT NOT NULL,
            Name TEXT NOT NULL, PhotoPath TEXT NULL, DateOfBirth TEXT NULL, BloodGroup TEXT NULL,
            AllergiesAndSensitivities TEXT NULL, EmergencyContactId TEXT NULL, Notes TEXT NULL,
            ProfileColor TEXT NOT NULL, ProfileIcon TEXT NOT NULL, IsPrimary INTEGER NOT NULL, IsArchived INTEGER NOT NULL
        );
        """,
        """
        CREATE TABLE IF NOT EXISTS Medicine (
            Id TEXT PRIMARY KEY NOT NULL, CreatedUtc TEXT NOT NULL, UpdatedUtc TEXT NOT NULL,
            ProfileId TEXT NOT NULL, Name TEXT NOT NULL, Form TEXT NOT NULL, StrengthText TEXT NULL,
            InstructionText TEXT NULL, PrescriberNotes TEXT NULL, PharmacyNotes TEXT NULL,
            StartDate TEXT NOT NULL, EndDate TEXT NULL, StockCount REAL NULL, RefillThreshold REAL NULL,
            PrescriptionDocumentId TEXT NULL, State INTEGER NOT NULL
        );
        """,
        """
        CREATE TABLE IF NOT EXISTS MedicineSchedule (
            Id TEXT PRIMARY KEY NOT NULL, CreatedUtc TEXT NOT NULL, UpdatedUtc TEXT NOT NULL,
            MedicineId TEXT NOT NULL, Kind INTEGER NOT NULL, StartDate TEXT NOT NULL, EndDate TEXT NULL,
            IntervalHours INTEGER NULL, CycleOnDays INTEGER NULL, CycleOffDays INTEGER NULL,
            WeekdayMask INTEGER NOT NULL, TimeZoneId TEXT NOT NULL, FollowUpMinutes INTEGER NULL, Enabled INTEGER NOT NULL
        );
        """,
        """
        CREATE TABLE IF NOT EXISTS ScheduleTime (
            Id TEXT PRIMARY KEY NOT NULL, CreatedUtc TEXT NOT NULL, UpdatedUtc TEXT NOT NULL,
            MedicineScheduleId TEXT NOT NULL, Hour INTEGER NOT NULL, Minute INTEGER NOT NULL
        );
        """,
        """
        CREATE TABLE IF NOT EXISTS ReminderOccurrence (
            Id TEXT PRIMARY KEY NOT NULL, CreatedUtc TEXT NOT NULL, UpdatedUtc TEXT NOT NULL,
            ScheduleId TEXT NOT NULL, MedicineId TEXT NOT NULL, ProfileId TEXT NOT NULL,
            OccurrenceKey TEXT NOT NULL UNIQUE, ScheduledUtc TEXT NOT NULL, LocalScheduledTime TEXT NOT NULL,
            TimeZoneId TEXT NOT NULL, State INTEGER NOT NULL, StateChangedUtc TEXT NULL,
            SnoozedUntilUtc TEXT NULL, PlatformNotificationId TEXT NULL, FollowUp INTEGER NOT NULL
        );
        """,
        """
        CREATE TABLE IF NOT EXISTS MedicationLogEntry (
            Id TEXT PRIMARY KEY NOT NULL, CreatedUtc TEXT NOT NULL, UpdatedUtc TEXT NOT NULL,
            ProfileId TEXT NOT NULL, MedicineId TEXT NOT NULL, ReminderOccurrenceId TEXT NULL,
            Status INTEGER NOT NULL, EventUtc TEXT NOT NULL, Note TEXT NULL, ManuallyEdited INTEGER NOT NULL
        );
        """,
        """
        CREATE TABLE IF NOT EXISTS Appointment (
            Id TEXT PRIMARY KEY NOT NULL, CreatedUtc TEXT NOT NULL, UpdatedUtc TEXT NOT NULL,
            ProfileId TEXT NOT NULL, Title TEXT NOT NULL, ClinicianOrFacility TEXT NULL,
            StartsUtc TEXT NOT NULL, TimeZoneId TEXT NOT NULL, Location TEXT NULL, PreparationNotes TEXT NULL,
            QuestionsToAsk TEXT NULL, AttachmentDocumentId TEXT NULL, FollowUpDate TEXT NULL,
            ReminderMinutesBefore INTEGER NULL, Archived INTEGER NOT NULL
        );
        """,
        """
        CREATE TABLE IF NOT EXISTS CareDocument (
            Id TEXT PRIMARY KEY NOT NULL, CreatedUtc TEXT NOT NULL, UpdatedUtc TEXT NOT NULL,
            ProfileId TEXT NOT NULL, Title TEXT NOT NULL, Category INTEGER NOT NULL,
            EncryptedFileName TEXT NOT NULL UNIQUE, OriginalFileName TEXT NOT NULL, ContentType TEXT NULL,
            OriginalSizeBytes INTEGER NOT NULL, Sha256 TEXT NOT NULL, EncryptionVersion INTEGER NOT NULL,
            Notes TEXT NULL
        );
        """,
        """
        CREATE TABLE IF NOT EXISTS Tag (
            Id TEXT PRIMARY KEY NOT NULL, CreatedUtc TEXT NOT NULL, UpdatedUtc TEXT NOT NULL,
            Name TEXT NOT NULL UNIQUE
        );
        """,
        """
        CREATE TABLE IF NOT EXISTS DocumentTag (
            DocumentId TEXT NOT NULL, TagId TEXT NOT NULL,
            PRIMARY KEY (DocumentId, TagId)
        );
        """,
        """
        CREATE TABLE IF NOT EXISTS StockAdjustment (
            Id TEXT PRIMARY KEY NOT NULL, CreatedUtc TEXT NOT NULL, UpdatedUtc TEXT NOT NULL,
            MedicineId TEXT NOT NULL, QuantityDelta REAL NOT NULL, EventUtc TEXT NOT NULL,
            Reason TEXT NULL, MedicationLogEntryId TEXT NULL
        );
        """,
        """
        CREATE TABLE IF NOT EXISTS EmergencyContact (
            Id TEXT PRIMARY KEY NOT NULL, CreatedUtc TEXT NOT NULL, UpdatedUtc TEXT NOT NULL,
            ProfileId TEXT NOT NULL, Name TEXT NOT NULL, Relationship TEXT NULL,
            PhoneNumber TEXT NULL, Notes TEXT NULL
        );
        """,
        """
        CREATE TABLE IF NOT EXISTS AppSetting (
            Key TEXT PRIMARY KEY NOT NULL, Value TEXT NOT NULL, UpdatedUtc TEXT NOT NULL
        );
        """,
        """
        CREATE TABLE IF NOT EXISTS BackupMetadata (
            Id TEXT PRIMARY KEY NOT NULL, CreatedUtc TEXT NOT NULL, UpdatedUtc TEXT NOT NULL,
            FormatVersion INTEGER NOT NULL, SchemaVersion INTEGER NOT NULL, CreatedAtUtc TEXT NOT NULL,
            AppVersion TEXT NOT NULL, DestinationHint TEXT NULL
        );
        """,
        """
        CREATE TABLE IF NOT EXISTS AuditEntry (
            Id TEXT PRIMARY KEY NOT NULL, CreatedUtc TEXT NOT NULL, UpdatedUtc TEXT NOT NULL,
            EntityType TEXT NOT NULL, EntityId TEXT NOT NULL, Action INTEGER NOT NULL, EventUtc TEXT NOT NULL,
            ChangedFieldsCsv TEXT NULL, SafeSummary TEXT NULL
        );
        """
    ];

    private static readonly string[] Migration2Statements =
    [
        "CREATE INDEX IF NOT EXISTS IX_Medicine_ProfileId ON Medicine(ProfileId);",
        "CREATE INDEX IF NOT EXISTS IX_MedicineSchedule_MedicineId ON MedicineSchedule(MedicineId);",
        "CREATE INDEX IF NOT EXISTS IX_ScheduleTime_ScheduleId ON ScheduleTime(MedicineScheduleId);",
        "CREATE INDEX IF NOT EXISTS IX_Occurrence_Time ON ReminderOccurrence(ScheduledUtc);",
        "CREATE INDEX IF NOT EXISTS IX_Occurrence_Profile_Time ON ReminderOccurrence(ProfileId, ScheduledUtc);",
        "CREATE INDEX IF NOT EXISTS IX_Log_Profile_Event ON MedicationLogEntry(ProfileId, EventUtc);",
        "CREATE INDEX IF NOT EXISTS IX_Appointment_Profile_Start ON Appointment(ProfileId, StartsUtc);",
        "CREATE INDEX IF NOT EXISTS IX_Document_Profile ON CareDocument(ProfileId);",
        "CREATE INDEX IF NOT EXISTS IX_Stock_Medicine_Event ON StockAdjustment(MedicineId, EventUtc);"
    ];

    private static readonly string[] Migration3Statements =
    [
        "CREATE INDEX IF NOT EXISTS IX_Audit_Entity ON AuditEntry(EntityType, EntityId, EventUtc);",
        "CREATE INDEX IF NOT EXISTS IX_Occurrence_Schedule ON ReminderOccurrence(ScheduleId, ScheduledUtc);",
        "CREATE INDEX IF NOT EXISTS IX_Occurrence_Key ON ReminderOccurrence(OccurrenceKey);"
    ];

    private static readonly string[] Migration4Statements =
    [
        "ALTER TABLE Medicine ADD COLUMN StockChangePerTakenEvent REAL NULL;",
        "ALTER TABLE Medicine ADD COLUMN RefillDate TEXT NULL;"
    ];

    private static readonly string[] Migration5Statements =
    [
        "ALTER TABLE CareDocument ADD COLUMN FolderName TEXT NULL;"
    ];

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
        {
            await _connection.CloseAsync();
        }
        _initGate.Dispose();
    }
}
