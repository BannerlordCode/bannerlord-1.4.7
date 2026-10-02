using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200004F RID: 79
	public class MbEvent<T1, T2, T3> : IMbEvent<T1, T2, T3>, IMbEventBase
	{
		// Token: 0x06000899 RID: 2201 RVA: 0x000269A0 File Offset: 0x00024BA0
		public void AddNonSerializedListener(object owner, Action<T1, T2, T3> action)
		{
			MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> eventHandlerRec = new MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3>(owner, action);
			MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x000269CA File Offset: 0x00024BCA
		public void Invoke(T1 t1, T2 t2, T3 t3)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2, t3);
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x000269DB File Offset: 0x00024BDB
		private void InvokeList(MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> list, T1 t1, T2 t2, T3 t3)
		{
			while (list != null)
			{
				list.Action(t1, t2, t3);
				list = list.Next;
			}
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x000269F9 File Offset: 0x00024BF9
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x00026A08 File Offset: 0x00024C08
		private void ClearListenerOfList(ref MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> list, object o)
		{
			MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> eventHandlerRec2 = list;
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

		// Token: 0x040002BD RID: 701
		private MbEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> _nonSerializedListenerList;

		// Token: 0x0200050F RID: 1295
		internal class EventHandlerRec<TS, TQ, TR>
		{
			// Token: 0x17000EDC RID: 3804
			// (get) Token: 0x06004C1F RID: 19487 RVA: 0x0017DD4A File Offset: 0x0017BF4A
			// (set) Token: 0x06004C20 RID: 19488 RVA: 0x0017DD52 File Offset: 0x0017BF52
			internal Action<TS, TQ, TR> Action { get; private set; }

			// Token: 0x17000EDD RID: 3805
			// (get) Token: 0x06004C21 RID: 19489 RVA: 0x0017DD5B File Offset: 0x0017BF5B
			// (set) Token: 0x06004C22 RID: 19490 RVA: 0x0017DD63 File Offset: 0x0017BF63
			internal object Owner { get; private set; }

			// Token: 0x06004C23 RID: 19491 RVA: 0x0017DD6C File Offset: 0x0017BF6C
			public EventHandlerRec(object owner, Action<TS, TQ, TR> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040015CC RID: 5580
			public MbEvent<T1, T2, T3>.EventHandlerRec<TS, TQ, TR> Next;
		}
	}
}
