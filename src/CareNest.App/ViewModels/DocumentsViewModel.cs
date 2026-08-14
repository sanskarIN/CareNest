using System.Collections.ObjectModel;
using System.Windows.Input;
using CareNest.App.Services;
using CareNest.Application.Contracts;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;

namespace CareNest.App.ViewModels;

public sealed record DocumentRow(
    string Id,
    string Title,
    string ProfileName,
    DocumentCategory Category,
    string? FolderName,
    string OriginalFileName,
    long SizeBytes,
    string Tags)
{
    public string SizeText =>
        SizeBytes switch
        {
            < 1024 => $"{SizeBytes} B",
            < 1024 * 1024 => $"{SizeBytes / 1024d:0.#} KB",
            _ => $"{SizeBytes / 1024d / 1024d:0.#} MB"
        };
}

public sealed class DocumentsViewModel : ObservableViewModel
{
    private readonly IDocumentService _documents;
    private readonly IProfileService _profiles;
    private readonly ICareNestRepository _repository;
    private readonly IAppFileGateway _files;
    private readonly List<DocumentRow> _all = [];

    private PersonProfile? _selectedProfile;
    private string _title = string.Empty;
    private DocumentCategory _category = DocumentCategory.Prescription;
    private string _notes = string.Empty;
    private string _folderName = string.Empty;
    private string _searchText = string.Empty;
    private long _storageUsageBytes;

    public DocumentsViewModel(
        IDocumentService documents,
        IProfileService profiles,
        ICareNestRepository repository,
        IAppFileGateway files,
        IDocumentStore store,
        SafeUiErrorService errors) : base(errors)
    {
        _documents = documents;
        _profiles = profiles;
        _repository = repository;
        _files = files;
        Store = store;

        ImportCommand = new AsyncCommand(ImportAsync);
        CaptureCommand = new AsyncCommand(CaptureAsync);
        ExportCommand = new AsyncCommand<DocumentRow>(ExportAsync);
    }

    private IDocumentStore Store { get; }

    public ObservableCollection<PersonProfile> Profiles { get; } = [];
    public ObservableCollection<DocumentRow> Items { get; } = [];
    public IReadOnlyList<DocumentCategory> Categories { get; } =
        Enum.GetValues<DocumentCategory>();

    public PersonProfile? SelectedProfile
    {
        get => _selectedProfile;
        set => SetProperty(ref _selectedProfile, value);
    }

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public DocumentCategory Category
    {
        get => _category;
        set => SetProperty(ref _category, value);
    }

    public string Notes
    {
        get => _notes;
        set => SetProperty(ref _notes, value);
    }

    public string FolderName
    {
        get => _folderName;
        set => SetProperty(ref _folderName, value);
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                ApplyFilter();
            }
        }
    }

    public long StorageUsageBytes
    {
        get => _storageUsageBytes;
        private set => SetProperty(ref _storageUsageBytes, value);
    }

    public ICommand ImportCommand { get; }
    public ICommand CaptureCommand { get; }
    public ICommand ExportCommand { get; }

    public Task LoadAsync() =>
        RunAsync(
            LoadCoreAsync,
            "CareNest could not load the document organizer.");

    public Task DeleteAsync(DocumentRow row) =>
        RunAsync(async ct =>
        {
            await _documents.DeleteAsync(row.Id, ct);
            await LoadCoreAsync(ct);
        },
        "CareNest could not delete this encrypted document.");

    public Task SetTagsAsync(DocumentRow row, string tagsText) =>
        RunAsync(async ct =>
        {
            var names = tagsText
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Where(x => x.Length <= 60)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Take(20)
                .ToArray();

            var existing = await _repository.GetTagsAsync(ct);
            var ids = new List<string>();

            foreach (var name in names)
            {
                var tag = existing.FirstOrDefault(
                    x => string.Equals(
                        x.Name,
                        name,
                        StringComparison.OrdinalIgnoreCase));

                if (tag is null)
                {
                    tag = new Tag
                    {
                        Name = name
                    };
                    await _repository.SaveTagAsync(tag, ct);
                }

                ids.Add(tag.Id);
            }

            await _repository.SetDocumentTagsAsync(
                row.Id,
                ids,
                ct);

            await LoadCoreAsync(ct);
        },
        "CareNest could not update document tags.");

    private async Task LoadCoreAsync(CancellationToken ct)
    {
        var selectedProfileId = SelectedProfile?.Id;
        Profiles.Clear();
        foreach (var profile in await _profiles.ListAsync(ct))
        {
            Profiles.Add(profile);
        }

        SelectedProfile = selectedProfileId is null
            ? Profiles.FirstOrDefault(x => x.IsPrimary) ?? Profiles.FirstOrDefault()
            : Profiles.FirstOrDefault(x => x.Id == selectedProfileId)
                ?? Profiles.FirstOrDefault(x => x.IsPrimary)
                ?? Profiles.FirstOrDefault();

        var profileNames = Profiles.ToDictionary(x => x.Id);
        var documents = await _documents.ListAsync(null, ct);

        _all.Clear();
        foreach (var document in documents)
        {
            var tags = await _repository.GetDocumentTagsAsync(
                document.Id,
                ct);

            _all.Add(new DocumentRow(
                document.Id,
                document.Title,
                profileNames.TryGetValue(document.ProfileId, out var profile)
                    ? profile.Name
                    : "Unknown profile",
                document.Category,
                document.FolderName,
                document.OriginalFileName,
                document.OriginalSizeBytes,
                string.Join(", ", tags.Select(x => x.Name))));
        }

        StorageUsageBytes = await Store.GetStorageUsageBytesAsync(ct);
        ApplyFilter();
    }

    private Task ImportAsync() =>
        RunAsync(async ct =>
        {
            var file = await _files.PickDocumentAsync(ct);
            if (file is null)
            {
                return;
            }

            await ImportPickedFileAsync(file, ct);
        },
        "CareNest could not import the selected document.");

    private Task CaptureAsync() =>
        RunAsync(async ct =>
        {
            var file = await _files.CapturePhotoAsync(ct);
            if (file is null)
            {
                StatusMessage =
                    "Camera capture is unavailable or was cancelled. You can import an existing file instead.";
                return;
            }

            await ImportPickedFileAsync(file, ct);
        },
        "CareNest could not capture and import the document.");

    private async Task ImportPickedFileAsync(
        PickedFile file,
        CancellationToken cancellationToken)
    {
        if (SelectedProfile is null)
        {
            StatusMessage = "Choose a profile before importing.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Title))
        {
            StatusMessage = "Enter a document title before importing.";
            return;
        }

        var document = await _documents.ImportAsync(
            SelectedProfile.Id,
            Title,
            Category,
            string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim(),
            file,
            cancellationToken);

        document.FolderName = string.IsNullOrWhiteSpace(FolderName) ? null : FolderName.Trim();
        await _repository.SaveDocumentAsync(document, cancellationToken);

        Title = string.Empty;
        Notes = string.Empty;
        FolderName = string.Empty;
        StatusMessage = "Document encrypted and stored locally.";
        await LoadCoreAsync(cancellationToken);
    }

    private Task ExportAsync(DocumentRow? row)
    {
        if (row is null)
        {
            return Task.CompletedTask;
        }

        return RunAsync(async ct =>
        {
            var directory = Path.Combine(FileSystem.Current.CacheDirectory, "Exports");
            Directory.CreateDirectory(directory);
            var path = await _documents.ExportToTemporaryFileAsync(
                row.Id,
                directory,
                ct);

            await _files.ShareFileAsync(
                path,
                "Export CareNest document",
                ct);
        },
        "CareNest could not export this document.");
    }

    private void ApplyFilter()
    {
        var query = SearchText.Trim();

        Items.Clear();
        foreach (var item in _all.Where(x =>
                     query.Length == 0 ||
                     x.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                     (x.FolderName?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                     x.OriginalFileName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                     x.Tags.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                     x.Category.ToString().Contains(query, StringComparison.OrdinalIgnoreCase)))
        {
            Items.Add(item);
        }
    }
}
