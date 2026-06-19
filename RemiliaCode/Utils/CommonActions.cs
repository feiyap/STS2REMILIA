using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Remilia.RemiliaCode.Utils;

/// <summary>
/// 原 BaseLib CommonActions 的精简替代，仅保留本 Mod 实际使用的方法。
/// </summary>
public static class CommonActions
{
    public static AttackCommand CardAttack(
        CardModel card,
        Creature? target,
        int hitCount = 1,
        string vfx = "vfx/vfx_attack_slash")
    {
        ArgumentNullException.ThrowIfNull(target);

        var command = DamageCmd.Attack(card.DynamicVars.Damage.BaseValue)
            .FromCard(card)
            .Targeting(target)
            .WithHitFx(vfx);

        if (hitCount > 1)
            command = command.WithHitCount(hitCount);

        return command;
    }

    public static Task CardBlock(CardModel card, CardPlay play)
    {
        return CreatureCmd.GainBlock(card.Owner.Creature, card.DynamicVars.Block, play);
    }

    public static Task CardBlock(CardModel card, BlockVar blockVar, CardPlay play)
    {
        return CreatureCmd.GainBlock(card.Owner.Creature, blockVar, play);
    }
}
