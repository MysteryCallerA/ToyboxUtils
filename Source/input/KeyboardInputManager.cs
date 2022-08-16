using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.input {
	public class KeyboardInputManager {

		private KeyboardState KState;
		private KeyboardState OKState;
		private Dictionary<Keys, int> HoldTime = new Dictionary<Keys, int>();
		private List<Keys> KeysPressed;

		public bool TrackHoldTime = false;
		public bool AllowHoldRepeats = false;
		public int HoldRepeatPeriod = 3;
		public int HoldRepeatStart = 25;

		public bool Blocked { get; private set; }

		public void UpdateControlStates(KeyboardState k) {
			OKState = KState;
			KState = k;

			Blocked = false;

			KeysPressed = GetKeysDown().Except(GetKeysWasDown()).ToList();

			if (!TrackHoldTime && !AllowHoldRepeats) return;

			//Update Held Keys
			Keys[] keys = k.GetPressedKeys().ToArray();
			foreach (Keys key in keys) {
				if (HoldTime.ContainsKey(key)) {
					HoldTime[key]++;
				} else {
					HoldTime.Add(key, 0);
				}
			}

			//Reset released keys
			keys = OKState.GetPressedKeys().ToArray();
			foreach (Keys key in keys) {
				if (k.IsKeyDown(key)) continue;
				HoldTime[key] = -1;
			}

			//Update collection of keys pressed for holdrepeats
			var holding = OKState.GetPressedKeys();
			foreach (var key in holding) {
				if (Pressed(key)) KeysPressed.Add(key);
			}
		}

		public void Block() {
			Blocked = true;
		}

		public bool Down(Keys k) {
			return KState.IsKeyDown(k);
		}

		public bool Up(Keys k) {
			return KState.IsKeyUp(k);
		}

		public bool WasDown(Keys k) {
			return OKState.IsKeyDown(k);
		}

		public bool WasUp(Keys k) {
			return OKState.IsKeyUp(k);
		}

		public bool Pressed(Keys k) {
			if (AllowHoldRepeats && HoldTime.ContainsKey(k) && HoldTime[k] >= HoldRepeatStart) {
				if ((HoldTime[k] - HoldRepeatStart) % HoldRepeatPeriod == 0) return true;
			}
			return KState.IsKeyDown(k) && OKState.IsKeyUp(k);
		}

		public bool Released(Keys k) {
			return KState.IsKeyUp(k) && OKState.IsKeyDown(k);
		}

		public int GetHoldTime(Keys k) {
			if (!HoldTime.ContainsKey(k)) {
				return -1;
			}
			return HoldTime[k];
		}

		public Keys[] GetKeysDown() {
			return KState.GetPressedKeys();
		}

		public Keys[] GetKeysWasDown() {
			return OKState.GetPressedKeys();
		}

		public Keys[] GetKeysPressed() {
			return KeysPressed.ToArray();
		}

		public bool CheckUpperCase() {
			bool output = KState.IsKeyDown(Keys.LeftShift) || KState.IsKeyDown(Keys.RightShift);
			if (KState.CapsLock) return !output;
			return output;
		}

		public bool ControlDown() {
			return KState.IsKeyDown(Keys.LeftControl) || KState.IsKeyDown(Keys.RightControl);
		}

		public bool ShiftDown() {
			return KState.IsKeyDown(Keys.LeftShift) || KState.IsKeyDown(Keys.RightShift);
		}

		public bool AltDown() {
			return KState.IsKeyDown(Keys.LeftAlt) || KState.IsKeyDown(Keys.RightAlt);
		}

		/// <summary> Gets the string representation of the supplied key, taking into account shift and caps-lock. </summary>
		public string KeyToString(Keys key) {
			if (key.ToString().Length == 1) {
				if (CheckUpperCase()) {
					return key.ToString().ToUpper();
				} else {
					return key.ToString().ToLower();
				}
			}
			if (key == Keys.Space) return " ";
			if (key == Keys.Enter) return "\n";

			if (ShiftDown()) {
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

	}
}
