using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace TitanVision.Helpers
{
	// TODO: taken from 2015 Berund2, maybe overhaul?

	public enum ImagePixelFormat : uint
	{
		RGBA8,
		RGB8,
		RGB5A1,
		RGB565,
		RGBA4,
		LA8,
		RG8,
		L8,
		A8,
		LA4,
		L4,
		A4,
		ETC1,
		ETC1A4
	}

	public static class TextureHelper
	{
		static Dictionary<ImagePixelFormat, Action<BinaryReader, byte[], int>> pixelDecoders = new Dictionary<ImagePixelFormat, Action<BinaryReader, byte[], int>>()
		{
			{ ImagePixelFormat.RGBA8, DecodeRGBA8 },
			{ ImagePixelFormat.RGB8, DecodeRGB8 },
			{ ImagePixelFormat.RGB565, DecodeRGB565 },
			{ ImagePixelFormat.RGB5A1, DecodeRGB5A1 },
			{ ImagePixelFormat.A8, DecodeA8 },
			{ ImagePixelFormat.L8, DecodeL8 },
		};

		static readonly int[] tileOrder =
		{
			0, 1, 8, 9,
			2, 3, 10, 11,
			16, 17, 24, 25,
			18, 19, 26, 27,

			4, 5, 12, 13,
			6, 7, 14, 15,
			20, 21, 28, 29,
			22, 23, 30, 31,

			32, 33, 40, 41,
			34, 35, 42, 43,
			48, 49, 56, 57,
			50, 51, 58, 59,

			36, 37, 44, 45,
			38, 39, 46, 47,
			52, 53, 60, 61,
			54, 55, 62, 63
		};

		static readonly int[] convert5To8 =
		{
			0x00, 0x08, 0x10, 0x18, 0x20, 0x29, 0x31, 0x39,
			0x41, 0x4A, 0x52, 0x5A, 0x62, 0x6A, 0x73, 0x7B,
			0x83, 0x8B, 0x94, 0x9C, 0xA4, 0xAC, 0xB4, 0xBD,
			0xC5, 0xCD, 0xD5, 0xDE, 0xE6, 0xEE, 0xF6, 0xFF
		};

		public static Bitmap GetBitmap(int width, int height, ImagePixelFormat format, byte[] data)
		{
			Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

			if (!pixelDecoders.ContainsKey(format)) throw new NotImplementedException(string.Format("Unimplemented format {0}", format));

			using (BinaryReader reader = new BinaryReader(new MemoryStream(data)))
			{
				if (format == ImagePixelFormat.ETC1 || format == ImagePixelFormat.ETC1A4)
				{
					throw new NotImplementedException("FIXME: ETC1/ETC1A4 not implemented");
				}
				else
				{
					BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
					byte[] pixelData = new byte[bmpData.Height * bmpData.Stride];
					Marshal.Copy(bmpData.Scan0, pixelData, 0, pixelData.Length);

					for (int y = 0; y < bitmap.Height; y += 8)
						for (int x = 0; x < bitmap.Width; x += 8)
							for (int t = 0; t < tileOrder.Length; t++)
								pixelDecoders[format](reader, pixelData, (int)(((((tileOrder[t] / 8) + y) * bitmap.Width) + ((tileOrder[t] % 8) + x)) * 4));

					Marshal.Copy(pixelData, 0, bmpData.Scan0, pixelData.Length);
					bitmap.UnlockBits(bmpData);
				}
			}

			return bitmap;
		}

		private static void DecodeRGBA8(BinaryReader reader, byte[] pixelData, int pixelOffset)
		{
			pixelData[pixelOffset + 3] = reader.ReadByte();
			pixelData[pixelOffset + 0] = reader.ReadByte();
			pixelData[pixelOffset + 1] = reader.ReadByte();
			pixelData[pixelOffset + 2] = reader.ReadByte();
		}

		private static void DecodeRGB8(BinaryReader reader, byte[] pixelData, int pixelOffset)
		{
			pixelData[pixelOffset + 3] = 0xFF;
			pixelData[pixelOffset + 0] = reader.ReadByte();
			pixelData[pixelOffset + 1] = reader.ReadByte();
			pixelData[pixelOffset + 2] = reader.ReadByte();
		}

		private static void DecodeRGB565(BinaryReader reader, byte[] pixelData, int pixelOffset)
		{
			ushort rgb = reader.ReadUInt16();
			pixelData[pixelOffset + 3] = 0xFF;
			pixelData[pixelOffset + 0] = (byte)convert5To8[rgb & 0x1F];
			pixelData[pixelOffset + 1] = (byte)(((rgb >> 5) & 0x3F) << 2);
			pixelData[pixelOffset + 2] = (byte)convert5To8[(rgb >> 11) & 0x1F];
		}

		private static void DecodeRGB5A1(BinaryReader reader, byte[] pixelData, int pixelOffset)
		{
			ushort rgba = reader.ReadUInt16();
			pixelData[pixelOffset + 3] = (byte)((rgba & 0x1) * 0xFF);
			pixelData[pixelOffset + 0] = (byte)convert5To8[(rgba >> 1) & 0x1F];
			pixelData[pixelOffset + 1] = (byte)convert5To8[(rgba >> 6) & 0x1F];
			pixelData[pixelOffset + 2] = (byte)convert5To8[(rgba >> 11) & 0x1F];
		}

		private static void DecodeA8(BinaryReader reader, byte[] pixelData, int pixelOffset)
		{
			pixelData[pixelOffset + 3] = reader.ReadByte();
			pixelData[pixelOffset + 0] = 0xFF;
			pixelData[pixelOffset + 1] = 0xFF;
			pixelData[pixelOffset + 2] = 0xFF;
		}

		private static void DecodeL8(BinaryReader reader, byte[] pixelData, int pixelOffset)
		{
			byte l = reader.ReadByte();
			pixelData[pixelOffset + 3] = 0xFF;//?
			pixelData[pixelOffset + 0] = l;
			pixelData[pixelOffset + 1] = l;
			pixelData[pixelOffset + 2] = l;
		}
	}
}
