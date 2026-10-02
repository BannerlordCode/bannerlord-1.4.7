using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.Library;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapBar
{
	// Token: 0x0200005A RID: 90
	public class MapBarVM : ViewModel
	{
		// Token: 0x0600066F RID: 1647 RVA: 0x00020BCD File Offset: 0x0001EDCD
		protected virtual MapInfoVM CreateInfoVM()
		{
			return new MapInfoVM();
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00020BD4 File Offset: 0x0001EDD4
		public void Initialize(INavigationHandler navigationHandler, IMapStateHandler mapStateHandler, Func<MapBarShortcuts> getMapBarShortcuts, Action openArmyManagement)
		{
			this._navigationHandler = navigationHandler;
			this._refreshTimeSpan = ((Campaign.Current.GetSimplifiedTimeControlMode() == CampaignTimeControlMode.UnstoppableFastForward) ? 0.1f : 2f);
			this._openArmyManagement = openArmyManagement;
			this._mapStateHandler = mapStateHandler;
			this.TutorialNotification = new ElementNotificationVM();
			this.MapInfo = this.CreateInfoVM();
			this.MapTimeControl = new MapTimeControlVM(getMapBarShortcuts, new Action(this.OnTimeControlChange), delegate
			{
				mapStateHandler.ResetCamera(false, false);
			});
			this.MapNavigation = new MapNavigationVM(navigationHandler, getMapBarShortcuts);
			this.GatherArmyHint = new HintViewModel();
			this.OnRefresh();
			this.IsEnabled = true;
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00020CA3 File Offset: 0x0001EEA3
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.MapInfo.RefreshValues();
			this.MapTimeControl.RefreshValues();
			this.MapNavigation.RefreshValues();
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00020CCC File Offset: 0x0001EECC
		public void OnRefresh()
		{
			this.MapInfo.Refresh();
			this.MapTimeControl.Refresh();
			this.MapNavigation.Refresh();
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00020CF0 File Offset: 0x0001EEF0
		public void Tick(float dt)
		{
			int simplifiedTimeControlMode = (int)Campaign.Current.GetSimplifiedTimeControlMode();
			this._refreshTimeSpan -= dt;
			if (this._refreshTimeSpan < 0f)
			{
				this.OnRefresh();
				this._refreshTimeSpan = ((simplifiedTimeControlMode == 2) ? 0.1f : 0.2f);
			}
			this.MapInfo.Tick();
			this.MapTimeControl.Tick();
			this.MapNavigation.Tick();
			if (this._mapStateHandler != null)
			{
				this.IsCameraCentered = this._mapStateHandler.IsCameraLockedToPlayerParty();
			}
			this.IsGatherArmyVisible = this.GetIsGatherArmyVisible();
			if (this.IsGatherArmyVisible)
			{
				this.UpdateCanGatherArmyAndReason();
			}
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00020D94 File Offset: 0x0001EF94
		private void UpdateCanGatherArmyAndReason()
		{
			TextObject textObject;
			this.CanGatherArmy = Campaign.Current.Models.ArmyManagementCalculationModel.CanPlayerCreateArmy(out textObject);
			this.GatherArmyHint.HintText = textObject;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00020DCC File Offset: 0x0001EFCC
		private bool GetIsGatherArmyVisible()
		{
			if (this.MapTimeControl.IsInMap)
			{
				MobileParty mainParty = MobileParty.MainParty;
				if (((mainParty != null) ? mainParty.Army : null) == null && !Hero.MainHero.IsPrisoner && Hero.MainHero.PartyBelongedTo != null && MobileParty.MainParty.MapEvent == null)
				{
					return this.MapTimeControl.IsCenterPanelEnabled;
				}
			}
			return false;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00020E2A File Offset: 0x0001F02A
		private void OnTimeControlChange()
		{
			this._refreshTimeSpan = ((Campaign.Current.GetSimplifiedTimeControlMode() == CampaignTimeControlMode.UnstoppableFastForward) ? 0.1f : 2f);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00020E4B File Offset: 0x0001F04B
		private void ExecuteResetCamera()
		{
			IMapStateHandler mapStateHandler = this._mapStateHandler;
			if (mapStateHandler == null)
			{
				return;
			}
			mapStateHandler.FastMoveCameraToMainParty();
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00020E5D File Offset: 0x0001F05D
		public void ExecuteArmyManagement()
		{
			this._openArmyManagement();
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00020E6C File Offset: 0x0001F06C
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent obj)
		{
			if (obj.NewNotificationElementID != this._latestTutorialElementID)
			{
				if (this._latestTutorialElementID != null)
				{
					this.TutorialNotification.ElementID = string.Empty;
				}
				this._latestTutorialElementID = obj.NewNotificationElementID;
				if (this._latestTutorialElementID != null)
				{
					this.TutorialNotification.ElementID = this._latestTutorialElementID;
					if (this._latestTutorialElementID == "PartySpeedLabel" && !this.MapInfo.IsInfoBarExtended)
					{
						this.MapInfo.IsInfoBarExtended = true;
					}
				}
			}
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00020EF4 File Offset: 0x0001F0F4
		public override void OnFinalize()
		{
			base.OnFinalize();
			this._mapStateHandler = null;
			MapNavigationVM mapNavigation = this._mapNavigation;
			if (mapNavigation != null)
			{
				mapNavigation.OnFinalize();
			}
			MapTimeControlVM mapTimeControl = this._mapTimeControl;
			if (mapTimeControl != null)
			{
				mapTimeControl.OnFinalize();
			}
			this._mapInfo = null;
			this._mapNavigation = null;
			this._mapTimeControl = null;
			Game game = Game.Current;
			if (game == null)
			{
				return;
			}
			EventManager eventManager = game.EventManager;
			if (eventManager == null)
			{
				return;
			}
			eventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x00020F6A File Offset: 0x0001F16A
		// (set) Token: 0x0600067C RID: 1660 RVA: 0x00020F72 File Offset: 0x0001F172
		[DataSourceProperty]
		public MapInfoVM MapInfo
		{
			get
			{
				return this._mapInfo;
			}
			set
			{
				if (value != this._mapInfo)
				{
					this._mapInfo = value;
					base.OnPropertyChangedWithValue<MapInfoVM>(value, "MapInfo");
				}
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x00020F90 File Offset: 0x0001F190
		// (set) Token: 0x0600067E RID: 1662 RVA: 0x00020F98 File Offset: 0x0001F198
		[DataSourceProperty]
		public MapTimeControlVM MapTimeControl
		{
			get
			{
				return this._mapTimeControl;
			}
			set
			{
				if (value != this._mapTimeControl)
				{
					this._mapTimeControl = value;
					base.OnPropertyChangedWithValue<MapTimeControlVM>(value, "MapTimeControl");
				}
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x00020FB6 File Offset: 0x0001F1B6
		// (set) Token: 0x06000680 RID: 1664 RVA: 0x00020FBE File Offset: 0x0001F1BE
		[DataSourceProperty]
		public MapNavigationVM MapNavigation
		{
			get
			{
				return this._mapNavigation;
			}
			set
			{
				if (value != this._mapNavigation)
				{
					this._mapNavigation = value;
					base.OnPropertyChangedWithValue<MapNavigationVM>(value, "MapNavigation");
				}
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x00020FDC File Offset: 0x0001F1DC
		// (set) Token: 0x06000682 RID: 1666 RVA: 0x00020FE4 File Offset: 0x0001F1E4
		[DataSourceProperty]
		public bool IsGatherArmyVisible
		{
			get
			{
				return this._isGatherArmyVisible;
			}
			set
			{
				if (value != this._isGatherArmyVisible)
				{
					this._isGatherArmyVisible = value;
					base.OnPropertyChangedWithValue(value, "IsGatherArmyVisible");
				}
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x00021002 File Offset: 0x0001F202
		// (set) Token: 0x06000684 RID: 1668 RVA: 0x0002100A File Offset: 0x0001F20A
		[DataSourceProperty]
		public bool IsInInfoMode
		{
			get
			{
				return this._isInInfoMode;
			}
			set
			{
				if (value != this._isInInfoMode)
				{
					this._isInInfoMode = value;
					base.OnPropertyChangedWithValue(value, "IsInInfoMode");
				}
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x00021028 File Offset: 0x0001F228
		// (set) Token: 0x06000686 RID: 1670 RVA: 0x00021030 File Offset: 0x0001F230
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x0002104E File Offset: 0x0001F24E
		// (set) Token: 0x06000688 RID: 1672 RVA: 0x00021056 File Offset: 0x0001F256
		[DataSourceProperty]
		public bool CanGatherArmy
		{
			get
			{
				return this._canGatherArmy;
			}
			set
			{
				if (value != this._canGatherArmy)
				{
					this._canGatherArmy = value;
					base.OnPropertyChangedWithValue(value, "CanGatherArmy");
				}
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x00021074 File Offset: 0x0001F274
		// (set) Token: 0x0600068A RID: 1674 RVA: 0x0002107C File Offset: 0x0001F27C
		[DataSourceProperty]
		public HintViewModel GatherArmyHint
		{
			get
			{
				return this._gatherArmyHint;
			}
			set
			{
				if (value != this._gatherArmyHint)
				{
					this._gatherArmyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "GatherArmyHint");
				}
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x0002109A File Offset: 0x0001F29A
		// (set) Token: 0x0600068C RID: 1676 RVA: 0x000210A2 File Offset: 0x0001F2A2
		[DataSourceProperty]
		public bool IsCameraCentered
		{
			get
			{
				return this._isCameraCentered;
			}
			set
			{
				if (value != this._isCameraCentered)
				{
					this._isCameraCentered = value;
					base.OnPropertyChangedWithValue(value, "IsCameraCentered");
				}
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x000210C0 File Offset: 0x0001F2C0
		// (set) Token: 0x0600068E RID: 1678 RVA: 0x000210C8 File Offset: 0x0001F2C8
		[DataSourceProperty]
		public string CurrentScreen
		{
			get
			{
				return this._currentScreen;
			}
			set
			{
				if (this._currentScreen != value)
				{
					this._currentScreen = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentScreen");
				}
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x000210EB File Offset: 0x0001F2EB
		// (set) Token: 0x06000690 RID: 1680 RVA: 0x000210F3 File Offset: 0x0001F2F3
		[DataSourceProperty]
		public ElementNotificationVM TutorialNotification
		{
			get
			{
				return this._tutorialNotification;
			}
			set
			{
				if (value != this._tutorialNotification)
				{
					this._tutorialNotification = value;
					base.OnPropertyChangedWithValue<ElementNotificationVM>(value, "TutorialNotification");
				}
			}
		}

		// Token: 0x040002C3 RID: 707
		protected INavigationHandler _navigationHandler;

		// Token: 0x040002C4 RID: 708
		private IMapStateHandler _mapStateHandler;

		// Token: 0x040002C5 RID: 709
		private Action _openArmyManagement;

		// Token: 0x040002C6 RID: 710
		private float _refreshTimeSpan;

		// Token: 0x040002C7 RID: 711
		private string _latestTutorialElementID;

		// Token: 0x040002C8 RID: 712
		private bool _isGatherArmyVisible;

		// Token: 0x040002C9 RID: 713
		private MapInfoVM _mapInfo;

		// Token: 0x040002CA RID: 714
		private MapTimeControlVM _mapTimeControl;

		// Token: 0x040002CB RID: 715
		private MapNavigationVM _mapNavigation;

		// Token: 0x040002CC RID: 716
		private bool _isEnabled;

		// Token: 0x040002CD RID: 717
		private bool _isCameraCentered;

		// Token: 0x040002CE RID: 718
		private bool _canGatherArmy;

		// Token: 0x040002CF RID: 719
		private bool _isInInfoMode;

		// Token: 0x040002D0 RID: 720
		private string _currentScreen;

		// Token: 0x040002D1 RID: 721
		private HintViewModel _gatherArmyHint;

		// Token: 0x040002D2 RID: 722
		private ElementNotificationVM _tutorialNotification;
	}
}
