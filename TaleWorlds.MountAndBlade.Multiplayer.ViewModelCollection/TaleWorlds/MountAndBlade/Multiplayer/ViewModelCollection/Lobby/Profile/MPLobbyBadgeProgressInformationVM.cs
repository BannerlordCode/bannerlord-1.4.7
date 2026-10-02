using System;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x02000034 RID: 52
	public class MPLobbyBadgeProgressInformationVM : ViewModel
	{
		// Token: 0x060004CC RID: 1228 RVA: 0x0001135C File Offset: 0x0000F55C
		public MPLobbyBadgeProgressInformationVM(Func<string> getExitText)
		{
			this._getExitText = getExitText;
			this.AvailableBadgeIDs = new MBBindingList<StringPairItemVM>();
			this.ShownBadgeCount = 5;
			for (int i = 0; i < this.ShownBadgeCount; i++)
			{
				this.AvailableBadgeIDs.Add(new StringPairItemVM(string.Empty, string.Empty, null));
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x000113B4 File Offset: 0x0000F5B4
		public void OpenWith(MPLobbyAchievementBadgeGroupVM badgeGroup)
		{
			this.BadgeGroup = badgeGroup;
			this.TitleText = (this.BadgeGroup.ShownBadgeItem.Badge as ConditionalBadge).BadgeConditions[0].Description.ToString();
			this._shownBadgeIndexOffset = 0;
			this.RefreshShownBadges();
			Func<string> getExitText = this._getExitText;
			this.ClickToCloseText = ((getExitText != null) ? getExitText() : null);
			this.IsEnabled = true;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00011424 File Offset: 0x0000F624
		private void RefreshShownBadges()
		{
			int num = this.BadgeGroup.Badges.IndexOf(this.BadgeGroup.ShownBadgeItem) + this._shownBadgeIndexOffset;
			int num2 = 0;
			int num3 = this.ShownBadgeCount / 2;
			for (int i = num - num3; i <= num + num3; i++)
			{
				if (i >= 0 && i < this.BadgeGroup.Badges.Count)
				{
					MPLobbyBadgeItemVM mplobbyBadgeItemVM = this.BadgeGroup.Badges[i];
					this.AvailableBadgeIDs[num2].Value = mplobbyBadgeItemVM.BadgeId;
					this.AvailableBadgeIDs[num2].Definition = mplobbyBadgeItemVM.Name;
				}
				else
				{
					this.AvailableBadgeIDs[num2].Value = string.Empty;
					this.AvailableBadgeIDs[num2].Definition = string.Empty;
				}
				num2++;
			}
			this.CanIncreaseBadgeIndices = this.BadgeGroup.Badges.IndexOf(this.BadgeGroup.Badges[num]) < this.BadgeGroup.Badges.Count - 1;
			this.CanDecreaseBadgeIndices = num > 0;
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00011546 File Offset: 0x0000F746
		public void ExecuteClosePopup()
		{
			this.BadgeGroup = null;
			this.IsEnabled = false;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00011556 File Offset: 0x0000F756
		public void ExecuteIncreaseActiveBadgeIndices()
		{
			if (this.CanIncreaseBadgeIndices)
			{
				this._shownBadgeIndexOffset++;
				this.RefreshShownBadges();
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00011574 File Offset: 0x0000F774
		public void ExecuteDecreaseActiveBadgeIndices()
		{
			if (this.CanDecreaseBadgeIndices)
			{
				this._shownBadgeIndexOffset--;
				this.RefreshShownBadges();
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00011592 File Offset: 0x0000F792
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM previousTabInputKey = this.PreviousTabInputKey;
			if (previousTabInputKey != null)
			{
				previousTabInputKey.OnFinalize();
			}
			InputKeyItemVM nextTabInputKey = this.NextTabInputKey;
			if (nextTabInputKey == null)
			{
				return;
			}
			nextTabInputKey.OnFinalize();
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x000115BB File Offset: 0x0000F7BB
		public void SetPreviousTabInputKey(HotKey hotKey)
		{
			this.PreviousTabInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x000115CA File Offset: 0x0000F7CA
		public void SetNextTabInputKey(HotKey hotKey)
		{
			this.NextTabInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x000115D9 File Offset: 0x0000F7D9
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x000115E1 File Offset: 0x0000F7E1
		[DataSourceProperty]
		public InputKeyItemVM PreviousTabInputKey
		{
			get
			{
				return this._previousTabInputKey;
			}
			set
			{
				if (value != this._previousTabInputKey)
				{
					this._previousTabInputKey = value;
					base.OnPropertyChanged("PreviousTabInputKey");
				}
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x000115FE File Offset: 0x0000F7FE
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x00011606 File Offset: 0x0000F806
		[DataSourceProperty]
		public InputKeyItemVM NextTabInputKey
		{
			get
			{
				return this._nextTabInputKey;
			}
			set
			{
				if (value != this._nextTabInputKey)
				{
					this._nextTabInputKey = value;
					base.OnPropertyChanged("NextTabInputKey");
				}
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00011623 File Offset: 0x0000F823
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x0001162B File Offset: 0x0000F82B
		[DataSourceProperty]
		public int ShownBadgeCount
		{
			get
			{
				return this._shownBadgeCount;
			}
			set
			{
				if (value != this._shownBadgeCount)
				{
					this._shownBadgeCount = value;
					base.OnPropertyChangedWithValue(value, "ShownBadgeCount");
				}
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x00011649 File Offset: 0x0000F849
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x00011651 File Offset: 0x0000F851
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

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x0001166F File Offset: 0x0000F86F
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x00011677 File Offset: 0x0000F877
		[DataSourceProperty]
		public bool CanIncreaseBadgeIndices
		{
			get
			{
				return this._canIncreaseBadgeIndices;
			}
			set
			{
				if (value != this._canIncreaseBadgeIndices)
				{
					this._canIncreaseBadgeIndices = value;
					base.OnPropertyChangedWithValue(value, "CanIncreaseBadgeIndices");
				}
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x00011695 File Offset: 0x0000F895
		// (set) Token: 0x060004E0 RID: 1248 RVA: 0x0001169D File Offset: 0x0000F89D
		[DataSourceProperty]
		public bool CanDecreaseBadgeIndices
		{
			get
			{
				return this._canDecreaseBadgeIndices;
			}
			set
			{
				if (value != this._canDecreaseBadgeIndices)
				{
					this._canDecreaseBadgeIndices = value;
					base.OnPropertyChangedWithValue(value, "CanDecreaseBadgeIndices");
				}
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x000116BB File Offset: 0x0000F8BB
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x000116C3 File Offset: 0x0000F8C3
		[DataSourceProperty]
		public string ClickToCloseText
		{
			get
			{
				return this._clickToCloseText;
			}
			set
			{
				if (value != this._clickToCloseText)
				{
					this._clickToCloseText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClickToCloseText");
				}
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x000116E6 File Offset: 0x0000F8E6
		// (set) Token: 0x060004E4 RID: 1252 RVA: 0x000116EE File Offset: 0x0000F8EE
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x00011711 File Offset: 0x0000F911
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x00011719 File Offset: 0x0000F919
		[DataSourceProperty]
		public MPLobbyAchievementBadgeGroupVM BadgeGroup
		{
			get
			{
				return this._badgeGroup;
			}
			set
			{
				if (value != this._badgeGroup)
				{
					this._badgeGroup = value;
					base.OnPropertyChangedWithValue<MPLobbyAchievementBadgeGroupVM>(value, "BadgeGroup");
				}
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x00011737 File Offset: 0x0000F937
		// (set) Token: 0x060004E8 RID: 1256 RVA: 0x0001173F File Offset: 0x0000F93F
		[DataSourceProperty]
		public MBBindingList<StringPairItemVM> AvailableBadgeIDs
		{
			get
			{
				return this._availableBadgeIDs;
			}
			set
			{
				if (value != this._availableBadgeIDs)
				{
					this._availableBadgeIDs = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemVM>>(value, "AvailableBadgeIDs");
				}
			}
		}

		// Token: 0x04000253 RID: 595
		private int _shownBadgeIndexOffset;

		// Token: 0x04000254 RID: 596
		private const int MaxShownBadgeCount = 5;

		// Token: 0x04000255 RID: 597
		private readonly Func<string> _getExitText;

		// Token: 0x04000256 RID: 598
		private InputKeyItemVM _previousTabInputKey;

		// Token: 0x04000257 RID: 599
		private InputKeyItemVM _nextTabInputKey;

		// Token: 0x04000258 RID: 600
		private int _shownBadgeCount;

		// Token: 0x04000259 RID: 601
		private bool _isEnabled;

		// Token: 0x0400025A RID: 602
		private bool _canIncreaseBadgeIndices;

		// Token: 0x0400025B RID: 603
		private bool _canDecreaseBadgeIndices;

		// Token: 0x0400025C RID: 604
		private string _clickToCloseText;

		// Token: 0x0400025D RID: 605
		private string _titleText;

		// Token: 0x0400025E RID: 606
		private MPLobbyAchievementBadgeGroupVM _badgeGroup;

		// Token: 0x0400025F RID: 607
		private MBBindingList<StringPairItemVM> _availableBadgeIDs;
	}
}
