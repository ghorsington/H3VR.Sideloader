using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using BepInEx;
using BepInEx.Logging;
using H3VR.Sideloader.AssetLoaders;
using H3VR.Sideloader.Shared;
using ICSharpCode.SharpZipLib.Zip;
using XUnity.ResourceRedirector;

namespace H3VR.Sideloader
{
    [BepInPlugin("horse.coder.h3vr.sideloader", H3VR.Sideloader.Shared.Info.NAME, H3VR.Sideloader.Shared.Info.VERSION)]
    [BepInDependency("gravydevsupreme.xunity.resourceredirector")]
    public class Sideloader : BaseUnityPlugin
    {
        internal new static ManualLogSource Logger;

        private readonly IList<ILoader> loaders = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(ILoader).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => (ILoader) Activator.CreateInstance(t))
            .ToList();

        private void Awake()
        {
            ZipConstants.DefaultCodePage = Encoding.UTF8.CodePage;
            Logger = base.Logger;
            ResourceRedirection.EnableSyncOverAsyncAssetLoads();

            LoadMods();
        }

        private void LoadMods()
        {
            Logger.LogInfo("Loading mods...");
            var mods = new List<Mod>();
            var modsPath = Path.Combine(Paths.GameRootPath, H3VR.Sideloader.Shared.Info.MODS_DIR);
            Directory.CreateDirectory(modsPath);
            var modIds = new HashSet<string>(); // TODO: Make more elaborate (check version, etc)

            void LoadMods(IEnumerable<string> paths, Func<string, Mod> loader)
            {
                foreach (var path in paths)
                    try
                    {
                        var mod = loader(path);
                        if (modIds.Contains(mod.Manifest.Guid))
                        {
                            Logger.LogWarning(
                                $"Skipping [{mod.Name}] because a mod with same GUID ({mod.Manifest.Guid}) was already loaded (check logs)");
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

            LoadMods(Directory.GetDirectories(modsPath, "*", SearchOption.TopDirectoryOnly), Mod.LoadFromDir);
            LoadMods(Extensions.GetAllFiles(modsPath, H3VR.Sideloader.Shared.Info.ModExts.Select(s => $"*.{s}").ToArray()),
                Mod.LoadFromZip);

            // TODO: Sanity checking etc

            foreach (var loader in loaders)
            {
                Logger.LogDebug($"Loading {loader}");
                loader.Initialize(mods);
            }


            Logger.LogInfo($"Loaded {mods.Count} mods!");
        }
    }
}