using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Remilia.RemiliaCode.Powers;

public class RemiliaCommon19Power : RemiliaPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side == base.Owner.Side)
        {
            await CardPileCmd.Draw(new BlockingPlayerChoiceContext(), base.Amount, base.Owner.Player);
            await PowerCmd.Decrement(this);
        }
    }
}