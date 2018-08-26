using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Newtonsoft.Json.Linq;

namespace TitanVision
{
	public partial class MainForm : Form
	{
		const string jsonConfigFileName = "Config.json";

		readonly static string programConfigDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Application.ProductName);
		readonly static string programConfigPath = Path.Combine(programConfigDir, jsonConfigFileName);

		readonly Brush comboInvalidStringBrush, comboNotTranslatedBrush, comboTranslatedBrush;

		Configuration config;

		GameRenderer[] renderers;
		List<string> enemyNames, skyItemNames, limitItemNames, useItemNames, equipItemNames;

		bool dataLoaded;

		string currentFilePath;
		Translation currentTranslationFile;

		public MainForm()
		{
			InitializeComponent();

			LoadConfiguration();

			comboInvalidStringBrush = new SolidBrush(Color.FromArgb(255, 224, 224));
			comboNotTranslatedBrush = new SolidBrush(Color.FromArgb(255, 255, 224));
			comboTranslatedBrush = new SolidBrush(Color.FromArgb(224, 255, 224));

			dataLoaded = false;

			cmbMessage.SelectedIndexChanged += (s, e) =>
			{
				if (cmbMessage.SelectedItem != null)
				{
					(cmbMessage.SelectedItem as TranslatableEntry).PropertyChanged -= MainForm_PropertyChanged;
					(cmbMessage.SelectedItem as TranslatableEntry).PropertyChanged += MainForm_PropertyChanged;
				}

				UpdateTextEditor();
			};

			cmbMessage.DrawItem += (s, e) =>
			{
				var entry = ((s as ComboBox).Items[e.Index] as TranslatableEntry);
				var text = ((entry.ID == -1) ? "(Invalid)" : $"#{entry.ID:D3}: {entry.Original.TrimEnd(Environment.NewLine.ToCharArray()).Replace(Environment.NewLine, " ")}");

				e.Graphics.FillRectangle(
					((entry.ID == -1) ? comboInvalidStringBrush : ((string.Compare(entry.Original, entry.Translation) == 0) ? comboNotTranslatedBrush : comboTranslatedBrush)),
					e.Bounds);
				TextRenderer.DrawText(e.Graphics, text, e.Font, e.Bounds, SystemColors.ControlText, TextFormatFlags.Left | TextFormatFlags.WordEllipsis);
				e.DrawFocusRectangle();
			};

			cmbMessage.DisplayMember = "Original";

			cmbFont.SelectedIndexChanged += (s, e) =>
			{
				UpdateTextEditor();
			};

			cmbFont.DisplayMember = "FontName";
			cmbFont.DataSource = renderers;

			FormClosing += (s, e) =>
			{
				config.SerializeToFile(programConfigPath);
			};

			UpdateFormTitle();
		}

		private void LoadConfiguration()
		{
			Directory.CreateDirectory(programConfigDir);

			if (!File.Exists(programConfigPath))
			{
				config = new Configuration();
				config.SerializeToFile(programConfigPath);
			}
			else
			{
				config = programConfigPath.DeserializeFromFile<Configuration>();
			}

			renderers = config.FontPaths.Where(x => File.Exists(x)).Select(x => new GameRenderer(x)).ToArray();
		}

		private void UpdateTextEditor()
		{
			textEditorControl.DataBindings.Clear();
			textEditorControl.DataBindings.Add(nameof(textEditorControl.TranslatableEntry), cmbMessage, nameof(cmbMessage.SelectedItem), false, DataSourceUpdateMode.OnPropertyChanged);
			textEditorControl.DataBindings.Add(nameof(textEditorControl.GameRenderer), cmbFont, nameof(cmbFont.SelectedItem), false, DataSourceUpdateMode.OnPropertyChanged);
			textEditorControl.ForceRedrawPreviews();
		}

		private void UpdateFormTitle()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append(Application.ProductName);

			if (dataLoaded)
				stringBuilder.Append($" - [{config.JsonRootDirectory}]");

			Text = stringBuilder.ToString();
		}

		private void UpdateInfoLabel()
		{
			lblInfo.Text = $"{currentTranslationFile.Description} ({currentTranslationFile.FileType}): {currentTranslationFile.Entries.Count(x => x.ID != -1 && string.Compare(x.Original, x.Translation) != 0)}/{currentTranslationFile.Entries.Count(x => x.ID != -1)} translated";
		}

		private void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (GUIHelper.ShowFolderBrowser("Select Directory", "Select JSON root directory...", config.JsonRootDirectory, out string selectedPath) == DialogResult.OK)
			{
				OpenData(selectedPath);
			}
		}

		private void OpenData(string rootDirectory)
		{
			config.JsonRootDirectory = rootDirectory;

			enemyNames = LoadStringSubstJson(Path.Combine(config.JsonRootDirectory, @"Monster\Table\enemynametable.json"));
			skyItemNames = LoadStringSubstJson(Path.Combine(config.JsonRootDirectory, @"Item\skyitemname.json"));
			limitItemNames = LoadStringSubstJson(Path.Combine(config.JsonRootDirectory, @"Item\limititemnametable.json"));
			useItemNames = LoadStringSubstJson(Path.Combine(config.JsonRootDirectory, @"Item\useitemnametable.json"));
			equipItemNames = LoadStringSubstJson(Path.Combine(config.JsonRootDirectory, @"Item\equipitemnametable.json"));

			foreach (var renderer in renderers)
			{
				renderer.SetSubstitutionList("Enemy", enemyNames);
				renderer.SetSubstitutionList("SkyItems", skyItemNames);
				renderer.SetSubstitutionList("LimitItems", limitItemNames);
				renderer.SetSubstitutionList("UseItems", useItemNames);
				renderer.SetSubstitutionList("EquipItems", equipItemNames);
			}

			tvTextFiles.Nodes.Clear();
			var rootDirectoryInfo = new DirectoryInfo(config.JsonRootDirectory);
			var rootNode = CreateDirectoryNode(rootDirectoryInfo);
			rootNode.Expand();
			tvTextFiles.Nodes.Add(rootNode);

			tvTextFiles.AfterSelect += (s, e) =>
			{
				if (e.Node.Tag is ValueTuple<string, Translation>)
				{
					(string path, Translation translation) = (ValueTuple<string, Translation>)e.Node.Tag;

					currentFilePath = path;
					currentTranslationFile = translation;
					cmbMessage.DataSource = currentTranslationFile.Entries;
					UpdateInfoLabel();
				}
			};

			dataLoaded = true;

			UpdateFormTitle();

			saveDirectoryToolStripMenuItem.Enabled = tvTextFiles.Enabled = cmbMessage.Enabled = cmbFont.Enabled = lblInfo.Enabled = textEditorControl.Enabled = true;
		}

		private void saveDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveData();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var description = System.Reflection.Assembly.GetExecutingAssembly().GetAttribute<System.Reflection.AssemblyDescriptionAttribute>().Description;
			var copyright = System.Reflection.Assembly.GetExecutingAssembly().GetAttribute<System.Reflection.AssemblyCopyrightAttribute>().Copyright;
			var version = new Version(Application.ProductVersion);

			GUIHelper.ShowInformationMessage("About", $"About {Application.ProductName}", $"{Application.ProductName} v{version.Major}.{version.Minor} - {description}\n\n{copyright.Replace(" - ", Environment.NewLine)}");
		}

		private void MainForm_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			cmbMessage.Invalidate();
			textEditorControl.ForceRedrawPreviews();

			UpdateInfoLabel();
		}

		private TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
		{
			var directoryNode = new TreeNode(directoryInfo.Name);
			foreach (var directory in directoryInfo.GetDirectories().Where(x => x.GetFiles("*", SearchOption.AllDirectories).Any(y => y.Extension == ".json")))
				directoryNode.Nodes.Add(CreateDirectoryNode(directory));
			foreach (var file in directoryInfo.GetFiles("*.json"))
				directoryNode.Nodes.Add(new TreeNode(file.Name) { Tag = (file.FullName, file.FullName.DeserializeFromFile<Translation>()) });
			return directoryNode;
		}

		private void SaveData()
		{
			foreach (var node in FindAllChangedTranslations(tvTextFiles.Nodes))
			{
				(string path, Translation translation) = (ValueTuple<string, Translation>)node.Tag;
				translation.SerializeToFile(path);
			}

			config.SerializeToFile(programConfigPath);
		}

		private IEnumerable<TreeNode> FindAllChangedTranslations(TreeNodeCollection nodes)
		{
			foreach (TreeNode node in nodes)
			{
				if (node.Tag is ValueTuple<string, Translation>)
				{
					(string path, Translation translation) = (ValueTuple<string, Translation>)node.Tag;
					if (translation.Entries.Any(x => string.Compare(x.Original, x.Translation) != 0))
						yield return node;
				}

				foreach (var child in FindAllChangedTranslations(node.Nodes))
					yield return child;
			}
		}

		private List<string> LoadStringSubstJson(string jsonFileName)
		{
			var jsonObject = JObject.Parse(File.ReadAllText(jsonFileName));
			return jsonObject["Entries"].Select(x => x["Translation"]).Values<string>().ToList();
		}

		private void pictureBox1_Paint(object sender, PaintEventArgs e)
		{
			/*string test = "This is a [Color:0003]colored [Color:0005]test[Color:0000]!\r\n" +
				"Look, it's [Color:0010]colorful[Color:0000]! [Color:0011]COLOR. FUL.[Color:0000]\r\n[Page]\r\n\r\n" +
				"Some enemy is called [Color:0005][Enemy:0003][Color:0000], huh!\r\n" +
				"And there's an item called [Color:0005][Item:0010][Color:0000], I think?";

			textRenderer.DrawString(e.Graphics, test);*/
		}
	}
}
