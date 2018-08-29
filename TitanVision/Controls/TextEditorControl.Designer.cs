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
			this.pnlOriginal = new System.Windows.Forms.Panel();
			this.pbOriginal = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.pbTranslation = new System.Windows.Forms.PictureBox();
			this.pnlOriginalText = new System.Windows.Forms.Panel();
			this.txtOriginal = new System.Windows.Forms.TextBox();
			this.pnlTranslationText = new System.Windows.Forms.Panel();
			this.txtTranslation = new System.Windows.Forms.TextBox();
			this.txtTranslationNotes = new System.Windows.Forms.TextBox();
			this.tlpPreviews.SuspendLayout();
			this.pnlOriginal.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbOriginal)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbTranslation)).BeginInit();
			this.pnlOriginalText.SuspendLayout();
			this.pnlTranslationText.SuspendLayout();
			this.SuspendLayout();
			// 
			// tlpPreviews
			// 
			this.tlpPreviews.ColumnCount = 3;
			this.tlpPreviews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpPreviews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
			this.tlpPreviews.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpPreviews.Controls.Add(this.pnlOriginal, 0, 2);
			this.tlpPreviews.Controls.Add(this.panel1, 2, 2);
			this.tlpPreviews.Controls.Add(this.pnlOriginalText, 0, 0);
			this.tlpPreviews.Controls.Add(this.pnlTranslationText, 2, 0);
			this.tlpPreviews.Controls.Add(this.txtTranslationNotes, 0, 4);
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
			// pnlOriginal
			// 
			this.pnlOriginal.AutoScroll = true;
			this.pnlOriginal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlOriginal.Controls.Add(this.pbOriginal);
			this.pnlOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlOriginal.Location = new System.Drawing.Point(0, 220);
			this.pnlOriginal.Margin = new System.Windows.Forms.Padding(0);
			this.pnlOriginal.Name = "pnlOriginal";
			this.pnlOriginal.Size = new System.Drawing.Size(246, 141);
			this.pnlOriginal.TabIndex = 0;
			// 
			// pbOriginal
			// 
			this.pbOriginal.BackColor = System.Drawing.Color.Transparent;
			this.pbOriginal.Location = new System.Drawing.Point(0, 0);
			this.pbOriginal.Margin = new System.Windows.Forms.Padding(0);
			this.pbOriginal.Name = "pbOriginal";
			this.pbOriginal.Size = new System.Drawing.Size(140, 150);
			this.pbOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pbOriginal.TabIndex = 3;
			this.pbOriginal.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.pbTranslation);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(254, 220);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(246, 141);
			this.panel1.TabIndex = 1;
			// 
			// pbTranslation
			// 
			this.pbTranslation.BackColor = System.Drawing.Color.Transparent;
			this.pbTranslation.Location = new System.Drawing.Point(0, 0);
			this.pbTranslation.Margin = new System.Windows.Forms.Padding(0);
			this.pbTranslation.Name = "pbTranslation";
			this.pbTranslation.Size = new System.Drawing.Size(140, 150);
			this.pbTranslation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pbTranslation.TabIndex = 4;
			this.pbTranslation.TabStop = false;
			// 
			// pnlOriginalText
			// 
			this.pnlOriginalText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
			this.tlpPreviews.SetColumnSpan(this.txtTranslationNotes, 3);
			this.txtTranslationNotes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtTranslationNotes.Location = new System.Drawing.Point(0, 369);
			this.txtTranslationNotes.Margin = new System.Windows.Forms.Padding(0);
			this.txtTranslationNotes.Multiline = true;
			this.txtTranslationNotes.Name = "txtTranslationNotes";
			this.txtTranslationNotes.Size = new System.Drawing.Size(500, 31);
			this.txtTranslationNotes.TabIndex = 11;
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
			this.pnlOriginal.ResumeLayout(false);
			this.pnlOriginal.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbOriginal)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbTranslation)).EndInit();
			this.pnlOriginalText.ResumeLayout(false);
			this.pnlOriginalText.PerformLayout();
			this.pnlTranslationText.ResumeLayout(false);
			this.pnlTranslationText.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpPreviews;
		private System.Windows.Forms.PictureBox pbTranslation;
		private System.Windows.Forms.PictureBox pbOriginal;
		private System.Windows.Forms.Panel pnlOriginal;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox txtOriginal;
		private System.Windows.Forms.TextBox txtTranslation;
		private System.Windows.Forms.Panel pnlOriginalText;
		private System.Windows.Forms.Panel pnlTranslationText;
		private System.Windows.Forms.TextBox txtTranslationNotes;
	}
}
