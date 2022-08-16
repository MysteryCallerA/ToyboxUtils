using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utils.text.TextInput;

namespace Utils.text {
	/// <summary> Useful events for TextInput.OnKeypress </summary>
	public static class KeypressEvents {

		public static void EventToUpper(object o, KeyPressArgs k) {
			k.String = k.String.ToUpper();
		}

		public static void EventToLower(object o, KeyPressArgs k) {
			k.String = k.String.ToLower();
		}

		public static void EventSingleDecimalOnly(object o, KeyPressArgs k) {
			if (k.String == "." && k.Content.Contains('.')) {
				k.DropInput = true;
			}
		}

		public static void EventLeadingMinusOnly(object o, KeyPressArgs k) {
			if (k.String == "-" && (k.Content.Contains('-') || k.Select != 0)) {
				k.DropInput = true;
			}
		}

		public static void EventBlockNewline(object o, KeyPressArgs k) {
			if (k.Key == Keys.Enter) {
				k.DropInput = true;
			}
		}

	}
}
