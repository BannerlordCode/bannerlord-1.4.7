using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.MountAndBlade.Objects
{
	// Token: 0x020003A1 RID: 929
	public class GenericMissionEvent : EventBase
	{
		// Token: 0x060034ED RID: 13549 RVA: 0x000D9C86 File Offset: 0x000D7E86
		public GenericMissionEvent(string eventId, string parameter)
		{
			this.EventId = eventId;
			this.Parameter = parameter;
		}

		// Token: 0x04001676 RID: 5750
		public readonly string EventId;

		// Token: 0x04001677 RID: 5751
		public readonly string Parameter;
	}
}
