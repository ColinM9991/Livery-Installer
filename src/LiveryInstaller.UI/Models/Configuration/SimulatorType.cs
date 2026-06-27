using System.ComponentModel.DataAnnotations;

namespace LiveryInstaller.UI.Models.Configuration;

public enum SimulatorType
{
    [Display(Name = "Prepar3D v4")]
    Prepar3Dv4,
    [Display(Name = "Prepar3D v5")]
    Prepar3Dv5
}