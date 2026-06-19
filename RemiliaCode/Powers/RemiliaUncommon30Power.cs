using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Remilia.RemiliaCode.Resources;

namespace Remilia.RemiliaCode.Powers;

public class RemiliaUncommon30Power : RemiliaPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != base.Owner.Player || cardPlay.Card.Type != CardType.Attack)
            return;
        
        Flash();
        int value1 = Amount;
        int value2 = RemiliaBloodPool.Get(base.Owner.Player);
        int value3 = Math.Max(0, base.Owner.MaxHp - base.Owner.CurrentHp);
        int count = new[] { value1, value2, value3 }.Min();
        
        await RemiliaBloodPool.Lose(base.Owner.Player, count, this);
        await CreatureCmd.Heal(base.Owner, count);
    }
}
