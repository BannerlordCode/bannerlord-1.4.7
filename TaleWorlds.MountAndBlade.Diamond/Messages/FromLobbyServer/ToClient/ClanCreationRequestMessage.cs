using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000021 RID: 33
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class ClanCreationRequestMessage : Message
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000BE RID: 190 RVA: 0x0000294B File Offset: 0x00000B4B
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00002953 File Offset: 0x00000B53
		[JsonProperty]
		public string CreatorPlayerName { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x0000295C File Offset: 0x00000B5C
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00002964 File Offset: 0x00000B64
		[JsonProperty]
		public PlayerId CreatorPlayerId { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x0000296D File Offset: 0x00000B6D
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00002975 File Offset: 0x00000B75
		[JsonProperty]
		public string ClanName { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x0000297E File Offset: 0x00000B7E
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00002986 File Offset: 0x00000B86
		[JsonProperty]
		public string ClanTag { get; private set; }

		// Token: 0x060000C6 RID: 198 RVA: 0x0000298F File Offset: 0x00000B8F
		public ClanCreationRequestMessage()
		{
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00002997 File Offset: 0x00000B97
		public ClanCreationRequestMessage(PlayerId creatorPlayerId, string creatorPlayerName, string clanName, string clanTag)
		{
			this.CreatorPlayerId = creatorPlayerId;
			this.CreatorPlayerName = creatorPlayerName;
			this.ClanName = clanName;
			this.ClanTag = clanTag;
		}
	}
}
