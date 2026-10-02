using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C9 RID: 201
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class UpdateUsedCosmeticItemsMessage : Message
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x000047C6 File Offset: 0x000029C6
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x000047CE File Offset: 0x000029CE
		[JsonProperty]
		public List<CosmeticItemInfo> UsedCosmetics { get; private set; }

		// Token: 0x060003A5 RID: 933 RVA: 0x000047D7 File Offset: 0x000029D7
		public UpdateUsedCosmeticItemsMessage()
		{
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x000047DF File Offset: 0x000029DF
		public UpdateUsedCosmeticItemsMessage(List<CosmeticItemInfo> usedCosmetics)
		{
			this.UsedCosmetics = usedCosmetics;
		}
	}
}
