using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TaleWorlds.Library.Graph
{
	// Token: 0x020000B7 RID: 183
	public class GraphVM : ViewModel
	{
		// Token: 0x060006D2 RID: 1746 RVA: 0x00017148 File Offset: 0x00015348
		public GraphVM(string horizontalAxisLabel, string verticalAxisLabel)
		{
			this.Lines = new MBBindingList<GraphLineVM>();
			this.HorizontalAxisLabel = horizontalAxisLabel;
			this.VerticalAxisLabel = verticalAxisLabel;
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0001716C File Offset: 0x0001536C
		public void Draw([TupleElementNames(new string[] { "line", "points" })] IEnumerable<ValueTuple<GraphLineVM, IEnumerable<GraphLinePointVM>>> linesWithPoints, in Vec2 horizontalRange, in Vec2 verticalRange, float autoRangeHorizontalCoefficient = 1f, float autoRangeVerticalCoefficient = 1f, bool useAutoHorizontalRange = false, bool useAutoVerticalRange = false)
		{
			this.Lines.Clear();
			float num = float.MaxValue;
			float num2 = float.MinValue;
			float num3 = float.MaxValue;
			float num4 = float.MinValue;
			foreach (ValueTuple<GraphLineVM, IEnumerable<GraphLinePointVM>> valueTuple in linesWithPoints)
			{
				GraphLineVM item = valueTuple.Item1;
				foreach (GraphLinePointVM graphLinePointVM in valueTuple.Item2)
				{
					if (useAutoHorizontalRange)
					{
						if (graphLinePointVM.HorizontalValue < num)
						{
							num = graphLinePointVM.HorizontalValue;
						}
						if (graphLinePointVM.HorizontalValue > num2)
						{
							num2 = graphLinePointVM.HorizontalValue;
						}
					}
					if (useAutoVerticalRange)
					{
						if (graphLinePointVM.VerticalValue < num3)
						{
							num3 = graphLinePointVM.VerticalValue;
						}
						if (graphLinePointVM.VerticalValue > num4)
						{
							num4 = graphLinePointVM.VerticalValue;
						}
					}
					item.Points.Add(graphLinePointVM);
				}
				this.Lines.Add(item);
			}
			Vec2 vec = horizontalRange;
			float x = vec.X;
			vec = horizontalRange;
			float y = vec.Y;
			vec = verticalRange;
			float x2 = vec.X;
			vec = verticalRange;
			float y2 = vec.Y;
			bool flag = num != float.MaxValue && num2 != float.MinValue;
			bool flag2 = num3 != float.MaxValue && num4 != float.MinValue;
			if (useAutoHorizontalRange && flag)
			{
				GraphVM.ExtendRangeToNearestMultipleOfCoefficient(num, num2, autoRangeHorizontalCoefficient, out x, out y);
			}
			if (useAutoVerticalRange && flag2)
			{
				GraphVM.ExtendRangeToNearestMultipleOfCoefficient(num3, num4, autoRangeVerticalCoefficient, out x2, out y2);
			}
			this.HorizontalMinValue = x;
			this.HorizontalMaxValue = y;
			this.VerticalMinValue = x2;
			this.VerticalMaxValue = y2;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00017344 File Offset: 0x00015544
		private static void ExtendRangeToNearestMultipleOfCoefficient(float minValue, float maxValue, float coefficient, out float extendedMinValue, out float extendedMaxValue)
		{
			if (coefficient > 1E-05f)
			{
				extendedMinValue = (float)MathF.Floor(minValue / coefficient) * coefficient;
				extendedMaxValue = (float)MathF.Ceiling(maxValue / coefficient) * coefficient;
				if (extendedMinValue.ApproximatelyEqualsTo(extendedMaxValue, 1E-05f))
				{
					if (extendedMinValue - coefficient > 0f)
					{
						extendedMinValue -= coefficient;
						return;
					}
					extendedMaxValue += coefficient;
					return;
				}
			}
			else
			{
				extendedMinValue = minValue;
				extendedMaxValue = maxValue;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x000173A9 File Offset: 0x000155A9
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x000173B1 File Offset: 0x000155B1
		[DataSourceProperty]
		public MBBindingList<GraphLineVM> Lines
		{
			get
			{
				return this._lines;
			}
			set
			{
				if (value != this._lines)
				{
					this._lines = value;
					base.OnPropertyChangedWithValue<MBBindingList<GraphLineVM>>(value, "Lines");
				}
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x000173CF File Offset: 0x000155CF
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x000173D7 File Offset: 0x000155D7
		[DataSourceProperty]
		public string HorizontalAxisLabel
		{
			get
			{
				return this._horizontalAxisLabel;
			}
			set
			{
				if (value != this._horizontalAxisLabel)
				{
					this._horizontalAxisLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "HorizontalAxisLabel");
				}
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x000173FA File Offset: 0x000155FA
		// (set) Token: 0x060006DA RID: 1754 RVA: 0x00017402 File Offset: 0x00015602
		[DataSourceProperty]
		public string VerticalAxisLabel
		{
			get
			{
				return this._verticalAxisLabel;
			}
			set
			{
				if (value != this._verticalAxisLabel)
				{
					this._verticalAxisLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "VerticalAxisLabel");
				}
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x00017425 File Offset: 0x00015625
		// (set) Token: 0x060006DC RID: 1756 RVA: 0x0001742D File Offset: 0x0001562D
		[DataSourceProperty]
		public float HorizontalMinValue
		{
			get
			{
				return this._horizontalMinValue;
			}
			set
			{
				if (value != this._horizontalMinValue)
				{
					this._horizontalMinValue = value;
					base.OnPropertyChangedWithValue(value, "HorizontalMinValue");
				}
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x0001744B File Offset: 0x0001564B
		// (set) Token: 0x060006DE RID: 1758 RVA: 0x00017453 File Offset: 0x00015653
		[DataSourceProperty]
		public float HorizontalMaxValue
		{
			get
			{
				return this._horizontalMaxValue;
			}
			set
			{
				if (value != this._horizontalMaxValue)
				{
					this._horizontalMaxValue = value;
					base.OnPropertyChangedWithValue(value, "HorizontalMaxValue");
				}
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x00017471 File Offset: 0x00015671
		// (set) Token: 0x060006E0 RID: 1760 RVA: 0x00017479 File Offset: 0x00015679
		[DataSourceProperty]
		public float VerticalMinValue
		{
			get
			{
				return this._verticalMinValue;
			}
			set
			{
				if (value != this._verticalMinValue)
				{
					this._verticalMinValue = value;
					base.OnPropertyChangedWithValue(value, "VerticalMinValue");
				}
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x00017497 File Offset: 0x00015697
		// (set) Token: 0x060006E2 RID: 1762 RVA: 0x0001749F File Offset: 0x0001569F
		[DataSourceProperty]
		public float VerticalMaxValue
		{
			get
			{
				return this._verticalMaxValue;
			}
			set
			{
				if (value != this._verticalMaxValue)
				{
					this._verticalMaxValue = value;
					base.OnPropertyChangedWithValue(value, "VerticalMaxValue");
				}
			}
		}

		// Token: 0x04000214 RID: 532
		private MBBindingList<GraphLineVM> _lines;

		// Token: 0x04000215 RID: 533
		private string _horizontalAxisLabel;

		// Token: 0x04000216 RID: 534
		private string _verticalAxisLabel;

		// Token: 0x04000217 RID: 535
		private float _horizontalMinValue;

		// Token: 0x04000218 RID: 536
		private float _horizontalMaxValue;

		// Token: 0x04000219 RID: 537
		private float _verticalMinValue;

		// Token: 0x0400021A RID: 538
		private float _verticalMaxValue;
	}
}
