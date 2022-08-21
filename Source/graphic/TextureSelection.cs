using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.graphic {
	public class TextureSelection:ITextureObject {

		public Texture2D Texture;
		public Rectangle Selection;
		public Vector2 Origin = Vector2.Zero;

		public TextureSelection(Texture2D t, Rectangle selection) {
			Texture = t;
			Selection = selection;
		}

		public TextureSelection(Texture2D t) {
			Texture = t;
			Selection = Texture.Bounds;
		}

		public int FrameCount {
			get { return 1; }
		}

		public void Draw(SpriteBatch s, Point dest, int frame, Vector2 scale) {
			Draw(s, dest, frame, scale, Color.White, 0, SpriteEffects.None);
		}

		public void Draw(SpriteBatch s, Point dest, int frame, Vector2 scale, Color c, float rotation, SpriteEffects effects) {
			var destrect = new Rectangle(dest.X, dest.Y, (int)(Selection.Width * scale.X), (int)(Selection.Height * scale.Y));
			s.Draw(Texture, destrect, Selection, c, rotation, Origin, effects, 0);
		}

		public Point GetDimensions(int frame, Vector2 scale) {
			return new Point(GetWidth(frame, scale.X), GetHeight(frame, scale.Y));
		}

		public int GetHeight(int frame, float scale) {
			return (int)(Selection.Height * scale);
		}

		public int GetWidth(int frame, float scale) {
			return (int)(Selection.Width * scale);
		}

		public void Dispose() {
			Texture.Dispose();
		}
	}
}
