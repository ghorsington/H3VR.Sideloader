using System;
using System.Collections.Generic;
using System.Linq;

namespace H3VR.Sideloader
{
    internal enum AssetType
    {
        Texture,
        Prefab,
        Mesh,
        Material
    }
    
    internal class AssetMapping
    {
        public AssetType type;
        public string gamePath;
        public string modPath;
    }
    
    internal class ModManifest
    {
        public const string MANIFEST_REVISION = "1";
        
        public string manifestRevision;
        public string guid;
        public string name;
        public string version;
        public AssetMapping[] assetsMappings;
        
        public Version Version => version != null ? new Version(version) : new Version(0, 0);

        public bool Verify(out string[] errors)
        {
            var errs = new List<string>();

            void Missing(string field) => errs.Add($"Missing required property `{guid}`.");
            
            if (manifestRevision == null)
                Missing(nameof(manifestRevision));
            if (manifestRevision != MANIFEST_REVISION)
                errs.Add($"Invalid manifest revision. Supported values are: `{MANIFEST_REVISION}`.");
            if (guid == null)
                Missing(nameof(guid));
            if (name == null)
                Missing(nameof(name));
            try
            {
                var ver = new Version(version);
            }
            catch
            {
                errs.Add("Failed to parse version. Version must be of form `X.X.X.X`.");
            }

            errors = errs.ToArray();
            return errors.Length != 0;
        }
    }
}