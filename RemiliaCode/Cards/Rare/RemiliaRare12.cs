using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Remilia.RemiliaCode.Cards;

namespace Remilia.RemiliaCode.Cards.Rare;

public class RemiliaRare12() : RemiliaCard(0,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("BloodCost", 15), new CardsVar(5)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Ethereal];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        foreach (CardModel item in PileType.Hand.GetPile(base.Owner).Cards.ToList())
        {
            await CardPileCmd.Add(item, PileType.Draw);
        }
        await CardPileCmd.Shuffle(choiceContext, base.Owner);
        
        CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 5);
        List<CardModel> cardsIn = PileType.Draw.GetPile(base.Owner).Cards.ToList();
        foreach (CardModel cm in await CardSelectCmd.FromSimpleGrid(choiceContext, cardsIn, base.Owner, prefs))
        {
            await CardPileCmd.Add(cm, PileType.Hand);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["BloodCost"].UpgradeValueBy(-5);
        base.AddKeyword(CardKeyword.Innate);
    }
}