using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200001E RID: 30
	public class FloatComparedStateChangerTextWidget : TextWidget
	{
		// Token: 0x06000170 RID: 368 RVA: 0x00006115 File Offset: 0x00004315
		public FloatComparedStateChangerTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00006120 File Offset: 0x00004320
		private void UpdateState()
		{
			if (string.IsNullOrEmpty(this.TrueState) || string.IsNullOrEmpty(this.FalseState))
			{
				return;
			}
			bool flag = false;
			if (this.ComparisonType == FloatComparedStateChangerTextWidget.ComparisonTypes.Equals)
			{
				flag = this.FirstValue == this.SecondValue;
			}
			else if (this.ComparisonType == FloatComparedStateChangerTextWidget.ComparisonTypes.NotEquals)
			{
				flag = this.FirstValue != this.SecondValue;
			}
			else if (this.ComparisonType == FloatComparedStateChangerTextWidget.ComparisonTypes.LessThan)
			{
				flag = this.FirstValue < this.SecondValue;
			}
			else if (this.ComparisonType == FloatComparedStateChangerTextWidget.ComparisonTypes.GreaterThan)
			{
				flag = this.FirstValue > this.SecondValue;
			}
			else if (this.ComparisonType == FloatComparedStateChangerTextWidget.ComparisonTypes.GreaterThanOrEqual)
			{
				flag = this.FirstValue >= this.SecondValue;
			}
			else if (this.ComparisonType == FloatComparedStateChangerTextWidget.ComparisonTypes.LessThanOrEqual)
			{
				flag = this.FirstValue <= this.SecondValue;
			}
			this.SetState(flag ? this.TrueState : this.FalseState);
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00006206 File Offset: 0x00004406
		// (set) Token: 0x06000173 RID: 371 RVA: 0x0000620E File Offset: 0x0000440E
		public FloatComparedStateChangerTextWidget.ComparisonTypes ComparisonType
		{
			get
			{
				return this._comparisonType;
			}
			set
			{
				if (value != this._comparisonType)
				{
					this._comparisonType = value;
					this.UpdateState();
				}
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00006226 File Offset: 0x00004426
		// (set) Token: 0x06000175 RID: 373 RVA: 0x0000622E File Offset: 0x0000442E
		public float FirstValue
		{
			get
			{
				return this._firstValue;
			}
			set
			{
				if (value != this._firstValue)
				{
					this._firstValue = value;
					this.UpdateState();
				}
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00006246 File Offset: 0x00004446
		// (set) Token: 0x06000177 RID: 375 RVA: 0x0000624E File Offset: 0x0000444E
		public float SecondValue
		{
			get
			{
				return this._secondValue;
			}
			set
			{
				if (value != this._secondValue)
				{
					this._secondValue = value;
					this.UpdateState();
				}
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00006266 File Offset: 0x00004466
		// (set) Token: 0x06000179 RID: 377 RVA: 0x0000626E File Offset: 0x0000446E
		public string TrueState
		{
			get
			{
				return this._trueState;
			}
			set
			{
				if (value != this._trueState)
				{
					this._trueState = value;
					this.UpdateState();
				}
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600017A RID: 378 RVA: 0x0000628B File Offset: 0x0000448B
		// (set) Token: 0x0600017B RID: 379 RVA: 0x00006293 File Offset: 0x00004493
		public string FalseState
		{
			get
			{
				return this._falseState;
			}
			set
			{
				if (value != this._falseState)
				{
					this._falseState = value;
					this.UpdateState();
				}
			}
		}

		// Token: 0x040000AB RID: 171
		private FloatComparedStateChangerTextWidget.ComparisonTypes _comparisonType;

		// Token: 0x040000AC RID: 172
		private float _firstValue;

		// Token: 0x040000AD RID: 173
		private float _secondValue;

		// Token: 0x040000AE RID: 174
		private string _trueState;

		// Token: 0x040000AF RID: 175
		private string _falseState;

		// Token: 0x0200019B RID: 411
		public enum ComparisonTypes
		{
			// Token: 0x04000996 RID: 2454
			Equals,
			// Token: 0x04000997 RID: 2455
			NotEquals,
			// Token: 0x04000998 RID: 2456
			GreaterThan,
			// Token: 0x04000999 RID: 2457
			LessThan,
			// Token: 0x0400099A RID: 2458
			GreaterThanOrEqual,
			// Token: 0x0400099B RID: 2459
			LessThanOrEqual
		}
	}
}
