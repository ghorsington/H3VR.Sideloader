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

        public static void CopyTo(this Stream from, Stream to)
        {
            var buf = new byte[4096];
            int read;
            while ((read = from.Read(buf, 0, buf.Length)) > 0)
                to.Write(buf, 0, read);
        }
    }
}