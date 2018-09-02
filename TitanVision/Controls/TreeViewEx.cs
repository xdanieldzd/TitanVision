using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TitanVision.Controls
{
	// https://stackoverflow.com/a/10364283

	public class TreeViewEx : TreeView
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

		private const int TVM_SETEXTENDEDSTYLE = (0x1100 + 44);
		private const int TVM_GETEXTENDEDSTYLE = (0x1100 + 45);
		private const int TVS_EX_DOUBLEBUFFER = 0x0004;

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

		protected override void OnHandleCreated(EventArgs e)
		{
			SendMessage(Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
			base.OnHandleCreated(e);
		}
	}
}
