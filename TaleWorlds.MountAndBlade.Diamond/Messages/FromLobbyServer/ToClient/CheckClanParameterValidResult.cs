using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200001D RID: 29
	[Serializable]
	public class CheckClanParameterValidResult : FunctionResult
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000AD RID: 173 RVA: 0x0000289B File Offset: 0x00000A9B
		// (set) Token: 0x060000AE RID: 174 RVA: 0x000028A3 File Offset: 0x00000AA3
		[JsonProperty]
		public bool IsValid { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000AF RID: 175 RVA: 0x000028AC File Offset: 0x00000AAC
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x000028B4 File Offset: 0x00000AB4
		[JsonProperty]
		public StringValidationError Error { get; private set; }

		// Token: 0x060000B1 RID: 177 RVA: 0x000028BD File Offset: 0x00000ABD
		public CheckClanParameterValidResult()
		{
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000028C5 File Offset: 0x00000AC5
		public CheckClanParameterValidResult(bool isValid, StringValidationError error)
		{
			this.IsValid = isValid;
			this.Error = error;
		}
	}
}
