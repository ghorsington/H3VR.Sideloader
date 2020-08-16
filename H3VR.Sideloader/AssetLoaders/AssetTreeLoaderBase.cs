using System.Collections.Generic;
using System.Linq;
using H3VR.Sideloader.Shared;
using H3VR.Sideloader.Util;

namespace H3VR.Sideloader.AssetLoaders
{
    internal abstract class AssetTreeLoaderBase : ILoader
    {
        protected abstract AssetType AssetType { get; }

        protected abstract int TargetPathLength { get; }

        protected AssetTree AssetTree { get; private set; }
        public abstract int Priority { get; }

        public virtual void Initialize(IEnumerable<Mod> mods)
        {
            AssetTree = new AssetTree(TargetPathLength);
            foreach (var mod in mods) RegisterTreeAssets(mod);
        }

        private void RegisterTreeAssets(Mod mod)
        {
            foreach (var manifestAssetMapping in mod.Manifest.AssetMappings.Where(m => m.Type == AssetType))
            {
                if (!mod.FileExists(mod.GetAssetPath(manifestAssetMapping.Path, out _)))
                {
                    Sideloader.Logger.LogWarning(
                        $"[{mod.Name}] Asset `{manifestAssetMapping.Path}` of type `{AssetType}` does not exist in the mod, skipping...");
                    continue;
                }

                AssetTree.AddMod(manifestAssetMapping.Target, manifestAssetMapping.Path, mod);
            }
        }
    }
}