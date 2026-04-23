using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Powers;

namespace Remilia.RemiliaCode.Cards.Rare;

public class RemiliaRare3() : RemiliaCard(3,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    private bool isFatal = false;
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(10m),
        new ExtraDamageVar(10m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel _, Creature? target) => target?.GetPowerAmount<ClawPrints>() ?? 0)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ClawPrints>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
            .Execute(choiceContext);
        
        bool shouldTriggerFatal = play.Target.Powers.All((PowerModel p) => p.ShouldOwnerDeathTriggerFatal());

        isFatal = false;
        if (shouldTriggerFatal && attackCommand.Results.Any((DamageResult r) => r.WasTargetKilled))
        {
            isFatal = true;
        }
    }
    
    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == base.Owner && cardPlay.Card == this && base.Pile.Type != PileType.Hand && isFatal)
        {
            CardModel cardModel = cardPlay.Card.CreateClone();
            //await CardPileCmd.Add(this, PileType.Hand);
            cardModel.SetToFreeThisTurn();
            cardModel.AddKeyword(CardKeyword.Exhaust);
            cardModel.AddKeyword(CardKeyword.Ethereal);
            await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, addedByPlayer: true);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.CalculationBase.UpgradeValueBy(5m);
        base.DynamicVars.ExtraDamage.UpgradeValueBy(5m);
    }
}