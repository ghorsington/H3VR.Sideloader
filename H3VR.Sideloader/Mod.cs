using System;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using MicroJson;

namespace H3VR.Sideloader
{
    internal class Mod
    {
        private const string MANIFEST_FILE = "manifest.json";

        public ModManifest Manifest { get; private set; }

        public string ModPath { get; private set; }

        private ZipFile Archive { get; set; }

        public static Mod LoadFromDir(string path)
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

        public static Mod LoadFromZip(string path)
        {
            var file = new ZipFile(path);

            var manifestEntry = file.GetEntry(MANIFEST_FILE);

            if (manifestEntry == null)
                throw new ModLoadException("The archive is not a valid zipmod.");

            using var manifestStream = file.GetInputStream(manifestEntry);
            using var s = new StreamReader(manifestStream);

            var manifest = new JsonSerializer().Deserialize<ModManifest>(s.ReadToEnd());
            if (!manifest.Verify(out var errors))
                throw new ModLoadException(
                    $"The manifest file is invalid. The following problems were found: {errors.Aggregate(new StringBuilder(), (sb, s) => sb.AppendLine($"* {s}"))}");

            return new Mod
            {
                Manifest = manifest,
                ModPath = path,
                Archive = file
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