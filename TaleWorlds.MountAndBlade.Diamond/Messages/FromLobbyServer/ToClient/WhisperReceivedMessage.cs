using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200006E RID: 110
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class WhisperReceivedMessage : Message
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600022D RID: 557 RVA: 0x00003879 File Offset: 0x00001A79
		// (set) Token: 0x0600022E RID: 558 RVA: 0x00003881 File Offset: 0x00001A81
		[JsonProperty]
		public string FromPlayer { get; private set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000388A File Offset: 0x00001A8A
		// (set) Token: 0x06000230 RID: 560 RVA: 0x00003892 File Offset: 0x00001A92
		[JsonProperty]
		public string ToPlayer { get; private set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000389B File Offset: 0x00001A9B
		// (set) Token: 0x06000232 RID: 562 RVA: 0x000038A3 File Offset: 0x00001AA3
		[JsonProperty]
		public string Message { get; private set; }

		// Token: 0x06000233 RID: 563 RVA: 0x000038AC File Offset: 0x00001AAC
		public WhisperReceivedMessage()
		{
		}

		// Token: 0x06000234 RID: 564 RVA: 0x000038B4 File Offset: 0x00001AB4
		public WhisperReceivedMessage(string fromPlayer, string toPlayer, string message)
		{
			this.FromPlayer = fromPlayer;
			this.ToPlayer = toPlayer;
			this.Message = message;
		}
	}
}
