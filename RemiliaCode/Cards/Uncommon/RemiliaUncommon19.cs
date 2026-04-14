using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Powers;

namespace Remilia.RemiliaCode.Cards.Uncommon;

public class RemiliaUncommon19() : RemiliaCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("BloodPool", 9m), new CardsVar(1)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<BloodPool>(base.Owner.Creature, base.DynamicVars["BloodPool"].BaseValue, base.Owner.Creature, null);
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, base.Owner);
        
        if (base.IsUpgraded)
        {
            CardModel cardModel = (await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1), context: choiceContext, player: base.Owner, filter: null, source: this)).FirstOrDefault();
            if (cardModel != null)
            {
                await CardCmd.Exhaust(choiceContext, cardModel);
            }
            return;
        }
        CardPile pile = PileType.Hand.GetPile(base.Owner);
        CardModel cardModel2 = base.Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
        if (cardModel2 != null)
        {
            await CardCmd.Exhaust(choiceContext, cardModel2);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["BloodPool"].UpgradeValueBy(3m);
    }
}