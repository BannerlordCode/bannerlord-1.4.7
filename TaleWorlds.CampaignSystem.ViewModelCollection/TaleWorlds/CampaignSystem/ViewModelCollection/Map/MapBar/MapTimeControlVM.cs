using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapBar
{
	// Token: 0x0200005F RID: 95
	public class MapTimeControlVM : ViewModel
	{
		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x0002209C File Offset: 0x0002029C
		// (set) Token: 0x060006E0 RID: 1760 RVA: 0x000220A4 File Offset: 0x000202A4
		public bool IsInBattleSimulation { get; set; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x000220AD File Offset: 0x000202AD
		// (set) Token: 0x060006E2 RID: 1762 RVA: 0x000220B5 File Offset: 0x000202B5
		public bool IsInRecruitment { get; set; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x000220BE File Offset: 0x000202BE
		// (set) Token: 0x060006E4 RID: 1764 RVA: 0x000220C6 File Offset: 0x000202C6
		public bool IsEncyclopediaOpen { get; set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x000220CF File Offset: 0x000202CF
		// (set) Token: 0x060006E6 RID: 1766 RVA: 0x000220D7 File Offset: 0x000202D7
		public bool IsInArmyManagement { get; set; }

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x000220E0 File Offset: 0x000202E0
		// (set) Token: 0x060006E8 RID: 1768 RVA: 0x000220E8 File Offset: 0x000202E8
		public bool IsInTownManagement { get; set; }

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x000220F1 File Offset: 0x000202F1
		// (set) Token: 0x060006EA RID: 1770 RVA: 0x000220F9 File Offset: 0x000202F9
		public bool IsInHideoutTroopManage { get; set; }

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x00022102 File Offset: 0x00020302
		// (set) Token: 0x060006EC RID: 1772 RVA: 0x0002210A File Offset: 0x0002030A
		public bool IsInMap { get; set; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x00022113 File Offset: 0x00020313
		// (set) Token: 0x060006EE RID: 1774 RVA: 0x0002211B File Offset: 0x0002031B
		public bool IsInCampaignOptions { get; set; }

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x00022124 File Offset: 0x00020324
		// (set) Token: 0x060006F0 RID: 1776 RVA: 0x0002212C File Offset: 0x0002032C
		public bool IsEscapeMenuOpened { get; set; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x00022135 File Offset: 0x00020335
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x0002213D File Offset: 0x0002033D
		public bool IsMarriageOfferPopupActive { get; set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x00022146 File Offset: 0x00020346
		// (set) Token: 0x060006F4 RID: 1780 RVA: 0x0002214E File Offset: 0x0002034E
		public bool IsHeirSelectionPopupActive { get; set; }

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x00022157 File Offset: 0x00020357
		// (set) Token: 0x060006F6 RID: 1782 RVA: 0x0002215F File Offset: 0x0002035F
		public bool IsMapCheatsActive { get; set; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x00022168 File Offset: 0x00020368
		// (set) Token: 0x060006F8 RID: 1784 RVA: 0x00022170 File Offset: 0x00020370
		public bool IsMapIncidentActive { get; set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x00022179 File Offset: 0x00020379
		// (set) Token: 0x060006FA RID: 1786 RVA: 0x00022181 File Offset: 0x00020381
		public bool IsOverlayContextMenuEnabled { get; set; }

		// Token: 0x060006FB RID: 1787 RVA: 0x0002218C File Offset: 0x0002038C
		public MapTimeControlVM(Func<MapBarShortcuts> getMapBarShortcuts, Action onTimeFlowStateChange, Action onCameraResetted)
		{
			this._onTimeFlowStateChange = onTimeFlowStateChange;
			this._getMapBarShortcuts = getMapBarShortcuts;
			this._onCameraReset = onCameraResetted;
			this.IsCenterPanelEnabled = false;
			this._lastSetDate = CampaignTime.Zero;
			this.PlayHint = new BasicTooltipViewModel();
			this.FastForwardHint = new BasicTooltipViewModel();
			this.PauseHint = new BasicTooltipViewModel();
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
			CampaignEvents.OnSaveStartedEvent.AddNonSerializedListener(this, new Action(this.OnSaveStarted));
			CampaignEvents.OnSaveOverEvent.AddNonSerializedListener(this, new Action<bool, string>(this.OnSaveOver));
			this.RefreshValues();
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00022244 File Offset: 0x00020444
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._shortcuts = this._getMapBarShortcuts();
			if (Input.IsGamepadActive)
			{
				this.PlayHint.SetHintCallback(() => GameTexts.FindText("str_play", null).ToString());
				this.FastForwardHint.SetHintCallback(() => GameTexts.FindText("str_fast_forward", null).ToString());
				this.PauseHint.SetHintCallback(() => GameTexts.FindText("str_pause", null).ToString());
			}
			else
			{
				this.PlayHint.SetHintCallback(delegate
				{
					GameTexts.SetVariable("TEXT", GameTexts.FindText("str_play", null).ToString());
					GameTexts.SetVariable("HOTKEY", this._shortcuts.PlayHotkey);
					return GameTexts.FindText("str_hotkey_with_hint", null).ToString();
				});
				this.FastForwardHint.SetHintCallback(delegate
				{
					GameTexts.SetVariable("TEXT", GameTexts.FindText("str_fast_forward", null).ToString());
					GameTexts.SetVariable("HOTKEY", this._shortcuts.FastForwardHotkey);
					return GameTexts.FindText("str_hotkey_with_hint", null).ToString();
				});
				this.PauseHint.SetHintCallback(delegate
				{
					GameTexts.SetVariable("TEXT", GameTexts.FindText("str_pause", null).ToString());
					GameTexts.SetVariable("HOTKEY", this._shortcuts.PauseHotkey);
					return GameTexts.FindText("str_hotkey_with_hint", null).ToString();
				});
			}
			this.RefreshPausedText();
			this.Date = CampaignTime.Now.ToString();
			this._lastSetDate = CampaignTime.Now;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00022364 File Offset: 0x00020564
		private void RefreshPausedText()
		{
			MobileParty mainParty = MobileParty.MainParty;
			if (mainParty == null)
			{
				Debug.FailedAssert("Main party is null when refreshing pause text", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Map\\MapBar\\MapTimeControlVM.cs", "RefreshPausedText", 107);
				this.PausedText = GameTexts.FindText("str_paused_capital", null).ToString();
				return;
			}
			if (this.IsCurrentlyPausedOnMap)
			{
				this.PausedText = GameTexts.FindText("str_paused_capital", null).ToString();
				return;
			}
			if (!MobileParty.MainParty.IsTransitionInProgress)
			{
				this.PausedText = string.Empty;
				return;
			}
			if (mainParty.IsCurrentlyAtSea)
			{
				this.PausedText = new TextObject("{=g1op0Thi}DISEMBARKING", null).ToString();
				return;
			}
			this.PausedText = new TextObject("{=Lt0PzKHN}EMBARKING", null).ToString();
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00022414 File Offset: 0x00020614
		public override void OnFinalize()
		{
			base.OnFinalize();
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
			this._onTimeFlowStateChange = null;
			this._getMapBarShortcuts = null;
			this._onCameraReset = null;
			CampaignEvents.OnSaveStartedEvent.ClearListeners(this);
			CampaignEvents.OnSaveOverEvent.ClearListeners(this);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00022472 File Offset: 0x00020672
		private void OnGamepadActiveStateChanged()
		{
			this.RefreshValues();
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0002247A File Offset: 0x0002067A
		private void OnSaveStarted()
		{
			this._isSaving = true;
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00022483 File Offset: 0x00020683
		private void OnSaveOver(bool wasSuccessful, string saveName)
		{
			this._isSaving = false;
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0002248C File Offset: 0x0002068C
		public void Tick()
		{
			this.TimeFlowState = (int)Campaign.Current.GetSimplifiedTimeControlMode();
			this.IsCurrentlyPausedOnMap = (this.TimeFlowState == 0 || this.TimeFlowState == 6) && this.IsCenterPanelEnabled && !this.IsEscapeMenuOpened && !this._isSaving;
			this.IsCenterPanelEnabled = !this.IsInBattleSimulation && !this.IsInRecruitment && !this.IsEncyclopediaOpen && !this.IsInTownManagement && !this.IsInArmyManagement && this.IsInMap && !this.IsInCampaignOptions && !this.IsInHideoutTroopManage && !this.IsMarriageOfferPopupActive && !this.IsHeirSelectionPopupActive && !this.IsMapCheatsActive && !this.IsMapIncidentActive && !this.IsOverlayContextMenuEnabled;
			if (MobileParty.MainParty.IsTransitionInProgress != this._mainPartyPreviousTransitioning)
			{
				this._mainPartyPreviousTransitioning = MobileParty.MainParty.IsTransitionInProgress;
				this.RefreshPausedText();
			}
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00022578 File Offset: 0x00020778
		public void Refresh()
		{
			if (!this._lastSetDate.StringSameAs(CampaignTime.Now))
			{
				this.Date = CampaignTime.Now.ToString();
				this._lastSetDate = CampaignTime.Now;
			}
			this.Time = CampaignTime.Now.ToHours % (double)CampaignTime.HoursInDay;
			this.TimeOfDayHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTimeOfDayAndResetCameraTooltip());
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x000225FF File Offset: 0x000207FF
		private void SetTimeSpeed(int speed)
		{
			Campaign.Current.SetTimeSpeed(speed);
			this._onTimeFlowStateChange();
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00022618 File Offset: 0x00020818
		public void ExecuteTimeControlChange(int selectedTimeSpeed)
		{
			if (Campaign.Current.CurrentMenuContext == null || (Campaign.Current.CurrentMenuContext.GameMenu.IsWaitActive && !Campaign.Current.TimeControlModeLock))
			{
				int num = selectedTimeSpeed;
				if (this._timeFlowState == 3 && num == 2)
				{
					num = 4;
				}
				else if (this._timeFlowState == 4 && num == 1)
				{
					num = 3;
				}
				else if (this._timeFlowState == 2 && num == 0)
				{
					num = 6;
				}
				if (num != this._timeFlowState)
				{
					this.TimeFlowState = num;
					this.SetTimeSpeed(selectedTimeSpeed);
				}
			}
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0002269C File Offset: 0x0002089C
		public void ExecuteResetCamera()
		{
			this._onCameraReset();
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x000226A9 File Offset: 0x000208A9
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x000226B1 File Offset: 0x000208B1
		[DataSourceProperty]
		public BasicTooltipViewModel TimeOfDayHint
		{
			get
			{
				return this._timeOfDayHint;
			}
			set
			{
				if (value != this._timeOfDayHint)
				{
					this._timeOfDayHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "TimeOfDayHint");
				}
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x000226CF File Offset: 0x000208CF
		// (set) Token: 0x0600070A RID: 1802 RVA: 0x000226D7 File Offset: 0x000208D7
		[DataSourceProperty]
		public bool IsCurrentlyPausedOnMap
		{
			get
			{
				return this._isCurrentlyPausedOnMap;
			}
			set
			{
				if (value != this._isCurrentlyPausedOnMap)
				{
					this._isCurrentlyPausedOnMap = value;
					base.OnPropertyChangedWithValue(value, "IsCurrentlyPausedOnMap");
					this.RefreshPausedText();
				}
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x000226FB File Offset: 0x000208FB
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x00022703 File Offset: 0x00020903
		[DataSourceProperty]
		public bool IsCenterPanelEnabled
		{
			get
			{
				return this._isCenterPanelEnabled;
			}
			set
			{
				if (value != this._isCenterPanelEnabled)
				{
					this._isCenterPanelEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsCenterPanelEnabled");
				}
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x00022721 File Offset: 0x00020921
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x00022729 File Offset: 0x00020929
		[DataSourceProperty]
		public double Time
		{
			get
			{
				return this._time;
			}
			set
			{
				if (this._time != value)
				{
					this._time = value;
					base.OnPropertyChangedWithValue(value, "Time");
				}
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x00022747 File Offset: 0x00020947
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x0002274F File Offset: 0x0002094F
		[DataSourceProperty]
		public string PausedText
		{
			get
			{
				return this._pausedText;
			}
			set
			{
				if (this._pausedText != value)
				{
					this._pausedText = value;
					base.OnPropertyChangedWithValue<string>(value, "PausedText");
				}
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x00022772 File Offset: 0x00020972
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x0002277A File Offset: 0x0002097A
		[DataSourceProperty]
		public string Date
		{
			get
			{
				return this._date;
			}
			set
			{
				if (value != this._date)
				{
					this._date = value;
					base.OnPropertyChangedWithValue<string>(value, "Date");
				}
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0002279D File Offset: 0x0002099D
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x000227A5 File Offset: 0x000209A5
		[DataSourceProperty]
		public int TimeFlowState
		{
			get
			{
				return this._timeFlowState;
			}
			set
			{
				if (value != this._timeFlowState)
				{
					this._timeFlowState = value;
					base.OnPropertyChangedWithValue(value, "TimeFlowState");
				}
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x000227C3 File Offset: 0x000209C3
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x000227CB File Offset: 0x000209CB
		[DataSourceProperty]
		public BasicTooltipViewModel PauseHint
		{
			get
			{
				return this._pauseHint;
			}
			set
			{
				if (value != this._pauseHint)
				{
					this._pauseHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "PauseHint");
				}
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x000227E9 File Offset: 0x000209E9
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x000227F1 File Offset: 0x000209F1
		[DataSourceProperty]
		public BasicTooltipViewModel PlayHint
		{
			get
			{
				return this._playHint;
			}
			set
			{
				if (value != this._playHint)
				{
					this._playHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "PlayHint");
				}
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x0002280F File Offset: 0x00020A0F
		// (set) Token: 0x0600071A RID: 1818 RVA: 0x00022817 File Offset: 0x00020A17
		[DataSourceProperty]
		public BasicTooltipViewModel FastForwardHint
		{
			get
			{
				return this._fastForwardHint;
			}
			set
			{
				if (value != this._fastForwardHint)
				{
					this._fastForwardHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "FastForwardHint");
				}
			}
		}

		// Token: 0x0400030B RID: 779
		private bool _mainPartyPreviousTransitioning;

		// Token: 0x0400030C RID: 780
		private Action _onTimeFlowStateChange;

		// Token: 0x0400030D RID: 781
		private Func<MapBarShortcuts> _getMapBarShortcuts;

		// Token: 0x0400030E RID: 782
		private MapBarShortcuts _shortcuts;

		// Token: 0x0400030F RID: 783
		private Action _onCameraReset;

		// Token: 0x04000310 RID: 784
		private CampaignTime _lastSetDate;

		// Token: 0x04000311 RID: 785
		private bool _isSaving;

		// Token: 0x04000312 RID: 786
		private int _timeFlowState = -1;

		// Token: 0x04000313 RID: 787
		private double _time;

		// Token: 0x04000314 RID: 788
		private string _date;

		// Token: 0x04000315 RID: 789
		private string _pausedText;

		// Token: 0x04000316 RID: 790
		private bool _isCurrentlyPausedOnMap;

		// Token: 0x04000317 RID: 791
		private bool _isCenterPanelEnabled;

		// Token: 0x04000318 RID: 792
		private BasicTooltipViewModel _pauseHint;

		// Token: 0x04000319 RID: 793
		private BasicTooltipViewModel _playHint;

		// Token: 0x0400031A RID: 794
		private BasicTooltipViewModel _fastForwardHint;

		// Token: 0x0400031B RID: 795
		private BasicTooltipViewModel _timeOfDayHint;
	}
}
