using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using Remilia.RemiliaCode.Relics;

namespace Remilia.RemiliaCode.Relics;

public class RemiliaRelicAncientDestiny() : RemiliaRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;

    public override bool ShouldAllowFreeTravel()
    {
        return true;
    }
}