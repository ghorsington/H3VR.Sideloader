using System;
using System.Collections.Generic;
using System.Linq;
using H3VR.Sideloader.Shared;
using HarmonyLib;
using UnityEngine;

namespace H3VR.Sideloader.AssetLoaders
{
    internal class AudioClipLoader : ILoader
    {
        private static readonly Dictionary<string, ModEntry> audioClips =
            new(StringComparer.InvariantCultureIgnoreCase);

        public void Initialize(IEnumerable<Mod> mods)
        {
            foreach (var mod in mods)
            foreach (var asset in mod.Manifest.AssetMappings.Where(a => a.Type == AssetType.AudioClip))
            {
                if (audioClips.TryGetValue(asset.Target, out var other))
                {
                    Sideloader.Logger.LogWarning(
                        $"[{mod.Name}] clip {asset.Target} is already replaced by [{other.Mod.Name}], skipping...");
                    continue;
                }

                audioClips[asset.Target] = new ModEntry
                {
                    Mod = mod,
                    AudioClipPath = asset.Path
                };
            }

            Harmony.CreateAndPatchAll(typeof(AudioClipLoader));
        }

        [HarmonyPatch(typeof(AudioSource), nameof(AudioSource.Play), new Type[0])]
        [HarmonyPrefix]
        private static void OnAudioSourcePlay(AudioSource __instance)
        {
            if (!__instance.clip)
                return;
            Sideloader.LogDebug($"AudioClip: {__instance.clip}");
            if (!audioClips.TryGetValue(__instance.clip.name, out var entry))
                return;
            try
            {
                var newClip = entry.Mod.LoadAudioClip(entry.AudioClipPath, __instance.clip.name);
                __instance.clip = newClip;
            }
            catch (Exception e)
            {
                Sideloader.Logger.LogWarning(
                    $"Failed to load AudioClip {__instance.clip} from mod [{entry.Mod.Name}]: {e.Message}");
            }
        }

        private class ModEntry
        {
            public Mod Mod { get; set; }
            public string AudioClipPath { get; set; }
        }
    }
}