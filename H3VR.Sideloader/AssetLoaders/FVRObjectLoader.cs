using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using Anvil;
using FistVR;
using H3VR.Sideloader.Shared;
using HarmonyLib;
using UnityEngine;

namespace H3VR.Sideloader.AssetLoaders
{
    internal class FVRObjectLoader : ILoader
    {
        private static readonly List<FVRObject> Objects = new List<FVRObject>();
        private static readonly List<ItemSpawnerID> ItemSpawnerIds = new List<ItemSpawnerID>();

        private static readonly AccessTools.FieldRef<FVRObject, AssetID> AssetIdRef =
            AccessTools.FieldRefAccess<FVRObject, AssetID>("m_anvilPrefab");

        private static readonly Dictionary<string, AssetBundleAsset> AbAssets =
            new Dictionary<string, AssetBundleAsset>();

        public void Initialize(IEnumerable<Mod> mods)
        {
            static bool IsNewAsset(AssetMapping mapping)
            {
                return mapping.Type == AssetType.FVRObject || mapping.Type == AssetType.ItemSpawnerID;
            }

            var validMods = mods.Where(m => m.Manifest.AssetMappings.Any(IsNewAsset));

            foreach (var validMod in validMods)
            foreach (var assetMapping in validMod.Manifest.AssetMappings.Where(IsNewAsset))
            {
                AbAssets[$"{validMod.Manifest.Guid}#{assetMapping.Path}"] = new AssetBundleAsset
                {
                    Path = assetMapping.Path,
                    Mod = validMod
                };
                switch (assetMapping.Type)
                {
                    case AssetType.FVRObject:
                    {
                        var obj = validMod.LoadAssetBundleAsset<FVRObject>(assetMapping.Target);
                        ref var assetId = ref AssetIdRef(obj);
                        assetId.Bundle = validMod.Manifest.Guid;
                        Objects.Add(obj);
                    }
                        break;
                    case AssetType.ItemSpawnerID:
                    {
                        var obj = validMod.LoadAssetBundleAsset<ItemSpawnerID>(assetMapping.Target);
                        ItemSpawnerIds.Add(obj);
                    }
                        break;
                }
            }

            Harmony.CreateAndPatchAll(typeof(FVRObjectLoader));
        }

        [HarmonyPatch(typeof(GM), "Awake")]
        [HarmonyPrefix]
        private static void InjectObjects()
        {
            foreach (var itemSpawnerId in ItemSpawnerIds)
            {
                IM.CD[itemSpawnerId.Category].Add(itemSpawnerId);
                IM.SCD[itemSpawnerId.SubCategory].Add(itemSpawnerId);
            }

            foreach (var fvrObject in Objects)
                IM.OD.Add(fvrObject.ItemID, fvrObject);
        }

        [HarmonyPatch(typeof(AnvilManager), "GetAssetBundleAsyncInternal")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> GetAssetBundleAsyncInternalTranspiler(
            IEnumerable<CodeInstruction> instr)
        {
            return new CodeMatcher(instr)
                .MatchForward(false,
                    new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(File), nameof(File.Exists))))
                .SetAndAdvance(OpCodes.Call,
                    Transpilers.EmitDelegate((Func<string, bool>) (s => AbAssets.ContainsKey(s) || File.Exists(s))))
                .MatchForward(false,
                    new CodeMatch(OpCodes.Call,
                        AccessTools.Method(typeof(AssetBundle), nameof(AssetBundle.LoadFromFileAsync),
                            new[] {typeof(string)})))
                .SetAndAdvance(OpCodes.Call, Transpilers.EmitDelegate((Func<string, AsyncOperation>) (s =>
                {
                    if (AbAssets.TryGetValue(s, out var abAsset))
                        return new AnvilDummyOperation(abAsset.Mod.LoadAssetBundle(abAsset.Path, out _));
                    return AssetBundle.LoadFromFileAsync(s);
                })))
                .InstructionEnumeration();
        }

        private class AssetBundleAsset
        {
            public Mod Mod;
            public string Path;
        }
    }
}