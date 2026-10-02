using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000026 RID: 38
	[Serializable]
	public class ClientQuitFromCustomGameMessage : Message
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00002A34 File Offset: 0x00000C34
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00002A3C File Offset: 0x00000C3C
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x060000D6 RID: 214 RVA: 0x00002A45 File Offset: 0x00000C45
		public ClientQuitFromCustomGameMessage()
		{
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00002A4D File Offset: 0x00000C4D
		public ClientQuitFromCustomGameMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
