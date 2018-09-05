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

		readonly Color invalidOrNothingColor, notTranslatedColor, translatedColor, ignoredColor;
		readonly Brush invalidOrNothingBrush, notTranslatedBrush, translatedBrush, ignoredBrush;

		Configuration config;

		List<GameRenderer> renderers;
		List<string> enemyNames, skyItemNames, limitItemNames, useItemNames, equipItemNames;

		bool dataLoaded, fontsReady, substListsAssigned;

		string currentFilePath;
		Translation currentTranslationFile;

		Dictionary<string, Translation> translationFilesOpened;

		public MainForm()
		{
			InitializeComponent();

			LoadConfiguration();

			invalidOrNothingBrush = new SolidBrush(invalidOrNothingColor = Color.FromArgb(255, 224, 224));
			notTranslatedBrush = new SolidBrush(notTranslatedColor = Color.FromArgb(255, 255, 224));
			translatedBrush = new SolidBrush(translatedColor = Color.FromArgb(224, 255, 224));
			ignoredBrush = new SolidBrush(ignoredColor = Color.FromArgb(224, 244, 255));

			dataLoaded = false;
			fontsReady = false;
			substListsAssigned = false;

			enableCharacterOverridesToolStripMenuItem.CheckedChanged += (s, e) =>
			{
				textEditorControl.Invalidate();
			};

			tvTextFiles.DrawNode += (s, e) =>
			{
				if (e.Node == null || !e.Node.IsVisible) return;

				var selected = ((e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected);
				var unfocused = (!e.Node.TreeView.Focused);
				var font = (e.Node.NodeFont ?? e.Node.TreeView.Font);

				var textFormatFlags = TextFormatFlags.GlyphOverhangPadding;
				var textBounds = new Rectangle(e.Bounds.X, e.Bounds.Y + 1, e.Bounds.Width, e.Bounds.Height);

				if (e.Node.Tag is ValueTuple<string, Translation>)
				{
					(string path, Translation translation) = (ValueTuple<string, Translation>)e.Node.Tag;

					var translatedCount = translation.Entries.Count(x => x.ID != -1 && !x.IsIgnored && string.Compare(x.Original, x.Translation) != 0);
					var totalValidCount = translation.Entries.Count(x => x.ID != -1 && !x.IsIgnored);

					var countNoteText = $"[{translatedCount}/{totalValidCount}]";
					var countNoteTextBounds = new Rectangle(textBounds.Right, textBounds.Top, TextRenderer.MeasureText(countNoteText, font).Width, textBounds.Height);
					var countNoteBackgroundBounds = new Rectangle(e.Node.Bounds.Right, e.Node.Bounds.Top, countNoteTextBounds.Width, e.Node.Bounds.Height);

					e.Graphics.FillRectangle(SystemBrushes.Window, countNoteBackgroundBounds);
					if (!selected)
					{
						e.Graphics.FillRectangle(((translatedCount == 0 && totalValidCount != 0) ? invalidOrNothingBrush : (translatedCount == totalValidCount ? translatedBrush : notTranslatedBrush)), e.Node.Bounds);
						TextRenderer.DrawText(e.Graphics, e.Node.Text, font, textBounds, SystemColors.ControlText, textFormatFlags);
						TextRenderer.DrawText(e.Graphics, countNoteText, font, countNoteTextBounds, ControlPaint.Dark((translatedCount == 0 && totalValidCount != 0) ? invalidOrNothingColor : (translatedCount == totalValidCount ? translatedColor : notTranslatedColor)), textFormatFlags);
					}
					else
					{
						e.Graphics.FillRectangle((unfocused ? SystemBrushes.Control : SystemBrushes.Highlight), e.Node.Bounds);
						TextRenderer.DrawText(e.Graphics, e.Node.Text, font, textBounds, (unfocused ? SystemColors.ControlText : SystemColors.HighlightText), textFormatFlags);
						TextRenderer.DrawText(e.Graphics, countNoteText, font, countNoteTextBounds, (unfocused ? ControlPaint.Dark(SystemColors.Control) : SystemColors.Highlight), textFormatFlags);
					}
				}
				else if (selected && unfocused)
				{
					e.Graphics.FillRectangle(SystemBrushes.Control, e.Node.Bounds);
					TextRenderer.DrawText(e.Graphics, e.Node.Text, font, textBounds, SystemColors.ControlText, textFormatFlags);
				}
				else if (!selected)
				{
					e.Graphics.FillRectangle(SystemBrushes.Window, e.Node.Bounds);
					TextRenderer.DrawText(e.Graphics, e.Node.Text, font, textBounds, SystemColors.WindowText, textFormatFlags);
				}
				else
				{
					e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Node.Bounds);
					TextRenderer.DrawText(e.Graphics, e.Node.Text, font, textBounds, SystemColors.HighlightText, textFormatFlags);
				}

				if (selected)
					ControlPaint.DrawFocusRectangle(e.Graphics, e.Node.Bounds);
			};

			lbMessages.SelectedIndexChanged += (s, e) =>
			{
				if (lbMessages.SelectedItem != null)
				{
					(lbMessages.SelectedItem as TranslatableEntry).PropertyChanged -= MainForm_PropertyChanged;
					(lbMessages.SelectedItem as TranslatableEntry).PropertyChanged += MainForm_PropertyChanged;
				}

				UpdateTextEditor();
			};
			lbMessages.DrawItem += (s, e) =>
			{
				if (e.Index == -1) return;

				var entry = ((s as ListBox).Items[e.Index] as TranslatableEntry);

				var selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);
				var unfocused = (!(s as ListBox).Focused);
				var font = (e.Font ?? (s as ListBox).Font);

				var textFormatFlags = (TextFormatFlags.Left | TextFormatFlags.WordEllipsis);

				var label = ((entry.ID == -1) ? "(Invalid)" : $"#{entry.ID:D3}: {entry.Original.TrimEnd(Environment.NewLine.ToCharArray()).Replace(Environment.NewLine, " ")}");

				if (!selected)
				{
					e.Graphics.FillRectangle(((entry.ID == -1) ? invalidOrNothingBrush : (entry.IsIgnored ? ignoredBrush : ((string.Compare(entry.Original, entry.Translation) == 0) ? notTranslatedBrush : translatedBrush))), e.Bounds);
					TextRenderer.DrawText(e.Graphics, label, e.Font, e.Bounds, SystemColors.ControlText, textFormatFlags);
				}
				else
				{
					e.Graphics.FillRectangle((unfocused ? SystemBrushes.Control : SystemBrushes.Highlight), e.Bounds);
					TextRenderer.DrawText(e.Graphics, label, font, e.Bounds, (unfocused ? SystemColors.ControlText : SystemColors.HighlightText), textFormatFlags);
				}

				if (selected)
					ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds);
			};

			cmbFont.SelectedIndexChanged += (s, e) =>
			{
				UpdateTextEditor();
			};

			cmbMarkerWidths.SelectedIndexChanged += (s, e) =>
			{
				UpdateTextEditor();
			};
			cmbMarkerWidths.DataSource = new BindingSource(new Dictionary<string, int>() { { "[None]", 0 } }.Concat(config.TextMarkerWidths.OrderBy(x => x.Key)), null);
			cmbMarkerWidths.DisplayMember = "Key";
			cmbMarkerWidths.ValueMember = "Value";

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

			EnableDisableUI(false);

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

				substListsAssigned = false;
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

					EnableDisableUI(dataLoaded && fontsReady);
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

			substListsAssigned = true;
		}

		private void UpdateTextEditor()
		{
			if (!fontsReady) return;

			textEditorControl.DataBindings.Clear();

			if (lbMessages.SelectedItem != null)
				textEditorControl.DataBindings.Add(nameof(textEditorControl.TranslatableEntry), lbMessages, nameof(lbMessages.SelectedItem), false, DataSourceUpdateMode.OnPropertyChanged);

			if (cmbFont.SelectedItem != null)
				textEditorControl.DataBindings.Add(nameof(textEditorControl.GameRenderer), cmbFont, nameof(cmbFont.SelectedItem), false, DataSourceUpdateMode.OnPropertyChanged);

			if (cmbMarkerWidths.SelectedItem != null)
				textEditorControl.DataBindings.Add(nameof(textEditorControl.LimitMarkerWidth), cmbMarkerWidths, nameof(cmbMarkerWidths.SelectedItem), false, DataSourceUpdateMode.OnPropertyChanged);

			textEditorControl.DataBindings.Add(nameof(textEditorControl.OverridesEnabled), config, nameof(config.OverridesEnabled), false, DataSourceUpdateMode.OnPropertyChanged);

			textEditorControl.Invalidate();
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
			EnableDisableUI(false);

			config.JsonRootDirectory = rootDirectory;

			enemyNames = LoadStringSubstJson(Path.Combine(config.JsonRootDirectory, @"Monster\Table\enemynametable.json"));
			skyItemNames = LoadStringSubstJson(Path.Combine(config.JsonRootDirectory, @"Item\skyitemname.json"));
			limitItemNames = LoadStringSubstJson(Path.Combine(config.JsonRootDirectory, @"Item\limititemnametable.json"));
			useItemNames = LoadStringSubstJson(Path.Combine(config.JsonRootDirectory, @"Item\useitemnametable.json"));
			equipItemNames = LoadStringSubstJson(Path.Combine(config.JsonRootDirectory, @"Item\equipitemnametable.json"));

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

					if (!translationFilesOpened.ContainsKey(path))
						translationFilesOpened[path] = translation;

					if (!substListsAssigned)
						SetRendererSubstitutionLists();

					currentFilePath = path;
					currentTranslationFile = translation;
					lbMessages.DataSource = currentTranslationFile.Entries;

					var matchingMarkerWidths = config.TextMarkerWidths.Where(x => x.Key.StartsWith(Path.GetFileNameWithoutExtension(currentTranslationFile.RelativePath)));
					if (matchingMarkerWidths != null)
						cmbMarkerWidths.SelectedItem = matchingMarkerWidths.FirstOrDefault();

					UpdateInfoLabel();
				}
			};

			translationFilesOpened = new Dictionary<string, Translation>();

			dataLoaded = true;

			UpdateFormTitle();

			EnableDisableUI(dataLoaded && fontsReady);
		}

		private void EnableDisableUI(bool state)
		{
			saveToolStripMenuItem.Enabled = saveAllToolStripMenuItem.Enabled =
				tvTextFiles.Enabled = lbMessages.Enabled =
				cmbFont.Enabled = lblFont.Enabled = cmbMarkerWidths.Enabled = lblMarkerWidths.Enabled =
				textEditorControl.Enabled = state;
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

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveData(true);
		}

		private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveData(false);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void characterOverridesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var characterOverrideForm = new CharacterOverrideForm(config.CharacterOverrides);
			characterOverrideForm.ShowDialog();

			textEditorControl.Invalidate();
		}

		private void fontPathsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var fontPathsForm = new FontPathsForm(config.FontPaths);
			fontPathsForm.ShowDialog();

			cmbFont.DataSource = null;

			CreateRenderers();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var description = System.Reflection.Assembly.GetExecutingAssembly().GetAttribute<System.Reflection.AssemblyDescriptionAttribute>().Description;
			var copyright = System.Reflection.Assembly.GetExecutingAssembly().GetAttribute<System.Reflection.AssemblyCopyrightAttribute>().Copyright;
			var version = new Version(Application.ProductVersion);

			GUIHelper.ShowInformationMessage("About", $"About {Application.ProductName}", $"{Application.ProductName} v{version.Major}.{version.Minor} - {description}\n\n{copyright.Replace(" - ", Environment.NewLine)}");
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			CreateRenderers();

			var args = CommandLineTools.CreateArgs(Environment.CommandLine);
			if (args.Length > 1 && Directory.Exists(args[1]))
				OpenData(args[1]);
		}

		private void MainForm_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			lbMessages.Invalidate();
			tvTextFiles.Invalidate();
			textEditorControl.Invalidate();

			UpdateInfoLabel();
		}

		private void SaveData(bool saveOnlyAccessedFiles)
		{
			if (saveOnlyAccessedFiles)
			{
				foreach (var pair in translationFilesOpened)
					pair.Value.SerializeToFile(pair.Key);
			}
			else
			{
				foreach (var node in FindAllTranslations(tvTextFiles.Nodes))
				{
					(string path, Translation translation) = (ValueTuple<string, Translation>)node.Tag;
					translation.SerializeToFile(path);
				}
			}

			config.SerializeToFile(programConfigPath);
		}

		private IEnumerable<TreeNode> FindAllTranslations(TreeNodeCollection nodes)
		{
			foreach (TreeNode node in nodes)
			{
				if (node.Tag is ValueTuple<string, Translation>)
					yield return node;
				foreach (var child in FindAllTranslations(node.Nodes))
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
