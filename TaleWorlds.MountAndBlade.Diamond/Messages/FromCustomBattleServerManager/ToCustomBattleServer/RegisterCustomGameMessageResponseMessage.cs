using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromCustomBattleServerManager.ToCustomBattleServer
{
	// Token: 0x02000013 RID: 19
	[MessageDescription("CustomBattleServerManager", "CustomBattleServer", true)]
	[Serializable]
	public class RegisterCustomGameMessageResponseMessage : FunctionResult
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000080 RID: 128 RVA: 0x000026C8 File Offset: 0x000008C8
		// (set) Token: 0x06000081 RID: 129 RVA: 0x000026D0 File Offset: 0x000008D0
		[JsonProperty]
		public bool ShouldReportActivities { get; private set; }

		// Token: 0x06000082 RID: 130 RVA: 0x000026D9 File Offset: 0x000008D9
		public RegisterCustomGameMessageResponseMessage()
		{
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000026E1 File Offset: 0x000008E1
		public RegisterCustomGameMessageResponseMessage(bool shouldReportActivities)
		{
			this.ShouldReportActivities = shouldReportActivities;
		}
	}
}
