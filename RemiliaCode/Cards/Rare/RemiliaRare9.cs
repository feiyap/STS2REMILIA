using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Powers;

namespace Remilia.RemiliaCode.Cards.Rare;

public class RemiliaRare9() : RemiliaCard(0,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyAlly)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("BloodCost", 10), new CardsVar(4)];
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override bool IsPlayable => IsBloodPoolCount(base.DynamicVars["BloodCost"].IntValue);

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<BloodPool>(base.Owner.Creature, -base.DynamicVars["BloodCost"].IntValue, base.Owner.Creature, null);
        
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        foreach (CardModel item in PileType.Hand.GetPile(play.Target.Player).Cards.ToList())
        {
            await CardPileCmd.Add(item, PileType.Draw);
        }
        await CardPileCmd.Shuffle(choiceContext, play.Target.Player);
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, play.Target.Player);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["BloodCost"].UpgradeValueBy(-5);
    }
}