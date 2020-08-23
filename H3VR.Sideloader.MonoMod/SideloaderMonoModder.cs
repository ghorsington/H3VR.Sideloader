using System.Collections.Generic;
using System.IO;
using BepInEx.Logging;
using Mono.Cecil;
using MonoMod;

namespace H3VR.Sideloader.MonoMod
{
    internal class SideloaderMonoModder : MonoModder
    {
        private readonly ManualLogSource logger;

        public SideloaderMonoModder(AssemblyDefinition assembly, ManualLogSource logger)
        {
            Module = assembly.MainModule;
            this.logger = logger;
        }

        public override void Log(string text)
        {
            logger.LogMessage(text);
        }

        public override void Log(object value)
        {
            logger.LogMessage(value);
        }

        public override void LogVerbose(object value)
        {
            logger.LogDebug(value);
        }

        public override void LogVerbose(string text)
        {
            logger.LogDebug(text);
        }

        public void Run(IEnumerable<Stream> assemblies)
        {
            Read();

            Log("Scanning mods");

            foreach (var assembly in assemblies)
                ReadMod(assembly);

            MapDependencies();

            Log($"Found {Mods.Count} mods");

            if (Mods.Count == 0)
                return;

            Log("mm.PatchRefs");
            PatchRefs();

            Log("mm.AutoPatch");
            AutoPatch();

            Log("Done");
        }

        public override void Dispose()
        {
            Module = null;
            base.Dispose();
        }
    }
}