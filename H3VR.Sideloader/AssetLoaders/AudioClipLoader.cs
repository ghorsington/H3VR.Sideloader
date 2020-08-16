using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace H3VR.Sideloader.AssetLoaders
{
    internal class AudioClipLoader : ILoader
    {
        public void Initialize(IEnumerable<Mod> mods)
        {
            Harmony.CreateAndPatchAll(typeof(AudioClipLoader));
        }

        [HarmonyPatch(typeof(AudioSource), nameof(AudioSource.Play))]
        [HarmonyPrefix]
        private static void OnAudioSourcePlay(AudioSource __instance)
        {
            Sideloader.Logger.LogInfo($"Playing ${__instance.name} with audio clip: {__instance.clip}");
        }
    }
}