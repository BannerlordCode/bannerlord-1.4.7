using System;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x0200005C RID: 92
	public class IntegerInputPercentageTextWidget : IntegerInputTextWidget
	{
		// Token: 0x0600063F RID: 1599 RVA: 0x0001AD15 File Offset: 0x00018F15
		public IntegerInputPercentageTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0001AD1E File Offset: 0x00018F1E
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!base.IsFocused)
			{
				this.SetPercentageText();
			}
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0001AD35 File Offset: 0x00018F35
		protected internal override void OnGainFocus()
		{
			base.OnGainFocus();
			this.SetIntText();
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0001AD43 File Offset: 0x00018F43
		private void SetPercentageText()
		{
			base.Text = this.PercentageText;
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0001AD54 File Offset: 0x00018F54
		private void SetIntText()
		{
			base.Text = base.IntText.ToString();
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0001AD75 File Offset: 0x00018F75
		// (set) Token: 0x06000645 RID: 1605 RVA: 0x0001AD7D File Offset: 0x00018F7D
		[Editor(false)]
		public string PercentageText
		{
			get
			{
				return this._percentageText;
			}
			set
			{
				if (this._percentageText != value)
				{
					this._percentageText = value;
					base.OnPropertyChanged<string>(value, "PercentageText");
				}
			}
		}

		// Token: 0x040002F5 RID: 757
		private string _percentageText;
	}
}
