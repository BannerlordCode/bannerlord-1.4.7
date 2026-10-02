using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000220 RID: 544
	public class FormationPocket
	{
		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06002080 RID: 8320 RVA: 0x000724A1 File Offset: 0x000706A1
		// (set) Token: 0x06002081 RID: 8321 RVA: 0x000724A9 File Offset: 0x000706A9
		public Func<Agent, int> PriorityFunction { get; private set; }

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06002082 RID: 8322 RVA: 0x000724B2 File Offset: 0x000706B2
		// (set) Token: 0x06002083 RID: 8323 RVA: 0x000724BA File Offset: 0x000706BA
		public int MaxValue { get; private set; }

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06002084 RID: 8324 RVA: 0x000724C3 File Offset: 0x000706C3
		// (set) Token: 0x06002085 RID: 8325 RVA: 0x000724CB File Offset: 0x000706CB
		public int TroopCount { get; private set; }

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06002086 RID: 8326 RVA: 0x000724D4 File Offset: 0x000706D4
		// (set) Token: 0x06002087 RID: 8327 RVA: 0x000724DC File Offset: 0x000706DC
		public int Index { get; private set; }

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06002088 RID: 8328 RVA: 0x000724E5 File Offset: 0x000706E5
		// (set) Token: 0x06002089 RID: 8329 RVA: 0x000724ED File Offset: 0x000706ED
		public int AddedTroopCount { get; private set; }

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x0600208A RID: 8330 RVA: 0x000724F6 File Offset: 0x000706F6
		// (set) Token: 0x0600208B RID: 8331 RVA: 0x000724FE File Offset: 0x000706FE
		public int ScoreToSeek { get; private set; }

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x0600208C RID: 8332 RVA: 0x00072507 File Offset: 0x00070707
		// (set) Token: 0x0600208D RID: 8333 RVA: 0x0007250F File Offset: 0x0007070F
		public int BestScoreSoFar { get; private set; }

		// Token: 0x0600208E RID: 8334 RVA: 0x00072518 File Offset: 0x00070718
		public FormationPocket(Func<Agent, int> priorityFunction, int maxValue, int troopCount, int index)
		{
			this.PriorityFunction = priorityFunction;
			this.MaxValue = maxValue;
			this.TroopCount = troopCount;
			this.Index = index;
			this.AddedTroopCount = 0;
			this.ScoreToSeek = maxValue;
			this.BestScoreSoFar = 0;
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x00072554 File Offset: 0x00070754
		public void AddTroop()
		{
			int addedTroopCount = this.AddedTroopCount;
			this.AddedTroopCount = addedTroopCount + 1;
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x00072571 File Offset: 0x00070771
		public bool IsFormationPocketFilled()
		{
			return this.AddedTroopCount >= this.TroopCount;
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x00072584 File Offset: 0x00070784
		public void UpdateScoreToSeek()
		{
			this.ScoreToSeek = this.BestScoreSoFar;
			this.BestScoreSoFar = 0;
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x00072599 File Offset: 0x00070799
		public void SetBestScoreSoFar(int bestScoreSoFar)
		{
			this.BestScoreSoFar = bestScoreSoFar;
		}
	}
}
