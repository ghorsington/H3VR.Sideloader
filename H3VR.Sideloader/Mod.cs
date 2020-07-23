using System;
using System.IO;
using System.Linq;
using System.Text;
using MicroJson;

namespace H3VR.Sideloader
{
    internal class Mod
    {
        private const string MANIFEST_FILE = "manifest.json";

        public ModManifest Manifest { get; private set; }

        public string ModPath { get; private set; }

        public static Mod LoadDir(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException("The path is not a valid mod directory!");

            var manifestPath = Path.Combine(path, MANIFEST_FILE);
            if (!File.Exists(manifestPath))
                throw new FileNotFoundException("Failed to find manifest.json, the directory is not valid!");

            var manifest = new JsonSerializer().Deserialize<ModManifest>(File.ReadAllText(manifestPath));
            if (!manifest.Verify(out var errors))
                throw new ModLoadException(
                    $"The manifest file is invalid. The following problems were found: {errors.Aggregate(new StringBuilder(), (sb, s) => sb.AppendLine($"* {s}"))}");

            return new Mod
            {
                Manifest = manifest,
                ModPath = path
            };
        }
    }

    internal class ModLoadException : Exception
    {
        public ModLoadException(string msg) : base(msg)
        {
        }
    }
}