using System;
using System.Windows.Forms;
using H3VR.Sideloader;

namespace SkinPacker
{
    internal partial class ModPacker : Form
    {
        private ModManifest manifest;
        private string baseDir;
        private string targetPath;
        
        public ModPacker(ModManifest manifest, string baseDir, string targetPath)
        {
            this.manifest = manifest;
            this.baseDir = baseDir;
            this.targetPath = targetPath;
            
            InitializeComponent();
            
            Shown += StartWork;
        }

        private void StartWork(object sender, EventArgs e)
        {
            
        }

        private void closButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}