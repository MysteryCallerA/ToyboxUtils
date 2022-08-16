using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.text {
	public static class Utils {

		public static string GetStringFromKey(Keys key, bool isUpperCase) {
			if (key.ToString().Length == 1) {
				if (isUpperCase) {
					return key.ToString().ToUpper();
				} else {
					return key.ToString().ToLower();
				}
			}
			if (key == Keys.Space) return " ";
			if (key == Keys.Enter) return "\n";

			if (isUpperCase) {
				switch (key) {
					case Keys.OemComma: return "<";
					case Keys.OemPeriod: return ">";
					case Keys.OemQuestion: return "?";
					case Keys.OemSemicolon: return ":";
					case Keys.OemQuotes: return "\"";
					case Keys.OemOpenBrackets: return "{";
					case Keys.OemCloseBrackets: return "}";
					case Keys.OemPipe: return "|";
					case Keys.OemMinus: return "_";
					case Keys.OemPlus: return "+";
					case Keys.D1: return "!";
					case Keys.D2: return "@";
					case Keys.D3: return "#";
					case Keys.D4: return "$";
					case Keys.D5: return "%";
					case Keys.D6: return "^";
					case Keys.D7: return "&";
					case Keys.D8: return "*";
					case Keys.D9: return "(";
					case Keys.D0: return ")";
				}
			} else {
				switch (key) {
					case Keys.OemComma: return ",";
					case Keys.OemPeriod: return ".";
					case Keys.OemQuestion: return "/";
					case Keys.OemSemicolon: return ";";
					case Keys.OemQuotes: return "'";
					case Keys.OemOpenBrackets: return "[";
					case Keys.OemCloseBrackets: return "]";
					case Keys.OemPipe: return "\\";
					case Keys.OemMinus: return "-";
					case Keys.OemPlus: return "=";
					case Keys.D1: case Keys.NumPad1: return "1";
					case Keys.D2: case Keys.NumPad2: return "2";
					case Keys.D3: case Keys.NumPad3: return "3";
					case Keys.D4: case Keys.NumPad4: return "4";
					case Keys.D5: case Keys.NumPad5: return "5";
					case Keys.D6: case Keys.NumPad6: return "6";
					case Keys.D7: case Keys.NumPad7: return "7";
					case Keys.D8: case Keys.NumPad8: return "8";
					case Keys.D9: case Keys.NumPad9: return "9";
					case Keys.D0: case Keys.NumPad0: return "0";
				}
			}

			return null;
		}

		public static string RemoveWhitespace(string input) {
			return new string(input.ToCharArray()
				.Where(c => !char.IsWhiteSpace(c))
				.ToArray());
		}

		public static string ToFileName(string i) {
			string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
			string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

			return System.Text.RegularExpressions.Regex.Replace(i, invalidRegStr, "_");
		}

		public static void GetMultilinePos(string[] lines, int pos, out int linenum, out int linepos) {
			linenum = 0;
			int pointer = 0;
			while (linenum < lines.Length) {
				if (pos < pointer + lines[linenum].Length) {
					linepos = pos - pointer;
					return;
				}
				pointer += lines[linenum].Length + 1;
				linenum++;
			}
			linepos = lines.Last().Length;
		}

		public static int GetOverallPos(string[] lines, int linenum, int linepos) {
			int pointer = 0;
			int output = 0;
			while (pointer < lines.Length && pointer < linenum) {
				output += lines[pointer].Length + 1;
				pointer++;
			}
			output += linepos;
			return output;
		}

		public readonly static HashSet<string> Ints = new HashSet<string> {
			 "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
		};

		public readonly static char[] Letters = new char[] {
			'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
			'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
		};

		public readonly static char[] AlphaNumeric = new char[] {
			'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
			'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
			'0','1','2','3','4','5','6','7','8','9'
		};

	}
}
