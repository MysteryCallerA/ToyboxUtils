using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.style {
	
	public class ActiveStyle:MetaStyle {

		public string State = "";

		public Style DefaultStyle;
		public Dictionary<string, Style> Styles = new Dictionary<string, Style>();

		public ActiveStyle() {
			DefaultStyle = new Style();
		}

		public ActiveStyle(Style s) {
			DefaultStyle = s;
		}

		protected override Style GetCurrentStyle() {
			if (State == "" || !Styles.ContainsKey(State)) {
				return DefaultStyle;
			}
			return Styles[State];
		}

		public override void Select(string state, string element) {
			State = state;
		}



	}
}
