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
    
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromCard<BloodCurse>()];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Dictionary<PowerModel, int> debuffAmounts = (from p in play.Target!.Powers
            where p.TypeForCurrentAmount == PowerType.Debuff
            select ((PowerModel)p.ClonePreservingMutability(), Amount: p.Amount)).ToDictionary();
        foreach (KeyValuePair<PowerModel, int> item in debuffAmounts.ToList())
        {
            if (item.Key is ITemporaryPower temporaryPower)
            {
                KeyValuePair<PowerModel, int> internalPower = debuffAmounts.FirstOrDefault(
                    p => p.Key.Id == temporaryPower.InternallyAppliedPower.Id);
                if (internalPower.Key != null)
                    debuffAmounts[internalPower.Key] += item.Value;
            }
        }

        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        foreach (Creature enemy in base.CombatState.HittableEnemies)
        {
            if (enemy == play.Target)
                continue;

            foreach (KeyValuePair<PowerModel, int> item in debuffAmounts)
            {
                if (item.Value == 0)
                    continue;

                PowerModel powerModel = PowerCmd.FindExistingInstanceForStacking(item.Key, enemy, item.Key.Applier);
                if (powerModel != null)
                    await PowerCmd.ModifyAmount(choiceContext, powerModel, item.Value, item.Key.Applier, this);
                else
                {
                    PowerModel power = (PowerModel)item.Key.ClonePreservingMutability();
                    await PowerCmd.Apply(choiceContext, power, enemy, item.Value, item.Key.Applier, this);
                }
            }
        }
        
        await BloodCurse.CreateInHand(base.Owner, base.CombatState);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(4m);
    }
}
