using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TitanVision.DataStorage;

namespace TitanVision.Controls
{
	public partial class TextEditorControl : UserControl
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

		readonly static Pen markerPenBlack = new Pen(Color.Black, 3.0f);
		readonly static Pen markerPenOrange = new Pen(Color.DarkOrange, 3.0f);

		public GameRenderer GameRenderer { get; set; }

		TranslatableEntry translatableEntry;
		public TranslatableEntry TranslatableEntry
		{
			get { return translatableEntry; }
			set
			{
				translatableEntry = value;

				txtOriginal.DataBindings.Clear();
				txtTranslation.DataBindings.Clear();
				txtTranslationNotes.DataBindings.Clear();
				chkIsIgnored.DataBindings.Clear();

				if (translatableEntry != null)
				{
					txtOriginal.DataBindings.Add("Text", TranslatableEntry, nameof(TranslatableEntry.Original), false, DataSourceUpdateMode.OnPropertyChanged);
					txtTranslation.DataBindings.Add("Text", TranslatableEntry, nameof(TranslatableEntry.Translation), false, DataSourceUpdateMode.OnPropertyChanged);
					txtTranslationNotes.DataBindings.Add("Text", TranslatableEntry, nameof(TranslatableEntry.Notes), false, DataSourceUpdateMode.OnPropertyChanged);
					chkIsIgnored.DataBindings.Add("Checked", TranslatableEntry, nameof(TranslatableEntry.IsIgnored), false, DataSourceUpdateMode.OnPropertyChanged);
				}

				txtOriginal.SelectionStart = txtOriginal.Text.Length;
				txtOriginal.DeselectAll();
			}
		}

		public bool OverridesEnabled { get; set; }

		public KeyValuePair<string, int> LimitMarkerWidth { get; set; }

		public TextEditorControl()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Opaque, true);
			DoubleBuffered = true;

			pnlOriginalImage.Paint += (s, e) => { RedrawPreview(e.Graphics, (s as Panel), TranslatableEntry?.Original); };
			pnlTranslationImage.Paint += (s, e) => { RedrawPreview(e.Graphics, (s as Panel), TranslatableEntry?.Translation); };
		}

		private void RedrawPreview(Graphics g, Panel panel, string text)
		{
			g.TranslateTransform(panel.AutoScrollPosition.X, panel.AutoScrollPosition.Y);
			panel.AutoScrollMinSize = new Size((panel.ClientSize.Width - SystemInformation.VerticalScrollBarWidth), GameRenderer?.MeasureStringHeight(text) ?? panel.ClientSize.Height);
			GameRenderer?.DrawString(g, text, OverridesEnabled);
			DrawLimitMarker(g, panel, text);
		}

		private void DrawLimitMarker(Graphics g, Panel panel, string text)
		{
			if (LimitMarkerWidth.Value != 0 && text != null && text != string.Empty)
				g.DrawLine((GameRenderer.HasVariableLengthControlCodes(text) ? markerPenOrange : markerPenBlack), LimitMarkerWidth.Value, 0, LimitMarkerWidth.Value, panel.AutoScrollMinSize.Height);
		}
	}
}
