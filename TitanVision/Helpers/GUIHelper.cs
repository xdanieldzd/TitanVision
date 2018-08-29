﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace TitanVision.Helpers
{
	public static class GUIHelper
	{
		// Yggdrasil copypasta

		public static void ShowErrorMessage(string caption, string instructionText, string text)
		{
			ShowErrorMessage(caption, instructionText, text, string.Empty, string.Empty, IntPtr.Zero);
		}

		public static void ShowErrorMessage(string caption, string instructionText, string text, string footerText)
		{
			ShowErrorMessage(caption, instructionText, text, footerText, string.Empty, IntPtr.Zero);
		}

		public static void ShowErrorMessage(string caption, string instructionText, string text, string footerText, string details)
		{
			ShowErrorMessage(caption, instructionText, text, footerText, details, IntPtr.Zero);
		}

		public static void ShowErrorMessage(string caption, string instructionText, string text, IntPtr ownerHandle)
		{
			ShowErrorMessage(caption, instructionText, text, string.Empty, string.Empty, ownerHandle);
		}

		public static void ShowErrorMessage(string caption, string instructionText, string text, string footerText, IntPtr ownerHandle)
		{
			ShowErrorMessage(caption, instructionText, text, footerText, string.Empty, ownerHandle);
		}

		public static void ShowErrorMessage(string caption, string instructionText, string text, string footerText, string details, IntPtr ownerHandle)
		{
			if (TaskDialog.IsPlatformSupported)
			{
				TaskDialog dialog = new TaskDialog
				{
					OwnerWindowHandle = ownerHandle,

					DetailsExpanded = false,
					Cancelable = false,
					Icon = TaskDialogStandardIcon.Error,
					FooterIcon = TaskDialogStandardIcon.Error,
					ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter,
					StartupLocation = TaskDialogStartupLocation.CenterOwner,

					Caption = caption,
					InstructionText = instructionText,
					Text = text,
					DetailsExpandedText = details,
					FooterText = footerText
				};

				dialog.Opened += ((s, e) =>
				{
					TaskDialog taskDialog = (s as TaskDialog);
					taskDialog.Icon = taskDialog.Icon;
					if (dialog.FooterText != string.Empty) taskDialog.FooterIcon = taskDialog.FooterIcon;
					taskDialog.InstructionText = taskDialog.InstructionText;
				});

				dialog.Show();
			}
			else
			{
				string message = string.Format("{1} {2}{0}{0}{3}", Environment.NewLine, instructionText, footerText, text);
				if (details != string.Empty)
				{
					message += string.Format("{0}{0}Do you want to see the error details?", Environment.NewLine);
					if (MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						MessageBox.Show(details, "Error Details", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
				else
					MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public static void ShowInformationMessage(string caption, string instructionText, string text)
		{
			ShowInformationMessage(caption, instructionText, text, string.Empty, string.Empty, IntPtr.Zero);
		}

		public static void ShowInformationMessage(string caption, string instructionText, string text, string footerText)
		{
			ShowInformationMessage(caption, instructionText, text, footerText, string.Empty, IntPtr.Zero);
		}

		public static void ShowInformationMessage(string caption, string instructionText, string text, string footerText, string details)
		{
			ShowInformationMessage(caption, instructionText, text, footerText, details, IntPtr.Zero);
		}

		public static void ShowInformationMessage(string caption, string instructionText, string text, IntPtr ownerHandle)
		{
			ShowInformationMessage(caption, instructionText, text, string.Empty, string.Empty, ownerHandle);
		}

		public static void ShowInformationMessage(string caption, string instructionText, string text, string footerText, IntPtr ownerHandle)
		{
			ShowInformationMessage(caption, instructionText, text, footerText, string.Empty, ownerHandle);
		}

		public static void ShowInformationMessage(string caption, string instructionText, string text, string footerText, string details, IntPtr ownerHandle)
		{
			if (TaskDialog.IsPlatformSupported)
			{
				TaskDialog dialog = new TaskDialog
				{
					OwnerWindowHandle = ownerHandle,

					DetailsExpanded = false,
					Cancelable = false,
					Icon = TaskDialogStandardIcon.Information,
					FooterIcon = TaskDialogStandardIcon.Information,
					ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter,
					StartupLocation = TaskDialogStartupLocation.CenterOwner,

					Caption = caption,
					InstructionText = instructionText,
					Text = text,
					DetailsExpandedText = details,
					FooterText = footerText
				};

				dialog.Opened += ((s, e) =>
				{
					TaskDialog taskDialog = (s as TaskDialog);
					taskDialog.Icon = taskDialog.Icon;
					if (dialog.FooterText != string.Empty) taskDialog.FooterIcon = taskDialog.FooterIcon;
					taskDialog.InstructionText = taskDialog.InstructionText;
				});

				dialog.Show();
			}
			else
			{
				string message = string.Format("{1}{0}{0}{2}", Environment.NewLine, instructionText, text);
				if (details != string.Empty)
				{
					message += string.Format("{0}{0}Do you want to see more details?", Environment.NewLine);
					if (MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						MessageBox.Show(details, "Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
				else
					MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		public static DialogResult ShowFolderBrowser(string title, string description, string initialPath, out string selectedPath)
		{
			if (CommonFileDialog.IsPlatformSupported)
			{
				CommonOpenFileDialog ofd = new CommonOpenFileDialog
				{
					IsFolderPicker = true,
					InitialDirectory = initialPath,
					Title = title
				};
				if (ofd.ShowDialog() == CommonFileDialogResult.Ok)
				{
					selectedPath = ofd.FileName;
					return DialogResult.OK;
				}
				else
				{
					selectedPath = string.Empty;
					return DialogResult.Cancel;
				}
			}
			else
			{
				FolderBrowserDialog fbd = new FolderBrowserDialog
				{
					Description = description,
					SelectedPath = initialPath
				};
				selectedPath = fbd.SelectedPath;
				return fbd.ShowDialog();
			}
		}

		public static DialogResult ShowFileOpenDialog(string title, string initialPath, string[] filters, out string selectedPath)
		{
			var initialDirectory = ((initialPath != null && initialPath != string.Empty) ? Path.GetDirectoryName(initialPath) : string.Empty);
			var initialFileName = ((initialPath != null && initialPath != string.Empty) ? Path.GetFileName(initialPath) : string.Empty);

			if (CommonFileDialog.IsPlatformSupported)
			{
				CommonFileDialog ofd = new CommonOpenFileDialog
				{
					InitialDirectory = initialDirectory,
					DefaultFileName = initialFileName,
					Title = title,
				};

				foreach (string filter in filters)
				{
					string[] components = filter.Split('|');
					if (components.Length == 2)
						ofd.Filters.Add(new CommonFileDialogFilter(components[0], components[1]) { ShowExtensions = true });
				}

				if (ofd.ShowDialog() == CommonFileDialogResult.Ok)
				{
					selectedPath = ofd.FileName;
					return DialogResult.OK;
				}
				else
				{
					selectedPath = string.Empty;
					return DialogResult.Cancel;
				}
			}
			else
			{
				OpenFileDialog ofd = new OpenFileDialog()
				{
					Title = title,
					InitialDirectory = initialPath
				};

				ofd.Filter = string.Join("|", filters);

				selectedPath = ofd.FileName;
				return ofd.ShowDialog();
			}
		}
	}
}
