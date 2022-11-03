using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.graphic {
	public class TextureMap:ITextureObject {

		public List<TextureMapFrame> Frames = new List<TextureMapFrame>();
		public int SelectedFrame = 0;

		public TextureMap(Texture2D t) {
			Texture = t;
		}

		public Texture2D Texture {
			get; set;
		}

		public Rectangle Source {
			get { return Frames[SelectedFrame].Bounds; }
		}

		public int Width {
			get { return Source.Width; }
		}

		public int Height {
			get { return Source.Height; }
		}

		public struct TextureMapFrame {

			public Rectangle Bounds;
			public Point Origin = Point.Zero;

			public TextureMapFrame(Rectangle bounds) {
				Bounds = bounds;
			}

		}
	}
}
