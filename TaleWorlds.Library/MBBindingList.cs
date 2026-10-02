using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TaleWorlds.Library
{
	// Token: 0x02000068 RID: 104
	public class MBBindingList<T> : Collection<T>, IMBBindingList, IList, ICollection, IEnumerable
	{
		// Token: 0x06000348 RID: 840 RVA: 0x0000C038 File Offset: 0x0000A238
		public MBBindingList()
			: base(new List<T>(64))
		{
			this._list = (List<T>)base.Items;
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000349 RID: 841 RVA: 0x0000C058 File Offset: 0x0000A258
		// (remove) Token: 0x0600034A RID: 842 RVA: 0x0000C079 File Offset: 0x0000A279
		public event ListChangedEventHandler ListChanged
		{
			add
			{
				if (this._eventHandlers == null)
				{
					this._eventHandlers = new List<ListChangedEventHandler>();
				}
				this._eventHandlers.Add(value);
			}
			remove
			{
				if (this._eventHandlers != null)
				{
					this._eventHandlers.Remove(value);
				}
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000C090 File Offset: 0x0000A290
		protected override void ClearItems()
		{
			base.ClearItems();
			this.FireListChanged(ListChangedType.Reset, -1);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000C0A0 File Offset: 0x0000A2A0
		protected override void InsertItem(int index, T item)
		{
			base.InsertItem(index, item);
			this.FireListChanged(ListChangedType.ItemAdded, index);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000C0B2 File Offset: 0x0000A2B2
		protected override void RemoveItem(int index)
		{
			this.FireListChanged(ListChangedType.ItemBeforeDeleted, index);
			base.RemoveItem(index);
			this.FireListChanged(ListChangedType.ItemDeleted, index);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000C0CB File Offset: 0x0000A2CB
		protected override void SetItem(int index, T item)
		{
			base.SetItem(index, item);
			this.FireListChanged(ListChangedType.ItemChanged, index);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000C0DD File Offset: 0x0000A2DD
		private void FireListChanged(ListChangedType type, int index)
		{
			this.OnListChanged(new ListChangedEventArgs(type, index));
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000C0EC File Offset: 0x0000A2EC
		protected virtual void OnListChanged(ListChangedEventArgs e)
		{
			if (this._eventHandlers != null)
			{
				foreach (ListChangedEventHandler listChangedEventHandler in this._eventHandlers)
				{
					listChangedEventHandler(this, e);
				}
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000C148 File Offset: 0x0000A348
		public void Sort()
		{
			this._list.Sort();
			this.FireListChanged(ListChangedType.Sorted, -1);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000C15D File Offset: 0x0000A35D
		public void Sort(IComparer<T> comparer)
		{
			if (!this.IsOrdered(comparer))
			{
				this._list.Sort(comparer);
				this.FireListChanged(ListChangedType.Sorted, -1);
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000C17C File Offset: 0x0000A37C
		public bool IsOrdered(IComparer<T> comparer)
		{
			for (int i = 1; i < this._list.Count; i++)
			{
				if (comparer.Compare(this._list[i - 1], this._list[i]) == 1)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000C1C8 File Offset: 0x0000A3C8
		public void ApplyActionOnAllItems(Action<T> action)
		{
			for (int i = 0; i < this._list.Count; i++)
			{
				T t = this._list[i];
				action(t);
			}
		}

		// Token: 0x04000130 RID: 304
		private readonly List<T> _list;

		// Token: 0x04000131 RID: 305
		private List<ListChangedEventHandler> _eventHandlers;
	}
}
