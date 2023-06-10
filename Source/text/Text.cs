using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Utils.text {
	public class Text { //TODO move text systems to Toybox and implement Renderer

		public Font Font;
		public int Scale = 1;
		public Point Position;
		public Color Color = Color.White;
		public Color? BackColor = null;
		public Rectangle Mask;
		public bool UseMask = false;
		public bool GreedyMask = true;

		public int WordSpace = 1;
		public int LineSpace = 1;
		public int LetterSpace = 1;
		public int QuoteSpace = 1;

		public Point Scroll = Point.Zero;

		public EventHandler OnContentChanged;
		private string _content = "";
		public virtual string Content {
			get { return _content; }
			set {
				value = value.Replace("\r", String.Empty);
				_content = value;
				OnContentChanged?.Invoke(this, new EventArgs());
			}
		}

		public int X {
			get { return Position.X; }
			set { Position.X = value; }
		}

		public int Y {
			get { return Position.Y; }
			set { Position.Y = value; }
		}

		public Text(Font f) {
			Font = f;
		}

		public void Draw(SpriteBatch s) {
			Draw(s, Color);
		}

		public virtual void Draw(SpriteBatch s, Color color) {
			Draw(s, color, Position - Scroll, Content);
		}

		public void Draw(SpriteBatch s, Color color, Point pos, string text) {
			Rectangle draw = new Rectangle(pos.X, pos.Y, 0, 0);
			char prev = ' ';

			for (int i = 0; i < text.Length; i++) {
				char c = text[i];

				if (prev == Font.Newline) {
					draw.X = pos.X;
					draw.Y += (Font.CharHeight + LineSpace) * Scale;
					draw.Width = 0;
				} else if (i != 0) {
					draw.X += LetterSpace * Scale;
				}

				draw = DrawChar(s, c, new Point(draw.Right, draw.Y), color);
				prev = c;
			}
		}

		public void Draw(SpriteBatch s, Color color, Point pos, string text, int scale) {
			var old = Scale;
			Scale = scale;
			Draw(s, color, pos, text);
			Scale = old;
		}

		private Rectangle DrawChar(SpriteBatch s, char c, Point pos, Color color) {
			if (c == '\r') return new Rectangle(pos, Point.Zero);
			if (c == ' ' || c == Font.Newline) {
				var r = GetCharDest(c, pos);
				if (BackColor.HasValue && c == ' ') s.Draw(Font.Graphic, new Rectangle(r.X, r.Y, r.Width + LetterSpace * Scale, r.Height), Font.Pixel, BackColor.Value);
				return r;
			}
			if (c == '\"') {
				var output = DrawChar(s, '\'', pos, color);
				pos.X += output.Width + (QuoteSpace * Scale);
				var output2 = DrawChar(s, '\'', pos, color);
				return Rectangle.Union(output, output2);
			}

			Rectangle source = GetCharSource(c);
			Rectangle dest = GetCharDest(c, pos, source);
			var unmasked = dest;

			if (UseMask) {
				ApplyMask(ref dest, ref source);
			}

			if (BackColor.HasValue) s.Draw(Font.Graphic, new Rectangle(dest.X, dest.Y, dest.Width + LetterSpace * Scale, dest.Height), Font.Pixel, BackColor.Value);
			s.Draw(Font.Graphic, dest, source, color);
			return unmasked;
		}

		private Rectangle GetCharSource(char c) {
			if (c == '\"') {
				return GetCharSource('\'');
			}

			if (!Font.Contains(c)) {
				return Font[Font.Missing];
			} else {
				return Font[c];
			}
		}

		internal Rectangle GetCharDest(char c, Point pos) {
			return GetCharDest(c, pos, GetCharSource(c));
		}

		internal Rectangle GetCharDest(char c, Point pos, Rectangle source) {
			if (c == ' ') {
				return new Rectangle(pos.X, pos.Y, WordSpace * Scale, Font.CharHeight * Scale);
			}
			if (c == Font.Newline) {
				return new Rectangle(pos.X, pos.Y, Scale, Font.CharHeight * Scale);
			}
			if (c == '\"') {
				var output = GetCharDest('\'', pos, source);
				pos.X += output.Width + (QuoteSpace * Scale);
				var output2 = GetCharDest('\'', pos, source);
				return Rectangle.Union(output, output2);
			}

			return new Rectangle(pos.X, pos.Y, source.Width * Scale, source.Height * Scale);
		}

		private void ApplyMask(ref Rectangle dest, ref Rectangle source) {
			var mask = Mask;
			if (!GreedyMask) {
				//mask.Inflate(-Scale, -Scale);
			}

			var masked = Rectangle.Intersect(dest, mask);
			if (masked == Rectangle.Empty) {
				dest = masked;
				return;
			}
			if (masked == dest) return;

			int right = (dest.Right - masked.Right) / Scale;
			int left = (masked.Left - dest.Left) / Scale;
			int top = (masked.Top - dest.Top) / Scale;
			int bot = (dest.Bottom - masked.Bottom) / Scale;
			source = new Rectangle(source.X + left, source.Y + top, source.Width - (left + right), source.Height - (top + bot));

			//Correct distortions
			masked.X = math.Utils.FloorMultiple(masked.X - dest.X, Scale) + dest.X;
			masked.Y = math.Utils.FloorMultiple(masked.Y - dest.Y, Scale) + dest.Y;
			masked.Width = source.Width * Scale;
			masked.Height = source.Height * Scale;
			dest = masked;
		}

		/// <summary> Accounts for scale. </summary>
		public int LineHeight {
			get { return Font.CharHeight * Scale; }
		}

		public Rectangle GetBounds() {
			return new Rectangle(Position, GetSize());
		}

		public Point GetSize() {
			return GetSize(Content);
		}

		public Point GetSize(string t) {
			Point output = new Point();
			output.Y += LineHeight;
			Point currentline = new Point();
			for (int i = 0; i < t.Length; i++) {
				if (t[i] == Font.Newline) {
					output.Y += LineHeight + LineSpace;
					if (currentline.X > output.X) output.X = currentline.X;
					currentline.X = 0;
					continue;
				}
				if (currentline.X != 0) {
					currentline.X += LetterSpace * Scale;
				}

				var rect = GetCharDest(t[i], currentline);
				currentline.X = rect.Right;
			}

			if (currentline.X > output.X) output.X = currentline.X;
			return output;
		}

	}
}
