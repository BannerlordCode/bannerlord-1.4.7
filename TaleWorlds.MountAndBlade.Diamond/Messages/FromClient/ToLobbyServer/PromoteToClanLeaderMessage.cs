using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000B5 RID: 181
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class PromoteToClanLeaderMessage : Message
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000334 RID: 820 RVA: 0x00004311 File Offset: 0x00002511
		// (set) Token: 0x06000335 RID: 821 RVA: 0x00004319 File Offset: 0x00002519
		[JsonProperty]
		public PlayerId PromotedPlayerId { get; private set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000336 RID: 822 RVA: 0x00004322 File Offset: 0x00002522
		// (set) Token: 0x06000337 RID: 823 RVA: 0x0000432A File Offset: 0x0000252A
		[JsonProperty]
		public bool DontUseNameForUnknownPlayer { get; private set; }

		// Token: 0x06000338 RID: 824 RVA: 0x00004333 File Offset: 0x00002533
		public PromoteToClanLeaderMessage()
		{
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000433B File Offset: 0x0000253B
		public PromoteToClanLeaderMessage(PlayerId promotedPlayerId, bool dontUseNameForUnknownPlayer)
		{
			this.PromotedPlayerId = promotedPlayerId;
			this.DontUseNameForUnknownPlayer = dontUseNameForUnknownPlayer;
		}
	}
}
