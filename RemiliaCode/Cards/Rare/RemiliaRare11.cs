using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Powers;

namespace Remilia.RemiliaCode.Cards.Rare;

public class RemiliaRare11() : RemiliaCard(2,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("BloodCost", 20m)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override bool IsPlayable => IsBloodPoolCount(base.DynamicVars["BloodCost"].IntValue);

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<BloodPool>(base.Owner.Creature, -base.DynamicVars["BloodCost"].IntValue, base.Owner.Creature, null);
        await CreatureCmd.Stun(play.Target);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["BloodCost"].UpgradeValueBy(-5m);
    }
}