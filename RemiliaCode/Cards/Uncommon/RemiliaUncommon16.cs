using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Resources;

namespace Remilia.RemiliaCode.Cards.Uncommon;

public class RemiliaUncommon16() : RemiliaCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int value2 = RemiliaBloodPool.Get(Owner);
        int value3 = Math.Max(0, base.Owner.Creature.MaxHp - base.Owner.Creature.CurrentHp);
        int count = new[] { value2, value3 }.Min();
        await RemiliaBloodPool.Lose(Owner, count, this);
        await CreatureCmd.Heal(base.Owner.Creature, count);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}