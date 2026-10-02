using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000146 RID: 326
	public class RectilinearSchiltronFormation : SquareFormation
	{
		// Token: 0x060010E2 RID: 4322 RVA: 0x000307E5 File Offset: 0x0002E9E5
		public RectilinearSchiltronFormation(IFormation owner)
			: base(owner)
		{
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x000307EE File Offset: 0x0002E9EE
		public override IFormationArrangement Clone(IFormation formation)
		{
			return new RectilinearSchiltronFormation(formation);
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x060010E4 RID: 4324 RVA: 0x000307F8 File Offset: 0x0002E9F8
		public override float MaximumWidth
		{
			get
			{
				int num;
				int maximumRankCount = SquareFormation.GetMaximumRankCount(base.GetUnitCountWithOverride(), out num);
				return SquareFormation.GetSideWidthFromUnitCount(base.GetUnitsPerSideFromRankCount(maximumRankCount), this.owner.MaximumInterval, base.UnitDiameter);
			}
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x00030830 File Offset: 0x0002EA30
		public void Form()
		{
			int num;
			int maximumRankCount = SquareFormation.GetMaximumRankCount(base.GetUnitCountWithOverride(), out num);
			base.FormFromRankCount(maximumRankCount);
		}
	}
}
