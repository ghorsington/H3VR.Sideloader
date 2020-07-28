using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BepInEx;
using BepInEx.Logging;
using FistVR;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using XUnity.ResourceRedirector;

namespace H3VR.Sideloader
{
    [BepInPlugin("horse.coder.h3vr.sideloader", NAME, VERSION)]
    [BepInDependency("gravydevsupreme.xunity.resourceredirector")]
    public class Sideloader : BaseUnityPlugin
    {
        internal const string VERSION = "1.0.0";
        internal const string NAME = "H3VR Sideloader";
        private const string MODS_DIR = "Mods";

        internal new static ManualLogSource Logger;

        private static readonly string[] TexturePathSchema =
        {
            "prefabPath",
            "materialName",
            "textureName",
            "materialParameter"
        };

        private static readonly string[] MaterialPathSchema =
        {
            "prefabPath",
            "materialName"
        };

        private static readonly string[] MeshPathSchema =
        {
            "prefabPath",
            "meshContainerName",
            "meshName"
        };

        private readonly AssetTree textureAssets = new AssetTree(TexturePathSchema.Length);
        private readonly AssetTree materialAssets = new AssetTree(MaterialPathSchema.Length);
        private readonly AssetTree meshAssets = new AssetTree(MeshPathSchema.Length);
        private readonly Dictionary<string, Mod> prefabReplacements = new Dictionary<string, Mod>();

        private void Awake()
        {
            ZipConstants.DefaultCodePage = Encoding.UTF8.CodePage;
            Logger = base.Logger;
            ResourceRedirection.EnableSyncOverAsyncAssetLoads();
            ResourceRedirection.RegisterAssetLoadedHook(HookBehaviour.OneCallbackPerResourceLoaded, PatchLoadedBundle);
            ResourceRedirection.RegisterAsyncAndSyncAssetLoadingHook(ReplacePrefab);

            LoadMods();
        }

        private void ReplacePrefab(IAssetLoadingContext ctx)
        {
            var path = $"{ctx.GetNormalizedAssetBundlePath()}\\{ctx.Parameters.Name}".ToLowerInvariant();

            if (prefabReplacements.TryGetValue(path, out var mod))
            {
                ctx.Asset = mod.LoadPrefab(path);
                ctx.Complete(skipAllPostfixes: false);
            }
        }

        private void LoadMods()
        {
            Logger.LogInfo("Loading mods...");

            var mods = new List<Mod>();
            var modsPath = Path.Combine(Paths.GameRootPath, MODS_DIR);
            Directory.CreateDirectory(modsPath);
            var modIds = new HashSet<string>(); // TODO: Make more elaborate (check version, etc)

            void LoadMods(IEnumerable<string> paths, Func<string, Mod> loader)
            {
                foreach (var path in paths)
                {
                    try
                    {
                        var mod = loader(path);
                        if (modIds.Contains(mod.Manifest.Guid))
                        {
                            Logger.LogWarning($"Skipping [{mod.Name}] because a mod with same GUID ({mod.Manifest.Guid}) was already loaded (check logs)");
                            continue;
                        }
                        Logger.LogDebug($"Loading {mod.Name}");
                        modIds.Add(mod.Manifest.Guid);
                        mods.Add(mod);
                    }
                    catch (Exception e)
                    {
                        Logger.LogWarning($"Skipping {path} because: ({e.GetType()}) {e.Message}");
                    }
                }
            }
            
            LoadMods(Directory.GetDirectories(modsPath, "*", SearchOption.TopDirectoryOnly), Mod.LoadFromDir);
            LoadMods(Extensions.GetAllFiles(modsPath, "*.h3mod", "*.hotmod"), Mod.LoadFromZip);

            // TODO: Sanity checking etc
            foreach (var mod in mods)
            {
                mod.RegisterTreeAssets(textureAssets, AssetType.Texture);
                mod.RegisterTreeAssets(materialAssets, AssetType.Material);
                mod.RegisterTreeAssets(meshAssets, AssetType.Mesh);
                mod.RegisterPrefabReplacements(prefabReplacements);
            }

            Logger.LogInfo($"Loaded {mods.Count} mods!");
        }

        private void PatchLoadedBundle(AssetLoadedContext ctx)
        {
            foreach (var obj in ctx.Assets)
            {
                var path = ctx.GetUniqueFileSystemAssetPath(obj);
                switch (obj)
                {
                    case ItemSpawnerID itemSpawnerId:
                        ReplaceItemSpawnerIcon(itemSpawnerId, path);
                        break;
                    case GameObject go:
                        ReplaceTexturesMaterials(go, path);
                        ReplaceMeshes(go, path);
                        break;
                }
            }
        }

        private void ReplaceItemSpawnerIcon(ItemSpawnerID itemSpawnerId, string path)
        {
            Logger.LogDebug($"ItemSpawnerID Icon: {string.Join(":", new []{ path, itemSpawnerId.Sprite.name, itemSpawnerId.Sprite.texture.name })}");
            var mod = textureAssets.Find(path, itemSpawnerId.Sprite.name, itemSpawnerId.Sprite.texture.name).FirstOrDefault();
            if (mod == null)
                return;
            var tex = mod.Mod.LoadTexture(mod.FullPath);
            var sprite = Sprite.Create(tex, itemSpawnerId.Sprite.rect, itemSpawnerId.Sprite.pivot,
                itemSpawnerId.Sprite.pixelsPerUnit, 0, SpriteMeshType.Tight, itemSpawnerId.Sprite.border);
            itemSpawnerId.Sprite = sprite;
        }

        private void ReplaceMeshes(GameObject go, string path)
        {
            // TODO: Eventually, might need to handle SkinnedMeshRenderers, but for now it seems H3 doesn't use those for guns
            var meshFilters = go.GetComponentsInChildren<MeshFilter>();
            foreach (var meshFilter in meshFilters)
            {
                var filterName = meshFilter.name;
                var meshName = meshFilter.mesh.name.Replace(" Instance", "");
                Logger.LogDebug($"Mesh: {string.Join(":", new []{ path, filterName, meshName })}");
                var replacement = meshAssets.Find(path, filterName, meshName).FirstOrDefault();
                if (replacement != null)
                    meshFilter.mesh = replacement.Mod.LoadMesh(replacement.FullPath);
            }
        }

        private void ReplaceTexturesMaterials(GameObject go, string path)
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
                    
                    Logger.LogDebug($"Material: {string.Join(":", new []{ path, materialName })}");
                    // Materials come before texture replacements
                    var materialMod = materialAssets.Find(path, materialName).FirstOrDefault();
                    if (materialMod != null)
                        materials[index] = material = materialMod.Mod.LoadMaterial(materialMod.FullPath);

                    // Finally, replace textures
                    if (material.mainTexture == null)
                        continue;
                    var textureName = material.mainTexture.name;
                    Logger.LogDebug($"Texture: {string.Join(":", new []{ path, materialName, textureName })}");
                    var nodes = textureAssets.Find(path, materialName, textureName);
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