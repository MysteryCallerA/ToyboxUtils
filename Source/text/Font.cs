using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.text {
	public class Font {

		public const string LettersUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		public const string LettersLower = "abcdefghijklmnopqrstuvwxyz";
		public const string Numbers = "1234567890";
		public const string NumRowLower = "1234567890-=";
		public const string NumRowUpper = "!@#$%^&*()_+";
		public const string SpecialCharsLower = ",./;'[]\\";
		public const string SpecialCharsUpper = "<>?:\"{}|";
		public const char Newline = '\n';

		public Texture2D Graphic;
		public Dictionary<char, Rectangle> CharData = new Dictionary<char, Rectangle>();
		public char Missing;
		public Rectangle Pixel;

		public readonly int CharHeight;
		public readonly int AverageCharWidth;

		public Font(Texture2D graphic, Rectangle pixel, int charWidth, int charHeight, string charset, char missingChar, params Tuple<char, int>[] widthExceptions) {
			ParseSizes(graphic, charset);
			
			Graphic = graphic;
			Missing = missingChar;
			CharHeight = charHeight;
			AverageCharWidth = charWidth;
			Pixel = pixel;

			foreach (Tuple<char, int> t in widthExceptions) {
				CharData.Add(t.Item1, new Rectangle(0, 0, t.Item2, charHeight));
			}

			Rectangle box = new Rectangle(0, 0, charWidth, charHeight);
			foreach (char c in charset) {
				if (c == Newline) {
					box.X = 0;
					box.Y += box.Height;
					continue;
				}
				box.Width = charWidth;
				if (CharData.ContainsKey(c)) {
					box.Width = CharData[c].Width;
					CharData[c] = box;
				} else {
					CharData.Add(c, box);
				}
				box.X += box.Width;
			}
		}

		public void SetSymbols(params Tuple<char, Rectangle>[] chars) {
			foreach (Tuple<char, Rectangle> c in chars) {
				CharData.Add(c.Item1, c.Item2);
			}
		}

		public bool Contains(char c) {
			return CharData.ContainsKey(c);
		}

		public Rectangle this[char c] {
			get { return CharData[c]; }
		}

		public static Font GetFontTiny(Texture2D t) {
			return new Font(t, new Rectangle(1, 0, 1, 1), 3, 5, LettersUpper + Newline + LettersLower + Newline + NumRowLower + SpecialCharsLower + Newline + NumRowUpper + SpecialCharsUpper, '?',
				Tuple.Create('M', 5), Tuple.Create('N', 4), Tuple.Create('O', 4), Tuple.Create('Q', 4), Tuple.Create('U', 4), Tuple.Create('V', 5), Tuple.Create('W', 5),
				Tuple.Create('Y', 5), Tuple.Create('Z', 4), Tuple.Create('f', 2), Tuple.Create('g', 2), Tuple.Create('i', 1), Tuple.Create('j', 1), Tuple.Create('l', 1),
				Tuple.Create('m', 5), Tuple.Create('r', 2), Tuple.Create('w', 5), Tuple.Create(',', 1), Tuple.Create('.', 1), Tuple.Create('/', 4), Tuple.Create(';', 1),
				Tuple.Create('\'', 1), Tuple.Create('[', 2), Tuple.Create(']', 2), Tuple.Create('\\', 4), Tuple.Create('!', 1), Tuple.Create('#', 5), Tuple.Create('$', 3),
				Tuple.Create('%', 4), Tuple.Create('&', 4), Tuple.Create('(', 2), Tuple.Create(')', 2), Tuple.Create(':', 1), Tuple.Create('|', 1)
				);
		}

		private void ParseSizes(Texture2D g, string charset) {
			var chardata = new Dictionary<char, Rectangle>();
			var gdata = graphic.Utils.GetPixels(g);

			List<int> hsplits = new List<int>();
			for (int y = 0; y < g.Height; y++) {
				bool clear = true;
				for (int x = 0; x < g.Width; x++) {
					if (graphic.Utils.GetPixel(gdata, x, y, g.Width).A > 0) {
						clear = false;
						break;
					}
				}
				if (clear) hsplits.Add(y);
			}


		}

	}
}
