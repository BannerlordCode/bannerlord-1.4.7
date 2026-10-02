using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200002A RID: 42
	[Serializable]
	public class CreatePremadeGameMessageResult : FunctionResult
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00002AD4 File Offset: 0x00000CD4
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00002ADC File Offset: 0x00000CDC
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x060000E6 RID: 230 RVA: 0x00002AE5 File Offset: 0x00000CE5
		public CreatePremadeGameMessageResult()
		{
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00002AED File Offset: 0x00000CED
		public CreatePremadeGameMessageResult(bool successful)
		{
			this.Successful = successful;
		}
	}
}
