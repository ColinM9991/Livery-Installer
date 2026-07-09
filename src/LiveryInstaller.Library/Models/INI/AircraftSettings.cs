namespace LiveryInstaller.Library.Models.INI;

public record AircraftSettings
{
    public string Aircraft { get; set; }
    
    public string Variant { get; set; }
    
    public string Name { get; set; }
}