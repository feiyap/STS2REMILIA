using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Remilia.RemiliaCode.Powers;

public class RemiliaUncommon28Power : RemiliaPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool ShouldScaleInMultiplayer => true;

    private int _reflectAmount;

    public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, Creature? _, out decimal modifiedAmount)
    {
        if (target != base.Owner)
        {
            modifiedAmount = amount;
            return false;
        }
        if (canonicalPower.GetTypeForAmount(amount) != PowerType.Debuff)
        {
            modifiedAmount = amount;
            return false;
        }
        if (!canonicalPower.IsVisible)
        {
            modifiedAmount = amount;
            return false;
        }
        if (canonicalPower.Applier == base.Owner)
        {
            modifiedAmount = amount;
            return false;
        }

        _reflectAmount = (int)amount;
        modifiedAmount = default(decimal);
        return true;
    }

    public override async Task AfterModifyingPowerAmountReceived(PowerModel power)
    {
        int reflectAmount = _reflectAmount;
        _reflectAmount = 0;
        await PowerCmd.Decrement(this);

        if (reflectAmount <= 0 || power.Applier == null)
            return;

        await PowerCmd.Apply(new ThrowingPlayerChoiceContext(), power, power.Applier, reflectAmount, base.Owner, null);
    }
}