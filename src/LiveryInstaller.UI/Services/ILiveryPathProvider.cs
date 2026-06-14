namespace LiveryInstaller.UI.Services;

public interface ILiveryPathProvider
{
    string GetIconPath(string aircraftName, string variantName, string liveryName);
    
    string GetLiveryPath(string aircraftName, string variantName, string liveryName);

    bool IsAircraftPathValid(string aircraftName);

    bool IsVariantPathValid(string aircraftName, string variantName);
    
    bool IsLiveryPathValid(string aircraftName, string variantName, string liveryName);
}