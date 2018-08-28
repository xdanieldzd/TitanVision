namespace TitanVision
{
	partial class CharacterOverrideForm
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
			this.btnClose = new System.Windows.Forms.Button();
			this.dgvDictionary = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.dgvDictionary)).BeginInit();
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(197, 327);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 0;
			this.btnClose.Text = "&Close";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// dgvDictionary
			// 
			this.dgvDictionary.AllowUserToResizeColumns = false;
			this.dgvDictionary.AllowUserToResizeRows = false;
			this.dgvDictionary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvDictionary.Location = new System.Drawing.Point(12, 12);
			this.dgvDictionary.Name = "dgvDictionary";
			this.dgvDictionary.Size = new System.Drawing.Size(260, 309);
			this.dgvDictionary.TabIndex = 1;
			this.dgvDictionary.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvDictionary_CellValidating);
			this.dgvDictionary.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDictionary_DataError);
			this.dgvDictionary.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvDictionary_RowValidating);
			// 
			// CharacterOverrideForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(284, 362);
			this.Controls.Add(this.dgvDictionary);
			this.Controls.Add(this.btnClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CharacterOverrideForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Character Overrides";
			((System.ComponentModel.ISupportInitialize)(this.dgvDictionary)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.DataGridView dgvDictionary;
	}
}