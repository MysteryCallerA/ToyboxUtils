using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.data {
	public class StableList<T>:IEnumerable {

		private readonly List<T> Content = new List<T>();
		private readonly Stack<int> EmptyBlocks = new Stack<int>();

		public StableList() {

		}

		public T this[int index] {
			get { return Content[index]; }
		}

		public int Add(T o) {
			if (EmptyBlocks.Count > 0) {
				var output = EmptyBlocks.Pop();
				Content[output] = o;
				return output;
			}
			Content.Add(o);
			return Content.Count - 1;
		}

		public void RemoveAt(int i) {
			Content[i] = default(T);
			EmptyBlocks.Push(i);
		}

		public void Remove(T value) {
			int i = Content.IndexOf(value);
			RemoveAt(i);
		}

		public int Count {
			get { return Content.Count; }
		}

		public void Clear() {
			Content.Clear();
			EmptyBlocks.Clear();
		}

		public IEnumerator GetEnumerator() {
			return Content.GetEnumerator();
		}

		/// <summary> Checks if the id is in bounds and isn't an empty block. Faster to use IsIdValid if type is nullable. </summary>
		public bool IsIdFilled(int id) {
			if (!IsIdValid(id)) return false;
			if (EmptyBlocks.Contains(id)) return false;
			return true;
		}

		/// <summary> Checks if the id is in bounds but doesn't check if it's an empty block.<br></br> Useful for nullable types since it's faster to get and check if null instead of searching EmptyBlocks. </summary>
		public bool IsIdValid(int id) {
			if (id < 0 || id >= Content.Count) return false;
			return true;
		}

	}
}
