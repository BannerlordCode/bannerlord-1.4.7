using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200001E RID: 30
	public class CheckClanTagValidResult : FunctionResult
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x000028DB File Offset: 0x00000ADB
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x000028E3 File Offset: 0x00000AE3
		[JsonProperty]
		public bool TagExists { get; private set; }

		// Token: 0x060000B5 RID: 181 RVA: 0x000028EC File Offset: 0x00000AEC
		public CheckClanTagValidResult()
		{
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000028F4 File Offset: 0x00000AF4
		public CheckClanTagValidResult(bool tagExists)
		{
			this.TagExists = tagExists;
		}
	}
}
