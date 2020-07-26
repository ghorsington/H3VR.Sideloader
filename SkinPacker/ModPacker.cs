using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using H3VR.Sideloader;
using ICSharpCode.SharpZipLib.Zip;

namespace SkinPacker
{
    internal partial class ModPacker : Form
    {
        private readonly string baseDir;
        private readonly ModManifest manifest;
        private readonly string targetPath;

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
            ThreadPool.QueueUserWorkItem(state =>
            {
                void Advance(string s)
                {
                    Invoke((Action) (() => AdvanceStep(s)));
                }

                void CloseWnd()
                {
                    Invoke((Action) Close);
                }

                var files = manifest.AssetMappings.Select(m => m.Path).Distinct().ToList();
                Invoke((Action) (() =>
                {
                    progressBar.Maximum = files.Count + 3;
                    progressLabel.Text = "Preparing";
                }));
                
                foreach (var file in files)
                {
                    var fullFile = Path.Combine(baseDir, file);
                    if (File.Exists(fullFile)) continue;
                    MessageBox.Show($"Failed to find file {fullFile}, cannot pack!", "Heck", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    CloseWnd();
                    return;
                }

                var zipName = Path.GetFileName(targetPath);
                Advance($"Packing into {zipName}");
                using (var zipStream = new ZipOutputStream(File.Create(targetPath)))
                {
                    foreach (var file in files)
                    {
                        Advance($"Packing {file}");
                        zipStream.PutNextEntry(new ZipEntry(file.Replace("\\", "/")));
                        using var fileStream = File.OpenRead(Path.Combine(baseDir, file));
                        fileStream.CopyTo(zipStream);
                    }

                    Advance("Packing manifest file");

                    zipStream.PutNextEntry(new ZipEntry(ModManifest.MANIFEST_FILE_NAME));
                    using var manifestStream = File.OpenRead(Path.Combine(baseDir, ModManifest.MANIFEST_FILE_NAME));
                    manifestStream.CopyTo(zipStream);
                }

                Advance("Done");

                Invoke((Action) (() =>
                {
                    closButton.Enabled = true;
                    openFolderButton.Enabled = true;
                }));
            });
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

        private void openFolderButton_Click(object sender, EventArgs e)
        {
            var folderPath = $"{Path.GetDirectoryName(targetPath).Trim('\\', '/')}\\";
            Process.Start(folderPath);
        }
    }
}