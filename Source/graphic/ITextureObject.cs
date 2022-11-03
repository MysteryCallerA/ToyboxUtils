using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.graphic {

	public interface ITextureObject {
	
		public Texture2D Texture {
			get;
		}

		public Rectangle Source {
			get;
		}

		public int Width {
			get;
		}

		public int Height {
			get;
		}
	
	}
}
