using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Powers;

namespace Remilia.RemiliaCode.Cards.Uncommon;

public class RemiliaUncommon29() : RemiliaCard(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<RemiliaUncommon29Power>(2m)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<RemiliaUncommon29Power>(base.Owner.Creature, base.DynamicVars["RemiliaUncommon28Power"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["RemiliaUncommon29Power"].UpgradeValueBy(1m);
    }
}