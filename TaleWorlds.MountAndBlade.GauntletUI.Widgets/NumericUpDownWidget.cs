using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000033 RID: 51
	public class NumericUpDownWidget : Widget
	{
		// Token: 0x06000300 RID: 768 RVA: 0x0000982F File Offset: 0x00007A2F
		public NumericUpDownWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000984E File Offset: 0x00007A4E
		private void OnUpButtonClicked(Widget widget)
		{
			this.ChangeValue(1);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00009857 File Offset: 0x00007A57
		private void OnDownButtonClicked(Widget widget)
		{
			this.ChangeValue(-1);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00009860 File Offset: 0x00007A60
		private void ChangeValue(int changeAmount)
		{
			int num = this.IntValue + changeAmount;
			if ((float)num <= this.MaxValue && (float)num >= this.MinValue)
			{
				this.IntValue = num;
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00009894 File Offset: 0x00007A94
		private void UpdateControlButtonsEnabled()
		{
			if (this.UpButton != null)
			{
				this.UpButton.IsEnabled = (float)(this._intValue + 1) <= this.MaxValue;
			}
			if (this.DownButton != null)
			{
				this.DownButton.IsEnabled = (float)(this._intValue - 1) >= this.MinValue;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000305 RID: 773 RVA: 0x000098EF File Offset: 0x00007AEF
		// (set) Token: 0x06000306 RID: 774 RVA: 0x000098F7 File Offset: 0x00007AF7
		[Editor(false)]
		public bool ShowOneAdded
		{
			get
			{
				return this._showOneAdded;
			}
			set
			{
				if (this._showOneAdded != value)
				{
					this._showOneAdded = value;
					base.OnPropertyChanged(value, "ShowOneAdded");
				}
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000307 RID: 775 RVA: 0x00009915 File Offset: 0x00007B15
		// (set) Token: 0x06000308 RID: 776 RVA: 0x00009920 File Offset: 0x00007B20
		[Editor(false)]
		public int IntValue
		{
			get
			{
				return this._intValue;
			}
			set
			{
				if (this._intValue != value)
				{
					this._intValue = value;
					this.Value = (float)this._intValue;
					base.OnPropertyChanged(value, "IntValue");
					this._textWidget.IntText = (this.ShowOneAdded ? (this.IntValue + 1) : this.IntValue);
					this.UpdateControlButtonsEnabled();
				}
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000309 RID: 777 RVA: 0x0000997F File Offset: 0x00007B7F
		// (set) Token: 0x0600030A RID: 778 RVA: 0x00009987 File Offset: 0x00007B87
		[Editor(false)]
		public float Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (this._value != value)
				{
					this._value = value;
					this.IntValue = (int)this._value;
					base.OnPropertyChanged(value, "Value");
				}
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600030B RID: 779 RVA: 0x000099B2 File Offset: 0x00007BB2
		// (set) Token: 0x0600030C RID: 780 RVA: 0x000099BA File Offset: 0x00007BBA
		[Editor(false)]
		public float MinValue
		{
			get
			{
				return this._minValue;
			}
			set
			{
				if (value != this._minValue)
				{
					this._minValue = value;
					base.OnPropertyChanged(value, "MinValue");
					this.UpdateControlButtonsEnabled();
				}
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600030D RID: 781 RVA: 0x000099DE File Offset: 0x00007BDE
		// (set) Token: 0x0600030E RID: 782 RVA: 0x000099E6 File Offset: 0x00007BE6
		[Editor(false)]
		public float MaxValue
		{
			get
			{
				return this._maxValue;
			}
			set
			{
				if (value != this._maxValue)
				{
					this._maxValue = value;
					base.OnPropertyChanged(value, "MaxValue");
					this.UpdateControlButtonsEnabled();
				}
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00009A0A File Offset: 0x00007C0A
		// (set) Token: 0x06000310 RID: 784 RVA: 0x00009A12 File Offset: 0x00007C12
		[Editor(false)]
		public TextWidget TextWidget
		{
			get
			{
				return this._textWidget;
			}
			set
			{
				if (this._textWidget != value)
				{
					this._textWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "TextWidget");
				}
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00009A30 File Offset: 0x00007C30
		// (set) Token: 0x06000312 RID: 786 RVA: 0x00009A38 File Offset: 0x00007C38
		[Editor(false)]
		public ButtonWidget UpButton
		{
			get
			{
				return this._upButton;
			}
			set
			{
				if (this._upButton != value)
				{
					this._upButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "UpButton");
					if (value != null && !this._upButton.ClickEventHandlers.Contains(new Action<Widget>(this.OnUpButtonClicked)))
					{
						this._upButton.ClickEventHandlers.Add(new Action<Widget>(this.OnUpButtonClicked));
					}
				}
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00009A9E File Offset: 0x00007C9E
		// (set) Token: 0x06000314 RID: 788 RVA: 0x00009AA8 File Offset: 0x00007CA8
		[Editor(false)]
		public ButtonWidget DownButton
		{
			get
			{
				return this._downButton;
			}
			set
			{
				if (this._downButton != value)
				{
					this._downButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "DownButton");
					if (value != null && !this._downButton.ClickEventHandlers.Contains(new Action<Widget>(this.OnDownButtonClicked)))
					{
						this._downButton.ClickEventHandlers.Add(new Action<Widget>(this.OnDownButtonClicked));
					}
				}
			}
		}

		// Token: 0x04000138 RID: 312
		private bool _showOneAdded;

		// Token: 0x04000139 RID: 313
		private float _minValue;

		// Token: 0x0400013A RID: 314
		private float _maxValue;

		// Token: 0x0400013B RID: 315
		private int _intValue = int.MinValue;

		// Token: 0x0400013C RID: 316
		private float _value = float.MinValue;

		// Token: 0x0400013D RID: 317
		private TextWidget _textWidget;

		// Token: 0x0400013E RID: 318
		private ButtonWidget _upButton;

		// Token: 0x0400013F RID: 319
		private ButtonWidget _downButton;
	}
}
