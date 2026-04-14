using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards;

namespace Remilia.RemiliaCode.Cards.Uncommon;

public class RemiliaUncommon23() : RemiliaCard(2,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(13m, ValueProp.Move), new CardsVar(1)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, base.Owner);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(3m);
        base.DynamicVars.Cards.UpgradeValueBy(1m);
    }
}