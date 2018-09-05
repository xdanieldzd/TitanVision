using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TitanVision.DataStorage
{
	public class Configuration
	{
		public string JsonRootDirectory { get; set; }
		public List<string> FontPaths { get; set; }
		public bool OverridesEnabled { get; set; }
		public Dictionary<char, char> CharacterOverrides { get; set; }
		public Dictionary<string, int> TextMarkerWidths { get; set; }

		public Configuration()
		{
			JsonRootDirectory = string.Empty;
			FontPaths = new List<string>();
			OverridesEnabled = false;
			CharacterOverrides = new Dictionary<char, char>();
			TextMarkerWidths = new Dictionary<string, int>();
		}
	}
}
