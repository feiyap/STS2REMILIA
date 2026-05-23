// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.TemporaryStrengthPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CE48D45-8CD3-4DF6-BE46-E66DDDBEF566
// Assembly location: F:\SteamLibrary\steamapps\common\Slay the Spire 2\data_sts2_windows_x86_64\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Remilia.RemiliaCode.Powers;

#nullable enable
namespace Remilia.RemiliaCode.Powers;

public class RemiliaUncommon24Power : RemiliaPower, ITemporaryPower
{
  private bool _shouldIgnoreNextInstance;

  public override PowerType Type => !this.IsPositive ? PowerType.Debuff : PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public PowerModel InternallyAppliedPower => (PowerModel) ModelDb.Power<StrengthPower>();

  protected virtual bool IsPositive => true;

  private int Sign => !this.IsPositive ? -1 : 1;

  public void IgnoreNextInstance() => this._shouldIgnoreNextInstance = true;
  
  public AbstractModel OriginModel { get; }

  public override async Task BeforeApplied(
    Creature target,
    Decimal amount,
    Creature? applier,
    CardModel? cardSource)
  {
    if (this._shouldIgnoreNextInstance)
    {
      this._shouldIgnoreNextInstance = false;
    }
    else
    {
      StrengthPower strPower = await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), target, (Decimal) this.Sign * amount, applier, cardSource, true);
      DexterityPower sexPower = await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), target, (Decimal) this.Sign * amount, applier, cardSource, true);
    }
  }

  public override async Task AfterPowerAmountChanged(
    PlayerChoiceContext choiceContext, 
    PowerModel power,
    Decimal amount,
    Creature? applier,
    CardModel? cardSource)
  {
    RemiliaUncommon24Power temporaryStrengthPower = this;
    if (amount == (Decimal) temporaryStrengthPower.Amount || power != temporaryStrengthPower)
      return;
    if (temporaryStrengthPower._shouldIgnoreNextInstance)
    {
      temporaryStrengthPower._shouldIgnoreNextInstance = false;
    }
    else
    {
      StrengthPower strPower = await PowerCmd.Apply<StrengthPower>(choiceContext,temporaryStrengthPower.Owner, (Decimal) temporaryStrengthPower.Sign * amount, applier, cardSource, true);
      DexterityPower dexPower = await PowerCmd.Apply<DexterityPower>(choiceContext,temporaryStrengthPower.Owner, (Decimal) temporaryStrengthPower.Sign * amount, applier, cardSource, true);
    }
  }

  public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
  {
    RemiliaUncommon24Power power = this;
    if (side != power.Owner.Side)
      return;
    power.Flash();
    await PowerCmd.Remove((PowerModel) power);
    StrengthPower stPower = await PowerCmd.Apply<StrengthPower>(choiceContext, power.Owner, (Decimal) (-power.Sign * power.Amount), power.Owner, (CardModel) null);
    DexterityPower dexthPower = await PowerCmd.Apply<DexterityPower>(choiceContext, power.Owner, (Decimal) (-power.Sign * power.Amount), power.Owner, (CardModel) null);
  }
}
