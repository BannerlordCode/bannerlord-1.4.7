using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x02000165 RID: 357
	public class CraftingItemStatSliderWidget : SliderWidget
	{
		// Token: 0x060012D9 RID: 4825 RVA: 0x00033A56 File Offset: 0x00031C56
		public CraftingItemStatSliderWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x00033A60 File Offset: 0x00031C60
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			float num = 1f;
			float x = base.SliderArea.Size.X;
			if (MathF.Abs(base.MaxValueFloat - base.MinValueFloat) > 1E-45f)
			{
				num = (base.ValueFloat - base.MinValueFloat) / (base.MaxValueFloat - base.MinValueFloat) * x;
				if (base.ReverseDirection)
				{
					num = 1f - num;
				}
			}
			if (this.HasValidTarget && this.TargetFill != null && base.Handle != null && this.ValueText != null)
			{
				float num2 = base.SliderArea.Size.X / base.MaxValueFloat * this.TargetValue;
				int num3 = MathF.Ceiling(MathF.Min(num, num2));
				int num4 = MathF.Floor(MathF.Max(num, num2));
				base.Filler.ScaledSuggestedWidth = (float)num3;
				this.TargetFill.ScaledPositionXOffset = (float)num3;
				this.TargetFill.ScaledSuggestedWidth = (float)(num4 - num3);
				base.Handle.ScaledPositionXOffset = num2 - base.Handle.Size.X / 2f;
				string text = ((this.IsExceedingBeneficial ? (base.ValueFloat >= this.TargetValue) : (base.ValueFloat <= this.TargetValue)) ? "Bonus" : "Penalty");
				this.TargetFill.SetState(text);
				this.ValueText.SetState(text);
				if (!this.HasValidValue)
				{
					this.LabelTextWidget.SetState(text);
					return;
				}
			}
			else
			{
				base.Filler.ScaledSuggestedWidth = num;
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x060012DB RID: 4827 RVA: 0x00033BFD File Offset: 0x00031DFD
		// (set) Token: 0x060012DC RID: 4828 RVA: 0x00033C05 File Offset: 0x00031E05
		[Editor(false)]
		public TextWidget ValueText
		{
			get
			{
				return this._valueText;
			}
			set
			{
				if (value != this._valueText)
				{
					this._valueText = value;
				}
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x060012DD RID: 4829 RVA: 0x00033C17 File Offset: 0x00031E17
		// (set) Token: 0x060012DE RID: 4830 RVA: 0x00033C1F File Offset: 0x00031E1F
		[Editor(false)]
		public TextWidget LabelTextWidget
		{
			get
			{
				return this._labelTextWidget;
			}
			set
			{
				if (value != this._labelTextWidget)
				{
					this._labelTextWidget = value;
				}
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x060012DF RID: 4831 RVA: 0x00033C31 File Offset: 0x00031E31
		// (set) Token: 0x060012E0 RID: 4832 RVA: 0x00033C39 File Offset: 0x00031E39
		[Editor(false)]
		public bool HasValidTarget
		{
			get
			{
				return this._hasValidTarget;
			}
			set
			{
				if (value != this._hasValidTarget)
				{
					this._hasValidTarget = value;
				}
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x060012E1 RID: 4833 RVA: 0x00033C4B File Offset: 0x00031E4B
		// (set) Token: 0x060012E2 RID: 4834 RVA: 0x00033C53 File Offset: 0x00031E53
		[Editor(false)]
		public bool HasValidValue
		{
			get
			{
				return this._hasValidValue;
			}
			set
			{
				if (value != this._hasValidValue)
				{
					this._hasValidValue = value;
				}
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x060012E3 RID: 4835 RVA: 0x00033C65 File Offset: 0x00031E65
		// (set) Token: 0x060012E4 RID: 4836 RVA: 0x00033C6D File Offset: 0x00031E6D
		[Editor(false)]
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

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x060012E5 RID: 4837 RVA: 0x00033C7F File Offset: 0x00031E7F
		// (set) Token: 0x060012E6 RID: 4838 RVA: 0x00033C87 File Offset: 0x00031E87
		[Editor(false)]
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

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x060012E7 RID: 4839 RVA: 0x00033C99 File Offset: 0x00031E99
		// (set) Token: 0x060012E8 RID: 4840 RVA: 0x00033CA1 File Offset: 0x00031EA1
		[Editor(false)]
		public BrushWidget TargetFill
		{
			get
			{
				return this._targetFill;
			}
			set
			{
				if (value != this._targetFill)
				{
					this._targetFill = value;
				}
			}
		}

		// Token: 0x0400088F RID: 2191
		private bool _hasValidTarget;

		// Token: 0x04000890 RID: 2192
		private bool _hasValidValue;

		// Token: 0x04000891 RID: 2193
		private bool _isExceedingBeneficial;

		// Token: 0x04000892 RID: 2194
		private float _targetValue;

		// Token: 0x04000893 RID: 2195
		private BrushWidget _targetFill;

		// Token: 0x04000894 RID: 2196
		private TextWidget _valueText;

		// Token: 0x04000895 RID: 2197
		private TextWidget _labelTextWidget;
	}
}
