using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000154 RID: 340
	public struct FormOrder
	{
		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x060011D7 RID: 4567 RVA: 0x00036E1F File Offset: 0x0003501F
		// (set) Token: 0x060011D8 RID: 4568 RVA: 0x00036E27 File Offset: 0x00035027
		public float CustomFlankWidth
		{
			get
			{
				return this._customFlankWidth;
			}
			set
			{
				this._customFlankWidth = value;
			}
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x00036E30 File Offset: 0x00035030
		private FormOrder(FormOrder.FormOrderEnum orderEnum, float customFlankWidth = -1f)
		{
			this.OrderEnum = orderEnum;
			this._customFlankWidth = customFlankWidth;
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x00036E40 File Offset: 0x00035040
		public static FormOrder FormOrderCustom(float customWidth)
		{
			return new FormOrder(FormOrder.FormOrderEnum.Custom, customWidth);
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x060011DB RID: 4571 RVA: 0x00036E4C File Offset: 0x0003504C
		public OrderType OrderType
		{
			get
			{
				switch (this.OrderEnum)
				{
				case FormOrder.FormOrderEnum.Wide:
					return OrderType.FormWide;
				case FormOrder.FormOrderEnum.Wider:
					return OrderType.FormWider;
				case FormOrder.FormOrderEnum.Custom:
					return OrderType.FormCustom;
				default:
					return OrderType.FormDeep;
				}
			}
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x00036E81 File Offset: 0x00035081
		public void OnApply(Formation formation)
		{
			this.OnApplyToArrangement(formation, formation.Arrangement);
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x00036E90 File Offset: 0x00035090
		public static int GetUnitCountOf(Formation formation)
		{
			if (formation.OverridenUnitCount == null)
			{
				return formation.CountOfUnitsWithoutDetachedOnes;
			}
			return formation.OverridenUnitCount.Value;
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x00036EC2 File Offset: 0x000350C2
		public bool OnApplyToCustomArrangement(Formation formation, IFormationArrangement arrangement)
		{
			return false;
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00036EC8 File Offset: 0x000350C8
		private void OnApplyToArrangement(Formation formation, IFormationArrangement arrangement)
		{
			if (!this.OnApplyToCustomArrangement(formation, arrangement))
			{
				if (arrangement is ColumnFormation)
				{
					ColumnFormation columnFormation = arrangement as ColumnFormation;
					if (FormOrder.GetUnitCountOf(formation) > 0)
					{
						columnFormation.FormFromWidth((float)this.GetRankVerticalFormFileCount(formation));
					}
					if (this.OrderEnum == FormOrder.FormOrderEnum.Custom && MathF.Abs(this.CustomFlankWidth - arrangement.FlankWidth) > 0.01f)
					{
						ArrangementOrder.TransposeLineFormation(formation);
						formation.OnTick += formation.TickForColumnArrangementInitialPositioning;
						return;
					}
				}
				else
				{
					if (arrangement is RectilinearSchiltronFormation)
					{
						(arrangement as RectilinearSchiltronFormation).Form();
						return;
					}
					if (arrangement is CircularSchiltronFormation)
					{
						(arrangement as CircularSchiltronFormation).Form();
						return;
					}
					if (arrangement is CircularFormation)
					{
						CircularFormation circularFormation = arrangement as CircularFormation;
						int unitCountOf = FormOrder.GetUnitCountOf(formation);
						int? maxFileCount = this.GetMaxFileCount(unitCountOf);
						float num2;
						if (maxFileCount != null)
						{
							int num = MathF.Max(1, MathF.Ceiling((float)unitCountOf * 1f / (float)maxFileCount.Value));
							num2 = circularFormation.GetCircumferenceFromRankCount(num);
						}
						else
						{
							num2 = 3.1415927f * this.CustomFlankWidth;
						}
						circularFormation.FormFromCircumference(num2);
						return;
					}
					if (arrangement is SquareFormation)
					{
						SquareFormation squareFormation = arrangement as SquareFormation;
						int unitCountOf2 = FormOrder.GetUnitCountOf(formation);
						int? maxFileCount2 = this.GetMaxFileCount(unitCountOf2);
						if (maxFileCount2 != null)
						{
							int num3 = MathF.Max(1, MathF.Ceiling((float)unitCountOf2 * 1f / (float)maxFileCount2.Value));
							squareFormation.FormFromRankCount(num3);
							return;
						}
						squareFormation.FormFromBorderSideWidth(this.CustomFlankWidth);
						return;
					}
					else if (arrangement is SkeinFormation)
					{
						SkeinFormation skeinFormation = arrangement as SkeinFormation;
						int unitCountOf3 = FormOrder.GetUnitCountOf(formation);
						int? maxFileCount3 = this.GetMaxFileCount(unitCountOf3);
						if (maxFileCount3 != null)
						{
							skeinFormation.FormFromFlankWidth(maxFileCount3.Value, false);
							return;
						}
						skeinFormation.FlankWidth = this.CustomFlankWidth;
						return;
					}
					else if (arrangement is WedgeFormation)
					{
						WedgeFormation wedgeFormation = arrangement as WedgeFormation;
						int unitCountOf4 = FormOrder.GetUnitCountOf(formation);
						int? maxFileCount4 = this.GetMaxFileCount(unitCountOf4);
						if (maxFileCount4 != null)
						{
							wedgeFormation.FormFromFlankWidth(maxFileCount4.Value, false);
							return;
						}
						wedgeFormation.FlankWidth = this.CustomFlankWidth;
						return;
					}
					else if (arrangement is TransposedLineFormation)
					{
						TransposedLineFormation transposedLineFormation = arrangement as TransposedLineFormation;
						int unitCountOf5 = FormOrder.GetUnitCountOf(formation);
						if (unitCountOf5 > 0)
						{
							int? maxFileCount5 = this.GetMaxFileCount(unitCountOf5);
							if (maxFileCount5 == null)
							{
								maxFileCount5 = new int?(transposedLineFormation.GetFileCountFromWidth(this.CustomFlankWidth));
							}
							MathF.Ceiling((float)unitCountOf5 * 1f / (float)maxFileCount5.Value);
							transposedLineFormation.FormFromFlankWidth(this.GetRankVerticalFormFileCount(formation), false);
							return;
						}
					}
					else if (arrangement is LineFormation)
					{
						LineFormation lineFormation = arrangement as LineFormation;
						int unitCountOf6 = FormOrder.GetUnitCountOf(formation);
						int? maxFileCount6 = this.GetMaxFileCount(unitCountOf6);
						if (maxFileCount6 != null)
						{
							lineFormation.FormFromFlankWidth(maxFileCount6.Value, unitCountOf6 > 40);
							return;
						}
						lineFormation.FlankWidth = this.CustomFlankWidth;
						return;
					}
					else
					{
						Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Orders\\FormOrder.cs", "OnApplyToArrangement", 230);
					}
				}
			}
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x000371AA File Offset: 0x000353AA
		private int? GetMaxFileCount(int unitCount)
		{
			return FormOrder.GetMaxFileCountStatic(this.OrderEnum, unitCount);
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x000371B8 File Offset: 0x000353B8
		public static int? GetMaxFileCountStatic(FormOrder.FormOrderEnum order, int unitCount)
		{
			return FormOrder.GetMaxFileCountAux(order, unitCount);
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x000371C4 File Offset: 0x000353C4
		private int GetRankVerticalFormFileCount(IFormation formation)
		{
			int arrangementAspectRatio = ColumnFormation.ArrangementAspectRatio;
			int countOfUnitsWithoutLooseDetachedOnes = (formation as Formation).CountOfUnitsWithoutLooseDetachedOnes;
			switch (this.OrderEnum)
			{
			case FormOrder.FormOrderEnum.Deep:
				return MathF.Max(MathF.Round(MathF.Sqrt((float)countOfUnitsWithoutLooseDetachedOnes / ((float)arrangementAspectRatio * 2f))), 1);
			case FormOrder.FormOrderEnum.Wide:
				return MathF.Max(MathF.Round(MathF.Sqrt((float)countOfUnitsWithoutLooseDetachedOnes / ((float)arrangementAspectRatio * 1f))), 1);
			case FormOrder.FormOrderEnum.Wider:
				return MathF.Max(MathF.Round(MathF.Sqrt((float)countOfUnitsWithoutLooseDetachedOnes / ((float)arrangementAspectRatio * 0.5f))), 1);
			case FormOrder.FormOrderEnum.Custom:
				return MathF.Floor((this._customFlankWidth + formation.Interval) / (formation.UnitDiameter + formation.Interval));
			default:
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Orders\\FormOrder.cs", "GetRankVerticalFormFileCount", 274);
				return 1;
			}
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x00037294 File Offset: 0x00035494
		private static int? GetMaxFileCountAux(FormOrder.FormOrderEnum order, int unitCount)
		{
			if (order == FormOrder.FormOrderEnum.Custom)
			{
				return null;
			}
			int num = 0;
			switch (order)
			{
			case FormOrder.FormOrderEnum.Deep:
				num = MathF.Max(MathF.Round(MathF.Sqrt((float)unitCount / 4f)), 1) * 4;
				break;
			case FormOrder.FormOrderEnum.Wide:
				num = MathF.Max(MathF.Round(MathF.Sqrt((float)unitCount / 16f)), 1) * 16;
				break;
			case FormOrder.FormOrderEnum.Wider:
				num = MathF.Max(MathF.Round(MathF.Sqrt((float)unitCount / 64f)), 1) * 64;
				break;
			}
			return new int?(num);
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x00037324 File Offset: 0x00035524
		public override bool Equals(object obj)
		{
			if (obj is FormOrder)
			{
				FormOrder formOrder = (FormOrder)obj;
				return formOrder == this;
			}
			return false;
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x00037350 File Offset: 0x00035550
		public override int GetHashCode()
		{
			return (int)this.OrderEnum;
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x00037358 File Offset: 0x00035558
		public static bool operator !=(FormOrder f1, FormOrder f2)
		{
			return f1.OrderEnum != f2.OrderEnum;
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x0003736B File Offset: 0x0003556B
		public static bool operator ==(FormOrder f1, FormOrder f2)
		{
			return f1.OrderEnum == f2.OrderEnum;
		}

		// Token: 0x04000452 RID: 1106
		private float _customFlankWidth;

		// Token: 0x04000453 RID: 1107
		public readonly FormOrder.FormOrderEnum OrderEnum;

		// Token: 0x04000454 RID: 1108
		public static readonly FormOrder FormOrderDeep = new FormOrder(FormOrder.FormOrderEnum.Deep, -1f);

		// Token: 0x04000455 RID: 1109
		public static readonly FormOrder FormOrderWide = new FormOrder(FormOrder.FormOrderEnum.Wide, -1f);

		// Token: 0x04000456 RID: 1110
		public static readonly FormOrder FormOrderWider = new FormOrder(FormOrder.FormOrderEnum.Wider, -1f);

		// Token: 0x02000476 RID: 1142
		public enum FormOrderEnum
		{
			// Token: 0x04001A76 RID: 6774
			Deep,
			// Token: 0x04001A77 RID: 6775
			Wide,
			// Token: 0x04001A78 RID: 6776
			Wider,
			// Token: 0x04001A79 RID: 6777
			Custom
		}
	}
}
