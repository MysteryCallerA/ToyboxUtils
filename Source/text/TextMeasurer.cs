using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Utils.text {
	
	public class TextMeasurer {

		private Text Measured;
		private string MeasuredContent = "";
		private List<Rectangle> Characters = new List<Rectangle>();
		private List<Rectangle> Words = new List<Rectangle>();
		private List<Rectangle> Lines = new List<Rectangle>();
		
		public Point Size {
			get; private set;
		}

		//TODO add bounds measuring

		public bool Outdated {
			get {
				if (Measured == null) return true;
				return MeasuredContent != Measured.Content; 
			}
		}

		public void Update(Text t) {
			int start = 0;
			char prev = ' ';
			while (start < t.Content.Length) { //Skip unchanged characters
				if (start >= MeasuredContent.Length) break;
				if (t.Content[start] != MeasuredContent[start]) break;
				prev = t.Content[start];
				start++;
			}
			Characters.RemoveRange(start, Characters.Count - start);

			Rectangle draw = new Rectangle(0, 0, 0, 0);
			if (start > 0) draw = Characters[start - 1];

			for (int i = start; i < t.Content.Length; i++) {
				char c = t.Content[i];
				
				if (prev == Font.Newline) {
					draw.X = 0;
					draw.Y += (t.Font.CharHeight + t.LineSpace) * t.Scale;
					draw.Width = 0;
				} else if (i != 0) {
					draw.X += t.LetterSpace * t.Scale;
				}

				draw = t.GetCharDest(c, new Point(draw.Right, draw.Y));
				Characters.Add(draw);
				prev = c;
			}

			MeasuredContent = t.Content;
			Measured = t;
			UpdateGroupedBounds();
		}

		private void UpdateGroupedBounds() {
			Words.Clear();
			Lines.Clear();
			var newsize = new Point(0, Measured.LineHeight);

			var word = new Rectangle();
			var line = new Rectangle(0, 0, 0, Measured.LineHeight);
			for (int i = 0; i < Characters.Count; i++) {
				line.Width += Characters[i].Right - line.Right;
				if (MeasuredContent[i] == ' ' || MeasuredContent[i] == Font.Newline) {
					if (word.Width > 0) Words.Add(word);
					word.Width = 0;
					if (MeasuredContent[i] == Font.Newline) {
						Lines.Add(line);
						if (newsize.X < line.Width) newsize.X = line.Width;
						newsize.Y += Measured.LineHeight + Measured.LineSpace * Measured.Scale;
						line.Y += line.Height + Measured.LineSpace * Measured.Scale;
						line.Width = 0;
					}
					continue;
				}

				if (word.Width == 0) word.X = Characters[i].X;
				word.Width += Characters[i].Right - word.Right;
			}
			if (word.Width > 0) Words.Add(word);
			if (newsize.X < line.Width) newsize.X = line.Width;
			Lines.Add(line);
			Size = newsize;
		}

		public Rectangle GetCharRect(int pos) {
			Rectangle output;
			if (pos < Characters.Count) output = Characters[pos];
			else if (Characters.Count == 0) output = new Rectangle(0, 0, 0, Measured.Font.CharHeight * Measured.Scale);
			else {
				output = Characters[Characters.Count - 1];
				if (MeasuredContent[Characters.Count - 1] == Font.Newline) {
					output.X = 0;
					output.Y += output.Height + (Measured.LineSpace * Measured.Scale);
					output.Width = 0;
				} else {
					output.X = output.Right + Measured.Scale;
				}
			}

			output.Location += Measured.Position - Measured.Scroll;
			return output;
		}

		public int FindNearestCharPos(Point pick) {
			if (Characters.Count == 0) return 0;
			var t = Measured;

			pick -= t.Position - t.Scroll;
			int nearestline = pick.Y / ((t.Font.CharHeight + t.LineSpace) * t.Scale);

			int prevlinestart = 0; //keep this info in case you try to select a line that doesn't exist
			int currentline = 0;
			int nearestpos = 0;
			try {
				//Find Line Start
				if (nearestline != 0) {
					while (nearestpos < t.Content.Length) {
						if (t.Content[nearestpos] == Font.Newline) {
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
				if (nearestpos >= t.Content.Length) {
					if (t.Content.Last() == Font.Newline) return t.Content.Length;
					nearestpos = prevlinestart;
				}

				//Pick Char
				if (pick.X < Characters[nearestpos].X) return nearestpos;
				while (pick.X > Characters[nearestpos].Right) {
					if (t.Content[nearestpos] == Font.Newline) break;
					nearestpos++;
					if (nearestpos >= Characters.Count) break;
				}
				return nearestpos;

			} catch (Exception e) {
				throw new Exception("CharRects/Content Mismatch: Call Update sometime between when Content is changed and FindNearestCharPos is called.", e);
			}
		}

		public Rectangle FindNearestLine(Point pick, out int linenum) {
			var t = Measured;
			pick -= t.Position + t.Scroll;
			Rectangle output = Lines.Last();
			linenum = Lines.Count - 1;

			for (int i = 0; i < Lines.Count; i++) {
				if (pick.Y < Lines[i].Bottom) {
					output = Lines[i];
					linenum = i;
					break;
				}
			}

			output.Location += t.Position + t.Scroll;
			return output;
		}

		public Rectangle? PickLine(Point pick, out int linenum) {
			var output = FindNearestLine(pick, out linenum);
			if (output.Contains(pick)) return output;

			linenum = -1;
			return null;
		}

	}
}
