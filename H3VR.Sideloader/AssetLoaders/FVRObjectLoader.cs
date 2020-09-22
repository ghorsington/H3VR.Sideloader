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
            AccessTools.FieldRefAccess<FVRObject, AssetID>(AccessTools.Field(typeof(FVRObject), "m_anvilPrefab"));

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
                var assetBundleId = $"{validMod.Manifest.Guid}#{assetMapping.Path}";
                AbAssets[assetBundleId] = new AssetBundleAsset
                {
                    Path = assetMapping.Path,
                    Mod = validMod
                };
                switch (assetMapping.Type)
                {
                    case AssetType.FVRObject:
                    {
                        var obj = validMod.LoadAssetBundleAsset<FVRObject>(assetMapping.Path);
                        ref var assetId = ref AssetIdRef(obj);
                        assetId.Bundle = assetBundleId;
                        Objects.Add(obj);
                    }
                        break;
                    case AssetType.ItemSpawnerID:
                    {
                        var obj = validMod.LoadAssetBundleAsset<ItemSpawnerID>(assetMapping.Path);
                        ItemSpawnerIds.Add(obj);
                    }
                        break;
                }
            }

            Harmony.CreateAndPatchAll(typeof(FVRObjectLoader));
        }

        [HarmonyPatch(typeof(IM), "GenerateItemDBs")]
        [HarmonyPostfix]
        private static void InjectObjects(IM __instance, Dictionary<string, ItemSpawnerID> ___SpawnerIDDic)
        {
            var im = __instance;
            foreach (var itemSpawnerId in ItemSpawnerIds)
            {
                IM.CD[itemSpawnerId.Category].Add(itemSpawnerId);
                IM.SCD[itemSpawnerId.SubCategory].Add(itemSpawnerId);
                if (!___SpawnerIDDic.ContainsKey(itemSpawnerId.ItemID))
                    ___SpawnerIDDic[itemSpawnerId.ItemID] = itemSpawnerId;
            }

            foreach (var fvrObject in Objects)
            {
                IM.OD.Add(fvrObject.ItemID, fvrObject);

                im.odicTagCategory.AddOrCreate(fvrObject.Category).Add(fvrObject);
                im.odicTagFirearmEra.AddOrCreate(fvrObject.TagEra).Add(fvrObject);
                im.odicTagFirearmSize.AddOrCreate(fvrObject.TagFirearmSize).Add(fvrObject);
                im.odicTagFirearmAction.AddOrCreate(fvrObject.TagFirearmAction).Add(fvrObject);
                im.odicTagFirearmFiringMode.AddOrCreate(fvrObject.TagFirearmFiringModes.FirstOrDefault()).Add(fvrObject);
                im.odicTagFirearmFeedOption.AddOrCreate(fvrObject.TagFirearmFeedOption.FirstOrDefault()).Add(fvrObject);
                im.odicTagFirearmMount.AddOrCreate(fvrObject.TagFirearmMounts.FirstOrDefault()).Add(fvrObject);
                im.odicTagAttachmentMount.AddOrCreate(fvrObject.TagAttachmentMount).Add(fvrObject);
                im.odicTagAttachmentFeature.AddOrCreate(fvrObject.TagAttachmentFeature).Add(fvrObject);
            }
        }

        [HarmonyPatch(typeof(AnvilManager), "GetAssetBundleAsyncInternal")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> GetAssetBundleAsyncInternalTranspiler(
            IEnumerable<CodeInstruction> instr)
        {
            return new CodeMatcher(instr)
                .MatchForward(false,
                    new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(File), nameof(File.Exists))))
                .SetAndAdvance(OpCodes.Nop, null)
                .Insert(
                    new CodeInstruction(OpCodes.Ldarg_0),
                    Transpilers.EmitDelegate((Func<string, string, bool>) ((path, bundle) =>
                        AbAssets.ContainsKey(bundle) || File.Exists(path))))
                .MatchForward(false,
                    new CodeMatch(OpCodes.Call,
                        AccessTools.Method(typeof(AssetBundle), nameof(AssetBundle.LoadFromFileAsync),
                            new[] {typeof(string)})))
                .SetAndAdvance(OpCodes.Nop, null)
                .Insert(
                    new CodeInstruction(OpCodes.Ldarg_0),
                    Transpilers.EmitDelegate((Func<string, string, AsyncOperation>) ((path, bundle) =>
                    {
                        if (AbAssets.TryGetValue(bundle, out var abAsset))
                            return new AnvilDummyOperation(abAsset.Mod.LoadAssetBundle(abAsset.Path, out _));
                        return AssetBundle.LoadFromFileAsync(path);
                    })))
                .InstructionEnumeration();
        }

        private class AssetBundleAsset
        {
            public Mod Mod;
            public string Path;
        }
    }

    public static class DictionaryExtension
    {
        public static TValue AddOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
            where TValue : new()
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, new TValue());
            return dictionary[key];
        }
    }
}