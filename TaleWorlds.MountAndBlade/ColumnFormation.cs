using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200013E RID: 318
	public class ColumnFormation : IFormationArrangement
	{
		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000F58 RID: 3928 RVA: 0x00029FD7 File Offset: 0x000281D7
		// (set) Token: 0x06000F59 RID: 3929 RVA: 0x00029FDF File Offset: 0x000281DF
		public IFormationUnit Vanguard
		{
			get
			{
				return this._vanguard;
			}
			private set
			{
				this.SetVanguard(value);
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000F5A RID: 3930 RVA: 0x00029FE8 File Offset: 0x000281E8
		// (set) Token: 0x06000F5B RID: 3931 RVA: 0x00029FF0 File Offset: 0x000281F0
		public int ColumnCount
		{
			get
			{
				return this.FileCount;
			}
			set
			{
				this.SetColumnCount(value);
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000F5C RID: 3932 RVA: 0x00029FF9 File Offset: 0x000281F9
		protected int FileCount
		{
			get
			{
				return this._units2D.Count1;
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000F5D RID: 3933 RVA: 0x0002A006 File Offset: 0x00028206
		public int RankCount
		{
			get
			{
				return this._units2D.Count2;
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000F5E RID: 3934 RVA: 0x0002A013 File Offset: 0x00028213
		public int VanguardFileIndex
		{
			get
			{
				if (this.FileCount % 2 != 0)
				{
					return this.FileCount / 2;
				}
				if (this.isExpandingFromRightSide)
				{
					return this.FileCount / 2 - 1;
				}
				return this.FileCount / 2;
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000F5F RID: 3935 RVA: 0x0002A043 File Offset: 0x00028243
		protected float Distance
		{
			get
			{
				return this.owner.Distance;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000F60 RID: 3936 RVA: 0x0002A050 File Offset: 0x00028250
		public float DistanceMultiplier
		{
			get
			{
				return 1.5f;
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000F61 RID: 3937 RVA: 0x0002A057 File Offset: 0x00028257
		protected float Interval
		{
			get
			{
				return this.owner.Interval;
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000F62 RID: 3938 RVA: 0x0002A064 File Offset: 0x00028264
		public float IntervalMultiplier
		{
			get
			{
				return 1.5f;
			}
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x0002A06C File Offset: 0x0002826C
		public ColumnFormation(IFormation ownerFormation, IFormationUnit vanguard = null, int columnCount = 1)
		{
			this.owner = ownerFormation;
			this._units2D = new MBList2D<IFormationUnit>(columnCount, 1);
			this._units2DWorkspace = new MBList2D<IFormationUnit>(columnCount, 1);
			this.ReconstructUnitsFromUnits2D();
			this._vanguard = vanguard;
			Action onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0002A0C4 File Offset: 0x000282C4
		public IFormationArrangement Clone(IFormation formation)
		{
			return new ColumnFormation(formation, this.Vanguard, this.ColumnCount);
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0002A0D8 File Offset: 0x000282D8
		public void DeepCopyFrom(IFormationArrangement arrangement)
		{
			this.UnitPositionsOnVanguardFileIndex = (arrangement as ColumnFormation).GetUnitPositionsOnVanguardFileIndex();
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000F66 RID: 3942 RVA: 0x0002A0EB File Offset: 0x000282EB
		// (set) Token: 0x06000F67 RID: 3943 RVA: 0x0002A0F3 File Offset: 0x000282F3
		public float Width
		{
			get
			{
				return this.FlankWidth;
			}
			set
			{
				this.FlankWidth = value;
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000F68 RID: 3944 RVA: 0x0002A0FC File Offset: 0x000282FC
		// (set) Token: 0x06000F69 RID: 3945 RVA: 0x0002A12C File Offset: 0x0002832C
		public float FlankWidth
		{
			get
			{
				return (float)(this.FileCount - 1) * (this.owner.Interval + this.owner.UnitDiameter) + this.owner.UnitDiameter;
			}
			set
			{
				int num = MathF.Max(0, (int)((value - this.owner.UnitDiameter) / (this.owner.Interval + this.owner.UnitDiameter) + 1E-05f)) + 1;
				num = MathF.Max(num, 1);
				this.SetColumnCount(num);
				Action onWidthChanged = this.OnWidthChanged;
				if (onWidthChanged == null)
				{
					return;
				}
				onWidthChanged();
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000F6A RID: 3946 RVA: 0x0002A18D File Offset: 0x0002838D
		// (set) Token: 0x06000F6B RID: 3947 RVA: 0x0002A195 File Offset: 0x00028395
		public List<Vec2> UnitPositionsOnVanguardFileIndex { get; private set; }

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000F6C RID: 3948 RVA: 0x0002A19E File Offset: 0x0002839E
		public float Depth
		{
			get
			{
				return this.RankDepth;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000F6D RID: 3949 RVA: 0x0002A1A6 File Offset: 0x000283A6
		public float RankDepth
		{
			get
			{
				return (float)(this.RankCount - 1) * (this.Distance + this.owner.UnitDiameter) + this.owner.UnitDiameter;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000F6E RID: 3950 RVA: 0x0002A1D0 File Offset: 0x000283D0
		public float MinimumWidth
		{
			get
			{
				return this.MinimumFlankWidth;
			}
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x0002A1D8 File Offset: 0x000283D8
		public IFormationUnit GetPlayerUnit()
		{
			return this._allUnits.FirstOrDefault<IFormationUnit>((IFormationUnit unit) => unit.IsPlayerUnit);
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000F70 RID: 3952 RVA: 0x0002A204 File Offset: 0x00028404
		public float MaximumWidth
		{
			get
			{
				return (float)(this.UnitCount - 1) * (this.owner.UnitDiameter + this.owner.Interval) + this.owner.UnitDiameter;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000F71 RID: 3953 RVA: 0x0002A234 File Offset: 0x00028434
		public float MinimumFlankWidth
		{
			get
			{
				return (float)(MathF.Max(1, MathF.Ceiling(MathF.Sqrt((float)(this.UnitCount / ColumnFormation.ArrangementAspectRatio)))) - 1) * (this.owner.UnitDiameter + this.owner.Interval) + this.owner.UnitDiameter;
			}
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x0002A285 File Offset: 0x00028485
		public MBReadOnlyList<IFormationUnit> GetAllUnits()
		{
			return this._allUnits;
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x0002A28D File Offset: 0x0002848D
		public void GetAllUnits(in MBList<IFormationUnit> allUnitsListToBeFilledIn)
		{
			allUnitsListToBeFilledIn.Clear();
			allUnitsListToBeFilledIn.AddRange(this._allUnits);
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x0002A2A3 File Offset: 0x000284A3
		public MBList<IFormationUnit> GetUnpositionedUnits()
		{
			return null;
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000F75 RID: 3957 RVA: 0x0002A2A6 File Offset: 0x000284A6
		public bool? IsLoose
		{
			get
			{
				return new bool?(false);
			}
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0002A2B0 File Offset: 0x000284B0
		private bool IsUnitPositionAvailable(int fileIndex, int rankIndex)
		{
			if (this.IsMiddleFrontUnitPositionReserved)
			{
				ValueTuple<int, int> middleFrontUnitPosition = this.GetMiddleFrontUnitPosition();
				if (fileIndex == middleFrontUnitPosition.Item1 && rankIndex == middleFrontUnitPosition.Item2)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0002A2E4 File Offset: 0x000284E4
		private bool GetNextVacancy(out int fileIndex, out int rankIndex)
		{
			if (this.RankCount == 0)
			{
				fileIndex = -1;
				rankIndex = -1;
				return false;
			}
			rankIndex = this.RankCount - 1;
			for (int i = 0; i < this.ColumnCount; i++)
			{
				int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(i, this.isExpandingFromRightSide ^ (this.ColumnCount % 2 == 1));
				fileIndex = this.VanguardFileIndex + columnOffsetFromColumnIndex;
				if (this._units2D[fileIndex, rankIndex] == null && this.IsUnitPositionAvailable(fileIndex, rankIndex))
				{
					return true;
				}
			}
			fileIndex = -1;
			rankIndex = -1;
			return false;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x0002A368 File Offset: 0x00028568
		private IFormationUnit GetLastUnit()
		{
			if (this.RankCount == 0)
			{
				return null;
			}
			int num = this.RankCount - 1;
			for (int i = this.ColumnCount - 1; i >= 0; i--)
			{
				int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(i, this.isExpandingFromRightSide);
				int num2 = this.VanguardFileIndex + columnOffsetFromColumnIndex;
				IFormationUnit formationUnit = this._units2D[num2, num];
				if (formationUnit != null)
				{
					return formationUnit;
				}
			}
			return null;
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0002A3C8 File Offset: 0x000285C8
		private void Deepen()
		{
			ColumnFormation.Deepen(this);
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0002A3D0 File Offset: 0x000285D0
		private void ReconstructUnitsFromUnits2D()
		{
			if (this._allUnits == null)
			{
				this._allUnits = new MBList<IFormationUnit>();
			}
			this._allUnits.Clear();
			for (int i = 0; i < this._units2D.Count1; i++)
			{
				for (int j = 0; j < this._units2D.Count2; j++)
				{
					if (this._units2D[i, j] != null)
					{
						this._allUnits.Add(this._units2D[i, j]);
					}
				}
			}
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0002A450 File Offset: 0x00028650
		private static void Deepen(ColumnFormation formation)
		{
			formation._units2DWorkspace.ResetWithNewCount(formation.FileCount, formation.RankCount + 1);
			for (int i = 0; i < formation.FileCount; i++)
			{
				formation._units2D.CopyRowTo(i, 0, formation._units2DWorkspace, i, 0, formation.RankCount);
			}
			MBList2D<IFormationUnit> units2D = formation._units2D;
			formation._units2D = formation._units2DWorkspace;
			formation._units2DWorkspace = units2D;
			formation.ReconstructUnitsFromUnits2D();
			Action onShapeChanged = formation.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0002A4D2 File Offset: 0x000286D2
		private void Shorten()
		{
			ColumnFormation.Shorten(this);
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x0002A4DC File Offset: 0x000286DC
		private static void Shorten(ColumnFormation formation)
		{
			formation._units2DWorkspace.ResetWithNewCount(formation.FileCount, formation.RankCount - 1);
			for (int i = 0; i < formation.FileCount; i++)
			{
				formation._units2D.CopyRowTo(i, 0, formation._units2DWorkspace, i, 0, formation.RankCount - 1);
			}
			MBList2D<IFormationUnit> units2D = formation._units2D;
			formation._units2D = formation._units2DWorkspace;
			formation._units2DWorkspace = units2D;
			formation.ReconstructUnitsFromUnits2D();
			Action onShapeChanged = formation.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x0002A560 File Offset: 0x00028760
		public bool AddUnit(IFormationUnit unit)
		{
			int num = 0;
			bool flag = false;
			while (!flag && num < 100)
			{
				num++;
				if (num > 10)
				{
					Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Formation\\ColumnFormation.cs", "AddUnit", 382);
				}
				int num2;
				int num3;
				if (this.GetNextVacancy(out num2, out num3))
				{
					unit.FormationFileIndex = num2;
					unit.FormationRankIndex = num3;
					this._units2D[num2, num3] = unit;
					this.ReconstructUnitsFromUnits2D();
					flag = true;
				}
				else
				{
					this.Deepen();
				}
			}
			if (flag)
			{
				int num4;
				IFormationUnit unitToFollow = this.GetUnitToFollow(unit, out num4);
				this.SetUnitToFollow(unit, unitToFollow, num4);
				Action onShapeChanged = this.OnShapeChanged;
				if (onShapeChanged != null)
				{
					onShapeChanged();
				}
			}
			return flag;
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x0002A602 File Offset: 0x00028802
		private IFormationUnit TryGetUnit(int fileIndex, int rankIndex)
		{
			if (fileIndex >= 0 && fileIndex < this.FileCount && rankIndex >= 0 && rankIndex < this.RankCount)
			{
				return this._units2D[fileIndex, rankIndex];
			}
			return null;
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x0002A630 File Offset: 0x00028830
		private void AdjustFollowDataOfUnitPosition(int fileIndex, int rankIndex)
		{
			IFormationUnit formationUnit = this._units2D[fileIndex, rankIndex];
			if (fileIndex == this.VanguardFileIndex)
			{
				if (formationUnit != null)
				{
					IFormationUnit formationUnit2 = this.TryGetUnit(fileIndex, rankIndex - 1);
					this.SetUnitToFollow(formationUnit, formationUnit2 ?? this.Vanguard, 0);
				}
				for (int i = 1; i < this.ColumnCount; i++)
				{
					int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(i, this.isExpandingFromRightSide);
					IFormationUnit formationUnit3 = this._units2D[fileIndex + columnOffsetFromColumnIndex, rankIndex];
					if (formationUnit3 != null)
					{
						this.SetUnitToFollow(formationUnit3, formationUnit ?? this.Vanguard, columnOffsetFromColumnIndex);
					}
				}
				IFormationUnit formationUnit4 = this.TryGetUnit(fileIndex, rankIndex + 1);
				if (formationUnit4 != null)
				{
					this.SetUnitToFollow(formationUnit4, formationUnit ?? this.Vanguard, 0);
					return;
				}
			}
			else if (formationUnit != null)
			{
				IFormationUnit formationUnit5 = this._units2D[this.VanguardFileIndex, rankIndex];
				int columnOffsetFromColumnIndex2 = ColumnFormation.GetColumnOffsetFromColumnIndex(fileIndex, this.isExpandingFromRightSide);
				this.SetUnitToFollow(formationUnit, formationUnit5 ?? this.Vanguard, columnOffsetFromColumnIndex2);
			}
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x0002A720 File Offset: 0x00028920
		private void ShiftUnitsForward(int fileIndex, int rankIndex)
		{
			for (;;)
			{
				IFormationUnit formationUnit = this.TryGetUnit(fileIndex, rankIndex + 1);
				if (formationUnit == null)
				{
					break;
				}
				IFormationUnit formationUnit2 = formationUnit;
				int formationRankIndex = formationUnit2.FormationRankIndex;
				formationUnit2.FormationRankIndex = formationRankIndex - 1;
				this._units2D[fileIndex, rankIndex] = formationUnit;
				this._units2D[fileIndex, rankIndex + 1] = null;
				this.ReconstructUnitsFromUnits2D();
				this.AdjustFollowDataOfUnitPosition(fileIndex, rankIndex);
				rankIndex++;
			}
			int num = 0;
			if (rankIndex == this.RankCount - 1)
			{
				for (int i = 0; i < this.ColumnCount; i++)
				{
					int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(i, this.isExpandingFromRightSide);
					if (this.VanguardFileIndex + columnOffsetFromColumnIndex == fileIndex)
					{
						num = i + 1;
					}
				}
			}
			IFormationUnit formationUnit3 = null;
			for (int j = this.ColumnCount - 1; j >= num; j--)
			{
				int columnOffsetFromColumnIndex2 = ColumnFormation.GetColumnOffsetFromColumnIndex(j, this.isExpandingFromRightSide);
				int num2 = this.VanguardFileIndex + columnOffsetFromColumnIndex2;
				formationUnit3 = this._units2D[num2, this.RankCount - 1];
				if (formationUnit3 != null)
				{
					break;
				}
			}
			if (formationUnit3 != null)
			{
				this._units2D[formationUnit3.FormationFileIndex, formationUnit3.FormationRankIndex] = null;
				formationUnit3.FormationFileIndex = fileIndex;
				formationUnit3.FormationRankIndex = rankIndex;
				this._units2D[fileIndex, rankIndex] = formationUnit3;
				this.ReconstructUnitsFromUnits2D();
				this.AdjustFollowDataOfUnitPosition(fileIndex, rankIndex);
			}
			if (this.IsLastRankEmpty())
			{
				this.Shorten();
			}
			Action onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x0002A870 File Offset: 0x00028A70
		private void ShiftUnitsBackwardForMakingRoomForVanguard(int fileIndex, int rankIndex)
		{
			if (this.RankCount == 1)
			{
				bool flag = false;
				int num = -1;
				for (int i = 0; i < this.ColumnCount; i++)
				{
					int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(i, this.isExpandingFromRightSide);
					if (this._units2D[this.VanguardFileIndex + columnOffsetFromColumnIndex, 0] == null)
					{
						flag = true;
						num = this.VanguardFileIndex + columnOffsetFromColumnIndex;
						break;
					}
				}
				if (flag)
				{
					IFormationUnit formationUnit = this._units2D[fileIndex, rankIndex];
					this._units2D[fileIndex, rankIndex] = null;
					this._units2D[num, 0] = formationUnit;
					this.ReconstructUnitsFromUnits2D();
					formationUnit.FormationFileIndex = num;
					formationUnit.FormationRankIndex = 0;
					return;
				}
				ColumnFormation.Deepen(this);
				IFormationUnit formationUnit2 = this._units2D[fileIndex, rankIndex];
				this._units2D[fileIndex, rankIndex] = null;
				this._units2D[fileIndex, rankIndex + 1] = formationUnit2;
				this.ReconstructUnitsFromUnits2D();
				IFormationUnit formationUnit3 = formationUnit2;
				int num2 = formationUnit3.FormationRankIndex;
				formationUnit3.FormationRankIndex = num2 + 1;
				return;
			}
			else
			{
				int num3 = rankIndex;
				IFormationUnit formationUnit4 = null;
				for (rankIndex = this.RankCount - 1; rankIndex >= num3; rankIndex--)
				{
					IFormationUnit formationUnit5 = this._units2D[fileIndex, rankIndex];
					this.TryGetUnit(fileIndex, rankIndex + 1);
					this._units2D[fileIndex, rankIndex] = null;
					if (rankIndex + 1 < this.RankCount)
					{
						IFormationUnit formationUnit6 = formationUnit5;
						int num2 = formationUnit6.FormationRankIndex;
						formationUnit6.FormationRankIndex = num2 + 1;
						this._units2D[fileIndex, rankIndex + 1] = formationUnit5;
					}
					else
					{
						formationUnit4 = formationUnit5;
						if (formationUnit4 != null)
						{
							formationUnit4.FormationFileIndex = -1;
							formationUnit4.FormationRankIndex = -1;
						}
					}
					this.ReconstructUnitsFromUnits2D();
				}
				for (rankIndex = this.RankCount - 1; rankIndex >= num3; rankIndex--)
				{
					this.AdjustFollowDataOfUnitPosition(fileIndex, rankIndex);
				}
				if (formationUnit4 != null)
				{
					this.AddUnit(formationUnit4);
				}
				Action onShapeChanged = this.OnShapeChanged;
				if (onShapeChanged == null)
				{
					return;
				}
				onShapeChanged();
				return;
			}
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x0002AA34 File Offset: 0x00028C34
		private bool IsLastRankEmpty()
		{
			if (this.RankCount == 0)
			{
				return false;
			}
			for (int i = 0; i < this.FileCount; i++)
			{
				if (this._units2D[i, this.RankCount - 1] != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0002AA78 File Offset: 0x00028C78
		public void RemoveUnit(IFormationUnit unit)
		{
			int formationFileIndex = unit.FormationFileIndex;
			int formationRankIndex = unit.FormationRankIndex;
			if (GameNetwork.IsServer)
			{
				MBDebug.Print(string.Concat(new object[] { "Removing unit at ", formationFileIndex, " ", formationRankIndex, " from column arrangement\nFileCount&RankCount: ", this.FileCount, " ", this.RankCount }), 0, Debug.DebugColor.White, 17592186044416UL);
			}
			this._units2D[unit.FormationFileIndex, unit.FormationRankIndex] = null;
			this.ReconstructUnitsFromUnits2D();
			this.ShiftUnitsForward(unit.FormationFileIndex, unit.FormationRankIndex);
			if (this.IsLastRankEmpty())
			{
				this.Shorten();
			}
			unit.FormationFileIndex = -1;
			unit.FormationRankIndex = -1;
			this.SetUnitToFollow(unit, null, 0);
			Action onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged != null)
			{
				onShapeChanged();
			}
			if (this.Vanguard == unit && !((Agent)unit).IsActive())
			{
				this._vanguard = null;
				if (this.FileCount > 0 && this.RankCount > 0)
				{
					this.AdjustFollowDataOfUnitPosition(formationFileIndex, formationRankIndex);
				}
			}
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0002ABA1 File Offset: 0x00028DA1
		public IFormationUnit GetUnit(int fileIndex, int rankIndex)
		{
			return this._units2D[fileIndex, rankIndex];
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x0002ABB0 File Offset: 0x00028DB0
		public void OnBatchRemoveStart()
		{
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x0002ABB2 File Offset: 0x00028DB2
		public void OnBatchRemoveEnd()
		{
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0002ABB4 File Offset: 0x00028DB4
		[Conditional("DEBUG")]
		private void AssertUnitPositions()
		{
			for (int i = 0; i < this.FileCount; i++)
			{
				for (int j = 0; j < this.RankCount; j++)
				{
					IFormationUnit formationUnit = this._units2D[i, j];
				}
			}
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x0002ABF4 File Offset: 0x00028DF4
		[Conditional("DEBUG")]
		private void AssertUnit(IFormationUnit unit, bool isAssertingFollowed = true)
		{
			if (unit == null)
			{
				return;
			}
			if (isAssertingFollowed)
			{
				int num;
				this.GetUnitToFollow(unit, out num);
			}
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0002AC14 File Offset: 0x00028E14
		private static int GetColumnOffsetFromColumnIndex(int columnIndex, bool isExpandingFromRightSide)
		{
			int num;
			if (isExpandingFromRightSide)
			{
				num = (columnIndex + 1) / 2 * ((columnIndex % 2 == 0) ? (-1) : 1);
			}
			else
			{
				num = (columnIndex + 1) / 2 * ((columnIndex % 2 == 0) ? 1 : (-1));
			}
			return num;
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x0002AC48 File Offset: 0x00028E48
		private IFormationUnit GetUnitToFollow(IFormationUnit unit, out int columnOffset)
		{
			IFormationUnit formationUnit;
			if (unit.FormationFileIndex == this.VanguardFileIndex)
			{
				columnOffset = 0;
				if (unit.FormationRankIndex > 0)
				{
					formationUnit = this._units2D[unit.FormationFileIndex, unit.FormationRankIndex - 1];
				}
				else
				{
					formationUnit = null;
				}
			}
			else
			{
				columnOffset = unit.FormationFileIndex - this.VanguardFileIndex;
				formationUnit = this._units2D[this.VanguardFileIndex, unit.FormationRankIndex];
			}
			if (formationUnit == null)
			{
				formationUnit = this.Vanguard;
			}
			return formationUnit;
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0002ACC1 File Offset: 0x00028EC1
		private IEnumerable<ValueTuple<int, int>> GetOrderedUnitPositionIndices()
		{
			int num2;
			for (int rankIndex = 0; rankIndex < this.RankCount; rankIndex = num2 + 1)
			{
				for (int columnIndex = 0; columnIndex < this.ColumnCount; columnIndex = num2 + 1)
				{
					int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(columnIndex, this.isExpandingFromRightSide);
					int num = this.VanguardFileIndex + columnOffsetFromColumnIndex;
					yield return new ValueTuple<int, int>(num, rankIndex);
					num2 = columnIndex;
				}
				num2 = rankIndex;
			}
			yield break;
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x0002ACD4 File Offset: 0x00028ED4
		private Vec2 GetLocalPositionOfUnit(int fileIndex, int rankIndex)
		{
			if (this.UnitPositionsOnVanguardFileIndex == null)
			{
				this.UnitPositionsOnVanguardFileIndex = this.GetUnitPositionsOnVanguardFileIndex();
			}
			Vec2 orderPosition = (this.owner as Formation).OrderPosition;
			List<Vec2> unitPositionsOnVanguardFileIndex = this.UnitPositionsOnVanguardFileIndex;
			unitPositionsOnVanguardFileIndex.Insert(0, orderPosition);
			float num = this.Distance + this.owner.UnitDiameter;
			int i = rankIndex;
			int num2 = 1;
			Vec2 vec = unitPositionsOnVanguardFileIndex[0];
			Vec2 vec2 = vec - unitPositionsOnVanguardFileIndex[num2];
			float num3 = vec2.Normalize();
			while (i > 0)
			{
				if (num3 >= num)
				{
					vec += -vec2 * num;
					num3 -= num;
				}
				else
				{
					float num4 = num - num3;
					vec += -vec2 * num3;
					if (++num2 < unitPositionsOnVanguardFileIndex.Count)
					{
						vec2 = vec - unitPositionsOnVanguardFileIndex[num2];
					}
					num3 = vec2.Normalize();
					vec += -vec2 * num4;
					num3 -= num4;
				}
				i--;
			}
			float num5 = (float)(this.FileCount - 1) * (this.Interval + this.owner.UnitDiameter);
			Vec2 vec3 = -vec2.TransformToParentUnitF(new Vec2((float)fileIndex * (this.Interval + this.owner.UnitDiameter) - num5 / 2f, 0f));
			vec += vec3;
			Vec2 vec4 = (this.owner as Formation).Direction.TransformToLocalUnitF(vec - unitPositionsOnVanguardFileIndex[0]);
			unitPositionsOnVanguardFileIndex.RemoveAt(0);
			return vec4;
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x0002AE74 File Offset: 0x00029074
		private Vec2 GetLocalDirectionOfUnit(int fileIndex, int rankIndex)
		{
			return Vec2.Forward;
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0002AE7C File Offset: 0x0002907C
		private WorldPosition? GetWorldPositionOfUnit(int fileIndex, int rankIndex)
		{
			return null;
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x0002AE94 File Offset: 0x00029094
		public Vec2? GetLocalPositionOfUnitOrDefault(int unitIndex)
		{
			ValueTuple<int, int> valueTuple = this.GetOrderedUnitPositionIndices().ElementAtOrValue(unitIndex, new ValueTuple<int, int>(-1, -1));
			Vec2? vec;
			if (valueTuple.Item1 != -1 && valueTuple.Item2 != -1)
			{
				int item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				vec = new Vec2?(this.GetLocalPositionOfUnit(item, item2));
			}
			else
			{
				vec = null;
			}
			return vec;
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x0002AEF0 File Offset: 0x000290F0
		public Vec2? GetLocalDirectionOfUnitOrDefault(int unitIndex)
		{
			ValueTuple<int, int> valueTuple = (from i in this.GetOrderedUnitPositionIndices()
				where this.IsUnitPositionAvailable(i.Item1, i.Item2)
				select i).ElementAtOrValue(unitIndex, new ValueTuple<int, int>(-1, -1));
			Vec2? vec;
			if (valueTuple.Item1 != -1 && valueTuple.Item2 != -1)
			{
				int item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				vec = new Vec2?(this.GetLocalDirectionOfUnit(item, item2));
			}
			else
			{
				vec = null;
			}
			return vec;
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0002AF5C File Offset: 0x0002915C
		public WorldPosition? GetWorldPositionOfUnitOrDefault(int unitIndex)
		{
			ValueTuple<int, int> valueTuple = (from i in this.GetOrderedUnitPositionIndices()
				where this.IsUnitPositionAvailable(i.Item1, i.Item2)
				select i).ElementAtOrValue(unitIndex, new ValueTuple<int, int>(-1, -1));
			WorldPosition? worldPosition;
			if (valueTuple.Item1 != -1 && valueTuple.Item2 != -1)
			{
				int item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				worldPosition = this.GetWorldPositionOfUnit(item, item2);
			}
			else
			{
				worldPosition = null;
			}
			return worldPosition;
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0002AFC2 File Offset: 0x000291C2
		public Vec2? GetLocalPositionOfUnitOrDefault(IFormationUnit unit)
		{
			return new Vec2?(this.GetLocalPositionOfUnit(unit.FormationFileIndex, unit.FormationRankIndex));
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x0002AFDB File Offset: 0x000291DB
		public Vec2? GetLocalPositionOfUnitOrDefaultWithAdjustment(IFormationUnit unit, float distanceBetweenAgentsAdjustment)
		{
			return new Vec2?(this.GetLocalPositionOfUnit(unit.FormationFileIndex, unit.FormationRankIndex));
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0002AFF4 File Offset: 0x000291F4
		public WorldPosition? GetWorldPositionOfUnitOrDefault(IFormationUnit unit)
		{
			return this.GetWorldPositionOfUnit(unit.FormationFileIndex, unit.FormationRankIndex);
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0002B008 File Offset: 0x00029208
		public Vec2? GetLocalDirectionOfUnitOrDefault(IFormationUnit unit)
		{
			return new Vec2?(this.GetLocalDirectionOfUnit(unit.FormationFileIndex, unit.FormationRankIndex));
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0002B024 File Offset: 0x00029224
		public List<IFormationUnit> GetUnitsToPop(int count)
		{
			List<IFormationUnit> list = new List<IFormationUnit>();
			for (int i = this.RankCount - 1; i >= 0; i--)
			{
				for (int j = this.ColumnCount - 1; j >= 0; j--)
				{
					int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(j, this.isExpandingFromRightSide);
					int num = this.VanguardFileIndex + columnOffsetFromColumnIndex;
					IFormationUnit formationUnit = this._units2D[num, i];
					if (formationUnit != null)
					{
						list.Add(formationUnit);
						count--;
						if (count == 0)
						{
							return list;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0002B09B File Offset: 0x0002929B
		public List<IFormationUnit> GetUnitsToPop(int count, Vec3 targetPosition)
		{
			return this.GetUnitsToPop(count);
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x0002B0A4 File Offset: 0x000292A4
		public IEnumerable<IFormationUnit> GetUnitsToPopWithCondition(int count, Func<IFormationUnit, bool> currentCondition)
		{
			int num2;
			for (int rankIndex = this.RankCount - 1; rankIndex >= 0; rankIndex = num2 - 1)
			{
				for (int columnIndex = this.ColumnCount - 1; columnIndex >= 0; columnIndex = num2 - 1)
				{
					int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(columnIndex, this.isExpandingFromRightSide);
					int num = this.VanguardFileIndex + columnOffsetFromColumnIndex;
					IFormationUnit formationUnit = this._units2D[num, rankIndex];
					if (formationUnit != null && currentCondition(formationUnit))
					{
						yield return formationUnit;
						num2 = count;
						count = num2 - 1;
						if (count == 0)
						{
							yield break;
						}
					}
					num2 = columnIndex;
				}
				num2 = rankIndex;
			}
			yield break;
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0002B0C2 File Offset: 0x000292C2
		public void SwitchUnitLocations(IFormationUnit firstUnit, IFormationUnit secondUnit)
		{
			this.SwitchUnitLocationsAux(firstUnit, secondUnit);
			this.AdjustFollowDataOfUnitPosition(firstUnit.FormationFileIndex, firstUnit.FormationRankIndex);
			this.AdjustFollowDataOfUnitPosition(secondUnit.FormationFileIndex, secondUnit.FormationRankIndex);
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0002B0F0 File Offset: 0x000292F0
		private void SwitchUnitLocationsAux(IFormationUnit firstUnit, IFormationUnit secondUnit)
		{
			int formationFileIndex = firstUnit.FormationFileIndex;
			int formationRankIndex = firstUnit.FormationRankIndex;
			int formationFileIndex2 = secondUnit.FormationFileIndex;
			int formationRankIndex2 = secondUnit.FormationRankIndex;
			this._units2D[formationFileIndex, formationRankIndex] = secondUnit;
			this._units2D[formationFileIndex2, formationRankIndex2] = firstUnit;
			this.ReconstructUnitsFromUnits2D();
			firstUnit.FormationFileIndex = formationFileIndex2;
			firstUnit.FormationRankIndex = formationRankIndex2;
			secondUnit.FormationFileIndex = formationFileIndex;
			secondUnit.FormationRankIndex = formationRankIndex;
			Action onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x0002B167 File Offset: 0x00029367
		public void SwitchUnitLocationsWithUnpositionedUnit(IFormationUnit firstUnit, IFormationUnit secondUnit)
		{
			Debug.FailedAssert("Column formation should NOT have an unpositioned unit", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Formation\\ColumnFormation.cs", "SwitchUnitLocationsWithUnpositionedUnit", 1215);
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x0002B184 File Offset: 0x00029384
		public void SwitchUnitLocationsWithBackMostUnit(IFormationUnit unit)
		{
			Agent agent;
			if (this.Vanguard == null || (agent = this.Vanguard as Agent) == null || agent != unit)
			{
				IFormationUnit lastUnit = this.GetLastUnit();
				if (lastUnit != null && unit != null && unit != lastUnit)
				{
					this.SwitchUnitLocations(unit, lastUnit);
				}
			}
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0002B1C5 File Offset: 0x000293C5
		public float GetUnitsDistanceToFrontLine(IFormationUnit unit)
		{
			return -1f;
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0002B1CC File Offset: 0x000293CC
		public Vec2? GetLocalDirectionOfRelativeFormationLocation(IFormationUnit unit)
		{
			return null;
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x0002B1E4 File Offset: 0x000293E4
		public Vec2? GetLocalWallDirectionOfRelativeFormationLocation(IFormationUnit unit)
		{
			return null;
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0002B1FA File Offset: 0x000293FA
		public IEnumerable<Vec2> GetUnavailableUnitPositions()
		{
			yield break;
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0002B203 File Offset: 0x00029403
		public float GetOccupationWidth(int unitCount)
		{
			return this.FlankWidth;
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0002B20C File Offset: 0x0002940C
		public Vec2? CreateNewPosition(int unitIndex)
		{
			int num = MathF.Ceiling((float)unitIndex * 1f / (float)this.ColumnCount) + ((unitIndex % this.ColumnCount == 0) ? 1 : 0);
			if (num > this.RankCount)
			{
				this._units2D.ResetWithNewCount(this.ColumnCount, num);
				this.ReconstructUnitsFromUnits2D();
			}
			Vec2? localPositionOfUnitOrDefault = this.GetLocalPositionOfUnitOrDefault(unitIndex);
			Action onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return localPositionOfUnitOrDefault;
			}
			onShapeChanged();
			return localPositionOfUnitOrDefault;
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0002B276 File Offset: 0x00029476
		public void InvalidateCacheOfUnitAux(Vec2 roundedLocalPosition)
		{
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0002B278 File Offset: 0x00029478
		public void BeforeFormationFrameChange()
		{
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x0002B27A File Offset: 0x0002947A
		public void OnFormationFrameChanged(bool updateCachedOrderedLocalPositions = false)
		{
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0002B27C File Offset: 0x0002947C
		private Vec2 CalculateArrangementOrientation()
		{
			IFormationUnit formationUnit = this.Vanguard ?? this._units2D[this.GetMiddleFrontUnitPosition().Item1, this.GetMiddleFrontUnitPosition().Item2];
			if (formationUnit is Agent && this.owner is Formation)
			{
				return ((formationUnit as Agent).Position.AsVec2 - ((Formation)this.owner).CachedMedianPosition.AsVec2).Normalized();
			}
			Debug.FailedAssert("Unexpected case", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Formation\\ColumnFormation.cs", "CalculateArrangementOrientation", 1300);
			return this.GetLocalDirectionOfUnit(formationUnit.FormationFileIndex, formationUnit.FormationRankIndex);
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0002B32E File Offset: 0x0002952E
		public void OnUnitLostMount(IFormationUnit unit)
		{
			this.RemoveUnit(unit);
			this.AddUnit(unit);
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0002B340 File Offset: 0x00029540
		public bool IsTurnBackwardsNecessary(Vec2 previousPosition, WorldPosition? newPosition, Vec2 previousDirection, bool hasNewDirection, Vec2? newDirection)
		{
			return newPosition != null && this.UnitCount > 0 && this.RankCount > 0 && (newPosition.Value.AsVec2 - previousPosition).LengthSquared >= this.RankDepth * this.RankDepth && MathF.Abs(MBMath.GetSmallestDifferenceBetweenTwoAngles(this.CalculateArrangementOrientation().RotationInRadians, (newPosition.Value.AsVec2 - (this.owner as Formation).CachedMedianPosition.AsVec2).Normalized().RotationInRadians)) >= 2.3561945f;
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x0002B400 File Offset: 0x00029600
		public void TurnBackwards()
		{
			if (!this.IsMiddleFrontUnitPositionReserved && !this._isMiddleFrontUnitPositionUsedByVanguardInFormation && this.RankCount > 1)
			{
				bool isMiddleFrontUnitPositionReserved = this.IsMiddleFrontUnitPositionReserved;
				IFormationUnit vanguard = this._vanguard;
				if (isMiddleFrontUnitPositionReserved)
				{
					this.ReleaseMiddleFrontUnitPosition();
				}
				int rankCount = this.RankCount;
				for (int i = 0; i < rankCount / 2; i++)
				{
					for (int j = 0; j < this.FileCount; j++)
					{
						IFormationUnit formationUnit = this._units2D[j, i];
						int num = rankCount - i - 1;
						int num2 = this.FileCount - j - 1;
						IFormationUnit formationUnit2 = this._units2D[num2, num];
						if (formationUnit2 == null)
						{
							this._units2D[num2, num] = formationUnit;
							this._units2D[j, i] = null;
							if (formationUnit != null)
							{
								formationUnit.FormationFileIndex = num2;
								formationUnit.FormationRankIndex = num;
							}
						}
						else if (formationUnit != null && formationUnit != formationUnit2)
						{
							this.SwitchUnitLocationsAux(formationUnit, formationUnit2);
						}
					}
				}
				for (int k = 0; k < this.FileCount; k++)
				{
					if (this._units2D[k, 0] == null && this._units2D[k, 1] != null)
					{
						for (int l = 1; l < rankCount; l++)
						{
							IFormationUnit formationUnit3 = this._units2D[k, l];
							IFormationUnit formationUnit4 = formationUnit3;
							int formationRankIndex = formationUnit4.FormationRankIndex;
							formationUnit4.FormationRankIndex = formationRankIndex - 1;
							this._units2D[k, l - 1] = formationUnit3;
							this._units2D[k, l] = null;
						}
					}
				}
				this.isExpandingFromRightSide = !this.isExpandingFromRightSide;
				this.ReconstructUnitsFromUnits2D();
				foreach (IFormationUnit formationUnit5 in this.GetAllUnits())
				{
					int num3;
					IFormationUnit unitToFollow = this.GetUnitToFollow(formationUnit5, out num3);
					this.SetUnitToFollow(formationUnit5, unitToFollow, num3);
				}
				Action onShapeChanged = this.OnShapeChanged;
				if (onShapeChanged != null)
				{
					onShapeChanged();
				}
				if (isMiddleFrontUnitPositionReserved)
				{
					this.ReserveMiddleFrontUnitPosition(vanguard);
				}
				Action onShapeChanged2 = this.OnShapeChanged;
				if (onShapeChanged2 == null)
				{
					return;
				}
				onShapeChanged2();
			}
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x0002B62C File Offset: 0x0002982C
		public void OnFormationDispersed()
		{
			foreach (IFormationUnit formationUnit in this.GetAllUnits().ToArray())
			{
				this.SwitchUnitIfLeftBehind(formationUnit);
			}
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x0002B65E File Offset: 0x0002985E
		public void Reset()
		{
			this._units2D.ResetWithNewCount(this.ColumnCount, 1);
			this.ReconstructUnitsFromUnits2D();
			Action onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged();
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000FAD RID: 4013 RVA: 0x0002B688 File Offset: 0x00029888
		// (remove) Token: 0x06000FAE RID: 4014 RVA: 0x0002B6C0 File Offset: 0x000298C0
		public event Action OnWidthChanged;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000FAF RID: 4015 RVA: 0x0002B6F8 File Offset: 0x000298F8
		// (remove) Token: 0x06000FB0 RID: 4016 RVA: 0x0002B730 File Offset: 0x00029930
		public event Action OnShapeChanged;

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0002B768 File Offset: 0x00029968
		public virtual void RearrangeFrom(IFormationArrangement arrangement)
		{
			if (arrangement is TransposedLineFormation)
			{
				this.FlankWidth = arrangement.FlankWidth;
				return;
			}
			if (arrangement is LineFormation)
			{
				this.FlankWidth = (float)MathF.Max(0, MathF.Ceiling(MathF.Sqrt((float)(arrangement.UnitCount / ColumnFormation.ArrangementAspectRatio))) - 1) * (this.owner.UnitDiameter + this.Interval) + this.owner.UnitDiameter;
			}
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0002B7D7 File Offset: 0x000299D7
		public virtual void RearrangeTo(IFormationArrangement arrangement)
		{
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0002B7DC File Offset: 0x000299DC
		public virtual void RearrangeTransferUnits(IFormationArrangement arrangement)
		{
			foreach (ValueTuple<int, int> valueTuple in this.GetOrderedUnitPositionIndices().ToList<ValueTuple<int, int>>())
			{
				IFormationUnit formationUnit = this._units2D[valueTuple.Item1, valueTuple.Item2];
				if (formationUnit != null)
				{
					formationUnit.FormationFileIndex = -1;
					formationUnit.FormationRankIndex = -1;
					this.SetUnitToFollow(formationUnit, null, 0);
					arrangement.AddUnit(formationUnit);
				}
			}
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0002B868 File Offset: 0x00029A68
		private void SetVanguard(IFormationUnit vanguard)
		{
			if (this.Vanguard != null || vanguard != null)
			{
				bool flag = false;
				bool flag2 = false;
				if (this.UnitCount > 0)
				{
					if (this.Vanguard == null && vanguard != null)
					{
						flag2 = true;
					}
					else if (this.Vanguard != null && vanguard == null)
					{
						flag = true;
					}
				}
				ValueTuple<int, int> middleFrontUnitPosition = this.GetMiddleFrontUnitPosition();
				if (flag)
				{
					Agent agent = this.Vanguard as Agent;
					if (((agent != null) ? agent.Formation : null) == this.owner)
					{
						this.RemoveUnit(this.Vanguard);
						this.AddUnit(this.Vanguard);
					}
					else if (this.RankCount > 0)
					{
						this.ShiftUnitsForward(middleFrontUnitPosition.Item1, middleFrontUnitPosition.Item2);
					}
				}
				else if (flag2)
				{
					Agent agent2 = vanguard as Agent;
					if (((agent2 != null) ? agent2.Formation : null) == this.owner)
					{
						this.RemoveUnit(vanguard);
						this.ShiftUnitsBackwardForMakingRoomForVanguard(middleFrontUnitPosition.Item1, middleFrontUnitPosition.Item2);
						if (this.RankCount > 0)
						{
							this._units2D[middleFrontUnitPosition.Item1, middleFrontUnitPosition.Item2] = vanguard;
							this.ReconstructUnitsFromUnits2D();
							vanguard.FormationFileIndex = middleFrontUnitPosition.Item1;
							vanguard.FormationRankIndex = middleFrontUnitPosition.Item2;
							if (this.RankCount == 2)
							{
								this.AdjustFollowDataOfUnitPosition(middleFrontUnitPosition.Item1, middleFrontUnitPosition.Item2);
								this.AdjustFollowDataOfUnitPosition(middleFrontUnitPosition.Item1, middleFrontUnitPosition.Item2 + 1);
								Action onShapeChanged = this.OnShapeChanged;
								if (onShapeChanged != null)
								{
									onShapeChanged();
								}
							}
						}
						else
						{
							this.AddUnit(vanguard);
						}
					}
					else
					{
						this.ShiftUnitsBackwardForMakingRoomForVanguard(middleFrontUnitPosition.Item1, middleFrontUnitPosition.Item2);
					}
				}
				this._vanguard = vanguard;
				if (this.RankCount > 0)
				{
					this.AdjustFollowDataOfUnitPosition(middleFrontUnitPosition.Item1, middleFrontUnitPosition.Item2);
				}
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x0002BA11 File Offset: 0x00029C11
		public int UnitCount
		{
			get
			{
				return this.GetAllUnits().Count;
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000FB6 RID: 4022 RVA: 0x0002BA1E File Offset: 0x00029C1E
		public int PositionedUnitCount
		{
			get
			{
				return this.UnitCount;
			}
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0002BA28 File Offset: 0x00029C28
		protected int GetUnitCountWithOverride()
		{
			int num;
			if (this.owner.OverridenUnitCount != null)
			{
				num = this.owner.OverridenUnitCount.Value;
			}
			else
			{
				num = this.UnitCount;
			}
			return num;
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0002BA68 File Offset: 0x00029C68
		private void SetColumnCount(int columnCount)
		{
			if (this.ColumnCount != columnCount)
			{
				IFormationUnit[] array = this.GetAllUnits().ToArray();
				this._units2D.ResetWithNewCount(columnCount, 1);
				this.ReconstructUnitsFromUnits2D();
				foreach (IFormationUnit formationUnit in array)
				{
					formationUnit.FormationFileIndex = -1;
					formationUnit.FormationRankIndex = -1;
					this.AddUnit(formationUnit);
				}
				Action onShapeChanged = this.OnShapeChanged;
				if (onShapeChanged == null)
				{
					return;
				}
				onShapeChanged();
			}
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0002BAD5 File Offset: 0x00029CD5
		public void FormFromWidth(float width)
		{
			this.ColumnCount = MathF.Ceiling(width);
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0002BAE4 File Offset: 0x00029CE4
		public IFormationUnit GetNeighborUnitOfLeftSide(IFormationUnit unit)
		{
			int formationRankIndex = unit.FormationRankIndex;
			for (int i = unit.FormationFileIndex - 1; i >= 0; i--)
			{
				if (this._units2D[i, formationRankIndex] != null)
				{
					return this._units2D[i, formationRankIndex];
				}
			}
			return null;
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0002BB2C File Offset: 0x00029D2C
		public IFormationUnit GetNeighborUnitOfRightSide(IFormationUnit unit)
		{
			int formationRankIndex = unit.FormationRankIndex;
			for (int i = unit.FormationFileIndex + 1; i < this.FileCount; i++)
			{
				if (this._units2D[i, formationRankIndex] != null)
				{
					return this._units2D[i, formationRankIndex];
				}
			}
			return null;
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0002BB76 File Offset: 0x00029D76
		public void ReserveMiddleFrontUnitPosition(IFormationUnit vanguard)
		{
			Agent agent = vanguard as Agent;
			if (((agent != null) ? agent.Formation : null) != this.owner)
			{
				this.IsMiddleFrontUnitPositionReserved = true;
			}
			else
			{
				this._isMiddleFrontUnitPositionUsedByVanguardInFormation = true;
			}
			this.Vanguard = vanguard;
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0002BBA9 File Offset: 0x00029DA9
		public void ReleaseMiddleFrontUnitPosition()
		{
			this.IsMiddleFrontUnitPositionReserved = false;
			this.Vanguard = null;
			this._isMiddleFrontUnitPositionUsedByVanguardInFormation = false;
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0002BBC0 File Offset: 0x00029DC0
		private ValueTuple<int, int> GetMiddleFrontUnitPosition()
		{
			return new ValueTuple<int, int>(this.VanguardFileIndex, 0);
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0002BBCE File Offset: 0x00029DCE
		public Vec2 GetLocalPositionOfReservedUnitPosition()
		{
			return Vec2.Zero;
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0002BBD8 File Offset: 0x00029DD8
		public void OnTickOccasionallyOfUnit(IFormationUnit unit, bool arrangementChangeAllowed)
		{
			if (arrangementChangeAllowed && unit.FollowedUnit != this._vanguard && unit.FollowedUnit is Agent && !((Agent)unit.FollowedUnit).IsAIControlled && unit.FollowedUnit.FormationFileIndex >= 0 && unit.FollowedUnit.FormationRankIndex >= 0)
			{
				IFormationUnit followedUnit = unit.FollowedUnit;
				this.RemoveUnit(unit.FollowedUnit);
				this.AddUnit(followedUnit);
			}
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0002BC4C File Offset: 0x00029E4C
		public void OnTickOccasionally()
		{
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x0002BC50 File Offset: 0x00029E50
		private MBList<IFormationUnit> GetUnitsBehind(IFormationUnit unit)
		{
			MBList<IFormationUnit> mblist = new MBList<IFormationUnit>();
			bool flag = false;
			for (int i = 0; i < this.ColumnCount; i++)
			{
				int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(i, this.isExpandingFromRightSide);
				int num = this.VanguardFileIndex + columnOffsetFromColumnIndex;
				if (num == unit.FormationFileIndex)
				{
					flag = true;
				}
				if (flag && this._units2D[num, unit.FormationRankIndex] != null)
				{
					mblist.Add(this._units2D[num, unit.FormationRankIndex]);
				}
			}
			for (int j = 0; j < this.FileCount; j++)
			{
				for (int k = unit.FormationRankIndex + 1; k < this.RankCount; k++)
				{
					if (this._units2D[j, k] != null)
					{
						mblist.Add(this._units2D[j, k]);
					}
				}
			}
			return mblist;
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0002BD24 File Offset: 0x00029F24
		private void SwitchUnitIfLeftBehind(IFormationUnit unit)
		{
			int num;
			IFormationUnit unitToFollow = this.GetUnitToFollow(unit, out num);
			if (unitToFollow == null)
			{
				float num2 = this.owner.UnitDiameter * 2f;
				IFormationUnit formationUnit = this.owner.GetClosestUnitTo(Vec2.Zero, new MBList<IFormationUnit> { unit }, new float?(num2));
				if (formationUnit == null)
				{
					formationUnit = this.owner.GetClosestUnitTo(Vec2.Zero, this.GetUnitsAtRanks(0, this.RankCount - 1), null);
				}
				if (formationUnit != null && formationUnit != unit && formationUnit is Agent && (formationUnit as Agent).IsAIControlled)
				{
					this.SwitchUnitLocations(unit, formationUnit);
					return;
				}
			}
			else
			{
				float num3 = this.GetFollowVector(num).Length * 1.5f;
				IFormationUnit formationUnit2 = this.owner.GetClosestUnitTo(unitToFollow, new MBList<IFormationUnit> { unit }, new float?(num3));
				if (formationUnit2 == null)
				{
					formationUnit2 = this.owner.GetClosestUnitTo(unitToFollow, this.GetUnitsBehind(unit), null);
				}
				Agent agent;
				if (formationUnit2 != null && formationUnit2 != unit && (agent = formationUnit2 as Agent) != null && agent.IsAIControlled)
				{
					this.SwitchUnitLocations(unit, agent);
				}
			}
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0002BE58 File Offset: 0x0002A058
		private void SetUnitToFollow(IFormationUnit unit, IFormationUnit unitToFollow, int columnOffset = 0)
		{
			Vec2 followVector = this.GetFollowVector(columnOffset);
			this.owner.SetUnitToFollow(unit, unitToFollow, followVector);
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0002BE7C File Offset: 0x0002A07C
		private Vec2 GetFollowVector(int columnOffset)
		{
			Vec2 vec;
			if (columnOffset == 0)
			{
				vec = -Vec2.Forward * (this.Distance + this.owner.UnitDiameter);
			}
			else
			{
				vec = Vec2.Side * (float)columnOffset * (this.owner.UnitDiameter + this.Interval);
			}
			return vec;
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x0002BED5 File Offset: 0x0002A0D5
		public float GetDirectionChangeTendencyOfUnit(IFormationUnit unit)
		{
			if (this.RankCount == 1 || unit.FormationRankIndex == -1)
			{
				return 0f;
			}
			return (float)unit.FormationRankIndex * 1f / (float)(this.RankCount - 1);
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0002BF08 File Offset: 0x0002A108
		private MBList<IFormationUnit> GetUnitsAtRanks(int rankIndex1, int rankIndex2)
		{
			MBList<IFormationUnit> mblist = new MBList<IFormationUnit>();
			for (int i = 0; i < this.ColumnCount; i++)
			{
				int columnOffsetFromColumnIndex = ColumnFormation.GetColumnOffsetFromColumnIndex(i, this.isExpandingFromRightSide);
				int num = this.VanguardFileIndex + columnOffsetFromColumnIndex;
				if (this._units2D[num, rankIndex1] != null)
				{
					mblist.Add(this._units2D[num, rankIndex1]);
				}
			}
			for (int j = 0; j < this.ColumnCount; j++)
			{
				int columnOffsetFromColumnIndex2 = ColumnFormation.GetColumnOffsetFromColumnIndex(j, this.isExpandingFromRightSide);
				int num2 = this.VanguardFileIndex + columnOffsetFromColumnIndex2;
				if (this._units2D[num2, rankIndex2] != null)
				{
					mblist.Add(this._units2D[num2, rankIndex2]);
				}
			}
			return mblist;
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0002BFB8 File Offset: 0x0002A1B8
		public IEnumerable<T> GetUnitsAtVanguardFile<T>() where T : IFormationUnit
		{
			int fileIndex = this.VanguardFileIndex;
			int num;
			for (int rankIndex = 0; rankIndex < this.RankCount; rankIndex = num + 1)
			{
				if (rankIndex == 0 && this.Vanguard != null)
				{
					yield return (T)((object)this.Vanguard);
				}
				if (this._units2D[fileIndex, rankIndex] != null)
				{
					yield return (T)((object)this._units2D[fileIndex, rankIndex]);
				}
				num = rankIndex;
			}
			yield break;
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0002BFC8 File Offset: 0x0002A1C8
		public void UpdateLocalPositionErrors(bool recalculateErrors)
		{
		}

		// Token: 0x1700038D RID: 909
		// (set) Token: 0x06000FCA RID: 4042 RVA: 0x0002BFCA File Offset: 0x0002A1CA
		bool IFormationArrangement.AreLocalPositionsDirty
		{
			set
			{
			}
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x0002BFCC File Offset: 0x0002A1CC
		public List<Vec2> GetUnitPositionsOnVanguardFileIndex()
		{
			IEnumerable<Agent> unitsAtVanguardFile = this.GetUnitsAtVanguardFile<Agent>();
			List<Vec2> list = new List<Vec2>(unitsAtVanguardFile.Count<Agent>());
			foreach (Agent agent in unitsAtVanguardFile)
			{
				list.Add(agent.Position.AsVec2);
			}
			return list;
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0002C03C File Offset: 0x0002A23C
		void IFormationArrangement.GetAllUnits(in MBList<IFormationUnit> allUnitsListToBeFilledIn)
		{
			this.GetAllUnits(in allUnitsListToBeFilledIn);
		}

		// Token: 0x040003C5 RID: 965
		public static readonly int ArrangementAspectRatio = 5;

		// Token: 0x040003C6 RID: 966
		private readonly IFormation owner;

		// Token: 0x040003C7 RID: 967
		private IFormationUnit _vanguard;

		// Token: 0x040003C8 RID: 968
		private MBList2D<IFormationUnit> _units2D;

		// Token: 0x040003C9 RID: 969
		private MBList2D<IFormationUnit> _units2DWorkspace;

		// Token: 0x040003CA RID: 970
		private MBList<IFormationUnit> _allUnits;

		// Token: 0x040003CB RID: 971
		private bool isExpandingFromRightSide = true;

		// Token: 0x040003CC RID: 972
		private bool IsMiddleFrontUnitPositionReserved;

		// Token: 0x040003CD RID: 973
		private bool _isMiddleFrontUnitPositionUsedByVanguardInFormation;
	}
}
