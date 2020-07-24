using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using MicroJson;
using UnityEngine;
using Object = UnityEngine.Object;

namespace H3VR.Sideloader
{
    internal class Mod
    {
        private const string MANIFEST_FILE = "manifest.json";
        public ModManifest Manifest { get; private set; }
        public string ModPath { get; private set; }
        private ZipFile Archive { get; set; }
        public string Name => $"{Manifest.Name} {Manifest.Version}";

        private readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private readonly Dictionary<string, Material> materials = new Dictionary<string, Material>();
        private readonly Dictionary<string, Mesh> meshes = new Dictionary<string, Mesh>();
        private readonly Dictionary<string, AssetBundle> assetBundles = new Dictionary<string, AssetBundle>();
        private readonly Dictionary<string, string> prefabPaths = new Dictionary<string, string>();

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

        public void RegisterPrefabReplacements(Dictionary<string, Mod> mappings)
        {
            foreach (var manifestAssetMapping in Manifest.AssetMappings.Where(m => m.Type == AssetType.Prefab))
            {
                if (!FileExists(ResolveCombinedPath(manifestAssetMapping.Path, out _)))
                    Sideloader.Logger.LogWarning(
                        $"[{Name}] Asset `{manifestAssetMapping.Path}` of type `{AssetType.Prefab}` does not exist in the mod, skipping...");
                if (mappings.TryGetValue(manifestAssetMapping.Target, out var otherMod))
                {
                    Sideloader.Logger.LogWarning(
                        $"[{Name}] prefab {manifestAssetMapping.Type} is already being replaced by [{otherMod.Name}], skipping setting prefab replacement.");
                    continue;
                }

                prefabPaths[manifestAssetMapping.Target] = manifestAssetMapping.Path;
                mappings[manifestAssetMapping.Target] = this;
            }
        }

        public void RegisterTreeAssets(AssetTree tree, AssetType type)
        {
            foreach (var manifestAssetMapping in Manifest.AssetMappings.Where(m => m.Type == type))
            {
                if (!FileExists(ResolveCombinedPath(manifestAssetMapping.Path, out _)))
                    Sideloader.Logger.LogWarning(
                        $"[{Name}] Asset `{manifestAssetMapping.Path}` of type `{type}` does not exist in the mod, skipping...");
                tree.AddMod(manifestAssetMapping.Target, manifestAssetMapping.Path, this);
                textures[manifestAssetMapping.Path] = null;
            }
        }

        public GameObject LoadPrefab(string target)
        {
            // Specifically don't cache prefabs because they are usually loaded only once per scene anyway
            if (prefabPaths.TryGetValue(target, out var assetPath))
                return LoadAssetBundleAsset(assetPath, new Dictionary<string, GameObject>());
            Sideloader.Logger.LogWarning($"[{Name}] no prefab defined at `{target}`");
            return null;
        }

        public Texture2D LoadTexture(string path)
        {
            if (!textures.TryGetValue(path, out var tex))
                throw new FileNotFoundException($"Tried to load non-existent texture `{path}` from mod {Name}");

            if (tex != null) return tex;

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
            var filePath = ResolveCombinedPath(path, out var assetPath);
            if (!assetBundles.TryGetValue(filePath, out var assetBundle))
                assetBundle = assetBundles[filePath] = AssetBundle.LoadFromMemory(LoadBytes(filePath));
            asset = assetCache[path] = assetBundle.LoadAsset<T>(assetPath);
            return asset;
        }

        private string ResolveCombinedPath(string path, out string assetPath)
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

        private bool FileExists(string path)
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
}