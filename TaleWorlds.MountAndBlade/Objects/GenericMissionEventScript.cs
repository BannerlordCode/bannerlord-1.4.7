using System;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.Objects
{
	// Token: 0x020003A2 RID: 930
	public class GenericMissionEventScript : ScriptComponentBehavior
	{
		// Token: 0x060034EE RID: 13550 RVA: 0x000D9C9C File Offset: 0x000D7E9C
		public GenericMissionEventScript()
		{
			this.EventId = string.Empty;
			this.Parameter = string.Empty;
			this.IsDisabled = false;
		}

		// Token: 0x060034EF RID: 13551 RVA: 0x000D9CC1 File Offset: 0x000D7EC1
		public GenericMissionEventScript(string eventId, string parameter)
		{
			this.EventId = eventId;
			this.Parameter = parameter;
			this.IsDisabled = false;
		}

		// Token: 0x04001678 RID: 5752
		public string EventId;

		// Token: 0x04001679 RID: 5753
		public string Parameter;

		// Token: 0x0400167A RID: 5754
		[EditableScriptComponentVariable(false, "")]
		public bool IsDisabled;
	}
}
