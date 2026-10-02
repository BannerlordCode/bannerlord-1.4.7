using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000030 RID: 48
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class FindGameAnswerMessage : Message
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00002BEC File Offset: 0x00000DEC
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00002BF4 File Offset: 0x00000DF4
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00002BFD File Offset: 0x00000DFD
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00002C05 File Offset: 0x00000E05
		[JsonProperty]
		public string[] SelectedAndEnabledGameTypes { get; private set; }

		// Token: 0x06000103 RID: 259 RVA: 0x00002C0E File Offset: 0x00000E0E
		public FindGameAnswerMessage()
		{
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00002C16 File Offset: 0x00000E16
		public FindGameAnswerMessage(bool successful, string[] selectedAndEnabledGameTypes)
		{
			this.Successful = successful;
			this.SelectedAndEnabledGameTypes = selectedAndEnabledGameTypes;
		}
	}
}
