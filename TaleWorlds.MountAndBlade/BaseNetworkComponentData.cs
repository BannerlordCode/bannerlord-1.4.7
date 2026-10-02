using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002F3 RID: 755
	public class BaseNetworkComponentData : UdpNetworkComponent
	{
		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06002B64 RID: 11108 RVA: 0x000A70DE File Offset: 0x000A52DE
		// (set) Token: 0x06002B65 RID: 11109 RVA: 0x000A70E6 File Offset: 0x000A52E6
		public int CurrentBattleIndex { get; private set; }

		// Token: 0x06002B66 RID: 11110 RVA: 0x000A70EF File Offset: 0x000A52EF
		public void UpdateCurrentBattleIndex(int currentBattleIndex)
		{
			this.CurrentBattleIndex = currentBattleIndex;
		}

		// Token: 0x040010D7 RID: 4311
		public const float MaxIntermissionStateTime = 240f;
	}
}
