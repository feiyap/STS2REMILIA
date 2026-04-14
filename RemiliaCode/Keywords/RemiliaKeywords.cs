using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Remilia.RemiliaCode.Keywords;

public class RemiliaKeywords
{
    // 自定义枚举的名字。最终会变成{前缀}-{枚举值大写}的形式，例如TEST-UNIQUE
    [CustomEnum("CLAWPRINTS")]
    // 放在原版卡牌描述的位置，这里是卡牌描述的前面
    [KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Unique;
}