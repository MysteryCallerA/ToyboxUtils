using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Utils.math {

	public static class Aspect {

		public static double GetAspect(Point source) {
			return (double)source.Y / source.X;
		}

		public static int GetAspectedWidth(Point aspect, int newheight) {
			return GetAspectedWidth(GetAspect(aspect), newheight);
		}

		public static int GetAspectedWidth(double aspect, int newheight) {
			return (int)(newheight / aspect);
		}

		public static int GetAspectedHeight(Point aspect, int newwidth) {
			return GetAspectedHeight(GetAspect(aspect), newwidth);
		}

		public static int GetAspectedHeight(double aspect, int newwidth) {
			return (int)(newwidth * aspect);
		}

		public static Point GetBestFit(Point aspect, Point maxDimensions) {
			double a = GetAspect(aspect);
			int w = GetAspectedWidth(a, maxDimensions.Y);
			int h = GetAspectedHeight(a, maxDimensions.X);
			if (w < h) return new Point(w, maxDimensions.Y);
			return new Point(maxDimensions.X, h);
		}

	}
}
