using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200002F RID: 47
	public class NavigationAutoScrollWidget : Widget
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000288 RID: 648 RVA: 0x00008B87 File Offset: 0x00006D87
		// (set) Token: 0x06000289 RID: 649 RVA: 0x00008B8F File Offset: 0x00006D8F
		public ScrollablePanel ParentPanel { get; set; }

		// Token: 0x0600028A RID: 650 RVA: 0x00008B98 File Offset: 0x00006D98
		public NavigationAutoScrollWidget(UIContext context)
			: base(context)
		{
			base.WidthSizePolicy = SizePolicy.Fixed;
			base.HeightSizePolicy = SizePolicy.Fixed;
			base.SuggestedHeight = 0f;
			base.SuggestedWidth = 0f;
			base.IsVisible = false;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00008BCC File Offset: 0x00006DCC
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			if (this.ParentPanel == null && base.ParentWidget != null)
			{
				for (Widget widget = base.ParentWidget; widget != null; widget = widget.ParentWidget)
				{
					ScrollablePanel scrollablePanel;
					if ((scrollablePanel = widget as ScrollablePanel) != null)
					{
						this.ParentPanel = scrollablePanel;
						return;
					}
				}
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00008C14 File Offset: 0x00006E14
		private void OnWidgetGainedGamepadFocus(Widget widget)
		{
			if (this.ParentPanel != null)
			{
				ScrollablePanel.AutoScrollParameters autoScrollParameters = new ScrollablePanel.AutoScrollParameters((float)this.AutoScrollTopOffset, (float)this.AutoScrollBottomOffset, (float)this.AutoScrollLeftOffset, (float)this.AutoScrollRightOffset, -1f, -1f, 0f);
				this.ParentPanel.ScrollToChild(this.ScrollTarget ?? widget, autoScrollParameters);
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00008C74 File Offset: 0x00006E74
		private void UpdateTargetAutoScrollAndChildren()
		{
			if (this._trackedWidget != null)
			{
				Widget trackedWidget = this._trackedWidget;
				trackedWidget.OnGamepadNavigationFocusGained = (Action<Widget>)Delegate.Combine(trackedWidget.OnGamepadNavigationFocusGained, new Action<Widget>(this.OnWidgetGainedGamepadFocus));
				foreach (Widget widget in this._trackedWidget.Children)
				{
					if (this.IncludeChildren)
					{
						Widget widget2 = widget;
						widget2.OnGamepadNavigationFocusGained = (Action<Widget>)Delegate.Combine(widget2.OnGamepadNavigationFocusGained, new Action<Widget>(this.OnWidgetGainedGamepadFocus));
					}
					else
					{
						Widget widget3 = widget;
						widget3.OnGamepadNavigationFocusGained = (Action<Widget>)Delegate.Remove(widget3.OnGamepadNavigationFocusGained, new Action<Widget>(this.OnWidgetGainedGamepadFocus));
					}
				}
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600028E RID: 654 RVA: 0x00008D48 File Offset: 0x00006F48
		// (set) Token: 0x0600028F RID: 655 RVA: 0x00008D50 File Offset: 0x00006F50
		public int AutoScrollTopOffset { get; set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000290 RID: 656 RVA: 0x00008D59 File Offset: 0x00006F59
		// (set) Token: 0x06000291 RID: 657 RVA: 0x00008D61 File Offset: 0x00006F61
		public int AutoScrollBottomOffset { get; set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000292 RID: 658 RVA: 0x00008D6A File Offset: 0x00006F6A
		// (set) Token: 0x06000293 RID: 659 RVA: 0x00008D72 File Offset: 0x00006F72
		public int AutoScrollLeftOffset { get; set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000294 RID: 660 RVA: 0x00008D7B File Offset: 0x00006F7B
		// (set) Token: 0x06000295 RID: 661 RVA: 0x00008D83 File Offset: 0x00006F83
		public int AutoScrollRightOffset { get; set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000296 RID: 662 RVA: 0x00008D8C File Offset: 0x00006F8C
		// (set) Token: 0x06000297 RID: 663 RVA: 0x00008D94 File Offset: 0x00006F94
		public bool IncludeChildren
		{
			get
			{
				return this._includeChildren;
			}
			set
			{
				if (value != this._includeChildren)
				{
					this._includeChildren = value;
					this.UpdateTargetAutoScrollAndChildren();
				}
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000298 RID: 664 RVA: 0x00008DAC File Offset: 0x00006FAC
		// (set) Token: 0x06000299 RID: 665 RVA: 0x00008DB4 File Offset: 0x00006FB4
		public Widget TrackedWidget
		{
			get
			{
				return this._trackedWidget;
			}
			set
			{
				if (value != this._trackedWidget)
				{
					if (this._trackedWidget != null)
					{
						this._trackedWidget.OnGamepadNavigationFocusGained = null;
					}
					this._trackedWidget = value;
					this.UpdateTargetAutoScrollAndChildren();
				}
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600029A RID: 666 RVA: 0x00008DE0 File Offset: 0x00006FE0
		// (set) Token: 0x0600029B RID: 667 RVA: 0x00008DE8 File Offset: 0x00006FE8
		public Widget ScrollTarget
		{
			get
			{
				return this._scrollTarget;
			}
			set
			{
				if (value != this._scrollTarget)
				{
					this._scrollTarget = value;
				}
			}
		}

		// Token: 0x0400012B RID: 299
		private bool _includeChildren;

		// Token: 0x0400012C RID: 300
		private Widget _trackedWidget;

		// Token: 0x0400012D RID: 301
		private Widget _scrollTarget;
	}
}
