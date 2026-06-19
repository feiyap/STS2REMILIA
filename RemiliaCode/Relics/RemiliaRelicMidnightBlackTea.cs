using Remilia;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using Remilia.RemiliaCode.Relics;
using Remilia.RemiliaCode.Resources;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Remilia.RemiliaCode.Relics;

public class RemiliaRelicMidnightBlackTea() : RemiliaRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Common;
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [SecondaryResourceVars.ForLocal("BloodPool", MainFile.ModId, RemiliaBloodPool.LocalId, 2m)];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom)
        {
            Flash();
            await RemiliaBloodPool.Gain(base.Owner, base.DynamicVars["BloodPool"].IntValue, this);
        }
    }
}
