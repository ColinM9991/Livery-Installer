using System.ComponentModel.DataAnnotations;
using System.Reflection;
using LiveryInstaller.Library.Models.Configuration;

namespace LiveryInstaller.Library.Models.DTO;

public record InstalledSimulator(SimulatorType Type)
{
    public string Name { get; set; } = Type.GetType().GetMember(Type.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>()?.Name ?? Type.ToString();

    public static explicit operator InstalledSimulator(SimulatorType type) => new(type);
    
    public static implicit operator SimulatorType(InstalledSimulator simulator) => simulator.Type;
}