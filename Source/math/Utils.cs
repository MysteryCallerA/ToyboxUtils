using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.math {
	public static class Utils {

		/// <summary> Returns the top number divided by the bot number, floored. </summary>
		public static int FloorDiv(int top, int bot) {
			return (int)Math.Floor((double)top / bot);
		}

		public static int GCD(int a, int b) {
			while (b > 0) {
				int rem = a % b;
				a = b;
				b = rem;
			}
			return a;
		}

		public static void Swap<T>(ref T var1, ref T var2) {
			T temp = var1;
			var1 = var2;
			var2 = temp;
		}

		public static Vector2 VectorFromRadians(float r) {
			r = r % ((float)Math.PI * 2);
			r -= (float)Math.PI;
			return new Vector2((float)Math.Cos(r), (float)Math.Sin(r));
		}

		public static Vector2 VectorFromDegrees(float d) {
			return VectorFromRadians(MathHelper.ToRadians(d));
		}

		public static int FloorMultiple(int value, int multipleof) {
			return (int)Math.Floor((float)value / multipleof) * multipleof;
		}

		public static Point FloorMultiple(Point value, int multipleof) {
			return new Point((int)Math.Floor((float)value.X / multipleof) * multipleof, (int)Math.Floor((float)value.Y / multipleof) * multipleof);
		}

		public static List<Point> LerpLine(Point start, Point end) {
			var output = new List<Point>();

			float n = Distance(start, end);
			for (var step = 0; step <= n; step++) {
				float t = n == 0 ? 0f : step / n;
				output.Add(Lerp(start, end, t));
			}
			return output;
		}

		public static int Distance(Point start, Point end) {
			return Math.Max(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y));
		}

		public static float Lerp(float start, float end, float t) {
			return start + t * (end - start);
		}

		public static Point Lerp(Point start, Point end, float t) {
			return new Point((int)Math.Round(Lerp(start.X, end.X, t)), (int)Math.Round(Lerp(start.Y, end.Y, t)));
		}

	}
}
