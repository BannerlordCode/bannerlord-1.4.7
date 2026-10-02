using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000F0 RID: 240
	[Serializable]
	public class BattlePlayerEntry
	{
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x00005524 File Offset: 0x00003724
		// (set) Token: 0x060004AB RID: 1195 RVA: 0x0000552C File Offset: 0x0000372C
		public PlayerId PlayerId { get; set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x00005535 File Offset: 0x00003735
		// (set) Token: 0x060004AD RID: 1197 RVA: 0x0000553D File Offset: 0x0000373D
		public int TeamNo { get; set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00005546 File Offset: 0x00003746
		// (set) Token: 0x060004AF RID: 1199 RVA: 0x0000554E File Offset: 0x0000374E
		public Guid Party { get; set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00005557 File Offset: 0x00003757
		// (set) Token: 0x060004B1 RID: 1201 RVA: 0x0000555F File Offset: 0x0000375F
		public BattlePlayerStatsBase PlayerStats { get; set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00005568 File Offset: 0x00003768
		// (set) Token: 0x060004B3 RID: 1203 RVA: 0x00005570 File Offset: 0x00003770
		public int PlayTime { get; set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00005579 File Offset: 0x00003779
		// (set) Token: 0x060004B5 RID: 1205 RVA: 0x00005581 File Offset: 0x00003781
		public DateTime LastJoinTime { get; set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x0000558A File Offset: 0x0000378A
		// (set) Token: 0x060004B7 RID: 1207 RVA: 0x00005592 File Offset: 0x00003792
		public bool Disconnected { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x0000559B File Offset: 0x0000379B
		// (set) Token: 0x060004B9 RID: 1209 RVA: 0x000055A3 File Offset: 0x000037A3
		public string GameType { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x000055AC File Offset: 0x000037AC
		// (set) Token: 0x060004BB RID: 1211 RVA: 0x000055B4 File Offset: 0x000037B4
		public bool Won { get; set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x000055BD File Offset: 0x000037BD
		// (set) Token: 0x060004BD RID: 1213 RVA: 0x000055C5 File Offset: 0x000037C5
		public BattleJoinType BattleJoinType { get; set; }
	}
}
