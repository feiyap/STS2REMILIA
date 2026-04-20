using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Remilia.RemiliaCode.Cards;

namespace Remilia.RemiliaCode.Cards.Rare;

public class RemiliaRare13() : RemiliaCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Equilibrium", 1m)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int num = 10 - base.Owner.PlayerCombatState.Hand.Cards.Count;
        await CardPileCmd.Draw(choiceContext, num, base.Owner);

        if (base.IsUpgraded)
        {
            await PowerCmd.Apply<RetainHandPower>(base.Owner.Creature, base.DynamicVars["Equilibrium"].BaseValue, base.Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {

    }
}