using BaseLib.Abstracts;
using BaseLib.Extensions;
using Remilia.RemiliaCode.Extensions;
using Godot;

namespace Remilia.RemiliaCode.Powers;

public abstract class RemiliaPower : CustomPowerModel
{
    //Loads from Remilia/images/powers/your_power.png
    public override string CustomPackedIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".PowerImagePath();
        }
    }

    public override string CustomBigIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_big.png".BigPowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".BigPowerImagePath();
        }
    }
}