using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.input {
	public class MouseInputManager {

		public bool Blocked { get; private set; }
		private bool TempBlocked = false;

		public bool Left { get; private set; }
		public bool Right { get; private set; }
		public bool Middle { get; private set; }
		public bool Mouse4 { get; private set; }
		public bool Mouse5 { get; private set; }

		public bool PrevLeft { get; private set; }
		public bool PrevRight { get; private set; }
		public bool PrevMiddle { get; private set; }
		public bool PrevMouse4 { get; private set; }
		public bool PrevMouse5 { get; private set; }

		public Point Position { get; private set; }
		public Point PrevPosition { get; private set; }

		/// <summary> The difference in scroll wheel position from last frame. </summary>
		public int Scroll { get; private set; }
		private int PrevScroll; //note this doesn't function like other prev values, it's just for comparison to get scroll

		public void UpdateControlStates(MouseState m) {
			PrevLeft = Left;
			PrevRight = Right;
			PrevMiddle = Middle;
			PrevMouse4 = Mouse4;
			PrevMouse5 = Mouse5;
			PrevPosition = Position;

			Left = m.LeftButton == ButtonState.Pressed;
			Right = m.RightButton == ButtonState.Pressed;
			Middle = m.MiddleButton == ButtonState.Pressed;
			Mouse4 = m.XButton1 == ButtonState.Pressed;
			Mouse5 = m.XButton2 == ButtonState.Pressed;
			Position = m.Position;

			Scroll = m.ScrollWheelValue - PrevScroll;
			PrevScroll = m.ScrollWheelValue;

			Blocked = false;
			TempBlocked = false;
		}

		/// <summary> Marks as blocked for the remainder of the frame. </summary>
		public void Block() {
			Blocked = true;
			if (TempBlocked) {
				TempBlocked = false;
			}
		}

		/// <summary> Marks as blocked if not already. Use UnTempBlock to undo without undoing a normal Block. </summary>
		public void TempBlock() {
			if (!Blocked) {
				Blocked = true;
				TempBlocked = true;
			}
		}

		public void UnTempBlock() {
			if (TempBlocked) {
				Blocked = false;
				TempBlocked = false;
			}
		}

		public bool LeftPress {
			get { return Left && !PrevLeft; }
		}

		public bool RightPress {
			get { return Right && !PrevRight; }
		}

		public bool MiddlePress {
			get { return Middle && !PrevMiddle; }
		}

		public bool Mouse4Press {
			get { return Mouse4 && !PrevMouse4; }
		}

		public bool Mouse5Press {
			get { return Mouse5 && !PrevMouse5; }
		}

		public bool LeftRelease {
			get { return !Left && PrevLeft; }
		}

		public bool RightRelease {
			get { return !Right && PrevRight; }
		}

		public bool MiddleRelease {
			get { return !Middle && PrevMiddle; }
		}

		public bool Mouse4Release {
			get { return !Mouse4 && PrevMouse4; }
		}

		public bool Mouse5Release {
			get { return !Mouse5 && PrevMouse5; }
		}

		public int X {
			get { return Position.X; }
		}

		public int Y {
			get { return Position.Y; }
		}

		public bool ScrollUp {
			get { return Scroll > 0; }
		}

		public bool ScrollDown {
			get { return Scroll < 0; }
		}

	}
}
