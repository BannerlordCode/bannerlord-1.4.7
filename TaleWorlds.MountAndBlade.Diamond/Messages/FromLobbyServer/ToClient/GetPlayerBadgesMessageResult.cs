using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200003C RID: 60
	[Serializable]
	public class GetPlayerBadgesMessageResult : FunctionResult
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00002DEA File Offset: 0x00000FEA
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00002DF2 File Offset: 0x00000FF2
		[JsonProperty]
		public string[] Badges { get; private set; }

		// Token: 0x06000133 RID: 307 RVA: 0x00002DFB File Offset: 0x00000FFB
		public GetPlayerBadgesMessageResult()
		{
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00002E03 File Offset: 0x00001003
		public GetPlayerBadgesMessageResult(string[] badges)
		{
			this.Badges = badges;
		}
	}
}
