using System.Collections.ObjectModel;
using System.Windows.Input;
using CareNest.App.Navigation;
using CareNest.App.Services;
using CareNest.Application.Contracts;
using CareNest.Domain.Entities;

namespace CareNest.App.ViewModels;

public sealed class ProfilesViewModel : ObservableViewModel
{
    private readonly IProfileService _profiles;
    private readonly IAppNavigator _navigator;

    public ProfilesViewModel(
        IProfileService profiles,
        IAppNavigator navigator,
        SafeUiErrorService errors) : base(errors)
    {
        _profiles = profiles;
        _navigator = navigator;

        AddCommand = new AsyncCommand(
            () => _navigator.GoToAsync(RouteNames.ProfileEditor));

        EditCommand = new AsyncCommand<PersonProfile>(
            profile => profile is null
                ? Task.CompletedTask
                : _navigator.GoToAsync(
                    RouteNames.ProfileEditor,
                    new Dictionary<string, object>
                    {
                        ["ProfileId"] = profile.Id
                    }));
    }

    public ObservableCollection<PersonProfile> Profiles { get; } = [];
    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }

    public Task LoadAsync() =>
        RunAsync(async ct =>
        {
            var profiles = await _profiles.ListAsync(ct);
            Profiles.Clear();
            foreach (var profile in profiles)
            {
                Profiles.Add(profile);
            }
        },
        "CareNest could not load local profiles.");
}
