using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000147 RID: 327
	public class SkeinFormation : LineFormation
	{
		// Token: 0x060010E6 RID: 4326 RVA: 0x00030852 File Offset: 0x0002EA52
		public SkeinFormation(IFormation owner)
			: base(owner, true)
		{
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x0003085C File Offset: 0x0002EA5C
		public override IFormationArrangement Clone(IFormation formation)
		{
			return new SkeinFormation(formation);
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x00030864 File Offset: 0x0002EA64
		protected override Vec2 GetLocalPositionOfUnit(int fileIndex, int rankIndex)
		{
			float num = (float)(base.FileCount - 1) * (base.Interval + base.UnitDiameter);
			Vec2 vec = new Vec2((float)fileIndex * (base.Interval + base.UnitDiameter) - num / 2f, (float)(-(float)rankIndex) * (base.Distance + base.UnitDiameter));
			float offsetOfFile = this.GetOffsetOfFile(fileIndex);
			vec.y -= offsetOfFile;
			return vec;
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x000308D0 File Offset: 0x0002EAD0
		protected override Vec2 GetLocalPositionOfUnitWithAdjustment(int fileIndex, int rankIndex, float distanceBetweenAgentsAdjustment)
		{
			float num = base.Interval + distanceBetweenAgentsAdjustment;
			float num2 = (float)(base.FileCount - 1) * (num + base.UnitDiameter);
			Vec2 vec = new Vec2((float)fileIndex * (num + base.UnitDiameter) - num2 / 2f, (float)(-(float)rankIndex) * (base.Distance + base.UnitDiameter));
			float offsetOfFile = this.GetOffsetOfFile(fileIndex);
			vec.y -= offsetOfFile;
			return vec;
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x0003093C File Offset: 0x0002EB3C
		private float GetOffsetOfFile(int fileIndex)
		{
			int num = base.FileCount / 2;
			return (float)MathF.Abs(fileIndex - num) * (base.Interval + base.UnitDiameter) / 2f;
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x00030970 File Offset: 0x0002EB70
		protected override bool TryGetUnitPositionIndexFromLocalPosition(Vec2 localPosition, out int fileIndex, out int rankIndex)
		{
			float num = (float)(base.FileCount - 1) * (base.Interval + base.UnitDiameter);
			fileIndex = MathF.Round((localPosition.x + num / 2f) / (base.Interval + base.UnitDiameter));
			if (fileIndex < 0 || fileIndex >= base.FileCount)
			{
				rankIndex = -1;
				return false;
			}
			float offsetOfFile = this.GetOffsetOfFile(fileIndex);
			localPosition.y += offsetOfFile;
			rankIndex = MathF.Round(-localPosition.y / (base.Distance + base.UnitDiameter));
			if (rankIndex < 0 || rankIndex >= base.RankCount)
			{
				fileIndex = -1;
				return false;
			}
			return true;
		}
	}
}
