#if PLUGIN
using System.IO;
using BepInEx;
using BepInEx.Configuration;

namespace H3VR.Sideloader.Shared
{
    public class Config
    {
        private ConfigFile file;
        
        public ConfigEntry<bool> WriteDebug { get; }
        
        public ConfigEntry<string> ModsFolder { get; }
        
        public Config()
        {
            file = new ConfigFile(Path.Combine(Paths.ConfigPath, "h3vr.sideloader.cfg"), true);
            WriteDebug = file.Bind("Debug", "Enabled", false, "Whether to output internal debug messages");
            ModsFolder = file.Bind("General", "ModsPath", "../Mods",
                "Folder to load mods from in relation to BepInEx folder");
        }
    }
}
#endif