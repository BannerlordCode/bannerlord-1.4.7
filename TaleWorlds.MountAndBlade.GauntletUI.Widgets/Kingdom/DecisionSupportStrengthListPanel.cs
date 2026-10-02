using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Kingdom
{
	// Token: 0x02000130 RID: 304
	public class DecisionSupportStrengthListPanel : ListPanel
	{
		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x0002B9BE File Offset: 0x00029BBE
		// (set) Token: 0x06000FD4 RID: 4052 RVA: 0x0002B9C6 File Offset: 0x00029BC6
		public bool IsAbstain { get; set; }

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06000FD5 RID: 4053 RVA: 0x0002B9CF File Offset: 0x00029BCF
		// (set) Token: 0x06000FD6 RID: 4054 RVA: 0x0002B9D7 File Offset: 0x00029BD7
		public bool IsPlayerSupporter { get; set; }

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x0002B9E0 File Offset: 0x00029BE0
		// (set) Token: 0x06000FD8 RID: 4056 RVA: 0x0002B9E8 File Offset: 0x00029BE8
		public bool IsOptionSelected { get; set; }

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x0002B9F1 File Offset: 0x00029BF1
		// (set) Token: 0x06000FDA RID: 4058 RVA: 0x0002B9F9 File Offset: 0x00029BF9
		public bool IsKingsOutcome { get; set; }

		// Token: 0x06000FDB RID: 4059 RVA: 0x0002BA02 File Offset: 0x00029C02
		public DecisionSupportStrengthListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0002BA0C File Offset: 0x00029C0C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			switch (this.CurrentIndex)
			{
			case 2:
				this.StrengthButton0.IsSelected = true;
				this.StrengthButton1.IsSelected = false;
				this.StrengthButton2.IsSelected = false;
				break;
			case 3:
				this.StrengthButton0.IsSelected = false;
				this.StrengthButton1.IsSelected = true;
				this.StrengthButton2.IsSelected = false;
				break;
			case 4:
				this.StrengthButton0.IsSelected = false;
				this.StrengthButton1.IsSelected = false;
				this.StrengthButton2.IsSelected = true;
				break;
			}
			base.GamepadNavigationIndex = (this.IsOptionSelected ? (-1) : 0);
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0002BABF File Offset: 0x00029CBF
		private void SetButtonsEnabled(bool isEnabled)
		{
			this.StrengthButton0.IsEnabled = isEnabled;
			this.StrengthButton1.IsEnabled = isEnabled;
			this.StrengthButton2.IsEnabled = isEnabled;
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06000FDE RID: 4062 RVA: 0x0002BAE5 File Offset: 0x00029CE5
		// (set) Token: 0x06000FDF RID: 4063 RVA: 0x0002BAED File Offset: 0x00029CED
		[Editor(false)]
		public int CurrentIndex
		{
			get
			{
				return this._currentIndex;
			}
			set
			{
				if (this._currentIndex != value)
				{
					this._currentIndex = value;
					base.OnPropertyChanged(value, "CurrentIndex");
				}
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x0002BB0B File Offset: 0x00029D0B
		// (set) Token: 0x06000FE1 RID: 4065 RVA: 0x0002BB13 File Offset: 0x00029D13
		[Editor(false)]
		public ButtonWidget StrengthButton0
		{
			get
			{
				return this._strengthButton0;
			}
			set
			{
				if (this._strengthButton0 != value)
				{
					this._strengthButton0 = value;
					base.OnPropertyChanged<ButtonWidget>(value, "StrengthButton0");
				}
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06000FE2 RID: 4066 RVA: 0x0002BB31 File Offset: 0x00029D31
		// (set) Token: 0x06000FE3 RID: 4067 RVA: 0x0002BB39 File Offset: 0x00029D39
		[Editor(false)]
		public ButtonWidget StrengthButton1
		{
			get
			{
				return this._strengthButton1;
			}
			set
			{
				if (this._strengthButton1 != value)
				{
					this._strengthButton1 = value;
					base.OnPropertyChanged<ButtonWidget>(value, "StrengthButton1");
				}
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x0002BB57 File Offset: 0x00029D57
		// (set) Token: 0x06000FE5 RID: 4069 RVA: 0x0002BB5F File Offset: 0x00029D5F
		[Editor(false)]
		public ButtonWidget StrengthButton2
		{
			get
			{
				return this._strengthButton2;
			}
			set
			{
				if (this._strengthButton2 != value)
				{
					this._strengthButton2 = value;
					base.OnPropertyChanged<ButtonWidget>(value, "StrengthButton2");
				}
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x0002BB7D File Offset: 0x00029D7D
		// (set) Token: 0x06000FE7 RID: 4071 RVA: 0x0002BB85 File Offset: 0x00029D85
		[Editor(false)]
		public RichTextWidget StrengthButton0Text
		{
			get
			{
				return this._strengthButton0Text;
			}
			set
			{
				if (this._strengthButton0Text != value)
				{
					this._strengthButton0Text = value;
					base.OnPropertyChanged<RichTextWidget>(value, "StrengthButton0Text");
				}
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x0002BBA3 File Offset: 0x00029DA3
		// (set) Token: 0x06000FE9 RID: 4073 RVA: 0x0002BBAB File Offset: 0x00029DAB
		[Editor(false)]
		public RichTextWidget StrengthButton1Text
		{
			get
			{
				return this._strengthButton1Text;
			}
			set
			{
				if (this._strengthButton1Text != value)
				{
					this._strengthButton1Text = value;
					base.OnPropertyChanged<RichTextWidget>(value, "StrengthButton1Text");
				}
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06000FEA RID: 4074 RVA: 0x0002BBC9 File Offset: 0x00029DC9
		// (set) Token: 0x06000FEB RID: 4075 RVA: 0x0002BBD1 File Offset: 0x00029DD1
		[Editor(false)]
		public RichTextWidget StrengthButton2Text
		{
			get
			{
				return this._strengthButton2Text;
			}
			set
			{
				if (this._strengthButton2Text != value)
				{
					this._strengthButton2Text = value;
					base.OnPropertyChanged<RichTextWidget>(value, "StrengthButton2Text");
				}
			}
		}

		// Token: 0x04000732 RID: 1842
		private ButtonWidget _strengthButton0;

		// Token: 0x04000733 RID: 1843
		private RichTextWidget _strengthButton0Text;

		// Token: 0x04000734 RID: 1844
		private ButtonWidget _strengthButton1;

		// Token: 0x04000735 RID: 1845
		private RichTextWidget _strengthButton1Text;

		// Token: 0x04000736 RID: 1846
		private ButtonWidget _strengthButton2;

		// Token: 0x04000737 RID: 1847
		private RichTextWidget _strengthButton2Text;

		// Token: 0x04000738 RID: 1848
		private int _currentIndex;
	}
}
