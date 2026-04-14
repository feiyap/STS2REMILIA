using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Powers;

namespace Remilia.RemiliaCode.Cards.Uncommon;

public class RemiliaUncommon1() : RemiliaCard(0,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6m, ValueProp.Move), new PowerVar<ClawPrints>(1m),new DynamicVar("BloodCost", 3m)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this)
            .Targeting(play.Target)
            .WithHitVfxNode((Creature t) => NScratchVfx.Create(t, goingRight: true))
            .Execute(choiceContext);
        await PowerCmd.Apply<ClawPrints>(play.Target, base.DynamicVars["ClawPrints"].BaseValue, base.Owner.Creature, null);
        await Cmd.Wait(0.25f);
    }
    
    protected override PileType GetResultPileType()
    {
        PileType resultPileType = base.GetResultPileType();
        if (resultPileType != PileType.Discard)
        {
            return resultPileType;
        }

        if (IsBloodPoolCount(3))
        {
            PowerCmd.Apply<BloodPool>(base.Owner.Creature, -3, base.Owner.Creature, null);
            return PileType.Hand;
        }
        return resultPileType;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}