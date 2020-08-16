using System.Collections.Generic;
using System.Linq;
using H3VR.Sideloader.Shared;
using XUnity.ResourceRedirector;

namespace H3VR.Sideloader.AssetLoaders
{
    internal class PrefabLoader : ILoader
    {
        private readonly Dictionary<string, Mod> prefabReplacements = new Dictionary<string, Mod>();

        public void Initialize(IEnumerable<Mod> mods)
        {
            foreach (var mod in mods)
                RegisterPrefabReplacements(mod);
            ResourceRedirection.RegisterAsyncAndSyncAssetLoadingHook(500, ReplacePrefab);
        }

        private void ReplacePrefab(IAssetLoadingContext ctx)
        {
            var path = $"{ctx.GetNormalizedAssetBundlePath()}\\{ctx.Parameters.Name}".ToLowerInvariant();

            if (!prefabReplacements.TryGetValue(path, out var mod)) return;
            ctx.Asset = mod.LoadPrefab(path);
            ctx.Complete(skipAllPostfixes: false);
        }

        private void RegisterPrefabReplacements(Mod mod)
        {
            foreach (var manifestAssetMapping in mod.Manifest.AssetMappings.Where(m => m.Type == AssetType.Prefab))
            {
                if (!mod.FileExists(mod.GetAssetPath(manifestAssetMapping.Path, out _)))
                    Sideloader.Logger.LogWarning(
                        $"[{mod.Name}] Asset `{manifestAssetMapping.Path}` of type `{AssetType.Prefab}` does not exist in the mod, skipping...");
                if (prefabReplacements.TryGetValue(manifestAssetMapping.Target, out var otherMod))
                {
                    Sideloader.Logger.LogWarning(
                        $"[{mod.Name}] prefab {manifestAssetMapping.Type} is already being replaced by [{otherMod.Name}], skipping setting prefab replacement.");
                    continue;
                }

                prefabReplacements[manifestAssetMapping.Target] = mod;
            }
        }
    }
}