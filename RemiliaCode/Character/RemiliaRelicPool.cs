using BaseLib.Abstracts;
using Remilia.RemiliaCode.Extensions;
using Godot;

namespace Remilia.RemiliaCode.Character;

public class RemiliaRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => Remilia.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}