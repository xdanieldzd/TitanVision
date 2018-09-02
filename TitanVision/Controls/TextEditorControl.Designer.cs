namespace TitanVision.Controls
{
	partial class TextEditorControl
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

		#region Vom Komponenten-Designer generierter Code

		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.tlpPreviews = new System.Windows.Forms.TableLayoutPanel();
			this.pnlOriginalText = new System.Windows.Forms.Panel();
			this.txtOriginal = new System.Windows.Forms.TextBox();
			this.pnlTranslationText = new System.Windows.Forms.Panel();
			this.txtTranslation = new System.Windows.Forms.TextBox();
			this.txtTranslationNotes = new System.Windows.Forms.TextBox();
			this.chkIsIgnored = new System.Windows.Forms.CheckBox();
			this.pnlOriginalImage = new TitanVision.Controls.PanelEx();
			this.pnlTranslationImage = new TitanVision.Controls.PanelEx();
			this.tlpPreviews.SuspendLayout();
			this.pnlOriginalText.SuspendLayout();
			this.pnlTranslationText.SuspendLayout();
			this.SuspendLayout();
			// 
			// tlpPreviews
			// 
			this.tlpPreviews.ColumnCount = 5;
			this.tlpPreviews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpPreviews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
			this.tlpPreviews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
			this.tlpPreviews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpPreviews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
			this.tlpPreviews.Controls.Add(this.pnlOriginalImage, 0, 2);
			this.tlpPreviews.Controls.Add(this.pnlTranslationImage, 3, 2);
			this.tlpPreviews.Controls.Add(this.pnlOriginalText, 0, 0);
			this.tlpPreviews.Controls.Add(this.pnlTranslationText, 3, 0);
			this.tlpPreviews.Controls.Add(this.txtTranslationNotes, 0, 4);
			this.tlpPreviews.Controls.Add(this.chkIsIgnored, 4, 4);
			this.tlpPreviews.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpPreviews.Location = new System.Drawing.Point(0, 0);
			this.tlpPreviews.Margin = new System.Windows.Forms.Padding(0);
			this.tlpPreviews.Name = "tlpPreviews";
			this.tlpPreviews.RowCount = 5;
			this.tlpPreviews.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
			this.tlpPreviews.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
			this.tlpPreviews.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
			this.tlpPreviews.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
			this.tlpPreviews.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tlpPreviews.Size = new System.Drawing.Size(500, 400);
			this.tlpPreviews.TabIndex = 3;
			// 
			// pnlOriginalText
			// 
			this.pnlOriginalText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tlpPreviews.SetColumnSpan(this.pnlOriginalText, 2);
			this.pnlOriginalText.Controls.Add(this.txtOriginal);
			this.pnlOriginalText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlOriginalText.Location = new System.Drawing.Point(0, 0);
			this.pnlOriginalText.Margin = new System.Windows.Forms.Padding(0);
			this.pnlOriginalText.Name = "pnlOriginalText";
			this.pnlOriginalText.Size = new System.Drawing.Size(246, 212);
			this.pnlOriginalText.TabIndex = 9;
			// 
			// txtOriginal
			// 
			this.txtOriginal.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtOriginal.Location = new System.Drawing.Point(0, 0);
			this.txtOriginal.Margin = new System.Windows.Forms.Padding(0);
			this.txtOriginal.Multiline = true;
			this.txtOriginal.Name = "txtOriginal";
			this.txtOriginal.ReadOnly = true;
			this.txtOriginal.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtOriginal.Size = new System.Drawing.Size(244, 210);
			this.txtOriginal.TabIndex = 0;
			// 
			// pnlTranslationText
			// 
			this.pnlTranslationText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tlpPreviews.SetColumnSpan(this.pnlTranslationText, 2);
			this.pnlTranslationText.Controls.Add(this.txtTranslation);
			this.pnlTranslationText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlTranslationText.Location = new System.Drawing.Point(254, 0);
			this.pnlTranslationText.Margin = new System.Windows.Forms.Padding(0);
			this.pnlTranslationText.Name = "pnlTranslationText";
			this.pnlTranslationText.Size = new System.Drawing.Size(246, 212);
			this.pnlTranslationText.TabIndex = 10;
			// 
			// txtTranslation
			// 
			this.txtTranslation.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtTranslation.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtTranslation.Location = new System.Drawing.Point(0, 0);
			this.txtTranslation.Margin = new System.Windows.Forms.Padding(0);
			this.txtTranslation.Multiline = true;
			this.txtTranslation.Name = "txtTranslation";
			this.txtTranslation.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtTranslation.Size = new System.Drawing.Size(244, 210);
			this.txtTranslation.TabIndex = 0;
			// 
			// txtTranslationNotes
			// 
			this.tlpPreviews.SetColumnSpan(this.txtTranslationNotes, 4);
			this.txtTranslationNotes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtTranslationNotes.Location = new System.Drawing.Point(0, 369);
			this.txtTranslationNotes.Margin = new System.Windows.Forms.Padding(0);
			this.txtTranslationNotes.Multiline = true;
			this.txtTranslationNotes.Name = "txtTranslationNotes";
			this.txtTranslationNotes.Size = new System.Drawing.Size(380, 31);
			this.txtTranslationNotes.TabIndex = 11;
			// 
			// chkIsIgnored
			// 
			this.chkIsIgnored.AutoSize = true;
			this.chkIsIgnored.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chkIsIgnored.Location = new System.Drawing.Point(390, 372);
			this.chkIsIgnored.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
			this.chkIsIgnored.Name = "chkIsIgnored";
			this.chkIsIgnored.Size = new System.Drawing.Size(107, 25);
			this.chkIsIgnored.TabIndex = 12;
			this.chkIsIgnored.Text = "Ignored/Unused";
			this.chkIsIgnored.UseVisualStyleBackColor = true;
			// 
			// pnlOriginalImage
			// 
			this.pnlOriginalImage.AutoScroll = true;
			this.pnlOriginalImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tlpPreviews.SetColumnSpan(this.pnlOriginalImage, 2);
			this.pnlOriginalImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlOriginalImage.Location = new System.Drawing.Point(0, 220);
			this.pnlOriginalImage.Margin = new System.Windows.Forms.Padding(0);
			this.pnlOriginalImage.Name = "pnlOriginalImage";
			this.pnlOriginalImage.Size = new System.Drawing.Size(246, 141);
			this.pnlOriginalImage.TabIndex = 0;
			// 
			// pnlTranslationImage
			// 
			this.pnlTranslationImage.AutoScroll = true;
			this.pnlTranslationImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tlpPreviews.SetColumnSpan(this.pnlTranslationImage, 2);
			this.pnlTranslationImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlTranslationImage.Location = new System.Drawing.Point(254, 220);
			this.pnlTranslationImage.Margin = new System.Windows.Forms.Padding(0);
			this.pnlTranslationImage.Name = "pnlTranslationImage";
			this.pnlTranslationImage.Size = new System.Drawing.Size(246, 141);
			this.pnlTranslationImage.TabIndex = 1;
			// 
			// TextEditorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tlpPreviews);
			this.Name = "TextEditorControl";
			this.Size = new System.Drawing.Size(500, 400);
			this.tlpPreviews.ResumeLayout(false);
			this.tlpPreviews.PerformLayout();
			this.pnlOriginalText.ResumeLayout(false);
			this.pnlOriginalText.PerformLayout();
			this.pnlTranslationText.ResumeLayout(false);
			this.pnlTranslationText.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpPreviews;
		private Controls.PanelEx pnlOriginalImage;
		private Controls.PanelEx pnlTranslationImage;
		private System.Windows.Forms.TextBox txtOriginal;
		private System.Windows.Forms.TextBox txtTranslation;
		private System.Windows.Forms.Panel pnlOriginalText;
		private System.Windows.Forms.Panel pnlTranslationText;
		private System.Windows.Forms.TextBox txtTranslationNotes;
		private System.Windows.Forms.CheckBox chkIsIgnored;
	}
}
