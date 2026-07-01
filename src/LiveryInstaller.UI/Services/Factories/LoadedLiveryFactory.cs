using System.Text.RegularExpressions;
using LiveryInstaller.UI.Models.DTO;
using LiveryInstaller.UI.Models.INI;
using LiveryInstaller.UI.Services.Configuration;
using Microsoft.Extensions.Logging;

namespace LiveryInstaller.UI.Services.Factories;

public sealed class LoadedLiveryFactory(
    IAirlinesConfigurationService airlinesConfigurationService,
    ILogger<LoadedLiveryFactory> logger) : ILoadedLiveryFactory
{
    public LoadedLivery Create(string packagePath, AircraftConfiguration aircraftConfiguration)
    {
        var flightSimSection = aircraftConfiguration.GetFirstFlightSimSection();
        var uiName = flightSimSection.GetValue("ui_variation");
        var title = flightSimSection.GetValue("title");

        logger.LogInformation("Extracting aircraft and variant from title: {Title}", title);

        var (aircraft, variant) = ExtractAircraftAndVariant(title);
        var airlineName = GetAirlineName(flightSimSection);

        logger.LogInformation("Extracted aircraft: {Aircraft} - variant: {Variant}", aircraft, variant);

        var manufacturer = flightSimSection.GetValue("ui_manufacturer");
        var aircraftType = flightSimSection.GetValue("ui_type");

        var livery = new LiveryDto(
            flightSimSection.GetValue("texture"),
            flightSimSection.GetValue("atc_id"),
            uiName,
            flightSimSection.GetValue("description"),
            airlineName,
            title.Replace("|", "_"));

        return new LoadedLivery(packagePath, aircraft, variant, manufacturer, aircraftType, livery);
    }

    private static (string aircraft, string variant) ExtractAircraftAndVariant(string title)
    {
        var aircraftMatch = RegularExpressions.AircraftRegex().Match(title);
        var variantMatch = RegularExpressions.AircraftVariantRegex().Match(title);

        return (aircraftMatch.Groups[1].Value, variantMatch.Groups[1].Value);
    }

    /// <summary>
    /// Again, PMDG's lack of standardization is appalling. There isn't one single source to pull the airliner information from so we go through an election process.
    /// </summary>
    /// <param name="flightSimSection"></param>
    /// <returns></returns>
    private string GetAirlineName(FlightSimSectionNode flightSimSection)
    {
        var uiVariation = flightSimSection.GetValue("ui_variation");

        // Match against a list of known airlines
        var candidateAirline = airlinesConfigurationService.GetAirlineName(uiVariation);
        if (!string.IsNullOrEmpty(candidateAirline))
            return candidateAirline;

        // Return airline_name if it exists. Some liveries use this
        var airlineName = flightSimSection.GetValue("airline_name");
        if (!string.IsNullOrEmpty(airlineName))
            return airlineName;

        // Try to parse the operator name from the ui_variation
        // E.g, Korean Air (D-ABCD)
        if (uiVariation.Contains('('))
            return uiVariation[..(uiVariation.IndexOf('(') - 1)];

        // Match with Regex for liveries that don't wrap the registration in parentheses
        // E.g, Korean Air D-ABCD
        var matches = RegularExpressions.AirlineUiVariationRegex().Matches(uiVariation);

        return matches.Count > 0
            ? matches[0].Groups[1].Value
            : flightSimSection.GetValue("atc_airline"); // Or fallback to the ATC airline, which isn't ideal.
    }
}

public static partial class RegularExpressions
{
    /// <summary>
    /// Very little consistency in the naming of aircraft variants within the PTP configs. Regular Expression to extract the variant name.
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex("^(PMDG \\d+-?[A-Za-z0-9]+(?: [A-Z]{2,})?)", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    public static partial Regex AircraftVariantRegex();

    [GeneratedRegex("^PMDG (\\d{3})", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    public static partial Regex AircraftRegex();

    [GeneratedRegex("^([A-Za-z ]+)[A-Za-z]-", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    public static partial Regex AirlineUiVariationRegex();
}