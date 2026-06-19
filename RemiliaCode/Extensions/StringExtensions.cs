using System.Text.RegularExpressions;
using Remilia;

namespace Remilia.RemiliaCode.Extensions;

// 资源路径辅助方法。
public static class StringExtensions
{
    public static string ImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "images", path);
    }

    public static string CardImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "images", "card_portraits", path);
    }

    public static string BigCardImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "images", "card_portraits", "big", path);
    }

    public static string PowerImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "images", "powers", path);
    }

    public static string BigPowerImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "images", "powers", "big", path);
    }

    public static string RelicImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "images", "relics", path);
    }

    public static string BigRelicImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "images", "relics", "big", path);
    }

    public static string CharacterUiPath(this string path)
    {
        return Path.Join(MainFile.ModId, "images", "charui", path);
    }

    /// <summary>
    /// 将模型 CLR 类型名转为资源文件名 stem（与 RitsuLib StringHelper.Slugify 规则一致，再转小写）。
    /// </summary>
    public static string ModelImageStem(this Type type)
    {
        var value = type.Name;
        value = Regex.Replace(value, "([A-Za-z0-9]|\\G(?!^))([A-Z])", "$1_$2");
        value = value.ToUpperInvariant();
        value = Regex.Replace(value, "\\s+", "_");
        value = Regex.Replace(value, "[^A-Z0-9_]", "");
        return value.ToLowerInvariant();
    }
}
