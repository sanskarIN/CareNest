using System.Collections.ObjectModel;
using System.Windows.Input;
using CareNest.App.Services;
using CareNest.Application.Contracts;
using CareNest.Domain.Entities;

namespace CareNest.App.ViewModels;

public sealed class ProfileEditorViewModel : ObservableViewModel
{
    private readonly IProfileService _profiles;
    private readonly ICareNestRepository _repository;
    private readonly IDocumentStore _documentStore;
    private readonly IAppFileGateway _files;
    private readonly IAppNavigator _navigator;
    private readonly SemaphoreSlim _photoGate = new(1, 1);
    private string? _profileId;
    private string _name = string.Empty;
    private DateTime _dateOfBirth = DateTime.Today.AddYears(-30);
    private bool _hasDateOfBirth;
    private string _bloodGroup = string.Empty;
    private string _allergies = string.Empty;
    private string _notes = string.Empty;
    private string _profileColor = "#5B7C6F";
    private bool _isPrimary;
    private bool _isExisting;
    private string? _persistedPhotoEncryptedFileName;
    private string? _photoEncryptedFileName;
    private string? _pendingObsoletePhotoEncryptedFileName;
    private string? _photoDisplayPath;

    public ProfileEditorViewModel(
        IProfileService profiles,
        ICareNestRepository repository,
        IDocumentStore documentStore,
        IAppFileGateway files,
        IAppNavigator navigator,
        SafeUiErrorService errors) : base(errors)
    {
        _profiles = profiles;
        _repository = repository;
        _documentStore = documentStore;
        _files = files;
        _navigator = navigator;
        SaveCommand = new AsyncCommand(SaveAsync);
        ChoosePhotoCommand = new AsyncCommand(() => ChangePhotoAsync(capture: false));
        CapturePhotoCommand = new AsyncCommand(() => ChangePhotoAsync(capture: true));
        RemovePhotoCommand = new AsyncCommand(RemovePhotoAsync);
        DeleteContactCommand = new AsyncCommand<EmergencyContact>(DeleteContactAsync);
    }

    public string Name { get => _name; set => SetProperty(ref _name, value); }
    public DateTime DateOfBirth { get => _dateOfBirth; set => SetProperty(ref _dateOfBirth, value); }
    public DateTime Today => DateTime.Today;
    public bool HasDateOfBirth { get => _hasDateOfBirth; set => SetProperty(ref _hasDateOfBirth, value); }
    public string BloodGroup { get => _bloodGroup; set => SetProperty(ref _bloodGroup, value); }
    public string Allergies { get => _allergies; set => SetProperty(ref _allergies, value); }
    public string Notes { get => _notes; set => SetProperty(ref _notes, value); }
    public string ProfileColor { get => _profileColor; set => SetProperty(ref _profileColor, value); }
    public bool IsPrimary { get => _isPrimary; set => SetProperty(ref _isPrimary, value); }
    public bool IsExisting { get => _isExisting; private set => SetProperty(ref _isExisting, value); }
    public string? PhotoDisplayPath { get => _photoDisplayPath; private set => SetProperty(ref _photoDisplayPath, value); }
    public bool HasPhoto => !string.IsNullOrWhiteSpace(PhotoDisplayPath);
    public ObservableCollection<EmergencyContact> EmergencyContacts { get; } = [];

    public ICommand SaveCommand { get; }
    public ICommand ChoosePhotoCommand { get; }
    public ICommand CapturePhotoCommand { get; }
    public ICommand RemovePhotoCommand { get; }
    public ICommand DeleteContactCommand { get; }

    public async Task LoadAsync(string? profileId)
    {
        await DiscardPendingPhotoAsync();
        _profileId = string.IsNullOrWhiteSpace(profileId) ? null : profileId;
        if (_profileId is null)
        {
            _persistedPhotoEncryptedFileName = null;
            _photoEncryptedFileName = null;
            IsExisting = false;
            return;
        }

        await RunAsync(async ct =>
        {
            var profile = await _profiles.GetAsync(_profileId, ct)
                ?? throw new InvalidOperationException("Profile was not found.");
            Name = profile.Name;
            HasDateOfBirth = profile.DateOfBirth is not null;
            DateOfBirth = profile.DateOfBirth ?? DateTime.Today.AddYears(-30);
            BloodGroup = profile.BloodGroup ?? string.Empty;
            Allergies = profile.AllergiesAndSensitivities ?? string.Empty;
            Notes = profile.Notes ?? string.Empty;
            ProfileColor = profile.ProfileColor;
            IsPrimary = profile.IsPrimary;
            IsExisting = true;
            _persistedPhotoEncryptedFileName = profile.PhotoPath;
            _photoEncryptedFileName = profile.PhotoPath;
            await LoadPhotoPreviewAsync(ct);
            await ReloadContactsAsync(ct);
        }, "CareNest could not load this profile.");
    }

    public Task AddEmergencyContactAsync(string name, string? relationship, string? phone, string? notes) =>
        RunAsync(async ct =>
        {
            if (_profileId is null)
            {
                throw new InvalidOperationException("Save the profile before adding an emergency contact.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("A contact name is required.");
            }

            var contact = new EmergencyContact
            {
                ProfileId = _profileId,
                Name = name.Trim(),
                Relationship = Clean(relationship),
                PhoneNumber = Clean(phone),
                Notes = Clean(notes)
            };
            await _repository.SaveEmergencyContactAsync(contact, ct);

            var profile = await _profiles.GetAsync(_profileId, ct);
            if (profile is not null && string.IsNullOrEmpty(profile.EmergencyContactId))
            {
                profile.EmergencyContactId = contact.Id;
                await _profiles.SaveAsync(profile, ct);
            }

            await ReloadContactsAsync(ct);
        }, "CareNest could not save the emergency contact.");

    public Task DeleteAsync() =>
        RunAsync(async ct =>
        {
            await DiscardPendingPhotoAsync();

            if (_profileId is not null)
            {
                await _profiles.DeleteAsync(_profileId, ct);
            }
            await _navigator.GoBackAsync(ct);
        }, "CareNest could not delete this profile and its local records.");

    public async Task DiscardPendingPhotoAsync()
    {
        await _photoGate.WaitAsync();
        try
        {
            try
            {
                await DeleteStagedPhotoIfNeededAsync();
            }
            catch
            {
                // Best-effort cleanup. The staged reference is still detached below.
            }

            await TryDeletePendingObsoletePhotoAsync();
            _photoEncryptedFileName = _persistedPhotoEncryptedFileName;
            TryClearPhotoPreview();
        }
        finally
        {
            _photoGate.Release();
        }
    }

    private Task DeleteContactAsync(EmergencyContact? contact) =>
        RunAsync(async ct =>
        {
            if (contact is null) return;
            await _repository.DeleteEmergencyContactAsync(contact.Id, ct);
            await ReloadContactsAsync(ct);
        }, "CareNest could not delete the emergency contact.");

    private Task ChangePhotoAsync(bool capture) =>
        RunAsync(async ct =>
        {
            var picked = capture
                ? await _files.CapturePhotoAsync(ct)
                : await _files.PickDocumentAsync(ct);
            if (picked is null) return;

            var extension = Path.GetExtension(picked.FileName);
            var isImage = picked.ContentType?.StartsWith("image/", StringComparison.OrdinalIgnoreCase) == true ||
                extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
                extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                extension.Equals(".png", StringComparison.OrdinalIgnoreCase);
            if (!isImage)
            {
                throw new InvalidDataException("Choose a JPEG or PNG image.");
            }

            await _photoGate.WaitAsync(ct);
            try
            {
                await using var input = await picked.OpenReadAsync(ct);
                var stored = await _documentStore.ImportAsync(input, picked.FileName, picked.ContentType, ct);
                if (stored.OriginalSizeBytes > 10 * 1024 * 1024)
                {
                    await _documentStore.DeleteAsync(stored.EncryptedFileName, CancellationToken.None);
                    throw new InvalidDataException("Profile photos must be 10 MB or smaller.");
                }

                try
                {
                    await DeleteStagedPhotoIfNeededAsync();
                }
                catch (Exception previousCleanupFailure)
                {
                    try
                    {
                        await _documentStore.DeleteAsync(stored.EncryptedFileName, CancellationToken.None);
                    }
                    catch (Exception newCleanupFailure)
                    {
                        throw new AggregateException(
                            "CareNest could not replace the staged profile photo and could not fully clean the new encrypted payload.",
                            previousCleanupFailure,
                            newCleanupFailure);
                    }

                    throw;
                }

                _photoEncryptedFileName = stored.EncryptedFileName;
                await LoadPhotoPreviewAsync(ct);
                StatusMessage = "Profile photo encrypted and staged locally. Save the profile to keep this change.";
            }
            finally
            {
                _photoGate.Release();
            }
        }, "CareNest could not store this profile photo.");

    private Task RemovePhotoAsync() =>
        RunAsync(async ct =>
        {
            await _photoGate.WaitAsync(ct);
            try
            {
                await DeleteStagedPhotoIfNeededAsync();
                _photoEncryptedFileName = null;
                TryClearPhotoPreview();
                StatusMessage = "Profile photo removal staged. Save the profile to keep this change.";
            }
            finally
            {
                _photoGate.Release();
            }
        }, "CareNest could not stage profile photo removal.");

    private async Task LoadPhotoPreviewAsync(CancellationToken ct)
    {
        PhotoDisplayPath = null;
        if (string.IsNullOrWhiteSpace(_photoEncryptedFileName))
        {
            TryClearPhotoPreview();
            return;
        }

        var path = PhotoPreviewPath();
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        var partialPath = $"{path}.{Guid.NewGuid():N}.partial";

        try
        {
            await using (var output = File.Create(partialPath))
            {
                await _documentStore.ExportDecryptedAsync(_photoEncryptedFileName, output, ct);
            }

            File.Move(partialPath, path, overwrite: true);
            PhotoDisplayPath = path;
            OnPropertyChanged(nameof(HasPhoto));
        }
        finally
        {
            if (File.Exists(partialPath))
            {
                File.Delete(partialPath);
            }
        }
    }

    private async Task DeleteStagedPhotoIfNeededAsync()
    {
        if (string.IsNullOrWhiteSpace(_photoEncryptedFileName) ||
            string.Equals(
                _photoEncryptedFileName,
                _persistedPhotoEncryptedFileName,
                StringComparison.Ordinal))
        {
            return;
        }

        var staged = _photoEncryptedFileName;
        try
        {
            await _documentStore.DeleteAsync(staged, CancellationToken.None);
        }
        finally
        {
            _photoEncryptedFileName = _persistedPhotoEncryptedFileName;
        }
    }

    private async Task TryDeletePendingObsoletePhotoAsync()
    {
        if (string.IsNullOrWhiteSpace(_pendingObsoletePhotoEncryptedFileName))
        {
            return;
        }

        try
        {
            await _documentStore.DeleteAsync(
                _pendingObsoletePhotoEncryptedFileName,
                CancellationToken.None);
            _pendingObsoletePhotoEncryptedFileName = null;
        }
        catch
        {
            // Best-effort lifecycle cleanup. Full local-data reset also enumerates orphaned .cndoc files.
        }
    }

    private void TryClearPhotoPreview()
    {
        try
        {
            var path = PhotoPreviewPath();
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch
        {
            // Best-effort plaintext cache cleanup must not block profile lifecycle operations.
        }
        finally
        {
            PhotoDisplayPath = null;
            OnPropertyChanged(nameof(HasPhoto));
        }
    }

    private string PhotoPreviewPath()
    {
        var directory = Path.Combine(FileSystem.Current.CacheDirectory, "ProfilePreviews");
        return Path.Combine(directory, $"{_profileId ?? "new"}.img");
    }

    private async Task ReloadContactsAsync(CancellationToken ct)
    {
        EmergencyContacts.Clear();
        if (_profileId is null) return;
        foreach (var contact in await _repository.GetEmergencyContactsAsync(_profileId, ct))
        {
            EmergencyContacts.Add(contact);
        }
    }

    private Task SaveAsync() =>
        RunAsync(async ct =>
        {
            string? obsoletePhoto = null;
            await _photoGate.WaitAsync(ct);
            try
            {
                var profile = _profileId is null
                    ? new PersonProfile()
                    : await _profiles.GetAsync(_profileId, ct)
                        ?? throw new InvalidOperationException("Profile was not found.");
                profile.Name = Name;
                profile.PhotoPath = _photoEncryptedFileName;
                profile.DateOfBirth = HasDateOfBirth ? DateOfBirth.Date : null;
                profile.BloodGroup = Clean(BloodGroup);
                profile.AllergiesAndSensitivities = Clean(Allergies);
                profile.Notes = Clean(Notes);
                profile.ProfileColor = string.IsNullOrWhiteSpace(ProfileColor) ? "#5B7C6F" : ProfileColor.Trim();
                profile.IsPrimary = IsPrimary;

                await _profiles.SaveAsync(profile, ct);

                obsoletePhoto = !string.IsNullOrWhiteSpace(_persistedPhotoEncryptedFileName) &&
                    !string.Equals(
                        _persistedPhotoEncryptedFileName,
                        _photoEncryptedFileName,
                        StringComparison.Ordinal)
                    ? _persistedPhotoEncryptedFileName
                    : null;

                _profileId = profile.Id;
                IsExisting = true;
                _persistedPhotoEncryptedFileName = _photoEncryptedFileName;

                if (obsoletePhoto is not null)
                {
                    _pendingObsoletePhotoEncryptedFileName = obsoletePhoto;
                    await TryDeletePendingObsoletePhotoAsync();
                }
            }
            finally
            {
                _photoGate.Release();
            }

            await _navigator.GoBackAsync(ct);
        }, "CareNest could not save this profile. Check the fields and try again.");

    private static string? Clean(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
