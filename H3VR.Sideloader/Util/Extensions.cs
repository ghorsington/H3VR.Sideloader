using System.IO;
using System.Linq;

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