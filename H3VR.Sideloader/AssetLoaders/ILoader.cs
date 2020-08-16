using System.Collections.Generic;

namespace H3VR.Sideloader.AssetLoaders
{
    internal interface ILoader
    {
        int Priority { get; }

        void Initialize(IEnumerable<Mod> mods);
    }
}