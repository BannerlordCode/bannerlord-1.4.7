using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C1 RID: 193
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class RequestJoinPlayerPartyMessage : Message
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000379 RID: 889 RVA: 0x0000460E File Offset: 0x0000280E
		// (set) Token: 0x0600037A RID: 890 RVA: 0x00004616 File Offset: 0x00002816
		[JsonProperty]
		public PlayerId TargetPlayer { get; private set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600037B RID: 891 RVA: 0x0000461F File Offset: 0x0000281F
		// (set) Token: 0x0600037C RID: 892 RVA: 0x00004627 File Offset: 0x00002827
		[JsonProperty]
		public bool InviteRequest { get; private set; }

		// Token: 0x0600037D RID: 893 RVA: 0x00004630 File Offset: 0x00002830
		public RequestJoinPlayerPartyMessage()
		{
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00004638 File Offset: 0x00002838
		public RequestJoinPlayerPartyMessage(PlayerId targetPlayer, bool inviteRequest)
		{
			this.TargetPlayer = targetPlayer;
			this.InviteRequest = inviteRequest;
		}
	}
}
