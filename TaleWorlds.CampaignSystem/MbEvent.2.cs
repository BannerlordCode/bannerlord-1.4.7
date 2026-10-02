using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000045 RID: 69
	public class MbEvent<T> : IMbEvent<T>, IMbEventBase
	{
		// Token: 0x06000876 RID: 2166 RVA: 0x000265AC File Offset: 0x000247AC
		public void AddNonSerializedListener(object owner, Action<T> action)
		{
			MbEvent<T>.EventHandlerRec<T> eventHandlerRec = new MbEvent<T>.EventHandlerRec<T>(owner, action);
			MbEvent<T>.EventHandlerRec<T> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x000265D6 File Offset: 0x000247D6
		public void Invoke(T t)
		{
			this.InvokeList(this._nonSerializedListenerList, t);
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x000265E5 File Offset: 0x000247E5
		private void InvokeList(MbEvent<T>.EventHandlerRec<T> list, T t)
		{
			while (list != null)
			{
				list.Action(t);
				list = list.Next;
			}
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00026600 File Offset: 0x00024800
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00026610 File Offset: 0x00024810
		private void ClearListenerOfList(ref MbEvent<T>.EventHandlerRec<T> list, object o)
		{
			MbEvent<T>.EventHandlerRec<T> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T>.EventHandlerRec<T> eventHandlerRec2 = list;
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

		// Token: 0x040002B8 RID: 696
		private MbEvent<T>.EventHandlerRec<T> _nonSerializedListenerList;

		// Token: 0x0200050A RID: 1290
		internal class EventHandlerRec<TS>
		{
			// Token: 0x17000ED2 RID: 3794
			// (get) Token: 0x06004C06 RID: 19462 RVA: 0x0017DC32 File Offset: 0x0017BE32
			// (set) Token: 0x06004C07 RID: 19463 RVA: 0x0017DC3A File Offset: 0x0017BE3A
			internal Action<TS> Action { get; private set; }

			// Token: 0x17000ED3 RID: 3795
			// (get) Token: 0x06004C08 RID: 19464 RVA: 0x0017DC43 File Offset: 0x0017BE43
			// (set) Token: 0x06004C09 RID: 19465 RVA: 0x0017DC4B File Offset: 0x0017BE4B
			internal object Owner { get; private set; }

			// Token: 0x06004C0A RID: 19466 RVA: 0x0017DC54 File Offset: 0x0017BE54
			public EventHandlerRec(object owner, Action<TS> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040015BD RID: 5565
			public MbEvent<T>.EventHandlerRec<TS> Next;
		}
	}
}
