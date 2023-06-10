using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.input;

namespace Utils.text {
	public class TextInput:Text {

		/// <summary> This event is called for every individual key press before the keypress is applied to the content. You can use KeyPressArgs to change how the object responds to specific keys. </summary>
		public EventHandler<KeyPressArgs> OnKeypress;

		public HashSet<string> Whitelist;
		public int Selection = 0;

		public bool CaretEnabled = true;
		private int CaretBlinkTimer = 0;
		public int CaretBlinkTime = 30;
		private bool Updating = false;
		public bool AllowNewline = false;
		public bool AllowHScroll = true;
		public bool AllowVScroll = true;
		public int HPadding = 2;
		public int VPadding = 2;
		public int MouseScrollSpeed = 3;
		private TextMeasurer Measurer = new TextMeasurer();

		/// <summary> Used only for calculating up and down caret movement. Locks horizontal position when LastMoveWasVertical </summary>
		private Point SelectPoint;
		/// <summary> Prevents horizontal migration when moving the caret up and down. </summary>
		private bool LastMoveWasVertical = false;
		/// <summary> Offset to use the center of the IBeam mouse cursor. </summary>
		private static Point CursorOffset = new Point(3, 0); //HACK would be nice if this could measure actual cursor size for systems with different size cursors.

		public Color ColorSelectBack = Color.White;
		public Color ColorSelectText = Color.Black;
		private int SelectionStart;
		private bool SelectingBlock = false;

		public TextInput(Font f) : base(f) {
		}

		protected virtual bool CaretIsDrawable {
			get { return Updating; }
		}

		public override void Draw(SpriteBatch s, Color c) {
			base.Draw(s, c);

			if (SelectingBlock && SelectionStart != Selection) {
				DrawSelectionBlock(s);
			} else if (CaretEnabled && CaretIsDrawable) {
				DrawCaret(s);
			}

			if (!Updating) {
				CaretBlinkTimer = 0;
				LastMoveWasVertical = false;
			} else {
				Updating = false;
			}
		}

		protected virtual void DrawCaret(SpriteBatch s) {
			if (CaretBlinkTimer > CaretBlinkTime) return;

			Rectangle select = Measurer.GetCharRect(Selection);

			select.Width = Scale;
			select.X -= Scale;
			select.Y -= Scale;
			select.Height += Scale;

			if (UseMask) {
				select = Rectangle.Intersect(select, Mask);
				if (select == Rectangle.Empty) return;
			}

			s.Draw(Font.Graphic, select, Font.Pixel, Color);

			if (LastMoveWasVertical) {
				SelectPoint = new Point(SelectPoint.X, select.Center.Y);
			} else {
				SelectPoint = select.Center;
			}
		}

		protected virtual void DrawSelectionBlock(SpriteBatch s) {
			int start = Math.Min(SelectionStart, Selection);
			int end = Math.Max(SelectionStart, Selection);
			end--;
			for (int i = start; i < end && i < Content.Length; i++) {
				if (Content[i] == Font.Newline) {
					DrawSelectionBlockLine(s, start, i);
					start = i + 1;
				}
			}
			if (end >= Content.Length) end = Content.Length - 1;
			if (start >= Content.Length) start = Content.Length - 1;
			DrawSelectionBlockLine(s, start, end);
		}

		private void DrawSelectionBlockLine(SpriteBatch s, int start, int end) {
			var r = Rectangle.Union(Measurer.GetCharRect(start), Measurer.GetCharRect(end));
			var textlocation = r.Location;
			r.Y -= Scale;
			r.Height += Scale;
			s.Draw(Font.Graphic, r, Font.Pixel, ColorSelectBack);
			string text = Content.Substring(start, end - start + 1);
			Draw(s, ColorSelectText, textlocation, text);
		}

		public string GetSelectedText() {
			int start = Math.Min(Selection, SelectionStart);
			int end = Math.Max(Selection, SelectionStart);
			return Content.Substring(start, end - start);
		}

		public virtual void Update(KeyboardInputManager k) { //TODO test this
			if (k.Blocked) return;

			Updating = true;
			if (Selection < 0) Selection = 0;
			if (Selection > Content.Length) Selection = Content.Length;
			var upper = k.CheckUpperCase();
			var oldcontent = Content;
			var oldselect = Selection;

			Keys[] keys = k.GetKeysPressed();
			for (int i = 0; i < keys.Count(); i++) {
				var key = keys[i];

				var args = new KeyPressArgs(Content, key, k.KeyToString(key), Selection);
				OnKeypress?.Invoke(this, args);
				if (args.DropInput) continue;
				Content = args.Content;
				key = args.Key;
				Selection = args.Select;

				if (key == Keys.Enter && !AllowNewline) {
					continue;
				}

				if (k.ControlDown()) {
					if (key == Keys.C) {
						PressedCopy();
						continue;
					}

					if (key == Keys.X) {
						PressedCut();
						continue;
					}

					if (key == Keys.V) { //TODO pasting skips OnKeypress, fix this
						PressedPaste();
						continue;
					}
				}

				if (key == Keys.Back) {
					PressedBackspace();
					continue;
				}

				if (key == Keys.Delete) {
					PressedDelete();
					continue;
				}

				if (key == Keys.Left) {
					PressedLeft();
					continue;
				}

				if (key == Keys.Right) {
					PressedRight();
					continue;
				}

				if (key == Keys.Up) {
					PressedUp();
					continue;
				}

				if (key == Keys.Down) {
					PressedDown();
					continue;
				}

				if (args.String != null) {
					PressedString(args.String);
				}
			}

			if (Selection < 0) Selection = 0;
			if (Selection > Content.Length) Selection = Content.Length;

			if (Measurer.Outdated) {
				Measurer.Update(this);
			}

			if (UseMask && Selection != oldselect) {
				if (AllowHScroll) UpdateHScroll();
				if (AllowVScroll) UpdateVScroll();
			}

			CaretBlinkTimer++;
			if (CaretBlinkTimer > CaretBlinkTime * 2) {
				CaretBlinkTimer = 0;
			}

			if (oldcontent != Content) OnContentChanged?.Invoke(this, new EventArgs());
		}

		private void UpdateHScroll() {
			var rect = Measurer.GetCharRect(Selection);
			if (rect.Left + (HPadding * Scale) > Mask.Right) Scroll.X += (rect.Left + (HPadding * Scale)) - Mask.Right;
			if (rect.Left - (HPadding * Scale) < Mask.Left) Scroll.X -= Mask.Left - (rect.Left - (HPadding * Scale));
		}

		private void UpdateVScroll() {
			if (!Content.Contains(Font.Newline)) return; //HACK bad way of doing this
			var rect = Measurer.GetCharRect(Selection);
			if (rect.Bottom + (VPadding * Scale) > Mask.Bottom) Scroll.Y += (rect.Bottom + (VPadding * Scale)) - Mask.Bottom;
			if (rect.Top - (VPadding * Scale) < Mask.Top) Scroll.Y -= Mask.Top - (rect.Top - (VPadding * Scale));
		}

		private void ScrollUp() {
			Scroll.Y -= MouseScrollSpeed * Scale;

			if (Scroll.Y < 0) Scroll.Y = 0;
		}

		private void ScrollDown() {
			var rect = Measurer.GetCharRect(Content.Length);
			Scroll.Y += MouseScrollSpeed * Scale;
			rect.Y -= MouseScrollSpeed * Scale;

			if (rect.Bottom + (VPadding * Scale) < Mask.Bottom) Scroll.Y -= Mask.Bottom - (rect.Bottom + (VPadding * Scale));
		}

		private void PressedCut() {
			int start = Math.Min(Selection, SelectionStart);
			int end = Math.Max(Selection, SelectionStart);
			System.Windows.Forms.Clipboard.SetText(Content.Substring(start, end - start));

			Content = Content.Remove(start, end - start);
			Selection = start;

			SelectingBlock = false;
			CaretBlinkTimer = 0;
			LastMoveWasVertical = false;
		}

		private void PressedCopy() {
			int start = Math.Min(Selection, SelectionStart);
			int end = Math.Max(Selection, SelectionStart);
			System.Windows.Forms.Clipboard.SetText(Content.Substring(start, end - start));
		}

		private void PressedPaste() {
			string paste = System.Windows.Forms.Clipboard.GetText();
			if (SelectingBlock && Selection != SelectionStart) {
				DeleteSelection();
			}

			paste = paste.Replace(Environment.NewLine, Font.Newline.ToString());
			paste = paste.Replace('\t', ' ');

			foreach (char c in paste) {
				var args = new KeyPressArgs(Content, Keys.None, c.ToString(), Selection);
				OnKeypress?.Invoke(this, args);
				if (args.DropInput) continue;
				Content = args.Content;
				Selection = args.Select;

				if (c == Font.Newline && !AllowNewline) {
					continue;
				}

				if (args.String != null) {
					PressedString(args.String);
				}
			}

			SelectingBlock = false;
			LastMoveWasVertical = false;
		}

		private void PressedBackspace() {
			if (Content.Length == 0) return;

			if (SelectingBlock && Selection != SelectionStart) {
				DeleteSelection();
				return;
			}

			if (Selection > 0) {
				if (Selection == Content.Length) {
					Content = Content.Substring(0, Content.Length - 1);
				} else {
					Content = Content.Substring(0, Selection - 1) + Content.Substring(Selection);
				}
				Selection--;

				CaretBlinkTimer = 0;
				LastMoveWasVertical = false;
			}
		}

		private void PressedDelete() {
			if (Content.Length == 0) return;

			if (SelectingBlock && Selection != SelectionStart) {
				DeleteSelection();
				return;
			}

			if (Selection >= Content.Length) return;

			if (Selection == 0) {
				Content = Content.Substring(1);
			} else {
				Content = Content.Substring(0, Selection) + Content.Substring(Selection + 1);
			}
			CaretBlinkTimer = 0;
			LastMoveWasVertical = false;
		}

		private void DeleteSelection() {
			int start = Math.Min(Selection, SelectionStart);
			int end = Math.Max(Selection, SelectionStart);
			Content = Content.Remove(start, end - start);
			Selection = start;

			SelectingBlock = false;
			CaretBlinkTimer = 0;
			LastMoveWasVertical = false;
		}

		private void PressedLeft() {
			SelectingBlock = false;
			CaretBlinkTimer = 0;
			LastMoveWasVertical = false;

			if (SelectingBlock && Selection != SelectionStart) {
				Selection = Math.Min(SelectionStart, Selection);
				return;
			}
			Selection--;
		}

		private void PressedRight() {
			SelectingBlock = false;
			CaretBlinkTimer = 0;
			LastMoveWasVertical = false;

			if (SelectingBlock && Selection != SelectionStart) {
				Selection = Math.Max(SelectionStart, Selection);
				return;
			}
			Selection++;
		}

		private void PressedUp() {
			var y = Measurer.GetCharRect(Selection).Y;
			Selection = Measurer.FindNearestCharPos(new Point(SelectPoint.X, y) - new Point(0, (LineSpace + Font.CharHeight) * Scale));
			LastMoveWasVertical = true;
			SelectingBlock = false;
			CaretBlinkTimer = 0;
		}

		private void PressedDown() {
			var y = Measurer.GetCharRect(Selection).Y;
			Selection = Measurer.FindNearestCharPos(new Point(SelectPoint.X, y) + new Point(0, (LineSpace + Font.CharHeight) * Scale));
			LastMoveWasVertical = true;
			SelectingBlock = false;
			CaretBlinkTimer = 0;
		}

		private void PressedString(string c) {
			if (Whitelist != null && !Whitelist.Contains(c)) {
				return;
			}

			if (SelectingBlock && Selection != SelectionStart) {
				DeleteSelection();
			}

			Content = Content.Insert(Selection, c);
			Selection++;

			LastMoveWasVertical = true;
			SelectingBlock = false;
			CaretBlinkTimer = 0;
		}

		public virtual void MouseUpdate(MouseInputManager mouse) {
			if (mouse.Blocked) return;

			if (UseMask) {
				if (Mask.Contains(mouse.Position)) {
					CursorRequest.Push(MouseCursor.IBeam);
				}
			} else {
				var nearestchar = Measurer.FindNearestCharPos(mouse.Position + CursorOffset);
				var rect = Measurer.GetCharRect(nearestchar);
				rect.Inflate((LetterSpace + 1) * Scale, (LetterSpace + 1) * Scale);
				if (rect.Contains(mouse.Position)) {
					CursorRequest.Push(MouseCursor.IBeam);
				}
			}

			if (mouse.LeftPress) {
				Selection = Measurer.FindNearestCharPos(mouse.Position + CursorOffset);
				CaretBlinkTimer = 0;
				LastMoveWasVertical = false;
				SelectionStart = Selection;
				SelectingBlock = true;
			} else if (mouse.Left) {
				Selection = Measurer.FindNearestCharPos(mouse.Position + CursorOffset);
			}

			if (AllowNewline && AllowVScroll) {
				if (mouse.ScrollUp) {
					ScrollUp();
				} else if (mouse.ScrollDown) {
					ScrollDown();
				}
			}
		}

		public override string ToString() {
			return Content;
		}

		public void SelectAll() {
			SelectionStart = 0;
			Selection = Content.Length;
			SelectingBlock = true;
		}

		public void DeSelect() {
			SelectionStart = 0;
			Selection = 0;
			SelectingBlock = false;
		}

		public class KeyPressArgs:EventArgs {
			public string Content;
			public Keys Key;
			public string String;
			public bool DropInput = false;
			public int Select;

			public KeyPressArgs(string content, Keys key, string s, int select) {
				Content = content;
				Key = key;
				Select = select;
				String = s;
			}
		}

	}
}
