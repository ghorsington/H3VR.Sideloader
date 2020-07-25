using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SkinPacker
{
    public partial class MainView : Form
    {
        public BindingSource assetMappings = new BindingSource();

        public MainView()
        {
            InitializeComponent();
            InitAssetMappingsView();
            AddToolTips();
            InitializeValidators();

            Load += (sender, args) => ControlsEnabled = true;
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
                DataPropertyName = "AssetType",
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

            deleteMappingButton.Click += (sender, args) => { assetMappings.RemoveCurrent(); };
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
            
            guidTextBox.AddValidator(() =>
            {
                if (string.IsNullOrWhiteSpace(guidTextBox.Text))
                    return "Required field";
                if (!guidPattern.IsMatch(guidTextBox.Text))
                    return "GUID contains invalid characters, check help";
                return string.Empty;
            });
            
            nameTextBox.AddValidator(() => string.IsNullOrWhiteSpace(nameTextBox.Text) ? "Required field" : string.Empty);
            
            versionTextBox.AddValidator(() =>
            {
                if (string.IsNullOrWhiteSpace(versionTextBox.Text))
                    return "Required field";
                if (!versionPattern.IsMatch(versionTextBox.Text))
                    return "Version must be of form X.X.X, check help for info";
                return string.Empty;
            });
        }
    }
}