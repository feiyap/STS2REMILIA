using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Powers;

namespace Remilia.RemiliaCode.Powers;

public class ClawPrints : RemiliaPower
{
    private const string _damageIncrease = "DamageIncrease";
    
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool AllowNegative => false;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [new DynamicVar("DamageIncrease", 1m)];
    
    public override bool ShouldPowerBeRemovedAfterOwnerDeath()
    {
        return false;
    }

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource, CardPlay? cardPlay)
    {
        if (target != base.Owner)
        {
            return base.ModifyDamageAdditive(target, amount, props, dealer, cardSource, cardPlay);
        }
        if (!props.IsPoweredAttack())
        {
            return base.ModifyDamageAdditive(target, amount, props, dealer, cardSource, cardPlay);
        }
        decimal num = base.DynamicVars["DamageIncrease"].BaseValue * Amount;
        
        return num;
    }
}