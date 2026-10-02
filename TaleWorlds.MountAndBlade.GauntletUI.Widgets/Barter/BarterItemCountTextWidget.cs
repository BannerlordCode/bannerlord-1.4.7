using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Barter
{
	// Token: 0x0200018F RID: 399
	public class BarterItemCountTextWidget : TextWidget
	{
		// Token: 0x06001492 RID: 5266 RVA: 0x00037F57 File Offset: 0x00036157
		public BarterItemCountTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06001493 RID: 5267 RVA: 0x00037F60 File Offset: 0x00036160
		// (set) Token: 0x06001494 RID: 5268 RVA: 0x00037F68 File Offset: 0x00036168
		[Editor(false)]
		public int Count
		{
			get
			{
				return this._count;
			}
			set
			{
				if (this._count != value)
				{
					this._count = value;
					base.OnPropertyChanged(value, "Count");
					base.IntText = value;
					base.IsVisible = value > 1;
				}
			}
		}

		// Token: 0x04000955 RID: 2389
		private int _count;
	}
}
