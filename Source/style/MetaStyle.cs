using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.style {
	
	public abstract class MetaStyle:Style {

		protected abstract Style GetCurrentStyle();

		public override int Scale {
			get { return GetCurrentStyle().Scale; }
			set { GetCurrentStyle().Scale = value; }
		}

		public override int PaddingLeft {
			get { return GetCurrentStyle().PaddingLeft; }
			set { GetCurrentStyle().PaddingLeft = value; }
		}
		public override int PaddingRight {
			get { return GetCurrentStyle().PaddingRight; }
			set { GetCurrentStyle().PaddingRight = value; }
		}
		public override int PaddingTop {
			get { return GetCurrentStyle().PaddingTop; }
			set { GetCurrentStyle().PaddingTop = value; }
		}
		public override int PaddingBottom {
			get { return GetCurrentStyle().PaddingBottom; }
			set { GetCurrentStyle().PaddingBottom = value; }
		}

		public override int MarginLeft {
			get { return GetCurrentStyle().MarginLeft; }
			set { GetCurrentStyle().MarginLeft = value; }
		}
		public override int MarginRight {
			get { return GetCurrentStyle().MarginRight; }
			set { GetCurrentStyle().MarginRight = value; }
		}
		public override int MarginTop {
			get { return GetCurrentStyle().MarginTop; }
			set { GetCurrentStyle().MarginTop = value; }
		}
		public override int MarginBottom {
			get { return GetCurrentStyle().MarginBottom; }
			set { GetCurrentStyle().MarginBottom = value; }
		}

		public override Color ColorBack {
			get { return GetCurrentStyle().ColorBack; }
			set { GetCurrentStyle().ColorBack = value; }
		}
		public override Color ColorText {
			get { return GetCurrentStyle().ColorText; }
			set { GetCurrentStyle().ColorText = value; }
		}
		public override Color ColorBorder {
			get { return GetCurrentStyle().ColorBorder; }
			set { GetCurrentStyle().ColorBorder = value; }
		}

		public override int BorderThickness {
			get { return GetCurrentStyle().BorderThickness; }
			set { GetCurrentStyle().BorderThickness = value; }
		}

	}
}
