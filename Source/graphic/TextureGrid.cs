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
	public class TextureGrid:ITextureObject, IXmlSaveable {

		public Texture2D Texture;
		public readonly int CellWidth;
		public readonly int CellHeight;
		public Vector2 Origin = Vector2.Zero;
		public readonly int Columns;
		public readonly int Rows;

		public TextureGrid(Texture2D t, int cellWidth, int cellHeight) {
			Texture = t;
			CellWidth = cellWidth;
			CellHeight = cellHeight;
			Columns = t.Width / cellWidth;
			Rows = t.Height / cellHeight;
		}

		public void Draw(SpriteBatch s, Point dest, int frame, Vector2 scale) {
			Draw(s, dest, FrameToCell(frame), scale, Color.White, 0, SpriteEffects.None);
		}

		public void Draw(SpriteBatch s, Point dest, int frame, Vector2 scale, Color c, float rotation, SpriteEffects effects) {
			Draw(s, dest, FrameToCell(frame), scale, c, rotation, effects);
		}

		public void Draw(SpriteBatch s, Point dest, Point cell, Vector2 scale) {
			Draw(s, dest, cell, scale, Color.White, 0, SpriteEffects.None);
		}

		public void Draw(SpriteBatch s, Point dest, Point cell, Vector2 scale, Color c, float rotation, SpriteEffects effects) {
			var source = new Rectangle(cell.X * CellWidth, cell.Y * CellHeight, CellWidth, CellHeight);
			var destrect = new Rectangle(dest.X, dest.Y, (int)(source.Width * scale.X), (int)(source.Height * scale.Y));
			s.Draw(Texture, destrect, source, c, rotation, Origin, effects, 0);
		}

		public void Draw(SpriteBatch s, Rectangle dest, Point cell) {
			Draw(s, dest, cell, Color.White, 0, SpriteEffects.None);
		}

		public void Draw(SpriteBatch s, Rectangle dest, Point cell, Color c, float rotation, SpriteEffects effects) {
			var source = new Rectangle(cell.X * CellWidth, cell.Y * CellHeight, CellWidth, CellHeight);
			s.Draw(Texture, dest, source, c, rotation, Origin, effects, 0);
		}

		/// <summary> Coordinates are given in cell space not texture space. </summary>
		public int CellToFrame(int x, int y) {
			return (y * Columns) + x;
		}

		public int CellToFrame(Point p) {
			return CellToFrame(p.X, p.Y);
		}

		public Point FrameToCell(int frame) {
			return new Point(frame % Columns, (int)Math.Floor((float)frame / Columns));
		}

		public Point TextureToCell(Point p) {
			return new Point(p.X / CellWidth, p.Y / CellHeight);
		}

		public Rectangle TextureToCell(Rectangle r) {
			var output = new Rectangle(r.X / CellWidth, r.Y / CellHeight, 0, 0);
			output.Width = (r.Right / CellWidth) - output.X;
			output.Height = (r.Bottom / CellHeight) - output.Y;
			return output;
		}

		public Point CellToTexture(Point p) {
			return new Point(p.X * CellWidth, p.Y * CellHeight);
		}

		public Rectangle CellToTexture(Rectangle r) {
			return new Rectangle(r.X * CellWidth, r.Y * CellHeight, r.Width * CellWidth, r.Height * CellHeight);
		}

		public int GetWidth(int frame, float scale) {
			return (int)(CellWidth * scale);
		}

		public int GetHeight(int frame, float scale) {
			return (int)(CellHeight * scale);
		}

		public Point GetDimensions(int frame, Vector2 scale) {
			return new Point(GetWidth(frame, scale.X), GetHeight(frame, scale.Y));
		}

		/// <summary> Returns texture space rectangle from cell space. </summary>
		public Rectangle GetCell(Point cell) {
			return new Rectangle(cell.X * CellWidth, cell.Y * CellHeight, CellWidth, CellHeight);
		}

		/// <summary> Returns texture space rectangle from frame number. </summary>
		public Rectangle GetCell(int frame) {
			return GetCell(FrameToCell(frame));
		}

		/// <summary> Returns texture space rectangle from cell space. The input rectangle describes multiple cells. </summary>
		public Rectangle GetCells(Rectangle cells) {
			return new Rectangle(cells.X * CellWidth, cells.Y * CellHeight, cells.Width * CellWidth, cells.Height * CellHeight);
		}

		public void Dispose() {
			Texture.Dispose();
		}

		public void Save(XmlWriter writer) {
			writer.WriteStartElement("texturegrid");

			writer.WriteElementString("source", Texture.Name);
			writer.WriteElementString("cellwidth", CellWidth.ToString());
			writer.WriteElementString("cellheight", CellHeight.ToString());

			writer.WriteEndElement();
		}

		public int FrameCount {
			get { return Columns * Rows; }
		}

	}
}
