using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000098 RID: 152
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetBannerlordIDMessage : Message
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002CF RID: 719 RVA: 0x00003F07 File Offset: 0x00002107
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x00003F0F File Offset: 0x0000210F
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x060002D1 RID: 721 RVA: 0x00003F18 File Offset: 0x00002118
		public GetBannerlordIDMessage()
		{
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00003F20 File Offset: 0x00002120
		public GetBannerlordIDMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
