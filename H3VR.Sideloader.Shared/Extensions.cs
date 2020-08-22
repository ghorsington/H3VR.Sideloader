using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace H3VR.Sideloader.Shared
{
    internal static class Extensions
    {
        public static IEnumerable<string> GetAllFiles(string dir, params string[] patterns)
        {
            return patterns.SelectMany(p => Directory.GetFiles(dir, p, SearchOption.TopDirectoryOnly)).ToArray();
        }
    }
}