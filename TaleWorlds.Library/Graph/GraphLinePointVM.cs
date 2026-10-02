using System;

namespace TaleWorlds.Library.Graph
{
	// Token: 0x020000B5 RID: 181
	public class GraphLinePointVM : ViewModel
	{
		// Token: 0x060006C6 RID: 1734 RVA: 0x00017049 File Offset: 0x00015249
		public GraphLinePointVM(float horizontalValue, float verticalValue)
		{
			this.HorizontalValue = horizontalValue;
			this.VerticalValue = verticalValue;
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x0001705F File Offset: 0x0001525F
		// (set) Token: 0x060006C8 RID: 1736 RVA: 0x00017067 File Offset: 0x00015267
		[DataSourceProperty]
		public float HorizontalValue
		{
			get
			{
				return this._horizontalValue;
			}
			set
			{
				if (value != this._horizontalValue)
				{
					this._horizontalValue = value;
					base.OnPropertyChangedWithValue(value, "HorizontalValue");
				}
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00017085 File Offset: 0x00015285
		// (set) Token: 0x060006CA RID: 1738 RVA: 0x0001708D File Offset: 0x0001528D
		[DataSourceProperty]
		public float VerticalValue
		{
			get
			{
				return this._verticalValue;
			}
			set
			{
				if (value != this._verticalValue)
				{
					this._verticalValue = value;
					base.OnPropertyChangedWithValue(value, "VerticalValue");
				}
			}
		}

		// Token: 0x0400020F RID: 527
		private float _horizontalValue;

		// Token: 0x04000210 RID: 528
		private float _verticalValue;
	}
}
