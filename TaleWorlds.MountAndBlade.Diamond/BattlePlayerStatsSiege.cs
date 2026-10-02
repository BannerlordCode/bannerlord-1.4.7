using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000F7 RID: 247
	[Serializable]
	public class BattlePlayerStatsSiege : BattlePlayerStatsBase
	{
		// Token: 0x060004E4 RID: 1252 RVA: 0x000057E7 File Offset: 0x000039E7
		public BattlePlayerStatsSiege()
		{
			base.GameType = "Siege";
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x000057FA File Offset: 0x000039FA
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x00005802 File Offset: 0x00003A02
		public int WallsBreached { get; set; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x0000580B File Offset: 0x00003A0B
		// (set) Token: 0x060004E8 RID: 1256 RVA: 0x00005813 File Offset: 0x00003A13
		public int SiegeEngineKills { get; set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x0000581C File Offset: 0x00003A1C
		// (set) Token: 0x060004EA RID: 1258 RVA: 0x00005824 File Offset: 0x00003A24
		public int SiegeEnginesDestroyed { get; set; }

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x0000582D File Offset: 0x00003A2D
		// (set) Token: 0x060004EC RID: 1260 RVA: 0x00005835 File Offset: 0x00003A35
		public int ObjectiveGoldGained { get; set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0000583E File Offset: 0x00003A3E
		// (set) Token: 0x060004EE RID: 1262 RVA: 0x00005846 File Offset: 0x00003A46
		public int Score { get; set; }
	}
}
