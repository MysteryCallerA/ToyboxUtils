using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.input {
	public class GameInputManager<T> where T : Enum {

		private KeyboardState KState;
		private MouseState MState;
		private MouseState OMState;
		private GamePadState GState;

		public Dictionary<T, VirtualKey> VirtualKeys = new Dictionary<T, VirtualKey>(); //TODO when a key is forced down, it should stay down until released and pressed again. inverse when forced up

		public bool BlockedKeyboard {
			get; private set;
		}

		public VirtualKey this[T id] {
			get { return VirtualKeys[id]; }
		}

		public void UpdateControlStates(KeyboardState kstate, MouseState mstate) {
			OMState = MState;
			KState = kstate;
			MState = mstate;

			foreach (var k in VirtualKeys.Values) {
				k.WasDown = k.Down;
				k.Down = false;

				foreach (var input in k.Keys) {
					if (KState.IsKeyDown(input)) {
						k.Down = true;
						break;
					}
				}
				if (k.Down) continue;

				if (k.LeftMouse && MState.LeftButton == ButtonState.Pressed) {
					k.Down = true; continue;
				}
				if (k.RightMouse && MState.RightButton == ButtonState.Pressed) {
					k.Down = true; continue;
				}
				if (k.MiddleMouse && MState.MiddleButton == ButtonState.Pressed) {
					k.Down = true; continue;
				}
				if (k.Mouse4 && MState.XButton1 == ButtonState.Pressed) {
					k.Down = true; continue;
				}
				if (k.Mouse5 && MState.XButton2 == ButtonState.Pressed) {
					k.Down = true; continue;
				}

				int scroll = MState.ScrollWheelValue - OMState.ScrollWheelValue;
				if (k.ScrollDown && scroll < 0) {
					k.Down = true; continue;
				}
				if (k.ScrollUp && scroll > 0) {
					k.Down = true; continue;
				}
			}
			BlockedKeyboard = false;
		}

		public void UpdateControlStates(KeyboardState kstate, MouseState mstate, GamePadState gstate) {
			UpdateControlStates(kstate, mstate);

			GState = gstate;

			foreach (var k in VirtualKeys.Values) {
				if (k.Down) continue;

				foreach (var input in k.Buttons) {
					if (GState.IsButtonDown(input)) {
						k.Down = true;
						break;
					}
				}
			}
		}

		public bool Down(T key) {
			var k = VirtualKeys[key];
			return k.Down;
		}

		public bool Up(T key) {
			var k = VirtualKeys[key];
			return !k.Down;
		}

		public bool WasDown(T key) {
			return VirtualKeys[key].WasDown;
		}

		public bool WasUp(T key) {
			return !WasDown(key);
		}

		public bool Pressed(T key) {
			var k = VirtualKeys[key];
			return k.Down && !k.WasDown;
		}

		public bool Released(T key) {
			var k = VirtualKeys[key];
			return !k.Down && k.WasDown;
		}

		public void ForceDown(T key) {
			VirtualKeys[key].Down = true;
		}

		public void ForceUp(T key) {
			VirtualKeys[key].Down = false;
		}

		public Point MousePosition {
			get { return MState.Position; }
		}

		public Point PrevMousePosition {
			get { return OMState.Position; }
		}

		public void BlockKeyboard() {
			BlockedKeyboard = true;
		}

		public void Add(T t, VirtualKey v) {
			VirtualKeys.Add(t, v);
		}

	}

	public class VirtualKey {
		public bool Down = false;
		public bool WasDown = false;

		public List<Keys> Keys = new List<Keys>();
		public List<Buttons> Buttons = new List<Buttons>();
		public bool LeftMouse = false;
		public bool RightMouse = false;
		public bool MiddleMouse = false;
		public bool Mouse4 = false;
		public bool Mouse5 = false;
		public bool ScrollUp = false;
		public bool ScrollDown = false;

		public VirtualKey(params Keys[] k) {
			Keys.AddRange(k);
		}

		public bool Pressed {
			get { return Down && !WasDown; }
		}

		public bool Released {
			get { return !Down && WasDown; }
		}
	}
}
