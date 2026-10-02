using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200005C RID: 92
	[Serializable]
	public class PlayerRemovedFromMatchmakerGame : Message
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00003498 File Offset: 0x00001698
		// (set) Token: 0x060001D3 RID: 467 RVA: 0x000034A0 File Offset: 0x000016A0
		[JsonProperty]
		public DisconnectType DisconnectType { get; private set; }

		// Token: 0x060001D4 RID: 468 RVA: 0x000034A9 File Offset: 0x000016A9
		public PlayerRemovedFromMatchmakerGame()
		{
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000034B1 File Offset: 0x000016B1
		public PlayerRemovedFromMatchmakerGame(DisconnectType disconnectType)
		{
			this.DisconnectType = disconnectType;
		}
	}
}
