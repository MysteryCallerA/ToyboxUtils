using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.graphic {
	public class TextureSelection:ITextureObject {

		public Rectangle Selection;

		public TextureSelection(Texture2D t, Rectangle selection) {
			Texture = t;
			Selection = selection;
		}

		public TextureSelection(Texture2D t) {
			Texture = t;
			Selection = Texture.Bounds;
		}

		public Texture2D Texture { 
			get; set;
		}

		public Rectangle Source {
			get { return Selection; }
		}

		public int Width {
			get { return Selection.Width; }
		}

		public int Height {
			get { return Selection.Height; }
		}
	}
}
