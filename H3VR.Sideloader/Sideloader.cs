using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using XUnity.ResourceRedirector;

namespace H3VR.Sideloader
{
    [BepInPlugin("horse.coder.h3vr.sideloader", NAME, VERSION)]
    [BepInDependency("gravydevsupreme.xunity.resourceredirector")]
    public class Sideloader : BaseUnityPlugin
    {
        internal const string VERSION = "1.0.0";
        internal const string NAME = "H3VR Sideloader";
        internal const string MODS_DIR = "Mods";

        internal static new ManualLogSource Logger;

        private static readonly string[] TexturePathSchema =
        {
            "prefabPath",
            "materialName",
            "materialParameter",
            "textureName"
        };

        private AssetTree textureAssets = new AssetTree(TexturePathSchema.Length);
        
        private void Awake()
        {
            Logger = base.Logger;
            ResourceRedirection.EnableSyncOverAsyncAssetLoads();
            ResourceRedirection.RegisterAsyncAndSyncAssetLoadingHook(PatchLoadedBundle);
            
            LoadMods();
        }

        private void LoadMods()
        {
            Logger.LogInfo("Loading mods...");

            var mods = new List<Mod>();

            var modsPath = Path.Combine(Paths.GameRootPath, MODS_DIR);
            foreach (var modDir in Directory.GetDirectories(modsPath))
                try
                {
                    var mod = Mod.LoadFromDir(modDir);
                    mods.Add(mod);
                }
                catch (Exception e)
                {
                    Logger.LogWarning($"Skipping {modDir} because: {e.Message}");
                }

            foreach (var file in Directory.GetFiles(modsPath, "*.h3mod", SearchOption.TopDirectoryOnly))
                try
                {
                    var mod = Mod.LoadFromZip(file);
                    mods.Add(mod);
                }
                catch (Exception e)
                {
                    Logger.LogWarning($"Skipping {file} because: {e.Message}");
                }
            
            // TODO: Sanity checking etc
            foreach (var mod in mods)
            {
                mod.RegisterTreeAssets(textureAssets, AssetType.Texture);
            }
        }

        private void PatchLoadedBundle(IAssetLoadingContext ctx)
        {
            Logger.LogDebug(
                $"Loading asset {ctx.Parameters.Name} from {ctx.GetAssetBundlePath()} (normalized: {ctx.GetNormalizedAssetBundlePath()})");
        }
    }
}