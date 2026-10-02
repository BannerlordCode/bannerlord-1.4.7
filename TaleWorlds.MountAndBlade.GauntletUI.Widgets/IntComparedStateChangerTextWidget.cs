using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000029 RID: 41
	public class IntComparedStateChangerTextWidget : TextWidget
	{
		// Token: 0x0600021E RID: 542 RVA: 0x00007C37 File Offset: 0x00005E37
		public IntComparedStateChangerTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00007C40 File Offset: 0x00005E40
		private void UpdateState()
		{
			if (string.IsNullOrEmpty(this.TrueState) || string.IsNullOrEmpty(this.FalseState))
			{
				return;
			}
			bool flag = false;
			if (this.ComparisonType == IntComparedStateChangerTextWidget.ComparisonTypes.Equals)
			{
				flag = this.FirstValue == this.SecondValue;
			}
			else if (this.ComparisonType == IntComparedStateChangerTextWidget.ComparisonTypes.NotEquals)
			{
				flag = this.FirstValue != this.SecondValue;
			}
			else if (this.ComparisonType == IntComparedStateChangerTextWidget.ComparisonTypes.LessThan)
			{
				flag = this.FirstValue < this.SecondValue;
			}
			else if (this.ComparisonType == IntComparedStateChangerTextWidget.ComparisonTypes.GreaterThan)
			{
				flag = this.FirstValue > this.SecondValue;
			}
			else if (this.ComparisonType == IntComparedStateChangerTextWidget.ComparisonTypes.GreaterThanOrEqual)
			{
				flag = this.FirstValue >= this.SecondValue;
			}
			else if (this.ComparisonType == IntComparedStateChangerTextWidget.ComparisonTypes.LessThanOrEqual)
			{
				flag = this.FirstValue <= this.SecondValue;
			}
			this.SetState(flag ? this.TrueState : this.FalseState);
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000220 RID: 544 RVA: 0x00007D26 File Offset: 0x00005F26
		// (set) Token: 0x06000221 RID: 545 RVA: 0x00007D2E File Offset: 0x00005F2E
		public IntComparedStateChangerTextWidget.ComparisonTypes ComparisonType
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

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00007D46 File Offset: 0x00005F46
		// (set) Token: 0x06000223 RID: 547 RVA: 0x00007D4E File Offset: 0x00005F4E
		public int FirstValue
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

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00007D66 File Offset: 0x00005F66
		// (set) Token: 0x06000225 RID: 549 RVA: 0x00007D6E File Offset: 0x00005F6E
		public int SecondValue
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

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000226 RID: 550 RVA: 0x00007D86 File Offset: 0x00005F86
		// (set) Token: 0x06000227 RID: 551 RVA: 0x00007D8E File Offset: 0x00005F8E
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

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000228 RID: 552 RVA: 0x00007DAB File Offset: 0x00005FAB
		// (set) Token: 0x06000229 RID: 553 RVA: 0x00007DB3 File Offset: 0x00005FB3
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

		// Token: 0x04000102 RID: 258
		private IntComparedStateChangerTextWidget.ComparisonTypes _comparisonType;

		// Token: 0x04000103 RID: 259
		private int _firstValue;

		// Token: 0x04000104 RID: 260
		private int _secondValue;

		// Token: 0x04000105 RID: 261
		private string _trueState;

		// Token: 0x04000106 RID: 262
		private string _falseState;

		// Token: 0x0200019D RID: 413
		public enum ComparisonTypes
		{
			// Token: 0x0400099F RID: 2463
			Equals,
			// Token: 0x040009A0 RID: 2464
			NotEquals,
			// Token: 0x040009A1 RID: 2465
			GreaterThan,
			// Token: 0x040009A2 RID: 2466
			LessThan,
			// Token: 0x040009A3 RID: 2467
			GreaterThanOrEqual,
			// Token: 0x040009A4 RID: 2468
			LessThanOrEqual
		}
	}
}
