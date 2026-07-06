using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Powers;
using Remilia.RemiliaCode.Resources;

namespace Remilia.RemiliaCode.Cards.Uncommon;

public class RemiliaUncommon1() : RemiliaCard(0,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4m, ValueProp.Move), new PowerVar<ClawPrints>(1m),new DynamicVar("BloodCost", 3m)];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<ClawPrints>()];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, play)
            .Targeting(play.Target)
            .WithHitVfxNode((Creature t) => NScratchVfx.Create(t, goingRight: true))
            .Execute(choiceContext);
        await PowerCmd.Apply<ClawPrints>(choiceContext, play.Target, base.DynamicVars["ClawPrints"].BaseValue, base.Owner.Creature, this);

        if (play.ResultPile == PileType.Hand)
            await RemiliaBloodPool.Lose(Owner, DynamicVars["BloodCost"].IntValue, this);

        await Cmd.Wait(0.25f);
    }
    
    protected override (PileType, CardPilePosition) GetResultPileTypeAndPositionForCardPlay()
    {
        var (pileType, position) = base.GetResultPileTypeAndPositionForCardPlay();
        if (pileType != PileType.Discard)
            return (pileType, position);

        return IsBloodPoolCount(DynamicVars["BloodCost"].IntValue)
            ? (PileType.Hand, CardPilePosition.Bottom)
            : (pileType, position);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}