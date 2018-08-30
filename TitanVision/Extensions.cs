using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;

using Newtonsoft.Json;

namespace TitanVision
{
	public static class Extensions
	{
		public static T ReadStruct<T>(this BinaryReader reader)
		{
			byte[] bytes = reader.ReadBytes(Marshal.SizeOf<T>());

			GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
			T @struct = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
			handle.Free();

			return @struct;
		}

		public static T GetAttribute<T>(this ICustomAttributeProvider assembly, bool inherit = false) where T : Attribute
		{
			return assembly.GetCustomAttributes(typeof(T), inherit).OfType<T>().FirstOrDefault();
		}

		public static void SerializeToFile(this object obj, string jsonFileName)
		{
			using (var writer = new StreamWriter(jsonFileName))
			{
				writer.Write(JsonConvert.SerializeObject(obj, Formatting.Indented));
			}
		}

		public static T DeserializeFromFile<T>(this string jsonFileName)
		{
			using (var reader = new StreamReader(jsonFileName))
			{
				return (T)JsonConvert.DeserializeObject(reader.ReadToEnd(), typeof(T), new JsonSerializerSettings() { Formatting = Formatting.Indented });
			}
		}

		// https://derekwill.com/2014/06/24/combating-the-lag-of-the-winforms-progressbar/
		public static void SetProgressNoAnimation(this ToolStripProgressBar pb, int value)
		{
			if (value == pb.Maximum)
			{
				pb.Maximum = value + 1;
				pb.Value = value + 1;
				pb.Maximum = value;
			}
			else
			{
				pb.Value = value + 1;
			}
			pb.Value = value;
		}
	}
}
