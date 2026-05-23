using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Remilia.RemiliaCode.Powers;

public class RemiliaUncommon30Power : RemiliaPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != base.Owner.Player || cardPlay.Card.Type != CardType.Attack)
        {
            return;
        }
        
        Flash();
        //int count = Math.Min(this.Amount, base.Owner.GetPower<BloodPool>()?.Amount ?? 0);

        int value1 = this.Amount;
        int value2 = base.Owner.GetPower<BloodPool>()?.Amount ?? 0;
        int value3 = Math.Max(0, base.Owner.MaxHp - base.Owner.CurrentHp);
        int count = new[] { value1, value2, value3 }.Min();
        
        await PowerCmd.Apply<BloodPool>(context, base.Owner, -count, base.Owner, null);
        await CreatureCmd.Heal(base.Owner, count);
    }
}