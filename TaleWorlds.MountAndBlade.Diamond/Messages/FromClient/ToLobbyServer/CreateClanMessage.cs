using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000087 RID: 135
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class CreateClanMessage : Message
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600028A RID: 650 RVA: 0x00003C29 File Offset: 0x00001E29
		// (set) Token: 0x0600028B RID: 651 RVA: 0x00003C31 File Offset: 0x00001E31
		[JsonProperty]
		public string ClanName { get; private set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600028C RID: 652 RVA: 0x00003C3A File Offset: 0x00001E3A
		// (set) Token: 0x0600028D RID: 653 RVA: 0x00003C42 File Offset: 0x00001E42
		[JsonProperty]
		public string ClanTag { get; private set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600028E RID: 654 RVA: 0x00003C4B File Offset: 0x00001E4B
		// (set) Token: 0x0600028F RID: 655 RVA: 0x00003C53 File Offset: 0x00001E53
		[JsonProperty]
		public string ClanFaction { get; private set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000290 RID: 656 RVA: 0x00003C5C File Offset: 0x00001E5C
		// (set) Token: 0x06000291 RID: 657 RVA: 0x00003C64 File Offset: 0x00001E64
		[JsonProperty]
		public string ClanSigil { get; private set; }

		// Token: 0x06000292 RID: 658 RVA: 0x00003C6D File Offset: 0x00001E6D
		public CreateClanMessage()
		{
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00003C75 File Offset: 0x00001E75
		public CreateClanMessage(string clanName, string clanTag, string clanFaction, string clanSigil)
		{
			this.ClanName = clanName;
			this.ClanTag = clanTag;
			this.ClanFaction = clanFaction;
			this.ClanSigil = clanSigil;
		}
	}
}
