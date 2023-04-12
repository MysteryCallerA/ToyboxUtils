using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.graphic {
	public class TextureSelection:TextureObject {

		public Rectangle Selection;

		public TextureSelection(Texture2D t, Rectangle selection):base(t) {
			Selection = selection;
		}

		public TextureSelection(Texture2D t):base(t) {
			Selection = Texture.Bounds;
		}

		public override Rectangle Source {
			get { return Selection; }
		}
	}
}
