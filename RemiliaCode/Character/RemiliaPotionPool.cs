using Remilia.RemiliaCode.Extensions;
using Godot;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Remilia.RemiliaCode.Character;

[RegisterSharedPotionPool]
public class RemiliaPotionPool : TypeListPotionPoolModel
{
    public override string EnergyColorName => "remilia";

    public override Color LabOutlineColor => Remilia.Color;

    public override string? BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string? TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}
