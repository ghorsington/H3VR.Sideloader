using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using SkinPacker;

namespace H3VR.Sideloader.Shared
{
    internal partial class TextureEditDialog : Form
    {
        private readonly string baseDir;
        private readonly AssetMapping mapping;

        public TextureEditDialog(AssetMapping mapping, string baseDir)
        {
            this.baseDir = baseDir;
            this.mapping = mapping;
            InitializeComponent();
            InitializeValidators();
            AddTooltips();
            FillValues();
        }

        private void InitializeValidators()
        {
            void SetValidator(Control c)
            {
                c.TextChanged += (sender, args) => ValidateValues();
            }

            SetValidator(texturePathTextBox);
            SetValidator(prefabPathTextBox);
            SetValidator(materialNameTextBox);
            SetValidator(texNameTextBox);
            SetValidator(texParamTextBox);
        }

        private void FillValues()
        {
            static string GetOrDefault(string[] parts, int index)
            {
                return index < parts.Length ? parts[index] : string.Empty;
            }

            texturePathTextBox.Text = mapping.Path;
            var parts = mapping.Target.Split(':').Select(t => t.Trim()).ToArray();
            prefabPathTextBox.Text = GetOrDefault(parts, 0);
            materialNameTextBox.Text = GetOrDefault(parts, 1);
            texNameTextBox.Text = GetOrDefault(parts, 2);
            texParamTextBox.Text = GetOrDefault(parts, 3);
            ValidateValues();
        }

        private void ValidateValues()
        {
            var valid = File.Exists(Path.Combine(baseDir, texturePathTextBox.Text))
                        && new[]
                        {
                            prefabPathTextBox.Text,
                            materialNameTextBox.Text,
                            texNameTextBox.Text,
                            texParamTextBox.Text
                        }.Any(s => !string.IsNullOrWhiteSpace(s));
            saveButton.Enabled = valid;
        }

        private void AddTooltips()
        {
            prefabPathHelp.AddTooltip("Prefab path",
                "Full path to the asset bundle and the prefab.",
                "If this is specified, only textures within the prefab will be replaced.",
                "The path must be lower case and start from `h3vr_data` folder.",
                "Example: `h3vr_data\\streamingassets\\assets_resources_objectids_weaponry_smg\\thompsonm1a1` to reference",
                "`ThompsonM1A1` prefab from `assets_resources_objectids_weaponry_smg` asset bundle.");

            materialNameHelp.AddTooltip("Material name",
                "Name of the Material object",
                "If specified, only textures defined in Materials with this name will be replaced.");

            texNameHelp.AddTooltip("BaseColor texture name",
                "Name of the BaseColor texture.",
                "If specified, only textures with this name will be replaced",
                "This is the name of the MAIN (i.e. the 'base color') texture!",
                "To target normal and alloy textures, specify TexParam as well!",
                "Example: `m1a1_BaseColor` to replace texture named `m1a1_BaseColor`.");

            texParamHelp.AddTooltip("Texture param name",
                "Name of the parameter in the material's shader.",
                "If specified, sets this texture as a shader parameter on the material that shows the texture.",
                "This is useful in cases where you want to replace normal maps or bumpmaps.",
                "Use the following values:",
                "  - _BumpMap for normal map textures (ones that have `normal` in the name)",
                "  - _SpecTex for alloy textures (ones that have `alloy` in the name)",
                "This value is the same as it is defined in the shader attached to the material.",
                "If not specified, the value defaults to `_MainTex` which is the shader's main texture.");
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void selectTextButton_Click(object sender, EventArgs e)
        {
            var fileDialog = new CommonOpenFileDialog
            {
                Filters =
                {
                    new CommonFileDialogFilter("PNG file", "*.png")
                },
                Title = "Select texture file"
            };

            var result = fileDialog.ShowDialog();
            if (result != CommonFileDialogResult.Ok)
                return;

            var fileName = fileDialog.FileName;
            var needsCopying = !fileDialog.FileName.StartsWith(baseDir);
            if (needsCopying)
            {
                var copyResult = MessageBox.Show(
                    $"Texture {fileDialog.FileName} will be copied over into project folder. Continue?\nThis will overwrite the previous texture with the same name if it exists.",
                    "Needs copying", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (copyResult != DialogResult.Yes)
                    return;
                fileName = Path.GetFileName(fileDialog.FileName);
                try
                {
                    File.Copy(fileDialog.FileName, Path.Combine(baseDir, fileName), true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to copy texture. Message: {ex.Message}", "Heck", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                fileName = fileName.Substring(baseDir.Length).Trim(PathUtils.PathSeparators);
            }

            texturePathTextBox.Text = fileName;
            ValidateValues();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            mapping.Path = texturePathTextBox.Text;
            mapping.Target = string.Join(":",
                prefabPathTextBox.Text.ToLowerInvariant().Trim(),
                materialNameTextBox.Text.Trim(),
                texNameTextBox.Text.Trim(),
                texParamTextBox.Text.Trim()
            );

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}