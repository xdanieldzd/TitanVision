using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TitanVision.Controls
{
	public class TreeViewEx : TreeView
	{
		public TreeViewEx() : base()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.EnableNotifyMessage, true);
		}

		protected override void OnNotifyMessage(Message m)
		{
			// Ignore WM_ERASEBKGND
			if (m.Msg != 0x14)
				base.OnNotifyMessage(m);
		}
	}
}
