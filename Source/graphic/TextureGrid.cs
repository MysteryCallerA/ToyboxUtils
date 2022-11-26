using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Utils.save;

namespace Utils.graphic {
	public class TextureGrid:ITextureObject {

		public readonly int CellWidth;
		public readonly int CellHeight;
		public readonly int Columns;
		public readonly int Rows;
		public Point SelectedCell;

		public TextureGrid(Texture2D t, int cellWidth, int cellHeight) {
			Texture = t;
			CellWidth = cellWidth;
			CellHeight = cellHeight;
			Columns = t.Width / cellWidth;
			Rows = t.Height / cellHeight;
		}

		public Texture2D Texture {
			get; set;
		}

		public Rectangle GetCell(Point cellpos) {
			return new Rectangle(cellpos.X * CellWidth, cellpos.Y * CellHeight, CellWidth, CellHeight);
		}

		public Rectangle Source {
			get { return GetCell(SelectedCell); }
		}

		public int Width {
			get { return Source.Width; }
		}

		public int Height {
			get { return Source.Height; }
		}

		public TextureSelection GetSelection(Point cell) {
			return new TextureSelection(Texture, GetCell(cell));
		}

	}
}
