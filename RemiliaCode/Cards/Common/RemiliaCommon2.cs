using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards;

namespace Remilia.RemiliaCode.Cards.Common;

public class RemiliaCommon2() : RemiliaCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(9m, ValueProp.Move),
        new HpLossVar(3m),
        new CardsVar(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Fatal)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        VfxCmd.PlayOnCreatureCenter(base.Owner.Creature, "vfx/vfx_bloody_impact");
        await CreatureCmd.Damage(choiceContext, base.Owner.Creature, base.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        AttackCommand attackCommand = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(base.CombatState)
            .WithHitFx("vfx/vfx_attack_blunt", null, "heavy_attack.mp3")
            .Execute(choiceContext);
        
        bool shouldTriggerFatal = play.Target.Powers.All((PowerModel p) => p.ShouldOwnerDeathTriggerFatal());
        if (shouldTriggerFatal && attackCommand.Results.Any((DamageResult r) => r.WasTargetKilled))
        {
            await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, base.Owner);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(4m);
    }
}