using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000069 RID: 105
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class SigilChangeAnswerMessage : Message
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00003794 File Offset: 0x00001994
		// (set) Token: 0x06000218 RID: 536 RVA: 0x0000379C File Offset: 0x0000199C
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x06000219 RID: 537 RVA: 0x000037A5 File Offset: 0x000019A5
		public SigilChangeAnswerMessage()
		{
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000037AD File Offset: 0x000019AD
		public SigilChangeAnswerMessage(bool answer)
		{
			this.Successful = answer;
		}
	}
}
