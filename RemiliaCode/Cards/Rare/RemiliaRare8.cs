using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Powers;
using Remilia.RemiliaCode.Resources;

namespace Remilia.RemiliaCode.Cards.Rare;

public class RemiliaRare8() : RemiliaCard(0,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyAlly)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5m, ValueProp.Move), new PowerVar<StrengthPower>(2m), new EnergyVar(2)];

    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        AttackCommand attackCommand = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this)
            .Targeting(play.Target)
            .WithHitVfxNode((Creature t) => NScratchVfx.Create(t, goingRight: true))
            .Execute(choiceContext);
        
        
        int count = attackCommand.Results.SelectMany((List<DamageResult> r) => r).Sum((DamageResult r) => r.TotalDamage + r.OverkillDamage);
        await RemiliaBloodPool.Gain(Owner, count, this);
        await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, base.DynamicVars["StrengthPower"].BaseValue, base.Owner.Creature, this);
        await PowerCmd.Apply<StrengthPower>(choiceContext, play.Target, base.DynamicVars["StrengthPower"].BaseValue, base.Owner.Creature, this);
        
        bool shouldTriggerFatal = play.Target.Powers.All((PowerModel p) => p.ShouldOwnerDeathTriggerFatal());
        if (shouldTriggerFatal && attackCommand.Results.SelectMany((List<DamageResult> r) => r).Any((DamageResult r) => r.WasTargetKilled))
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, base.DynamicVars["StrengthPower"].BaseValue, base.Owner.Creature, this);
            await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
        base.DynamicVars["StrengthPower"].UpgradeValueBy(2m);
        base.DynamicVars.Energy.UpgradeValueBy(2m);
    }
}