using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Remilia.RemiliaCode.Relics;

namespace Remilia.RemiliaCode.Relics;

public class RemiliaRelicAncientSeptet() : RemiliaRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new MaxHpVar(1m)];

    public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature target, bool wasRemovalPrevented, float deathAnimLength)
    {
        if (target.Side != base.Owner.Creature.Side && target.Powers.All((PowerModel p) => p.ShouldOwnerDeathTriggerFatal()))
        {
            Flash();
            await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.IntValue);
        }
    }
}