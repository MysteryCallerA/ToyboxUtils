using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.data {
	public class BiList<T> {

		private int ZeroPos = 0;
		private List<T> Data = new List<T>();

		public BiList() {

		}

		public BiList(int left) {
			ZeroPos = -left;
		}

		public T this[int index] {
			get { return Data[index + ZeroPos]; }
			set { Data[index + ZeroPos] = value; }
		}

		public void Add(T value) {
			Data.Add(value);
		}

		public void AddLeft(T value) {
			Data.Insert(0, value);
			ZeroPos++;
		}

		public void Insert(int index, T value) {
			Data.Insert(index + ZeroPos, value);
		}

		public int Count {
			get { return Data.Count; }
		}

		public T First {
			get { return Data.First(); }
		}

		public T Last {
			get { return Data.Last(); }
		}

		public int Left {
			get { return -ZeroPos; }
		}

		public int Right {
			get { return (Data.Count - 1) - ZeroPos; }
		}

		public void Clear() {
			Data.Clear();
			ZeroPos = 0;
		}

		public bool Contains(T value) {
			return Data.Contains(value);
		}

		public bool IsInBounds(int index) {
			if (index >= Left && index <= Right) {
				return true;
			}
			return false;
		}

		public void Remove(T value) {
			Data.Remove(value);
		}

		public void RemoveAt(int index) {
			Data.RemoveAt(index + ZeroPos);
		}

	}
}
