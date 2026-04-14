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
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Remilia.RemiliaCode.Powers;

public class RemiliaUncommon29Power : RemiliaPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar("SelfDamage", 0m, ValueProp.Unblockable | ValueProp.Unpowered)];
    
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == base.Owner.Player)
        {
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NFireSmokePuffVfx.Create(base.Owner));
            await Cmd.CustomScaledWait(0.2f, 0.4f);
            DamageVar damageVar = (DamageVar)base.DynamicVars["SelfDamage"];
            await CreatureCmd.Damage(choiceContext, base.Owner, damageVar.BaseValue, damageVar.Props, base.Owner, null);
        }
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != base.Owner || result.UnblockedDamage <= 0 || base.Owner.CombatState.CurrentSide != base.Owner.Side)
        {
            return;
        }
        foreach (Creature hittableEnemy in base.CombatState.HittableEnemies)
        {
            NFireBurstVfx child = NFireBurstVfx.Create(hittableEnemy, 0.75f);
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
        }
        await CreatureCmd.Damage(choiceContext, base.CombatState.HittableEnemies, base.Amount, ValueProp.Unpowered, base.Owner, null);
        
        int count = Math.Min(base.Amount, base.Owner.GetPower<BloodPool>()?.Amount ?? 0);
        await PowerCmd.Apply<BloodPool>(base.Owner, -count, base.Owner, null);
        await CreatureCmd.Heal(base.Owner, count);
    }
    
    public void IncrementSelfDamage()
    {
        AssertMutable();
        base.DynamicVars["SelfDamage"].BaseValue++;
    }
}