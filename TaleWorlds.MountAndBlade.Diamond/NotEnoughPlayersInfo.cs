using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000106 RID: 262
	[Serializable]
	public class NotEnoughPlayersInfo
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x0000708C File Offset: 0x0000528C
		// (set) Token: 0x06000590 RID: 1424 RVA: 0x00007094 File Offset: 0x00005294
		[JsonProperty]
		public int CurrentPlayerCount { get; private set; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x0000709D File Offset: 0x0000529D
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x000070A5 File Offset: 0x000052A5
		[JsonProperty]
		public int RequiredPlayerCount { get; private set; }

		// Token: 0x06000593 RID: 1427 RVA: 0x000070AE File Offset: 0x000052AE
		public NotEnoughPlayersInfo(int currentPlayerCount, int requiredPlayerCount)
		{
			this.CurrentPlayerCount = currentPlayerCount;
			this.RequiredPlayerCount = requiredPlayerCount;
		}
	}
}
