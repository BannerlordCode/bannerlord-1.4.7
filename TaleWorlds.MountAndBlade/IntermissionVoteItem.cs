using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000301 RID: 769
	public class IntermissionVoteItem
	{
		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06002BCD RID: 11213 RVA: 0x000A84A9 File Offset: 0x000A66A9
		// (set) Token: 0x06002BCE RID: 11214 RVA: 0x000A84B1 File Offset: 0x000A66B1
		public int VoteCount { get; private set; }

		// Token: 0x06002BCF RID: 11215 RVA: 0x000A84BA File Offset: 0x000A66BA
		public IntermissionVoteItem(string id, int index)
		{
			this.Id = id;
			this.Index = index;
			this.VoteCount = 0;
		}

		// Token: 0x06002BD0 RID: 11216 RVA: 0x000A84D7 File Offset: 0x000A66D7
		public void SetVoteCount(int voteCount)
		{
			this.VoteCount = voteCount;
		}

		// Token: 0x06002BD1 RID: 11217 RVA: 0x000A84E0 File Offset: 0x000A66E0
		public void IncreaseVoteCount(int incrementAmount)
		{
			this.VoteCount += incrementAmount;
		}

		// Token: 0x04001146 RID: 4422
		public readonly string Id;

		// Token: 0x04001147 RID: 4423
		public readonly int Index;
	}
}
