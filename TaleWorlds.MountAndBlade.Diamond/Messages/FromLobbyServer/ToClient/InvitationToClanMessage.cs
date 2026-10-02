using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000048 RID: 72
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class InvitationToClanMessage : Message
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000305C File Offset: 0x0000125C
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00003064 File Offset: 0x00001264
		[JsonProperty]
		public PlayerId InviterId { get; private set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600016F RID: 367 RVA: 0x0000306D File Offset: 0x0000126D
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00003075 File Offset: 0x00001275
		[JsonProperty]
		public string ClanName { get; private set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000171 RID: 369 RVA: 0x0000307E File Offset: 0x0000127E
		// (set) Token: 0x06000172 RID: 370 RVA: 0x00003086 File Offset: 0x00001286
		[JsonProperty]
		public string ClanTag { get; private set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000173 RID: 371 RVA: 0x0000308F File Offset: 0x0000128F
		// (set) Token: 0x06000174 RID: 372 RVA: 0x00003097 File Offset: 0x00001297
		[JsonProperty]
		public int ClanPlayerCount { get; private set; }

		// Token: 0x06000175 RID: 373 RVA: 0x000030A0 File Offset: 0x000012A0
		public InvitationToClanMessage()
		{
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000030A8 File Offset: 0x000012A8
		public InvitationToClanMessage(PlayerId inviterId, string clanName, string clanTag, int clanPlayerCount)
		{
			this.InviterId = inviterId;
			this.ClanName = clanName;
			this.ClanTag = clanTag;
			this.ClanPlayerCount = clanPlayerCount;
		}
	}
}
