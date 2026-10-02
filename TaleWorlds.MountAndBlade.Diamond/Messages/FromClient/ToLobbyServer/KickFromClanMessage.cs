using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000B0 RID: 176
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class KickFromClanMessage : Message
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000320 RID: 800 RVA: 0x00004249 File Offset: 0x00002449
		// (set) Token: 0x06000321 RID: 801 RVA: 0x00004251 File Offset: 0x00002451
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x06000322 RID: 802 RVA: 0x0000425A File Offset: 0x0000245A
		public KickFromClanMessage()
		{
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00004262 File Offset: 0x00002462
		public KickFromClanMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
