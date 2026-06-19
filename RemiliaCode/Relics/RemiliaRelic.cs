using Remilia.RemiliaCode.Character;
using Remilia.RemiliaCode.Extensions;
using Godot;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Remilia.RemiliaCode.Relics;

#pragma warning disable RITSU001 // 抽象遗物模板，本地化由具体派生遗物提供
[RegisterRelic(typeof(RemiliaRelicPool), Inherit = true)]
public abstract class RemiliaRelic : ModRelicTemplate
{
    private string ImageStem => GetType().ModelImageStem();

    public override string CustomIconPath
    {
        get
        {
            var path = $"{ImageStem}.png".RelicImagePath();
            return ResourceLoader.Exists(path) ? path : "relic.png".RelicImagePath();
        }
    }

    public override string CustomIconOutlinePath
    {
        get
        {
            var path = $"{ImageStem}_outline.png".RelicImagePath();
            return ResourceLoader.Exists(path) ? path : "relic_outline.png".RelicImagePath();
        }
    }

    public override string CustomBigIconPath
    {
        get
        {
            var path = $"{ImageStem}_big.png".BigRelicImagePath();
            return ResourceLoader.Exists(path) ? path : "relic.png".BigRelicImagePath();
        }
    }
}
