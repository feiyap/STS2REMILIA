using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Resources;

namespace Remilia.RemiliaCode.Cards.Common;

public class RemiliaCommon7() : RemiliaCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("BloodPool", 4m)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await RemiliaBloodPool.Gain(Owner, base.DynamicVars["BloodPool"].IntValue, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["BloodPool"].UpgradeValueBy(2m);
    }
}