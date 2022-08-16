using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.graphic {

	public static class Utils {

		/// <summary> Get a 1d array of pixels. To find a specific pixel, use GetPixel. </summary>
		public static Color[] GetPixels(Texture2D t) {
			Color[] colors1d = new Color[t.Width * t.Height];
			t.GetData<Color>(colors1d);
			return colors1d;
		}

		/// <summary> Get specific pixel from texture2D. Use GetPixels(texture) to get an array of colors. </summary>
		public static Color GetPixel(Color[] colors, int x, int y, int textureWidth) {
			return colors[x + (y * textureWidth)];
		}

	}

}
