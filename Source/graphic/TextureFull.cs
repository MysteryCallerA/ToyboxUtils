using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.graphic {
	public class TextureFull:ITextureObject {

		public TextureFull(Texture2D texture) {
			Texture = texture;
		}

		public Texture2D Texture {
			get; set;
		}

		public Rectangle Source {
			get { return Texture.Bounds; }
		}

		public int Width {
			get { return Texture.Width; } 
		}

		public int Height {
			get { return Texture.Height; }
		}
	}
}
