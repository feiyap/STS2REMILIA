using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Remilia.RemiliaCode.Powers;

public class RemiliaUncommon32Power : RemiliaPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(1m)];
    
    public override bool ShouldDieLate(Creature creature)
    {
        if (creature != base.Owner)
        {
            return true;
        }
        
        return false;
    }

    public override async Task AfterPreventingDeath(Creature creature)
    {
        Flash();
        decimal amount = Math.Max(1m, (decimal)creature.MaxHp * (base.DynamicVars.Heal.BaseValue / 100m));
        await CreatureCmd.Heal(creature, amount);
        await PowerCmd.Decrement(this);
    }
}