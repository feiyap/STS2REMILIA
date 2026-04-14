using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Powers;

namespace Remilia.RemiliaCode.Cards.Uncommon;

public class RemiliaUncommon13() : RemiliaCard(3,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("BloodCost", 20m), new DamageVar(50m,ValueProp.Move)];

    protected override bool IsPlayable => IsBloodPoolCount(base.DynamicVars["BloodCost"].IntValue);
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        VfxCmd.PlayOnCreatureCenter(base.Owner.Creature, "vfx/vfx_bloody_impact");
        await PowerCmd.Apply<BloodPool>(base.Owner.Creature, -base.DynamicVars["BloodCost"].IntValue, base.Owner.Creature, null);
        
        await DamageCmd.Attack(base.DynamicVars.CalculatedDamage.BaseValue).FromCard(this)
            .Targeting(play.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(20m);
    }
}