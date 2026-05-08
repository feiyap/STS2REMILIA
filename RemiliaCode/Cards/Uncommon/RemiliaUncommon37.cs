using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Powers;

namespace Remilia.RemiliaCode.Cards.Uncommon;

public class RemiliaUncommon37() : RemiliaCard(0,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5m, ValueProp.Move), new PowerVar<BloodPlague>(1m), new PowerVar<ClawPrints>(1m)];
    
    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        int num = ResolveEnergyXValue();
        if (base.IsUpgraded)
        {
            num++;
        }
        
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).WithHitCount(num)
            .Targeting(play.Target)
            .WithHitVfxNode((Creature t) => NScratchVfx.Create(t, goingRight: true))
            .Execute(choiceContext);

        await PowerCmd.Apply<BloodPlague>(play.Target, base.DynamicVars["BloodPlague"].BaseValue * num, base.Owner.Creature, this);
        await PowerCmd.Apply<ClawPrints>(play.Target, base.DynamicVars["ClawPrints"].BaseValue * num, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}