using System.Collections.Generic;

namespace H3VR.Sideloader.AssetLoaders
{
    internal interface ILoader
    {
        void Initialize(IEnumerable<Mod> mods);
    }
}