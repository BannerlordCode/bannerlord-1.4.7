using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000034 RID: 52
	[Serializable]
	public class GetAvailableScenesResult : FunctionResult
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00002CAA File Offset: 0x00000EAA
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00002CB2 File Offset: 0x00000EB2
		[JsonProperty]
		public AvailableScenes AvailableScenes { get; private set; }

		// Token: 0x06000113 RID: 275 RVA: 0x00002CBB File Offset: 0x00000EBB
		public GetAvailableScenesResult()
		{
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00002CC3 File Offset: 0x00000EC3
		public GetAvailableScenesResult(AvailableScenes scenes)
		{
			this.AvailableScenes = scenes;
		}
	}
}
