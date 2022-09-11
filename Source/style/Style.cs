using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utils.style {
	
	public class Style {

		public virtual int Scale {
			get; set;
		} = 1;

		public virtual int PaddingLeft {
			get; set;
		} = 0;
		public virtual int PaddingRight {
			get; set;
		} = 0;
		public virtual int PaddingTop {
			get; set;
		} = 0;
		public virtual int PaddingBottom {
			get; set;
		} = 0;

		public virtual int MarginLeft {
			get; set;
		} = 0;
		public virtual int MarginRight {
			get; set;
		} = 0;
		public virtual int MarginTop {
			get; set;
		} = 0;
		public virtual int MarginBottom {
			get; set;
		} = 0;

		public virtual Color ColorBack {
			get; set;
		} = Color.Black;
		public virtual Color ColorText {
			get; set;
		} = Color.White;
		public virtual Color ColorBorder {
			get; set;
		} = Color.Transparent;

		public virtual int BorderThickness {
			get; set;
		} = 0;

		public virtual Style GetCopy() {
			Style output = new Style();
			foreach (PropertyInfo property in typeof(Style).GetProperties().Where(p => p.CanWrite)) {
				property.SetValue(output, property.GetValue(this, null), null);
			}
			return output;
		}

		public int VPadding {
			set {
				PaddingTop = value;
				PaddingBottom = value;
			}
		}

		public int HPadding {
			set {
				PaddingLeft = value;
				PaddingRight = value;
			}
		}

		public int Padding {
			set {
				PaddingBottom = value;
				PaddingTop = value;
				PaddingLeft = value;
				PaddingRight = value;
			}
		}

		public int VMargin {
			set {
				MarginTop = value;
				MarginBottom = value;
			}
		}

		public int HMargin {
			set {
				MarginLeft = value;
				MarginRight = value;
			}
		}

		public int Margin {
			set {
				MarginBottom = value;
				MarginTop = value;
				MarginLeft = value;
				MarginRight = value;
			}
		}

	}
}
