using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Cards.Curse;

namespace Remilia.RemiliaCode.Cards.Uncommon;

public class RemiliaUncommon8() : RemiliaCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10m, ValueProp.Move)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<BloodCurse>()];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        List<PowerModel> originalDebuffs = (from p in play.Target.Powers
            where p.TypeForCurrentAmount == PowerType.Debuff
            select (PowerModel)p.ClonePreservingMutability()).ToList();
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        foreach (Creature enemy in base.CombatState.HittableEnemies)
        {
            if (enemy == play.Target)
            {
                continue;
            }
            foreach (PowerModel item in originalDebuffs)
            {
                PowerModel powerById = enemy.GetPowerById(item.Id);
                if (powerById != null && !powerById.IsInstanced)
                {
                    DoHackyThingsForSpecificPowers(powerById);
                    await PowerCmd.ModifyAmount(powerById, item.Amount, base.Owner.Creature, this);
                }
                else
                {
                    PowerModel power = (PowerModel)item.ClonePreservingMutability();
                    DoHackyThingsForSpecificPowers(power);
                    await PowerCmd.Apply(power, enemy, item.Amount, base.Owner.Creature, this);
                }
            }
        }
        
        await BloodCurse.CreateInHand(base.Owner, base.CombatState);
    }
    
    private static void DoHackyThingsForSpecificPowers(PowerModel power)
    {
        if (power is ITemporaryPower temporaryPower)
        {
            temporaryPower.IgnoreNextInstance();
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(4m);
    }
}