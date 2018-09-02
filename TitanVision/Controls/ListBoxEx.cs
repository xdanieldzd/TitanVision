using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TitanVision.Controls
{
	public class ListBoxEx : ListBox
	{
		public ListBoxEx() : base()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Opaque | ControlStyles.ResizeRedraw, true);
			DoubleBuffered = true;
		}
	}
}
