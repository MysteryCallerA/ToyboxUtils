﻿using Microsoft.Xna.Framework;
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
		public const string SpecialCharsUpper = "<>?:{}|";
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

		public Font(Texture2D graphic, string charset, char missingChar, Rectangle pixel) {
			Graphic = graphic;
			Missing = missingChar;
			Pixel = pixel;

			CharData = ParseSizes(graphic, charset);
			CharHeight = CharData[charset[0]].Height;
			AverageCharWidth = CharData[charset[0]].Width;
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

		private Dictionary<char, Rectangle> ParseSizes(Texture2D g, string charset) {
			charset = charset.Replace("\n", String.Empty);
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
			hsplits.Add(g.Height);

			int nextchar = 0;

			for (int hsplit = 0; hsplit < hsplits.Count; hsplit++) {
				Rectangle charrect = new Rectangle(0, 0, 0, hsplits[hsplit]);
				if (hsplit != 0) {
					charrect.Y = hsplits[hsplit - 1] + 1;
					charrect.Height -= charrect.Y;
				}

				for (int x = 0; x < g.Width; x++) {
					bool clear = true;
					for (int y = charrect.Y; y < charrect.Bottom; y++) {
						if (graphic.Utils.GetPixel(gdata, x, y, g.Width).A > 0) {
							clear = false;
							break;
						}
					}

					if (x == g.Width - 1) { //Catch last char on line if its right seperation would be outofbounds
						x++;
						clear = true;
					}

					if (clear) {
						charrect.Width = x - charrect.X;
						if (charrect.Width == 0) break; //Starts next line if two clear lines in a row
						chardata.Add(charset[nextchar], charrect);
						charrect.X = charrect.Right + 1;
						nextchar++;
						if (nextchar >= charset.Length) return chardata;
					}
				}
			}

			return chardata;
		}

	}
}
