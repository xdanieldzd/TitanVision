using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TitanVision.Controls
{
	public class ListViewEx : ListView
	{
		public ListViewEx() : base()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.EnableNotifyMessage, true);
		}

		protected override void OnNotifyMessage(Message m)
		{
			// Ignore WM_ERASEBKGND
			if (m.Msg != 0x14)
				base.OnNotifyMessage(m);
		}

		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
		public extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6)
			{
				SetWindowTheme(Handle, "explorer", null);
			}
		}
	}
}
