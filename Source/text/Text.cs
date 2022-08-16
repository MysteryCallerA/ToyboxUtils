using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.text {
	public class Text {

		public Font Font;
		public int Scale = 1;
		public Point Position;
		public Color Color = Color.White;
		public Rectangle Mask;
		public bool UseMask = false;
		public bool GreedyMask = true;

		public int WordSpace = 1;
		public int LineSpace = 1;
		public int LetterSpace = 1;

		public Point Scroll = Point.Zero;
		/// <summary> These positions are relative. </summary>
		private List<Rectangle> CharRects = new List<Rectangle>();
		/// <summary> The string cooresponding to the current charRects data. </summary>
		private string CharRectContent = "";

		protected bool CharRectsOutdated {
			get { return CharRectContent != Content; }
		}

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

		public Rectangle GetBounds() { //HACK This doesn't take into account multiline widths
			if (CharRects.Count == 0) return new Rectangle(Position.X, Position.Y, 0, Font.CharHeight * Scale);
			return new Rectangle(Position.X, Position.Y, CharRects.Last().Right, CharRects.Last().Bottom);
		}

		public Point GetSize() { //HACK This doesn't take into account multiline widths
			if (CharRects.Count == 0) return new Point(0, Font.CharHeight * Scale);
			var r = Rectangle.Union(CharRects[0], CharRects.Last()).Size;
			return r;
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

		private Rectangle DrawChar(SpriteBatch s, char c, Point pos, Color color) {
			if (c == ' ' || c == Font.Newline) {
				return GetCharDest(c, pos);
			}

			Rectangle source = GetCharSource(c);
			Rectangle dest = GetCharDest(c, pos, source);
			var unmasked = dest;

			if (UseMask) {
				ApplyMask(ref dest, ref source);
			}

			s.Draw(Font.Graphic, dest, source, color);
			return unmasked;
		}

		private Rectangle GetCharSource(char c) {
			if (!Font.Contains(c)) {
				return Font[Font.Missing];
			} else {
				return Font[c];
			}
		}

		private Rectangle GetCharDest(char c, Point pos) {
			return GetCharDest(c, pos, GetCharSource(c));
		}

		private Rectangle GetCharDest(char c, Point pos, Rectangle source) {
			if (c == ' ') {
				return new Rectangle(pos.X, pos.Y, WordSpace * Scale, Font.CharHeight * Scale);
			}
			if (c == Font.Newline) {
				return new Rectangle(pos.X, pos.Y, Scale, Font.CharHeight * Scale);
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

		public void UpdateCharRects() {
			int start = 0;
			char prev = ' ';
			while (start < Content.Length) {
				if (start >= CharRectContent.Length) break;
				if (Content[start] != CharRectContent[start]) break;
				prev = Content[start];
				start++;
			}
			CharRects.RemoveRange(start, CharRects.Count - start);

			Rectangle draw = new Rectangle(0, 0, 0, 0);
			if (start > 0) draw = CharRects[start - 1];

			for (int i = start; i < Content.Length; i++) {
				char c = Content[i];

				if (prev == Font.Newline) {
					draw.X = 0;
					draw.Y += (Font.CharHeight + LineSpace) * Scale;
					draw.Width = 0;
				} else if (i != 0) {
					draw.X += LetterSpace * Scale;
				}

				draw = GetCharDest(c, new Point(draw.Right, draw.Y));
				CharRects.Add(draw);
				prev = c;
			}

			CharRectContent = Content;
		}

		public Rectangle GetCharRect(int pos) {
			Rectangle output;
			if (pos < CharRects.Count) output = CharRects[pos];
			else if (CharRects.Count == 0) output = new Rectangle(0, 0, 0, Font.CharHeight * Scale);
			else {
				output = CharRects[CharRects.Count - 1];
				if (Content[CharRects.Count - 1] == Font.Newline) {
					output.X = 0;
					output.Y += output.Height + (LineSpace * Scale);
					output.Width = 0;
				} else {
					output.X = output.Right + Scale;
				}
			}

			output.Location += Position - Scroll;
			return output;
		}

		protected int FindNearestCharPos(Point pick) {
			if (CharRects.Count == 0) return 0;

			pick -= Position - Scroll;
			int nearestline = pick.Y / ((Font.CharHeight + LineSpace) * Scale);

			int prevlinestart = 0; //keep this info in case you try to select a line that doesn't exist
			int currentline = 0;
			int nearestpos = 0;
			try {
				//Find Line Start
				if (nearestline != 0) {
					while (nearestpos < Content.Length) {
						if (Content[nearestpos] == Font.Newline) {
							currentline++;
							if (currentline == nearestline) {
								nearestpos++;
								break;
							}
							prevlinestart = nearestpos + 1;
						}
						nearestpos++;
					}
				}

				//Prevents exceptions when trying to select below the bottom of the content
				if (nearestpos >= Content.Length) {
					if (Content.Last() == Font.Newline) return Content.Length;
					nearestpos = prevlinestart;
				}

				//Pick Char
				if (pick.X < CharRects[nearestpos].X) return nearestpos;
				while (pick.X > CharRects[nearestpos].Right) {
					if (Content[nearestpos] == Font.Newline) break;
					nearestpos++;
					if (nearestpos >= CharRects.Count) break;
				}
				return nearestpos;

			} catch (Exception e) {
				throw new Exception("CharRects/Content Mismatch: Call UpdateCharRects sometime between when Content is changed and FindNearestCharPos is called.", e);
			}
		}

		/// <summary> Accounts for scale. </summary>
		public int LineHeight {
			get { return Font.CharHeight * Scale; }
		}

	}
}
