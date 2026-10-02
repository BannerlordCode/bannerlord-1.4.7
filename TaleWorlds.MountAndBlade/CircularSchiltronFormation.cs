using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200013D RID: 317
	public class CircularSchiltronFormation : CircularFormation
	{
		// Token: 0x06000F54 RID: 3924 RVA: 0x00029F45 File Offset: 0x00028145
		public CircularSchiltronFormation(IFormation owner)
			: base(owner)
		{
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x00029F4E File Offset: 0x0002814E
		public override IFormationArrangement Clone(IFormation formation)
		{
			return new CircularSchiltronFormation(formation);
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000F56 RID: 3926 RVA: 0x00029F58 File Offset: 0x00028158
		public override float MaximumWidth
		{
			get
			{
				int unitCountWithOverride = base.GetUnitCountWithOverride();
				int currentMaximumRankCount = base.GetCurrentMaximumRankCount(unitCountWithOverride);
				float num = this.owner.MaximumInterval + base.UnitDiameter;
				float num2 = this.owner.MaximumDistance + base.UnitDiameter;
				return base.GetCircumferenceAux(unitCountWithOverride, currentMaximumRankCount, num, num2) / 3.1415927f;
			}
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00029FAC File Offset: 0x000281AC
		public void Form()
		{
			int unitCountWithOverride = base.GetUnitCountWithOverride();
			int currentMaximumRankCount = base.GetCurrentMaximumRankCount(unitCountWithOverride);
			float circumferenceFromRankCount = base.GetCircumferenceFromRankCount(currentMaximumRankCount);
			base.FormFromCircumference(circumferenceFromRankCount);
		}
	}
}
