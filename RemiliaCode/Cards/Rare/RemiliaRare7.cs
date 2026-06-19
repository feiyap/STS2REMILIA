using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards;

namespace Remilia.RemiliaCode.Cards.Rare;

public class RemiliaRare7() : RemiliaCard(0,
    CardType.Attack, CardRarity.Rare,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9m, ValueProp.Move), new DynamicVar("BloodCost", 5m)];

    protected override bool AutoBindBloodCost => true;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this)
            .TargetingAllOpponents(base.CombatState)
            .WithHitVfxNode((Creature t) => NScratchVfx.Create(t, goingRight: true))
            .Execute(choiceContext);
        
        await Cmd.Wait(0.25f);
    }
    
    protected override PileType GetResultPileTypeForCardPlay()
    {
        PileType resultPileType = base.GetResultPileTypeForCardPlay();
        if (resultPileType != PileType.Discard)
        {
            return resultPileType;
        }
        return PileType.Hand;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}