using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200003A RID: 58
	public class MbEvent : IMbEvent
	{
		// Token: 0x060003D9 RID: 985 RVA: 0x0001E7E8 File Offset: 0x0001C9E8
		public void AddNonSerializedListener(object owner, Action action)
		{
			MbEvent.EventHandlerRec eventHandlerRec = new MbEvent.EventHandlerRec(owner, action);
			MbEvent.EventHandlerRec nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0001E812 File Offset: 0x0001CA12
		public void Invoke()
		{
			this.InvokeList(this._nonSerializedListenerList);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0001E820 File Offset: 0x0001CA20
		private void InvokeList(MbEvent.EventHandlerRec list)
		{
			while (list != null)
			{
				list.Action();
				list = list.Next;
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0001E83A File Offset: 0x0001CA3A
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0001E84C File Offset: 0x0001CA4C
		private void ClearListenerOfList(ref MbEvent.EventHandlerRec list, object o)
		{
			MbEvent.EventHandlerRec eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent.EventHandlerRec eventHandlerRec2 = list;
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

		// Token: 0x04000184 RID: 388
		private MbEvent.EventHandlerRec _nonSerializedListenerList;

		// Token: 0x02000506 RID: 1286
		internal class EventHandlerRec
		{
			// Token: 0x17000ECE RID: 3790
			// (get) Token: 0x06004BDD RID: 19421 RVA: 0x0017D8AA File Offset: 0x0017BAAA
			// (set) Token: 0x06004BDE RID: 19422 RVA: 0x0017D8B2 File Offset: 0x0017BAB2
			internal Action Action { get; private set; }

			// Token: 0x17000ECF RID: 3791
			// (get) Token: 0x06004BDF RID: 19423 RVA: 0x0017D8BB File Offset: 0x0017BABB
			// (set) Token: 0x06004BE0 RID: 19424 RVA: 0x0017D8C3 File Offset: 0x0017BAC3
			internal object Owner { get; private set; }

			// Token: 0x06004BE1 RID: 19425 RVA: 0x0017D8CC File Offset: 0x0017BACC
			public EventHandlerRec(object owner, Action action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x0400159F RID: 5535
			public MbEvent.EventHandlerRec Next;
		}
	}
}
