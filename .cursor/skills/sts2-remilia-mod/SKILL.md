---
name: sts2-remilia-mod
description: Develops the Remilia Slay the Spire 2 mod — cards, powers, relics, localization, and Godot assets. Use when the user attaches this skill or asks about Remilia mod work, STS2 modding, Blood Pool, or content in RemiliaCode/.
disable-model-invocation: true
---

# Remilia STS2 Mod

## Project layout

| Path | Purpose |
|------|---------|
| `RemiliaCode/` | C# gameplay (cards, powers, relics, character) |
| `Remilia/` | Godot assets + `localization/{eng,zhs,jpn,kor}/` |
| `Remilia.json` | Mod manifest (`id`: `Remilia`, depends on BaseLib ≥ 3.1.3) |
| `MainFile.cs` | `ModId = "Remilia"`, Harmony entry |
| `Remilia.csproj` | Builds DLL → `mods/Remilia/`, exports `.pck` via Godot 4.5.1 |

**Stack:** .NET 9, Godot 4.5.1 (must match game), `Alchyr.Sts2.BaseLib`, `MegaCrit.Sts2` APIs, `Alchyr.Sts2.ModAnalyzers`.

## Base classes — always extend these

- **Cards:** `RemiliaCard` in `RemiliaCode/Cards/` — auto portrait paths, `IsBloodPoolCount()`, `IsDrawInRound()`
- **Powers:** `RemiliaPower` in `RemiliaCode/Powers/`
- **Relics:** `RemiliaRelic` in `RemiliaCode/Relics/` — `[Pool(typeof(RemiliaRelicPool))]`
- **Character:** `Remilia` — pools, starting deck/relics, UI paths

Naming: `RemiliaCommon1`, `RemiliaRare18Power`, `RemiliaRelicRedBlood`. File name = class name.

## Card template

```csharp
public class RemiliaCommonN() : RemiliaCard(cost,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(11, ValueProp.Move), new DynamicVar("BloodPool", 1m)];

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this)
            .Targeting(play.Target).Execute(ctx);
        await PowerCmd.Apply<BloodPool>(ctx, Owner.Creature,
            DynamicVars["BloodPool"].BaseValue, Owner.Creature, null);
    }

    protected override void OnUpgrade() =>
        DynamicVars.Damage.UpgradeValueBy(3m);
}
```

**Blood Pool consumption:** gate with `IsPlayable => IsBloodPoolCount(bloodCost)`; apply negative amount: `PowerCmd.Apply<BloodPool>(..., -cost, ...)`.

## Power / relic hooks

Override async hooks on `RemiliaPower` / `RemiliaRelic` (e.g. `AfterDamageReceived`, `AfterSideTurnStart`, `AfterPowerAmountChanged`). Call `Flash()` for VFX. Persist run state with `[SavedProperty]` + `AssertMutable()` on setters.

## Localization

Keys: `REMILIA-{MODEL_ID}.title` / `.description` — model ID is class name uppercased with underscores (e.g. `RemiliaCommon1` → `REMILIA-REMILIA_COMMON1`).

Update **all** locales when adding strings: `eng`, `zhs`, `jpn`, `kor` under `Remilia/localization/`.

Description syntax:
- `{Damage:diff()}` — upgrade diff
- `{BloodPool:diff()}`, `{Amount}` on powers
- `[gold]Blood Pool[/gold]` — keyword styling
- `{Cards:plural:|s}` — pluralization

Register custom keywords in `RemiliaCode/Keywords/RemiliaKeywords.cs` (`[CustomEnum]`, `[KeywordProperties]`).

## Assets (auto-resolved from class `Id`)

| Type | Path under `Remilia/images/` |
|------|------------------------------|
| Card portrait | `card_portraits/{snake_case_id}.png` |
| Power | `powers/{id}.png`, `powers/big/{id}_big.png` |
| Relic | `relics/{id}.png`, `{id}_outline.png`, `relics/big/{id}_big.png` |

Use `StringExtensions`: `.CardImagePath()`, `.PowerImagePath()`, `.RelicImagePath()`, `.CharacterUiPath()`. Missing files fall back to `card.png` / `power.png` / `relic.png`.

## Core mechanics

- **Blood Pool** (`BloodPool` power): buff counter; skills consume via negative apply; many cards check `IsBloodPoolCount`
- **Blood Plague** (`BloodPlague`): debuff; triggers damage when debuffs applied to holder
- **Claw Marks** (`ClawPrints`): custom keyword enum `CLAWPRINTS`

Reuse existing powers with `PowerCmd.Apply<T>`, `GetPowerAmount<T>()`, `GetPower<T>()`.

## Build & test

1. Set `Sts2DataDir` / `GodotPath` in `Remilia.csproj` (or install StS2 in default Steam path)
2. `dotnet build` — copies DLL + `Remilia.json` to `mods/Remilia/`
3. `dotnet publish` — also exports `Remilia.pck` if Godot path is valid
4. Do not commit `.godot/` cache or duplicate scene copies

## Conventions

- Match existing file's `using` order and primary constructor syntax `() : Base(...)`
- Chinese comment on card = design note; keep new comments minimal
- Minimize diff scope — one card/power/relic + matching localization + art paths
- Run/build after logic changes; fix `ModAnalyzers` warnings

## Additional resources

- New content checklist: [reference.md](reference.md)
