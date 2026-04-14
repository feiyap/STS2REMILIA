using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards;

namespace Remilia.RemiliaCode.Cards.Uncommon;

public class RemiliaUncommon5() : RemiliaCard(2,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7m, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int count = 2;
        if (base.CombatState.Enemies.Count > 1)
        {
            count = 3;
        }
        AttackCommand attackCommand = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(count).FromCard(this)
            .Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
    }
}