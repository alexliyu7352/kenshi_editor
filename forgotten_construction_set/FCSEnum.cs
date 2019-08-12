using System;
using System.Collections;
using System.Collections.Generic;

namespace forgotten_construction_set
{
	public class FCSEnum : IEnumerable<KeyValuePair<string, int>>, IEnumerable
	{
		private int maxValue;

		private Dictionary<string, int> values = new Dictionary<string, int>();

		public int Max
		{
			get
			{
				return this.maxValue;
			}
		}

		public FCSEnum()
		{
		}

		public void addValue(string key)
		{
			this.values.Add(key, this.maxValue);
			this.maxValue++;
		}

		public void addValue(string key, int value)
		{
			if (value >= this.maxValue)
			{
				this.maxValue = value + 1;
			}
			this.values.Add(key, value);
		}

		public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
		{
			return this.values.GetEnumerator();
		}

		public string name(int i)
		{
			string key;
			Dictionary<string, int>.Enumerator enumerator = this.values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, int> current = enumerator.Current;
					if (current.Value != i)
					{
						continue;
					}
					key = current.Key;
					return key;
				}
				return null;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return key;
		}

		public int parse(string s)
		{
			if (!this.values.ContainsKey(s))
			{
				return -1;
			}
			return this.values[s];
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.values.GetEnumerator();
		}
	}
}