using System.Text.RegularExpressions;
using LiveryInstaller.UI.Models.DTO;
using LiveryInstaller.UI.Models.INI;
using Microsoft.Extensions.Logging;

namespace LiveryInstaller.UI.Services.Factories;

public sealed class LoadedLiveryFactory(
    ILogger<LoadedLiveryFactory> logger) : ILoadedLiveryFactory
{
    public LoadedLivery Create(string packagePath, AircraftConfiguration aircraftConfiguration)
    {
        var flightSimSection = aircraftConfiguration.GetFirstFlightSimSection();
        var uiName = flightSimSection.GetValue("ui_variation");
        var airlineName = flightSimSection.GetValue("airline_name");
        
        var title = flightSimSection.GetValue("title");
        
        logger.LogInformation("Extracting aircraft and variant from title: {Title}", title);
        
        var aircraftMatch = RegularExpressions.AircraftRegex().Match(title);
        var variantMatch = RegularExpressions.AircraftVariantRegex().Match(title);
        
        var aircraft = aircraftMatch.Groups[1].Value;
        var variant = variantMatch.Groups[1].Value;
        
        logger.LogInformation("Extracted aircraft: {Aircraft} - variant: {Variant}", aircraft, variant);
        
        var manufacturer = flightSimSection.GetValue("ui_manufacturer");
        var aircraftType = flightSimSection.GetValue("ui_type");

        var livery = new LiveryDto(
            flightSimSection.GetValue("texture"),
            flightSimSection.GetValue("atc_id"),
            uiName,
            flightSimSection.GetValue("description"),
            airlineName ?? uiName[..(uiName.IndexOf('(') - 1)],
            title.Replace("|", "_"));
        
        return new LoadedLivery(packagePath, aircraft, variant, manufacturer, aircraftType, livery);
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
    
        
}