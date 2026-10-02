using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200013C RID: 316
	public class CircularFormation : LineFormation
	{
		// Token: 0x06000F39 RID: 3897 RVA: 0x00029914 File Offset: 0x00027B14
		public CircularFormation(IFormation owner)
			: base(owner, true, true)
		{
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x0002991F File Offset: 0x00027B1F
		public override IFormationArrangement Clone(IFormation formation)
		{
			return new CircularFormation(formation);
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x00029928 File Offset: 0x00027B28
		private float GetDistanceFromCenterOfRank(int rankIndex)
		{
			float num = this.Radius - (float)rankIndex * (base.Distance + base.UnitDiameter);
			if (num >= 0f)
			{
				return num;
			}
			return 0f;
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x0002995C File Offset: 0x00027B5C
		protected override bool IsDeepenApplicable()
		{
			return this.Radius - (float)base.RankCount * (base.Distance + base.UnitDiameter) >= 0f;
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x00029984 File Offset: 0x00027B84
		protected override bool IsNarrowApplicable(int amount)
		{
			return ((float)(base.FileCount - 1 - amount) * (base.Interval + base.UnitDiameter) + base.UnitDiameter) / 6.2831855f - (float)base.RankCount * (base.Distance + base.UnitDiameter) >= 0f;
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x000299D8 File Offset: 0x00027BD8
		private int GetUnitCountOfRank(int rankIndex)
		{
			if (rankIndex == 0)
			{
				return base.FileCount;
			}
			float distanceFromCenterOfRank = this.GetDistanceFromCenterOfRank(rankIndex);
			int num = MathF.Floor(6.2831855f * distanceFromCenterOfRank / (base.Interval + base.UnitDiameter));
			return MathF.Max(1, num);
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000F3F RID: 3903 RVA: 0x00029A19 File Offset: 0x00027C19
		// (set) Token: 0x06000F40 RID: 3904 RVA: 0x00029A24 File Offset: 0x00027C24
		public override float Width
		{
			get
			{
				return this.Diameter;
			}
			set
			{
				float num = 3.1415927f * value;
				this.FormFromCircumference(num);
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000F41 RID: 3905 RVA: 0x00029A42 File Offset: 0x00027C42
		public override float Depth
		{
			get
			{
				return this.Diameter;
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000F42 RID: 3906 RVA: 0x00029A4A File Offset: 0x00027C4A
		private float Diameter
		{
			get
			{
				return 2f * this.Radius;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000F43 RID: 3907 RVA: 0x00029A58 File Offset: 0x00027C58
		private float Radius
		{
			get
			{
				return (base.FlankWidth + base.Interval) / 6.2831855f;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000F44 RID: 3908 RVA: 0x00029A70 File Offset: 0x00027C70
		public override float MinimumWidth
		{
			get
			{
				int unitCountWithOverride = base.GetUnitCountWithOverride();
				int currentMaximumRankCount = this.GetCurrentMaximumRankCount(unitCountWithOverride);
				float num = this.owner.MinimumInterval + base.UnitDiameter;
				float num2 = this.owner.MinimumDistance + base.UnitDiameter;
				return this.GetCircumferenceAux(unitCountWithOverride, currentMaximumRankCount, num, num2) / 3.1415927f;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000F45 RID: 3909 RVA: 0x00029AC4 File Offset: 0x00027CC4
		public override float MaximumWidth
		{
			get
			{
				int unitCountWithOverride = base.GetUnitCountWithOverride();
				float num = this.owner.MaximumInterval + base.UnitDiameter;
				return MathF.Max(0f, (float)unitCountWithOverride * num) / 3.1415927f;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000F46 RID: 3910 RVA: 0x00029AFF File Offset: 0x00027CFF
		private int MaxRank
		{
			get
			{
				return MathF.Floor(this.Radius / (base.Distance + base.UnitDiameter));
			}
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x00029B1C File Offset: 0x00027D1C
		protected override bool IsUnitPositionRestrained(int fileIndex, int rankIndex)
		{
			if (base.IsUnitPositionRestrained(fileIndex, rankIndex))
			{
				return true;
			}
			if (rankIndex > this.MaxRank)
			{
				return true;
			}
			int unitCountOfRank = this.GetUnitCountOfRank(rankIndex);
			int num = (base.FileCount - unitCountOfRank) / 2;
			return fileIndex < num || fileIndex >= num + unitCountOfRank;
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x00029B60 File Offset: 0x00027D60
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

		// Token: 0x06000F49 RID: 3913 RVA: 0x00029BA8 File Offset: 0x00027DA8
		protected override Vec2 GetLocalDirectionOfUnit(int fileIndex, int rankIndex)
		{
			int unitCountOfRank = this.GetUnitCountOfRank(rankIndex);
			int num = (base.FileCount - unitCountOfRank) / 2;
			Vec2 vec = Vec2.FromRotation((float)((fileIndex - num) * 2) * 3.1415927f / (float)unitCountOfRank + 3.1415927f);
			vec.x *= -1f;
			return vec;
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x00029BF4 File Offset: 0x00027DF4
		public override Vec2? GetLocalDirectionOfUnitOrDefault(IFormationUnit unit)
		{
			if (unit.FormationFileIndex < 0 || unit.FormationRankIndex < 0)
			{
				return null;
			}
			return new Vec2?(this.GetLocalDirectionOfUnit(unit.FormationFileIndex, unit.FormationRankIndex));
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x00029C34 File Offset: 0x00027E34
		protected override Vec2 GetLocalPositionOfUnit(int fileIndex, int rankIndex)
		{
			Vec2 vec = new Vec2(0f, -this.Radius);
			Vec2 localDirectionOfUnit = this.GetLocalDirectionOfUnit(fileIndex, rankIndex);
			float distanceFromCenterOfRank = this.GetDistanceFromCenterOfRank(rankIndex);
			return vec + localDirectionOfUnit * distanceFromCenterOfRank;
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x00029C6F File Offset: 0x00027E6F
		protected override Vec2 GetLocalPositionOfUnitWithAdjustment(int fileIndex, int rankIndex, float distanceBetweenAgentsAdjustment)
		{
			return this.GetLocalPositionOfUnit(fileIndex, rankIndex);
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x00029C7C File Offset: 0x00027E7C
		protected override bool TryGetUnitPositionIndexFromLocalPosition(Vec2 localPosition, out int fileIndex, out int rankIndex)
		{
			Vec2 vec = new Vec2(0f, -this.Radius);
			Vec2 vec2 = localPosition - vec;
			float length = vec2.Length;
			rankIndex = MathF.Round((length - this.Radius) / (base.Distance + base.UnitDiameter) * -1f);
			if (rankIndex < 0 || rankIndex >= base.RankCount)
			{
				fileIndex = -1;
				return false;
			}
			if (this.Radius - (float)rankIndex * (base.Distance + base.UnitDiameter) < 0f)
			{
				fileIndex = -1;
				return false;
			}
			int unitCountOfRank = this.GetUnitCountOfRank(rankIndex);
			int num = (base.FileCount - unitCountOfRank) / 2;
			vec2.x *= -1f;
			float num2 = vec2.RotationInRadians;
			num2 -= 3.1415927f;
			if (num2 < 0f)
			{
				num2 += 6.2831855f;
			}
			int num3 = MathF.Round(num2 / 2f / 3.1415927f * (float)unitCountOfRank);
			fileIndex = num3 + num;
			return fileIndex >= 0 && fileIndex < base.FileCount;
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x00029D84 File Offset: 0x00027F84
		protected int GetCurrentMaximumRankCount(int unitCount)
		{
			int num = 0;
			int i = 0;
			float num2 = base.Interval + base.UnitDiameter;
			float num3 = base.Distance + base.UnitDiameter;
			while (i < unitCount)
			{
				float num4 = (float)num * num3;
				int num5 = (int)(6.2831855f * num4 / num2);
				i += MathF.Max(1, num5);
				num++;
			}
			return MathF.Max(num, 1);
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x00029DE0 File Offset: 0x00027FE0
		public float GetCircumferenceFromRankCount(int rankCount)
		{
			int unitCountWithOverride = base.GetUnitCountWithOverride();
			rankCount = MathF.Min(this.GetCurrentMaximumRankCount(unitCountWithOverride), rankCount);
			float num = base.Interval + base.UnitDiameter;
			float num2 = base.Distance + base.UnitDiameter;
			return this.GetCircumferenceAux(unitCountWithOverride, rankCount, num, num2);
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x00029E2C File Offset: 0x0002802C
		public void FormFromCircumference(float circumference)
		{
			int unitCountWithOverride = base.GetUnitCountWithOverride();
			int currentMaximumRankCount = this.GetCurrentMaximumRankCount(unitCountWithOverride);
			float num = base.Interval + base.UnitDiameter;
			float num2 = base.Distance + base.UnitDiameter;
			float circumferenceAux = this.GetCircumferenceAux(unitCountWithOverride, currentMaximumRankCount, num, num2);
			float num3 = MathF.Max(0f, (float)unitCountWithOverride * num);
			circumference = MBMath.ClampFloat(circumference, circumferenceAux, num3);
			base.FlankWidth = Math.Max(circumference - base.Interval, base.UnitDiameter);
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x00029EA8 File Offset: 0x000280A8
		protected float GetCircumferenceAux(int unitCount, int rankCount, float radialInterval, float distanceInterval)
		{
			float num = (float)(6.283185307179586 * (double)distanceInterval);
			float num2 = MathF.Max(0f, (float)unitCount * radialInterval);
			float num3;
			int unitCountAux;
			do
			{
				num3 = num2;
				num2 = MathF.Max(0f, num3 - num);
				unitCountAux = CircularFormation.GetUnitCountAux(num2, rankCount, radialInterval, distanceInterval);
			}
			while (unitCountAux > unitCount && num3 > 0f);
			return num3;
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00029EFC File Offset: 0x000280FC
		private static int GetUnitCountAux(float circumference, int rankCount, float radialInterval, float distanceInterval)
		{
			int num = 0;
			double num2 = 6.283185307179586 * (double)distanceInterval;
			for (int i = 1; i <= rankCount; i++)
			{
				num += (int)(Math.Max(0.0, (double)circumference - (double)(rankCount - i) * num2) / (double)radialInterval);
			}
			return num;
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x00029F43 File Offset: 0x00028143
		protected override void UpdateFrontUnitTypeDelegate()
		{
		}
	}
}
