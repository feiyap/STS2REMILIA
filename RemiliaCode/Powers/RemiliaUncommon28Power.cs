using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Remilia.RemiliaCode.Powers;

public class RemiliaUncommon28Power : RemiliaPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override bool ShouldScaleInMultiplayer => true;

    private int powerCount = 0;

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
            Console.WriteLine("LL1");
            modifiedAmount = amount;
            return false;
        }
        Console.WriteLine("LL2");
        Console.WriteLine(amount);
        powerCount = (int)amount;
        modifiedAmount = default(decimal);
        return true;
    }

    public override async Task AfterModifyingPowerAmountReceived(PowerModel power)
    {
        await PowerCmd.Decrement(this);
        
        Console.WriteLine(power);
        Console.WriteLine(power.Applier);
        Console.WriteLine(power.Amount);
        Console.WriteLine(powerCount);
        await PowerCmd.Apply(power, power.Applier, powerCount, base.Owner, null);
    }
}