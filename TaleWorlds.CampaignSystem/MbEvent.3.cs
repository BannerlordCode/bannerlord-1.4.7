using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200004D RID: 77
	public class MbEvent<T1, T2> : IMbEvent<T1, T2>, IMbEventBase
	{
		// Token: 0x06000892 RID: 2194 RVA: 0x000268D4 File Offset: 0x00024AD4
		public void AddNonSerializedListener(object owner, Action<T1, T2> action)
		{
			MbEvent<T1, T2>.EventHandlerRec<T1, T2> eventHandlerRec = new MbEvent<T1, T2>.EventHandlerRec<T1, T2>(owner, action);
			MbEvent<T1, T2>.EventHandlerRec<T1, T2> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x000268FE File Offset: 0x00024AFE
		public void Invoke(T1 t1, T2 t2)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2);
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0002690E File Offset: 0x00024B0E
		private void InvokeList(MbEvent<T1, T2>.EventHandlerRec<T1, T2> list, T1 t1, T2 t2)
		{
			while (list != null)
			{
				list.Action(t1, t2);
				list = list.Next;
			}
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0002692A File Offset: 0x00024B2A
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0002693C File Offset: 0x00024B3C
		private void ClearListenerOfList(ref MbEvent<T1, T2>.EventHandlerRec<T1, T2> list, object o)
		{
			MbEvent<T1, T2>.EventHandlerRec<T1, T2> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T1, T2>.EventHandlerRec<T1, T2> eventHandlerRec2 = list;
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

		// Token: 0x040002BC RID: 700
		private MbEvent<T1, T2>.EventHandlerRec<T1, T2> _nonSerializedListenerList;

		// Token: 0x0200050E RID: 1294
		internal class EventHandlerRec<TS, TQ>
		{
			// Token: 0x17000EDA RID: 3802
			// (get) Token: 0x06004C1A RID: 19482 RVA: 0x0017DD12 File Offset: 0x0017BF12
			// (set) Token: 0x06004C1B RID: 19483 RVA: 0x0017DD1A File Offset: 0x0017BF1A
			internal Action<TS, TQ> Action { get; private set; }

			// Token: 0x17000EDB RID: 3803
			// (get) Token: 0x06004C1C RID: 19484 RVA: 0x0017DD23 File Offset: 0x0017BF23
			// (set) Token: 0x06004C1D RID: 19485 RVA: 0x0017DD2B File Offset: 0x0017BF2B
			internal object Owner { get; private set; }

			// Token: 0x06004C1E RID: 19486 RVA: 0x0017DD34 File Offset: 0x0017BF34
			public EventHandlerRec(object owner, Action<TS, TQ> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040015C9 RID: 5577
			public MbEvent<T1, T2>.EventHandlerRec<TS, TQ> Next;
		}
	}
}
