using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.Library;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions.ItemTypes
{
	// Token: 0x0200007A RID: 122
	public class DecisionItemBaseVM : ViewModel
	{
		// Token: 0x17000300 RID: 768
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x0002B767 File Offset: 0x00029967
		// (set) Token: 0x060009FB RID: 2555 RVA: 0x0002B76F File Offset: 0x0002996F
		public KingdomElection KingdomDecisionMaker { get; private set; }

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x060009FC RID: 2556 RVA: 0x0002B778 File Offset: 0x00029978
		private float _currentInfluenceCost
		{
			get
			{
				if (this._currentSelectedOption != null && !this._currentSelectedOption.IsOptionForAbstain)
				{
					if (!this.IsPlayerSupporter)
					{
						return (float)Campaign.Current.Models.ClanPoliticsModel.GetInfluenceRequiredToOverrideKingdomDecision(this.KingdomDecisionMaker.PossibleOutcomes.MaxBy<DecisionOutcome, float>((DecisionOutcome o) => o.WinChance), this._currentSelectedOption.Option, this._decision);
					}
					if (this._currentSelectedOption.CurrentSupportWeight != Supporter.SupportWeights.Choose)
					{
						return (float)this.KingdomDecisionMaker.GetInfluenceCostOfOutcome(this._currentSelectedOption.Option, Clan.PlayerClan, this._currentSelectedOption.CurrentSupportWeight);
					}
				}
				return 0f;
			}
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0002B838 File Offset: 0x00029A38
		public DecisionItemBaseVM(KingdomDecision decision, Action onDecisionOver)
		{
			this._decision = decision;
			this._onDecisionOver = onDecisionOver;
			this.DecisionType = 0;
			this.DecisionOptionsList = new MBBindingList<DecisionOptionVM>();
			this.EndDecisionHint = new HintViewModel();
			CampaignEvents.KingdomDecisionConcluded.AddNonSerializedListener(this, new Action<KingdomDecision, DecisionOutcome, bool>(this.OnKingdomDecisionConcluded));
			this.RefreshValues();
			this.InitValues();
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
			eventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0002B8D0 File Offset: 0x00029AD0
		private void OnKingdomDecisionConcluded(KingdomDecision decision, DecisionOutcome outcome, bool isPlayerInvolved)
		{
			if (decision == this._decision)
			{
				this.IsKingsDecisionOver = true;
				this.CurrentStageIndex = 1;
				foreach (DecisionOptionVM decisionOptionVM in this.DecisionOptionsList)
				{
					if (decisionOptionVM.Option == outcome)
					{
						decisionOptionVM.IsKingsOutcome = true;
					}
					decisionOptionVM.AfterKingChooseOutcome();
				}
			}
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0002B944 File Offset: 0x00029B44
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.DoneText = GameTexts.FindText("str_done", null).ToString();
			GameTexts.SetVariable("TOTAL_INFLUENCE", MathF.Round(Hero.MainHero.Clan.Influence));
			this.TotalInfluenceText = GameTexts.FindText("str_total_influence", null).ToString();
			this.RefreshInfluenceCost();
			MBBindingList<DecisionOptionVM> decisionOptionsList = this.DecisionOptionsList;
			if (decisionOptionsList == null)
			{
				return;
			}
			decisionOptionsList.ApplyActionOnAllItems(delegate(DecisionOptionVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0002B9D8 File Offset: 0x00029BD8
		protected virtual void InitValues()
		{
			this.DecisionOptionsList.Clear();
			this.KingdomDecisionMaker = new KingdomElection(this._decision);
			this.KingdomDecisionMaker.StartElection();
			this.CurrentStageIndex = ((!this.KingdomDecisionMaker.IsPlayerChooser) ? 0 : 1);
			this.IsPlayerSupporter = !this.KingdomDecisionMaker.IsPlayerChooser;
			this.KingdomDecisionMaker.DetermineOfficialSupport();
			foreach (DecisionOutcome decisionOutcome in this.KingdomDecisionMaker.PossibleOutcomes)
			{
				DecisionOptionVM decisionOptionVM = new DecisionOptionVM(decisionOutcome, this._decision, this.KingdomDecisionMaker, new Action<DecisionOptionVM>(this.OnChangeVote), new Action<DecisionOptionVM>(this.OnSupportStrengthChange))
				{
					WinPercentage = MathF.Round(decisionOutcome.WinChance * 100f),
					InitialPercentage = MathF.Round(decisionOutcome.WinChance * 100f)
				};
				this.DecisionOptionsList.Add(decisionOptionVM);
			}
			DecisionOptionVM decisionOptionVM2 = new DecisionOptionVM(null, null, this.KingdomDecisionMaker, new Action<DecisionOptionVM>(this.OnChangeVote), new Action<DecisionOptionVM>(this.OnSupportStrengthChange));
			this.DecisionOptionsList.Add(decisionOptionVM2);
			this.TitleText = this.KingdomDecisionMaker.GetTitle().ToString();
			this.DescriptionText = this.KingdomDecisionMaker.GetDescription().ToString();
			this.RefreshInfluenceCost();
			this.RefreshCanEndDecision();
			this.RefreshRelationChangeText();
			this.IsActive = true;
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0002BB64 File Offset: 0x00029D64
		private void OnChangeVote(DecisionOptionVM target)
		{
			if (this._currentSelectedOption != target)
			{
				if (this._currentSelectedOption != null)
				{
					this._currentSelectedOption.IsSelected = false;
				}
				this._currentSelectedOption = target;
				this._currentSelectedOption.IsSelected = true;
				if (this._currentSelectedOption.IsOptionForAbstain && !this.IsPlayerSupporter)
				{
					this.KingdomDecisionMaker.OnPlayerAbstainedAsRuler();
				}
				else
				{
					this.KingdomDecisionMaker.OnPlayerSupport(this._currentSelectedOption.Option, this._currentSelectedOption.CurrentSupportWeight);
				}
				this.RefreshWinPercentages();
				this.RefreshInfluenceCost();
				this.RefreshCanEndDecision();
				this.RefreshRelationChangeText();
			}
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0002BBFF File Offset: 0x00029DFF
		private void OnSupportStrengthChange(DecisionOptionVM option)
		{
			this.RefreshWinPercentages();
			this.RefreshCanEndDecision();
			this.RefreshRelationChangeText();
			this.RefreshInfluenceCost();
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0002BC1C File Offset: 0x00029E1C
		private void RefreshWinPercentages()
		{
			this.KingdomDecisionMaker.DetermineOfficialSupport();
			using (List<DecisionOutcome>.Enumerator enumerator = this.KingdomDecisionMaker.PossibleOutcomes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DecisionOutcome option = enumerator.Current;
					DecisionOptionVM decisionOptionVM = this.DecisionOptionsList.FirstOrDefault<DecisionOptionVM>((DecisionOptionVM c) => c.Option == option);
					if (decisionOptionVM == null)
					{
						Debug.FailedAssert("Couldn't find option to update win chance for!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\KingdomManagement\\Decisions\\ItemTypes\\DecisionItemBaseVM.cs", "RefreshWinPercentages", 190);
					}
					else
					{
						decisionOptionVM.WinPercentage = (int)MathF.Round(option.WinChance * 100f, 2);
					}
				}
			}
			int num = this.DecisionOptionsList.Where<DecisionOptionVM>((DecisionOptionVM d) => !d.IsOptionForAbstain).Sum<DecisionOptionVM>((DecisionOptionVM d) => d.WinPercentage);
			if (num != 100)
			{
				int num2 = 100 - num;
				List<DecisionOptionVM> list = this.DecisionOptionsList.Where<DecisionOptionVM>((DecisionOptionVM opt) => opt.Sponsor != null).ToList<DecisionOptionVM>();
				int num3 = list.Select<DecisionOptionVM, int>((DecisionOptionVM opt) => opt.WinPercentage).Sum();
				if (num3 == 0)
				{
					int num4 = num2 / list.Count;
					foreach (DecisionOptionVM decisionOptionVM2 in list)
					{
						decisionOptionVM2.WinPercentage += num4;
					}
					list[0].WinPercentage += num2 - num4 * list.Count;
					return;
				}
				int num5 = 0;
				foreach (DecisionOptionVM decisionOptionVM3 in list.Where<DecisionOptionVM>((DecisionOptionVM opt) => opt.WinPercentage > 0).ToList<DecisionOptionVM>())
				{
					int num6 = MathF.Floor((float)num2 * ((float)decisionOptionVM3.WinPercentage / (float)num3));
					decisionOptionVM3.WinPercentage += num6;
					num5 += num6;
				}
				list[0].WinPercentage += num2 - num5;
			}
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0002BEB8 File Offset: 0x0002A0B8
		private void RefreshInfluenceCost()
		{
			if (this._currentInfluenceCost > 0f)
			{
				GameTexts.SetVariable("AMOUNT", this._currentInfluenceCost);
				GameTexts.SetVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">");
				this.InfluenceCostText = GameTexts.FindText(this.IsPlayerSupporter ? "str_decision_influence_cost" : "str_decision_ruler_influence_cost", null).ToString();
				return;
			}
			this.InfluenceCostText = "";
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0002BF24 File Offset: 0x0002A124
		private void RefreshRelationChangeText()
		{
			this.RelationChangeText = "";
			DecisionOptionVM currentSelectedOption = this._currentSelectedOption;
			if (currentSelectedOption != null && !currentSelectedOption.IsOptionForAbstain)
			{
				DecisionOptionVM currentSelectedOption2 = this._currentSelectedOption;
				if (currentSelectedOption2 == null || currentSelectedOption2.CurrentSupportWeight > Supporter.SupportWeights.Choose)
				{
					foreach (DecisionOptionVM decisionOptionVM in this.DecisionOptionsList)
					{
						DecisionOutcome option = decisionOptionVM.Option;
						if (((option != null) ? option.SponsorClan : null) != null && decisionOptionVM.Option.SponsorClan != Clan.PlayerClan)
						{
							bool flag = this._currentSelectedOption == decisionOptionVM;
							GameTexts.SetVariable("HERO_NAME", decisionOptionVM.Option.SponsorClan.Leader.EncyclopediaLinkWithName);
							string text = (flag ? GameTexts.FindText("str_decision_relation_increase", null).ToString() : GameTexts.FindText("str_decision_relation_decrease", null).ToString());
							if (string.IsNullOrEmpty(this.RelationChangeText))
							{
								this.RelationChangeText = text;
							}
							else
							{
								GameTexts.SetVariable("newline", "\n");
								GameTexts.SetVariable("STR1", this.RelationChangeText);
								GameTexts.SetVariable("STR2", text);
								this.RelationChangeText = GameTexts.FindText("str_string_newline_string", null).ToString();
							}
						}
					}
				}
			}
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0002C080 File Offset: 0x0002A280
		private void RefreshCanEndDecision()
		{
			bool flag = this._currentSelectedOption != null && (!this.IsPlayerSupporter || this._currentSelectedOption.CurrentSupportWeight > Supporter.SupportWeights.Choose);
			bool flag2 = this._currentInfluenceCost <= Clan.PlayerClan.Influence || this._currentInfluenceCost == 0f;
			DecisionOptionVM currentSelectedOption = this._currentSelectedOption;
			bool flag3 = currentSelectedOption != null && currentSelectedOption.IsOptionForAbstain;
			this.CanEndDecision = !this._finalSelectionDone && (flag3 || (flag && flag2));
			if (this.CanEndDecision)
			{
				this.EndDecisionHint.HintText = TextObject.GetEmpty();
				return;
			}
			if (flag)
			{
				if (!flag2)
				{
					this.EndDecisionHint.HintText = GameTexts.FindText("str_decision_not_enough_influence", null);
				}
				return;
			}
			if (this.IsPlayerSupporter)
			{
				this.EndDecisionHint.HintText = GameTexts.FindText("str_decision_need_to_select_an_option_and_support", null);
				return;
			}
			this.EndDecisionHint.HintText = GameTexts.FindText("str_decision_need_to_select_an_outcome", null);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0002C16D File Offset: 0x0002A36D
		protected void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0002C180 File Offset: 0x0002A380
		protected void ExecuteShowStageTooltip()
		{
			if (!this.IsPlayerSupporter)
			{
				MBInformationManager.ShowHint(GameTexts.FindText("str_decision_second_stage_player_decider", null).ToString());
				return;
			}
			if (this.CurrentStageIndex == 0)
			{
				MBInformationManager.ShowHint(GameTexts.FindText("str_decision_first_stage_player_supporter", null).ToString());
				return;
			}
			MBInformationManager.ShowHint(GameTexts.FindText("str_decision_second_stage_player_supporter", null).ToString());
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0002C1DE File Offset: 0x0002A3DE
		protected void ExecuteHideStageTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0002C1E5 File Offset: 0x0002A3E5
		public void ExecuteFinalSelection()
		{
			if (this.CanEndDecision)
			{
				this.KingdomDecisionMaker.ApplySelection();
				this._finalSelectionDone = true;
				this.RefreshCanEndDecision();
			}
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0002C208 File Offset: 0x0002A408
		protected void ExecuteDone()
		{
			TextObject chosenOutcomeText = this.KingdomDecisionMaker.GetChosenOutcomeText();
			this.IsActive = false;
			InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_decision_outcome", null).ToString(), chosenOutcomeText.ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", delegate
			{
				this._onDecisionOver();
			}, null, "", 0f, null, null, null), false, false);
			CampaignEvents.KingdomDecisionConcluded.ClearListeners(this);
			this._currentSelectedOption = null;
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0002C28D File Offset: 0x0002A48D
		public override void OnFinalize()
		{
			base.OnFinalize();
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

		// Token: 0x06000A0D RID: 2573 RVA: 0x0002C2BC File Offset: 0x0002A4BC
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent obj)
		{
			if (this._latestTutorialElementID != obj.NewNotificationElementID)
			{
				this._latestTutorialElementID = obj.NewNotificationElementID;
				if (this._isDecisionOptionsHighlightEnabled && this._latestTutorialElementID != this._decisionOptionsHighlightID)
				{
					this.SetOptionsHighlight(false);
					this._isDecisionOptionsHighlightEnabled = false;
					return;
				}
				if (!this._isDecisionOptionsHighlightEnabled && this._latestTutorialElementID == this._decisionOptionsHighlightID)
				{
					this.SetOptionsHighlight(true);
					this._isDecisionOptionsHighlightEnabled = true;
				}
			}
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0002C33C File Offset: 0x0002A53C
		private void SetOptionsHighlight(bool state)
		{
			for (int i = 0; i < this.DecisionOptionsList.Count; i++)
			{
				DecisionOptionVM decisionOptionVM = this.DecisionOptionsList[i];
				if (decisionOptionVM.CanBeChosen)
				{
					decisionOptionVM.IsHighlightEnabled = state;
				}
			}
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0002C37B File Offset: 0x0002A57B
		public void SetDoneInputKey(InputKeyItemVM inputKeyItemVM)
		{
			this.DoneInputKey = inputKeyItemVM;
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0002C384 File Offset: 0x0002A584
		// (set) Token: 0x06000A11 RID: 2577 RVA: 0x0002C38C File Offset: 0x0002A58C
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000A12 RID: 2578 RVA: 0x0002C3AA File Offset: 0x0002A5AA
		// (set) Token: 0x06000A13 RID: 2579 RVA: 0x0002C3B2 File Offset: 0x0002A5B2
		[DataSourceProperty]
		public HintViewModel EndDecisionHint
		{
			get
			{
				return this._endDecisionHint;
			}
			set
			{
				if (value != this._endDecisionHint)
				{
					this._endDecisionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "EndDecisionHint");
				}
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x0002C3D0 File Offset: 0x0002A5D0
		// (set) Token: 0x06000A15 RID: 2581 RVA: 0x0002C3D8 File Offset: 0x0002A5D8
		[DataSourceProperty]
		public int DecisionType
		{
			get
			{
				return this._decisionType;
			}
			set
			{
				if (value != this._decisionType)
				{
					this._decisionType = value;
					base.OnPropertyChangedWithValue(value, "DecisionType");
				}
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000A16 RID: 2582 RVA: 0x0002C3F6 File Offset: 0x0002A5F6
		// (set) Token: 0x06000A17 RID: 2583 RVA: 0x0002C3FE File Offset: 0x0002A5FE
		[DataSourceProperty]
		public string TotalInfluenceText
		{
			get
			{
				return this._totalInfluenceText;
			}
			set
			{
				if (value != this._totalInfluenceText)
				{
					this._totalInfluenceText = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalInfluenceText");
				}
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000A18 RID: 2584 RVA: 0x0002C421 File Offset: 0x0002A621
		// (set) Token: 0x06000A19 RID: 2585 RVA: 0x0002C429 File Offset: 0x0002A629
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
				}
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000A1A RID: 2586 RVA: 0x0002C447 File Offset: 0x0002A647
		// (set) Token: 0x06000A1B RID: 2587 RVA: 0x0002C44F File Offset: 0x0002A64F
		[DataSourceProperty]
		public int CurrentStageIndex
		{
			get
			{
				return this._currentStageIndex;
			}
			set
			{
				if (value != this._currentStageIndex)
				{
					this._currentStageIndex = value;
					base.OnPropertyChangedWithValue(value, "CurrentStageIndex");
				}
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000A1C RID: 2588 RVA: 0x0002C46D File Offset: 0x0002A66D
		// (set) Token: 0x06000A1D RID: 2589 RVA: 0x0002C475 File Offset: 0x0002A675
		[DataSourceProperty]
		public bool IsPlayerSupporter
		{
			get
			{
				return this._isPlayerSupporter;
			}
			set
			{
				if (value != this._isPlayerSupporter)
				{
					this._isPlayerSupporter = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerSupporter");
				}
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000A1E RID: 2590 RVA: 0x0002C493 File Offset: 0x0002A693
		// (set) Token: 0x06000A1F RID: 2591 RVA: 0x0002C49B File Offset: 0x0002A69B
		[DataSourceProperty]
		public bool CanEndDecision
		{
			get
			{
				return this._canEndDecision;
			}
			set
			{
				if (value != this._canEndDecision)
				{
					this._canEndDecision = value;
					base.OnPropertyChangedWithValue(value, "CanEndDecision");
				}
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000A20 RID: 2592 RVA: 0x0002C4B9 File Offset: 0x0002A6B9
		// (set) Token: 0x06000A21 RID: 2593 RVA: 0x0002C4C1 File Offset: 0x0002A6C1
		[DataSourceProperty]
		public bool IsKingsDecisionOver
		{
			get
			{
				return this._isKingsDecisionOver;
			}
			set
			{
				if (value != this._isKingsDecisionOver)
				{
					this._isKingsDecisionOver = value;
					base.OnPropertyChangedWithValue(value, "IsKingsDecisionOver");
				}
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000A22 RID: 2594 RVA: 0x0002C4DF File Offset: 0x0002A6DF
		// (set) Token: 0x06000A23 RID: 2595 RVA: 0x0002C4E7 File Offset: 0x0002A6E7
		[DataSourceProperty]
		public string RelationChangeText
		{
			get
			{
				return this._increaseRelationText;
			}
			set
			{
				if (value != this._increaseRelationText)
				{
					this._increaseRelationText = value;
					base.OnPropertyChangedWithValue<string>(value, "RelationChangeText");
				}
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x0002C50A File Offset: 0x0002A70A
		// (set) Token: 0x06000A25 RID: 2597 RVA: 0x0002C512 File Offset: 0x0002A712
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x0002C535 File Offset: 0x0002A735
		// (set) Token: 0x06000A27 RID: 2599 RVA: 0x0002C53D File Offset: 0x0002A73D
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

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000A28 RID: 2600 RVA: 0x0002C560 File Offset: 0x0002A760
		// (set) Token: 0x06000A29 RID: 2601 RVA: 0x0002C568 File Offset: 0x0002A768
		[DataSourceProperty]
		public string DoneText
		{
			get
			{
				return this._doneText;
			}
			set
			{
				if (value != this._doneText)
				{
					this._doneText = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneText");
				}
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000A2A RID: 2602 RVA: 0x0002C58B File Offset: 0x0002A78B
		// (set) Token: 0x06000A2B RID: 2603 RVA: 0x0002C593 File Offset: 0x0002A793
		[DataSourceProperty]
		public string InfluenceCostText
		{
			get
			{
				return this._influenceCostText;
			}
			set
			{
				if (value != this._influenceCostText)
				{
					this._influenceCostText = value;
					base.OnPropertyChangedWithValue<string>(value, "InfluenceCostText");
				}
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000A2C RID: 2604 RVA: 0x0002C5B6 File Offset: 0x0002A7B6
		// (set) Token: 0x06000A2D RID: 2605 RVA: 0x0002C5BE File Offset: 0x0002A7BE
		[DataSourceProperty]
		public MBBindingList<DecisionOptionVM> DecisionOptionsList
		{
			get
			{
				return this._decisionOptionsList;
			}
			set
			{
				if (value != this._decisionOptionsList)
				{
					this._decisionOptionsList = value;
					base.OnPropertyChangedWithValue<MBBindingList<DecisionOptionVM>>(value, "DecisionOptionsList");
				}
			}
		}

		// Token: 0x04000470 RID: 1136
		protected readonly KingdomDecision _decision;

		// Token: 0x04000471 RID: 1137
		private readonly Action _onDecisionOver;

		// Token: 0x04000472 RID: 1138
		private DecisionOptionVM _currentSelectedOption;

		// Token: 0x04000473 RID: 1139
		private bool _finalSelectionDone;

		// Token: 0x04000474 RID: 1140
		private bool _isDecisionOptionsHighlightEnabled;

		// Token: 0x04000475 RID: 1141
		private string _decisionOptionsHighlightID = "DecisionOptions";

		// Token: 0x04000476 RID: 1142
		private string _latestTutorialElementID;

		// Token: 0x04000477 RID: 1143
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000478 RID: 1144
		private int _decisionType;

		// Token: 0x04000479 RID: 1145
		private bool _isActive;

		// Token: 0x0400047A RID: 1146
		private bool _isPlayerSupporter;

		// Token: 0x0400047B RID: 1147
		private bool _canEndDecision;

		// Token: 0x0400047C RID: 1148
		private bool _isKingsDecisionOver;

		// Token: 0x0400047D RID: 1149
		private int _currentStageIndex = -1;

		// Token: 0x0400047E RID: 1150
		private string _titleText;

		// Token: 0x0400047F RID: 1151
		private string _doneText;

		// Token: 0x04000480 RID: 1152
		private string _descriptionText;

		// Token: 0x04000481 RID: 1153
		private string _influenceCostText;

		// Token: 0x04000482 RID: 1154
		private string _totalInfluenceText;

		// Token: 0x04000483 RID: 1155
		private string _increaseRelationText;

		// Token: 0x04000484 RID: 1156
		private HintViewModel _endDecisionHint;

		// Token: 0x04000485 RID: 1157
		private MBBindingList<DecisionOptionVM> _decisionOptionsList;

		// Token: 0x020001DD RID: 477
		protected enum DecisionTypes
		{
			// Token: 0x0400112E RID: 4398
			Default,
			// Token: 0x0400112F RID: 4399
			Settlement,
			// Token: 0x04001130 RID: 4400
			ExpelClan,
			// Token: 0x04001131 RID: 4401
			Policy,
			// Token: 0x04001132 RID: 4402
			DeclareWar,
			// Token: 0x04001133 RID: 4403
			MakePeace,
			// Token: 0x04001134 RID: 4404
			KingSelection,
			// Token: 0x04001135 RID: 4405
			StartAlliance,
			// Token: 0x04001136 RID: 4406
			AcceptCallToWarAgreement,
			// Token: 0x04001137 RID: 4407
			ProposeCallToWarAgreement,
			// Token: 0x04001138 RID: 4408
			Trade
		}
	}
}
