using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.TownManagement
{
	// Token: 0x02000106 RID: 262
	public class AutoClosePopupWidget : Widget
	{
		// Token: 0x06000DF5 RID: 3573 RVA: 0x00026338 File Offset: 0x00024538
		public AutoClosePopupWidget(UIContext context)
			: base(context)
		{
			for (int i = 0; i < base.ChildCount; i++)
			{
				AutoClosePopupClosingWidget autoClosePopupClosingWidget;
				if ((autoClosePopupClosingWidget = base.GetChild(i) as AutoClosePopupClosingWidget) != null)
				{
					this._closingWidgets.Add(autoClosePopupClosingWidget);
				}
			}
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00026384 File Offset: 0x00024584
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.IsVisible && base.EventManager.LatestMouseUpWidget != this.PopupParentWidget && base.EventManager.LatestMouseUpWidget != this._lastCheckedMouseUpWidget)
			{
				base.IsVisible = base.EventManager.LatestMouseUpWidget == this || base.CheckIsMyChildRecursive(base.EventManager.LatestMouseUpWidget);
				this.CheckClosingWidgetsAndUpdateVisibility();
				this._lastCheckedMouseUpWidget = (base.IsVisible ? base.EventManager.LatestMouseUpWidget : null);
			}
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00026410 File Offset: 0x00024610
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			AutoClosePopupClosingWidget autoClosePopupClosingWidget;
			if ((autoClosePopupClosingWidget = child as AutoClosePopupClosingWidget) != null)
			{
				this._closingWidgets.Add(autoClosePopupClosingWidget);
			}
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0002643C File Offset: 0x0002463C
		protected void CheckClosingWidgetsAndUpdateVisibility()
		{
			if (base.IsVisible)
			{
				using (List<AutoClosePopupClosingWidget>.Enumerator enumerator = this._closingWidgets.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ShouldClosePopup())
						{
							base.IsVisible = false;
							break;
						}
					}
				}
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06000DF9 RID: 3577 RVA: 0x000264A0 File Offset: 0x000246A0
		// (set) Token: 0x06000DFA RID: 3578 RVA: 0x000264A8 File Offset: 0x000246A8
		[Editor(false)]
		public Widget PopupParentWidget
		{
			get
			{
				return this._popupParentWidget;
			}
			set
			{
				if (this._popupParentWidget != value)
				{
					this._popupParentWidget = value;
					base.OnPropertyChanged<Widget>(value, "PopupParentWidget");
				}
			}
		}

		// Token: 0x04000654 RID: 1620
		private List<AutoClosePopupClosingWidget> _closingWidgets = new List<AutoClosePopupClosingWidget>();

		// Token: 0x04000655 RID: 1621
		protected Widget _lastCheckedMouseUpWidget;

		// Token: 0x04000656 RID: 1622
		private Widget _popupParentWidget;
	}
}
