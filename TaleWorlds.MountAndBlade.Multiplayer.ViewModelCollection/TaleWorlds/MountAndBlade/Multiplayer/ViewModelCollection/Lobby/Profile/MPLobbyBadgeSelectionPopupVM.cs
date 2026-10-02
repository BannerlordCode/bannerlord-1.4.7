using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x02000035 RID: 53
	public class MPLobbyBadgeSelectionPopupVM : ViewModel
	{
		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x0001175D File Offset: 0x0000F95D
		// (set) Token: 0x060004EA RID: 1258 RVA: 0x00011765 File Offset: 0x0000F965
		public List<LobbyNotification> ActiveNotifications { get; private set; }

		// Token: 0x060004EB RID: 1259 RVA: 0x0001176E File Offset: 0x0000F96E
		public MPLobbyBadgeSelectionPopupVM(Action onBadgeNotificationRead, Action onBadgeSelectionUpdated, Action<MPLobbyAchievementBadgeGroupVM> onBadgeProgressInfoRequested)
		{
			this._onBadgeNotificationRead = onBadgeNotificationRead;
			this._onBadgeSelectionUpdated = onBadgeSelectionUpdated;
			this._onBadgeProgressInfoRequested = onBadgeProgressInfoRequested;
			this.ActiveNotifications = new List<LobbyNotification>();
			this.Badges = new MBBindingList<MPLobbyBadgeItemVM>();
			this.AchivementBadgeGroups = new MBBindingList<MPLobbyAchievementBadgeGroupVM>();
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x000117AC File Offset: 0x0000F9AC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CloseText = GameTexts.FindText("str_close", null).ToString();
			this.BadgesText = new TextObject("{=nqYaiEo2}My Badges", null).ToString();
			this.SpecialBadgesText = new TextObject("{=yI9EV0II}Special Badges", null).ToString();
			this.AchievementBadgesText = new TextObject("{=n6yb5VCI}Achievement Badges", null).ToString();
			this.AchivementBadgeGroups.ApplyActionOnAllItems(delegate(MPLobbyAchievementBadgeGroupVM g)
			{
				g.RefreshValues();
			});
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00011841 File Offset: 0x0000FA41
		public void RefreshPlayerData(PlayerData playerData)
		{
			this.UpdateBadges(false);
			this.UpdateBadgeSelection();
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00011850 File Offset: 0x0000FA50
		public void RefreshKeyBindings(HotKey inspectProgressKey)
		{
			this._inspectProgressKey = inspectProgressKey;
			foreach (MPLobbyAchievementBadgeGroupVM mplobbyAchievementBadgeGroupVM in this.AchivementBadgeGroups)
			{
				mplobbyAchievementBadgeGroupVM.RefreshKeyBindings(inspectProgressKey);
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x000118A4 File Offset: 0x0000FAA4
		public async void UpdateBadges(bool shouldClear = false)
		{
			Badge[] array = await NetworkMain.GameClient.GetPlayerBadges();
			this._playerEarnedBadges = array;
			if (shouldClear)
			{
				this.Badges.Clear();
			}
			if (!this.Badges.Any<MPLobbyBadgeItemVM>((MPLobbyBadgeItemVM b) => b.Badge == null))
			{
				this.Badges.Add(new MPLobbyBadgeItemVM(null, new Action(this.UpdateBadgeSelection), (Badge b) => true, new Action<MPLobbyBadgeItemVM>(this.OnBadgeInspected)));
			}
			if (BadgeManager.Badges != null)
			{
				using (List<Badge>.Enumerator enumerator = BadgeManager.Badges.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Badge badge = enumerator.Current;
						if ((badge.IsActive && !badge.IsVisibleOnlyWhenEarned) || this._playerEarnedBadges.Contains(badge))
						{
							if (badge.GroupId != null)
							{
								MPLobbyAchievementBadgeGroupVM mplobbyAchievementBadgeGroupVM = this.AchivementBadgeGroups.FirstOrDefault<MPLobbyAchievementBadgeGroupVM>((MPLobbyAchievementBadgeGroupVM g) => g.GroupID == badge.GroupId);
								if (mplobbyAchievementBadgeGroupVM == null)
								{
									mplobbyAchievementBadgeGroupVM = new MPLobbyAchievementBadgeGroupVM(badge.GroupId, this._onBadgeProgressInfoRequested);
									mplobbyAchievementBadgeGroupVM.RefreshKeyBindings(this._inspectProgressKey);
									this.AchivementBadgeGroups.Add(mplobbyAchievementBadgeGroupVM);
									mplobbyAchievementBadgeGroupVM.OnGroupBadgeAdded(new MPLobbyBadgeItemVM(badge, new Action(this.UpdateBadgeSelection), new Func<Badge, bool>(this.HasPlayerEarnedBadge), new Action<MPLobbyBadgeItemVM>(this.OnBadgeInspected)));
								}
								else
								{
									MPLobbyBadgeItemVM mplobbyBadgeItemVM = mplobbyAchievementBadgeGroupVM.Badges.FirstOrDefault<MPLobbyBadgeItemVM>((MPLobbyBadgeItemVM b) => b.Badge == badge);
									if (mplobbyBadgeItemVM == null)
									{
										mplobbyAchievementBadgeGroupVM.OnGroupBadgeAdded(new MPLobbyBadgeItemVM(badge, new Action(this.UpdateBadgeSelection), new Func<Badge, bool>(this.HasPlayerEarnedBadge), new Action<MPLobbyBadgeItemVM>(this.OnBadgeInspected)));
									}
									else
									{
										mplobbyBadgeItemVM.UpdateWith(badge);
									}
								}
							}
							else
							{
								MPLobbyBadgeItemVM mplobbyBadgeItemVM2 = this.Badges.SingleOrDefault<MPLobbyBadgeItemVM>((MPLobbyBadgeItemVM b) => b.Badge == badge);
								if (mplobbyBadgeItemVM2 == null)
								{
									this.Badges.Add(new MPLobbyBadgeItemVM(badge, new Action(this.UpdateBadgeSelection), new Func<Badge, bool>(this.HasPlayerEarnedBadge), new Action<MPLobbyBadgeItemVM>(this.OnBadgeInspected)));
								}
								else
								{
									mplobbyBadgeItemVM2.UpdateWith(badge);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x000118E8 File Offset: 0x0000FAE8
		public void UpdateBadgeSelection()
		{
			foreach (MPLobbyBadgeItemVM mplobbyBadgeItemVM in this.Badges)
			{
				mplobbyBadgeItemVM.UpdateIsSelected();
			}
			foreach (MPLobbyAchievementBadgeGroupVM mplobbyAchievementBadgeGroupVM in this.AchivementBadgeGroups)
			{
				mplobbyAchievementBadgeGroupVM.UpdateBadgeSelection();
			}
			Action onBadgeSelectionUpdated = this._onBadgeSelectionUpdated;
			if (onBadgeSelectionUpdated == null)
			{
				return;
			}
			onBadgeSelectionUpdated();
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001197C File Offset: 0x0000FB7C
		private bool HasPlayerEarnedBadge(Badge badge)
		{
			Badge[] playerEarnedBadges = this._playerEarnedBadges;
			return playerEarnedBadges != null && playerEarnedBadges.Contains(badge);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00011990 File Offset: 0x0000FB90
		public void OnNotificationReceived(LobbyNotification notification)
		{
			string badgeID = notification.Parameters["badge_id"];
			IEnumerable<MPLobbyBadgeItemVM> badges = this.Badges;
			Func<MPLobbyBadgeItemVM, bool> <>9__0;
			Func<MPLobbyBadgeItemVM, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = (MPLobbyBadgeItemVM badge) => badge.BadgeId == badgeID);
			}
			foreach (MPLobbyBadgeItemVM mplobbyBadgeItemVM in badges.Where<MPLobbyBadgeItemVM>(func))
			{
				mplobbyBadgeItemVM.HasNotification = true;
			}
			this.ActiveNotifications.Add(notification);
			this.RefreshNotificationInfo();
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00011A30 File Offset: 0x0000FC30
		public void OnBadgeInspected(MPLobbyBadgeItemVM badge)
		{
			this.InspectedBadge = badge;
			if (badge != null)
			{
				string badgeID = badge.BadgeId;
				foreach (LobbyNotification lobbyNotification in this.ActiveNotifications.Where<LobbyNotification>((LobbyNotification n) => n.Parameters["badge_id"] == badgeID))
				{
					NetworkMain.GameClient.MarkNotificationAsRead(lobbyNotification.Id);
				}
				this.ActiveNotifications.RemoveAll((LobbyNotification n) => n.Parameters["badge_id"] == badgeID);
				this.RefreshNotificationInfo();
				Action onBadgeNotificationRead = this._onBadgeNotificationRead;
				if (onBadgeNotificationRead == null)
				{
					return;
				}
				onBadgeNotificationRead();
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00011AE4 File Offset: 0x0000FCE4
		private void RefreshNotificationInfo()
		{
			this.HasNotifications = this.ActiveNotifications.Count > 0;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00011AFA File Offset: 0x0000FCFA
		public void Open()
		{
			this.IsEnabled = true;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00011B03 File Offset: 0x0000FD03
		public void ExecuteClosePopup()
		{
			this.IsEnabled = false;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00011B0C File Offset: 0x0000FD0C
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey == null)
			{
				return;
			}
			cancelInputKey.OnFinalize();
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00011B24 File Offset: 0x0000FD24
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x00011B33 File Offset: 0x0000FD33
		// (set) Token: 0x060004FA RID: 1274 RVA: 0x00011B3B File Offset: 0x0000FD3B
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChanged("CancelInputKey");
				}
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x00011B58 File Offset: 0x0000FD58
		// (set) Token: 0x060004FC RID: 1276 RVA: 0x00011B60 File Offset: 0x0000FD60
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

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x00011B7E File Offset: 0x0000FD7E
		// (set) Token: 0x060004FE RID: 1278 RVA: 0x00011B86 File Offset: 0x0000FD86
		[DataSourceProperty]
		public bool HasNotifications
		{
			get
			{
				return this._hasNotifications;
			}
			set
			{
				if (value != this._hasNotifications)
				{
					this._hasNotifications = value;
					base.OnPropertyChangedWithValue(value, "HasNotifications");
				}
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x00011BA4 File Offset: 0x0000FDA4
		// (set) Token: 0x06000500 RID: 1280 RVA: 0x00011BAC File Offset: 0x0000FDAC
		[DataSourceProperty]
		public string CloseText
		{
			get
			{
				return this._closeText;
			}
			set
			{
				if (value != this._closeText)
				{
					this._closeText = value;
					base.OnPropertyChangedWithValue<string>(value, "CloseText");
				}
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x00011BCF File Offset: 0x0000FDCF
		// (set) Token: 0x06000502 RID: 1282 RVA: 0x00011BD7 File Offset: 0x0000FDD7
		[DataSourceProperty]
		public string BadgesText
		{
			get
			{
				return this._badgesText;
			}
			set
			{
				if (value != this._badgesText)
				{
					this._badgesText = value;
					base.OnPropertyChangedWithValue<string>(value, "BadgesText");
				}
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x00011BFA File Offset: 0x0000FDFA
		// (set) Token: 0x06000504 RID: 1284 RVA: 0x00011C02 File Offset: 0x0000FE02
		[DataSourceProperty]
		public string SpecialBadgesText
		{
			get
			{
				return this._specialBadgesText;
			}
			set
			{
				if (value != this._specialBadgesText)
				{
					this._specialBadgesText = value;
					base.OnPropertyChangedWithValue<string>(value, "SpecialBadgesText");
				}
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x00011C25 File Offset: 0x0000FE25
		// (set) Token: 0x06000506 RID: 1286 RVA: 0x00011C2D File Offset: 0x0000FE2D
		[DataSourceProperty]
		public string AchievementBadgesText
		{
			get
			{
				return this._achievementBadgesText;
			}
			set
			{
				if (value != this._achievementBadgesText)
				{
					this._achievementBadgesText = value;
					base.OnPropertyChangedWithValue<string>(value, "AchievementBadgesText");
				}
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x00011C50 File Offset: 0x0000FE50
		// (set) Token: 0x06000508 RID: 1288 RVA: 0x00011C58 File Offset: 0x0000FE58
		[DataSourceProperty]
		public MBBindingList<MPLobbyBadgeItemVM> Badges
		{
			get
			{
				return this._badges;
			}
			set
			{
				if (value != this._badges)
				{
					this._badges = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyBadgeItemVM>>(value, "Badges");
				}
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x00011C76 File Offset: 0x0000FE76
		// (set) Token: 0x0600050A RID: 1290 RVA: 0x00011C7E File Offset: 0x0000FE7E
		[DataSourceProperty]
		public MBBindingList<MPLobbyAchievementBadgeGroupVM> AchivementBadgeGroups
		{
			get
			{
				return this._achievementBadgeGroups;
			}
			set
			{
				if (value != this._achievementBadgeGroups)
				{
					this._achievementBadgeGroups = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyAchievementBadgeGroupVM>>(value, "AchivementBadgeGroups");
				}
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x00011C9C File Offset: 0x0000FE9C
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x00011CA4 File Offset: 0x0000FEA4
		[DataSourceProperty]
		public MPLobbyBadgeItemVM InspectedBadge
		{
			get
			{
				return this._inspectedBadge;
			}
			set
			{
				if (value != this._inspectedBadge)
				{
					this._inspectedBadge = value;
					base.OnPropertyChangedWithValue<MPLobbyBadgeItemVM>(value, "InspectedBadge");
				}
			}
		}

		// Token: 0x04000260 RID: 608
		private Badge[] _playerEarnedBadges;

		// Token: 0x04000262 RID: 610
		private Action _onBadgeNotificationRead;

		// Token: 0x04000263 RID: 611
		private Action<MPLobbyAchievementBadgeGroupVM> _onBadgeProgressInfoRequested;

		// Token: 0x04000264 RID: 612
		private Action _onBadgeSelectionUpdated;

		// Token: 0x04000265 RID: 613
		private HotKey _inspectProgressKey;

		// Token: 0x04000266 RID: 614
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000267 RID: 615
		private bool _isEnabled;

		// Token: 0x04000268 RID: 616
		private bool _hasNotifications;

		// Token: 0x04000269 RID: 617
		private string _closeText;

		// Token: 0x0400026A RID: 618
		private string _badgesText;

		// Token: 0x0400026B RID: 619
		private string _specialBadgesText;

		// Token: 0x0400026C RID: 620
		private string _achievementBadgesText;

		// Token: 0x0400026D RID: 621
		private MBBindingList<MPLobbyBadgeItemVM> _badges;

		// Token: 0x0400026E RID: 622
		private MBBindingList<MPLobbyAchievementBadgeGroupVM> _achievementBadgeGroups;

		// Token: 0x0400026F RID: 623
		private MPLobbyBadgeItemVM _inspectedBadge;
	}
}
