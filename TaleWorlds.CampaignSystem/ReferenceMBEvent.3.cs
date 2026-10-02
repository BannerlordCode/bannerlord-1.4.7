using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200004B RID: 75
	public class ReferenceMBEvent<T1, T2, T3> : ReferenceIMBEvent<T1, T2, T3>, IMbEventBase
	{
		// Token: 0x0600088B RID: 2187 RVA: 0x00026808 File Offset: 0x00024A08
		public void AddNonSerializedListener(object owner, ReferenceAction<T1, T2, T3> action)
		{
			ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> eventHandlerRec = new ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3>(owner, action);
			ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> nonSerializedListenerList = this._nonSerializedListenerList;
			this._nonSerializedListenerList = eventHandlerRec;
			eventHandlerRec.Next = nonSerializedListenerList;
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00026832 File Offset: 0x00024A32
		public void Invoke(T1 t1, T2 t2, ref T3 t3)
		{
			this.InvokeList(this._nonSerializedListenerList, t1, t2, ref t3);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00026843 File Offset: 0x00024A43
		private void InvokeList(ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> list, T1 t1, T2 t2, ref T3 t3)
		{
			while (list != null)
			{
				list.Action(t1, t2, ref t3);
				list = list.Next;
			}
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00026861 File Offset: 0x00024A61
		public void ClearListeners(object o)
		{
			this.ClearListenerOfList(ref this._nonSerializedListenerList, o);
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00026870 File Offset: 0x00024A70
		private void ClearListenerOfList(ref ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> list, object o)
		{
			ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> eventHandlerRec = list;
			while (eventHandlerRec != null && eventHandlerRec.Owner != o)
			{
				eventHandlerRec = eventHandlerRec.Next;
			}
			if (eventHandlerRec == null)
			{
				return;
			}
			ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> eventHandlerRec2 = list;
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

		// Token: 0x040002BB RID: 699
		private ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<T1, T2, T3> _nonSerializedListenerList;

		// Token: 0x0200050D RID: 1293
		internal class EventHandlerRec<TS, TQ, TR>
		{
			// Token: 0x17000ED8 RID: 3800
			// (get) Token: 0x06004C15 RID: 19477 RVA: 0x0017DCDA File Offset: 0x0017BEDA
			// (set) Token: 0x06004C16 RID: 19478 RVA: 0x0017DCE2 File Offset: 0x0017BEE2
			internal ReferenceAction<TS, TQ, TR> Action { get; private set; }

			// Token: 0x17000ED9 RID: 3801
			// (get) Token: 0x06004C17 RID: 19479 RVA: 0x0017DCEB File Offset: 0x0017BEEB
			// (set) Token: 0x06004C18 RID: 19480 RVA: 0x0017DCF3 File Offset: 0x0017BEF3
			internal object Owner { get; private set; }

			// Token: 0x06004C19 RID: 19481 RVA: 0x0017DCFC File Offset: 0x0017BEFC
			public EventHandlerRec(object owner, ReferenceAction<TS, TQ, TR> action)
			{
				this.Action = action;
				this.Owner = owner;
			}

			// Token: 0x040015C6 RID: 5574
			public ReferenceMBEvent<T1, T2, T3>.EventHandlerRec<TS, TQ, TR> Next;
		}
	}
}
