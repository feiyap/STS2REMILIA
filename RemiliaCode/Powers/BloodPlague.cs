using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;
using Remilia.RemiliaCode.Powers;

namespace Remilia.RemiliaCode.Powers;

public class BloodPlague : RemiliaPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool AllowNegative => false;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [new DamageVar(1m, ValueProp.Unpowered)];

    public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (!(amount == 0m) && power.GetTypeForAmount(amount) == PowerType.Debuff && power.Owner == base.Owner && !(power is ITemporaryPower))
        {
            Flash();
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), power.Owner, base.Amount, ValueProp.Unpowered, base.Applier, null);
        }
    }
}