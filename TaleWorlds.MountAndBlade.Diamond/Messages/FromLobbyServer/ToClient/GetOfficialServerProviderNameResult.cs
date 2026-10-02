using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200003A RID: 58
	[Serializable]
	public class GetOfficialServerProviderNameResult : FunctionResult
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00002D9A File Offset: 0x00000F9A
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00002DA2 File Offset: 0x00000FA2
		[JsonProperty]
		public string Name { get; private set; }

		// Token: 0x0600012B RID: 299 RVA: 0x00002DAB File Offset: 0x00000FAB
		public GetOfficialServerProviderNameResult()
		{
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00002DB3 File Offset: 0x00000FB3
		public GetOfficialServerProviderNameResult(string name)
		{
			this.Name = name;
		}
	}
}
