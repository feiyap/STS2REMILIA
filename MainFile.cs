using System.Reflection;
using Godot;
using MegaCrit.Sts2.Core.Modding;
using STS2RitsuLib;
using STS2RitsuLib.Interop;
using STS2RitsuLib.Patching.Core;
using StsLogger = MegaCrit.Sts2.Core.Logging.Logger;

namespace Remilia;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "Remilia";

    public static StsLogger Logger { get; private set; } = null!;

    public static void Initialize()
    {
        var assembly = Assembly.GetExecutingAssembly();

        Logger = RitsuLibFramework.CreateLogger(ModId);
        ModTypeDiscoveryHub.RegisterModAssembly(ModId, assembly);
        RitsuLibFramework.EnsureGodotScriptsRegistered(assembly, Logger);

        var patcher = RitsuLibFramework.CreatePatcher(ModId, "main");
        RitsuLibFramework.ApplyRequiredPatcher(patcher, static () =>
            Logger.Error("Remilia 因 RitsuLib 必要补丁未能应用而停用。"));
    }
}
