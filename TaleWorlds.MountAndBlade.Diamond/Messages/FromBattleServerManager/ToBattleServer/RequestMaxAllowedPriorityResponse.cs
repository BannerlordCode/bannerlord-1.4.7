using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromBattleServerManager.ToBattleServer
{
	// Token: 0x020000E5 RID: 229
	[MessageDescription("BattleServerManager", "BattleServer", true)]
	[Serializable]
	public class RequestMaxAllowedPriorityResponse : FunctionResult
	{
		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x00004E18 File Offset: 0x00003018
		// (set) Token: 0x06000431 RID: 1073 RVA: 0x00004E20 File Offset: 0x00003020
		[JsonProperty]
		public sbyte Priority { get; private set; }

		// Token: 0x06000432 RID: 1074 RVA: 0x00004E29 File Offset: 0x00003029
		public RequestMaxAllowedPriorityResponse()
		{
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00004E31 File Offset: 0x00003031
		public RequestMaxAllowedPriorityResponse(sbyte priority)
		{
			this.Priority = priority;
		}
	}
}
