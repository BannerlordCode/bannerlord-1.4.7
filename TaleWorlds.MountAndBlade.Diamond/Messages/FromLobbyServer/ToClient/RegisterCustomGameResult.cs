using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000062 RID: 98
	[Serializable]
	public class RegisterCustomGameResult : FunctionResult
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000363B File Offset: 0x0000183B
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x00003643 File Offset: 0x00001843
		[JsonProperty]
		public bool Success { get; private set; }

		// Token: 0x060001F8 RID: 504 RVA: 0x0000364C File Offset: 0x0000184C
		public RegisterCustomGameResult()
		{
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00003654 File Offset: 0x00001854
		public RegisterCustomGameResult(bool success)
		{
			this.Success = success;
		}
	}
}
