using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x02000033 RID: 51
	public class MPLobbyAchievementBadgeGroupVM : ViewModel
	{
		// Token: 0x060004BA RID: 1210 RVA: 0x00011013 File Offset: 0x0000F213
		public MPLobbyAchievementBadgeGroupVM(string groupID, Action<MPLobbyAchievementBadgeGroupVM> onBadgeProgressInfoRequested)
		{
			this._onBadgeProgressInfoRequested = onBadgeProgressInfoRequested;
			this.GroupID = groupID;
			this.Badges = new MBBindingList<MPLobbyBadgeItemVM>();
			this.RefreshValues();
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001103A File Offset: 0x0000F23A
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ProgressCompletedText = new TextObject("{=vlACTion}You've unlocked all badges!", null).ToString();
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00011058 File Offset: 0x0000F258
		public void RefreshKeyBindings(HotKey inspectProgressKey)
		{
			this._inspectProgressKey = inspectProgressKey;
			foreach (MPLobbyBadgeItemVM mplobbyBadgeItemVM in this.Badges)
			{
				mplobbyBadgeItemVM.RefreshKeyBindings(inspectProgressKey);
			}
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x000110AC File Offset: 0x0000F2AC
		public void OnGroupBadgeAdded(MPLobbyBadgeItemVM badgeItem)
		{
			if (this.ShownBadgeItem == null)
			{
				this.ShownBadgeItem = badgeItem;
			}
			else if (badgeItem.IsEarned && badgeItem.Badge.Index > this.ShownBadgeItem.Badge.Index)
			{
				this.ShownBadgeItem = badgeItem;
			}
			badgeItem.SetGroup(this, this._onBadgeProgressInfoRequested);
			this._totalBadgeCount++;
			if (badgeItem.IsEarned)
			{
				this._unlockedBadgeCount++;
			}
			this.IsProgressComplete = this._totalBadgeCount == this._unlockedBadgeCount;
			ConditionalBadge conditionalBadge;
			if ((conditionalBadge = badgeItem.Badge as ConditionalBadge) != null && conditionalBadge.BadgeConditions.Count > 0 && !conditionalBadge.IsTimed)
			{
				BadgeCondition badgeCondition = conditionalBadge.BadgeConditions[0];
				string text;
				int num;
				if (badgeCondition.Parameters.TryGetValue("min_value", out text) && int.TryParse(text, out num))
				{
					int num2 = NetworkMain.GameClient.PlayerData.GetBadgeConditionNumericValue(badgeCondition);
					if (badgeCondition.StringId.Equals("Playtime"))
					{
						num /= 3600;
						num2 /= 3600;
					}
					this.TotalProgress = Math.Max(this.TotalProgress, num);
					this.CurrentProgress = num2;
				}
				else
				{
					this.SetProgressAsCompleted();
				}
			}
			else
			{
				this.SetProgressAsCompleted();
			}
			badgeItem.RefreshKeyBindings(this._inspectProgressKey);
			this.Badges.Add(badgeItem);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0001120C File Offset: 0x0000F40C
		private void SetProgressAsCompleted()
		{
			this.TotalProgress = 1;
			this.CurrentProgress = 1;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0001121C File Offset: 0x0000F41C
		public void UpdateBadgeSelection()
		{
			foreach (MPLobbyBadgeItemVM mplobbyBadgeItemVM in this.Badges)
			{
				mplobbyBadgeItemVM.UpdateIsSelected();
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x00011268 File Offset: 0x0000F468
		// (set) Token: 0x060004C1 RID: 1217 RVA: 0x00011270 File Offset: 0x0000F470
		[DataSourceProperty]
		public bool IsProgressComplete
		{
			get
			{
				return this._isProgressComplete;
			}
			set
			{
				if (value != this._isProgressComplete)
				{
					this._isProgressComplete = value;
					base.OnPropertyChangedWithValue(value, "IsProgressComplete");
					if (value)
					{
						this.SetProgressAsCompleted();
					}
				}
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x00011297 File Offset: 0x0000F497
		// (set) Token: 0x060004C3 RID: 1219 RVA: 0x0001129F File Offset: 0x0000F49F
		[DataSourceProperty]
		public string ProgressCompletedText
		{
			get
			{
				return this._progressCompletedText;
			}
			set
			{
				if (value != this._progressCompletedText)
				{
					this._progressCompletedText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProgressCompletedText");
				}
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x000112C2 File Offset: 0x0000F4C2
		// (set) Token: 0x060004C5 RID: 1221 RVA: 0x000112CA File Offset: 0x0000F4CA
		[DataSourceProperty]
		public int CurrentProgress
		{
			get
			{
				return this._currentProgress;
			}
			set
			{
				if (value != this._currentProgress)
				{
					this._currentProgress = value;
					base.OnPropertyChangedWithValue(value, "CurrentProgress");
				}
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x000112E8 File Offset: 0x0000F4E8
		// (set) Token: 0x060004C7 RID: 1223 RVA: 0x000112F0 File Offset: 0x0000F4F0
		[DataSourceProperty]
		public int TotalProgress
		{
			get
			{
				return this._totalProgress;
			}
			set
			{
				if (value != this._totalProgress)
				{
					this._totalProgress = value;
					base.OnPropertyChangedWithValue(value, "TotalProgress");
				}
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x0001130E File Offset: 0x0000F50E
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x00011316 File Offset: 0x0000F516
		[DataSourceProperty]
		public MPLobbyBadgeItemVM ShownBadgeItem
		{
			get
			{
				return this._shownBadgeItem;
			}
			set
			{
				if (value != this._shownBadgeItem)
				{
					this._shownBadgeItem = value;
					base.OnPropertyChangedWithValue<MPLobbyBadgeItemVM>(value, "ShownBadgeItem");
				}
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x00011334 File Offset: 0x0000F534
		// (set) Token: 0x060004CB RID: 1227 RVA: 0x0001133C File Offset: 0x0000F53C
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

		// Token: 0x04000247 RID: 583
		public readonly string GroupID;

		// Token: 0x04000248 RID: 584
		private readonly Action<MPLobbyAchievementBadgeGroupVM> _onBadgeProgressInfoRequested;

		// Token: 0x04000249 RID: 585
		private int _unlockedBadgeCount;

		// Token: 0x0400024A RID: 586
		private int _totalBadgeCount;

		// Token: 0x0400024B RID: 587
		private const string PlaytimeConditionID = "Playtime";

		// Token: 0x0400024C RID: 588
		private HotKey _inspectProgressKey;

		// Token: 0x0400024D RID: 589
		private bool _isProgressComplete;

		// Token: 0x0400024E RID: 590
		private string _progressCompletedText;

		// Token: 0x0400024F RID: 591
		private int _currentProgress;

		// Token: 0x04000250 RID: 592
		private int _totalProgress;

		// Token: 0x04000251 RID: 593
		private MPLobbyBadgeItemVM _shownBadgeItem;

		// Token: 0x04000252 RID: 594
		private MBBindingList<MPLobbyBadgeItemVM> _badges;
	}
}
