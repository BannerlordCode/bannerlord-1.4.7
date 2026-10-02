using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000148 RID: 328
	public class SquareFormation : LineFormation
	{
		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x060010EC RID: 4332 RVA: 0x00030A12 File Offset: 0x0002EC12
		// (set) Token: 0x060010ED RID: 4333 RVA: 0x00030A2C File Offset: 0x0002EC2C
		public override float Width
		{
			get
			{
				return SquareFormation.GetSideWidthFromUnitCount(this.UnitCountOfOuterSide, base.Interval, base.UnitDiameter);
			}
			set
			{
				this.FormFromBorderSideWidth(value);
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x00030A42 File Offset: 0x0002EC42
		public override float Depth
		{
			get
			{
				return SquareFormation.GetSideWidthFromUnitCount(this.UnitCountOfOuterSide, base.Interval, base.UnitDiameter);
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x00030A5C File Offset: 0x0002EC5C
		public override float MinimumWidth
		{
			get
			{
				int num;
				int maximumRankCount = SquareFormation.GetMaximumRankCount(base.GetUnitCountWithOverride(), out num);
				return SquareFormation.GetSideWidthFromUnitCount(this.GetUnitsPerSideFromRankCount(maximumRankCount), this.owner.MinimumInterval, base.UnitDiameter);
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x060010F0 RID: 4336 RVA: 0x00030A94 File Offset: 0x0002EC94
		public override float MaximumWidth
		{
			get
			{
				return SquareFormation.GetSideWidthFromUnitCount(this.GetUnitsPerSideFromRankCount(1), this.owner.MaximumInterval, base.UnitDiameter);
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x060010F1 RID: 4337 RVA: 0x00030AB3 File Offset: 0x0002ECB3
		private int UnitCountOfOuterSide
		{
			get
			{
				return MathF.Ceiling((float)base.FileCount / 4f) + 1;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x060010F2 RID: 4338 RVA: 0x00030AC9 File Offset: 0x0002ECC9
		private int MaxRank
		{
			get
			{
				return (this.UnitCountOfOuterSide + 1) / 2;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x060010F3 RID: 4339 RVA: 0x00030AD5 File Offset: 0x0002ECD5
		private new float Distance
		{
			get
			{
				return base.Interval;
			}
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00030ADD File Offset: 0x0002ECDD
		public SquareFormation(IFormation owner)
			: base(owner, true, true)
		{
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x00030AE8 File Offset: 0x0002ECE8
		public override IFormationArrangement Clone(IFormation formation)
		{
			return new SquareFormation(formation);
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x00030AF0 File Offset: 0x0002ECF0
		public override void DeepCopyFrom(IFormationArrangement arrangement)
		{
			base.DeepCopyFrom(arrangement);
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00030AFC File Offset: 0x0002ECFC
		public void FormFromBorderSideWidth(float borderSideWidth)
		{
			int num = MathF.Max(1, (int)((borderSideWidth - base.UnitDiameter) / (base.Interval + base.UnitDiameter) + 1E-05f)) + 1;
			this.FormFromBorderUnitCountPerSide(num);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00030B36 File Offset: 0x0002ED36
		public void FormFromBorderUnitCountPerSide(int unitCountPerSide)
		{
			if (unitCountPerSide == 1)
			{
				base.FlankWidth = base.UnitDiameter;
				return;
			}
			base.FlankWidth = (float)(4 * (unitCountPerSide - 1) - 1) * (base.Interval + base.UnitDiameter) + base.UnitDiameter;
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00030B6C File Offset: 0x0002ED6C
		public int GetUnitsPerSideFromRankCount(int rankCount)
		{
			int unitCountWithOverride = base.GetUnitCountWithOverride();
			int num;
			rankCount = MathF.Min(SquareFormation.GetMaximumRankCount(unitCountWithOverride, out num), rankCount);
			float num2 = (float)unitCountWithOverride / (4f * (float)rankCount) + (float)rankCount;
			int num3 = MathF.Ceiling(num2);
			int num4 = MathF.Round(num2);
			if (num4 < num3 && num4 * num4 == unitCountWithOverride)
			{
				num3 = num4;
			}
			if (num3 == 0)
			{
				num3 = 1;
			}
			return num3;
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x00030BC0 File Offset: 0x0002EDC0
		protected static int GetMaximumRankCount(int unitCount, out int minimumFlankCount)
		{
			int num = (int)MathF.Sqrt((float)unitCount);
			if (num * num != unitCount)
			{
				num++;
			}
			minimumFlankCount = num;
			return MathF.Max(1, (num + 1) / 2);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00030BF0 File Offset: 0x0002EDF0
		public void FormFromRankCount(int rankCount)
		{
			int unitsPerSideFromRankCount = this.GetUnitsPerSideFromRankCount(rankCount);
			this.FormFromBorderUnitCountPerSide(unitsPerSideFromRankCount);
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00030C0C File Offset: 0x0002EE0C
		private SquareFormation.Side GetSideOfUnitPosition(int fileIndex)
		{
			return (SquareFormation.Side)(fileIndex / (this.UnitCountOfOuterSide - 1));
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x00030C18 File Offset: 0x0002EE18
		private SquareFormation.Side? GetSideOfUnitPosition(int fileIndex, int rankIndex)
		{
			SquareFormation.Side sideOfUnitPosition = this.GetSideOfUnitPosition(fileIndex);
			if (rankIndex == 0)
			{
				return new SquareFormation.Side?(sideOfUnitPosition);
			}
			int num = this.UnitCountOfOuterSide - 2 * rankIndex;
			if (num == 1 && sideOfUnitPosition != SquareFormation.Side.Front)
			{
				return null;
			}
			int num2 = fileIndex % (this.UnitCountOfOuterSide - 1);
			int num3 = this.UnitCountOfOuterSide - num;
			num3 /= 2;
			if (num2 >= num3 && this.UnitCountOfOuterSide - num2 - 1 > num3)
			{
				return new SquareFormation.Side?(sideOfUnitPosition);
			}
			return null;
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x00030C94 File Offset: 0x0002EE94
		private Vec2 GetLocalPositionOfUnitAux(int fileIndex, int rankIndex, float usedInterval)
		{
			if (this.UnitCountOfOuterSide == 1)
			{
				return Vec2.Zero;
			}
			SquareFormation.Side sideOfUnitPosition = this.GetSideOfUnitPosition(fileIndex);
			float num = (float)(this.UnitCountOfOuterSide - 1) * (usedInterval + base.UnitDiameter);
			float num2 = (float)(fileIndex % (this.UnitCountOfOuterSide - 1)) * (usedInterval + base.UnitDiameter);
			float num3 = (float)rankIndex * (this.Distance + base.UnitDiameter);
			Vec2 vec;
			switch (sideOfUnitPosition)
			{
			case SquareFormation.Side.Front:
				vec = new Vec2(-num / 2f, 0f);
				vec += new Vec2(num2, -num3);
				break;
			case SquareFormation.Side.Right:
				vec = new Vec2(num / 2f, 0f);
				vec += new Vec2(-num3, -num2);
				break;
			case SquareFormation.Side.Rear:
				vec = new Vec2(num / 2f, -num);
				vec += new Vec2(-num2, num3);
				break;
			case SquareFormation.Side.Left:
				vec = new Vec2(-num / 2f, -num);
				vec += new Vec2(num3, num2);
				break;
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Formation\\SquareFormation.cs", "GetLocalPositionOfUnitAux", 369);
				vec = Vec2.Zero;
				break;
			}
			return vec;
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x00030DC8 File Offset: 0x0002EFC8
		protected override Vec2 GetLocalPositionOfUnit(int fileIndex, int rankIndex)
		{
			int num = this.ShiftFileIndex(fileIndex);
			return this.GetLocalPositionOfUnitAux(num, rankIndex, base.Interval);
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00030DEC File Offset: 0x0002EFEC
		protected override Vec2 GetLocalPositionOfUnitWithAdjustment(int fileIndex, int rankIndex, float distanceBetweenAgentsAdjustment)
		{
			int num = this.ShiftFileIndex(fileIndex);
			return this.GetLocalPositionOfUnitAux(num, rankIndex, base.Interval + distanceBetweenAgentsAdjustment);
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00030E14 File Offset: 0x0002F014
		protected override Vec2 GetLocalDirectionOfUnit(int fileIndex, int rankIndex)
		{
			int num = this.ShiftFileIndex(fileIndex);
			switch (this.GetSideOfUnitPosition(num))
			{
			case SquareFormation.Side.Front:
				return Vec2.Forward;
			case SquareFormation.Side.Right:
				return Vec2.Side;
			case SquareFormation.Side.Rear:
				return -Vec2.Forward;
			case SquareFormation.Side.Left:
				return -Vec2.Side;
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Formation\\SquareFormation.cs", "GetLocalDirectionOfUnit", 448);
				return Vec2.Forward;
			}
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00030E8C File Offset: 0x0002F08C
		public override Vec2? GetLocalDirectionOfUnitOrDefault(IFormationUnit unit)
		{
			if (unit.FormationFileIndex < 0 || unit.FormationRankIndex < 0)
			{
				return null;
			}
			return new Vec2?(this.GetLocalDirectionOfUnit(unit.FormationFileIndex, unit.FormationRankIndex));
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00030ECC File Offset: 0x0002F0CC
		protected override bool IsUnitPositionRestrained(int fileIndex, int rankIndex)
		{
			if (base.IsUnitPositionRestrained(fileIndex, rankIndex))
			{
				return true;
			}
			if (rankIndex >= this.MaxRank)
			{
				return true;
			}
			int num = this.ShiftFileIndex(fileIndex);
			return this.GetSideOfUnitPosition(num, rankIndex) == null;
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00030F0C File Offset: 0x0002F10C
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

		// Token: 0x06001105 RID: 4357 RVA: 0x00030F54 File Offset: 0x0002F154
		private SquareFormation.Side GetSideOfLocalPosition(Vec2 localPosition)
		{
			float num = (float)(this.UnitCountOfOuterSide - 1) * (base.Interval + base.UnitDiameter);
			Vec2 vec = new Vec2(0f, -num / 2f);
			Vec2 vec2 = localPosition - vec;
			vec2.y *= (base.Interval + base.UnitDiameter) / (this.Distance + base.UnitDiameter);
			float num2 = vec2.RotationInRadians;
			if (num2 < 0f)
			{
				num2 += 6.2831855f;
			}
			if (num2 <= 0.7863982f || num2 > 5.4987874f)
			{
				return SquareFormation.Side.Front;
			}
			if (num2 <= 2.3571944f)
			{
				return SquareFormation.Side.Left;
			}
			if (num2 <= 3.927991f)
			{
				return SquareFormation.Side.Rear;
			}
			return SquareFormation.Side.Right;
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x00030FFC File Offset: 0x0002F1FC
		protected override bool TryGetUnitPositionIndexFromLocalPosition(Vec2 localPosition, out int fileIndex, out int rankIndex)
		{
			SquareFormation.Side sideOfLocalPosition = this.GetSideOfLocalPosition(localPosition);
			float num = (float)(this.UnitCountOfOuterSide - 1) * (base.Interval + base.UnitDiameter);
			float num2;
			float num3;
			switch (sideOfLocalPosition)
			{
			case SquareFormation.Side.Front:
			{
				Vec2 vec = localPosition - new Vec2(-num / 2f, 0f);
				num2 = vec.x;
				num3 = -vec.y;
				break;
			}
			case SquareFormation.Side.Right:
			{
				Vec2 vec2 = localPosition - new Vec2(num / 2f, 0f);
				num2 = -vec2.y;
				num3 = -vec2.x;
				break;
			}
			case SquareFormation.Side.Rear:
			{
				Vec2 vec3 = localPosition - new Vec2(num / 2f, -num);
				num2 = -vec3.x;
				num3 = vec3.y;
				break;
			}
			case SquareFormation.Side.Left:
			{
				Vec2 vec4 = localPosition - new Vec2(-num / 2f, -num);
				num2 = vec4.y;
				num3 = vec4.x;
				break;
			}
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Formation\\SquareFormation.cs", "TryGetUnitPositionIndexFromLocalPosition", 575);
				num2 = 0f;
				num3 = 0f;
				break;
			}
			rankIndex = MathF.Round(num3 / (this.Distance + base.UnitDiameter));
			if (rankIndex < 0 || rankIndex >= base.RankCount || rankIndex >= this.MaxRank)
			{
				fileIndex = -1;
				return false;
			}
			int num4 = MathF.Round(num2 / (base.Interval + base.UnitDiameter));
			if (num4 >= this.UnitCountOfOuterSide - 1)
			{
				fileIndex = 1;
				return false;
			}
			int num5 = num4 + (this.UnitCountOfOuterSide - 1) * (int)sideOfLocalPosition;
			fileIndex = this.UnshiftFileIndex(num5);
			return fileIndex >= 0 && fileIndex < base.FileCount;
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x0003118C File Offset: 0x0002F38C
		private int ShiftFileIndex(int fileIndex)
		{
			int num = this.UnitCountOfOuterSide + this.UnitCountOfOuterSide / 2 - 2;
			int num2 = fileIndex - num;
			if (num2 < 0)
			{
				num2 += (this.UnitCountOfOuterSide - 1) * 4;
			}
			return num2;
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x000311C4 File Offset: 0x0002F3C4
		private int UnshiftFileIndex(int shiftedFileIndex)
		{
			int num = this.UnitCountOfOuterSide + this.UnitCountOfOuterSide / 2 - 2;
			int num2 = shiftedFileIndex + num;
			if (num2 >= (this.UnitCountOfOuterSide - 1) * 4)
			{
				num2 -= (this.UnitCountOfOuterSide - 1) * 4;
			}
			return num2;
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x00031202 File Offset: 0x0002F402
		protected static float GetSideWidthFromUnitCount(int sideUnitCount, float interval, float unitDiameter)
		{
			if (sideUnitCount > 0)
			{
				return (float)(sideUnitCount - 1) * (interval + unitDiameter) + unitDiameter;
			}
			return 0f;
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00031218 File Offset: 0x0002F418
		public override void TurnBackwards()
		{
			int num = base.FileCount / 2;
			for (int i = 0; i <= base.FileCount / 2; i++)
			{
				for (int j = 0; j < base.RankCount; j++)
				{
					int num2 = i + num;
					if (num2 < base.FileCount)
					{
						IFormationUnit unitAt = base.GetUnitAt(i, j);
						IFormationUnit unitAt2 = base.GetUnitAt(num2, j);
						if (unitAt != unitAt2)
						{
							if (unitAt != null && unitAt2 != null)
							{
								base.SwitchUnitLocations(unitAt, unitAt2);
							}
							else if (unitAt != null)
							{
								if (base.IsUnitPositionAvailable(num2, j))
								{
									base.RelocateUnit(unitAt, num2, j);
								}
							}
							else if (unitAt2 != null && base.IsUnitPositionAvailable(i, j))
							{
								base.RelocateUnit(unitAt2, i, j);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x000312C5 File Offset: 0x0002F4C5
		protected override void UpdateFrontUnitTypeDelegate()
		{
		}

		// Token: 0x02000463 RID: 1123
		private enum Side
		{
			// Token: 0x04001A17 RID: 6679
			Front,
			// Token: 0x04001A18 RID: 6680
			Right,
			// Token: 0x04001A19 RID: 6681
			Rear,
			// Token: 0x04001A1A RID: 6682
			Left
		}
	}
}
