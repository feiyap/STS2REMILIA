using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Relics;
using Remilia.RemiliaCode.Resources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Remilia.RemiliaCode.Relics;

public class RemiliaRelicBloodstainedDress() : RemiliaRelic, ISecondaryResourceHookListener
{
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(1m, ValueProp.Unpowered)];

    public async Task AfterSecondaryResourceChanged(SecondaryResourceChangeContext context)
    {
        if (context.Definition.Id != RemiliaBloodPool.Id || context.Delta >= 0)
            return;

        Flash();
        decimal count = base.DynamicVars.Block.BaseValue * -context.Delta;
        await CreatureCmd.GainBlock(base.Owner.Creature, count, ValueProp.Unpowered, null);
    }
}
