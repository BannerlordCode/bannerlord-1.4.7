using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Nameplate
{
	// Token: 0x02000081 RID: 129
	public class SettlementNameplateItemWidget : Widget
	{
		// Token: 0x06000722 RID: 1826 RVA: 0x00014DAB File Offset: 0x00012FAB
		public SettlementNameplateItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x00014DB4 File Offset: 0x00012FB4
		// (set) Token: 0x06000724 RID: 1828 RVA: 0x00014DBC File Offset: 0x00012FBC
		public bool IsOverWidget { get; private set; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x00014DC5 File Offset: 0x00012FC5
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x00014DCD File Offset: 0x00012FCD
		public int QuestType { get; set; }

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x00014DD6 File Offset: 0x00012FD6
		// (set) Token: 0x06000728 RID: 1832 RVA: 0x00014DDE File Offset: 0x00012FDE
		public int IssueType { get; set; }

		// Token: 0x06000729 RID: 1833 RVA: 0x00014DE8 File Offset: 0x00012FE8
		public void ParallelUpdate(float dt)
		{
			Widget widgetToShow = this._widgetToShow;
			Widget parentWidget = base.ParentWidget;
			if (widgetToShow == null)
			{
				Debug.FailedAssert("widgetToShow is null during ParallelUpdate!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Nameplate\\SettlementNameplateItemWidget.cs", "ParallelUpdate", 24);
				return;
			}
			if (parentWidget != null && parentWidget.IsEnabled)
			{
				this.IsOverWidget = this.IsMouseOverWidget();
				if (this.IsOverWidget && !this._hoverBegan)
				{
					this._hoverBegan = true;
					widgetToShow.IsVisible = true;
				}
				else if (!this.IsOverWidget && this._hoverBegan)
				{
					this._hoverBegan = false;
					widgetToShow.IsVisible = false;
				}
				if (!this.IsOverWidget && widgetToShow.IsVisible)
				{
					widgetToShow.IsVisible = false;
					return;
				}
			}
			else
			{
				widgetToShow.IsVisible = false;
			}
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00014E94 File Offset: 0x00013094
		private bool IsMouseOverWidget()
		{
			if (!base.EventManager.GetIsHitThisFrame() || !base.EventManager.IsPointInsideUsableArea(base.EventManager.MousePosition))
			{
				return false;
			}
			Vector2 mousePosition = base.EventManager.MousePosition;
			return this.AreaRect.IsPointInside(in mousePosition);
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x00014EE1 File Offset: 0x000130E1
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x00014EE9 File Offset: 0x000130E9
		public Widget InspectedIconWidget
		{
			get
			{
				return this._inspectedIconWidget;
			}
			set
			{
				if (this._inspectedIconWidget != value)
				{
					this._inspectedIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "InspectedIconWidget");
				}
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x00014F07 File Offset: 0x00013107
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x00014F0F File Offset: 0x0001310F
		public Widget PortIconWidget
		{
			get
			{
				return this._portIconWidget;
			}
			set
			{
				if (this._portIconWidget != value)
				{
					this._portIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "PortIconWidget");
				}
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00014F2D File Offset: 0x0001312D
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x00014F35 File Offset: 0x00013135
		public GridWidget SettlementPartiesGridWidget
		{
			get
			{
				return this._settlementPartiesGridWidget;
			}
			set
			{
				if (this._settlementPartiesGridWidget != value)
				{
					this._settlementPartiesGridWidget = value;
					base.OnPropertyChanged<GridWidget>(value, "SettlementPartiesGridWidget");
				}
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x00014F53 File Offset: 0x00013153
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x00014F5B File Offset: 0x0001315B
		public MapEventVisualBrushWidget MapEventVisualWidget
		{
			get
			{
				return this._mapEventVisualWidget;
			}
			set
			{
				if (this._mapEventVisualWidget != value)
				{
					this._mapEventVisualWidget = value;
					base.OnPropertyChanged<MapEventVisualBrushWidget>(value, "MapEventVisualWidget");
				}
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x00014F79 File Offset: 0x00013179
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x00014F81 File Offset: 0x00013181
		[Editor(false)]
		public Widget WidgetToShow
		{
			get
			{
				return this._widgetToShow;
			}
			set
			{
				if (this._widgetToShow != value)
				{
					this._widgetToShow = value;
					base.OnPropertyChanged<Widget>(value, "WidgetToShow");
				}
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x00014F9F File Offset: 0x0001319F
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x00014FA7 File Offset: 0x000131A7
		public MaskedTextureWidget SettlementBannerWidget
		{
			get
			{
				return this._settlementBannerWidget;
			}
			set
			{
				if (this._settlementBannerWidget != value)
				{
					this._settlementBannerWidget = value;
					base.OnPropertyChanged<MaskedTextureWidget>(value, "SettlementBannerWidget");
				}
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x00014FC5 File Offset: 0x000131C5
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x00014FCD File Offset: 0x000131CD
		public TextWidget SettlementNameTextWidget
		{
			get
			{
				return this._settlementNameTextWidget;
			}
			set
			{
				if (this._settlementNameTextWidget != value)
				{
					this._settlementNameTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "SettlementNameTextWidget");
				}
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x00014FEB File Offset: 0x000131EB
		// (set) Token: 0x0600073A RID: 1850 RVA: 0x00014FF3 File Offset: 0x000131F3
		public Widget ParleyIconWidget
		{
			get
			{
				return this._parleyIconWidget;
			}
			set
			{
				if (this._parleyIconWidget != value)
				{
					this._parleyIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "ParleyIconWidget");
				}
			}
		}

		// Token: 0x04000319 RID: 793
		private bool _hoverBegan;

		// Token: 0x0400031D RID: 797
		private Widget _inspectedIconWidget;

		// Token: 0x0400031E RID: 798
		private Widget _portIconWidget;

		// Token: 0x0400031F RID: 799
		private MapEventVisualBrushWidget _mapEventVisualWidget;

		// Token: 0x04000320 RID: 800
		private MaskedTextureWidget _settlementBannerWidget;

		// Token: 0x04000321 RID: 801
		private TextWidget _settlementNameTextWidget;

		// Token: 0x04000322 RID: 802
		private GridWidget _settlementPartiesGridWidget;

		// Token: 0x04000323 RID: 803
		private Widget _widgetToShow;

		// Token: 0x04000324 RID: 804
		private Widget _parleyIconWidget;
	}
}
