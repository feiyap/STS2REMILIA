using BaseLib.Abstracts;
using BaseLib.Utils;
using Remilia.RemiliaCode.Character;

namespace Remilia.RemiliaCode.Potions;

[Pool(typeof(RemiliaPotionPool))]
public abstract class RemiliaPotion : CustomPotionModel;