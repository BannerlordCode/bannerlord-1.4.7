using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.ClassLoadout;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Perks
{
	// Token: 0x02000098 RID: 152
	public class MultiplayerPerkContainerPanelWidget : Widget
	{
		// Token: 0x0600082A RID: 2090 RVA: 0x00017809 File Offset: 0x00015A09
		public MultiplayerPerkContainerPanelWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x00017814 File Offset: 0x00015A14
		protected override void OnUpdate(float dt)
		{
			Widget latestMouseUpWidget = base.EventManager.LatestMouseUpWidget;
			if (this.TroopTupleBodyWidget != null)
			{
				MultiplayerClassLoadoutTroopSubclassButtonWidget troopTupleBodyWidget = this.TroopTupleBodyWidget;
				if (troopTupleBodyWidget == null || !troopTupleBodyWidget.IsSelected)
				{
					goto IL_005E;
				}
			}
			if (!base.CheckIsMyChildRecursive(latestMouseUpWidget) && (this.PopupWidgetFirst.IsVisible || this.PopupWidgetSecond.IsVisible || this.PopupWidgetThird.IsVisible))
			{
				this.ClosePanel();
			}
			IL_005E:
			MultiplayerClassLoadoutTroopSubclassButtonWidget troopTupleBodyWidget2 = this.TroopTupleBodyWidget;
			if ((troopTupleBodyWidget2 == null || !troopTupleBodyWidget2.IsSelected) && this._currentSelectedItem != null)
			{
				this._currentSelectedItem.IsSelected = false;
				this._currentSelectedItem = null;
			}
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x000178B4 File Offset: 0x00015AB4
		public void PerkSelected(MultiplayerPerkItemToggleWidget selectedItem)
		{
			if (selectedItem == this._currentSelectedItem || selectedItem == null)
			{
				this.ClosePanel();
				return;
			}
			if (selectedItem != null && selectedItem.ParentWidget != null)
			{
				if (this._currentSelectedItem != null)
				{
					this._currentSelectedItem.IsSelected = false;
				}
				int childIndex = selectedItem.ParentWidget.GetChildIndex(selectedItem);
				this.PopupWidgetFirst.IsVisible = childIndex == 0;
				this.PopupWidgetFirst.IsEnabled = childIndex == 0;
				this.PopupWidgetSecond.IsVisible = childIndex == 1;
				this.PopupWidgetSecond.IsEnabled = childIndex == 1;
				this.PopupWidgetThird.IsVisible = childIndex == 2;
				this.PopupWidgetThird.IsEnabled = childIndex == 2;
				this.PopupWidgetFirst.SetPopupPerksContainer(this);
				this.PopupWidgetSecond.SetPopupPerksContainer(this);
				this.PopupWidgetThird.SetPopupPerksContainer(this);
				this._currentSelectedItem = selectedItem;
				this._currentSelectedItem.IsSelected = true;
			}
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x00017998 File Offset: 0x00015B98
		private void ClosePanel()
		{
			if (this._currentSelectedItem != null)
			{
				this._currentSelectedItem.IsSelected = false;
			}
			this._currentSelectedItem = null;
			this.PopupWidgetFirst.IsVisible = false;
			this.PopupWidgetSecond.IsVisible = false;
			this.PopupWidgetThird.IsVisible = false;
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x000179E4 File Offset: 0x00015BE4
		// (set) Token: 0x0600082F RID: 2095 RVA: 0x000179EC File Offset: 0x00015BEC
		public MultiplayerPerkPopupWidget PopupWidgetFirst
		{
			get
			{
				return this._popupWidgetFirst;
			}
			set
			{
				if (value != this._popupWidgetFirst)
				{
					this._popupWidgetFirst = value;
					base.OnPropertyChanged<MultiplayerPerkPopupWidget>(value, "PopupWidgetFirst");
				}
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x00017A0A File Offset: 0x00015C0A
		// (set) Token: 0x06000831 RID: 2097 RVA: 0x00017A12 File Offset: 0x00015C12
		public MultiplayerPerkPopupWidget PopupWidgetSecond
		{
			get
			{
				return this._popupWidgetSecond;
			}
			set
			{
				if (value != this._popupWidgetSecond)
				{
					this._popupWidgetSecond = value;
					base.OnPropertyChanged<MultiplayerPerkPopupWidget>(value, "PopupWidgetSecond");
				}
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x00017A30 File Offset: 0x00015C30
		// (set) Token: 0x06000833 RID: 2099 RVA: 0x00017A38 File Offset: 0x00015C38
		public MultiplayerPerkPopupWidget PopupWidgetThird
		{
			get
			{
				return this._popupWidgetThird;
			}
			set
			{
				if (value != this._popupWidgetThird)
				{
					this._popupWidgetThird = value;
					base.OnPropertyChanged<MultiplayerPerkPopupWidget>(value, "PopupWidgetThird");
				}
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000834 RID: 2100 RVA: 0x00017A56 File Offset: 0x00015C56
		// (set) Token: 0x06000835 RID: 2101 RVA: 0x00017A5E File Offset: 0x00015C5E
		public MultiplayerClassLoadoutTroopSubclassButtonWidget TroopTupleBodyWidget
		{
			get
			{
				return this._troopTupleBodyWidget;
			}
			set
			{
				if (value != this._troopTupleBodyWidget)
				{
					this._troopTupleBodyWidget = value;
					base.OnPropertyChanged<MultiplayerClassLoadoutTroopSubclassButtonWidget>(value, "TroopTupleBodyWidget");
				}
			}
		}

		// Token: 0x040003A6 RID: 934
		private MultiplayerPerkItemToggleWidget _currentSelectedItem;

		// Token: 0x040003A7 RID: 935
		private MultiplayerPerkPopupWidget _popupWidgetFirst;

		// Token: 0x040003A8 RID: 936
		private MultiplayerPerkPopupWidget _popupWidgetSecond;

		// Token: 0x040003A9 RID: 937
		private MultiplayerPerkPopupWidget _popupWidgetThird;

		// Token: 0x040003AA RID: 938
		private MultiplayerClassLoadoutTroopSubclassButtonWidget _troopTupleBodyWidget;
	}
}
