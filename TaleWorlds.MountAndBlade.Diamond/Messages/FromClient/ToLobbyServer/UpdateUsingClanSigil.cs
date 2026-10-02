using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000CA RID: 202
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class UpdateUsingClanSigil : Message
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x000047EE File Offset: 0x000029EE
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x000047F6 File Offset: 0x000029F6
		[JsonProperty]
		public bool IsUsed { get; private set; }

		// Token: 0x060003A9 RID: 937 RVA: 0x000047FF File Offset: 0x000029FF
		public UpdateUsingClanSigil()
		{
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00004807 File Offset: 0x00002A07
		public UpdateUsingClanSigil(bool isUsed)
		{
			this.IsUsed = isUsed;
		}
	}
}
