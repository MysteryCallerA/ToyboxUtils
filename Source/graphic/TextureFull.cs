using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.graphic {
	public class TextureFull:ITextureObject {

		public Texture2D Texture;
		public Vector2 Origin;

		public TextureFull(Texture2D texture) {
			Texture = texture;
		}

		public int FrameCount {
			get { return 1; }
		}

		public void Dispose() {
			Texture.Dispose();
		}

		public void Draw(SpriteBatch s, Point dest, int frame, Vector2 scale) {
			Draw(s, dest, frame, scale, Color.White, 0, SpriteEffects.None);
		}

		public void Draw(SpriteBatch s, Point dest, int frame, Vector2 scale, Color c, float rotation, SpriteEffects effects) {
			s.Draw(Texture, new Rectangle(dest.X, dest.Y, (int)(Texture.Width * scale.X), (int)(Texture.Height * scale.Y)), null, c, rotation, Origin, effects, 0);
		}

		public Point GetDimensions(int frame, Vector2 scale) {
			return new Point(GetWidth(frame, scale.X), GetHeight(frame, scale.Y));
		}

		public int GetHeight(int frame, float scale) {
			return (int)(Texture.Height * scale);
		}

		public int GetWidth(int frame, float scale) {
			return (int)(Texture.Width * scale);
		}
	}
}
