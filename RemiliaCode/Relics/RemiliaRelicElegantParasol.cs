using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Remilia.RemiliaCode.Relics;

namespace Remilia.RemiliaCode.Relics;

public class RemiliaRelicElegantParasol() : RemiliaRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;

    public override decimal ModifyPowerAmountGiven(PowerModel power, Creature giver, decimal amount, Creature? target, CardModel? cardSource)
    {
        if (power.GetTypeForAmount(amount) != PowerType.Debuff)
        {
            return amount;
        }
        if (giver != base.Owner.Creature)
        {
            return amount;
        }
        if (Random.Shared.NextDouble() >= 0.33)
        {
            return amount;
        }
        return amount + 1;
    }

    public override Task AfterModifyingPowerAmountGiven(PowerModel power)
    {
        Flash();
        return Task.CompletedTask;
    }
}