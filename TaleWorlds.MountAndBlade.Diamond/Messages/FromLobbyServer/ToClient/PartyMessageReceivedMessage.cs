using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000054 RID: 84
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class PartyMessageReceivedMessage : Message
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00003370 File Offset: 0x00001570
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00003378 File Offset: 0x00001578
		[JsonProperty]
		public string PlayerName { get; private set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00003381 File Offset: 0x00001581
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x00003389 File Offset: 0x00001589
		[JsonProperty]
		public string Message { get; private set; }

		// Token: 0x060001B9 RID: 441 RVA: 0x00003392 File Offset: 0x00001592
		public PartyMessageReceivedMessage()
		{
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000339A File Offset: 0x0000159A
		public PartyMessageReceivedMessage(string playerName, string message)
		{
			this.PlayerName = playerName;
			this.Message = message;
		}
	}
}
