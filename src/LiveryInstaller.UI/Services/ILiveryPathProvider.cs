namespace LiveryInstaller.UI.Services;

public interface ILiveryPathProvider
{
    string GetIconPath(string aircraftName, string variantName, string liveryName);
    
    string GetLiveryPath(string aircraftName, string variantName, string liveryName);
}