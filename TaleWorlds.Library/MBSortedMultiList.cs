using System;
using System.Collections;
using System.Collections.Generic;

namespace TaleWorlds.Library
{
	// Token: 0x02000070 RID: 112
	public class MBSortedMultiList<TKey, TValue> : IReadOnlyList<TValue>, IEnumerable<TValue>, IEnumerable, IReadOnlyCollection<TValue>, IMBCollection where TKey : IComparable<TKey>
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x0000DE01 File Offset: 0x0000C001
		public MBSortedMultiList<TKey, TValue>.ComparerType Comparer
		{
			get
			{
				return this._comparerType;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x0000DE09 File Offset: 0x0000C009
		private bool IsAscending
		{
			get
			{
				return this._comparerType == MBSortedMultiList<TKey, TValue>.ComparerType.Ascending;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0000DE14 File Offset: 0x0000C014
		private bool IsDescending
		{
			get
			{
				return this._comparerType == MBSortedMultiList<TKey, TValue>.ComparerType.Descending;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x0000DE1F File Offset: 0x0000C01F
		public int Count
		{
			get
			{
				return this._items.Count;
			}
		}

		// Token: 0x17000060 RID: 96
		public TValue this[int index]
		{
			get
			{
				return this._items[index].Value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x0000DE50 File Offset: 0x0000C050
		public TValue FirstValue
		{
			get
			{
				return this._items[0].Value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x0000DE74 File Offset: 0x0000C074
		public TValue LastValue
		{
			get
			{
				return this._items[this._items.Count - 1].Value;
			}
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000DEA1 File Offset: 0x0000C0A1
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000DEA9 File Offset: 0x0000C0A9
		public MBSortedMultiList(IComparer<TKey> customComparer)
		{
			this._items = new List<KeyValuePair<TKey, TValue>>();
			this.SetCustomComparer(customComparer);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000DEC3 File Offset: 0x0000C0C3
		public MBSortedMultiList(bool isAscending = true)
		{
			this._items = new List<KeyValuePair<TKey, TValue>>();
			this.SetDefaultComparer(isAscending);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000DEDD File Offset: 0x0000C0DD
		public bool Contains(TKey key)
		{
			return this.FirstIndexOf(key) >= 0;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000DEEC File Offset: 0x0000C0EC
		public bool Contains(TKey key, TValue value)
		{
			return this.FirstIndexOf(key, value) >= 0;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000DEFC File Offset: 0x0000C0FC
		public KeyValuePair<TKey, TValue> Get(int index)
		{
			return this._items[index];
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000DF0C File Offset: 0x0000C10C
		public int FirstIndexOf(TKey key)
		{
			if (this._items.Count > 0)
			{
				int num = this.LowerBound(key);
				if (num < this._items.Count)
				{
					TKey key2 = this._items[num].Key;
					if (key2.CompareTo(key) == 0)
					{
						return num;
					}
				}
			}
			return -1;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000DF68 File Offset: 0x0000C168
		public int FirstIndexOf(TKey key, TValue value)
		{
			if (this._items.Count > 0)
			{
				int i = this.LowerBound(key);
				EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
				while (i < this._items.Count)
				{
					TKey key2 = this._items[i].Key;
					if (key2.CompareTo(key) != 0)
					{
						break;
					}
					if (@default.Equals(this._items[i].Value, value))
					{
						return i;
					}
					i++;
				}
			}
			return -1;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000DFEC File Offset: 0x0000C1EC
		public int LastIndexOf(TKey key)
		{
			if (this._items.Count > 0)
			{
				int num = this.UpperBound(key) - 1;
				if (num >= 0 && num < this._items.Count)
				{
					TKey key2 = this._items[num].Key;
					if (key2.CompareTo(key) == 0)
					{
						return num;
					}
				}
			}
			return -1;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000E04C File Offset: 0x0000C24C
		public int LastIndexOf(TKey key, TValue value)
		{
			if (this._items.Count > 0)
			{
				int i = this.UpperBound(key) - 1;
				EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
				while (i >= 0)
				{
					TKey key2 = this._items[i].Key;
					if (key2.CompareTo(key) != 0)
					{
						break;
					}
					if (@default.Equals(this._items[i].Value, value))
					{
						return i;
					}
					i--;
				}
			}
			return -1;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000E0C8 File Offset: 0x0000C2C8
		public bool All(Predicate<KeyValuePair<TKey, TValue>> predicate)
		{
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this._items)
			{
				if (!predicate(keyValuePair))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000E124 File Offset: 0x0000C324
		public bool Any(Predicate<KeyValuePair<TKey, TValue>> predicate)
		{
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this._items)
			{
				if (predicate(keyValuePair))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000E180 File Offset: 0x0000C380
		public IEnumerator<TValue> GetValues(TKey key)
		{
			int num = this.LowerBound(key);
			List<KeyValuePair<TKey, TValue>> items = this._items;
			int num2;
			if (num < this._items.Count)
			{
				TKey key2 = this._items[num].Key;
				if (key2.CompareTo(key) == 0)
				{
					num2 = num;
					goto IL_0050;
				}
			}
			num2 = this._items.Count;
			IL_0050:
			return new MBSortedMultiList<TKey, TValue>.SMLKeyValueEnumerator(items, key, num2);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000E1E8 File Offset: 0x0000C3E8
		public bool Find(Predicate<KeyValuePair<TKey, TValue>> predicate, out KeyValuePair<TKey, TValue> found, bool searchForward = true)
		{
			if (searchForward)
			{
				for (int i = 0; i < this._items.Count; i++)
				{
					KeyValuePair<TKey, TValue> keyValuePair = this._items[i];
					if (predicate(keyValuePair))
					{
						found = keyValuePair;
						return true;
					}
				}
			}
			else
			{
				for (int j = this._items.Count - 1; j >= 0; j--)
				{
					KeyValuePair<TKey, TValue> keyValuePair2 = this._items[j];
					if (predicate(keyValuePair2))
					{
						found = keyValuePair2;
						return true;
					}
				}
			}
			found = default(KeyValuePair<TKey, TValue>);
			return false;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000E270 File Offset: 0x0000C470
		public int FindIndex(Predicate<KeyValuePair<TKey, TValue>> predicate, bool searchForward = true)
		{
			if (searchForward)
			{
				for (int i = 0; i < this._items.Count; i++)
				{
					KeyValuePair<TKey, TValue> keyValuePair = this._items[i];
					if (predicate(keyValuePair))
					{
						return i;
					}
				}
			}
			else
			{
				for (int j = this._items.Count - 1; j >= 0; j--)
				{
					KeyValuePair<TKey, TValue> keyValuePair2 = this._items[j];
					if (predicate(keyValuePair2))
					{
						return j;
					}
				}
			}
			return -1;
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000E2E4 File Offset: 0x0000C4E4
		public MBList<KeyValuePair<TKey, TValue>> FindAll(Predicate<KeyValuePair<TKey, TValue>> predicate)
		{
			MBList<KeyValuePair<TKey, TValue>> mblist = new MBList<KeyValuePair<TKey, TValue>>();
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this._items)
			{
				if (predicate(keyValuePair))
				{
					mblist.Add(keyValuePair);
				}
			}
			return mblist;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000E348 File Offset: 0x0000C548
		public void Add(TKey key, TValue value)
		{
			KeyValuePair<TKey, TValue> keyValuePair = new KeyValuePair<TKey, TValue>(key, value);
			int num = this.UpperBound(key);
			this._items.Insert(num, keyValuePair);
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000E373 File Offset: 0x0000C573
		public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
		{
			this._items.AddRange(items);
			this._items.Sort(this._pairComparer);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000E394 File Offset: 0x0000C594
		public bool Remove(TKey key, TValue value)
		{
			int num = this.LastIndexOf(key, value);
			if (num >= 0)
			{
				this._items.RemoveAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000E3C0 File Offset: 0x0000C5C0
		public bool Remove(TKey key)
		{
			int num = this.LastIndexOf(key);
			if (num >= 0)
			{
				this._items.RemoveAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000E3E8 File Offset: 0x0000C5E8
		public int RemoveAll(Predicate<KeyValuePair<TKey, TValue>> predicate)
		{
			int num = 0;
			for (int i = this._items.Count - 1; i >= 0; i--)
			{
				if (predicate(this._items[i]))
				{
					this._items.RemoveAt(i);
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000E434 File Offset: 0x0000C634
		public void RemoveAt(int index)
		{
			this._items.RemoveAt(index);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000E442 File Offset: 0x0000C642
		public void RemoveLast()
		{
			this._items.RemoveAt(this._items.Count - 1);
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000E45C File Offset: 0x0000C65C
		public void Clear()
		{
			this._items.Clear();
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000E469 File Offset: 0x0000C669
		public void SetCustomComparer(IComparer<TKey> customComparer)
		{
			this._keyComparer = customComparer;
			this._pairComparer = this.GetPairComparerFromKeyComparer();
			this._comparerType = MBSortedMultiList<TKey, TValue>.ComparerType.Custom;
			if (this._items.Count > 0)
			{
				this._items.Sort(this._pairComparer);
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000E4A4 File Offset: 0x0000C6A4
		public void SetDefaultComparer(bool isAscending = true)
		{
			bool flag = false;
			if (isAscending && this._comparerType != MBSortedMultiList<TKey, TValue>.ComparerType.Ascending)
			{
				this._keyComparer = MBSortedMultiList<TKey, TValue>.DefaultAscendingKeyComparer;
				this._pairComparer = this.GetPairComparerFromKeyComparer();
				this._comparerType = MBSortedMultiList<TKey, TValue>.ComparerType.Ascending;
				flag = true;
			}
			else if (!isAscending && this._comparerType != MBSortedMultiList<TKey, TValue>.ComparerType.Descending)
			{
				this._keyComparer = MBSortedMultiList<TKey, TValue>.DefaultDescendingKeyComparer;
				this._pairComparer = this.GetPairComparerFromKeyComparer();
				this._comparerType = MBSortedMultiList<TKey, TValue>.ComparerType.Descending;
				flag = true;
			}
			if (flag && this._items.Count > 0)
			{
				this._items.Sort(this._pairComparer);
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000E52F File Offset: 0x0000C72F
		public void Reverse()
		{
			if (this._comparerType == MBSortedMultiList<TKey, TValue>.ComparerType.Ascending)
			{
				this.SetDefaultComparer(false);
				return;
			}
			if (this._comparerType == MBSortedMultiList<TKey, TValue>.ComparerType.Descending)
			{
				this.SetDefaultComparer(true);
				return;
			}
			Debug.FailedAssert("Comparer type must not be custom", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\MBSortedMultiList.cs", "Reverse", 562);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000E56C File Offset: 0x0000C76C
		public override string ToString()
		{
			return string.Format("MBSortedMultiList[{0}, {1}], Count = {2}, Comparer Type = {3}", new object[]
			{
				typeof(TKey).Name,
				typeof(TValue).Name,
				this.Count,
				this._comparerType.ToString()
			});
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000E5CF File Offset: 0x0000C7CF
		public IEnumerator<TValue> GetEnumerator()
		{
			return new MBSortedMultiList<TKey, TValue>.SMLValueEnumerator(this._items);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000E5E4 File Offset: 0x0000C7E4
		private int LowerBound(TKey key)
		{
			int i = 0;
			int num = this._items.Count;
			while (i < num)
			{
				int num2 = (i + num) / 2;
				if (this._keyComparer.Compare(this._items[num2].Key, key) < 0)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2;
				}
			}
			return i;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000E638 File Offset: 0x0000C838
		private int UpperBound(TKey key)
		{
			int i = 0;
			int num = this._items.Count;
			while (i < num)
			{
				int num2 = (i + num) / 2;
				if (this._keyComparer.Compare(this._items[num2].Key, key) <= 0)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2;
				}
			}
			return i;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000E68B File Offset: 0x0000C88B
		private IComparer<KeyValuePair<TKey, TValue>> GetPairComparerFromKeyComparer()
		{
			return Comparer<KeyValuePair<TKey, TValue>>.Create((KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y) => this._keyComparer.Compare(x.Key, y.Key));
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x0000E69E File Offset: 0x0000C89E
		private static IComparer<TKey> DefaultAscendingKeyComparer
		{
			get
			{
				return Comparer<TKey>.Default;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0000E6A5 File Offset: 0x0000C8A5
		private static IComparer<TKey> DefaultDescendingKeyComparer
		{
			get
			{
				return Comparer<TKey>.Create((TKey x, TKey y) => y.CompareTo(x));
			}
		}

		// Token: 0x04000141 RID: 321
		private readonly List<KeyValuePair<TKey, TValue>> _items;

		// Token: 0x04000142 RID: 322
		private MBSortedMultiList<TKey, TValue>.ComparerType _comparerType;

		// Token: 0x04000143 RID: 323
		private IComparer<TKey> _keyComparer;

		// Token: 0x04000144 RID: 324
		private IComparer<KeyValuePair<TKey, TValue>> _pairComparer;

		// Token: 0x020000DA RID: 218
		public enum ComparerType
		{
			// Token: 0x040002CA RID: 714
			None,
			// Token: 0x040002CB RID: 715
			Custom,
			// Token: 0x040002CC RID: 716
			Ascending,
			// Token: 0x040002CD RID: 717
			Descending
		}

		// Token: 0x020000DB RID: 219
		private struct SMLValueEnumerator : IEnumerator<TValue>, IEnumerator, IDisposable
		{
			// Token: 0x0600077C RID: 1916 RVA: 0x00018D73 File Offset: 0x00016F73
			public SMLValueEnumerator(List<KeyValuePair<TKey, TValue>> list)
			{
				this._list = list;
				this._index = -1;
				this._current = default(TValue);
			}

			// Token: 0x0600077D RID: 1917 RVA: 0x00018D90 File Offset: 0x00016F90
			public bool MoveNext()
			{
				int num = this._index + 1;
				this._index = num;
				if (num < this._list.Count)
				{
					this._current = this._list[this._index].Value;
					return true;
				}
				return false;
			}

			// Token: 0x170000FA RID: 250
			// (get) Token: 0x0600077E RID: 1918 RVA: 0x00018DDD File Offset: 0x00016FDD
			public TValue Current
			{
				get
				{
					return this._current;
				}
			}

			// Token: 0x170000FB RID: 251
			// (get) Token: 0x0600077F RID: 1919 RVA: 0x00018DE5 File Offset: 0x00016FE5
			object IEnumerator.Current
			{
				get
				{
					return this._current;
				}
			}

			// Token: 0x06000780 RID: 1920 RVA: 0x00018DF2 File Offset: 0x00016FF2
			public void Dispose()
			{
			}

			// Token: 0x06000781 RID: 1921 RVA: 0x00018DF4 File Offset: 0x00016FF4
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040002CE RID: 718
			private readonly List<KeyValuePair<TKey, TValue>> _list;

			// Token: 0x040002CF RID: 719
			private int _index;

			// Token: 0x040002D0 RID: 720
			private TValue _current;
		}

		// Token: 0x020000DC RID: 220
		private struct SMLKeyValueEnumerator : IEnumerator<TValue>, IEnumerator, IDisposable
		{
			// Token: 0x06000782 RID: 1922 RVA: 0x00018DFB File Offset: 0x00016FFB
			public SMLKeyValueEnumerator(List<KeyValuePair<TKey, TValue>> list, TKey key, int startIndex)
			{
				this._list = list;
				this._key = key;
				this._index = startIndex - 1;
				this._current = default(TValue);
			}

			// Token: 0x06000783 RID: 1923 RVA: 0x00018E20 File Offset: 0x00017020
			public bool MoveNext()
			{
				this._index++;
				if (this._index < this._list.Count)
				{
					TKey key = this._list[this._index].Key;
					if (key.CompareTo(this._key) == 0)
					{
						this._current = this._list[this._index].Value;
						return true;
					}
				}
				return false;
			}

			// Token: 0x170000FC RID: 252
			// (get) Token: 0x06000784 RID: 1924 RVA: 0x00018E9F File Offset: 0x0001709F
			public TValue Current
			{
				get
				{
					return this._current;
				}
			}

			// Token: 0x170000FD RID: 253
			// (get) Token: 0x06000785 RID: 1925 RVA: 0x00018EA7 File Offset: 0x000170A7
			object IEnumerator.Current
			{
				get
				{
					return this._current;
				}
			}

			// Token: 0x06000786 RID: 1926 RVA: 0x00018EB4 File Offset: 0x000170B4
			public void Dispose()
			{
			}

			// Token: 0x06000787 RID: 1927 RVA: 0x00018EB6 File Offset: 0x000170B6
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040002D1 RID: 721
			private readonly List<KeyValuePair<TKey, TValue>> _list;

			// Token: 0x040002D2 RID: 722
			private readonly TKey _key;

			// Token: 0x040002D3 RID: 723
			private int _index;

			// Token: 0x040002D4 RID: 724
			private TValue _current;
		}
	}
}
