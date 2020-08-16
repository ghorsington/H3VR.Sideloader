using System.Collections.Generic;
using System.Linq;
using H3VR.Sideloader.Shared;
using UnityEngine;
using XUnity.ResourceRedirector;

namespace H3VR.Sideloader.AssetLoaders
{
    internal class MaterialLoader : AssetTreeLoaderBase
    {
        private static readonly string[] MaterialPathSchema =
        {
            "prefabPath",
            "materialName"
        };

        // Load materials before loading textures
        public override int Priority { get; } = 10;

        protected override AssetType AssetType { get; } = AssetType.Material;
        protected override int TargetPathLength { get; } = MaterialPathSchema.Length;

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
                ReplaceMaterials(go, path);
            }
        }

        private void ReplaceMaterials(GameObject go, string path)
        {
            var meshRenderers = go.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
                var materials = meshRenderer.materials;
                if (materials == null)
                    continue;
                for (var index = 0; index < materials.Length; index++)
                {
                    var material = materials[index];
                    var materialName = material.name.Replace(" (Instance)", "");

                    Sideloader.Logger.LogDebug($"Material: {string.Join(":", new[] {path, materialName})}");
                    var materialMod = AssetTree.Find(path, materialName).FirstOrDefault();
                    if (materialMod != null)
                        materials[index] = materialMod.Mod.LoadMaterial(materialMod.FullPath);
                }

                meshRenderer.materials = materials;
            }
        }
    }
}