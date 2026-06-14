using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveryInstaller.UI.Models;
using LiveryInstaller.UI.Models.DTO;
using LiveryInstaller.UI.Services;

namespace LiveryInstaller.UI.ViewModels;

public partial class LiveryPageViewModel : ObservableObject, IPage
{
    private const string AllOperatorsOption = "All";
    private readonly ILiveryViewModelFactory _liveryViewModelFactory;
    private IReadOnlyList<LiveryViewModel> _selectedVariantLiveries = [];

    public LiveryPageViewModel(
        ILiveryConfigurationService liveryConfigurationService,
        ILiveryViewModelFactory liveryViewModelFactory)
    {
        _liveryViewModelFactory = liveryViewModelFactory;
        AircraftOptions = liveryConfigurationService.GetAvailableAircraft();
        SelectedAircraft = AircraftOptions.FirstOrDefault();
    }

    public IReadOnlyCollection<AircraftDto> AircraftOptions { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedAircraftVariants))]
    public partial AircraftDto SelectedAircraft { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedVariantLiveries))]
    [NotifyPropertyChangedFor(nameof(AvailableOperators))]
    public partial VariantDto SelectedVariant { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedVariantLiveries))]
    public partial string SelectedOperator { get; set; }

    public IEnumerable<VariantDto> SelectedAircraftVariants => SelectedAircraft?.Variants ?? [];

    public IEnumerable<LiveryViewModel> SelectedVariantLiveries => _selectedVariantLiveries.Where(MatchesLivery);

    public Visibility LiveriesPanelVisibility =>
        _selectedVariantLiveries.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

    public Visibility WarningPanelVisibility =>
        LiveriesPanelVisibility is Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;

    public IReadOnlyList<string> AvailableOperators
    {
        get
        {
            if (SelectedVariant is null)
                return [];

            return [AllOperatorsOption, .. SelectedVariant.Liveries.Select(x => x.Airline).Distinct()];
        }
    }

    private bool MatchesLivery(LiveryViewModel livery) =>
        string.IsNullOrWhiteSpace(SelectedOperator) || SelectedOperator == AllOperatorsOption ||
        livery.Airline == SelectedOperator;

    partial void OnSelectedAircraftChanged(AircraftDto value)
    {
        SelectedVariant = value.Variants.FirstOrDefault();
    }

    partial void OnSelectedVariantChanged(VariantDto value)
    {
        _selectedVariantLiveries = value?.Liveries
            .Select(x => _liveryViewModelFactory.Create(SelectedAircraft, value, x)).ToList() ?? [];

        SelectedOperator = AllOperatorsOption;
        OnPropertyChanged(nameof(SelectedVariantLiveries));
        OnPropertyChanged(nameof(LiveriesPanelVisibility));
        OnPropertyChanged(nameof(WarningPanelVisibility));
    }
}