using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000080 RID: 128
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ChangePlayerSigilMessage : Message
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000271 RID: 625 RVA: 0x00003B31 File Offset: 0x00001D31
		// (set) Token: 0x06000272 RID: 626 RVA: 0x00003B39 File Offset: 0x00001D39
		[JsonProperty]
		public string SigilId { get; private set; }

		// Token: 0x06000273 RID: 627 RVA: 0x00003B42 File Offset: 0x00001D42
		public ChangePlayerSigilMessage()
		{
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00003B4A File Offset: 0x00001D4A
		public ChangePlayerSigilMessage(string sigilId)
		{
			this.SigilId = sigilId;
		}
	}
}
