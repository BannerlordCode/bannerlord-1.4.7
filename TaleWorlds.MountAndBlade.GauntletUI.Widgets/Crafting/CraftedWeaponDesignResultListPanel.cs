using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x02000163 RID: 355
	public class CraftedWeaponDesignResultListPanel : ListPanel
	{
		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x060012B5 RID: 4789 RVA: 0x00033648 File Offset: 0x00031848
		// (set) Token: 0x060012B6 RID: 4790 RVA: 0x00033650 File Offset: 0x00031850
		public CounterTextBrushWidget ChangeValueTextWidget { get; set; }

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x060012B7 RID: 4791 RVA: 0x00033659 File Offset: 0x00031859
		// (set) Token: 0x060012B8 RID: 4792 RVA: 0x00033661 File Offset: 0x00031861
		public CounterTextBrushWidget ValueTextWidget { get; set; }

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x060012B9 RID: 4793 RVA: 0x0003366A File Offset: 0x0003186A
		// (set) Token: 0x060012BA RID: 4794 RVA: 0x00033672 File Offset: 0x00031872
		public RichTextWidget GoldEffectorTextWidget { get; set; }

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x060012BB RID: 4795 RVA: 0x0003367B File Offset: 0x0003187B
		// (set) Token: 0x060012BC RID: 4796 RVA: 0x00033683 File Offset: 0x00031883
		public Brush PositiveChangeBrush { get; set; }

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060012BD RID: 4797 RVA: 0x0003368C File Offset: 0x0003188C
		// (set) Token: 0x060012BE RID: 4798 RVA: 0x00033694 File Offset: 0x00031894
		public Brush NegativeChangeBrush { get; set; }

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060012BF RID: 4799 RVA: 0x0003369D File Offset: 0x0003189D
		// (set) Token: 0x060012C0 RID: 4800 RVA: 0x000336A5 File Offset: 0x000318A5
		public Brush NeutralBrush { get; set; }

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060012C1 RID: 4801 RVA: 0x000336AE File Offset: 0x000318AE
		// (set) Token: 0x060012C2 RID: 4802 RVA: 0x000336B6 File Offset: 0x000318B6
		public float FadeInTimeIndexOffset { get; set; } = 2f;

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060012C3 RID: 4803 RVA: 0x000336BF File Offset: 0x000318BF
		// (set) Token: 0x060012C4 RID: 4804 RVA: 0x000336C7 File Offset: 0x000318C7
		public float FadeInTime { get; set; } = 0.5f;

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060012C5 RID: 4805 RVA: 0x000336D0 File Offset: 0x000318D0
		// (set) Token: 0x060012C6 RID: 4806 RVA: 0x000336D8 File Offset: 0x000318D8
		public float CounterStartTime { get; set; } = 2f;

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x060012C7 RID: 4807 RVA: 0x000336E1 File Offset: 0x000318E1
		private bool _hasChange
		{
			get
			{
				return this.ChangeAmount != 0f;
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x060012C8 RID: 4808 RVA: 0x000336F3 File Offset: 0x000318F3
		private float _valueTextStartFadeInTime
		{
			get
			{
				return (float)base.GetSiblingIndex() * this.FadeInTimeIndexOffset;
			}
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00033703 File Offset: 0x00031903
		public CraftedWeaponDesignResultListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00033730 File Offset: 0x00031930
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.ValueTextWidget.FloatTarget = this.InitValue;
				this.ValueTextWidget.ForceSetValue(this.InitValue);
				if (this._hasChange)
				{
					this.ValueTextWidget.Brush = ((this.ChangeAmount > 0f) ? this.PositiveChangeBrush : this.NegativeChangeBrush);
					this.ChangeValueTextWidget.Brush = ((this.ChangeAmount > 0f) ? this.PositiveChangeBrush : this.NegativeChangeBrush);
					this.ChangeValueTextWidget.IsVisible = true;
				}
				else
				{
					this.ChangeValueTextWidget.IsVisible = false;
					this.ValueTextWidget.Brush = this.NeutralBrush;
				}
				this.ChangeValueTextWidget.SetGlobalAlphaRecursively(0f);
				this.ValueTextWidget.SetGlobalAlphaRecursively(0f);
				this.ChangeValueTextWidget.ShowSign = true;
				if (this.InitValue == 0f)
				{
					this.LabelTextWidget.SetState(this._isExceedingBeneficial ? "Bonus" : "Penalty");
				}
				this._initialized = true;
			}
			if (this._totalTime > this._valueTextStartFadeInTime)
			{
				float num = (this._totalTime - this._valueTextStartFadeInTime) / this.FadeInTime;
				if (num >= 0f && num <= 1f)
				{
					float num2 = MathF.Lerp(0f, 1f, num, 1E-05f);
					if (num2 < 1f)
					{
						this.ValueTextWidget.SetGlobalAlphaRecursively(num2);
					}
				}
				if (this._hasChange && this._totalTime > this._valueTextStartFadeInTime + this.CounterStartTime)
				{
					this.ValueTextWidget.FloatTarget = this.InitValue + this.ChangeAmount;
					num = (this._totalTime - this._valueTextStartFadeInTime - this.FadeInTime) / this.FadeInTime;
					if (num >= 0f && num <= 1f)
					{
						float num3 = MathF.Lerp(0f, 1f, num, 1E-05f);
						if (num3 < 1f)
						{
							this.ChangeValueTextWidget.SetGlobalAlphaRecursively(num3);
						}
					}
					this.ChangeValueTextWidget.FloatTarget = this.ChangeAmount;
				}
			}
			this._totalTime += dt;
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x060012CB RID: 4811 RVA: 0x0003395C File Offset: 0x00031B5C
		// (set) Token: 0x060012CC RID: 4812 RVA: 0x00033964 File Offset: 0x00031B64
		public RichTextWidget LabelTextWidget
		{
			get
			{
				return this._labelTextWidget;
			}
			set
			{
				if (this._labelTextWidget != value)
				{
					this._labelTextWidget = value;
				}
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x060012CD RID: 4813 RVA: 0x00033976 File Offset: 0x00031B76
		// (set) Token: 0x060012CE RID: 4814 RVA: 0x0003397E File Offset: 0x00031B7E
		public float InitValue
		{
			get
			{
				return this._initValue;
			}
			set
			{
				if (this._initValue != value)
				{
					this._initValue = value;
				}
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x060012CF RID: 4815 RVA: 0x00033990 File Offset: 0x00031B90
		// (set) Token: 0x060012D0 RID: 4816 RVA: 0x00033998 File Offset: 0x00031B98
		public float ChangeAmount
		{
			get
			{
				return this._changeAmount;
			}
			set
			{
				if (this._changeAmount != value)
				{
					this._changeAmount = value;
				}
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x060012D1 RID: 4817 RVA: 0x000339AA File Offset: 0x00031BAA
		// (set) Token: 0x060012D2 RID: 4818 RVA: 0x000339B2 File Offset: 0x00031BB2
		public bool IsExceedingBeneficial
		{
			get
			{
				return this._isExceedingBeneficial;
			}
			set
			{
				if (value != this._isExceedingBeneficial)
				{
					this._isExceedingBeneficial = value;
				}
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x060012D3 RID: 4819 RVA: 0x000339C4 File Offset: 0x00031BC4
		// (set) Token: 0x060012D4 RID: 4820 RVA: 0x000339CC File Offset: 0x00031BCC
		public float TargetValue
		{
			get
			{
				return this._targetValue;
			}
			set
			{
				if (value != this._targetValue)
				{
					this._targetValue = value;
				}
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060012D5 RID: 4821 RVA: 0x000339DE File Offset: 0x00031BDE
		// (set) Token: 0x060012D6 RID: 4822 RVA: 0x000339E6 File Offset: 0x00031BE6
		public bool IsOrderResult
		{
			get
			{
				return this._isOrderResult;
			}
			set
			{
				if (value != this._isOrderResult)
				{
					this._isOrderResult = value;
				}
			}
		}

		// Token: 0x04000885 RID: 2181
		private bool _initialized;

		// Token: 0x04000886 RID: 2182
		private float _totalTime;

		// Token: 0x04000887 RID: 2183
		private RichTextWidget _labelTextWidget;

		// Token: 0x04000888 RID: 2184
		private float _initValue;

		// Token: 0x04000889 RID: 2185
		private float _changeAmount;

		// Token: 0x0400088A RID: 2186
		private float _targetValue;

		// Token: 0x0400088B RID: 2187
		private bool _isExceedingBeneficial;

		// Token: 0x0400088C RID: 2188
		private bool _isOrderResult;
	}
}
