using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using H3VR.Sideloader;
using ICSharpCode.SharpZipLib.Zip;

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
            var files = manifest.AssetMappings.Select(m => m.Path).Distinct().ToList();
            progressBar.Maximum = files.Count * 2 + 3;
            progressLabel.Text = "Preparing";
            
            bool IsInProjectDir(string file) => !PathUtils.IsFullPath(file) || file.StartsWith(baseDir);
            
            foreach (var file in files)
            {
                AdvanceStep($"Copying {file}");
                var fileName = Path.GetFileName(file);
                if (IsInProjectDir(file)) continue;
                try
                {
                    File.Copy(file, Path.Combine(baseDir, fileName));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to copy asset {file} because: {ex.Message}.", "Heck",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                }
            }

            var zipName = Path.GetFileName(targetPath);
            AdvanceStep($"Packing into {zipName}");
            using (var zipStream = new ZipOutputStream(File.Create(targetPath)))
            {
                foreach (var file in files)
                {
                    var zipFilePath = file.StartsWith(baseDir) ? file.Substring(0, baseDir.Length).Trim('\\', '/') : Path.GetFileName(file);
                    AdvanceStep($"Packing {zipFilePath}");
                    zipStream.PutNextEntry(new ZipEntry(zipFilePath.Replace('\\', '/')));
                    using var fileStream = File.OpenRead(Path.Combine(baseDir, zipFilePath));
                    fileStream.CopyTo(zipStream);
                }
                
                AdvanceStep("Packing manifest file");
                
                zipStream.PutNextEntry(new ZipEntry(ModManifest.MANIFEST_FILE_NAME));
                using var manifestStream = File.OpenRead(Path.Combine(baseDir, ModManifest.MANIFEST_FILE_NAME));
                manifestStream.CopyTo(zipStream);
            }
            
            AdvanceStep("Done");
        }

        private void AdvanceStep(string status)
        {
            progressLabel.Text = $"Progress: {status}";
            progressBar.Value += 1;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}