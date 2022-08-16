using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils {
	public static class CursorRequest {

		private static MouseCursor Requested;
		private static MouseCursor Prev = MouseCursor.Arrow;

		public static List<MouseCursor> Priority = new List<MouseCursor>() {
			MouseCursor.SizeNESW, MouseCursor.SizeNWSE, MouseCursor.SizeNS, MouseCursor.SizeWE, MouseCursor.IBeam, MouseCursor.Hand, MouseCursor.Arrow
		};

		public static void Push(MouseCursor m) {
			for (int i = 0; i < Priority.Count; i++) {
				if (Priority[i] == Requested) return;
				if (Priority[i] == m) {
					Requested = m;
					return;
				}
			}
		}

		public static void Apply(GameWindow w) {
			var m = Mouse.GetState();
			if (!w.ClientBounds.Contains(m.Position + w.ClientBounds.Location)) {
				Requested = null;
				return;
			}

			var setto = MouseCursor.Arrow;
			if (Requested != null) setto = Requested;
			Requested = null;

			if (setto != Prev) {
				Mouse.SetCursor(setto);
			}
			Prev = setto;
		}

	}
}
