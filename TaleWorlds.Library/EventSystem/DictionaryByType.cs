using System;
using System.Collections.Generic;

namespace TaleWorlds.Library.EventSystem
{
	// Token: 0x020000B9 RID: 185
	public class DictionaryByType
	{
		// Token: 0x060006E9 RID: 1769 RVA: 0x00017578 File Offset: 0x00015778
		public void Add<T>(Action<T> value)
		{
			object obj;
			if (!this._eventsByType.TryGetValue(typeof(T), out obj))
			{
				obj = new List<Action<T>>();
				this._eventsByType[typeof(T)] = obj;
			}
			((List<Action<T>>)obj).Add(value);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x000175C8 File Offset: 0x000157C8
		public void Remove<T>(Action<T> value)
		{
			object obj;
			if (this._eventsByType.TryGetValue(typeof(T), out obj))
			{
				List<Action<T>> list = (List<Action<T>>)obj;
				list.Remove(value);
				this._eventsByType[typeof(T)] = list;
				return;
			}
			Debug.FailedAssert("Event: " + typeof(T).Name + " were not registered in the first place", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\EventSystem\\EventManager.cs", "Remove", 106);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00017644 File Offset: 0x00015844
		public void InvokeActions<T>(T item)
		{
			object obj;
			if (this._eventsByType.TryGetValue(typeof(T), out obj))
			{
				foreach (Action<T> action in ((List<Action<T>>)obj))
				{
					action(item);
				}
			}
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x000176B0 File Offset: 0x000158B0
		public List<Action<T>> Get<T>()
		{
			return (List<Action<T>>)this._eventsByType[typeof(T)];
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x000176CC File Offset: 0x000158CC
		public bool TryGet<T>(out List<Action<T>> value)
		{
			object obj;
			if (this._eventsByType.TryGetValue(typeof(T), out obj))
			{
				value = (List<Action<T>>)obj;
				return true;
			}
			value = null;
			return false;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00017700 File Offset: 0x00015900
		public IDictionary<Type, object> GetClone()
		{
			return new Dictionary<Type, object>(this._eventsByType);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0001770D File Offset: 0x0001590D
		public void Clear()
		{
			this._eventsByType.Clear();
		}

		// Token: 0x0400021C RID: 540
		private readonly IDictionary<Type, object> _eventsByType = new Dictionary<Type, object>();
	}
}
