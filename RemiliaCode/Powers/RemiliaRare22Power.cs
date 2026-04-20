using BaseLib.Extensions;
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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Remilia.RemiliaCode.Powers;

public class RemiliaRare22Power : RemiliaPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar("SelfDamage", 1m, ValueProp.Unblockable | ValueProp.Unpowered)];

    public override async Task AfterCardDrawnEarly(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner.Creature == base.Owner && card.Type == CardType.Curse)
        {
            await CreatureCmd.TriggerAnim(base.Owner, "Cast", base.Owner.Player.Character.CastAnimDelay);
            VfxCmd.PlayOnCreatureCenter(base.Owner, "vfx/vfx_bloody_impact");
            DamageVar damageVar = (DamageVar)base.DynamicVars["SelfDamage"];
            await CreatureCmd.Damage(choiceContext, base.Owner, damageVar.BaseValue, damageVar.Props, base.Owner, null);
            
            await CardCmd.Exhaust(choiceContext, card);
        }
    }
    
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool _)
    {
        if (card.Owner.Creature == base.Owner && card.Type == CardType.Curse)
        {
            await PowerCmd.Apply<StrengthPower>(base.Owner, base.Amount, base.Owner, null);
            await CardPileCmd.Draw(choiceContext, 1, base.Owner.Player);
        }
    }
}