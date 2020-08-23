using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BepInEx;
using BepInEx.Logging;
using H3VR.Sideloader.Shared;
using ICSharpCode.SharpZipLib.Zip;
using Mono.Cecil;

namespace H3VR.Sideloader.MonoMod
{
    public static class SideloaderMonoModPatcher
    {
        private static readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("Sideloader.MonoMod");

        private static IEnumerable<Stream> assemblyStreams;

        // ReSharper disable once InconsistentNaming
        public static IEnumerable<string> TargetDLLs { get; } = new[] {"Assembly-CSharp.dll"};

        private static IEnumerable<Stream> Init()
        {
            var modsDir = Path.Combine(Paths.GameRootPath, Info.MODS_DIR);
            Directory.CreateDirectory(modsDir);

            static IEnumerable<Stream> LoadMods(IEnumerable<string> entries, Func<string, IEnumerable<Stream>> loader)
            {
                return entries.SelectMany(entry =>
                {
                    try
                    {
                        return loader(entry);
                    }
                    catch (Exception e)
                    {
                        Logger.LogWarning($"Failed to load {entry}: ({e.GetType().Name}) {e.Message}, skipping it");
                        return new Stream[0];
                    }
                });
            }

            IEnumerable<Stream> result = new Stream[0];
            result = result.Concat(LoadMods(Directory.GetDirectories(modsDir, "*", SearchOption.TopDirectoryOnly),
                LoadMonoModPatchesFromDirectory));
            result = result.Concat(LoadMods(
                Extensions.GetAllFiles(modsDir, Shared.Info.ModExts.Select(s => $"*.{s}").ToArray()),
                LoadMonoModPatchesFromZip));
            return result;
        }

        public static void Initialize()
        {
            ZipConstants.DefaultCodePage = Encoding.UTF8.CodePage;
            try
            {
                assemblyStreams = Init();
            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to load mods: ({e.GetType().Name}) {e.Message}");
            }
        }

        private static IEnumerable<Stream> LoadMonoModPatchesFromZip(string zip)
        {
            ZipFile file;
            try
            {
                file = new ZipFile(zip);
            }
            catch (Exception e)
            {
                Logger.LogWarning($"Failed to open {zip}: {e.Message}");
                yield break;
            }

            for (var i = 0; i < file.Count; i++)
            {
                var zipEntry = file[i];
                if (zipEntry.IsDirectory)
                    continue;
                var fileName = Path.GetFileName(zipEntry.Name).ToLowerInvariant();
                if (!fileName.EndsWith(".mm.dll"))
                    continue;
                using var s = file.GetInputStream(zipEntry);
                yield return s;
            }

            file.Close();
        }

        private static IEnumerable<Stream> LoadMonoModPatchesFromDirectory(string dir)
        {
            foreach (var file in Directory.GetFiles(dir, "*.mm.dll", SearchOption.AllDirectories))
            {
                FileStream f = null;
                try
                {
                    f = new FileStream(file, FileMode.Open);
                }
                catch (Exception e)
                {
                    Logger.LogInfo($"Failed to open ${file}: {e.Message}");
                }

                if (f == null)
                    continue;
                yield return f;
                f.Dispose();
            }
        }

        public static void Patch(AssemblyDefinition ass)
        {
            if (assemblyStreams == null)
                return;

            using var modder = new SideloaderMonoModder(ass, Logger);
            modder.Run(assemblyStreams);
        }
    }
}