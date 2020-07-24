using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BepInEx;
using BepInEx.Logging;
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
        internal const string MODS_DIR = "Mods";

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

        private AssetTree textureAssets = new AssetTree(TexturePathSchema.Length);
        private AssetTree materialAssets = new AssetTree(MaterialPathSchema.Length);

        private void Awake()
        {
            ZipConstants.DefaultCodePage = Encoding.UTF8.CodePage;
            Logger = base.Logger;
            ResourceRedirection.EnableSyncOverAsyncAssetLoads();
            ResourceRedirection.RegisterAssetLoadedHook(HookBehaviour.OneCallbackPerResourceLoaded, PatchLoadedBundle);

            LoadMods();
        }

        private void LoadMods()
        {
            Logger.LogInfo("Loading mods...");

            var mods = new List<Mod>();

            var modsPath = Path.Combine(Paths.GameRootPath, MODS_DIR);

            Directory.CreateDirectory(modsPath);

            foreach (var modDir in Directory.GetDirectories(modsPath))
                try
                {
                    var mod = Mod.LoadFromDir(modDir);
                    mods.Add(mod);
                }
                catch (Exception e)
                {
                    Logger.LogWarning($"Skipping {modDir} because: ({e.GetType()}) {e.Message}");
                }

            foreach (var file in Directory.GetFiles(modsPath, "*.h3mod", SearchOption.TopDirectoryOnly))
                try
                {
                    var mod = Mod.LoadFromZip(file);
                    mods.Add(mod);
                }
                catch (Exception e)
                {
                    Logger.LogWarning($"Skipping {file} because: ({e.GetType()}) {e.Message}");
                }

            // TODO: Sanity checking etc
            foreach (var mod in mods)
            {
                mod.RegisterTreeAssets(textureAssets, AssetType.Texture);
                mod.RegisterTreeAssets(materialAssets, AssetType.Material);
            } 

            Logger.LogInfo($"Loaded {mods.Count} mods!");
        }

        private void PatchLoadedBundle(AssetLoadedContext ctx)
        {
            foreach (var obj in ctx.Assets)
            {
                var path = ctx.GetUniqueFileSystemAssetPath(obj);
                if (!(obj is GameObject go)) continue;
                ReplaceTexturesMaterials(go, path);
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
                    var materialName = material.name;

                    // Materials come before texture replacements
                    var materialMod = materialAssets.Find(path, materialName).FirstOrDefault();
                    if (materialMod != null)
                    {
                        materials[index] = material = materialMod.Mod.LoadMaterial(materialMod.FullPath);
                    }

                    if (material.mainTexture == null)
                        continue;
                    var textureName = material.mainTexture.name;
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