using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveryInstaller.UI.Models;
using LiveryInstaller.UI.Models.DTO;
using LiveryInstaller.UI.Services;
using LiveryInstaller.UI.Services.Factories;

namespace LiveryInstaller.UI.ViewModels;

public partial class LiveryPageViewModel : ObservableObject, IPage
{
    private const string AllOperatorsOption = "All";
    private IReadOnlyList<LiveryViewModel> _selectedVariantLiveries = [];
    private readonly ILiveryConfigurationFactory _liveryConfigurationFactory;
    private readonly ILiveryViewModelFactory _liveryViewModelFactory;

    /// <inheritdoc/>
    public LiveryPageViewModel(ILiveryConfigurationFactory liveryConfigurationFactory,
        ILiveryViewModelFactory liveryViewModelFactory,
        ISimulatorService simulatorService)
    {
        _liveryConfigurationFactory = liveryConfigurationFactory;
        _liveryViewModelFactory = liveryViewModelFactory;
        AvailableSimulators = simulatorService.GetInstalledSimulators();
        
        SelectedSimulator = AvailableSimulators.FirstOrDefault();
    }

    public IReadOnlyList<InstalledSimulator> AvailableSimulators { get; }

    [ObservableProperty]
    public partial IReadOnlyCollection<AircraftDto> AircraftOptions { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedAircraft))]
    public partial InstalledSimulator SelectedSimulator { get; set; }

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

    partial void OnSelectedSimulatorChanged(InstalledSimulator value)
    {
        _ = InitializeAsync();
        return;
        
        async Task InitializeAsync()
        {
            AircraftOptions = await Task.Run(() => _liveryConfigurationFactory.GetAvailableAircraftAsync(value));
            SelectedAircraft = AircraftOptions.FirstOrDefault();
            OnPropertyChanged(nameof(AircraftOptions));
        }
    }

    partial void OnSelectedAircraftChanged(AircraftDto value)
    {
        SelectedVariant = value?.Variants.FirstOrDefault();
    }

    partial void OnSelectedVariantChanged(VariantDto value)
    {
        _selectedVariantLiveries = value?.Liveries
            .Select(x => _liveryViewModelFactory.Create(SelectedSimulator, SelectedAircraft, value, x)).ToList() ?? [];

        SelectedOperator = AllOperatorsOption;
        OnPropertyChanged(nameof(SelectedVariantLiveries));
        OnPropertyChanged(nameof(LiveriesPanelVisibility));
        OnPropertyChanged(nameof(WarningPanelVisibility));
    }
}