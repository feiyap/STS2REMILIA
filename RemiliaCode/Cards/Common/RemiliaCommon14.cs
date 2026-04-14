using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Powers;

namespace Remilia.RemiliaCode.Cards.Common;

public class RemiliaCommon14() : RemiliaCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6m, ValueProp.Move),
        new PowerVar<ClawPrints>(2m)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ClawPrints>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(base.CombatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        await PowerCmd.Apply<ClawPrints>(base.CombatState.HittableEnemies, base.DynamicVars["ClawPrints"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }
}