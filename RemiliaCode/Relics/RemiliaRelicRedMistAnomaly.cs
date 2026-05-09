using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using Remilia.RemiliaCode.Relics;

namespace Remilia.RemiliaCode.Relics;

[Pool(typeof(RelicPool))]
public class RemiliaRelicRedMistAnomaly() : RemiliaRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    
}