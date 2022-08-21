using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.data {
	public class BiList2d<T> {

		private BiList<BiList<T>> Data;
		public T DefaultValue = default(T);
		public bool AutoExpand = true;

		public BiList2d() {
		}

		public BiList2d(int left, int top) {
			Data = new BiList<BiList<T>>(left);
			Data.Add(new BiList<T>(top));
		}

		public BiList2d(int left, int top, int right, int bot, T defaultVal) {
			DefaultValue = defaultVal;
			Data = new BiList<BiList<T>>(left);
			for (int col = left; col <= right; col++) {
				var b = new BiList<T>(top);
				for (int row = top; row <= bot; row++) {
					b.Add(DefaultValue);
				}
				Data.Add(b);
			}
		}

		public bool IsEmpty {
			get { return Data == null || Data.Count == 0; }
		}

		public T this[int col, int row] {
			get {
				return Get(col, row);
			}
			set {
				Data[col][row] = value;
			}
		}

		public T Get(int col, int row) {
			return Data[col][row];
		}

		public bool TryGet(int col, int row, out T output) {
			if (!IsInBounds(col, row)) {
				output = default(T);
				return false;
			}
			output = Get(col, row);
			return true;
		}

		public void Set(int col, int row, T value) {
			if (Data == null) {
				Data = new BiList<BiList<T>>(col);
				Data.Add(new BiList<T>(row));
				Data[col].Add(value);
				return;
			}
			if (AutoExpand && !IsInBounds(col, row)) {
				ExpandToInclude(col, row);
			}

			Data[col][row] = value;
		}

		public void Set(Point p, T value) {
			Set(p.X, p.Y, value);
		}

		public bool TrySet(int col, int row, T value) {
			if (!IsInBounds(col, row)) {
				if (AutoExpand) {
					ExpandToInclude(col, row);
				} else {
					return false;
				}
			}
			Set(col, row, value);
			return true;
		}

		public void ExpandToInclude(int col, int row) {
			if (col > Right) {
				ExpandRight(col - Right);
			}
			if (col < Left) {
				ExpandLeft(-(col - Left));
			}
			if (row > Bottom) {
				ExpandDown(row - Bottom);
			}
			if (row < Top) {
				ExpandUp(-(row - Top));
			}
		}

		public void ExpandRight(int num = 1) {
			for (int i = 0; i < num; i++) {
				var b = new BiList<T>(Top);
				for (int r = 0; r < Rows; r++) {
					b.Add(DefaultValue);
				}
				Data.Add(b);
			}
		}

		public void ExpandLeft(int num = 1) {
			for (int i = 0; i < num; i++) {
				var b = new BiList<T>(Top);
				for (int r = 0; r < Rows; r++) {
					b.Add(DefaultValue);
				}
				Data.AddLeft(b);
			}
		}

		public void ExpandDown(int num = 1) {
			for (int col = Left; col <= Right; col++) {
				for (int i = 0; i < num; i++) {
					Data[col].Add(DefaultValue);
				}
			}
		}

		public void ExpandUp(int num = 1) {
			for (int col = Left; col <= Right; col++) {
				for (int i = 0; i < num; i++) {
					Data[col].AddLeft(DefaultValue);
				}
			}
		}

		public bool IsInBounds(int col, int row) {
			if (Data.IsInBounds(col) && Data[col].IsInBounds(row)) {
				return true;
			}
			return false;
		}

		public void SetBounds(int left, int top, int right, int bot) {
			if (left < Left) {
				ExpandLeft(-(left - Left));
			}
			if (right > Right) {
				ExpandRight(right - Right);
			}
			if (top < Top) {
				ExpandUp(-(top - Top));
			}
			if (bot > Bottom) {
				ExpandDown(bot - Bottom);
			}
		}

		public int Left {
			get { return Data.Left; }
		}

		public int Right {
			get { return Data.Right; }
		}

		public int Top {
			get { return Data[Data.Left].Left; }
		}

		public int Bottom {
			get { return Data[Data.Left].Right; }
		}

		public int Cols {
			get {
				return Data.Count;
			}
		}

		public int Rows {
			get {
				return Data[Data.Left].Count;
			}
		}

	}
}
