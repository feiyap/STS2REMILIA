using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Powers;
using Remilia.RemiliaCode.Relics;

namespace Remilia.RemiliaCode.Relics;

public class RemiliaRelicScarletBlood() : RemiliaRelic
{
    public override bool ShowCounter => true;
    private int _savedBlood;

    [SavedProperty]
    private int SavedBlood
    {
        get => _savedBlood;
        set
        {
            AssertMutable();
            _savedBlood = value;
            InvokeDisplayAmountChanged();
        }
    }
    
    public override RelicRarity Rarity =>
        RelicRarity.Starter;
    
    public override int DisplayAmount => SavedBlood;

    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<RemiliaRelicRedBlood>();
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [new DynamicVar("HpLossReduction", 100m), new DynamicVar("BloodPoolLift", 50m)];
    
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        int count = (int)base.DynamicVars["HpLossReduction"].BaseValue * result.UnblockedDamage / 100;
        if (target == base.Owner.Creature && result.UnblockedDamage > 0)
        {
            Flash();
            await PowerCmd.Apply<BloodPool>(base.Owner.Creature, count, base.Owner.Creature, null);
        }
    }
    
    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side == base.Owner.Creature.Side && combatState.RoundNumber <= 1)
        {
            Flash();
            await PowerCmd.Apply<BloodPool>(base.Owner.Creature, SavedBlood, base.Owner.Creature, null);
            SavedBlood = 0;
        }
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        if (!base.Owner.Creature.IsDead)
        {
            Flash();
            int count = base.Owner.Creature.GetPower<BloodPool>()?.Amount ?? 0;
            SavedBlood = (int)((base.DynamicVars["BloodPoolLift"].BaseValue * count) / 100);
        }

        return Task.CompletedTask;
    }
}