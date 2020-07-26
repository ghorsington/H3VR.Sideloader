using System.ComponentModel;

namespace SkinPacker
{
    partial class TextureEditDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.texParamHelp = new System.Windows.Forms.LinkLabel();
            this.texNameHelp = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.texturePathTextBox = new System.Windows.Forms.TextBox();
            this.selectTextButton = new System.Windows.Forms.Button();
            this.prefabPathHelp = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.prefabPathTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.materialNameTextBox = new System.Windows.Forms.TextBox();
            this.texNameTextBox = new System.Windows.Forms.TextBox();
            this.texParamTextBox = new System.Windows.Forms.TextBox();
            this.materialNameHelp = new System.Windows.Forms.LinkLabel();
            this.label8 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.texParamHelp, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.texNameHelp, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.texturePathTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.selectTextButton, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.prefabPathHelp, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.prefabPathTextBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.materialNameTextBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.texNameTextBox, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.texParamTextBox, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.materialNameHelp, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.label8, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(494, 251);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // texParamHelp
            // 
            this.texParamHelp.AutoSize = true;
            this.texParamHelp.Dock = System.Windows.Forms.DockStyle.Left;
            this.texParamHelp.Location = new System.Drawing.Point(416, 156);
            this.texParamHelp.Name = "texParamHelp";
            this.texParamHelp.Size = new System.Drawing.Size(29, 26);
            this.texParamHelp.TabIndex = 15;
            this.texParamHelp.TabStop = true;
            this.texParamHelp.Text = "Help";
            this.texParamHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // texNameHelp
            // 
            this.texNameHelp.AutoSize = true;
            this.texNameHelp.Dock = System.Windows.Forms.DockStyle.Left;
            this.texNameHelp.Location = new System.Drawing.Point(416, 130);
            this.texNameHelp.Name = "texNameHelp";
            this.texNameHelp.Size = new System.Drawing.Size(29, 26);
            this.texNameHelp.TabIndex = 14;
            this.texNameHelp.TabStop = true;
            this.texNameHelp.Text = "Help";
            this.texNameHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Texture";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // texturePathTextBox
            // 
            this.texturePathTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.texturePathTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.texturePathTextBox.Location = new System.Drawing.Point(109, 3);
            this.texturePathTextBox.Name = "texturePathTextBox";
            this.texturePathTextBox.ReadOnly = true;
            this.texturePathTextBox.Size = new System.Drawing.Size(301, 20);
            this.texturePathTextBox.TabIndex = 1;
            // 
            // selectTextButton
            // 
            this.selectTextButton.AutoSize = true;
            this.selectTextButton.Location = new System.Drawing.Point(416, 3);
            this.selectTextButton.Name = "selectTextButton";
            this.selectTextButton.Size = new System.Drawing.Size(75, 23);
            this.selectTextButton.TabIndex = 2;
            this.selectTextButton.Text = "Select file...";
            this.selectTextButton.UseVisualStyleBackColor = true;
            // 
            // prefabPathHelp
            // 
            this.prefabPathHelp.AutoSize = true;
            this.prefabPathHelp.Dock = System.Windows.Forms.DockStyle.Left;
            this.prefabPathHelp.Location = new System.Drawing.Point(416, 78);
            this.prefabPathHelp.Name = "prefabPathHelp";
            this.prefabPathHelp.Size = new System.Drawing.Size(29, 26);
            this.prefabPathHelp.TabIndex = 3;
            this.prefabPathHelp.TabStop = true;
            this.prefabPathHelp.Text = "Help";
            this.prefabPathHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 3);
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(3, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(488, 2);
            this.label2.TabIndex = 4;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Location = new System.Drawing.Point(41, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 26);
            this.label3.TabIndex = 5;
            this.label3.Text = "Prefab path";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // prefabPathTextBox
            // 
            this.prefabPathTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prefabPathTextBox.Location = new System.Drawing.Point(109, 81);
            this.prefabPathTextBox.Name = "prefabPathTextBox";
            this.prefabPathTextBox.Size = new System.Drawing.Size(301, 20);
            this.prefabPathTextBox.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Location = new System.Drawing.Point(30, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 26);
            this.label4.TabIndex = 7;
            this.label4.Text = "Material name";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Right;
            this.label5.Location = new System.Drawing.Point(31, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 26);
            this.label5.TabIndex = 8;
            this.label5.Text = "Texture name";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Right;
            this.label6.Location = new System.Drawing.Point(48, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 26);
            this.label6.TabIndex = 9;
            this.label6.Text = "TexParam";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // materialNameTextBox
            // 
            this.materialNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialNameTextBox.Location = new System.Drawing.Point(109, 107);
            this.materialNameTextBox.Name = "materialNameTextBox";
            this.materialNameTextBox.Size = new System.Drawing.Size(301, 20);
            this.materialNameTextBox.TabIndex = 10;
            // 
            // texNameTextBox
            // 
            this.texNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.texNameTextBox.Location = new System.Drawing.Point(109, 133);
            this.texNameTextBox.Name = "texNameTextBox";
            this.texNameTextBox.Size = new System.Drawing.Size(301, 20);
            this.texNameTextBox.TabIndex = 11;
            // 
            // texParamTextBox
            // 
            this.texParamTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.texParamTextBox.Location = new System.Drawing.Point(109, 159);
            this.texParamTextBox.Name = "texParamTextBox";
            this.texParamTextBox.Size = new System.Drawing.Size(301, 20);
            this.texParamTextBox.TabIndex = 12;
            // 
            // materialNameHelp
            // 
            this.materialNameHelp.AutoSize = true;
            this.materialNameHelp.Dock = System.Windows.Forms.DockStyle.Left;
            this.materialNameHelp.Location = new System.Drawing.Point(416, 104);
            this.materialNameHelp.Name = "materialNameHelp";
            this.materialNameHelp.Size = new System.Drawing.Size(29, 26);
            this.materialNameHelp.TabIndex = 13;
            this.materialNameHelp.TabStop = true;
            this.materialNameHelp.Text = "Help";
            this.materialNameHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Left;
            this.label8.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label8.Location = new System.Drawing.Point(109, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(277, 39);
            this.label8.TabIndex = 16;
            this.label8.Text = "Specify what values to use in order to find the textures to replace.\r\nAll values " + "are optional, but specify AT LEAST ONE.";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 3);
            this.flowLayoutPanel1.Controls.Add(this.closeButton);
            this.flowLayoutPanel1.Controls.Add(this.saveButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 219);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(488, 29);
            this.flowLayoutPanel1.TabIndex = 17;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(410, 3);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "Discard";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(329, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.linkLabel4, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // linkLabel4
            // 
            this.linkLabel4.Location = new System.Drawing.Point(97, 80);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(100, 20);
            this.linkLabel4.TabIndex = 14;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "linkLabel4";
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 20);
            this.label7.TabIndex = 0;
            this.label7.Text = "Texture";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox6
            // 
            this.textBox6.BackColor = System.Drawing.SystemColors.Window;
            this.textBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox6.Location = new System.Drawing.Point(109, 3);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(1, 20);
            this.textBox6.TabIndex = 1;
            // 
            // TextureEditDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 251);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TextureEditDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit texture mapping";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.LinkLabel linkLabel4;
        private System.Windows.Forms.LinkLabel materialNameHelp;
        private System.Windows.Forms.TextBox materialNameTextBox;
        private System.Windows.Forms.LinkLabel prefabPathHelp;
        private System.Windows.Forms.TextBox prefabPathTextBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button selectTextButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.LinkLabel texNameHelp;
        private System.Windows.Forms.TextBox texNameTextBox;
        private System.Windows.Forms.LinkLabel texParamHelp;
        private System.Windows.Forms.TextBox texParamTextBox;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox texturePathTextBox;

        #endregion
    }
}