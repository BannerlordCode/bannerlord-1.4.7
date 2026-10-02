using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;
using TaleWorlds.MountAndBlade.Diamond.Ranked;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.AfterBattle
{
	// Token: 0x02000086 RID: 134
	public class MPAfterBattlePopupVM : ViewModel
	{
		// Token: 0x06000D0A RID: 3338 RVA: 0x000284F5 File Offset: 0x000266F5
		public MPAfterBattlePopupVM(Func<string> getExitText)
		{
			this._getExitText = getExitText;
			this.RefreshValues();
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0002852C File Offset: 0x0002672C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._battleResultsTitleText = new TextObject("{=pguhTmXw}Battle Results", null).ToString();
			this._levelUpTitleText = new TextObject("{=0tUYng4e}Leveled Up!", null).ToString();
			this._rankProgressTitleText = new TextObject("{=XEGaQB2G}Rank Progression", null).ToString();
			this._promotedTitleText = new TextObject("{=bn0v5ST0}Promoted!", null).ToString();
			this._demotedTitleText = new TextObject("{=HUndnpNw}Demoted!", null).ToString();
			this._evaluationFinishedTitleText = new TextObject("{=2KZLf51A}Evaluation Matches Finished", null).ToString();
			this.LevelText = GameTexts.FindText("str_level", null).ToString();
			this.ExperienceText = new TextObject("{=SwSaXwQg}exp", null).ToString();
			this.PointsText = new TextObject("{=4dRTWSN3}Points", null).ToString();
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x00028608 File Offset: 0x00026808
		public void OpenWith(int oldExperience, int newExperience, List<string> badgesEarned, int lootGained, RankBarInfo oldRankBarInfo, RankBarInfo newRankBarInfo)
		{
			Func<string> getExitText = this._getExitText;
			this.ClickToContinueText = ((getExitText != null) ? getExitText() : null);
			this._oldExperience = oldExperience;
			this._newExperience = newExperience;
			this._earnedBadgeIDs = badgesEarned;
			this._lootGained = lootGained;
			this._oldRankBarInfo = oldRankBarInfo;
			this._newRankBarInfo = newRankBarInfo;
			this._hasRatingChanged = oldRankBarInfo != null && newRankBarInfo != null && !oldRankBarInfo.IsEvaluating && !newRankBarInfo.IsEvaluating;
			this._hasRankChanged = this._hasRatingChanged && oldRankBarInfo.RankId != newRankBarInfo.RankId;
			this._hasFinishedEvaluation = this._oldRankBarInfo != null && this._newRankBarInfo != null && this._oldRankBarInfo.IsEvaluating && !this._newRankBarInfo.IsEvaluating;
			this.AdvanceState();
			this.IsEnabled = true;
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x000286E4 File Offset: 0x000268E4
		private void AdvanceState()
		{
			this.HideInfo();
			switch (this._currentState)
			{
			case MPAfterBattlePopupVM.AfterBattleState.None:
				this._currentState = MPAfterBattlePopupVM.AfterBattleState.GeneralProgression;
				this.ShowGeneralProgression();
				return;
			case MPAfterBattlePopupVM.AfterBattleState.GeneralProgression:
				if (this._hasLeveledUp)
				{
					this._currentState = MPAfterBattlePopupVM.AfterBattleState.LevelUp;
					this.ShowLevelUp();
					return;
				}
				if (this._hasRatingChanged)
				{
					this._currentState = MPAfterBattlePopupVM.AfterBattleState.RatingChange;
					this.ShowRankProgression();
					return;
				}
				if (this._hasFinishedEvaluation)
				{
					this._currentState = MPAfterBattlePopupVM.AfterBattleState.RankChange;
					this.ShowRankChange();
					return;
				}
				this.Disable();
				return;
			case MPAfterBattlePopupVM.AfterBattleState.LevelUp:
				if (this._hasRatingChanged)
				{
					this._currentState = MPAfterBattlePopupVM.AfterBattleState.RatingChange;
					this.ShowRankProgression();
					return;
				}
				if (this._hasFinishedEvaluation)
				{
					this._currentState = MPAfterBattlePopupVM.AfterBattleState.RankChange;
					this.ShowRankChange();
					return;
				}
				this.Disable();
				return;
			case MPAfterBattlePopupVM.AfterBattleState.RatingChange:
				if (this._hasRankChanged)
				{
					this._currentState = MPAfterBattlePopupVM.AfterBattleState.RankChange;
					this.ShowRankChange();
					return;
				}
				this.Disable();
				return;
			case MPAfterBattlePopupVM.AfterBattleState.RankChange:
				this.Disable();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x000287C8 File Offset: 0x000269C8
		private void ShowGeneralProgression()
		{
			this.TitleText = this._battleResultsTitleText;
			this.InitialRatio = 0;
			this.FinalRatio = 0;
			this.NumOfLevelUps = 0;
			PlayerDataExperience playerDataExperience = new PlayerDataExperience(this._oldExperience);
			PlayerDataExperience playerDataExperience2 = new PlayerDataExperience(this._newExperience);
			this.GainedExperience = this._newExperience - this._oldExperience;
			this.CurrentLevel = playerDataExperience.Level;
			this.NextLevel = this.CurrentLevel + 1;
			this.InitialRatio = (int)((float)playerDataExperience.ExperienceInCurrentLevel / (float)(playerDataExperience.ExperienceToNextLevel + playerDataExperience.ExperienceInCurrentLevel) * 100f);
			this.FinalRatio = (int)((float)playerDataExperience2.ExperienceInCurrentLevel / (float)(playerDataExperience2.ExperienceToNextLevel + playerDataExperience2.ExperienceInCurrentLevel) * 100f);
			this.NumOfLevelUps = playerDataExperience2.Level - playerDataExperience.Level;
			this._hasLeveledUp = this.NumOfLevelUps > 0;
			this.HasLostRating = this.GainedExperience < 0;
			float num = (float)this.NumOfLevelUps + (float)this.FinalRatio / 100f;
			this.LevelsExperienceRequirment = (int)((float)this._newExperience / num);
			this.RewardsEarned = new MBBindingList<MPAfterBattleRewardItemVM>();
			foreach (string text in this._earnedBadgeIDs)
			{
				Badge byId = BadgeManager.GetById(text);
				if (byId != null)
				{
					this.RewardsEarned.Add(new MPAfterBattleBadgeRewardItemVM(byId));
				}
			}
			if (this._lootGained > 0)
			{
				int num2 = this._lootGained - this._earnedBadgeIDs.Count * Parameters.LootRewardPerBadgeEarned;
				int num3 = this._lootGained - num2;
				this.RewardsEarned.Add(new MPAfterBattleLootRewardItemVM(num2, num3));
			}
			this.IsShowingGeneralProgression = true;
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00028994 File Offset: 0x00026B94
		private void ShowLevelUp()
		{
			this.TitleText = this._levelUpTitleText;
			int level = new PlayerDataExperience(this._newExperience).Level;
			GameTexts.SetVariable("STR1", GameTexts.FindText("str_level", null));
			GameTexts.SetVariable("STR2", level);
			this.ReachedLevelText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			SoundEvent.PlaySound2D("event:/ui/multiplayer/levelup");
			this.IsShowingNewLevel = true;
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00028A0C File Offset: 0x00026C0C
		private void ShowRankProgression()
		{
			this.TitleText = this._rankProgressTitleText;
			this.OldRankID = this._oldRankBarInfo.RankId;
			this.NewRankID = this._newRankBarInfo.RankId;
			this.OldRankName = MPLobbyVM.GetLocalizedRankName(this.OldRankID);
			this.NewRankName = MPLobbyVM.GetLocalizedRankName(this.NewRankID);
			this.HasLostRating = this._oldRankBarInfo.Rating > this._newRankBarInfo.Rating;
			this.ShownRating = this._newRankBarInfo.Rating;
			this.InitialRatio = (int)this._oldRankBarInfo.ProgressPercentage;
			this.FinalRatio = (int)this._newRankBarInfo.ProgressPercentage;
			this.NumOfLevelUps = Ranks.RankIds.IndexOf(this.NewRankID) - Ranks.RankIds.IndexOf(this.OldRankID);
			if (this.HasLostRating)
			{
				this._pointsLostTextObj.SetTextVariable("POINTS", this._oldRankBarInfo.Rating - this._newRankBarInfo.Rating);
				this.PointChangedText = this._pointsLostTextObj.ToString();
			}
			else
			{
				this._pointsGainedTextObj.SetTextVariable("POINTS", this._newRankBarInfo.Rating - this._oldRankBarInfo.Rating);
				this.PointChangedText = this._pointsGainedTextObj.ToString();
			}
			this.IsShowingRankProgression = true;
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x00028B68 File Offset: 0x00026D68
		private void ShowRankChange()
		{
			if (this.OldRankID != string.Empty && this.OldRankID != null)
			{
				this.TitleText = ((Ranks.RankIds.IndexOf(this.OldRankID) < Ranks.RankIds.IndexOf(this.NewRankID)) ? this._promotedTitleText : this._demotedTitleText);
				this.IsShowingNewRank = true;
				return;
			}
			if (this._hasFinishedEvaluation)
			{
				this.OldRankID = string.Empty;
				this.NewRankID = this._newRankBarInfo.RankId;
				this.OldRankName = MPLobbyVM.GetLocalizedRankName(this.OldRankID);
				this.NewRankName = MPLobbyVM.GetLocalizedRankName(this.NewRankID);
				this.TitleText = this._evaluationFinishedTitleText;
				this.IsShowingNewRank = true;
			}
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x00028C2A File Offset: 0x00026E2A
		private void HideInfo()
		{
			this.IsShowingGeneralProgression = false;
			this.IsShowingNewLevel = false;
			this.IsShowingRankProgression = false;
			this.IsShowingNewRank = false;
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00028C48 File Offset: 0x00026E48
		private void Disable()
		{
			this.HideInfo();
			this.ShownRating = 0;
			this._currentState = MPAfterBattlePopupVM.AfterBattleState.None;
			this.IsEnabled = false;
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x00028C65 File Offset: 0x00026E65
		public void ExecuteClose()
		{
			if (this.IsEnabled)
			{
				this.AdvanceState();
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x00028C75 File Offset: 0x00026E75
		// (set) Token: 0x06000D16 RID: 3350 RVA: 0x00028C7D File Offset: 0x00026E7D
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

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06000D17 RID: 3351 RVA: 0x00028C9B File Offset: 0x00026E9B
		// (set) Token: 0x06000D18 RID: 3352 RVA: 0x00028CA3 File Offset: 0x00026EA3
		[DataSourceProperty]
		public bool IsShowingGeneralProgression
		{
			get
			{
				return this._isShowingGeneralProgression;
			}
			set
			{
				if (value != this._isShowingGeneralProgression)
				{
					this._isShowingGeneralProgression = value;
					base.OnPropertyChangedWithValue(value, "IsShowingGeneralProgression");
				}
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x00028CC1 File Offset: 0x00026EC1
		// (set) Token: 0x06000D1A RID: 3354 RVA: 0x00028CC9 File Offset: 0x00026EC9
		[DataSourceProperty]
		public bool IsShowingNewLevel
		{
			get
			{
				return this._isShowingNewLevel;
			}
			set
			{
				if (value != this._isShowingNewLevel)
				{
					this._isShowingNewLevel = value;
					base.OnPropertyChangedWithValue(value, "IsShowingNewLevel");
				}
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x00028CE7 File Offset: 0x00026EE7
		// (set) Token: 0x06000D1C RID: 3356 RVA: 0x00028CEF File Offset: 0x00026EEF
		[DataSourceProperty]
		public bool IsShowingRankProgression
		{
			get
			{
				return this._isShowingRankProgression;
			}
			set
			{
				if (value != this._isShowingRankProgression)
				{
					this._isShowingRankProgression = value;
					base.OnPropertyChangedWithValue(value, "IsShowingRankProgression");
				}
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x00028D0D File Offset: 0x00026F0D
		// (set) Token: 0x06000D1E RID: 3358 RVA: 0x00028D15 File Offset: 0x00026F15
		[DataSourceProperty]
		public bool IsShowingNewRank
		{
			get
			{
				return this._isShowingNewRank;
			}
			set
			{
				if (value != this._isShowingNewRank)
				{
					this._isShowingNewRank = value;
					base.OnPropertyChangedWithValue(value, "IsShowingNewRank");
				}
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x00028D33 File Offset: 0x00026F33
		// (set) Token: 0x06000D20 RID: 3360 RVA: 0x00028D3B File Offset: 0x00026F3B
		[DataSourceProperty]
		public bool HasLostRating
		{
			get
			{
				return this._hasLostRating;
			}
			set
			{
				if (value != this._hasLostRating)
				{
					this._hasLostRating = value;
					base.OnPropertyChangedWithValue(value, "HasLostRating");
				}
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06000D21 RID: 3361 RVA: 0x00028D59 File Offset: 0x00026F59
		// (set) Token: 0x06000D22 RID: 3362 RVA: 0x00028D61 File Offset: 0x00026F61
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

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x00028D84 File Offset: 0x00026F84
		// (set) Token: 0x06000D24 RID: 3364 RVA: 0x00028D8C File Offset: 0x00026F8C
		[DataSourceProperty]
		public string LevelText
		{
			get
			{
				return this._levelText;
			}
			set
			{
				if (value != this._levelText)
				{
					this._levelText = value;
					base.OnPropertyChangedWithValue<string>(value, "LevelText");
				}
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06000D25 RID: 3365 RVA: 0x00028DAF File Offset: 0x00026FAF
		// (set) Token: 0x06000D26 RID: 3366 RVA: 0x00028DB7 File Offset: 0x00026FB7
		[DataSourceProperty]
		public string ExperienceText
		{
			get
			{
				return this._experienceText;
			}
			set
			{
				if (value != this._experienceText)
				{
					this._experienceText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExperienceText");
				}
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x00028DDA File Offset: 0x00026FDA
		// (set) Token: 0x06000D28 RID: 3368 RVA: 0x00028DE2 File Offset: 0x00026FE2
		[DataSourceProperty]
		public string ClickToContinueText
		{
			get
			{
				return this._clickToContinueText;
			}
			set
			{
				if (value != this._clickToContinueText)
				{
					this._clickToContinueText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClickToContinueText");
				}
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06000D29 RID: 3369 RVA: 0x00028E05 File Offset: 0x00027005
		// (set) Token: 0x06000D2A RID: 3370 RVA: 0x00028E0D File Offset: 0x0002700D
		[DataSourceProperty]
		public string ReachedLevelText
		{
			get
			{
				return this._reachedLevelText;
			}
			set
			{
				if (value != this._reachedLevelText)
				{
					this._reachedLevelText = value;
					base.OnPropertyChangedWithValue<string>(value, "ReachedLevelText");
				}
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06000D2B RID: 3371 RVA: 0x00028E30 File Offset: 0x00027030
		// (set) Token: 0x06000D2C RID: 3372 RVA: 0x00028E38 File Offset: 0x00027038
		[DataSourceProperty]
		public string PointsText
		{
			get
			{
				return this._pointsText;
			}
			set
			{
				if (value != this._pointsText)
				{
					this._pointsText = value;
					base.OnPropertyChangedWithValue<string>(value, "PointsText");
				}
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06000D2D RID: 3373 RVA: 0x00028E5B File Offset: 0x0002705B
		// (set) Token: 0x06000D2E RID: 3374 RVA: 0x00028E63 File Offset: 0x00027063
		[DataSourceProperty]
		public string PointChangedText
		{
			get
			{
				return this._pointChangeText;
			}
			set
			{
				if (value != this._pointChangeText)
				{
					this._pointChangeText = value;
					base.OnPropertyChangedWithValue<string>(value, "PointChangedText");
				}
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06000D2F RID: 3375 RVA: 0x00028E86 File Offset: 0x00027086
		// (set) Token: 0x06000D30 RID: 3376 RVA: 0x00028E8E File Offset: 0x0002708E
		[DataSourceProperty]
		public string OldRankID
		{
			get
			{
				return this._oldRankID;
			}
			set
			{
				if (value != this._oldRankID)
				{
					this._oldRankID = value;
					base.OnPropertyChangedWithValue<string>(value, "OldRankID");
				}
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06000D31 RID: 3377 RVA: 0x00028EB1 File Offset: 0x000270B1
		// (set) Token: 0x06000D32 RID: 3378 RVA: 0x00028EB9 File Offset: 0x000270B9
		[DataSourceProperty]
		public string NewRankID
		{
			get
			{
				return this._newRankID;
			}
			set
			{
				if (value != this._newRankID)
				{
					this._newRankID = value;
					base.OnPropertyChangedWithValue<string>(value, "NewRankID");
				}
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06000D33 RID: 3379 RVA: 0x00028EDC File Offset: 0x000270DC
		// (set) Token: 0x06000D34 RID: 3380 RVA: 0x00028EE4 File Offset: 0x000270E4
		[DataSourceProperty]
		public string OldRankName
		{
			get
			{
				return this._oldRankName;
			}
			set
			{
				if (value != this._oldRankName)
				{
					this._oldRankName = value;
					base.OnPropertyChangedWithValue<string>(value, "OldRankName");
				}
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06000D35 RID: 3381 RVA: 0x00028F07 File Offset: 0x00027107
		// (set) Token: 0x06000D36 RID: 3382 RVA: 0x00028F0F File Offset: 0x0002710F
		[DataSourceProperty]
		public string NewRankName
		{
			get
			{
				return this._newRankName;
			}
			set
			{
				if (value != this._newRankName)
				{
					this._newRankName = value;
					base.OnPropertyChangedWithValue<string>(value, "NewRankName");
				}
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06000D37 RID: 3383 RVA: 0x00028F32 File Offset: 0x00027132
		// (set) Token: 0x06000D38 RID: 3384 RVA: 0x00028F3A File Offset: 0x0002713A
		[DataSourceProperty]
		public int FinalRatio
		{
			get
			{
				return this._finalRatio;
			}
			set
			{
				if (value != this._finalRatio)
				{
					this._finalRatio = value;
					base.OnPropertyChangedWithValue(value, "FinalRatio");
				}
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06000D39 RID: 3385 RVA: 0x00028F58 File Offset: 0x00027158
		// (set) Token: 0x06000D3A RID: 3386 RVA: 0x00028F60 File Offset: 0x00027160
		[DataSourceProperty]
		public int NumOfLevelUps
		{
			get
			{
				return this._numOfLevelUps;
			}
			set
			{
				if (value != this._numOfLevelUps)
				{
					this._numOfLevelUps = value;
					base.OnPropertyChangedWithValue(value, "NumOfLevelUps");
				}
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06000D3B RID: 3387 RVA: 0x00028F7E File Offset: 0x0002717E
		// (set) Token: 0x06000D3C RID: 3388 RVA: 0x00028F86 File Offset: 0x00027186
		[DataSourceProperty]
		public int InitialRatio
		{
			get
			{
				return this._initialRatio;
			}
			set
			{
				if (value != this._initialRatio)
				{
					this._initialRatio = value;
					base.OnPropertyChangedWithValue(value, "InitialRatio");
				}
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06000D3D RID: 3389 RVA: 0x00028FA4 File Offset: 0x000271A4
		// (set) Token: 0x06000D3E RID: 3390 RVA: 0x00028FAC File Offset: 0x000271AC
		[DataSourceProperty]
		public int GainedExperience
		{
			get
			{
				return this._gainedExperience;
			}
			set
			{
				if (value != this._gainedExperience)
				{
					this._gainedExperience = value;
					base.OnPropertyChangedWithValue(value, "GainedExperience");
				}
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06000D3F RID: 3391 RVA: 0x00028FCA File Offset: 0x000271CA
		// (set) Token: 0x06000D40 RID: 3392 RVA: 0x00028FD2 File Offset: 0x000271D2
		[DataSourceProperty]
		public int LevelsExperienceRequirment
		{
			get
			{
				return this._levelsExperienceRequirment;
			}
			set
			{
				if (value != this._levelsExperienceRequirment)
				{
					this._levelsExperienceRequirment = value;
					base.OnPropertyChangedWithValue(value, "LevelsExperienceRequirment");
				}
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06000D41 RID: 3393 RVA: 0x00028FF0 File Offset: 0x000271F0
		// (set) Token: 0x06000D42 RID: 3394 RVA: 0x00028FF8 File Offset: 0x000271F8
		[DataSourceProperty]
		public int NextLevel
		{
			get
			{
				return this._nextLevel;
			}
			set
			{
				if (value != this._nextLevel)
				{
					this._nextLevel = value;
					base.OnPropertyChangedWithValue(value, "NextLevel");
				}
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x00029016 File Offset: 0x00027216
		// (set) Token: 0x06000D44 RID: 3396 RVA: 0x0002901E File Offset: 0x0002721E
		[DataSourceProperty]
		public int CurrentLevel
		{
			get
			{
				return this._currentLevel;
			}
			set
			{
				if (value != this._currentLevel)
				{
					this._currentLevel = value;
					base.OnPropertyChangedWithValue(value, "CurrentLevel");
				}
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x0002903C File Offset: 0x0002723C
		// (set) Token: 0x06000D46 RID: 3398 RVA: 0x00029044 File Offset: 0x00027244
		[DataSourceProperty]
		public int ShownRating
		{
			get
			{
				return this._shownRating;
			}
			set
			{
				if (value != this._shownRating)
				{
					this._shownRating = value;
					base.OnPropertyChangedWithValue(value, "ShownRating");
				}
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06000D47 RID: 3399 RVA: 0x00029062 File Offset: 0x00027262
		// (set) Token: 0x06000D48 RID: 3400 RVA: 0x0002906A File Offset: 0x0002726A
		[DataSourceProperty]
		public MBBindingList<MPAfterBattleRewardItemVM> RewardsEarned
		{
			get
			{
				return this._rewardsEarned;
			}
			set
			{
				if (value != this._rewardsEarned)
				{
					this._rewardsEarned = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPAfterBattleRewardItemVM>>(value, "RewardsEarned");
				}
			}
		}

		// Token: 0x040005E6 RID: 1510
		private MPAfterBattlePopupVM.AfterBattleState _currentState;

		// Token: 0x040005E7 RID: 1511
		private bool _hasLeveledUp;

		// Token: 0x040005E8 RID: 1512
		private int _oldExperience;

		// Token: 0x040005E9 RID: 1513
		private int _newExperience;

		// Token: 0x040005EA RID: 1514
		private List<string> _earnedBadgeIDs;

		// Token: 0x040005EB RID: 1515
		private int _lootGained;

		// Token: 0x040005EC RID: 1516
		private bool _hasRatingChanged;

		// Token: 0x040005ED RID: 1517
		private bool _hasRankChanged;

		// Token: 0x040005EE RID: 1518
		private bool _hasFinishedEvaluation;

		// Token: 0x040005EF RID: 1519
		private RankBarInfo _oldRankBarInfo;

		// Token: 0x040005F0 RID: 1520
		private RankBarInfo _newRankBarInfo;

		// Token: 0x040005F1 RID: 1521
		private string _battleResultsTitleText;

		// Token: 0x040005F2 RID: 1522
		private string _levelUpTitleText;

		// Token: 0x040005F3 RID: 1523
		private string _rankProgressTitleText;

		// Token: 0x040005F4 RID: 1524
		private string _promotedTitleText;

		// Token: 0x040005F5 RID: 1525
		private string _demotedTitleText;

		// Token: 0x040005F6 RID: 1526
		private string _evaluationFinishedTitleText;

		// Token: 0x040005F7 RID: 1527
		private TextObject _pointsGainedTextObj = new TextObject("{=EFU3uo0y}You've gained {POINTS} points", null);

		// Token: 0x040005F8 RID: 1528
		private TextObject _pointsLostTextObj = new TextObject("{=oMYz0PvL}You've lost {POINTS} points", null);

		// Token: 0x040005F9 RID: 1529
		private readonly Func<string> _getExitText;

		// Token: 0x040005FA RID: 1530
		private bool _isEnabled;

		// Token: 0x040005FB RID: 1531
		private bool _isShowingGeneralProgression;

		// Token: 0x040005FC RID: 1532
		private bool _isShowingNewLevel;

		// Token: 0x040005FD RID: 1533
		private bool _isShowingRankProgression;

		// Token: 0x040005FE RID: 1534
		private bool _isShowingNewRank;

		// Token: 0x040005FF RID: 1535
		private bool _hasLostRating;

		// Token: 0x04000600 RID: 1536
		private string _titleText;

		// Token: 0x04000601 RID: 1537
		private string _levelText;

		// Token: 0x04000602 RID: 1538
		private string _experienceText;

		// Token: 0x04000603 RID: 1539
		private string _clickToContinueText;

		// Token: 0x04000604 RID: 1540
		private string _reachedLevelText;

		// Token: 0x04000605 RID: 1541
		private string _pointsText;

		// Token: 0x04000606 RID: 1542
		private string _pointChangeText;

		// Token: 0x04000607 RID: 1543
		private string _oldRankID;

		// Token: 0x04000608 RID: 1544
		private string _newRankID;

		// Token: 0x04000609 RID: 1545
		private string _oldRankName;

		// Token: 0x0400060A RID: 1546
		private string _newRankName;

		// Token: 0x0400060B RID: 1547
		private int _initialRatio;

		// Token: 0x0400060C RID: 1548
		private int _finalRatio;

		// Token: 0x0400060D RID: 1549
		private int _numOfLevelUps;

		// Token: 0x0400060E RID: 1550
		private int _gainedExperience;

		// Token: 0x0400060F RID: 1551
		private int _levelsExperienceRequirment;

		// Token: 0x04000610 RID: 1552
		private int _currentLevel;

		// Token: 0x04000611 RID: 1553
		private int _nextLevel;

		// Token: 0x04000612 RID: 1554
		private int _shownRating;

		// Token: 0x04000613 RID: 1555
		private MBBindingList<MPAfterBattleRewardItemVM> _rewardsEarned;

		// Token: 0x02000174 RID: 372
		private enum AfterBattleState
		{
			// Token: 0x04000A09 RID: 2569
			None,
			// Token: 0x04000A0A RID: 2570
			GeneralProgression,
			// Token: 0x04000A0B RID: 2571
			LevelUp,
			// Token: 0x04000A0C RID: 2572
			RatingChange,
			// Token: 0x04000A0D RID: 2573
			RankChange
		}
	}
}
