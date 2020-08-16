using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using H3VR.Sideloader.Shared;
using ICSharpCode.SharpZipLib.Zip;
using MicroJson;
using UnityEngine;
using Object = UnityEngine.Object;

namespace H3VR.Sideloader
{
    internal class Mod
    {
        private readonly Dictionary<string, AssetBundle> assetBundles = new Dictionary<string, AssetBundle>();
        private readonly Dictionary<string, Material> materials = new Dictionary<string, Material>();
        private readonly Dictionary<string, Mesh> meshes = new Dictionary<string, Mesh>();

        private readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        public ModManifest Manifest { get; private set; }
        public string ModPath { get; private set; }
        private ZipFile Archive { get; set; }
        public string Name => $"{Manifest.Name} {Manifest.Version}";

        public static Mod LoadFromDir(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException("The path is not a valid mod directory!");

            var manifestPath = Path.Combine(path, ModManifest.MANIFEST_FILE_NAME);
            if (!File.Exists(manifestPath))
                throw new FileNotFoundException(
                    $"Failed to find {ModManifest.MANIFEST_FILE_NAME}, the directory is not valid!");

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

            var manifestEntry = file.GetEntry(ModManifest.MANIFEST_FILE_NAME);

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

        public GameObject LoadPrefab(string target)
        {
            // Specifically don't cache prefabs because they are usually loaded only once per scene anyway
            if (FileExists(GetAssetPath(target, out _)))
                return LoadAssetBundleAsset(target, new Dictionary<string, GameObject>());
            Sideloader.Logger.LogWarning($"[{Name}] no prefab defined at `{target}`");
            return null;
        }

        public Texture2D LoadTexture(string path)
        {
            if (!FileExists(path))
                throw new FileNotFoundException($"Tried to load non-existent texture `{path}` from mod {Name}");

            if (textures.TryGetValue(path, out var tex)) return tex;

            tex = textures[path] = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            tex.LoadImage(LoadBytes(path));
            return tex;
        }

        public Material LoadMaterial(string path)
        {
            return LoadAssetBundleAsset(path, materials);
        }

        public Mesh LoadMesh(string path)
        {
            return LoadAssetBundleAsset(path, meshes);
        }

        private T LoadAssetBundleAsset<T>(string path, IDictionary<string, T> assetCache) where T : Object
        {
            Sideloader.Logger.LogDebug($"Loading asset from {path}");
            if (assetCache.TryGetValue(path, out var asset))
                return asset;
            var filePath = GetAssetPath(path, out var assetPath);
            if (!assetBundles.TryGetValue(filePath, out var assetBundle))
                assetBundle = assetBundles[filePath] = AssetBundle.LoadFromMemory(LoadBytes(filePath));
            asset = assetCache[path] = assetBundle.LoadAsset<T>(assetPath);
            return asset;
        }

        public string GetAssetPath(string path, out string assetPath)
        {
            assetPath = null;
            var parts = path.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return null;
            assetPath = parts.Length >= 2 ? parts[parts.Length - 1] : null;
            return parts[0];
        }

        private byte[] LoadBytes(string path)
        {
            if (!FileExists(path))
                throw new FileNotFoundException($"`{path}` does not exist in {Name}");
            var entry = Archive?.GetEntry(path);
            using var stream = Archive != null
                ? Archive.GetInputStream(entry)
                : File.OpenRead(Path.Combine(ModPath, path));
            var result = new byte[entry?.Size ?? stream.Length];
            stream.Read(result, 0, result.Length);
            return result;
        }

        public bool FileExists(string path)
        {
            if (Archive != null)
                return Archive.GetEntry(path) != null;
            return File.Exists(Path.Combine(ModPath, path));
        }
    }

    internal class ModLoadException : Exception
    {
        public ModLoadException(string msg) : base(msg)
        {
        }
    }

    internal static class ModExtensions
    {
        public static bool Verify(this ModManifest manifest, out string[] errors)
        {
            var errs = new List<string>();

            void Missing(string field)
            {
                errs.Add($"Missing required property `{field}`.");
            }

            if (manifest.ManifestRevision == null)
                Missing(nameof(manifest.ManifestRevision));
            if (manifest.ManifestRevision != ModManifest.MANIFEST_REVISION)
                errs.Add($"Invalid manifest revision. Supported values are: `{ModManifest.MANIFEST_REVISION}`.");
            if (manifest.Guid == null)
                Missing(nameof(manifest.Guid));
            if (manifest.Name == null)
                Missing(nameof(manifest.Name));
            if (manifest.Version == null)
                errs.Add("Missing or invalid `version`. Version must be of form `X.X.X`.");

            foreach (var assetMapping in manifest.AssetMappings)
            {
                if (assetMapping.Path == null)
                    Missing(nameof(assetMapping.Path));
                if (assetMapping.Target == null)
                    Missing(nameof(assetMapping.Path));
            }

            errors = errs.ToArray();
            return errors.Length == 0;
        }
    }
}