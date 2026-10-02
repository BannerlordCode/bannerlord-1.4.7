using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200006C RID: 108
	[Serializable]
	public class UpdateShownBadgeIdMessageResult : FunctionResult
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000225 RID: 549 RVA: 0x00003829 File Offset: 0x00001A29
		// (set) Token: 0x06000226 RID: 550 RVA: 0x00003831 File Offset: 0x00001A31
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x06000227 RID: 551 RVA: 0x0000383A File Offset: 0x00001A3A
		public UpdateShownBadgeIdMessageResult()
		{
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00003842 File Offset: 0x00001A42
		public UpdateShownBadgeIdMessageResult(bool successful)
		{
			this.Successful = successful;
		}
	}
}
