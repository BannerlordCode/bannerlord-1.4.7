using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000020 RID: 32
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ClanCreationRequestAnsweredMessage : Message
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x0000290B File Offset: 0x00000B0B
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x00002913 File Offset: 0x00000B13
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000BA RID: 186 RVA: 0x0000291C File Offset: 0x00000B1C
		// (set) Token: 0x060000BB RID: 187 RVA: 0x00002924 File Offset: 0x00000B24
		[JsonProperty]
		public ClanCreationAnswer ClanCreationAnswer { get; private set; }

		// Token: 0x060000BC RID: 188 RVA: 0x0000292D File Offset: 0x00000B2D
		public ClanCreationRequestAnsweredMessage()
		{
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00002935 File Offset: 0x00000B35
		public ClanCreationRequestAnsweredMessage(PlayerId playerId, ClanCreationAnswer clanCreationAnswer)
		{
			this.PlayerId = playerId;
			this.ClanCreationAnswer = clanCreationAnswer;
		}
	}
}
