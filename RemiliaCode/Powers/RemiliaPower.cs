using Remilia.RemiliaCode.Extensions;
using Godot;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Remilia.RemiliaCode.Powers;

#pragma warning disable RITSU001 // 抽象能力模板，本地化由具体派生能力提供
[RegisterPower(Inherit = true)]
public abstract class RemiliaPower : ModPowerTemplate
{
    private string ImageStem => GetType().ModelImageStem();

    public override string CustomIconPath
    {
        get
        {
            var path = $"{ImageStem}.png".PowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".PowerImagePath();
        }
    }

    public override string CustomBigIconPath
    {
        get
        {
            var path = $"{ImageStem}_big.png".BigPowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".BigPowerImagePath();
        }
    }
}
