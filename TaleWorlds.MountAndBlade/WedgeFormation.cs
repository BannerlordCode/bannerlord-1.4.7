using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000149 RID: 329
	public class WedgeFormation : LineFormation
	{
		// Token: 0x0600110C RID: 4364 RVA: 0x000312C7 File Offset: 0x0002F4C7
		public WedgeFormation(IFormation owner)
			: base(owner, true)
		{
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x000312D1 File Offset: 0x0002F4D1
		public override IFormationArrangement Clone(IFormation formation)
		{
			return new WedgeFormation(formation);
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x000312DC File Offset: 0x0002F4DC
		private int GetUnitCountOfRank(int rankIndex)
		{
			int num = rankIndex * 2 * 3 + 3;
			return MathF.Min(base.FileCount, num);
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00031300 File Offset: 0x0002F500
		protected override bool IsUnitPositionRestrained(int fileIndex, int rankIndex)
		{
			if (base.IsUnitPositionRestrained(fileIndex, rankIndex))
			{
				return true;
			}
			int unitCountOfRank = this.GetUnitCountOfRank(rankIndex);
			int num = (base.FileCount - unitCountOfRank) / 2;
			return fileIndex < num || fileIndex >= num + unitCountOfRank;
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x0003133C File Offset: 0x0002F53C
		protected override void MakeRestrainedPositionsUnavailable()
		{
			for (int i = 0; i < base.FileCount; i++)
			{
				for (int j = 0; j < base.RankCount; j++)
				{
					if (this.IsUnitPositionRestrained(i, j))
					{
						this.UnitPositionAvailabilities[i, j] = 1;
					}
				}
			}
		}
	}
}
