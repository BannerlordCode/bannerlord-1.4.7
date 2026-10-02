using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x0200007A RID: 122
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class BuyCosmeticMessage : Message
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600025F RID: 607 RVA: 0x00003A81 File Offset: 0x00001C81
		// (set) Token: 0x06000260 RID: 608 RVA: 0x00003A89 File Offset: 0x00001C89
		[JsonProperty]
		public string CosmeticId { get; private set; }

		// Token: 0x06000261 RID: 609 RVA: 0x00003A92 File Offset: 0x00001C92
		public BuyCosmeticMessage()
		{
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00003A9A File Offset: 0x00001C9A
		public BuyCosmeticMessage(string cosmeticId)
		{
			this.CosmeticId = cosmeticId;
		}
	}
}
