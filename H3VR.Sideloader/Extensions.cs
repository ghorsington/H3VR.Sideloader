using System.Collections.Generic;

namespace H3VR.Sideloader
{
    internal static class Extensions
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