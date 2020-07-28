using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using H3VR.Sideloader;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SkinPacker
{
    public partial class MainView : Form
    {
        private const string TITLE = "Skin Packer";
        private readonly BindingSource assetMappings = new BindingSource();

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    OverrideSpecifiedNames = true
                }
            },
            Formatting = Formatting.Indented,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        private readonly JsonSerializer serializer;
        private bool isDirty;
        private bool isLoading;
        private ModManifest manifest;

        public MainView()
        {
            serializer = JsonSerializer.Create(jsonSerializerSettings);
            serializer.Converters.Add(new StringEnumConverter());
            InitializeComponent();
            Text = TITLE;
            InitAssetMappingsView();
            AddToolTips();
            InitializeValidators();

            Load += (sender, args) => ControlsEnabled = false;
        }

        private bool Dirty
        {
            get => isDirty;
            set
            {
                isDirty = value;
                if (isLoading) return;
                Text = isDirty ? $"{TITLE} [UNSAVED CHANGES]" : TITLE;
            }
        }

        private bool ControlsEnabled
        {
            get => guidTextBox.Enabled;
            set
            {
                guidTextBox.Enabled = value;
                nameTextBox.Enabled = value;
                versionTextBox.Enabled = value;
                descTextBox.Enabled = value;
                addMappingButton.Enabled = value;
                assetMappingView.Enabled = value;
                editMappingButton.Enabled = value && assetMappingView.SelectedRows.Count != 0;
                deleteMappingButton.Enabled = value && assetMappingView.SelectedRows.Count != 0;
                saveManifestButton.Enabled = value;
                packModButton.Enabled = value;
                ValidateChildren(ValidationConstraints.Selectable);
            }
        }

        private string BaseDir => projectFolderTextBox.Text;

        private void MarkDirtyOnChange(Control c)
        {
            c.TextChanged += (sender, args) => { Dirty = true; };
        }

        private void InitAssetMappingsView()
        {
            // Initialize data view manually because we want to set the size mode as well
            // TODO: Maybe generate via Reflection because ComponentModel API doesn't provide an autosize attribute?
            assetMappingView.AutoGenerateColumns = false;
            assetMappingView.DataSource = assetMappings;
            assetMappingView.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Asset",
                Name = "AssetType",
                DataPropertyName = "Type",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            assetMappingView.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Target",
                Name = "Target",
                DataPropertyName = "Target",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            assetMappingView.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Path",
                Name = "Path",
                DataPropertyName = "Path",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            assetMappingView.SelectionChanged += (sender, args) =>
            {
                editMappingButton.Enabled = assetMappingView.SelectedRows.Count != 0;
                deleteMappingButton.Enabled = assetMappingView.SelectedRows.Count != 0;
            };

            deleteMappingButton.Click += (sender, args) =>
            {
                assetMappings.RemoveCurrent();
                Dirty = true;
            };
        }

        private void AddToolTips()
        {
            guidHelp.AddTooltip("[REQUIRED] `guid` field",
                "A unique ID assigned to the mod.",
                "The ID can be any unique string with the following limitations:",
                "  * Only allowed characters are lowercase a-z and numbers 0-9",
                "  * Only allowed punctuation are dashes (-), underscores (_) and dots (.)",
                "Reverse domain notation is preferable, for example `h3vr.myusername.modname`");

            nameHelp.AddTooltip("[REQUIRED] `name` field",
                "A human-readable name of the mod.",
                "Keep it short but make sure it's understandable enough by the end-user.");

            versionHelp.AddTooltip("[REQUIRED] `version` field",
                "Version of the mod.",
                "Must be of form <major>.<minor>.<fix> where",
                "  * <major> is an integer that describes major version of the mod (big change or overhaul, initial version)",
                "  * <minor> is an integer that describes minor version of the mod (new features that don't break support for older versions of the game)",
                "  * <fix> is an integer that describes fix version of the mod (small fix to the existing version)",
                "<fix> is optional, in which case it's assumed to be `0`.");

            descHelp.AddTooltip("`description` field",
                "A human-readable description of the mod.",
                "Can contain additional information that doesn't fit the title.");

            skinsHelp.AddTooltip("`assetMappings` field",
                "This table describes what textures belong to the mod and how to load them in-game.",
                "To add a texture, click `Add`, select texture to add and fill additional mapping info.",
                "To edit a texture, select one from the list and click `Edit`.",
                "To delete a texture, select from the list and click `Remove`.");
        }

        private void InitializeValidators()
        {
            var guidPattern = new Regex("^[a-z0-9_.-]+$");
            var versionPattern = new Regex("^\\d+\\.\\d+(\\.\\d+)?$");

            MarkDirtyOnChange(guidTextBox);
            guidTextBox.AddValidator(() =>
            {
                if (string.IsNullOrWhiteSpace(guidTextBox.Text))
                    return "Required field";
                if (!guidPattern.IsMatch(guidTextBox.Text))
                    return "GUID contains invalid characters, check help";
                return string.Empty;
            });

            MarkDirtyOnChange(nameTextBox);
            nameTextBox.AddValidator(
                () => string.IsNullOrWhiteSpace(nameTextBox.Text) ? "Required field" : string.Empty);

            MarkDirtyOnChange(versionTextBox);
            versionTextBox.AddValidator(() =>
            {
                if (string.IsNullOrWhiteSpace(versionTextBox.Text))
                    return "Required field";
                if (!versionPattern.IsMatch(versionTextBox.Text))
                    return "Version must be of form X.X.X, check help for info";
                return string.Empty;
            });

            MarkDirtyOnChange(descTextBox);
        }

        private void selectProjectFolderButton_Click(object sender, EventArgs e)
        {
            var folderSelect = new CommonOpenFileDialog {IsFolderPicker = true};

            var result = folderSelect.ShowDialog();

            if (result == CommonFileDialogResult.Ok && TryLoadManifest(folderSelect.FileName, out var newManifest))
            {
                if (isDirty)
                {
                    var saveResult = MessageBox.Show("There are unsaved changes. Save them?", "Save?",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (saveResult == DialogResult.Yes && !SaveManifest())
                        return;
                }

                manifest = newManifest;
                projectFolderTextBox.Text = folderSelect.FileName;
                LoadManifest();
            }
        }

        private bool SaveManifest()
        {
            if (!ValidateChildren(ValidationConstraints.Selectable))
            {
                MessageBox.Show("The manifest data is not valid! Please fix the errors first.", "Cannot save",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            manifest.ManifestRevision = ModManifest.MANIFEST_REVISION;
            manifest.Guid = guidTextBox.Text;
            manifest.Name = nameTextBox.Text;
            manifest.Version = versionTextBox.Text;
            manifest.Description = descTextBox.Text;
            manifest.AssetMappings = assetMappings.Cast<AssetMapping>().ToArray();
            var manifestPath = Path.Combine(BaseDir, ModManifest.MANIFEST_FILE_NAME);
            using var file = File.CreateText(manifestPath);
            serializer.Serialize(file, manifest);
            Dirty = false;
            return true;
        }

        private void LoadManifest()
        {
            isLoading = true;
            assetMappings.Clear();
            guidTextBox.Text = manifest.Guid;
            nameTextBox.Text = manifest.Name;
            versionTextBox.Text = manifest.Version;
            descTextBox.Text = manifest.Description;

            var hasUnsupportedTypes = false;
            foreach (var manifestAssetMapping in manifest.AssetMappings)
                if (manifestAssetMapping.Type != AssetType.Texture)
                    hasUnsupportedTypes = true;
                else
                    assetMappings.Add(manifestAssetMapping);

            if (hasUnsupportedTypes)
                MessageBox.Show(
                    "This manifest has unsupported asset types. Only textures were loaded. Saving the manifest will remove other asset types. It's suggested to not edit this manifest with this tool.",
                    "Unsupported manifest", MessageBoxButtons.OK, MessageBoxIcon.Information);
            isLoading = false;
            ControlsEnabled = true;
            Dirty = false;
        }

        private bool TryLoadManifest(string path, out ModManifest newManifest)
        {
            var manifestFile = Path.Combine(path, ModManifest.MANIFEST_FILE_NAME);
            if (!File.Exists(manifestFile))
            {
                var result = MessageBox.Show("This folder does not have a manifest file. Create a new one?",
                    "New project", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    newManifest = null;
                    return false;
                }

                newManifest = new ModManifest
                {
                    ManifestRevision = ModManifest.MANIFEST_REVISION,
                    Guid = "",
                    Name = "",
                    Version = "",
                    AssetMappings = new AssetMapping[0]
                };
                return true;
            }

            try
            {
                newManifest =
                    serializer.Deserialize<ModManifest>(
                        new JsonTextReader(new StringReader(File.ReadAllText(manifestFile))));
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    $"Failed to read manifest. Likely it's invalid or the program cannot access it. Error message: {exception.Message}",
                    "Heck", MessageBoxButtons.OK, MessageBoxIcon.Error);
                newManifest = null;
                return false;
            }
        }

        private void saveManifestButton_Click(object sender, EventArgs e)
        {
            SaveManifest();
        }

        private void packModButton_Click(object sender, EventArgs e)
        {
            if (!SaveManifest())
                return;

            var savePathDialog = new CommonSaveFileDialog
            {
                Filters =
                {
                    new CommonFileDialogFilter("H3VR Sideloader Mod", "*.h3mod;*.hotmod"),
                },
                Title = "Save sideloader file",
                DefaultExtension = ".h3mod"
            };
            var result = savePathDialog.ShowDialog();
            if (result != CommonFileDialogResult.Ok)
                return;
            var packer = new ModPacker(manifest, BaseDir, savePathDialog.FileName);
            packer.ShowDialog();
        }

        private void addMappingButton_Click(object sender, EventArgs e)
        {
            var newMapping = new AssetMapping {Type = AssetType.Texture, Path = "", Target = ""};
            var editDialog = new TextureEditDialog(newMapping, BaseDir);
            var result = editDialog.ShowDialog();

            if (result == DialogResult.OK)
                assetMappings.Add(newMapping);
            Dirty = true;
        }

        private void editMappingButton_Click(object sender, EventArgs e)
        {
            var current = assetMappings.Current as AssetMapping;
            var editDialog = new TextureEditDialog(current, BaseDir);
            var result = editDialog.ShowDialog();
            if (result == DialogResult.OK)
                Dirty = true;
        }
    }
}