using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000145 RID: 325
	public class TransposedLineFormation : LineFormation
	{
		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x060010DD RID: 4317 RVA: 0x00030767 File Offset: 0x0002E967
		public override float IntervalMultiplier
		{
			get
			{
				return 1.5f;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x060010DE RID: 4318 RVA: 0x0003076E File Offset: 0x0002E96E
		public override float DistanceMultiplier
		{
			get
			{
				return 1.5f;
			}
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x00030775 File Offset: 0x0002E975
		public TransposedLineFormation(IFormation owner)
			: base(owner, true)
		{
			base.IsStaggered = false;
			this.IsTransforming = true;
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x0003078D File Offset: 0x0002E98D
		public override IFormationArrangement Clone(IFormation formation)
		{
			return new TransposedLineFormation(formation);
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00030798 File Offset: 0x0002E998
		public override void RearrangeFrom(IFormationArrangement arrangement)
		{
			ColumnFormation columnFormation;
			if ((columnFormation = arrangement as ColumnFormation) != null)
			{
				base.FormFromFlankWidth(columnFormation.ColumnCount, false);
			}
			else
			{
				int num = MathF.Ceiling(MathF.Sqrt((float)(arrangement.UnitCount / ColumnFormation.ArrangementAspectRatio)));
				base.FormFromFlankWidth(num, false);
			}
			base.RearrangeFrom(arrangement);
		}
	}
}
