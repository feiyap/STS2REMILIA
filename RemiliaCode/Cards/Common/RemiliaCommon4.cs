using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Powers;

namespace Remilia.RemiliaCode.Cards.Common;

public class RemiliaCommon4() : RemiliaCard(0,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("BloodCost", 11m),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => Math.Min(card.Owner.Creature.GetPowerAmount<BloodPool>(), card.DynamicVars["BloodCost"].BaseValue))
    ];
    
    public override bool GainsBlock => true;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        decimal blockValue = Math.Min(base.Owner.Creature.GetPowerAmount<BloodPool>(), base.DynamicVars["BloodCost"].BaseValue);
        //decimal blockValue = base.DynamicVars.CalculatedBlock.Calculate(play.Target);
        await CreatureCmd.GainBlock(base.Owner.Creature, blockValue, base.DynamicVars.CalculatedBlock.Props, play);
        await PowerCmd.Apply<BloodPool>(base.Owner.Creature, -blockValue, base.Owner.Creature, null);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["BloodCost"].UpgradeValueBy(3m);
    }
}