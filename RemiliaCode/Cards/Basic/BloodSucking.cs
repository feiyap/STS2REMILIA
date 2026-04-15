using BaseLib.Abstracts;
using BaseLib.Utils;
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

namespace Remilia.RemiliaCode.Cards.Basic;

public class BloodSucking() : RemiliaCard(1,
    CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy), ITranscendenceCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8, ValueProp.Move)];
    
    public CardModel GetTranscendenceTransformedCard() => ModelDb.Card<RemiliaAncient2>();

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this)
            .Targeting(play.Target)
            .WithHitVfxNode((Creature t) => NScratchVfx.Create(t, goingRight: true))
            .Execute(choiceContext);
        
        
        int count = Math.Min(attackCommand.Results.Sum((DamageResult r) => r.TotalDamage + r.OverkillDamage), base.Owner.Creature.GetPower<BloodPool>()?.Amount ?? 0);
        await PowerCmd.Apply<BloodPool>(base.Owner.Creature, -count, base.Owner.Creature, null);
        await CreatureCmd.Heal(base.Owner.Creature, count);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}