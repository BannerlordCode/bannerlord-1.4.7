using System;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x02000017 RID: 23
	public class TwoWaySliderWidget : SliderWidget
	{
		// Token: 0x06000135 RID: 309 RVA: 0x0000726C File Offset: 0x0000546C
		public TwoWaySliderWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00007278 File Offset: 0x00005478
		protected override void OnValueIntChanged(int value)
		{
			base.OnValueIntChanged(value);
			if (this.ChangeFillWidget == null || base.MaxValueInt == 0)
			{
				return;
			}
			float num = base.Size.X / base._scaleToUse;
			float num2 = (float)this.BaseValueInt / base.MaxValueFloat * num;
			if (value < this.BaseValueInt)
			{
				this.ChangeFillWidget.SetState("Positive");
				this.ChangeFillWidget.SuggestedWidth = (float)(this.BaseValueInt - value) / base.MaxValueFloat * num;
				this.ChangeFillWidget.PositionXOffset = num2 - this.ChangeFillWidget.SuggestedWidth;
			}
			else if (value > this.BaseValueInt)
			{
				this.ChangeFillWidget.SetState("Negative");
				this.ChangeFillWidget.SuggestedWidth = (float)(value - this.BaseValueInt) / base.MaxValueFloat * num;
				this.ChangeFillWidget.PositionXOffset = num2;
			}
			else
			{
				this.ChangeFillWidget.SetState("Default");
				this.ChangeFillWidget.SuggestedWidth = 0f;
			}
			if (this._handleClicked || this._valueChangedByMouse || this._manuallyIncreased)
			{
				this._manuallyIncreased = false;
				base.OnPropertyChanged(base.ValueInt, "ValueInt");
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x000073A5 File Offset: 0x000055A5
		private void ChangeFillWidgetUpdated()
		{
			if (this.ChangeFillWidget != null)
			{
				this.ChangeFillWidget.AddState("Negative");
				this.ChangeFillWidget.AddState("Positive");
				this.ChangeFillWidget.HorizontalAlignment = HorizontalAlignment.Left;
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000073DB File Offset: 0x000055DB
		private void BaseValueIntUpdated()
		{
			this.OnValueIntChanged(base.ValueInt);
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000139 RID: 313 RVA: 0x000073E9 File Offset: 0x000055E9
		// (set) Token: 0x0600013A RID: 314 RVA: 0x000073F1 File Offset: 0x000055F1
		[Editor(false)]
		public BrushWidget ChangeFillWidget
		{
			get
			{
				return this._changeFillWidget;
			}
			set
			{
				if (this._changeFillWidget != value)
				{
					this._changeFillWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "ChangeFillWidget");
					this.ChangeFillWidgetUpdated();
				}
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00007415 File Offset: 0x00005615
		// (set) Token: 0x0600013C RID: 316 RVA: 0x0000741D File Offset: 0x0000561D
		[Editor(false)]
		public int BaseValueInt
		{
			get
			{
				return this._baseValueInt;
			}
			set
			{
				if (this._baseValueInt != value)
				{
					this._baseValueInt = value;
					base.OnPropertyChanged(value, "BaseValueInt");
					this.BaseValueIntUpdated();
				}
			}
		}

		// Token: 0x04000098 RID: 152
		protected bool _manuallyIncreased;

		// Token: 0x04000099 RID: 153
		private BrushWidget _changeFillWidget;

		// Token: 0x0400009A RID: 154
		private int _baseValueInt;
	}
}
