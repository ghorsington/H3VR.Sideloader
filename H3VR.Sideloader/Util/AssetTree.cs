using System;
using System.Collections.Generic;
using System.Linq;

namespace H3VR.Sideloader.Util
{
    internal class AssetTree
    {
        public const char PATH_SEPARATOR = ':';
        private readonly int pathParts;

        private readonly PathNode root;

        public AssetTree(int pathParts)
        {
            this.pathParts = pathParts;
            root = new PathNode();
        }

        public void AddMod(string targetPath, string fullPath, Mod mod)
        {
            // Remove any trailing empty setters in order to place the item as high in the tree as possible
            var parts = targetPath.Split(PATH_SEPARATOR).Select(p => p.Trim())
                .Select(p => p.Length == 0 ? null : p)
                .Reverse()
                .SkipWhile(p => p == null)
                .Reverse().ToArray();
            if (parts.Length > pathParts)
                throw new ArgumentException($"Path '{targetPath}' must consist of at most {pathParts} parts!",
                    nameof(targetPath));

            var node = root;
            var currentIndex = 0;
            for (; currentIndex < parts.Length; currentIndex++)
            {
                var p = parts[currentIndex];
                var child = node.Children.FirstOrDefault(n => n.Path == p);

                if (child == null)
                {
                    child = new PathNode {Path = p};
                    node.Children.Add(child);
                }

                node = child;
            }

            var modNode = new ModNode
            {
                Path = parts.Length == pathParts ? parts[pathParts - 1] : null,
                Mod = mod,
                FullPath = fullPath
            };
            node.Children.Add(modNode);
        }

        public ModNode[] Find(params string[] path)
        {
            static void CollectAll(PathNode node, List<ModNode> modNodes)
            {
                if (node is ModNode mn)
                    modNodes.Add(mn);
                else
                    foreach (var nodeChild in node.Children)
                        CollectAll(nodeChild, modNodes);
            }

            ModNode[] Process(int index, PathNode node)
            {
                if (node is ModNode mn)
                    return new[] {mn};

                var part = path[index];
                if (node.Path != null && node.Path != part)
                    return new ModNode[0];

                if (index == path.Length - 1)
                {
                    var result = new List<ModNode>();
                    CollectAll(node, result);
                    return result.ToArray();
                }
                
                foreach (var nodeChild in node.Children.Where(n => n.Path != null))
                {
                    var result = Process(index + 1, nodeChild);
                    if (result.Length != 0)
                        return result;
                }

                var globalNodes = node.Children.FirstOrDefault(n => n.Path == null);
                return globalNodes != null ? Process(index + 1, globalNodes) : new ModNode[0];
            }

            foreach (var rootChild in root.Children.Where(n => !(n is ModNode)))
            {
                var result = Process(0, rootChild);
                if (result.Length != 0)
                    return result;
            }

            return root.Children.Where(n => n is ModNode).Cast<ModNode>().ToArray();
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