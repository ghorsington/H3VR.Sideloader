using System.Collections.Generic;
using System.IO;
using System.Linq;
using H3VR.Sideloader.Shared;

namespace H3VR.Sideloader.Util
{
    internal static class Extensions
    {
        public static string[] GetAllFiles(string dir, params string[] patterns)
        {
            return patterns.SelectMany(p => Directory.GetFiles(dir, p, SearchOption.TopDirectoryOnly)).ToArray();
        }

        
    }
}