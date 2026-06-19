using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using Remilia.RemiliaCode.Character;
using Remilia.RemiliaCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using Remilia.RemiliaCode.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Remilia.RemiliaCode.Cards;

#pragma warning disable RITSU001 // 抽象卡牌模板，本地化由具体派生卡牌提供
[RegisterCard(typeof(RemiliaCardPool), Inherit = true)]
public abstract class RemiliaCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    ModCardTemplate(cost, type, rarity, target)
{
    private string ImageStem => GetType().ModelImageStem();

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

    public bool IsBloodPoolCount(int count)
    {
        return base.Owner.Creature.GetPowerAmount<BloodPool>() >= count;
    }

    public bool IsDrawInRound()
    {
        return CombatManager.Instance.History.Entries.OfType<CardDrawnEntry>().Count((CardDrawnEntry e) =>
            e.HappenedThisTurn(base.CombatState) && e.Actor == base.Owner.Creature && !e.FromHandDraw) > 0;
    }
}
