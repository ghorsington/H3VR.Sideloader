namespace SkinPacker.Model
{
    public enum AssetType
    {
        Texture,
        Prefab,
        Mesh,
        Material
    }

    public class AssetMapping
    {
        public AssetType AssetType { get; set; }

        public string Target { get; set; }

        public string Path { get; set; }
    }
}