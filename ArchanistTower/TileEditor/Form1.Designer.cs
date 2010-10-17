namespace TileEditor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.contentPathTextBox = new System.Windows.Forms.TextBox();
            this.browseForContentButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.drawRadioButton = new System.Windows.Forms.RadioButton();
            this.eraseRadioButton = new System.Windows.Forms.RadioButton();
            this.layerListBox = new System.Windows.Forms.ListBox();
            this.addLayerButton = new System.Windows.Forms.Button();
            this.removeLayerButton = new System.Windows.Forms.Button();
            this.textureListBox = new System.Windows.Forms.ListBox();
            this.addTextureButton = new System.Windows.Forms.Button();
            this.removeTextureButton = new System.Windows.Forms.Button();
            this.texturePreviewBox = new System.Windows.Forms.PictureBox();
            this.tileDisplay1 = new TileEditor.TileDisplay();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.texturePreviewBox)).BeginInit();
            this.SuspendLayout();
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(2, 557);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(578, 19);
            this.hScrollBar1.TabIndex = 1;
            this.hScrollBar1.Visible = false;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(579, 29);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(19, 528);
            this.vScrollBar1.TabIndex = 2;
            this.vScrollBar1.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(836, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.openToolStripMenuItem1,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.openToolStripMenuItem.Text = "New Tile Map";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            this.openToolStripMenuItem1.Size = new System.Drawing.Size(137, 22);
            this.openToolStripMenuItem1.Text = "Open ";
            this.openToolStripMenuItem1.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // contentPathTextBox
            // 
            this.contentPathTextBox.Location = new System.Drawing.Point(605, 29);
            this.contentPathTextBox.Name = "contentPathTextBox";
            this.contentPathTextBox.ReadOnly = true;
            this.contentPathTextBox.Size = new System.Drawing.Size(190, 20);
            this.contentPathTextBox.TabIndex = 4;
            // 
            // browseForContentButton
            // 
            this.browseForContentButton.Location = new System.Drawing.Point(801, 27);
            this.browseForContentButton.Name = "browseForContentButton";
            this.browseForContentButton.Size = new System.Drawing.Size(35, 23);
            this.browseForContentButton.TabIndex = 5;
            this.browseForContentButton.Text = "...";
            this.browseForContentButton.UseVisualStyleBackColor = true;
            this.browseForContentButton.Click += new System.EventHandler(this.browseForContentButton_Click);
            // 
            // drawRadioButton
            // 
            this.drawRadioButton.AutoSize = true;
            this.drawRadioButton.Checked = true;
            this.drawRadioButton.Location = new System.Drawing.Point(674, 55);
            this.drawRadioButton.Name = "drawRadioButton";
            this.drawRadioButton.Size = new System.Drawing.Size(50, 17);
            this.drawRadioButton.TabIndex = 6;
            this.drawRadioButton.TabStop = true;
            this.drawRadioButton.Text = "Draw";
            this.drawRadioButton.UseVisualStyleBackColor = true;
            // 
            // eraseRadioButton
            // 
            this.eraseRadioButton.AutoSize = true;
            this.eraseRadioButton.Location = new System.Drawing.Point(674, 78);
            this.eraseRadioButton.Name = "eraseRadioButton";
            this.eraseRadioButton.Size = new System.Drawing.Size(52, 17);
            this.eraseRadioButton.TabIndex = 6;
            this.eraseRadioButton.TabStop = true;
            this.eraseRadioButton.Text = "Erase";
            this.eraseRadioButton.UseVisualStyleBackColor = true;
            // 
            // layerListBox
            // 
            this.layerListBox.FormattingEnabled = true;
            this.layerListBox.Location = new System.Drawing.Point(601, 101);
            this.layerListBox.Name = "layerListBox";
            this.layerListBox.Size = new System.Drawing.Size(235, 95);
            this.layerListBox.TabIndex = 7;
            this.layerListBox.SelectedIndexChanged += new System.EventHandler(this.layerListBox_SelectedIndexChanged);
            // 
            // addLayerButton
            // 
            this.addLayerButton.Location = new System.Drawing.Point(633, 202);
            this.addLayerButton.Name = "addLayerButton";
            this.addLayerButton.Size = new System.Drawing.Size(75, 23);
            this.addLayerButton.TabIndex = 8;
            this.addLayerButton.Text = "Add";
            this.addLayerButton.UseVisualStyleBackColor = true;
            this.addLayerButton.Click += new System.EventHandler(this.addLayerButton_Click);
            // 
            // removeLayerButton
            // 
            this.removeLayerButton.Location = new System.Drawing.Point(728, 202);
            this.removeLayerButton.Name = "removeLayerButton";
            this.removeLayerButton.Size = new System.Drawing.Size(75, 23);
            this.removeLayerButton.TabIndex = 8;
            this.removeLayerButton.Text = "Remove";
            this.removeLayerButton.UseVisualStyleBackColor = true;
            this.removeLayerButton.Click += new System.EventHandler(this.removeLayerButton_Click);
            // 
            // textureListBox
            // 
            this.textureListBox.FormattingEnabled = true;
            this.textureListBox.Location = new System.Drawing.Point(601, 231);
            this.textureListBox.Name = "textureListBox";
            this.textureListBox.Size = new System.Drawing.Size(235, 95);
            this.textureListBox.TabIndex = 7;
            this.textureListBox.SelectedIndexChanged += new System.EventHandler(this.textureListBox_SelectedIndexChanged);
            // 
            // addTextureButton
            // 
            this.addTextureButton.Location = new System.Drawing.Point(634, 333);
            this.addTextureButton.Name = "addTextureButton";
            this.addTextureButton.Size = new System.Drawing.Size(75, 23);
            this.addTextureButton.TabIndex = 8;
            this.addTextureButton.Text = "Add";
            this.addTextureButton.UseVisualStyleBackColor = true;
            this.addTextureButton.Click += new System.EventHandler(this.addTextureButton_Click);
            // 
            // removeTextureButton
            // 
            this.removeTextureButton.Location = new System.Drawing.Point(729, 333);
            this.removeTextureButton.Name = "removeTextureButton";
            this.removeTextureButton.Size = new System.Drawing.Size(75, 23);
            this.removeTextureButton.TabIndex = 8;
            this.removeTextureButton.Text = "Remove";
            this.removeTextureButton.UseVisualStyleBackColor = true;
            this.removeTextureButton.Click += new System.EventHandler(this.removeTextureButton_Click);
            // 
            // texturePreviewBox
            // 
            this.texturePreviewBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.texturePreviewBox.Location = new System.Drawing.Point(617, 371);
            this.texturePreviewBox.Name = "texturePreviewBox";
            this.texturePreviewBox.Size = new System.Drawing.Size(200, 200);
            this.texturePreviewBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.texturePreviewBox.TabIndex = 9;
            this.texturePreviewBox.TabStop = false;
            this.texturePreviewBox.Click += new System.EventHandler(this.texturePreviewBox_Click);
            // 
            // tileDisplay1
            // 
            this.tileDisplay1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.tileDisplay1.Location = new System.Drawing.Point(2, 29);
            this.tileDisplay1.Name = "tileDisplay1";
            this.tileDisplay1.Size = new System.Drawing.Size(597, 549);
            this.tileDisplay1.TabIndex = 0;
            this.tileDisplay1.Text = "tileDisplay1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 579);
            this.Controls.Add(this.texturePreviewBox);
            this.Controls.Add(this.removeTextureButton);
            this.Controls.Add(this.removeLayerButton);
            this.Controls.Add(this.addTextureButton);
            this.Controls.Add(this.addLayerButton);
            this.Controls.Add(this.textureListBox);
            this.Controls.Add(this.layerListBox);
            this.Controls.Add(this.eraseRadioButton);
            this.Controls.Add(this.drawRadioButton);
            this.Controls.Add(this.browseForContentButton);
            this.Controls.Add(this.contentPathTextBox);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.tileDisplay1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.texturePreviewBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TileDisplay tileDisplay1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox contentPathTextBox;
        private System.Windows.Forms.Button browseForContentButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.RadioButton drawRadioButton;
        private System.Windows.Forms.RadioButton eraseRadioButton;
        private System.Windows.Forms.ListBox layerListBox;
        private System.Windows.Forms.Button addLayerButton;
        private System.Windows.Forms.Button removeLayerButton;
        private System.Windows.Forms.ListBox textureListBox;
        private System.Windows.Forms.Button addTextureButton;
        private System.Windows.Forms.Button removeTextureButton;
        private System.Windows.Forms.PictureBox texturePreviewBox;
    }
}

