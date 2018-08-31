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
using System.Threading;

using Newtonsoft.Json.Linq;

using TitanVision.DataStorage;
using TitanVision.Forms;
using TitanVision.Helpers;

namespace TitanVision
{
	public partial class MainForm : Form
	{
		const string jsonConfigFileName = "Config.json";

		readonly static string programConfigDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Application.ProductName);
		readonly static string programConfigPath = Path.Combine(programConfigDir, jsonConfigFileName);

		readonly Brush comboInvalidStringBrush, comboNotTranslatedBrush, comboTranslatedBrush, comboIgnoredBrush;

		Configuration config;

		List<GameRenderer> renderers;
		List<string> enemyNames, skyItemNames, limitItemNames, useItemNames, equipItemNames;

		bool dataLoaded, fontsReady;

		string currentFilePath;
		Translation currentTranslationFile;

		public MainForm()
		{
			InitializeComponent();

			LoadConfiguration();

			comboInvalidStringBrush = new SolidBrush(Color.FromArgb(255, 224, 224));
			comboNotTranslatedBrush = new SolidBrush(Color.FromArgb(255, 255, 224));
			comboTranslatedBrush = new SolidBrush(Color.FromArgb(224, 255, 224));
			comboIgnoredBrush = new SolidBrush(Color.FromArgb(224, 244, 255));

			dataLoaded = false;
			fontsReady = false;

			enableCharacterOverridesToolStripMenuItem.CheckedChanged += (s, e) =>
			{
				textEditorControl.ForceRedrawPreviews();
			};

			cmbMessage.SelectedIndexChanged += (s, e) =>
			{
				if (cmbMessage.SelectedItem != null)
				{
					(cmbMessage.SelectedItem as TranslatableEntry).PropertyChanged -= MainForm_PropertyChanged;
					(cmbMessage.SelectedItem as TranslatableEntry).PropertyChanged += MainForm_PropertyChanged;
				}

				UpdateTextEditor();
			};
			cmbMessage.MeasureItem += (s, e) =>
			{
				var comboBox = (s as ComboBox);
				var entry = (comboBox.Items[e.Index] as TranslatableEntry);
				var lines = entry.Original.Count(x => x == '\n');
				if (lines == 0) lines = 1;
				if (lines >= 3) lines = 3;

				var size = TextRenderer.MeasureText("TEXT", comboBox.Font);
				e.ItemHeight = ((size.Height * lines) + 6);
			};
			cmbMessage.DrawItem += (s, e) =>
			{
				var entry = ((s as ComboBox).Items[e.Index] as TranslatableEntry);

				string label = "(Invalid)";
				Rectangle rectangle = new Rectangle(e.Bounds.X, (e.Bounds.Y + 2), e.Bounds.Width, (e.Bounds.Height - 4));

				if (entry.ID != -1)
				{
					var text = entry.Original.TrimEnd(Environment.NewLine.ToCharArray());
					if ((e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit)
					{
						label = $"#{entry.ID:D3}: {text.Replace(Environment.NewLine, " ")}";
						rectangle = e.Bounds;
					}
					else
						label = text;
				}

				e.Graphics.FillRectangle(
					((entry.ID == -1) ? comboInvalidStringBrush :
					(entry.IsIgnored ? comboIgnoredBrush :
					((string.Compare(entry.Original, entry.Translation) == 0) ? comboNotTranslatedBrush :
					comboTranslatedBrush))),
					e.Bounds);

				TextRenderer.DrawText(e.Graphics, label, e.Font, rectangle, SystemColors.ControlText, TextFormatFlags.Left | TextFormatFlags.WordEllipsis);

				if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
					ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds);
			};
			cmbMessage.DisplayMember = "Original";

			cmbFont.SelectedIndexChanged += (s, e) =>
			{
				UpdateTextEditor();
			};

			FormClosing += (s, e) =>
			{
				config.SerializeToFile(programConfigPath);
			};

			UpdateFormTitle();

			tsslStatus.Text = "Ready";
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

			enableCharacterOverridesToolStripMenuItem.DataBindings.Add(nameof(enableCharacterOverridesToolStripMenuItem.Checked), config, nameof(config.OverridesEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
		}

		private void CreateRenderers()
		{
			var tokenSource = new CancellationTokenSource();
			var ct = tokenSource.Token;

			tspbProgress.Visible = true;

			Task task = Task.Factory.StartNew(() =>
			{
				var _lock = new object();
				renderers = new List<GameRenderer>();

				ct.ThrowIfCancellationRequested();

				int count = 0;
				Parallel.ForEach(config.FontPaths, (file) =>
				{
					if (ct.IsCancellationRequested)
						ct.ThrowIfCancellationRequested();

					if (!File.Exists(file)) return;

					var renderer = new GameRenderer(file);
					foreach (var charaOverride in config.CharacterOverrides)
						renderer.SetCharacterOverride(charaOverride.Key, charaOverride.Value);
					lock (_lock) { renderers.Add(renderer); }

					BeginInvoke((Action)delegate ()
					{
						tsslStatus.Text = $"Loading font {Path.GetFileName(file)}...";
						tspbProgress.SetProgressNoAnimation((tspbProgress.Maximum / (config.FontPaths.Count)) * count);
						count++;
					});
				});

				renderers = renderers.OrderBy(x => config.FontPaths.FindIndex(y => Path.GetFileNameWithoutExtension(y) == x.FontName)).ToList();
			}, tokenSource.Token);

			task.ContinueWith((t) =>
			{
				BeginInvoke((Action)delegate ()
				{
					tsslStatus.Text = "Ready";
					tspbProgress.Visible = false;

					cmbFont.DisplayMember = "FontName";
					cmbFont.DataSource = renderers;

					fontsReady = true;
				});
			});
		}

		private void SetRendererSubstitutionLists()
		{
			if (renderers == null) return;

			foreach (var renderer in renderers)
			{
				renderer.SetSubstitutionList("Enemy", enemyNames);
				renderer.SetSubstitutionList("SkyItems", skyItemNames);
				renderer.SetSubstitutionList("LimitItems", limitItemNames);
				renderer.SetSubstitutionList("UseItems", useItemNames);
				renderer.SetSubstitutionList("EquipItems", equipItemNames);
			}
		}

		private void UpdateTextEditor()
		{
			if (!fontsReady) return;

			textEditorControl.DataBindings.Clear();

			if (cmbMessage.SelectedItem != null)
				textEditorControl.DataBindings.Add(nameof(textEditorControl.TranslatableEntry), cmbMessage, nameof(cmbMessage.SelectedItem), false, DataSourceUpdateMode.OnPropertyChanged);

			if (cmbFont.SelectedItem != null)
				textEditorControl.DataBindings.Add(nameof(textEditorControl.GameRenderer), cmbFont, nameof(cmbFont.SelectedItem), false, DataSourceUpdateMode.OnPropertyChanged);

			textEditorControl.DataBindings.Add(nameof(textEditorControl.OverridesEnabled), config, nameof(config.OverridesEnabled), false, DataSourceUpdateMode.OnPropertyChanged);

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
			var stringBuilder = new StringBuilder();
			stringBuilder.Append($"Current file: {Path.GetFileNameWithoutExtension(currentTranslationFile.RelativePath)} ({currentTranslationFile.Description})");
			stringBuilder.Append(" - ");

			var validEntryCount = currentTranslationFile.Entries.Count(x => x.ID != -1);
			var ignoredEntryCount = currentTranslationFile.Entries.Count(x => x.ID != -1 && x.IsIgnored);
			var translatedEntryCount = currentTranslationFile.Entries.Count(x => x.ID != -1 && string.Compare(x.Original, x.Translation) != 0);
			stringBuilder.Append($"Translated {translatedEntryCount}/{validEntryCount - ignoredEntryCount} valid entries (additional {ignoredEntryCount} ignored)");

			tsslStatus.Text = stringBuilder.ToString();
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

			SetRendererSubstitutionLists();

			tvTextFiles.Nodes.Clear();
			var rootDirectoryInfo = new DirectoryInfo(config.JsonRootDirectory);
			var rootNode = CreateDirectoryNode(rootDirectoryInfo);
			rootNode.Text = "Root";
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

			saveDirectoryToolStripMenuItem.Enabled = tvTextFiles.Enabled = cmbMessage.Enabled = cmbFont.Enabled = lblPreviewFont.Enabled = textEditorControl.Enabled = true;
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

		private void saveDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveData();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var description = System.Reflection.Assembly.GetExecutingAssembly().GetAttribute<System.Reflection.AssemblyDescriptionAttribute>().Description;
			var copyright = System.Reflection.Assembly.GetExecutingAssembly().GetAttribute<System.Reflection.AssemblyCopyrightAttribute>().Copyright;
			var version = new Version(Application.ProductVersion);

			GUIHelper.ShowInformationMessage("About", $"About {Application.ProductName}", $"{Application.ProductName} v{version.Major}.{version.Minor} - {description}\n\n{copyright.Replace(" - ", Environment.NewLine)}");
		}

		private void characterOverridesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var characterOverrideForm = new CharacterOverrideForm(config.CharacterOverrides);
			characterOverrideForm.ShowDialog();

			textEditorControl.ForceRedrawPreviews();
		}

		private void fontPathsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var fontPathsForm = new FontPathsForm(config.FontPaths);
			fontPathsForm.ShowDialog();

			cmbFont.DataSource = null;

			CreateRenderers();
			SetRendererSubstitutionLists();
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			CreateRenderers();
		}

		private void MainForm_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			cmbMessage.Invalidate();
			textEditorControl.ForceRedrawPreviews();

			UpdateInfoLabel();
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
	}
}
