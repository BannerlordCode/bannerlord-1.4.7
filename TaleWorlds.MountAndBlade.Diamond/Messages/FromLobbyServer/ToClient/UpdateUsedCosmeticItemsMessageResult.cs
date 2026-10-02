using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200006D RID: 109
	[Serializable]
	public class UpdateUsedCosmeticItemsMessageResult : FunctionResult
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000229 RID: 553 RVA: 0x00003851 File Offset: 0x00001A51
		// (set) Token: 0x0600022A RID: 554 RVA: 0x00003859 File Offset: 0x00001A59
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x0600022B RID: 555 RVA: 0x00003862 File Offset: 0x00001A62
		public UpdateUsedCosmeticItemsMessageResult()
		{
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000386A File Offset: 0x00001A6A
		public UpdateUsedCosmeticItemsMessageResult(bool successful)
		{
			this.Successful = successful;
		}
	}
}
