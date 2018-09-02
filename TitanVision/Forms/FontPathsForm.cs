using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TitanVision.Helpers;

namespace TitanVision.Forms
{
	public partial class FontPathsForm : Form
	{
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x02000000;
				return cp;
			}
		}

		const string bcfntFilter = "3DS Font Files|*.bcfnt";

		List<string> sourcePathList;

		public FontPathsForm(List<string> pathList)
		{
			InitializeComponent();

			sourcePathList = pathList;
			lvPaths.VirtualListSize = sourcePathList.Count;

			EnableDisableControls();
		}

		private void lvPaths_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
		{
			e.Item = new ListViewItem() { Text = sourcePathList[e.ItemIndex] };
		}

		private void lvPaths_SelectedIndexChanged(object sender, EventArgs e)
		{
			EnableDisableControls();
		}

		private void lvPaths_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ChangePath();
		}

		private void EnableDisableControls()
		{
			btnChange.Enabled = btnRemove.Enabled = (lvPaths.SelectedIndices.Count > 0);
		}

		private void ChangePath()
		{
			if (lvPaths.SelectedIndices.Count == 0) return;

			if ((GUIHelper.ShowFileOpenDialog(string.Empty, sourcePathList[lvPaths.SelectedIndices[0]], new string[] { bcfntFilter }, out string selectedPath) == DialogResult.OK))
			{
				sourcePathList[lvPaths.SelectedIndices[0]] = selectedPath;
				lvPaths.VirtualListSize = sourcePathList.Count;

				lvPaths.RedrawItems(lvPaths.SelectedIndices[0], lvPaths.SelectedIndices[0], false);
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			if ((GUIHelper.ShowFileOpenDialog(string.Empty, string.Empty, new string[] { bcfntFilter }, out string selectedPath) == DialogResult.OK))
			{
				sourcePathList.Add(selectedPath);
				lvPaths.VirtualListSize = sourcePathList.Count;
			}
		}

		private void btnChange_Click(object sender, EventArgs e)
		{
			ChangePath();
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			if (lvPaths.SelectedIndices.Count == 0) return;

			sourcePathList.RemoveAt(lvPaths.SelectedIndices[0]);
			lvPaths.VirtualListSize = sourcePathList.Count;

			EnableDisableControls();
		}
	}
}
