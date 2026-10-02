using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000BA RID: 186
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class RejoinBattleRequestMessage : Message
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000354 RID: 852 RVA: 0x00004484 File Offset: 0x00002684
		// (set) Token: 0x06000355 RID: 853 RVA: 0x0000448C File Offset: 0x0000268C
		[JsonProperty]
		public bool IsRejoinAccepted { get; private set; }

		// Token: 0x06000356 RID: 854 RVA: 0x00004495 File Offset: 0x00002695
		public RejoinBattleRequestMessage()
		{
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000449D File Offset: 0x0000269D
		public RejoinBattleRequestMessage(bool isRejoinAccepted)
		{
			this.IsRejoinAccepted = isRejoinAccepted;
		}
	}
}
