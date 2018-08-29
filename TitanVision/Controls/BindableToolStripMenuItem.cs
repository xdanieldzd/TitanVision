using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace TitanVision.Controls
{
	// https://social.msdn.microsoft.com/Forums/windows/en-US/0b8cba1e-f7ce-4ab0-a45b-2093dc38afc8/bind-property-in-toolstripmenuitem

	class BindableToolStripMenuItem : ToolStripMenuItem, IBindableComponent
	{
		private BindingContext bindingContext;
		private ControlBindingsCollection dataBindings;

		[Browsable(false)]
		public BindingContext BindingContext
		{
			get
			{
				if (bindingContext == null)
					bindingContext = new BindingContext();
				return bindingContext;
			}
			set { bindingContext = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ControlBindingsCollection DataBindings
		{
			get
			{
				if (dataBindings == null)
					dataBindings = new ControlBindingsCollection(this);
				return dataBindings;
			}
		}
	}
}
