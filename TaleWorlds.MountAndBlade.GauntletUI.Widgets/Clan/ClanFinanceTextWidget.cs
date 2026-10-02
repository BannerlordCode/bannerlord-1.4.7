using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Clan
{
	// Token: 0x02000174 RID: 372
	public class ClanFinanceTextWidget : TextWidget
	{
		// Token: 0x06001379 RID: 4985 RVA: 0x00034E82 File Offset: 0x00033082
		public ClanFinanceTextWidget(UIContext context)
			: base(context)
		{
			base.intPropertyChanged += this.IntText_PropertyChanged;
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x00034E9D File Offset: 0x0003309D
		private void IntText_PropertyChanged(PropertyOwnerObject widget, string propertyName, int propertyValue)
		{
			if (this.NegativeMarkWidget != null && propertyName == "IntText")
			{
				this.NegativeMarkWidget.IsVisible = propertyValue < 0;
			}
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x00034EC4 File Offset: 0x000330C4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.Text != null && base.Text != string.Empty)
			{
				base.Text = MathF.Abs(base.IntText).ToString();
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x0600137C RID: 4988 RVA: 0x00034F0B File Offset: 0x0003310B
		// (set) Token: 0x0600137D RID: 4989 RVA: 0x00034F13 File Offset: 0x00033113
		[Editor(false)]
		public TextWidget NegativeMarkWidget
		{
			get
			{
				return this._negativeMarkWidget;
			}
			set
			{
				if (this._negativeMarkWidget != value)
				{
					this._negativeMarkWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "NegativeMarkWidget");
				}
			}
		}

		// Token: 0x040008D4 RID: 2260
		private TextWidget _negativeMarkWidget;
	}
}
