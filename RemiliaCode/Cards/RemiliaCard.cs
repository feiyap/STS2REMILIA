using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Models;
using Remilia.RemiliaCode.Character;
using Remilia.RemiliaCode.Extensions;
using Remilia.RemiliaCode.Resources;
using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Remilia.RemiliaCode.Cards;

#pragma warning disable RITSU001 // 抽象卡牌模板，本地化由具体派生卡牌提供
[RegisterCard(typeof(RemiliaCardPool), Inherit = true)]
public abstract class RemiliaCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    ModCardTemplate(cost, type, rarity, target)
{
    private string ImageStem => GetType().ModelImageStem();

    /// <summary>
    /// 是否在创建/升级时自动将 DynamicVar「BloodCost」绑定为次级资源费用。
    /// </summary>
    protected virtual bool AutoBindBloodCost => false;

    public override string CustomPortraitPath
    {
        get
        {
            var path = $"{ImageStem}.png".CardImagePath();
            return ResourceLoader.Exists(path) ? path : "card.png".CardImagePath();
        }
    }

    public override string PortraitPath
    {
        get
        {
            var path = $"{ImageStem}.png".CardImagePath();
            return ResourceLoader.Exists(path) ? path : "card.png".CardImagePath();
        }
    }

    public override string BetaPortraitPath
    {
        get
        {
            var path = $"beta/{ImageStem}.png".CardImagePath();
            return ResourceLoader.Exists(path) ? path : "card.png".CardImagePath();
        }
    }

    /// <summary>
    /// 注册克隆回调，确保每场战斗复制出的卡牌实例也会绑定血池费用。
    /// </summary>
    public static void RegisterLifecycleHooks()
    {
        RitsuLibFramework.GetModelCloneRegistry(MainFile.ModId)
            .Register<CardModel>("blood_cost_sync", static (_, clone) =>
            {
                if (clone is RemiliaCard card)
                    card.EnsureBloodCostBound();
            });
    }

    public override void AfterCreated()
    {
        base.AfterCreated();
        EnsureBloodCostBound();
    }

    protected override void AfterDeserialized()
    {
        base.AfterDeserialized();
        EnsureBloodCostBound();
    }

    internal void EnsureBloodCostBound()
    {
        if (AutoBindBloodCost)
            SyncBloodCost();
    }

    protected void SyncBloodCost(string varName = "BloodCost")
    {
        this.SecondaryCosts().Set(RemiliaBloodPool.Id, DynamicVars[varName].IntValue);
    }

    public bool HasBloodPool(int count) => RemiliaBloodPool.Has(Owner, count);

    public bool IsBloodPoolCount(int count) => HasBloodPool(count);

    public bool IsDrawInRound()
    {
        return CombatManager.Instance.History.Entries.OfType<CardDrawnEntry>().Count((CardDrawnEntry e) =>
            e.HappenedThisTurn(base.CombatState) && e.Actor == base.Owner.Creature && !e.FromHandDraw) > 0;
    }
}
