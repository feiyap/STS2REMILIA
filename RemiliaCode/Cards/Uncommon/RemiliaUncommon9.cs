using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Cards.Curse;
using Remilia.RemiliaCode.Powers;

namespace Remilia.RemiliaCode.Cards.Uncommon;

public class RemiliaUncommon9() : RemiliaCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7m, ValueProp.Move), new PowerVar<BloodPlague>(3)];
    
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<BloodPlague>(), 
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromPower<ArtifactPower>(),
        HoverTipFactory.FromCard<BloodCurse>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.LoseBlock(play.Target, play.Target.Block);
        if (play.Target.HasPower<ArtifactPower>())
        {
            await PowerCmd.Remove<ArtifactPower>(play.Target);
        }
        
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this)
            .Targeting(play.Target)
            .WithHitVfxNode((Creature t) => NScratchVfx.Create(t, goingRight: true))
            .Execute(choiceContext);
        await PowerCmd.Apply<BloodPlague>(play.Target, base.DynamicVars["BloodPlague"].BaseValue, base.Owner.Creature, null);
        
        await BloodCurse.CreateInHand(base.Owner, base.CombatState);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
        base.DynamicVars["BloodPlague"].UpgradeValueBy(3m);
    }
}