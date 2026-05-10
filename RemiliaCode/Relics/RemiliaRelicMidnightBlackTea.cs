using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using Remilia.RemiliaCode.Powers;
using Remilia.RemiliaCode.Relics;

namespace Remilia.RemiliaCode.Relics;

public class RemiliaRelicMidnightBlackTea() : RemiliaRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Common;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BloodPool>(2m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<BloodPool>()];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom)
        {
            Flash();
            await PowerCmd.Apply<BloodPool>(base.Owner.Creature, base.DynamicVars["BloodPool"].BaseValue, base.Owner.Creature, null);
        }
    }
}