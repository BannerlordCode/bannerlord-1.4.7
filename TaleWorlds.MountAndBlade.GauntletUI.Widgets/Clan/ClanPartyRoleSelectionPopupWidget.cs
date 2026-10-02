using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.TownManagement;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Clan
{
	// Token: 0x02000176 RID: 374
	public class ClanPartyRoleSelectionPopupWidget : AutoClosePopupWidget
	{
		// Token: 0x06001382 RID: 4994 RVA: 0x00035004 File Offset: 0x00033204
		public ClanPartyRoleSelectionPopupWidget(UIContext context)
			: base(context)
		{
			this._toggleWidgets = new List<Widget>();
			base.IsVisible = false;
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00035020 File Offset: 0x00033220
		protected override void OnLateUpdate(float dt)
		{
			if (base.IsVisible && base.EventManager.LatestMouseUpWidget != this._lastCheckedMouseUpWidget && !this._toggleWidgets.Contains(base.EventManager.LatestMouseUpWidget))
			{
				base.IsVisible = base.EventManager.LatestMouseUpWidget == this || base.CheckIsMyChildRecursive(base.EventManager.LatestMouseUpWidget);
				base.CheckClosingWidgetsAndUpdateVisibility();
			}
			if (!base.IsVisible)
			{
				this.ActiveToggleWidget = null;
			}
			this._lastCheckedMouseUpWidget = base.EventManager.LatestMouseUpWidget;
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x000350AE File Offset: 0x000332AE
		public void AddToggleWidget(Widget widget)
		{
			if (!this._toggleWidgets.Contains(widget))
			{
				this._toggleWidgets.Add(widget);
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06001385 RID: 4997 RVA: 0x000350CA File Offset: 0x000332CA
		// (set) Token: 0x06001386 RID: 4998 RVA: 0x000350D2 File Offset: 0x000332D2
		[Editor(false)]
		public Widget ActiveToggleWidget
		{
			get
			{
				return this._activeToggleWidget;
			}
			set
			{
				if (this._activeToggleWidget != value)
				{
					this._activeToggleWidget = value;
					base.OnPropertyChanged<Widget>(value, "ActiveToggleWidget");
				}
			}
		}

		// Token: 0x040008D6 RID: 2262
		private List<Widget> _toggleWidgets;

		// Token: 0x040008D7 RID: 2263
		private Widget _activeToggleWidget;
	}
}
