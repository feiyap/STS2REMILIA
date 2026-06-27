using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using Remilia.RemiliaCode.Cards;

namespace Remilia.RemiliaCode.Cards.Rare;

public class RemiliaRare10() : RemiliaCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardModel handCard = (await CardSelectCmd.FromHand(
            prefs: new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1),
            context: choiceContext,
            player: base.Owner,
            filter: null,
            source: this)).FirstOrDefault();

        if (handCard == null)
            return;

        CardModel? deckCard = handCard.DeckVersion;
        if (deckCard != null)
        {
            // 永久变化牌组原版，并同步更新手牌中的战斗克隆
            CardModel deckReplacement = CardFactory.CreateRandomCardForTransform(
                deckCard, isInCombat: false, base.Owner.RunState.Rng.Niche);
            await CardCmd.Transform(deckCard, deckReplacement, CardPreviewStyle.EventLayout);

            CardModel handReplacement = base.CombatState.CloneCard(deckReplacement);
            handReplacement.DeckVersion = deckReplacement;
            await CardCmd.Transform(handCard, handReplacement, CardPreviewStyle.HorizontalLayout);
        }
        else
        {
            await CardCmd.TransformToRandom(handCard, base.Owner.RunState.Rng.Niche, CardPreviewStyle.HorizontalLayout);
        }
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
