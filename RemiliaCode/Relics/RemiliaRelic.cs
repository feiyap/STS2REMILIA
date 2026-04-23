using System.Diagnostics;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Remilia.RemiliaCode.Character;
using Remilia.RemiliaCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Logging;

namespace Remilia.RemiliaCode.Relics;

[Pool(typeof(RemiliaRelicPool))]
public abstract class RemiliaRelic : CustomRelicModel
{
    public override string PackedIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
            return ResourceLoader.Exists(path) ? path : "relic.png".RelicImagePath();
        }
    }

    protected override string PackedIconOutlinePath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
            return ResourceLoader.Exists(path) ? path : "relic_outline.png".RelicImagePath();
        }
    }

    protected override string BigIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_big.png".BigRelicImagePath();
            return ResourceLoader.Exists(path) ? path : "relic.png".BigRelicImagePath();
        }
    }
}