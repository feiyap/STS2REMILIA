using System.Reflection;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using Remilia.RemiliaCode.Extensions;
using STS2RitsuLib;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Scaffolding.Godot.NodeAttachments;
using CharacterRemilia = Remilia.RemiliaCode.Character.Remilia;

namespace Remilia.RemiliaCode.Resources;

/// <summary>
/// 血池次级资源注册与读写封装。
/// </summary>
public static class RemiliaBloodPool
{
    public const string LocalId = "blood_pool";
    private const string CombatUiRowLocalId = "blood_pool_row";

    /// <summary>
    /// 相对能量计数器原点的偏移：右侧 120、上方 120。
    /// </summary>
    private static readonly Vector2 EnergyCounterOffset = new(120f, -60f);

    private static readonly FieldInfo? EnergyCounterField =
        typeof(NCombatUi).GetField("_energyCounter", BindingFlags.Instance | BindingFlags.NonPublic);

    public static SecondaryResourceDefinition Definition { get; private set; } = null!;

    public static string Id => Definition.Id;

    public static void Register()
    {
        var registry = RitsuLibFramework.GetSecondaryResourceRegistry(MainFile.ModId);

        Definition = registry.Register(LocalId, new SecondaryResourceDefinition(
            defaultAmount: 0,
            minAmount: 0,
            turnStartPolicy: SecondaryResourceTurnStartPolicy.None,
            persistencePolicy: SecondaryResourcePersistencePolicy.None,
            smallIconPath: "blood_pool.png".PowerImagePath(),
            largeIconPath: "blood_pool_big.png".BigPowerImagePath()));

        registry.AlwaysShowInCombatUiForCharacter<CharacterRemilia>(Definition.LocalId);

        registry.RegisterCombatUi<NCombatUi, NSecondaryResourceCounterRow>(
            CombatUiRowLocalId,
            static _ => new NSecondaryResourceCounterRow(),
            static ctx =>
            {
                // 节点 attachment 重建后，旧 updater 闭包仍可能持有已释放的 ctx.Node。
                if (!ModNodeAttachmentRegistry.For(MainFile.ModId)
                        .TryGetAttached(ctx.Parent, CombatUiRowLocalId, out NSecondaryResourceCounterRow row))
                    return;

                EnsureCombatUiPlacement(ctx.Parent, row);
                row.Bind(ctx.Player, ctx.VisibleDefinitions);
            },
            CreateCombatUiOptions());
    }

    public static int Get(Player player) => SecondaryResourceCmd.Get(player, Id);

    public static bool Has(Player player, int amount) => Get(player) >= amount;

    public static Task Gain(Player player, int amount, AbstractModel? source = null) =>
        SecondaryResourceCmd.Gain(player, Id, amount, source);

    public static Task Lose(Player player, int amount, AbstractModel? source = null) =>
        SecondaryResourceCmd.Lose(player, Id, amount, source);

    public static Task Spend(Player player, int amount, CardModel? card = null, AbstractModel? source = null) =>
        SecondaryResourceCmd.Spend(player, Id, amount, card, source);

    private static NodeAttachmentOptions CreateCombatUiOptions() => new()
    {
        Name = CombatUiRowLocalId,
        DuplicatePolicy = NodeAttachmentDuplicatePolicy.ReuseExistingByName,
        // 挂到 EnergyCounterContainer 而非 _energyCounter：Activate 每次都会重建能量球，
        // 联机模式下旧 energy counter 被释放时会连带释放血池行，触发 RitsuLib 陈旧 updater 崩溃。
        AttachParentSelector = static combatUi =>
            combatUi is NCombatUi ui && GodotObject.IsInstanceValid(ui.EnergyCounterContainer)
                ? ui.EnergyCounterContainer
                : combatUi,
    };

    private static bool TryGetEnergyCounter(Node combatUi, out Node energyCounter)
    {
        energyCounter = null!;
        if (combatUi is not NCombatUi ||
            EnergyCounterField?.GetValue(combatUi) is not Node counter ||
            !GodotObject.IsInstanceValid(counter))
            return false;

        energyCounter = counter;
        return true;
    }

    private static void EnsureCombatUiPlacement(NCombatUi combatUi, NSecondaryResourceCounterRow row)
    {
        if (!GodotObject.IsInstanceValid(combatUi) || !GodotObject.IsInstanceValid(row))
            return;

        var attachParent = combatUi.EnergyCounterContainer;
        if (!GodotObject.IsInstanceValid(attachParent))
            return;

        if (row.GetParent() != attachParent)
        {
            var scale = row.Scale;
            row.Reparent(attachParent);
            row.Scale = scale;
        }

        row.Position = TryGetEnergyCounter(combatUi, out var energyCounter) && energyCounter is Control energyControl
            ? energyControl.Position + EnergyCounterOffset
            : EnergyCounterOffset;
    }
}
