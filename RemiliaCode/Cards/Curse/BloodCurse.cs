using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Cards;

namespace Remilia.RemiliaCode.Cards.Curse;

public class BloodCurse() : RemiliaCard(0,
    CardType.Curse, CardRarity.Curse,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HpLossVar(1m), new CardsVar(1)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    public override bool HasTurnEndInHandEffect => true;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        VfxCmd.PlayOnCreatureCenter(base.Owner.Creature, "vfx/vfx_bloody_impact");
        await CreatureCmd.Damage(choiceContext, base.Owner.Creature, base.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, base.Owner);
    }

    public override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        await CreatureCmd.Damage(choiceContext, base.Owner.Creature, base.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
    }
    
    public static async Task<CardModel?> CreateInHand(Player owner, CombatState combatState)
    {
        return (await CreateInHand(owner, 1, combatState)).FirstOrDefault();
    }
    
    public static async Task<IEnumerable<CardModel>> CreateInHand(Player owner, int count, CombatState combatState)
    {
        if (count == 0)
        {
            return Array.Empty<CardModel>();
        }
        if (CombatManager.Instance.IsOverOrEnding)
        {
            return Array.Empty<CardModel>();
        }
        List<CardModel> bloodCurses = new List<CardModel>();
        for (int i = 0; i < count; i++)
        {
            bloodCurses.Add(combatState.CreateCard<BloodCurse>(owner));
        }
        await CardPileCmd.AddGeneratedCardsToCombat(bloodCurses, PileType.Hand, addedByPlayer: true);
        return bloodCurses;
    }
}