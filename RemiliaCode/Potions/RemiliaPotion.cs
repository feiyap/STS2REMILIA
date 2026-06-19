using Remilia.RemiliaCode.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Remilia.RemiliaCode.Potions;

#pragma warning disable RITSU001 // 抽象药水模板，本地化由具体派生药水提供
[RegisterPotion(typeof(RemiliaPotionPool), Inherit = true)]
public abstract class RemiliaPotion : ModPotionTemplate;
