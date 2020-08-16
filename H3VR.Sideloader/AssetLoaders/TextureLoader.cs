using System.Collections.Generic;
using System.Linq;
using FistVR;
using H3VR.Sideloader.Shared;
using UnityEngine;
using XUnity.ResourceRedirector;

namespace H3VR.Sideloader.AssetLoaders
{
    internal class TextureLoader : AssetTreeLoaderBase
    {
        private static readonly string[] TexturePathSchema =
        {
            "prefabPath",
            "materialName",
            "textureName",
            "materialParameter"
        };

        public override int Priority { get; } = 5;
        protected override AssetType AssetType { get; } = AssetType.Texture;
        protected override int TargetPathLength { get; } = TexturePathSchema.Length;

        public override void Initialize(IEnumerable<Mod> mods)
        {
            base.Initialize(mods);
            ResourceRedirection.RegisterAssetLoadedHook(HookBehaviour.OneCallbackPerResourceLoaded, PatchLoadedAsset);
            ResourceRedirection.RegisterResourceLoadedHook(HookBehaviour.OneCallbackPerResourceLoaded, PatchLoadedResource);
        }

        private void PatchLoadedResource(ResourceLoadedContext ctx)
        {
            Sideloader.Logger.LogDebug($"Loaded resource to load resource {ctx.Parameters.Path}, {ctx.Parameters.Type}");
            foreach (var obj in ctx.Assets)
            {
                var path = ctx.GetUniqueFileSystemAssetPath(obj);
                if (!(obj is ItemSpawnerID itemSpawnerId)) continue;
                ReplaceItemSpawnerIcon(itemSpawnerId, path);
            }
        }
        
        private void ReplaceItemSpawnerIcon(ItemSpawnerID itemSpawnerId, string path)
        {
            Sideloader.Logger.LogDebug(
                $"ItemSpawnerID Icon: {string.Join(":", new[] {path, itemSpawnerId.Sprite.name, itemSpawnerId.Sprite.texture.name})}");
            var mod = AssetTree.Find(path, itemSpawnerId.Sprite.name, itemSpawnerId.Sprite.texture.name).FirstOrDefault();
            if (mod == null)
                return;
            var tex = mod.Mod.LoadTexture(mod.FullPath);
            var sprite = Sprite.Create(tex, itemSpawnerId.Sprite.rect, itemSpawnerId.Sprite.pivot,
                itemSpawnerId.Sprite.pixelsPerUnit, 0, SpriteMeshType.Tight, itemSpawnerId.Sprite.border);
            itemSpawnerId.Sprite = sprite;
        }

        private void PatchLoadedAsset(AssetLoadedContext ctx)
        {
            foreach (var obj in ctx.Assets)
            {
                var path = ctx.GetUniqueFileSystemAssetPath(obj);
                if (!(obj is GameObject go)) continue;
                ReplaceTextures(go, path);
            }
        }
        
        private void ReplaceTextures(GameObject go, string path)
        {
            var meshRenderers = go.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
                var materials = meshRenderer.materials;
                if (materials == null)
                    continue;
                foreach (var material in materials)
                {
                    var materialName = material.name.Replace(" (Instance)", "");

                    if (material.mainTexture == null)
                        continue;
                    var textureName = material.mainTexture.name;
                    Sideloader.Logger.LogDebug($"Texture: {string.Join(":", new[] {path, materialName, textureName})}");
                    var nodes = AssetTree.Find(path, materialName, textureName);
                    if (nodes.Length == 0)
                        continue;
                    // TODO: Remove duplicates to prevent duplicate loading
                    foreach (var modNode in nodes)
                    {
                        var tex = modNode.Mod.LoadTexture(modNode.FullPath);
                        if (modNode.Path == null)
                            material.mainTexture = tex;
                        else
                            material.SetTexture(modNode.Path, tex);
                    }
                }

                meshRenderer.materials = materials;
            }
        }
    }
}