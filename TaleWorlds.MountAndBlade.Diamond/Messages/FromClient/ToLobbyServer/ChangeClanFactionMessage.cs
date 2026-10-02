using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x0200007D RID: 125
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ChangeClanFactionMessage : Message
	{
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000265 RID: 613 RVA: 0x00003AB9 File Offset: 0x00001CB9
		// (set) Token: 0x06000266 RID: 614 RVA: 0x00003AC1 File Offset: 0x00001CC1
		[JsonProperty]
		public string NewFaction { get; private set; }

		// Token: 0x06000267 RID: 615 RVA: 0x00003ACA File Offset: 0x00001CCA
		public ChangeClanFactionMessage()
		{
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00003AD2 File Offset: 0x00001CD2
		public ChangeClanFactionMessage(string newFaction)
		{
			this.NewFaction = newFaction;
		}
	}
}
