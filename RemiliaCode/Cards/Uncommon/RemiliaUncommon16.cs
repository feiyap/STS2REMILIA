using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Powers;

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
        int count = Math.Min(0, base.Owner.Creature.GetPower<BloodPool>()?.Amount ?? 0);
        await PowerCmd.Apply<BloodPool>(base.Owner.Creature, -count, base.Owner.Creature, null);
        await CreatureCmd.Heal(base.Owner.Creature, count);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}