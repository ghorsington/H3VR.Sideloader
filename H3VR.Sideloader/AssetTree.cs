using System;
using System.Collections.Generic;
using System.Linq;

namespace H3VR.Sideloader
{
    internal class AssetTree
    {
        public const char PATH_SEPARATOR = ':';

        private PathNode root;
        private int pathParts;

        public AssetTree(int pathParts)
        {
            this.pathParts = pathParts;
            root = new PathNode();
        }

        public void AddMod(string path, Mod mod)
        {
            // Remove any trailing empty setters in order to place the item as high in the tree as possible
            var parts = path.Split(PATH_SEPARATOR).Select(p => p.Trim())
                .Reverse()
                .SkipWhile(p => p.Length == 0)
                .Reverse().ToArray();
            if (parts.Length > pathParts)
                throw new ArgumentException($"Path '{path}' must consist of at most {pathParts} parts!", nameof(path));

            var node = root;
            var currentIndex = 0;
            for (; currentIndex < parts.Length; currentIndex++)
            {
                var newNode = new PathNode { Path = parts[currentIndex] };
                node.Children.Add(newNode);
                node = newNode;
            }
            
            var modNode = new ModNode
            {
                Path = parts.Length > 0 ? parts[currentIndex] : null,
                Mod = mod,
                FullPath = path
            };
            node.Children.Add(modNode);
        }

        public ModNode Find(params string[] path)
        {
            ModNode Process(int index, PathNode node)
            {
                var part = path[index];
                if (node.Path == part)
                {
                    if (node is ModNode mn)
                        return mn;
                    return (node.Children.FirstOrDefault(childNode => childNode is ModNode) as ModNode);
                }

                if (node.Children.Count == 0 && node is ModNode n)
                    return n;
                if (!(node is ModNode))
                    return null;
                
                foreach (var nodeChild in node.Children)
                {
                    var result = Process(index + 1, nodeChild);
                    if (result != null)
                        return result;
                }

                return null;
            }

            return Process(0, root);
        }

        public class PathNode
        {
            public string Path { get; set; }
            public List<PathNode> Children { get; } = new List<PathNode>();
        }

        public class ModNode : PathNode
        {
            public string FullPath { get; set; }
            public Mod Mod { get; set; }
        }
    }
}