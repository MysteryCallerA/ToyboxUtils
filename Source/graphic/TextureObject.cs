using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.graphic {

	public abstract class TextureObject {

		public Texture2D Texture;

		public TextureObject(Texture2D t) {
			Texture = t;
		}

		public abstract Rectangle Source {
			get;
		}

		private Rectangle _Origin = Rectangle.Empty;

		public Rectangle Origin {
			get {
				if (Effect == SpriteEffects.None) return _Origin;

				var output = _Origin;
				if (Effect.HasFlag(SpriteEffects.FlipHorizontally)) {
					output.X = (Source.Width - _Origin.Right);
				}
				if (Effect.HasFlag(SpriteEffects.FlipVertically)) {
					output.Y = Source.Height - _Origin.Bottom;
				}
				return output;
			}
			set { _Origin = value; }
		}

		public SpriteEffects Effect = SpriteEffects.None;
	
	}
}
