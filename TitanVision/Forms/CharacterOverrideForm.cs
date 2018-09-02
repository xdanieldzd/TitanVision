using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TitanVision.Forms
{
	// partially https://stackoverflow.com/a/577105

	public partial class CharacterOverrideForm : Form
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

		Dictionary<char, char> sourceDictionary;
		BindingList<CharPair> pairs;

		public CharacterOverrideForm(Dictionary<char, char> dictionary)
		{
			InitializeComponent();

			sourceDictionary = dictionary;

			pairs = new BindingList<CharPair>();
			foreach (var dictEntry in sourceDictionary)
				pairs.Add(new CharPair { Key = dictEntry.Key, Value = dictEntry.Value });

			pairs.AddingNew += (s, e) => { e.NewObject = new CharPair { Parent = pairs }; };

			dgvDictionary.DataSource = pairs;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			sourceDictionary.Clear();
			foreach (var pair in pairs)
				sourceDictionary[pair.Key] = pair.Value;
		}

		private void dgvDictionary_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			// ehh i dunno if this is okay, but let's just do it since it should work fine regardless as stuff's handled below?

			e.ThrowException = false;
			e.Cancel = false;
		}

		private void dgvDictionary_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			var dataGridView = (sender as DataGridView);

			var valueString = e.FormattedValue.ToString();
			if (valueString.Length > 1)
			{
				dataGridView.Rows[e.RowIndex].ErrorText = "Value must be single character";
				e.Cancel = true;
			}
			else
			{
				dataGridView.Rows[e.RowIndex].ErrorText = string.Empty;
				e.Cancel = false;
			}
		}

		private void dgvDictionary_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
		{
			var dataGridView = (sender as DataGridView);

			var emptyCellCount = dataGridView.Rows[e.RowIndex].Cells.Cast<DataGridViewCell>().Count(x => x.Value == null || ((x.Value is char chr) && (chr == '\0')));
			if (emptyCellCount > 0 && emptyCellCount < dataGridView.ColumnCount)
			{
				dataGridView.Rows[e.RowIndex].ErrorText = "Values cannot be empty";
				e.Cancel = true;
			}
			else
			{
				dataGridView.Rows[e.RowIndex].ErrorText = string.Empty;
				e.Cancel = false;
			}
		}
	}

	class CharPair : IDataErrorInfo
	{
		internal IList<CharPair> Parent { get; set; }

		public char Key { get; set; }
		public char Value { get; set; }

		string IDataErrorInfo.Error
		{
			get { return string.Empty; }
		}

		string IDataErrorInfo.this[string columnName]
		{
			get
			{
				if (Parent != null)
				{
					if (columnName == "Key" && Parent.Any(x => x.Key == Key && !ReferenceEquals(x, this)))
						return "Duplicate key";

					if (columnName == "Value" && Parent.Any(x => x.Value == Value && !ReferenceEquals(x, this)))
						return "Duplicate value";
				}

				return "";
			}
		}
	}
}
