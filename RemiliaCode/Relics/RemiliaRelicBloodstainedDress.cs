using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Powers;
using Remilia.RemiliaCode.Relics;

namespace Remilia.RemiliaCode.Relics;

public class RemiliaRelicBloodstainedDress() : RemiliaRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(1m, ValueProp.Unpowered)];

    public override Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power is BloodPool && amount < 0)
        {
            Flash();
            decimal count = base.DynamicVars.Block.BaseValue * -amount;
            CreatureCmd.GainBlock(base.Owner.Creature, count, ValueProp.Unpowered, null);
        }
        return base.AfterPowerAmountChanged(power, amount, applier, cardSource);
    }
}