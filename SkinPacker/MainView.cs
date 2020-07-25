using System.Windows.Forms;
using SkinPacker.Model;

namespace SkinPacker
{
    public partial class MainView : Form
    {
        public BindingSource assetMappings = new BindingSource();

        public MainView()
        {
            InitializeComponent();
            InitAssetMappingsView();

            Load += (sender, args) => ControlsEnabled = false;
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
    }
}