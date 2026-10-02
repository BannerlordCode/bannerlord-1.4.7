using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000CB RID: 203
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class WhisperMessage : Message
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060003AB RID: 939 RVA: 0x00004816 File Offset: 0x00002A16
		// (set) Token: 0x060003AC RID: 940 RVA: 0x0000481E File Offset: 0x00002A1E
		[JsonProperty]
		public string TargetPlayerName { get; private set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00004827 File Offset: 0x00002A27
		// (set) Token: 0x060003AE RID: 942 RVA: 0x0000482F File Offset: 0x00002A2F
		[JsonProperty]
		public string Message { get; private set; }

		// Token: 0x060003AF RID: 943 RVA: 0x00004838 File Offset: 0x00002A38
		public WhisperMessage()
		{
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00004840 File Offset: 0x00002A40
		public WhisperMessage(string targetPlayerName, string message)
		{
			this.TargetPlayerName = targetPlayerName;
			this.Message = message;
		}
	}
}
