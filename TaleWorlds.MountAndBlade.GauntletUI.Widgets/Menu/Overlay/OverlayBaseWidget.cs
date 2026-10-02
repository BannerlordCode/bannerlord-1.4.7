using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.Overlay
{
	// Token: 0x02000111 RID: 273
	public class OverlayBaseWidget : Widget
	{
		// Token: 0x06000E91 RID: 3729 RVA: 0x00028194 File Offset: 0x00026394
		public OverlayBaseWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x0002819D File Offset: 0x0002639D
		// (set) Token: 0x06000E93 RID: 3731 RVA: 0x000281A5 File Offset: 0x000263A5
		[Editor(false)]
		public OverlayPopupWidget PopupWidget
		{
			get
			{
				return this._popupWidget;
			}
			set
			{
				if (this._popupWidget != value)
				{
					this._popupWidget = value;
					base.OnPropertyChanged<OverlayPopupWidget>(value, "PopupWidget");
				}
			}
		}

		// Token: 0x0400069D RID: 1693
		private OverlayPopupWidget _popupWidget;
	}
}
