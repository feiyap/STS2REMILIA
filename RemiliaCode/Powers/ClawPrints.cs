using BaseLib.Extensions;
using BaseLib.Utils;
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

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        if (target != base.Owner)
        {
            return base.ModifyDamageAdditive(target, amount, props, dealer, cardSource);
        }
        if (!props.IsPoweredAttack_())
        {
            return base.ModifyDamageAdditive(target, amount, props, dealer, cardSource);
        }
        decimal num = base.DynamicVars["DamageIncrease"].BaseValue * Amount;
        
        return num;
    }
}