using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TitanVision
{
	public class BCFNTReader
	{
		BCFNT_Header header;

		public BCFNT_FontInformation FontInformationBlock { get; private set; }
		public BCFNT_TextureGlyphBlock TextureGlyphBlock { get; private set; }

		readonly List<BCFNT_FontCharacterWidthBlock> fontCharacterWidthBlocks;
		readonly List<BCFNT_FontCodeMapBlock> fontCodeMapBlocks;

		public Dictionary<ushort, BCFNT_CharWidth> CharacterWidths { get; private set; }
		public Dictionary<ushort, ushort> CodeMap { get; private set; }

		public List<Bitmap> TextureSheets { get; private set; }

		public BCFNTReader(string fileName)
		{
			using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				using (BinaryReader reader = new BinaryReader(stream))
				{
					header = reader.ReadStruct<BCFNT_Header>();
					FontInformationBlock = reader.ReadStruct<BCFNT_FontInformation>();

					stream.Seek(FontInformationBlock.FontGlyphPointer - BCFNT_BlockHeader.SizeOf, SeekOrigin.Begin);
					TextureGlyphBlock = reader.ReadStruct<BCFNT_TextureGlyphBlock>();

					CharacterWidths = new Dictionary<ushort, BCFNT_CharWidth>();
					fontCharacterWidthBlocks = ReadBlocks<BCFNT_FontCharacterWidthBlock>(reader, FontInformationBlock.FontWidthPointer, ReadCharacterWidths);

					CodeMap = new Dictionary<ushort, ushort>();
					fontCodeMapBlocks = ReadBlocks<BCFNT_FontCodeMapBlock>(reader, FontInformationBlock.FontCodeMapPointer, ReadCodeMaps);

					TextureSheets = new List<Bitmap>();
					ReadTextureSheets(reader);
				}
			}
		}

		private List<T> ReadBlocks<T>(BinaryReader reader, long startOffset, Action<BinaryReader, IBCFNT_ContinueBlock> action) where T : IBCFNT_ContinueBlock
		{
			List<T> blocks = new List<T>();
			for (var nextPointer = startOffset; (nextPointer != 0);)
			{
				reader.BaseStream.Seek(nextPointer - BCFNT_BlockHeader.SizeOf, SeekOrigin.Begin);
				T block = reader.ReadStruct<T>();
				action?.Invoke(reader, block);

				nextPointer = block.GetNextPointer();
				blocks.Add(block);
			}
			return blocks;
		}

		private void ReadCharacterWidths(BinaryReader reader, IBCFNT_ContinueBlock block)
		{
			var charWidthBlock = (BCFNT_FontCharacterWidthBlock)block;
			for (ushort index = charWidthBlock.IndexBegin; index <= charWidthBlock.IndexEnd; index++)
				CharacterWidths.Add(index, reader.ReadStruct<BCFNT_CharWidth>());
		}

		private void ReadCodeMaps(BinaryReader reader, IBCFNT_ContinueBlock block)
		{
			var codeMapBlock = (BCFNT_FontCodeMapBlock)block;
			{
				switch (codeMapBlock.MappingMethod)
				{
					case BCFNT_FontMapMethod.Direct:
						var directMappingOffset = reader.ReadUInt16();
						for (int i = codeMapBlock.CCodeBegin; i <= codeMapBlock.CCodeEnd; i++)
							CodeMap.Add((ushort)i, (ushort)((i - codeMapBlock.CCodeBegin) + directMappingOffset));
						break;

					case BCFNT_FontMapMethod.Table:
						for (int i = codeMapBlock.CCodeBegin; i <= codeMapBlock.CCodeEnd; i++)
							CodeMap.Add((ushort)i, reader.ReadUInt16());
						break;

					case BCFNT_FontMapMethod.Scan:
						var numEntries = reader.ReadUInt16();
						for (int i = 0; i < numEntries; i++)
							CodeMap.Add(reader.ReadUInt16(), reader.ReadUInt16());
						break;
				}
			}
		}

		private void ReadTextureSheets(BinaryReader reader)
		{
			if ((TextureGlyphBlock.SheetFormat & BCFNT_FontSheetFormat.FormatMask) != BCFNT_FontSheetFormat.FormatCompressedFlag)
			{
				reader.BaseStream.Seek(TextureGlyphBlock.SheetImagePointer, SeekOrigin.Begin);
				for (int i = 0; i < TextureGlyphBlock.NumSheets; i++)
				{
					var sheetData = reader.ReadBytes((int)TextureGlyphBlock.SheetSize);
					TextureSheets.Add(TextureHelper.GetBitmap(TextureGlyphBlock.SheetWidth, TextureGlyphBlock.SheetHeight, (ImagePixelFormat)TextureGlyphBlock.SheetFormat, sheetData));
				}
			}
			else
				throw new Exception("TODO: read compressed texture sheets");
		}
	}

	public enum BCFNT_FontType : byte
	{
		Glyph,
		Texture
	};

	public enum BCFNT_CharacterCode : byte
	{
		Unicode = 1,
		SJIS,
		CP1252
	};

	public enum BCFNT_FontMapMethod : ushort
	{
		Direct,
		Table,
		Scan
	};

	public enum BCFNT_FontSheetFormat : ushort
	{
		RGBA8,
		RGB8,
		RGB5A1,
		RGB565,
		RGBA4,
		LA8,
		A8 = 8,
		LA4,
		A4 = 11,
		FormatMask = 0x7FFF,
		FormatCompressedFlag = 0x8000
	};

	[StructLayout(LayoutKind.Sequential)]
	struct BCFNT_Header
	{
		public static int SizeOf = Marshal.SizeOf<BCFNT_Header>();

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public char[] Signature;
		public ushort ByteOrder;
		public ushort HeaderSize;
		public uint Version;
		public uint FileSize;
		public ushort DataBlocks;
		public ushort Reserved;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct BCFNT_BlockHeader
	{
		public static int SizeOf = Marshal.SizeOf<BCFNT_BlockHeader>();

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public char[] Signature;
		public uint BlockSize;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct BCFNT_CharWidth
	{
		public static int SizeOf = Marshal.SizeOf<BCFNT_CharWidth>();

		public sbyte Left;
		public byte GlyphWidth;
		public sbyte CharWidth;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct BCFNT_FontInformation
	{
		public static int SizeOf = Marshal.SizeOf<BCFNT_FontInformation>();

		public BCFNT_BlockHeader BlockHeader;
		public BCFNT_FontType FontType;
		public sbyte LineFeed;
		public ushort AlterCharIndex;
		public BCFNT_CharWidth DefaultWidth;
		public BCFNT_CharacterCode CharacterCode;
		public uint FontGlyphPointer;
		public uint FontWidthPointer;
		public uint FontCodeMapPointer;
		public byte Height;
		public byte Width;
		public byte Ascend;
		public byte Padding;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct BCFNT_TextureGlyphBlock
	{
		public static int SizeOf = Marshal.SizeOf<BCFNT_TextureGlyphBlock>();

		public BCFNT_BlockHeader BlockHeader;
		public byte CellWidth;
		public byte CellHeight;
		public sbyte BaselinePos;
		public byte MaxCharWidth;
		public uint SheetSize;
		public ushort NumSheets;
		public BCFNT_FontSheetFormat SheetFormat;
		public ushort NumSheetColumns;
		public ushort NumSheetRows;
		public ushort SheetWidth;
		public ushort SheetHeight;
		public uint SheetImagePointer;
	}

	interface IBCFNT_ContinueBlock { uint GetNextPointer(); }

	[StructLayout(LayoutKind.Sequential)]
	struct BCFNT_FontCharacterWidthBlock : IBCFNT_ContinueBlock
	{
		public static int SizeOf = Marshal.SizeOf<BCFNT_FontCharacterWidthBlock>();

		public BCFNT_BlockHeader BlockHeader;
		public ushort IndexBegin;
		public ushort IndexEnd;
		public uint NextPointer;

		public uint GetNextPointer() => NextPointer;
	}

	[StructLayout(LayoutKind.Sequential)]
	struct BCFNT_FontCodeMapBlock : IBCFNT_ContinueBlock
	{
		public static int SizeOf = Marshal.SizeOf<BCFNT_FontCodeMapBlock>();

		public BCFNT_BlockHeader BlockHeader;
		public ushort CCodeBegin;
		public ushort CCodeEnd;
		public BCFNT_FontMapMethod MappingMethod;
		public ushort Reserved;
		public uint NextPointer;

		public uint GetNextPointer() => NextPointer;
	}
}
