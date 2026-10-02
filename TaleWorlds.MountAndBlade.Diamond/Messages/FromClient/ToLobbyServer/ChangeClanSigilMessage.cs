using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x0200007E RID: 126
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ChangeClanSigilMessage : Message
	{
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00003AE1 File Offset: 0x00001CE1
		// (set) Token: 0x0600026A RID: 618 RVA: 0x00003AE9 File Offset: 0x00001CE9
		[JsonProperty]
		public string NewSigil { get; private set; }

		// Token: 0x0600026B RID: 619 RVA: 0x00003AF2 File Offset: 0x00001CF2
		public ChangeClanSigilMessage()
		{
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00003AFA File Offset: 0x00001CFA
		public ChangeClanSigilMessage(string newSigil)
		{
			this.NewSigil = newSigil;
		}
	}
}
