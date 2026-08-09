using System.Text.Json;
using CareNest.Application.Contracts;
using CareNest.Shared;

namespace CareNest.Infrastructure.Reports;

public sealed class ReportService(ICareNestRepository repository) : IReportService
{
    public async Task<string> CreateProfileDataJsonAsync(
        string profileId,
        string outputPath,
        CancellationToken cancellationToken = default)
    {
        var profile = await repository.GetProfileAsync(profileId, cancellationToken)
            ?? throw new InvalidOperationException("Profile was not found.");

        var medicines = await repository.GetMedicinesAsync(profileId, true, cancellationToken);
        var medicineExports = new List<object>();
        foreach (var medicine in medicines)
        {
            var schedules = await repository.GetSchedulesForMedicineAsync(medicine.Id, cancellationToken);
            var scheduleExports = new List<object>();
            foreach (var schedule in schedules)
            {
                scheduleExports.Add(new
                {
                    Schedule = schedule,
                    Times = await repository.GetScheduleTimesAsync(schedule.Id, cancellationToken)
                });
            }

            medicineExports.Add(new
            {
                Medicine = medicine,
                Schedules = scheduleExports,
                StockAdjustments = await repository.GetStockAdjustmentsAsync(medicine.Id, cancellationToken)
            });
        }

        var payload = new
        {
            Format = "CareNest profile data export",
            FormatVersion = 1,
            ExportedUtc = DateTime.UtcNow,
            Disclaimer = "User-entered organizational data. Not a verified clinical record. CareNest does not diagnose, determine dosage, or recommend treatment.",
            Profile = profile,
            EmergencyContacts = await repository.GetEmergencyContactsAsync(profileId, cancellationToken),
            Medicines = medicineExports,
            MedicationLog = await repository.GetMedicationLogAsync(profileId, cancellationToken: cancellationToken),
            Appointments = await repository.GetAppointmentsAsync(profileId, true, cancellationToken),
            DocumentMetadata = await repository.GetDocumentsAsync(profileId, cancellationToken),
            DocumentContentNote = "Encrypted document contents are not embedded in this JSON. Export selected documents separately from CareNest Documents."
        };

        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        await using var stream = File.Create(outputPath);
        await JsonSerializer.SerializeAsync(
            stream,
            payload,
            new JsonSerializerOptions { WriteIndented = true },
            cancellationToken);
        return outputPath;
    }

    public async Task<string> CreateProfileSummaryPdfAsync(
        string profileId,
        string outputPath,
        CancellationToken cancellationToken = default)
    {
        var profile = await repository.GetProfileAsync(profileId, cancellationToken)
            ?? throw new InvalidOperationException("Profile was not found.");

        var medicines = await repository.GetMedicinesAsync(profileId, false, cancellationToken);
        var appointments = await repository.GetAppointmentsAsync(profileId, true, cancellationToken);
        var documents = await repository.GetDocumentsAsync(profileId, cancellationToken);
        var logs = await repository.GetMedicationLogAsync(
            profileId,
            cancellationToken: cancellationToken);

        var lines = new List<string>
        {
            $"Profile: {profile.Name}",
            $"Generated: {DateTimeOffset.Now:g}",
            "",
            "IMPORTANT:",
            "This report contains user-entered organizational data.",
            "It is not a verified clinical record.",
            "CareNest does not diagnose, determine dosage, recommend treatment,",
            "or replace qualified professionals.",
            "",
            $"Active medicine records: {medicines.Count}",
            $"Appointments in history/upcoming: {appointments.Count}",
            $"Documents organized: {documents.Count}",
            $"Medication log entries: {logs.Count}",
            "",
            "Medicine records:"
        };

        foreach (var medicine in medicines.OrderBy(x => x.Name))
        {
            var estimatedStock = await repository.CalculateCurrentStockAsync(medicine.Id, cancellationToken);
            lines.Add(
                $"- {medicine.Name} | form: {medicine.Form} | " +
                $"strength text: {medicine.StrengthText ?? "not entered"} | state: {medicine.State} | " +
                $"estimated stock: {(estimatedStock?.ToString("0.##") ?? "not tracked")} | " +
                $"refill date: {(medicine.RefillDate?.ToString("yyyy-MM-dd") ?? "not entered")}");
        }

        lines.Add("");
        lines.Add("Upcoming appointments:");

        foreach (var appointment in appointments
                     .Where(x => x.StartsUtc >= DateTime.UtcNow)
                     .OrderBy(x => x.StartsUtc)
                     .Take(20))
        {
            lines.Add(
                $"- {appointment.Title} | {appointment.StartsUtc:u} | " +
                $"{appointment.Location ?? "location not entered"}");
        }

        lines.Add("");
        lines.Add(AppConstants.MedicalDisclaimer);

        await SimplePdfWriter.WriteTextReportAsync(
            outputPath,
            "CareNest profile summary",
            lines,
            cancellationToken);

        return outputPath;
    }

    public async Task<string> CreateMedicationLogCsvAsync(
        string? profileId,
        string outputPath,
        CancellationToken cancellationToken = default)
    {
        var logs = await repository.GetMedicationLogAsync(
            profileId,
            cancellationToken: cancellationToken);

        var medicines = (await repository.GetMedicinesAsync(
            profileId,
            true,
            cancellationToken)).ToDictionary(x => x.Id);

        var profiles = (await repository.GetProfilesAsync(
            true,
            cancellationToken)).ToDictionary(x => x.Id);

        var rows = new List<IReadOnlyList<object?>>
        {
            new object?[] { "CareNest medication log — user-entered/unverified organizational record" },
            new object?[] { "Profile", "Medicine", "Status", "EventUtc", "Note", "ManuallyEdited" }
        };

        rows.AddRange(logs.Select(x => (IReadOnlyList<object?>)new object?[]
        {
            profiles.TryGetValue(x.ProfileId, out var p) ? p.Name : "Unknown profile",
            medicines.TryGetValue(x.MedicineId, out var m) ? m.Name : "Unknown medicine",
            x.Status,
            x.EventUtc.ToString("O"),
            x.Note,
            x.ManuallyEdited
        }));

        await CsvWriter.WriteAsync(outputPath, rows, cancellationToken);
        return outputPath;
    }

    public async Task<string> CreateUpcomingScheduleCsvAsync(
        string? profileId,
        string outputPath,
        CancellationToken cancellationToken = default)
    {
        var from = DateTime.UtcNow;
        var occurrences = await repository.GetOccurrencesAsync(
            from,
            from.AddDays(30),
            profileId,
            cancellationToken);

        var medicines = (await repository.GetMedicinesAsync(
            profileId,
            true,
            cancellationToken)).ToDictionary(x => x.Id);

        var rows = new List<IReadOnlyList<object?>>
        {
            new object?[] { "CareNest upcoming schedule — reminder delivery is not guaranteed" },
            new object?[] { "Medicine", "ScheduledUtc", "LocalScheduledTime", "TimeZone", "State", "FollowUp" }
        };

        rows.AddRange(occurrences.Select(x => (IReadOnlyList<object?>)new object?[]
        {
            medicines.TryGetValue(x.MedicineId, out var m) ? m.Name : "Unknown medicine",
            x.ScheduledUtc.ToString("O"),
            x.LocalScheduledTime.ToString("s"),
            x.TimeZoneId,
            x.State,
            x.FollowUp
        }));

        await CsvWriter.WriteAsync(outputPath, rows, cancellationToken);
        return outputPath;
    }

    public async Task<string> CreateAppointmentHistoryCsvAsync(
        string? profileId,
        string outputPath,
        CancellationToken cancellationToken = default)
    {
        var appointments = await repository.GetAppointmentsAsync(
            profileId,
            true,
            cancellationToken);

        var rows = new List<IReadOnlyList<object?>>
        {
            new object?[] { "CareNest appointment history — user-entered organizational data" },
            new object?[] { "Title", "ClinicianOrFacility", "StartsUtc", "TimeZone", "Location", "FollowUpDate", "Archived" }
        };

        rows.AddRange(appointments.Select(x => (IReadOnlyList<object?>)new object?[]
        {
            x.Title,
            x.ClinicianOrFacility,
            x.StartsUtc.ToString("O"),
            x.TimeZoneId,
            x.Location,
            x.FollowUpDate?.ToString("O"),
            x.Archived
        }));

        await CsvWriter.WriteAsync(outputPath, rows, cancellationToken);
        return outputPath;
    }

    public async Task<string> CreateDocumentListCsvAsync(
        string? profileId,
        string outputPath,
        CancellationToken cancellationToken = default)
    {
        var documents = await repository.GetDocumentsAsync(
            profileId,
            cancellationToken);

        var rows = new List<IReadOnlyList<object?>>
        {
            new object?[] { "CareNest document list — filenames/titles only; no medical interpretation" },
            new object?[] { "Title", "Category", "Folder", "OriginalFileName", "SizeBytes", "CreatedUtc" }
        };

        rows.AddRange(documents.Select(x => (IReadOnlyList<object?>)new object?[]
        {
            x.Title,
            x.Category,
            x.FolderName,
            x.OriginalFileName,
            x.OriginalSizeBytes,
            x.CreatedUtc.ToString("O")
        }));

        await CsvWriter.WriteAsync(outputPath, rows, cancellationToken);
        return outputPath;
    }

    public async Task<string> CreateStockRefillCsvAsync(
        string? profileId,
        string outputPath,
        CancellationToken cancellationToken = default)
    {
        var medicines = await repository.GetMedicinesAsync(profileId, true, cancellationToken);
        var profiles = (await repository.GetProfilesAsync(true, cancellationToken)).ToDictionary(x => x.Id);
        var rows = new List<IReadOnlyList<object?>>
        {
            new object?[] { "CareNest stock/refill report — estimates depend on correct user entries; check actual supply" },
            new object?[] { "Profile", "Medicine", "StartingQuantity", "EstimatedQuantity", "LowStockThreshold", "UserEnteredChangePerTakenEvent", "RefillDate", "State" }
        };

        foreach (var medicine in medicines.OrderBy(x => x.Name))
        {
            var estimated = await repository.CalculateCurrentStockAsync(medicine.Id, cancellationToken);
            rows.Add(new object?[]
            {
                profiles.TryGetValue(medicine.ProfileId, out var profile) ? profile.Name : "Unknown profile",
                medicine.Name,
                medicine.StockCount,
                estimated,
                medicine.RefillThreshold,
                medicine.StockChangePerTakenEvent,
                medicine.RefillDate?.ToString("yyyy-MM-dd"),
                medicine.State
            });
        }

        await CsvWriter.WriteAsync(outputPath, rows, cancellationToken);
        return outputPath;
    }

    public async Task<string> CreateMissedRemindersCsvAsync(
        string? profileId,
        string outputPath,
        CancellationToken cancellationToken = default)
    {
        var to = DateTime.UtcNow;
        var from = to.AddDays(-90);
        var occurrences = await repository.GetOccurrencesAsync(from, to, profileId, cancellationToken);
        var medicines = (await repository.GetMedicinesAsync(profileId, true, cancellationToken)).ToDictionary(x => x.Id);
        var profiles = (await repository.GetProfilesAsync(true, cancellationToken)).ToDictionary(x => x.Id);
        var rows = new List<IReadOnlyList<object?>>
        {
            new object?[] { "CareNest missed reminders — organizational reminder status, not a clinical adherence score" },
            new object?[] { "Profile", "Medicine", "ScheduledUtc", "LocalScheduledTime", "TimeZone", "State" }
        };

        rows.AddRange(
            occurrences
                .Where(x => x.State == CareNest.Domain.Enums.ReminderState.Missed)
                .OrderByDescending(x => x.ScheduledUtc)
                .Select(x => (IReadOnlyList<object?>)new object?[]
                {
                    profiles.TryGetValue(x.ProfileId, out var p) ? p.Name : "Unknown profile",
                    medicines.TryGetValue(x.MedicineId, out var m) ? m.Name : "Unknown medicine",
                    x.ScheduledUtc.ToString("O"),
                    x.LocalScheduledTime.ToString("s"),
                    x.TimeZoneId,
                    x.State
                }));

        await CsvWriter.WriteAsync(outputPath, rows, cancellationToken);
        return outputPath;
    }

}
