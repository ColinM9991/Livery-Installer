namespace LiveryInstaller.UI.Models.Configuration;

public class SimulatorConfiguration
{
    public IDictionary<SimulatorType, string> InstallationPaths { get; set; } = new Dictionary<SimulatorType, string>();
}