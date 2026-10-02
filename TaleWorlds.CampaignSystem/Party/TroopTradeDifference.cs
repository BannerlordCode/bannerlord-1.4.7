using System;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x02000301 RID: 769
	public struct TroopTradeDifference
	{
		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06002D29 RID: 11561 RVA: 0x000BF060 File Offset: 0x000BD260
		// (set) Token: 0x06002D2A RID: 11562 RVA: 0x000BF068 File Offset: 0x000BD268
		public CharacterObject Troop { get; set; }

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06002D2B RID: 11563 RVA: 0x000BF071 File Offset: 0x000BD271
		// (set) Token: 0x06002D2C RID: 11564 RVA: 0x000BF079 File Offset: 0x000BD279
		public bool IsPrisoner { get; set; }

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06002D2D RID: 11565 RVA: 0x000BF082 File Offset: 0x000BD282
		// (set) Token: 0x06002D2E RID: 11566 RVA: 0x000BF08A File Offset: 0x000BD28A
		public int FromCount { get; set; }

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06002D2F RID: 11567 RVA: 0x000BF093 File Offset: 0x000BD293
		// (set) Token: 0x06002D30 RID: 11568 RVA: 0x000BF09B File Offset: 0x000BD29B
		public int ToCount { get; set; }

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x06002D31 RID: 11569 RVA: 0x000BF0A4 File Offset: 0x000BD2A4
		public int DifferenceCount
		{
			get
			{
				return this.FromCount - this.ToCount;
			}
		}

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x06002D32 RID: 11570 RVA: 0x000BF0B3 File Offset: 0x000BD2B3
		// (set) Token: 0x06002D33 RID: 11571 RVA: 0x000BF0BB File Offset: 0x000BD2BB
		public bool IsEmpty { get; private set; }

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x06002D34 RID: 11572 RVA: 0x000BF0C4 File Offset: 0x000BD2C4
		public static TroopTradeDifference Empty
		{
			get
			{
				return new TroopTradeDifference
				{
					IsEmpty = true
				};
			}
		}
	}
}
