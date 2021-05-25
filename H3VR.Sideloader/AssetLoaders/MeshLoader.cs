using System.Collections.Generic;
using System.Linq;
using H3VR.Sideloader.Shared;
using UnityEngine;
using XUnity.ResourceRedirector;

namespace H3VR.Sideloader.AssetLoaders
{
    internal class MeshLoader : AssetTreeLoaderBase
    {
        private static readonly string[] MeshPathSchema =
        {
            "prefabPath",
            "meshContainerName",
            "meshName"
        };

        protected override AssetType AssetType { get; } = AssetType.Mesh;
        protected override int TargetPathLength { get; } = MeshPathSchema.Length;

        public override void Initialize(IEnumerable<Mod> mods)
        {
            base.Initialize(mods);
            ResourceRedirection.RegisterAssetLoadedHook(HookBehaviour.OneCallbackPerResourceLoaded, PatchLoadedAsset);
        }

        private void PatchLoadedAsset(AssetLoadedContext ctx)
        {
            foreach (var obj in ctx.Assets)
            {
                var path = ctx.GetUniqueFileSystemAssetPath(obj);
                if (!(obj is GameObject go)) continue;
                ReplaceMeshes(go, path);
            }
        }

        private void ReplaceMeshes(GameObject go, string path)
        {
            // TODO: Eventually, might need to handle SkinnedMeshRenderers, but for now it seems H3 doesn't use those for guns
            var meshFilters = go.GetComponentsInChildren<MeshFilter>();
            foreach (var meshFilter in meshFilters)
            {
                var filterName = meshFilter.name;
                var meshName = meshFilter.mesh.name.Replace(" Instance", "");
                Sideloader.LogDebug($"Mesh: {string.Join(":", new[] {path, filterName, meshName})}");
                var replacement = AssetTree.Find(path, filterName, meshName).FirstOrDefault();
                if (replacement != null)
                    meshFilter.mesh = replacement.Mod.LoadMesh(replacement.FullPath);
            }
        }
    }
}