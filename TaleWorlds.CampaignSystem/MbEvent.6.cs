using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000053 RID: 83
	public class MbEvent<T1, T2, T3, T4, T5> : IMbEvent<T1, T2, T3, T4, T5>, IMbEventBase
	{
		// Token: 0x060008A7 RID: 2215 RVA: 0x00026B3C File Offset: 0x00024D3C
		public void AddNonSerializedListener(object owner, Action<T1, T2, T3, T4, T5> action)
		{
			MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> eventHandlerRec = new MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5>(owner, action);
			MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00026B66 File Offset: 0x00024D66
		public void Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2, t3, t4, t5);
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00026B7B File Offset: 0x00024D7B
		private void InvokeList(MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> list, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
		{
			while (list != null)
			{
				list.Action(t1, t2, t3, t4, t5);
				list = list.Next;
			}
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x00026B9D File Offset: 0x00024D9D
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x00026BAC File Offset: 0x00024DAC
		private void ClearListenerOfList(ref MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> list, object o)
		{
			MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> eventHandlerRec2 = list;
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

		// Token: 0x040002BF RID: 703
		private MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<T1, T2, T3, T4, T5> _nonSerializedListenerList;

		// Token: 0x02000511 RID: 1297
		internal class EventHandlerRec<TA, TB, TC, TD, TE>
		{
			// Token: 0x17000EE0 RID: 3808
			// (get) Token: 0x06004C29 RID: 19497 RVA: 0x0017DDBA File Offset: 0x0017BFBA
			// (set) Token: 0x06004C2A RID: 19498 RVA: 0x0017DDC2 File Offset: 0x0017BFC2
			internal Action<TA, TB, TC, TD, TE> Action { get; private set; }

			// Token: 0x17000EE1 RID: 3809
			// (get) Token: 0x06004C2B RID: 19499 RVA: 0x0017DDCB File Offset: 0x0017BFCB
			// (set) Token: 0x06004C2C RID: 19500 RVA: 0x0017DDD3 File Offset: 0x0017BFD3
			internal object Owner { get; private set; }

			// Token: 0x06004C2D RID: 19501 RVA: 0x0017DDDC File Offset: 0x0017BFDC
			public EventHandlerRec(object owner, Action<TA, TB, TC, TD, TE> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040015D2 RID: 5586
			public MbEvent<T1, T2, T3, T4, T5>.EventHandlerRec<TA, TB, TC, TD, TE> Next;
		}
	}
}
