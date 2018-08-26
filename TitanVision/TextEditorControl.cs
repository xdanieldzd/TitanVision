﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TitanMessage;

namespace TitanVision
{
	public partial class TextEditorControl : UserControl
	{
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

				if (translatableEntry != null)
				{
					txtOriginal.DataBindings.Add("Text", TranslatableEntry, nameof(TranslatableEntry.Original), false, DataSourceUpdateMode.OnPropertyChanged);
					txtTranslation.DataBindings.Add("Text", TranslatableEntry, nameof(TranslatableEntry.Translation), false, DataSourceUpdateMode.OnPropertyChanged);
					txtTranslationNotes.DataBindings.Add("Text", TranslatableEntry, nameof(TranslatableEntry.Notes), false, DataSourceUpdateMode.OnPropertyChanged);
				}

				txtOriginal.SelectionStart = txtOriginal.Text.Length;
				txtOriginal.DeselectAll();

				ForceRedrawPreviews();
			}
		}

		public TextEditorControl()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Opaque, true);
			DoubleBuffered = true;
		}

		public void ForceRedrawPreviews()
		{
			if (GameRenderer == null || TranslatableEntry == null) return;

			var oldOriginalImage = pbOriginal.Image;
			pbOriginal.Image = GameRenderer.GetBitmap(TranslatableEntry.Original);
			oldOriginalImage?.Dispose();

			var oldTranslationImage = pbTranslation.Image;
			pbTranslation.Image = GameRenderer.GetBitmap(TranslatableEntry.Translation);
			oldTranslationImage?.Dispose();
		}
	}
}