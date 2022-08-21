using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.graphic {
	public interface ITextureObject:IDisposable {

		void Draw(SpriteBatch s, Point dest, int frame, Vector2 scale);

		void Draw(SpriteBatch s, Point dest, int frame, Vector2 scale, Color c, float rotation, SpriteEffects effects);

		int FrameCount { get; }

		int GetWidth(int frame, float scale);

		int GetHeight(int frame, float scale);

		Point GetDimensions(int frame, Vector2 scale);

	}
}
