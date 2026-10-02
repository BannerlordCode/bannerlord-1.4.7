using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000081 RID: 129
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ChangeRegionMessage : Message
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000275 RID: 629 RVA: 0x00003B59 File Offset: 0x00001D59
		// (set) Token: 0x06000276 RID: 630 RVA: 0x00003B61 File Offset: 0x00001D61
		[JsonProperty]
		public string Region { get; private set; }

		// Token: 0x06000277 RID: 631 RVA: 0x00003B6A File Offset: 0x00001D6A
		public ChangeRegionMessage()
		{
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00003B72 File Offset: 0x00001D72
		public ChangeRegionMessage(string region)
		{
			this.Region = region;
		}
	}
}
