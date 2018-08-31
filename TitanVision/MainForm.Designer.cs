﻿namespace TitanVision
{
	partial class MainForm
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.cmbMessage = new System.Windows.Forms.ComboBox();
			this.lblPreviewFont = new System.Windows.Forms.Label();
			this.cmbFont = new System.Windows.Forms.ComboBox();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.characterOverridesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fontPathsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.tspbProgress = new System.Windows.Forms.ToolStripProgressBar();
			this.enableCharacterOverridesToolStripMenuItem = new TitanVision.Controls.BindableToolStripMenuItem();
			this.tvTextFiles = new TitanVision.Controls.TreeViewEx();
			this.textEditorControl = new TitanVision.Controls.TextEditorControl();
			this.menuStrip.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmbMessage
			// 
			this.cmbMessage.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.cmbMessage.DropDownHeight = 250;
			this.cmbMessage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbMessage.DropDownWidth = 400;
			this.cmbMessage.Enabled = false;
			this.cmbMessage.FormattingEnabled = true;
			this.cmbMessage.IntegralHeight = false;
			this.cmbMessage.Location = new System.Drawing.Point(252, 27);
			this.cmbMessage.Name = "cmbMessage";
			this.cmbMessage.Size = new System.Drawing.Size(300, 21);
			this.cmbMessage.TabIndex = 0;
			// 
			// lblPreviewFont
			// 
			this.lblPreviewFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblPreviewFont.AutoSize = true;
			this.lblPreviewFont.Enabled = false;
			this.lblPreviewFont.Location = new System.Drawing.Point(940, 30);
			this.lblPreviewFont.Name = "lblPreviewFont";
			this.lblPreviewFont.Size = new System.Drawing.Size(76, 13);
			this.lblPreviewFont.TabIndex = 3;
			this.lblPreviewFont.Text = "Selected Font:";
			// 
			// cmbFont
			// 
			this.cmbFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbFont.Enabled = false;
			this.cmbFont.FormattingEnabled = true;
			this.cmbFont.Location = new System.Drawing.Point(1022, 27);
			this.cmbFont.Name = "cmbFont";
			this.cmbFont.Size = new System.Drawing.Size(150, 21);
			this.cmbFont.TabIndex = 4;
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(1184, 24);
			this.menuStrip.TabIndex = 5;
			this.menuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDirectoryToolStripMenuItem,
            this.saveDirectoryToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openDirectoryToolStripMenuItem
			// 
			this.openDirectoryToolStripMenuItem.Name = "openDirectoryToolStripMenuItem";
			this.openDirectoryToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.openDirectoryToolStripMenuItem.Text = "&Open Directory...";
			this.openDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openDirectoryToolStripMenuItem_Click);
			// 
			// saveDirectoryToolStripMenuItem
			// 
			this.saveDirectoryToolStripMenuItem.Enabled = false;
			this.saveDirectoryToolStripMenuItem.Name = "saveDirectoryToolStripMenuItem";
			this.saveDirectoryToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.saveDirectoryToolStripMenuItem.Text = "&Save Directory";
			this.saveDirectoryToolStripMenuItem.Click += new System.EventHandler(this.saveDirectoryToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(160, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableCharacterOverridesToolStripMenuItem,
            this.toolStripMenuItem1,
            this.characterOverridesToolStripMenuItem,
            this.fontPathsToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.toolsToolStripMenuItem.Text = "&Tools";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(213, 6);
			// 
			// characterOverridesToolStripMenuItem
			// 
			this.characterOverridesToolStripMenuItem.Name = "characterOverridesToolStripMenuItem";
			this.characterOverridesToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.characterOverridesToolStripMenuItem.Text = "&Character Overrides...";
			this.characterOverridesToolStripMenuItem.Click += new System.EventHandler(this.characterOverridesToolStripMenuItem_Click);
			// 
			// fontPathsToolStripMenuItem
			// 
			this.fontPathsToolStripMenuItem.Name = "fontPathsToolStripMenuItem";
			this.fontPathsToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.fontPathsToolStripMenuItem.Text = "&Font Paths...";
			this.fontPathsToolStripMenuItem.Click += new System.EventHandler(this.fontPathsToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
			this.aboutToolStripMenuItem.Text = "&About...";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslStatus,
            this.tspbProgress});
			this.statusStrip.Location = new System.Drawing.Point(0, 655);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(1184, 22);
			this.statusStrip.TabIndex = 6;
			this.statusStrip.Text = "statusStrip1";
			// 
			// tsslStatus
			// 
			this.tsslStatus.Name = "tsslStatus";
			this.tsslStatus.Size = new System.Drawing.Size(1169, 17);
			this.tsslStatus.Spring = true;
			this.tsslStatus.Text = "---";
			this.tsslStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tspbProgress
			// 
			this.tspbProgress.Name = "tspbProgress";
			this.tspbProgress.Size = new System.Drawing.Size(200, 16);
			this.tspbProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.tspbProgress.Visible = false;
			// 
			// enableCharacterOverridesToolStripMenuItem
			// 
			this.enableCharacterOverridesToolStripMenuItem.CheckOnClick = true;
			this.enableCharacterOverridesToolStripMenuItem.Name = "enableCharacterOverridesToolStripMenuItem";
			this.enableCharacterOverridesToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.enableCharacterOverridesToolStripMenuItem.Text = "Enable Character &Overrides";
			// 
			// tvTextFiles
			// 
			this.tvTextFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.tvTextFiles.Enabled = false;
			this.tvTextFiles.HideSelection = false;
			this.tvTextFiles.Location = new System.Drawing.Point(12, 27);
			this.tvTextFiles.Name = "tvTextFiles";
			this.tvTextFiles.Size = new System.Drawing.Size(234, 625);
			this.tvTextFiles.TabIndex = 2;
			// 
			// textEditorControl
			// 
			this.textEditorControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textEditorControl.Enabled = false;
			this.textEditorControl.GameRenderer = null;
			this.textEditorControl.Location = new System.Drawing.Point(252, 54);
			this.textEditorControl.Name = "textEditorControl";
			this.textEditorControl.OverridesEnabled = false;
			this.textEditorControl.Size = new System.Drawing.Size(920, 598);
			this.textEditorControl.TabIndex = 1;
			this.textEditorControl.TranslatableEntry = null;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1184, 677);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.menuStrip);
			this.Controls.Add(this.cmbFont);
			this.Controls.Add(this.lblPreviewFont);
			this.Controls.Add(this.tvTextFiles);
			this.Controls.Add(this.cmbMessage);
			this.Controls.Add(this.textEditorControl);
			this.DoubleBuffered = true;
			this.MainMenuStrip = this.menuStrip;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private Controls.TextEditorControl textEditorControl;
		private System.Windows.Forms.ComboBox cmbMessage;
		private Controls.TreeViewEx tvTextFiles;
		private System.Windows.Forms.Label lblPreviewFont;
		private System.Windows.Forms.ComboBox cmbFont;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openDirectoryToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveDirectoryToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private Controls.BindableToolStripMenuItem enableCharacterOverridesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem characterOverridesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fontPathsToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel tsslStatus;
		private System.Windows.Forms.ToolStripProgressBar tspbProgress;
	}
}

