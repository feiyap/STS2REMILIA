using Remilia.RemiliaCode.Extensions;
using Godot;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Remilia.RemiliaCode.Character;

[RegisterSharedCardPool]
public class RemiliaCardPool : TypeListCardPoolModel
{
    public override string Title => Remilia.CharacterId;

    public override string EnergyColorName => "remilia";

    public override string? BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string? TextEnergyIconPath => "charui/text_energy.png".ImagePath();

    public override Color DeckEntryCardColor => new("FF3030");

    public override bool IsColorless => false;
}
