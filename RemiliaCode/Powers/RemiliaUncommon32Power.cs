using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using Remilia.RemiliaCode.Resources;

namespace Remilia.RemiliaCode.Powers;

public class RemiliaUncommon32Power : RemiliaPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool ShouldDieLate(Creature creature)
    {
        if (creature != base.Owner)
            return true;

        if (base.Amount <= 0)
            return true;

        return false;
    }

    public override async Task AfterPreventingDeath(Creature creature)
    {
        Flash();
        int count = RemiliaBloodPool.Get(base.Owner.Player);
        await RemiliaBloodPool.Lose(base.Owner.Player, count, this);
        await CreatureCmd.Heal(creature, count);
        await PowerCmd.Decrement(this);
    }
}
