using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace TitanVision
{
	public class GameRenderer
	{
		// TitanMessage copypasta

		// (also this whole thing is super-fragile, the string substitution part especially I'd say. PROCEED WITH CAUTION.)

		const char controlCodeBegin = '[', controlCodeEnd = ']', controlCodeArgSeperator = ':';
		const string controlCodePageBreak = "Page";

		readonly static Encoding shiftJisEncoding = Encoding.GetEncoding(932);

		readonly static Dictionary<char, char> shiftJisToAscii = new Dictionary<char, char>()
		{
			{ '　', ' ' }, { '，', ',' }, { '．', '.' }, { '：', ':' }, { '；', ';' }, { '？', '?' }, { '！', '!' }, { '－', '-' },
			{ '／', '/' }, { '～', '~' }, { '’', '\'' }, { '”', '\"' }, { '（', '(' }, { '）', ')' }, { '［', '[' }, { '］', ']' },
			{ '〈', '<' }, { '〉', '>' }, { '＋', '+' }, { '＊', '*' }, { '＆', '&' },

			{ '０', '0' }, { '１', '1' }, { '２', '2' }, { '３', '3' }, { '４', '4' }, { '５', '5' }, { '６', '6' }, { '７', '7' },
			{ '８', '8' }, { '９', '9' },

			{ 'Ａ', 'A' }, { 'Ｂ', 'B' }, { 'Ｃ', 'C' }, { 'Ｄ', 'D' }, { 'Ｅ', 'E' }, { 'Ｆ', 'F' }, { 'Ｇ', 'G' }, { 'Ｈ', 'H' },
			{ 'Ｉ', 'I' }, { 'Ｊ', 'J' }, { 'Ｋ', 'K' }, { 'Ｌ', 'L' }, { 'Ｍ', 'M' }, { 'Ｎ', 'N' }, { 'Ｏ', 'O' }, { 'Ｐ', 'P' },
			{ 'Ｑ', 'Q' }, { 'Ｒ', 'R' }, { 'Ｓ', 'S' }, { 'Ｔ', 'T' }, { 'Ｕ', 'U' }, { 'Ｖ', 'V' }, { 'Ｗ', 'W' }, { 'Ｘ', 'X' },
			{ 'Ｙ', 'Y' }, { 'Ｚ', 'Z' },

			{ 'ａ', 'a' }, { 'ｂ', 'b' }, { 'ｃ', 'c' }, { 'ｄ', 'd' }, { 'ｅ', 'e' }, { 'ｆ', 'f' }, { 'ｇ', 'g' }, { 'ｈ', 'h' },
			{ 'ｉ', 'i' }, { 'ｊ', 'j' }, { 'ｋ', 'k' }, { 'ｌ', 'l' }, { 'ｍ', 'm' }, { 'ｎ', 'n' }, { 'ｏ', 'o' }, { 'ｐ', 'p' },
			{ 'ｑ', 'q' }, { 'ｒ', 'r' }, { 'ｓ', 's' }, { 'ｔ', 't' }, { 'ｕ', 'u' }, { 'ｖ', 'v' }, { 'ｗ', 'w' }, { 'ｘ', 'x' },
			{ 'ｙ', 'y' }, { 'ｚ', 'z' },
		};
		readonly static Dictionary<char, char> asciiToShiftJis = shiftJisToAscii.ToDictionary(x => x.Value, x => x.Key);

		readonly static Dictionary<string, Func<string, string>> preProcessFunctions = new Dictionary<string, Func<string, string>>()
		{
			{ "Enemy1", PreProcessEnemy },
			{ "Item", PreProcessItem },
		};
		readonly static Dictionary<string, List<string>> preProcessSubstitutionList;

		readonly static Dictionary<string, Action<Graphics, string>> postProcessFunctions = new Dictionary<string, Action<Graphics, string>>()
		{
			{ "Color", PostProcessColor },
		};

		static ImageAttributes imageAttributes;
		static float[][] colorMatrix;

		readonly Dictionary<ushort, (Bitmap Image, sbyte Left, byte GlyphWidth)> charMap;
		readonly (int Width, int Height) cellSize;

		public string FontName { get; private set; }

		static GameRenderer()
		{
			ResetRenderMisc();

			preProcessSubstitutionList = new Dictionary<string, List<string>>();
		}

		public GameRenderer(string fileName)
		{
			FontName = System.IO.Path.GetFileNameWithoutExtension(fileName);

			var bcfnt = new BCFNTReader(fileName);

			cellSize = ((bcfnt.TextureGlyphBlock.CellWidth + 1), (bcfnt.TextureGlyphBlock.CellHeight + 1));

			var numCells = (bcfnt.TextureGlyphBlock.NumSheetColumns * bcfnt.TextureGlyphBlock.NumSheetRows);

			charMap = new Dictionary<ushort, (Bitmap Image, sbyte Left, byte GlyphWidth)>();

			foreach (var code in bcfnt.CodeMap)
			{
				if (!bcfnt.CharacterWidths.ContainsKey(code.Value)) continue;

				var sheetNumber = (code.Value / numCells);
				var sheetYPos = ((code.Value % numCells) / bcfnt.TextureGlyphBlock.NumSheetColumns) * cellSize.Height;
				var sheetXPos = (code.Value % bcfnt.TextureGlyphBlock.NumSheetColumns) * cellSize.Width;
				var charWidths = bcfnt.CharacterWidths[code.Value];

				var image = new Bitmap(cellSize.Width, cellSize.Height);
				using (Graphics g = Graphics.FromImage(image))
				{
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

					g.DrawImage(bcfnt.TextureSheets[sheetNumber], 0, 0, new Rectangle(sheetXPos, sheetYPos, cellSize.Width, cellSize.Height), GraphicsUnit.Pixel);
				}

				charMap.Add(code.Key, (image, charWidths.Left, charWidths.GlyphWidth));
			}
		}

		public void SetSubstitutionList(string code, List<string> strings)
		{
			preProcessSubstitutionList[code] = strings;
		}

		private static void ResetRenderMisc()
		{
			imageAttributes = new ImageAttributes();
			colorMatrix = new float[][]
			{
				new float[] { 0, 0, 0, 0, 0 },
				new float[] { 0, 0, 0, 0, 0 },
				new float[] { 0, 0, 0, 0, 0 },
				new float[] { 0, 0, 0, 1, 0 },
				new float[] { 0.1f, 0.2f, 0.1f, 0, 1 }
			};
			imageAttributes.SetColorMatrix(new ColorMatrix(colorMatrix), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
		}

		public Bitmap GetBitmap(string str)
		{
			if (str == null || str.Length == 0)
				return new Bitmap(32, 32, PixelFormat.Format32bppArgb);

			var image = new Bitmap(2048, Math.Max(1, (str.Length - str.Replace(Environment.NewLine, "").Length)) * cellSize.Height, PixelFormat.Format32bppArgb);
			using (Graphics g = Graphics.FromImage(image))
			{
				DrawString(g, str);
			}

			// TODO: maybe make less inefficient, I guess? tho it works so, eh, dunno

			BitmapData bmpData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
			byte[] pixelData = new byte[bmpData.Height * bmpData.Stride];
			Marshal.Copy(bmpData.Scan0, pixelData, 0, pixelData.Length);

			int bpp = (Image.GetPixelFormatSize(image.PixelFormat) / 8);

			int[] widths = new int[image.Height];
			List<int> emptyLines = new List<int>();
			int croppedHeight = 0;

			for (int y = (image.Height - 1); y >= 0; y--)
			{
				for (int x = (image.Width - 1); x >= 0; x--)
				{
					int ofs = (y * bmpData.Stride) + (x * bpp);
					if (pixelData[ofs] > 0)
					{
						widths[y] = Math.Max(widths[y], x);
						break;
					}
				}

				if (widths[y] == 0)
					emptyLines.Add(y);
			}

			for (int i = 0; i < (emptyLines.Count - 1); i++)
			{
				if (emptyLines[i] != emptyLines[i + 1] + 1)
				{
					croppedHeight = emptyLines[i];
					break;
				}
			}

			Marshal.Copy(pixelData, 0, bmpData.Scan0, pixelData.Length);
			image.UnlockBits(bmpData);

			Bitmap cropped = new Bitmap(widths.Max() + 4, croppedHeight + 4, image.PixelFormat);
			using (Graphics g = Graphics.FromImage(cropped))
			{
				g.DrawImageUnscaled(image, Point.Empty);
			}

			return cropped;
		}

		// this function is voodoo and I don't mean 3dfx

		public void DrawString(Graphics g, string str)
		{
			ResetRenderMisc();

			for (int idx = 0, xPos = 0, yPos = 0; idx < str.Length; idx++)
			{
				var chrAscii = str[idx];
				if (str[idx] == controlCodeBegin)
				{
					var controlCodeLen = GetControlCodeLength(str, idx + 1);
					if (controlCodeLen != -1)
					{
						var controlCode = str.Substring(idx + 1, controlCodeLen);
						var codeString = GetControlCode(controlCode);
						var argString = GetControlCodeArgument(controlCode);

						if (codeString == controlCodePageBreak)
						{
							yPos += cellSize.Height;
							xPos = 0;
							idx += ((controlCodeLen + (Environment.NewLine.Length * 2)) + 1);
							continue;
						}
						else if (preProcessFunctions.ContainsKey(codeString))
						{
							var tmp = preProcessFunctions[codeString](argString);
							str = str.Remove(idx, controlCodeLen + 2).Insert(idx, tmp);
							idx--;
							continue;
						}
						else if (postProcessFunctions.ContainsKey(codeString))
						{
							postProcessFunctions[codeString](g, argString);
							idx += (controlCodeLen + 1);
							continue;
						}
					}
				}
				else if (chrAscii == '\r')
				{
					continue;
				}
				else if (chrAscii == '\n')
				{
					yPos += cellSize.Height;
					xPos = 0;
					continue;
				}

				var chr = Encoding.GetEncoding(932).GetBytes(new char[] { (asciiToShiftJis.ContainsKey(chrAscii) ? asciiToShiftJis[chrAscii] : chrAscii) });
				if (chr.Length != 2) chr = new byte[] { 0x81, 0x48 };
				if (chr[0] == 0x81 && chr[1] == 0x40) { chr[0] = 0x00; chr[1] = 0x20; }

				var chrCode = (ushort)(chr[0] << 8 | chr[1]);
				var chrInfo = charMap[chrCode];

				xPos += chrInfo.Left;
				g.DrawImage(chrInfo.Image, new Rectangle(xPos, yPos, chrInfo.Image.Width, chrInfo.Image.Height),
					0, 0, chrInfo.Image.Width, chrInfo.Image.Height,
					GraphicsUnit.Pixel, imageAttributes);
				xPos += chrInfo.GlyphWidth;
			}
		}

		// Partial Berund2 copypasta

		private static void PostProcessColor(Graphics g, string args)
		{
			ushort.TryParse(args, out ushort color);
			switch (color)
			{
				/* Blue */
				case 0x0003: UpdateColorMatrix(new int[] { 4, 4, 4 }, new int[] { 0, 1, 2 }, new float[] { 0.1f, 0.1f, 1.0f }); break;
				/* Red */
				case 0x0005: UpdateColorMatrix(new int[] { 4, 4, 4 }, new int[] { 0, 1, 2 }, new float[] { 1.0f, 0.25f, 0.25f }); break;
				/* Green */
				case 0x0006: UpdateColorMatrix(new int[] { 4, 4, 4 }, new int[] { 0, 1, 2 }, new float[] { 0.25f, 0.75f, 0.25f }); break;
				/* Yellow */
				case 0x0008: UpdateColorMatrix(new int[] { 4, 4, 4 }, new int[] { 0, 1, 2 }, new float[] { 1.0f, 1.0f, 0.1f }); break;
				/* Dark green */
				case 0x000A: UpdateColorMatrix(new int[] { 4, 4, 4 }, new int[] { 0, 1, 2 }, new float[] { 0.1f, 0.5f, 0.1f }); break;
				/* Dark red */
				case 0x000B: UpdateColorMatrix(new int[] { 4, 4, 4 }, new int[] { 0, 1, 2 }, new float[] { 0.5f, 0.1f, 0.1f }); break;

				/* Default */
				case 0x0000:
				default: UpdateColorMatrix(new int[] { 4, 4, 4 }, new int[] { 0, 1, 2 }, new float[] { 0.1f, 0.2f, 0.1f }); break;
			}
		}

		private static string PreProcessEnemy(string args)
		{
			ushort.TryParse(args, out ushort enemyId);
			return preProcessSubstitutionList["Enemy"][enemyId];
		}

		private static string PreProcessItem(string args)
		{
			ushort.TryParse(args, out ushort itemId);
			if (itemId >= 2048)
			{
				/* Sky items (food, ship equip) */
				return preProcessSubstitutionList["SkyItems"][itemId - 2048];
			}
			else if (itemId >= 1599)
			{
				/* Burst skills */
				return preProcessSubstitutionList["LimitItems"][itemId - 1599];
			}
			else if (itemId >= 924)
			{
				/* Monster drops, useable items, etc */
				return preProcessSubstitutionList["UseItems"][itemId - 924];
			}
			else
			{
				/* Equipment etc */
				return preProcessSubstitutionList["EquipItems"][itemId - 0];
			}
		}

		private static void UpdateColorMatrix(int[] columns, int[] rows, float[] values)
		{
			if (columns.Length != values.Length || rows.Length != values.Length) throw new ArgumentException("Array size mismatch");
			for (int i = 0; i < values.Length; i++)
			{
				colorMatrix[columns[i]][rows[i]] = values[i];
			}
			imageAttributes.SetColorMatrix(new ColorMatrix(colorMatrix), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
		}

		// More TitanMessage copypasta

		private int GetControlCodeLength(string str, int idx)
		{
			var length = 0;
			for (; idx < str.Length; idx++)
			{
				if (str[idx] == controlCodeBegin) break;
				if (str[idx] == controlCodeEnd) return length;
				length++;
			}
			return -1;
		}

		private string GetControlCode(string code)
		{
			var argIdx = code.IndexOf(controlCodeArgSeperator);
			if (argIdx == -1) return code;
			return code.Substring(0, argIdx);
		}

		private string GetControlCodeArgument(string code)
		{
			var argIdx = code.IndexOf(controlCodeArgSeperator);
			if (argIdx == -1) return string.Empty;
			return code.Substring(argIdx + 1);
		}
	}
}
