using System.Linq;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Remilia.RemiliaCode.Powers;

public class RemiliaRare20Power : RemiliaPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [HoverTipFactory.FromPower<ClawPrints>()];

    // 在 AfterAttack 中施加爪痕，避免在 AfterDamageGiven 中嵌套施加能力导致与蜂巢术士等敌人的受击效果死锁。
    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        if (command.Attacker != base.Owner || !command.DamageProps.IsPoweredAttack())
            return;
        if (command.ModelSource is not CardModel cardSource || cardSource.Type != CardType.Attack)
            return;

        foreach (DamageResult result in command.Results.SelectMany(r => r))
        {
            if (result.UnblockedDamage > 0 && result.Receiver.IsAlive)
            {
                Flash();
                await PowerCmd.Apply<ClawPrints>(choiceContext, result.Receiver, base.Amount, base.Owner, cardSource);
            }
        }
    }
}
