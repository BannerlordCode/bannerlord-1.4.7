using System;

namespace TaleWorlds.MountAndBlade.Diamond.Ranked
{
	// Token: 0x0200015E RID: 350
	[Serializable]
	public class GameTypeRankInfo
	{
		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060009B5 RID: 2485 RVA: 0x0000EFD6 File Offset: 0x0000D1D6
		// (set) Token: 0x060009B6 RID: 2486 RVA: 0x0000EFDE File Offset: 0x0000D1DE
		public string GameType { get; private set; }

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060009B7 RID: 2487 RVA: 0x0000EFE7 File Offset: 0x0000D1E7
		// (set) Token: 0x060009B8 RID: 2488 RVA: 0x0000EFEF File Offset: 0x0000D1EF
		public RankBarInfo RankBarInfo { get; private set; }

		// Token: 0x060009B9 RID: 2489 RVA: 0x0000EFF8 File Offset: 0x0000D1F8
		public GameTypeRankInfo(string gameType, RankBarInfo rankBarInfo)
		{
			this.GameType = gameType;
			this.RankBarInfo = rankBarInfo;
		}
	}
}
