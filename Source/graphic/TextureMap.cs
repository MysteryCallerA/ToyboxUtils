using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.graphic {
	public class TextureMap:ITextureObject {

		public Texture2D Texture;
		public List<TextureMapFrame> Frames = new List<TextureMapFrame>();


		public TextureMap(Texture2D t) {
			Texture = t;
		}

		public int FrameCount {
			get { return Frames.Count; }
		}

		public void AddFrame(Rectangle frame) {
			AddFrame(frame, Point.Zero);
		}

		public void AddFrame(Rectangle frame, Point origin) {
			Frames.Add(new TextureMapFrame(frame, origin));
		}

		public void Dispose() {
			Texture.Dispose();
		}

		public void Draw(SpriteBatch s, Point dest, int frame, Vector2 scale) {
			Draw(s, dest, frame, scale, Color.White, 0, SpriteEffects.None);
		}

		public void Draw(SpriteBatch s, Point dest, int frame, Vector2 scale, Color c, float rotation, SpriteEffects effects) {
			Rectangle source = Frames[frame].Bounds;
			Rectangle destrect = new Rectangle(dest.X, dest.Y, (int)(source.Width * scale.X), (int)(source.Height * scale.Y));
			s.Draw(Texture, destrect, source, c, rotation, Frames[frame].Origin.ToVector2(), effects, 0);
		}

		public Point GetDimensions(int frame, Vector2 scale) {
			return new Point(GetWidth(frame, scale.X), GetHeight(frame, scale.Y));
		}

		public int GetHeight(int frame, float scale) {
			return (int)(Frames[frame].Bounds.Width * scale);
		}

		public int GetWidth(int frame, float scale) {
			return (int)(Frames[frame].Bounds.Height * scale);
		}

		public struct TextureMapFrame {

			public Rectangle Bounds;
			public Point Origin;

			public TextureMapFrame(Rectangle bounds, Point origin) {
				Bounds = bounds;
				Origin = origin;
			}

		}
	}
}
