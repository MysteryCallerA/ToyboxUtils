using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utils {
	public static class ContentLoader {

		public static GraphicsDevice GraphicsDevice;

		public static Texture2D Load(string path) {
			using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open)) {
				var t = Texture2D.FromStream(GraphicsDevice, stream);
				t.Name = path;
				return t;
			}
		}

		public static string LocalPath {
			get {
				return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			}
		}

	}
}
