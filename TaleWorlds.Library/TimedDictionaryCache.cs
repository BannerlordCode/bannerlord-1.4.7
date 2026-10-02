using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TaleWorlds.Library
{
	// Token: 0x02000095 RID: 149
	public class TimedDictionaryCache<TKey, TValue>
	{
		// Token: 0x06000555 RID: 1365 RVA: 0x00012FC5 File Offset: 0x000111C5
		public TimedDictionaryCache(long validMilliseconds)
		{
			this._dictionary = new Dictionary<TKey, ValueTuple<long, TValue>>();
			this._stopwatch = new Stopwatch();
			this._stopwatch.Start();
			this._validMilliseconds = validMilliseconds;
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00012FF5 File Offset: 0x000111F5
		public TimedDictionaryCache(TimeSpan validTimeSpan)
			: this((long)validTimeSpan.TotalMilliseconds)
		{
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00013005 File Offset: 0x00011205
		private bool IsItemExpired(TKey key)
		{
			return this._stopwatch.ElapsedMilliseconds - this._dictionary[key].Item1 >= this._validMilliseconds;
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001302F File Offset: 0x0001122F
		private bool RemoveIfExpired(TKey key)
		{
			if (this.IsItemExpired(key))
			{
				this._dictionary.Remove(key);
				return true;
			}
			return false;
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001304C File Offset: 0x0001124C
		public void PruneExpiredItems()
		{
			List<TKey> list = new List<TKey>();
			foreach (KeyValuePair<TKey, ValueTuple<long, TValue>> keyValuePair in this._dictionary)
			{
				if (this.IsItemExpired(keyValuePair.Key))
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (TKey tkey in list)
			{
				this._dictionary.Remove(tkey);
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00013100 File Offset: 0x00011300
		public void Clear()
		{
			this._dictionary.Clear();
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001310D File Offset: 0x0001130D
		public bool ContainsKey(TKey key)
		{
			return this._dictionary.ContainsKey(key) && !this.RemoveIfExpired(key);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00013129 File Offset: 0x00011329
		public bool Remove(TKey key)
		{
			this.RemoveIfExpired(key);
			return this._dictionary.Remove(key);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001313F File Offset: 0x0001133F
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (this.ContainsKey(key))
			{
				value = this._dictionary[key].Item2;
				return true;
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x17000092 RID: 146
		public TValue this[TKey key]
		{
			get
			{
				this.RemoveIfExpired(key);
				return this._dictionary[key].Item2;
			}
			set
			{
				this._dictionary[key] = new ValueTuple<long, TValue>(this._stopwatch.ElapsedMilliseconds, value);
			}
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x000131A8 File Offset: 0x000113A8
		public MBReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary()
		{
			this.PruneExpiredItems();
			Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
			foreach (KeyValuePair<TKey, ValueTuple<long, TValue>> keyValuePair in this._dictionary)
			{
				dictionary[keyValuePair.Key] = keyValuePair.Value.Item2;
			}
			return dictionary.GetReadOnlyDictionary<TKey, TValue>();
		}

		// Token: 0x040001A8 RID: 424
		[TupleElementNames(new string[] { "Timestamp", "Value" })]
		private readonly Dictionary<TKey, ValueTuple<long, TValue>> _dictionary;

		// Token: 0x040001A9 RID: 425
		private readonly Stopwatch _stopwatch;

		// Token: 0x040001AA RID: 426
		private readonly long _validMilliseconds;
	}
}
