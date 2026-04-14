using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards;

namespace Remilia.RemiliaCode.Cards.Rare;

public class RemiliaRare2() : RemiliaCard(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("BloodCost", 6m), new DamageVar(13m, ValueProp.Move), new PowerVar<WeakPower>(2m)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(4m);
    }
}