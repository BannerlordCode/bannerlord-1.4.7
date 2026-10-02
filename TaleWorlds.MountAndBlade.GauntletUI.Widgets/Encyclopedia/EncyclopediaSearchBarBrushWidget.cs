using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Encyclopedia
{
	// Token: 0x0200015C RID: 348
	public class EncyclopediaSearchBarBrushWidget : BrushWidget
	{
		// Token: 0x0600126E RID: 4718 RVA: 0x00032B6E File Offset: 0x00030D6E
		public EncyclopediaSearchBarBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x00032B78 File Offset: 0x00030D78
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			bool flag = base.EventManager.LatestMouseUpWidget == this || base.CheckIsMyChildRecursive(base.EventManager.LatestMouseUpWidget) || base.EventManager.LatestMouseDownWidget == this || base.CheckIsMyChildRecursive(base.EventManager.LatestMouseDownWidget);
			bool flag2 = this.SearchResultPanel.CheckIsMyChildRecursive(base.EventManager.LatestMouseUpWidget) || this.SearchResultPanel.CheckIsMyChildRecursive(base.EventManager.LatestMouseDownWidget);
			this.ShowResults = (flag || flag2) && this.SearchInputWidget.Text.Length >= this.MinCharAmountToShowResults;
			this.SearchResultPanel.IsVisible = this.ShowResults;
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x00032C3C File Offset: 0x00030E3C
		protected override void OnMousePressed()
		{
			base.OnMousePressed();
			base.EventFired("SearchBarClick", Array.Empty<object>());
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06001271 RID: 4721 RVA: 0x00032C54 File Offset: 0x00030E54
		// (set) Token: 0x06001272 RID: 4722 RVA: 0x00032C5C File Offset: 0x00030E5C
		public bool ShowResults
		{
			get
			{
				return this._showChat;
			}
			set
			{
				if (value != this._showChat)
				{
					this._showChat = value;
					base.OnPropertyChanged(value, "ShowResults");
				}
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06001273 RID: 4723 RVA: 0x00032C7A File Offset: 0x00030E7A
		// (set) Token: 0x06001274 RID: 4724 RVA: 0x00032C84 File Offset: 0x00030E84
		public EditableTextWidget SearchInputWidget
		{
			get
			{
				return this._searchInputWidget;
			}
			set
			{
				if (value != this._searchInputWidget)
				{
					if (this._searchInputWidget != null)
					{
						this._searchInputWidget.EventFire -= this.OnSearchInputClick;
					}
					this._searchInputWidget = value;
					base.OnPropertyChanged<EditableTextWidget>(value, "SearchInputWidget");
					if (this._searchInputWidget != null)
					{
						this._searchInputWidget.EventFire += this.OnSearchInputClick;
					}
				}
			}
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x00032CEB File Offset: 0x00030EEB
		private void OnSearchInputClick(Widget widget, string eventName, object[] arguments)
		{
			if (eventName == "MouseDown")
			{
				base.EventFired("SearchBarClick", Array.Empty<object>());
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06001276 RID: 4726 RVA: 0x00032D0A File Offset: 0x00030F0A
		// (set) Token: 0x06001277 RID: 4727 RVA: 0x00032D12 File Offset: 0x00030F12
		public ScrollablePanel SearchResultPanel
		{
			get
			{
				return this._searchResultPanel;
			}
			set
			{
				if (value != this._searchResultPanel)
				{
					this._searchResultPanel = value;
					base.OnPropertyChanged<ScrollablePanel>(value, "SearchResultPanel");
				}
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06001278 RID: 4728 RVA: 0x00032D30 File Offset: 0x00030F30
		// (set) Token: 0x06001279 RID: 4729 RVA: 0x00032D38 File Offset: 0x00030F38
		public int MinCharAmountToShowResults
		{
			get
			{
				return this._minCharAmountToShowResults;
			}
			set
			{
				if (value != this._minCharAmountToShowResults)
				{
					this._minCharAmountToShowResults = value;
					base.OnPropertyChanged(value, "MinCharAmountToShowResults");
				}
			}
		}

		// Token: 0x0400085F RID: 2143
		private bool _showChat;

		// Token: 0x04000860 RID: 2144
		private ScrollablePanel _searchResultPanel;

		// Token: 0x04000861 RID: 2145
		private EditableTextWidget _searchInputWidget;

		// Token: 0x04000862 RID: 2146
		private int _minCharAmountToShowResults;
	}
}
