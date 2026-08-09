using System.Collections.ObjectModel;
using System.Windows.Input;
using CareNest.App.Navigation;
using CareNest.App.Services;
using CareNest.Application.Contracts;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;

namespace CareNest.App.ViewModels;

public sealed class MedicineEditorViewModel : ObservableViewModel
{
    private readonly IMedicineService _medicines;
    private readonly IProfileService _profiles;
    private readonly IDocumentService _documents;
    private readonly ICareNestRepository _repository;
    private readonly IAppFileGateway _files;
    private readonly IAppNavigator _navigator;

    private string? _medicineId;
    private PersonProfile? _selectedProfile;
    private string _name = string.Empty;
    private string _form = "Tablet";
    private string _strengthText = string.Empty;
    private string _instructionText = string.Empty;
    private string _prescriberNotes = string.Empty;
    private string _pharmacyNotes = string.Empty;
    private DateTime _startDate = DateTime.Today;
    private DateTime _endDate = DateTime.Today.AddMonths(1);
    private bool _hasEndDate;
    private string _stockCountText = string.Empty;
    private string _refillThresholdText = string.Empty;
    private string _stockChangePerTakenEventText = string.Empty;
    private DateTime _refillDate = DateTime.Today.AddMonths(1);
    private bool _hasRefillDate;
    private MedicineState _state = MedicineState.Active;
    private bool _isExisting;
    private string? _prescriptionDocumentId;
    private string _prescriptionLabel = "No prescription attached";

    public MedicineEditorViewModel(
        IMedicineService medicines,
        IProfileService profiles,
        IDocumentService documents,
        ICareNestRepository repository,
        IAppFileGateway files,
        IAppNavigator navigator,
        SafeUiErrorService errors) : base(errors)
    {
        _medicines = medicines;
        _profiles = profiles;
        _documents = documents;
        _repository = repository;
        _files = files;
        _navigator = navigator;

        SaveCommand = new AsyncCommand(
            () => SaveAsync(openSchedule: false));

        SaveAndScheduleCommand = new AsyncCommand(
            () => SaveAsync(openSchedule: true));
        AttachPrescriptionCommand = new AsyncCommand(AttachPrescriptionAsync);
        ExportPrescriptionCommand = new AsyncCommand(ExportPrescriptionAsync);
        DetachPrescriptionCommand = new AsyncCommand(DetachPrescriptionAsync);
    }

    public ObservableCollection<PersonProfile> Profiles { get; } = [];
    public IReadOnlyList<MedicineState> States { get; } = Enum.GetValues<MedicineState>();

    public PersonProfile? SelectedProfile { get => _selectedProfile; set => SetProperty(ref _selectedProfile, value); }
    public string Name { get => _name; set => SetProperty(ref _name, value); }
    public string Form { get => _form; set => SetProperty(ref _form, value); }
    public string StrengthText { get => _strengthText; set => SetProperty(ref _strengthText, value); }
    public string InstructionText { get => _instructionText; set => SetProperty(ref _instructionText, value); }
    public string PrescriberNotes { get => _prescriberNotes; set => SetProperty(ref _prescriberNotes, value); }
    public string PharmacyNotes { get => _pharmacyNotes; set => SetProperty(ref _pharmacyNotes, value); }
    public DateTime StartDate { get => _startDate; set => SetProperty(ref _startDate, value); }
    public DateTime EndDate { get => _endDate; set => SetProperty(ref _endDate, value); }
    public bool HasEndDate { get => _hasEndDate; set => SetProperty(ref _hasEndDate, value); }
    public string StockCountText { get => _stockCountText; set => SetProperty(ref _stockCountText, value); }
    public string RefillThresholdText { get => _refillThresholdText; set => SetProperty(ref _refillThresholdText, value); }
    public string StockChangePerTakenEventText { get => _stockChangePerTakenEventText; set => SetProperty(ref _stockChangePerTakenEventText, value); }
    public DateTime RefillDate { get => _refillDate; set => SetProperty(ref _refillDate, value); }
    public bool HasRefillDate { get => _hasRefillDate; set => SetProperty(ref _hasRefillDate, value); }
    public MedicineState State { get => _state; set => SetProperty(ref _state, value); }
    public bool IsExisting { get => _isExisting; private set => SetProperty(ref _isExisting, value); }
    public string PrescriptionLabel { get => _prescriptionLabel; private set => SetProperty(ref _prescriptionLabel, value); }
    public bool HasPrescription => !string.IsNullOrWhiteSpace(_prescriptionDocumentId);

    public ICommand SaveCommand { get; }
    public ICommand SaveAndScheduleCommand { get; }
    public ICommand AttachPrescriptionCommand { get; }
    public ICommand ExportPrescriptionCommand { get; }
    public ICommand DetachPrescriptionCommand { get; }

    public async Task LoadAsync(string? medicineId)
    {
        _medicineId = string.IsNullOrWhiteSpace(medicineId) ? null : medicineId;

        await RunAsync(async ct =>
        {
            Profiles.Clear();
            foreach (var profile in await _profiles.ListAsync(ct))
            {
                Profiles.Add(profile);
            }

            if (_medicineId is null)
            {
                SelectedProfile = Profiles.FirstOrDefault(x => x.IsPrimary)
                    ?? Profiles.FirstOrDefault();
                IsExisting = false;
                return;
            }

            var medicine = await _medicines.GetAsync(_medicineId, ct)
                ?? throw new InvalidOperationException("Medicine record was not found.");

            SelectedProfile = Profiles.FirstOrDefault(x => x.Id == medicine.ProfileId);
            Name = medicine.Name;
            Form = medicine.Form;
            StrengthText = medicine.StrengthText ?? string.Empty;
            InstructionText = medicine.InstructionText ?? string.Empty;
            PrescriberNotes = medicine.PrescriberNotes ?? string.Empty;
            PharmacyNotes = medicine.PharmacyNotes ?? string.Empty;
            StartDate = medicine.StartDate;
            HasEndDate = medicine.EndDate is not null;
            EndDate = medicine.EndDate ?? medicine.StartDate.AddMonths(1);
            StockCountText = medicine.StockCount?.ToString("0.##") ?? string.Empty;
            RefillThresholdText = medicine.RefillThreshold?.ToString("0.##") ?? string.Empty;
            StockChangePerTakenEventText = medicine.StockChangePerTakenEvent?.ToString("0.##") ?? string.Empty;
            HasRefillDate = medicine.RefillDate is not null;
            RefillDate = medicine.RefillDate ?? DateTime.Today.AddMonths(1);
            State = medicine.State;
            _prescriptionDocumentId = medicine.PrescriptionDocumentId;
            if (!string.IsNullOrWhiteSpace(_prescriptionDocumentId))
            {
                var document = await _repository.GetDocumentAsync(_prescriptionDocumentId, ct);
                PrescriptionLabel = document?.Title ?? "Attached prescription";
            }
            OnPropertyChanged(nameof(HasPrescription));
            IsExisting = true;
        },
        "CareNest could not load this medicine record.");
    }

    public Task DeleteAsync() =>
        RunAsync(async ct =>
        {
            if (_medicineId is null)
            {
                await _navigator.GoBackAsync(ct);
                return;
            }

            await _medicines.DeleteAsync(_medicineId, ct);
            await _navigator.GoBackAsync(ct);
        },
        "CareNest could not delete this medicine record.");

    public Task AddStockCorrectionAsync(decimal delta, string? reason) =>
        RunAsync(async ct =>
        {
            if (_medicineId is null)
            {
                StatusMessage = "Save the medicine record before entering a stock correction.";
                return;
            }

            await _medicines.ApplyStockAdjustmentAsync(
                _medicineId,
                delta,
                reason,
                cancellationToken: ct);

            var stock = await _repository.CalculateCurrentStockAsync(
                _medicineId,
                ct);

            StatusMessage = stock is null
                ? "Stock is not enabled for this record."
                : $"Estimated stock is now {stock:0.##}. Check the actual supply.";
        },
        "CareNest could not apply the stock correction.");


    private Task AttachPrescriptionAsync() =>
        RunAsync(async ct =>
        {
            if (SelectedProfile is null)
            {
                throw new InvalidOperationException("Choose a profile before importing a prescription.");
            }

            var picked = await _files.PickDocumentAsync(ct);
            if (picked is null) return;

            var title = string.IsNullOrWhiteSpace(Name)
                ? $"Prescription - {picked.FileName}"
                : $"{Name.Trim()} prescription - {picked.FileName}";

            var document = await _documents.ImportAsync(
                SelectedProfile.Id,
                title,
                DocumentCategory.Prescription,
                "Imported as a medicine prescription attachment. CareNest does not interpret this file.",
                picked,
                ct);

            _prescriptionDocumentId = document.Id;
            PrescriptionLabel = document.Title;
            OnPropertyChanged(nameof(HasPrescription));
            StatusMessage = "Prescription encrypted and stored locally. Save the medicine record to keep the association.";
        }, "CareNest could not import the prescription.");

    private Task ExportPrescriptionAsync() =>
        RunAsync(async ct =>
        {
            if (string.IsNullOrWhiteSpace(_prescriptionDocumentId)) return;
            var directory = Path.Combine(FileSystem.Current.CacheDirectory, "Exports");
            Directory.CreateDirectory(directory);
            var path = await _documents.ExportToTemporaryFileAsync(_prescriptionDocumentId, directory, ct);
            await _files.ShareFileAsync(path, "Export CareNest prescription", ct);
        }, "CareNest could not export the prescription.");

    private Task DetachPrescriptionAsync() =>
        RunAsync(ct =>
        {
            ct.ThrowIfCancellationRequested();
            _prescriptionDocumentId = null;
            PrescriptionLabel = "No prescription attached";
            OnPropertyChanged(nameof(HasPrescription));
            StatusMessage = "Prescription detached from this medicine record. The encrypted document remains in Documents until you delete it there.";
            return Task.CompletedTask;
        }, "CareNest could not detach the prescription.");

    private Task SaveAsync(bool openSchedule) =>
        RunAsync(async ct =>
        {
            if (SelectedProfile is null)
            {
                StatusMessage = "Choose a local profile.";
                return;
            }

            var stock = ParseOptionalDecimal(
                StockCountText,
                "Starting stock");

            var threshold = ParseOptionalDecimal(
                RefillThresholdText,
                "Refill threshold");

            var perTaken = ParseOptionalDecimal(
                StockChangePerTakenEventText,
                "Stock change per taken event");

            var medicine = _medicineId is null
                ? new Medicine()
                : await _medicines.GetAsync(_medicineId, ct)
                    ?? throw new InvalidOperationException("Medicine record was not found.");

            medicine.ProfileId = SelectedProfile.Id;
            medicine.Name = Name;
            medicine.Form = Form;
            medicine.StrengthText = NullIfBlank(StrengthText);
            medicine.InstructionText = NullIfBlank(InstructionText);
            medicine.PrescriberNotes = NullIfBlank(PrescriberNotes);
            medicine.PharmacyNotes = NullIfBlank(PharmacyNotes);
            medicine.StartDate = StartDate.Date;
            medicine.EndDate = HasEndDate ? EndDate.Date : null;
            medicine.StockCount = stock;
            medicine.RefillThreshold = threshold;
            medicine.StockChangePerTakenEvent = perTaken;
            medicine.RefillDate = HasRefillDate ? RefillDate.Date : null;
            medicine.PrescriptionDocumentId = _prescriptionDocumentId;
            medicine.State = State;

            await _medicines.SaveAsync(medicine, ct);
            _medicineId = medicine.Id;
            IsExisting = true;

            if (openSchedule)
            {
                await _navigator.GoToAsync(
                    RouteNames.ScheduleEditor,
                    new Dictionary<string, object>
                    {
                        ["MedicineId"] = medicine.Id
                    },
                    ct);
            }
            else
            {
                await _navigator.GoBackAsync(ct);
            }
        },
        "CareNest could not save this medicine record. Check the fields and try again.");

    private static decimal? ParseOptionalDecimal(
        string value,
        string label)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (!decimal.TryParse(value, out var parsed) || parsed < 0)
        {
            throw new ArgumentException(
                $"{label} must be a non-negative number.");
        }

        return parsed;
    }

    private static string? NullIfBlank(string value) =>
        string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
}
