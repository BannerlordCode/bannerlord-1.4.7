using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000BD RID: 189
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	internal class JPropertyKeyedCollection : Collection<JToken>
	{
		// Token: 0x06000A7C RID: 2684 RVA: 0x00029B93 File Offset: 0x00027D93
		public JPropertyKeyedCollection()
			: base(new List<JToken>())
		{
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00029BA0 File Offset: 0x00027DA0
		private void AddKey(string key, JToken item)
		{
			this.EnsureDictionary();
			this._dictionary[key] = item;
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00029BB8 File Offset: 0x00027DB8
		protected void ChangeItemKey(JToken item, string newKey)
		{
			if (!this.ContainsItem(item))
			{
				throw new ArgumentException("The specified item does not exist in this KeyedCollection.");
			}
			string keyForItem = this.GetKeyForItem(item);
			if (!JPropertyKeyedCollection.Comparer.Equals(keyForItem, newKey))
			{
				if (newKey != null)
				{
					this.AddKey(newKey, item);
				}
				if (keyForItem != null)
				{
					this.RemoveKey(keyForItem);
				}
			}
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00029C04 File Offset: 0x00027E04
		protected override void ClearItems()
		{
			base.ClearItems();
			Dictionary<string, JToken> dictionary = this._dictionary;
			if (dictionary == null)
			{
				return;
			}
			dictionary.Clear();
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00029C1C File Offset: 0x00027E1C
		public bool Contains(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this._dictionary != null && this._dictionary.ContainsKey(key);
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00029C44 File Offset: 0x00027E44
		private bool ContainsItem(JToken item)
		{
			if (this._dictionary == null)
			{
				return false;
			}
			string keyForItem = this.GetKeyForItem(item);
			JToken jtoken;
			return this._dictionary.TryGetValue(keyForItem, out jtoken);
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00029C71 File Offset: 0x00027E71
		private void EnsureDictionary()
		{
			if (this._dictionary == null)
			{
				this._dictionary = new Dictionary<string, JToken>(JPropertyKeyedCollection.Comparer);
			}
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00029C8B File Offset: 0x00027E8B
		private string GetKeyForItem(JToken item)
		{
			return ((JProperty)item).Name;
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x00029C98 File Offset: 0x00027E98
		protected override void InsertItem(int index, JToken item)
		{
			this.AddKey(this.GetKeyForItem(item), item);
			base.InsertItem(index, item);
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x00029CB0 File Offset: 0x00027EB0
		public bool Remove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			JToken jtoken;
			return this._dictionary != null && this._dictionary.TryGetValue(key, out jtoken) && base.Remove(jtoken);
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00029CF0 File Offset: 0x00027EF0
		protected override void RemoveItem(int index)
		{
			string keyForItem = this.GetKeyForItem(base.Items[index]);
			this.RemoveKey(keyForItem);
			base.RemoveItem(index);
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00029D1E File Offset: 0x00027F1E
		private void RemoveKey(string key)
		{
			Dictionary<string, JToken> dictionary = this._dictionary;
			if (dictionary == null)
			{
				return;
			}
			dictionary.Remove(key);
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00029D34 File Offset: 0x00027F34
		protected override void SetItem(int index, JToken item)
		{
			string keyForItem = this.GetKeyForItem(item);
			string keyForItem2 = this.GetKeyForItem(base.Items[index]);
			if (JPropertyKeyedCollection.Comparer.Equals(keyForItem2, keyForItem))
			{
				if (this._dictionary != null)
				{
					this._dictionary[keyForItem] = item;
				}
			}
			else
			{
				this.AddKey(keyForItem, item);
				if (keyForItem2 != null)
				{
					this.RemoveKey(keyForItem2);
				}
			}
			base.SetItem(index, item);
		}

		// Token: 0x170001E8 RID: 488
		public JToken this[string key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				if (this._dictionary != null)
				{
					return this._dictionary[key];
				}
				throw new KeyNotFoundException();
			}
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00029DC5 File Offset: 0x00027FC5
		public bool TryGetValue(string key, [Nullable(2)] [NotNullWhen(true)] out JToken value)
		{
			if (this._dictionary == null)
			{
				value = null;
				return false;
			}
			return this._dictionary.TryGetValue(key, out value);
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000A8B RID: 2699 RVA: 0x00029DE1 File Offset: 0x00027FE1
		public ICollection<string> Keys
		{
			get
			{
				this.EnsureDictionary();
				return this._dictionary.Keys;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000A8C RID: 2700 RVA: 0x00029DF4 File Offset: 0x00027FF4
		public ICollection<JToken> Values
		{
			get
			{
				this.EnsureDictionary();
				return this._dictionary.Values;
			}
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x00029E07 File Offset: 0x00028007
		public int IndexOfReference(JToken t)
		{
			return ((List<JToken>)base.Items).IndexOfReference(t);
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x00029E1C File Offset: 0x0002801C
		public bool Compare(JPropertyKeyedCollection other)
		{
			if (this == other)
			{
				return true;
			}
			Dictionary<string, JToken> dictionary = this._dictionary;
			Dictionary<string, JToken> dictionary2 = other._dictionary;
			if (dictionary == null && dictionary2 == null)
			{
				return true;
			}
			if (dictionary == null)
			{
				return dictionary2.Count == 0;
			}
			if (dictionary2 == null)
			{
				return dictionary.Count == 0;
			}
			if (dictionary.Count != dictionary2.Count)
			{
				return false;
			}
			foreach (KeyValuePair<string, JToken> keyValuePair in dictionary)
			{
				JToken jtoken;
				if (!dictionary2.TryGetValue(keyValuePair.Key, out jtoken))
				{
					return false;
				}
				JProperty jproperty = (JProperty)keyValuePair.Value;
				JProperty jproperty2 = (JProperty)jtoken;
				if (jproperty.Value == null)
				{
					return jproperty2.Value == null;
				}
				if (!jproperty.Value.DeepEquals(jproperty2.Value))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000369 RID: 873
		private static readonly IEqualityComparer<string> Comparer = StringComparer.Ordinal;

		// Token: 0x0400036A RID: 874
		[Nullable(new byte[] { 2, 1, 1 })]
		private Dictionary<string, JToken> _dictionary;
	}
}
