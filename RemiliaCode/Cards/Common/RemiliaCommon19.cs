using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Powers;

namespace Remilia.RemiliaCode.Cards.Common;

public class RemiliaCommon19() : RemiliaCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new DynamicVar("RemiliaCommon19Power", 2)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, base.Owner);
        await PowerCmd.Apply<RemiliaCommon19Power>(base.Owner.Creature, base.DynamicVars["RemiliaCommon19Power"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["RemiliaCommon19Power"].UpgradeValueBy(1m);
    }
}