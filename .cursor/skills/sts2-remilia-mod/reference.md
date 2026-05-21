# Remilia mod — new content checklist

## New card

- [ ] `RemiliaCode/Cards/{Rarity}/Remilia{Rarity}{N}.cs` extends `RemiliaCard`
- [ ] `CanonicalVars`, `OnPlay`, `OnUpgrade` as needed
- [ ] Blood-cost cards: `BloodCost` var + `IsPlayable` / negative `BloodPool` apply
- [ ] Linked power class if card applies a custom power
- [ ] `Remilia/images/card_portraits/remilia_{rarity}{n}.png` (snake_case from `Id.Entry`)
- [ ] Keys in `cards.json` for eng, zhs, jpn, kor
- [ ] Add to pool only if needed (cards auto-register via model discovery)

## New power

- [ ] `RemiliaCode/Powers/Remilia{Card}Power.cs` or standalone name
- [ ] Set `Type`, `StackType`, `AllowNegative` if debuff/counter
- [ ] `CanonicalVars` for smart descriptions
- [ ] `powers.json` in all four locales
- [ ] `Remilia/images/powers/{id}.png` + `powers/big/{id}_big.png`

## New relic

- [ ] `RemiliaCode/Relics/RemiliaRelic{Name}.cs` extends `RemiliaRelic`
- [ ] `Rarity`, hooks, `[SavedProperty]` if counter persists across combats
- [ ] `relics.json` in all locales
- [ ] Icon + outline + big images under `Remilia/images/relics/`

## Localization key reference

| Class | Typical key prefix |
|-------|-------------------|
| `StrikeRemilia` | `REMILIA-STRIKE_REMILIA` |
| `RemiliaCommon1` | `REMILIA-REMILIA_COMMON1` |
| `RemiliaRare18Power` | `REMILIA-REMILIA_RARE18_POWER` |
| `RemiliaRelicRedBlood` | `REMILIA-REMILIA_RELIC_RED_BLOOD` |

Power smart descriptions use `{Amount}`; card vars use the `DynamicVar` name (e.g. `{BloodPool:diff()}`).

## Common commands

```csharp
// Attack
await DamageCmd.Attack(value).FromCard(this).Targeting(target).Execute(ctx);

// Block
await CreatureCmd.GainBlock(ctx, Owner.Creature, amount);

// Draw
await CardPileCmd.Draw(ctx, count, Owner);

// Apply power
await PowerCmd.Apply<PowerType>(ctx, creature, amount, applier, cardSource);

// VFX
await CreatureCmd.TriggerAnim(creature, "Cast", delay);
VfxCmd.PlayOnCreatureCenter(creature, "vfx/vfx_bloody_impact");
```

## Namespace map

```
Remilia                          → MainFile, mod init
Remilia.RemiliaCode.Cards        → RemiliaCard, card folders
Remilia.RemiliaCode.Powers       → RemiliaPower, BloodPool, etc.
Remilia.RemiliaCode.Relics       → RemiliaRelic
Remilia.RemiliaCode.Character    → Remilia, pools
Remilia.RemiliaCode.Extensions   → asset path helpers
Remilia.RemiliaCode.Keywords     → custom card keywords
```
