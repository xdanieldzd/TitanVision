using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TitanVision
{
	public class Configuration
	{
		public string JsonRootDirectory { get; set; }
		public List<string> FontPaths { get; set; }

		public Configuration()
		{
			JsonRootDirectory = string.Empty;
			FontPaths = new List<string>();
		}
	}
}
