using System;
using System.Windows.Forms;
using H3VR.Sideloader;

namespace SkinPacker
{
    internal partial class TextureEditDialog : Form
    {
        private AssetMapping mapping; 
            
        public TextureEditDialog(AssetMapping mapping)
        {
            this.mapping = mapping;
            InitializeComponent();
            AddTooltips();
        }

        private void AddTooltips()
        {
            prefabPathHelp.AddTooltip("Prefab path", 
                "Full path to the asset bundle and the prefab.",
                "If this is specified, only textures within the prefab will be replaced.",
                "The path must be lower case and start from `h3vr_data` folder.",
                "Example: `h3vr_data\\streamingassets\\assets_resources_objectids_weaponry_smg\\thompsonm1a1` to reference `ThompsonM1A1` prefab from `assets_resources_objectids_weaponry_smg` asset bundle.");
            
            materialNameHelp.AddTooltip("Material name",
                "Name of the Material object",
                "If specified, only textures defined in Materials with this name will be replaced.");
            
            texNameHelp.AddTooltip("Texture name",
                "Name of the texture.",
                "If specified, only textures with this name will be replaced",
                "This is what you usually want! The name of the texture is the same as it is defined in the asset bundle.",
                "Example: `m1a1_BaseColor` to replace texture named `m1a1_BaseColor`.");
            
            texParamHelp.AddTooltip("Texture param name", 
                "Name of the parameter in the material's shader.",
                "If specified, sets this texture as a shader parameter on the material that shows the texture.",
                "This is useful in cases where you want to replace normal maps or bumpmaps.",
                "This value is the same as it is defined in the shader attached to the material.",
                "If not specified, the value defaults to `_MainTexture` which is the shader's main texture.");
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}