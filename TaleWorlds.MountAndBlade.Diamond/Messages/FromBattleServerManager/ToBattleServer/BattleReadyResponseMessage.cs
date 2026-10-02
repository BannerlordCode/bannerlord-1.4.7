using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromBattleServerManager.ToBattleServer
{
	// Token: 0x020000DF RID: 223
	[MessageDescription("BattleServerManager", "BattleServer", true)]
	[Serializable]
	public class BattleReadyResponseMessage : FunctionResult
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00004CFF File Offset: 0x00002EFF
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x00004D07 File Offset: 0x00002F07
		[JsonProperty]
		public bool ShouldReportActivities { get; private set; }

		// Token: 0x06000417 RID: 1047 RVA: 0x00004D10 File Offset: 0x00002F10
		public BattleReadyResponseMessage()
		{
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00004D18 File Offset: 0x00002F18
		public BattleReadyResponseMessage(bool shouldReportActivities)
		{
			this.ShouldReportActivities = shouldReportActivities;
		}
	}
}
