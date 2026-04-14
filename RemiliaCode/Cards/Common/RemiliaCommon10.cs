using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Powers;

namespace Remilia.RemiliaCode.Cards.Common;

public class RemiliaCommon10() : RemiliaCard(0,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("BloodCost", 6m), new EnergyVar(2)];
    
    protected override bool IsPlayable => IsBloodPoolCount(base.DynamicVars["BloodCost"].IntValue);

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        VfxCmd.PlayOnCreatureCenter(base.Owner.Creature, "vfx/vfx_bloody_impact");
        await PowerCmd.Apply<BloodPool>(base.Owner.Creature, -base.DynamicVars["BloodCost"].IntValue, base.Owner.Creature, null);
        await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Energy.UpgradeValueBy(1m);
    }
}