using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Powers;

namespace Remilia.RemiliaCode.Cards.Uncommon;

public class RemiliaUncommon7() : RemiliaCard(2,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("BloodCost", 12m),
        new CalculationBaseVar(0m),
        new ExtraDamageVar(1m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => Math.Min(card.Owner.Creature.GetPowerAmount<BloodPool>(), card.DynamicVars["BloodCost"].BaseValue))
    ];
    
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        decimal damageValue = Math.Min(base.Owner.Creature.GetPowerAmount<BloodPool>(), base.DynamicVars["BloodCost"].BaseValue);
        
        AttackCommand attackCommand = await DamageCmd.Attack(damageValue).FromCard(this)
            .Targeting(play.Target)
            .WithHitVfxNode((Creature t) => NScratchVfx.Create(t, goingRight: true))
            .Execute(choiceContext);
        
        await CreatureCmd.GainBlock(base.Owner.Creature, damageValue, ValueProp.Move, play);
        await PowerCmd.Apply<BloodPool>(base.Owner.Creature, -damageValue, base.Owner.Creature, null);
        await CreatureCmd.Heal(base.Owner.Creature, damageValue);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["BloodCost"].UpgradeValueBy(4m);
    }
}