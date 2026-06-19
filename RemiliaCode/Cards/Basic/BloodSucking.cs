using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards.Ancient;
using Remilia.RemiliaCode.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Remilia.RemiliaCode.Cards.Basic;

[RegisterArchaicToothTranscendence(typeof(RemiliaAncient2))]
public class BloodSucking() : RemiliaCard(1,
    CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this)
            .Targeting(play.Target)
            .WithHitVfxNode((Creature t) => NScratchVfx.Create(t, goingRight: true))
            .Execute(choiceContext);
        
        int value1 = attackCommand.Results.SelectMany((List<DamageResult> r) => r).Sum(r => r.TotalDamage + r.OverkillDamage);
        int value2 = base.Owner.Creature.GetPower<BloodPool>()?.Amount ?? 0;
        int value3 = Math.Max(0, base.Owner.Creature.MaxHp - base.Owner.Creature.CurrentHp);
        int count = new[] { value1, value2, value3 }.Min();
        
        await PowerCmd.Apply<BloodPool>(choiceContext, base.Owner.Creature, -count, base.Owner.Creature, null);
        await CreatureCmd.Heal(base.Owner.Creature, count);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}