using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Remilia.RemiliaCode.Cards;
using Remilia.RemiliaCode.Cards.Curse;
using Remilia.RemiliaCode.Powers;

namespace Remilia.RemiliaCode.Cards.Uncommon;

public class RemiliaUncommon21() : RemiliaCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BloodPlague>(2m)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BloodPlague>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        foreach (Creature hittableEnemy in base.CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<BloodPlague>(hittableEnemy, base.DynamicVars["BloodPlague"].BaseValue, base.Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["BloodPlague"].UpgradeValueBy(1m);
    }
}