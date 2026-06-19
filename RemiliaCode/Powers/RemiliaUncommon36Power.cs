using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Remilia.RemiliaCode.Resources;

namespace Remilia.RemiliaCode.Powers;

public class RemiliaUncommon36Power : RemiliaPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool _)
    {
        if (card.Owner.Creature == base.Owner)
            await RemiliaBloodPool.Gain(base.Owner.Player, Amount, this);
    }
}
