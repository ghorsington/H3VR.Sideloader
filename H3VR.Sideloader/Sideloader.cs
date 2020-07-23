using System;
using System.IO;
using BepInEx;
using XUnity.ResourceRedirector;

namespace H3VR.Sideloader
{
    [BepInPlugin("horse.coder.h3vr.sideloader", NAME, VERSION)]
    [BepInDependency("gravydevsupreme.xunity.resourceredirector")]
    public class Sideloader : BaseUnityPlugin
    {
        internal const string VERSION = "1.0.0";
        internal const string NAME = "H3VR Sideloader";

        internal const string ModsDir = "Mods";

        private void Awake()
        {
            ResourceRedirection.EnableSyncOverAsyncAssetLoads();
            ResourceRedirection.RegisterAsyncAndSyncAssetLoadingHook(PatchLoadedBundle);
        }

        private void LoadMods()
        {
            Logger.LogInfo("Loading mods...");

            var modsPath = Path.Combine(Paths.GameRootPath, ModsDir);
            foreach (var modDir in Directory.GetDirectories(modsPath))
                try
                {
                    var mod = Mod.LoadDir(modDir);
                }
                catch (Exception e)
                {
                    Logger.LogWarning($"Skipping {modDir} because: {e.Message}");
                }
        }

        private void PatchLoadedBundle(IAssetLoadingContext ctx)
        {
            Logger.LogDebug(
                $"Loading asset {ctx.Parameters.Name} from {ctx.GetAssetBundlePath()} (normalized: {ctx.GetNormalizedAssetBundlePath()})");
        }
    }
}