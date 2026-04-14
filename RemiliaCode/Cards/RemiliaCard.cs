using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using Remilia.RemiliaCode.Character;
using Remilia.RemiliaCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using Remilia.RemiliaCode.Powers;

namespace Remilia.RemiliaCode.Cards;

[Pool(typeof(RemiliaCardPool))]
public abstract class RemiliaCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    CustomCardModel(cost, type, rarity, target)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath
    {
        get
        {
            var path =  $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
            return ResourceLoader.Exists(path) ? path : "card.png".CardImagePath();
        }
    }
    
    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190
    
    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath
    {
        get
        {
            var path =  $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            return ResourceLoader.Exists(path) ? path : "card.png".CardImagePath();
        }
    }
    public override string BetaPortraitPath
    {
        get
        {
            var path =  $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            return ResourceLoader.Exists(path) ? path : "card.png".CardImagePath();
        }
    }
    
    public bool IsBloodPoolCount(int  count)
    {
        return base.Owner.Creature.GetPowerAmount<BloodPool>() >= count;
    }

    public bool IsDrawInRound()
    {
        return CombatManager.Instance.History.Entries.OfType<CardDrawnEntry>().Count((CardDrawnEntry e) =>
            e.HappenedThisTurn(base.CombatState) && e.Actor == base.Owner.Creature && !e.FromHandDraw) > 0;
    }
}