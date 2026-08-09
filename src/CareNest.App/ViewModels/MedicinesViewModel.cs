using System.Collections.ObjectModel;
using System.Windows.Input;
using CareNest.App.Navigation;
using CareNest.App.Services;
using CareNest.Application.Contracts;
using CareNest.Domain.Entities;

namespace CareNest.App.ViewModels;

public sealed record MedicineRow(
    string Id,
    string Name,
    string Form,
    string? StrengthText,
    string ProfileName,
    string State,
    decimal? EstimatedStock,
    decimal? RefillThreshold)
{
    public string StockText =>
        EstimatedStock is null
            ? "Stock not tracked"
            : $"Estimated stock: {EstimatedStock:0.##}";

    public bool LowStock =>
        EstimatedStock is not null &&
        RefillThreshold is not null &&
        EstimatedStock <= RefillThreshold;
}

public sealed class MedicinesViewModel : ObservableViewModel
{
    private readonly IMedicineService _medicines;
    private readonly IProfileService _profiles;
    private readonly ICareNestRepository _repository;
    private readonly IAppNavigator _navigator;

    public MedicinesViewModel(
        IMedicineService medicines,
        IProfileService profiles,
        ICareNestRepository repository,
        IAppNavigator navigator,
        SafeUiErrorService errors) : base(errors)
    {
        _medicines = medicines;
        _profiles = profiles;
        _repository = repository;
        _navigator = navigator;

        AddCommand = new AsyncCommand(
            () => _navigator.GoToAsync(RouteNames.MedicineEditor));

        EditCommand = new AsyncCommand<MedicineRow>(
            row => row is null
                ? Task.CompletedTask
                : _navigator.GoToAsync(
                    RouteNames.MedicineEditor,
                    new Dictionary<string, object>
                    {
                        ["MedicineId"] = row.Id
                    }));
    }

    public ObservableCollection<MedicineRow> Items { get; } = [];
    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }

    public Task LoadAsync() =>
        RunAsync(async ct =>
        {
            var medicines = await _medicines.ListAsync(null, ct);
            var profiles = (await _profiles.ListAsync(ct))
                .ToDictionary(x => x.Id);

            Items.Clear();

            foreach (var medicine in medicines)
            {
                var stock = await _repository.CalculateCurrentStockAsync(
                    medicine.Id,
                    ct);

                Items.Add(new MedicineRow(
                    medicine.Id,
                    medicine.Name,
                    medicine.Form,
                    medicine.StrengthText,
                    profiles.TryGetValue(medicine.ProfileId, out var profile)
                        ? profile.Name
                        : "Unknown profile",
                    medicine.State.ToString(),
                    stock,
                    medicine.RefillThreshold));
            }
        },
        "CareNest could not load medicine records.");
}
