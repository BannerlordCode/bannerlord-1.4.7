using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C8 RID: 200
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class UpdateShownBadgeIdMessage : Message
	{
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600039F RID: 927 RVA: 0x0000479E File Offset: 0x0000299E
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x000047A6 File Offset: 0x000029A6
		[JsonProperty]
		public string ShownBadgeId { get; private set; }

		// Token: 0x060003A1 RID: 929 RVA: 0x000047AF File Offset: 0x000029AF
		public UpdateShownBadgeIdMessage()
		{
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x000047B7 File Offset: 0x000029B7
		public UpdateShownBadgeIdMessage(string shownBadgeId)
		{
			this.ShownBadgeId = shownBadgeId;
		}
	}
}
