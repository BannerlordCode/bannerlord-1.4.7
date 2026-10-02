using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000051 RID: 81
	public class MbEvent<T1, T2, T3, T4> : IMbEvent<T1, T2, T3, T4>, IMbEventBase
	{
		// Token: 0x060008A0 RID: 2208 RVA: 0x00026A6C File Offset: 0x00024C6C
		public void AddNonSerializedListener(object owner, Action<T1, T2, T3, T4> action)
		{
			MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> eventHandlerRec = new MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4>(owner, action);
			MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00026A96 File Offset: 0x00024C96
		public void Invoke(T1 t1, T2 t2, T3 t3, T4 t4)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2, t3, t4);
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x00026AA9 File Offset: 0x00024CA9
		private void InvokeList(MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> list, T1 t1, T2 t2, T3 t3, T4 t4)
		{
			while (list != null)
			{
				list.Action(t1, t2, t3, t4);
				list = list.Next;
			}
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00026AC9 File Offset: 0x00024CC9
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00026AD8 File Offset: 0x00024CD8
		private void ClearListenerOfList(ref MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> list, object o)
		{
			MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> eventHandlerRec2 = list;
			if (eventHandlerRec2 == eventHandlerRec)
			{
				list = eventHandlerRec2.Next;
				return;
			}
			while (eventHandlerRec2 != null)
			{
				if (eventHandlerRec2.Next == eventHandlerRec)
				{
					eventHandlerRec2.Next = eventHandlerRec.Next;
				}
				else
				{
					eventHandlerRec2 = eventHandlerRec2.Next;
				}
			}
		}

		// Token: 0x040002BE RID: 702
		private MbEvent<T1, T2, T3, T4>.EventHandlerRec<T1, T2, T3, T4> _nonSerializedListenerList;

		// Token: 0x02000510 RID: 1296
		internal class EventHandlerRec<TA, TB, TC, TD>
		{
			// Token: 0x17000EDE RID: 3806
			// (get) Token: 0x06004C24 RID: 19492 RVA: 0x0017DD82 File Offset: 0x0017BF82
			// (set) Token: 0x06004C25 RID: 19493 RVA: 0x0017DD8A File Offset: 0x0017BF8A
			internal Action<TA, TB, TC, TD> Action { get; private set; }

			// Token: 0x17000EDF RID: 3807
			// (get) Token: 0x06004C26 RID: 19494 RVA: 0x0017DD93 File Offset: 0x0017BF93
			// (set) Token: 0x06004C27 RID: 19495 RVA: 0x0017DD9B File Offset: 0x0017BF9B
			internal object Owner { get; private set; }

			// Token: 0x06004C28 RID: 19496 RVA: 0x0017DDA4 File Offset: 0x0017BFA4
			public EventHandlerRec(object owner, Action<TA, TB, TC, TD> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040015CF RID: 5583
			public MbEvent<T1, T2, T3, T4>.EventHandlerRec<TA, TB, TC, TD> Next;
		}
	}
}
