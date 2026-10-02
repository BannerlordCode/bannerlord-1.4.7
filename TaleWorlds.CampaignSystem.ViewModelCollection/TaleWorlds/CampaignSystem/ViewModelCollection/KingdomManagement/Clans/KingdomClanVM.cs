using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Clans
{
	// Token: 0x02000088 RID: 136
	public class KingdomClanVM : KingdomCategoryVM
	{
		// Token: 0x06000B6F RID: 2927 RVA: 0x000302E8 File Offset: 0x0002E4E8
		public KingdomClanVM(Action<KingdomDecision> forceDecide)
		{
			this._forceDecide = forceDecide;
			this.SupportHint = new HintViewModel();
			this.ExpelHint = new HintViewModel();
			this._clans = new MBBindingList<KingdomClanItemVM>();
			base.IsAcceptableItemSelected = false;
			this.RefreshClanList();
			base.NotificationCount = 0;
			this.SupportCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfSupportingClan();
			this.ExpelCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfExpellingClan(Clan.PlayerClan);
			TextObject textObject;
			this.CanSupportCurrentClan = this.GetCanSupportCurrentClanWithReason(this.SupportCost, out textObject);
			this.SupportHint.HintText = textObject;
			TextObject textObject2;
			this.CanExpelCurrentClan = this.GetCanExpelCurrentClanWithReason(this._isThereAPendingDecisionToExpelThisClan, this.ExpelCost, out textObject2);
			this.ExpelHint.HintText = textObject2;
			this.ClanSortController = new KingdomClanSortControllerVM(ref this._clans);
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			this.RefreshValues();
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x000303E4 File Offset: 0x0002E5E4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SupportText = new TextObject("{=N63XYX2r}Support", null).ToString();
			this.NameText = GameTexts.FindText("str_scoreboard_header", "name").ToString();
			this.InfluenceText = GameTexts.FindText("str_influence", null).ToString();
			this.FiefsText = GameTexts.FindText("str_fiefs", null).ToString();
			this.MembersText = GameTexts.FindText("str_members", null).ToString();
			this.BannerText = GameTexts.FindText("str_banner", null).ToString();
			this.TypeText = GameTexts.FindText("str_sort_by_type_label", null).ToString();
			base.CategoryNameText = new TextObject("{=j4F7tTzy}Clan", null).ToString();
			base.NoItemSelectedText = GameTexts.FindText("str_kingdom_no_clan_selected", null).ToString();
			this.SupportActionExplanationText = GameTexts.FindText("str_support_clan_action_explanation", null).ToString();
			this.ExpelActionExplanationText = GameTexts.FindText("str_expel_clan_action_explanation", null).SetTextVariable("SUPPORT", GameTexts.FindText("str_decision_outcome_support_status", KingdomElection.ElectionOutcomeSupport.LowSupport.ToString())).ToString();
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x00030510 File Offset: 0x0002E710
		private void SetCurrentSelectedClan(KingdomClanItemVM clan)
		{
			if (clan != this.CurrentSelectedClan)
			{
				if (this.CurrentSelectedClan != null)
				{
					this.CurrentSelectedClan.IsSelected = false;
				}
				this.CurrentSelectedClan = clan;
				this.CurrentSelectedClan.IsSelected = true;
				this.SupportCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfSupportingClan();
				this._isThereAPendingDecisionToExpelThisClan = Clan.PlayerClan.Kingdom.UnresolvedDecisions.Any<KingdomDecision>(delegate(KingdomDecision x)
				{
					ExpelClanFromKingdomDecision expelClanFromKingdomDecision;
					return (expelClanFromKingdomDecision = x as ExpelClanFromKingdomDecision) != null && expelClanFromKingdomDecision.ClanToExpel == this.CurrentSelectedClan.Clan && !x.ShouldBeCancelled();
				});
				TextObject textObject;
				this.CanExpelCurrentClan = this.GetCanExpelCurrentClanWithReason(this._isThereAPendingDecisionToExpelThisClan, this.ExpelCost, out textObject);
				this.ExpelHint.HintText = textObject;
				if (this._isThereAPendingDecisionToExpelThisClan)
				{
					this.ExpelActionText = GameTexts.FindText("str_resolve", null).ToString();
					this.ExpelActionExplanationText = GameTexts.FindText("str_resolve_explanation", null).ToString();
					this.ExpelCost = 0;
					return;
				}
				this.ExpelActionText = GameTexts.FindText("str_policy_propose", null).ToString();
				this.ExpelCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfExpellingClan(Clan.PlayerClan);
				TextObject textObject2;
				this.CanSupportCurrentClan = this.GetCanSupportCurrentClanWithReason(this.SupportCost, out textObject2);
				this.SupportHint.HintText = textObject2;
				this.ExpelActionExplanationText = GameTexts.FindText("str_expel_clan_action_explanation", null).SetTextVariable("SUPPORT", this.GetExpelLikelihoodText(this.CurrentSelectedClan)).ToString();
				base.IsAcceptableItemSelected = this.CurrentSelectedClan != null;
			}
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00030684 File Offset: 0x0002E884
		private TextObject GetExpelLikelihoodText(KingdomClanItemVM clan)
		{
			ExpelClanFromKingdomDecision expelClanFromKingdomDecision = new ExpelClanFromKingdomDecision(Clan.PlayerClan, clan.Clan);
			return GameTexts.FindText("str_decision_outcome_support_status", KingdomElection.GetElectionOutcomeSupport(expelClanFromKingdomDecision, Clan.PlayerClan).ToString());
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x000306C8 File Offset: 0x0002E8C8
		private bool GetCanSupportCurrentClanWithReason(int supportCost, out TextObject disabledReason)
		{
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			if (Hero.MainHero.Clan.Influence < (float)supportCost)
			{
				disabledReason = GameTexts.FindText("str_warning_you_dont_have_enough_influence", null);
				return false;
			}
			if (this.CurrentSelectedClan.Clan == Clan.PlayerClan)
			{
				disabledReason = GameTexts.FindText("str_cannot_support_your_clan", null);
				return false;
			}
			if (Hero.MainHero.Clan.IsUnderMercenaryService)
			{
				disabledReason = GameTexts.FindText("str_mercenaries_cannot_support_clans", null);
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x00030750 File Offset: 0x0002E950
		private bool GetCanExpelCurrentClanWithReason(bool isThereAPendingDecision, int expelCost, out TextObject disabledReason)
		{
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			if (Hero.MainHero.Clan.IsUnderMercenaryService)
			{
				disabledReason = GameTexts.FindText("str_mercenaries_cannot_expel_clans", null);
				return false;
			}
			if (!isThereAPendingDecision)
			{
				if (Hero.MainHero.Clan.Influence < (float)expelCost)
				{
					disabledReason = GameTexts.FindText("str_warning_you_dont_have_enough_influence", null);
					return false;
				}
				if (this.CurrentSelectedClan.Clan == Clan.PlayerClan)
				{
					disabledReason = GameTexts.FindText("str_cannot_expel_your_clan", null);
					return false;
				}
				Clan clan = this.CurrentSelectedClan.Clan;
				Kingdom kingdom = this.CurrentSelectedClan.Clan.Kingdom;
				if (clan == ((kingdom != null) ? kingdom.RulingClan : null))
				{
					disabledReason = GameTexts.FindText("str_cannot_expel_ruling_clan", null);
					return false;
				}
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x00030814 File Offset: 0x0002EA14
		public void RefreshClan()
		{
			this.RefreshClanList();
			foreach (KingdomClanItemVM kingdomClanItemVM in this.Clans)
			{
				kingdomClanItemVM.Refresh();
			}
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00030864 File Offset: 0x0002EA64
		public void SelectClan(Clan clan)
		{
			foreach (KingdomClanItemVM kingdomClanItemVM in this.Clans)
			{
				if (kingdomClanItemVM.Clan == clan)
				{
					this.OnClanSelection(kingdomClanItemVM);
					break;
				}
			}
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x000308BC File Offset: 0x0002EABC
		private void OnClanSelection(KingdomClanItemVM clan)
		{
			if (this._currentSelectedClan != clan)
			{
				this.SetCurrentSelectedClan(clan);
			}
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x000308D0 File Offset: 0x0002EAD0
		private void ExecuteExpelCurrentClan()
		{
			if (Hero.MainHero.Clan.Influence >= (float)this.ExpelCost)
			{
				KingdomDecision kingdomDecision = new ExpelClanFromKingdomDecision(Clan.PlayerClan, this._currentSelectedClan.Clan);
				Clan.PlayerClan.Kingdom.AddDecision(kingdomDecision, false);
				this._forceDecide(kingdomDecision);
			}
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00030928 File Offset: 0x0002EB28
		private void ExecuteSupport()
		{
			if (Hero.MainHero.Clan.Influence >= (float)this.SupportCost)
			{
				this._currentSelectedClan.Clan.OnSupportedByClan(Hero.MainHero.Clan);
				Clan clan = this._currentSelectedClan.Clan;
				this.RefreshClan();
				this.SelectClan(clan);
			}
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00030980 File Offset: 0x0002EB80
		private int CalculateExpelLikelihood(KingdomClanItemVM clan)
		{
			return MathF.Round(new KingdomElection(new ExpelClanFromKingdomDecision(Clan.PlayerClan, clan.Clan)).GetLikelihoodForSponsor(Clan.PlayerClan) * 100f);
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x000309AC File Offset: 0x0002EBAC
		private void RefreshClanList()
		{
			this.Clans.Clear();
			if (Clan.PlayerClan.Kingdom != null)
			{
				foreach (Clan clan in Clan.PlayerClan.Kingdom.Clans)
				{
					this.Clans.Add(new KingdomClanItemVM(clan, new Action<KingdomClanItemVM>(this.OnClanSelection)));
				}
			}
			if (this.Clans.Count > 0)
			{
				this.SetCurrentSelectedClan(this.Clans.FirstOrDefault<KingdomClanItemVM>());
			}
			if (this.ClanSortController != null)
			{
				this.ClanSortController.SortByCurrentState();
			}
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x00030A68 File Offset: 0x0002EC68
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.OnClanChangedKingdomEvent.ClearListeners(this);
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x00030A7B File Offset: 0x0002EC7B
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification)
		{
			if (clan != Clan.PlayerClan && (oldKingdom == Clan.PlayerClan.Kingdom || newKingdom == Clan.PlayerClan.Kingdom))
			{
				this.RefreshClanList();
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000B7E RID: 2942 RVA: 0x00030AA5 File Offset: 0x0002ECA5
		// (set) Token: 0x06000B7F RID: 2943 RVA: 0x00030AAD File Offset: 0x0002ECAD
		[DataSourceProperty]
		public KingdomClanSortControllerVM ClanSortController
		{
			get
			{
				return this._clanSortController;
			}
			set
			{
				if (value != this._clanSortController)
				{
					this._clanSortController = value;
					base.OnPropertyChangedWithValue<KingdomClanSortControllerVM>(value, "ClanSortController");
				}
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x00030ACB File Offset: 0x0002ECCB
		// (set) Token: 0x06000B81 RID: 2945 RVA: 0x00030AD3 File Offset: 0x0002ECD3
		[DataSourceProperty]
		public KingdomClanItemVM CurrentSelectedClan
		{
			get
			{
				return this._currentSelectedClan;
			}
			set
			{
				if (value != this._currentSelectedClan)
				{
					this._currentSelectedClan = value;
					base.OnPropertyChangedWithValue<KingdomClanItemVM>(value, "CurrentSelectedClan");
				}
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x00030AF1 File Offset: 0x0002ECF1
		// (set) Token: 0x06000B83 RID: 2947 RVA: 0x00030AF9 File Offset: 0x0002ECF9
		[DataSourceProperty]
		public string ExpelActionExplanationText
		{
			get
			{
				return this._expelActionExplanationText;
			}
			set
			{
				if (value != this._expelActionExplanationText)
				{
					this._expelActionExplanationText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExpelActionExplanationText");
				}
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000B84 RID: 2948 RVA: 0x00030B1C File Offset: 0x0002ED1C
		// (set) Token: 0x06000B85 RID: 2949 RVA: 0x00030B24 File Offset: 0x0002ED24
		[DataSourceProperty]
		public string SupportActionExplanationText
		{
			get
			{
				return this._supportActionExplanationText;
			}
			set
			{
				if (value != this._supportActionExplanationText)
				{
					this._supportActionExplanationText = value;
					base.OnPropertyChangedWithValue<string>(value, "SupportActionExplanationText");
				}
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x00030B47 File Offset: 0x0002ED47
		// (set) Token: 0x06000B87 RID: 2951 RVA: 0x00030B4F File Offset: 0x0002ED4F
		[DataSourceProperty]
		public string BannerText
		{
			get
			{
				return this._bannerText;
			}
			set
			{
				if (value != this._bannerText)
				{
					this._bannerText = value;
					base.OnPropertyChangedWithValue<string>(value, "BannerText");
				}
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000B88 RID: 2952 RVA: 0x00030B72 File Offset: 0x0002ED72
		// (set) Token: 0x06000B89 RID: 2953 RVA: 0x00030B7A File Offset: 0x0002ED7A
		[DataSourceProperty]
		public string TypeText
		{
			get
			{
				return this._typeText;
			}
			set
			{
				if (value != this._typeText)
				{
					this._typeText = value;
					base.OnPropertyChangedWithValue<string>(value, "TypeText");
				}
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000B8A RID: 2954 RVA: 0x00030B9D File Offset: 0x0002ED9D
		// (set) Token: 0x06000B8B RID: 2955 RVA: 0x00030BA5 File Offset: 0x0002EDA5
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000B8C RID: 2956 RVA: 0x00030BC8 File Offset: 0x0002EDC8
		// (set) Token: 0x06000B8D RID: 2957 RVA: 0x00030BD0 File Offset: 0x0002EDD0
		[DataSourceProperty]
		public string InfluenceText
		{
			get
			{
				return this._influenceText;
			}
			set
			{
				if (value != this._influenceText)
				{
					this._influenceText = value;
					base.OnPropertyChangedWithValue<string>(value, "InfluenceText");
				}
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x00030BF3 File Offset: 0x0002EDF3
		// (set) Token: 0x06000B8F RID: 2959 RVA: 0x00030BFB File Offset: 0x0002EDFB
		[DataSourceProperty]
		public string FiefsText
		{
			get
			{
				return this._fiefsText;
			}
			set
			{
				if (value != this._fiefsText)
				{
					this._fiefsText = value;
					base.OnPropertyChangedWithValue<string>(value, "FiefsText");
				}
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x00030C1E File Offset: 0x0002EE1E
		// (set) Token: 0x06000B91 RID: 2961 RVA: 0x00030C26 File Offset: 0x0002EE26
		[DataSourceProperty]
		public string MembersText
		{
			get
			{
				return this._membersText;
			}
			set
			{
				if (value != this._membersText)
				{
					this._membersText = value;
					base.OnPropertyChangedWithValue<string>(value, "MembersText");
				}
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x00030C49 File Offset: 0x0002EE49
		// (set) Token: 0x06000B93 RID: 2963 RVA: 0x00030C51 File Offset: 0x0002EE51
		[DataSourceProperty]
		public MBBindingList<KingdomClanItemVM> Clans
		{
			get
			{
				return this._clans;
			}
			set
			{
				if (value != this._clans)
				{
					this._clans = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomClanItemVM>>(value, "Clans");
				}
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x00030C6F File Offset: 0x0002EE6F
		// (set) Token: 0x06000B95 RID: 2965 RVA: 0x00030C77 File Offset: 0x0002EE77
		[DataSourceProperty]
		public bool CanSupportCurrentClan
		{
			get
			{
				return this._canSupportCurrentClan;
			}
			set
			{
				if (value != this._canSupportCurrentClan)
				{
					this._canSupportCurrentClan = value;
					base.OnPropertyChangedWithValue(value, "CanSupportCurrentClan");
				}
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x00030C95 File Offset: 0x0002EE95
		// (set) Token: 0x06000B97 RID: 2967 RVA: 0x00030C9D File Offset: 0x0002EE9D
		[DataSourceProperty]
		public bool CanExpelCurrentClan
		{
			get
			{
				return this._canExpelCurrentClan;
			}
			set
			{
				if (value != this._canExpelCurrentClan)
				{
					this._canExpelCurrentClan = value;
					base.OnPropertyChangedWithValue(value, "CanExpelCurrentClan");
				}
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x00030CBB File Offset: 0x0002EEBB
		// (set) Token: 0x06000B99 RID: 2969 RVA: 0x00030CC3 File Offset: 0x0002EEC3
		[DataSourceProperty]
		public string SupportText
		{
			get
			{
				return this._supportText;
			}
			set
			{
				if (value != this._supportText)
				{
					this._supportText = value;
					base.OnPropertyChangedWithValue<string>(value, "SupportText");
				}
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x00030CE6 File Offset: 0x0002EEE6
		// (set) Token: 0x06000B9B RID: 2971 RVA: 0x00030CEE File Offset: 0x0002EEEE
		[DataSourceProperty]
		public string ExpelActionText
		{
			get
			{
				return this._expelActionText;
			}
			set
			{
				if (value != this._expelActionText)
				{
					this._expelActionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExpelActionText");
				}
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000B9C RID: 2972 RVA: 0x00030D11 File Offset: 0x0002EF11
		// (set) Token: 0x06000B9D RID: 2973 RVA: 0x00030D19 File Offset: 0x0002EF19
		[DataSourceProperty]
		public int SupportCost
		{
			get
			{
				return this._supportCost;
			}
			set
			{
				if (value != this._supportCost)
				{
					this._supportCost = value;
					base.OnPropertyChangedWithValue(value, "SupportCost");
				}
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x00030D37 File Offset: 0x0002EF37
		// (set) Token: 0x06000B9F RID: 2975 RVA: 0x00030D3F File Offset: 0x0002EF3F
		[DataSourceProperty]
		public int ExpelCost
		{
			get
			{
				return this._expelCost;
			}
			set
			{
				if (value != this._expelCost)
				{
					this._expelCost = value;
					base.OnPropertyChangedWithValue(value, "ExpelCost");
				}
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x00030D5D File Offset: 0x0002EF5D
		// (set) Token: 0x06000BA1 RID: 2977 RVA: 0x00030D65 File Offset: 0x0002EF65
		[DataSourceProperty]
		public HintViewModel ExpelHint
		{
			get
			{
				return this._expelHint;
			}
			set
			{
				if (value != this._expelHint)
				{
					this._expelHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ExpelHint");
				}
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x00030D83 File Offset: 0x0002EF83
		// (set) Token: 0x06000BA3 RID: 2979 RVA: 0x00030D8B File Offset: 0x0002EF8B
		[DataSourceProperty]
		public HintViewModel SupportHint
		{
			get
			{
				return this._supportHint;
			}
			set
			{
				if (value != this._supportHint)
				{
					this._supportHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "SupportHint");
				}
			}
		}

		// Token: 0x0400051B RID: 1307
		private Action<KingdomDecision> _forceDecide;

		// Token: 0x0400051C RID: 1308
		private bool _isThereAPendingDecisionToExpelThisClan;

		// Token: 0x0400051D RID: 1309
		private MBBindingList<KingdomClanItemVM> _clans;

		// Token: 0x0400051E RID: 1310
		private HintViewModel _expelHint;

		// Token: 0x0400051F RID: 1311
		private HintViewModel _supportHint;

		// Token: 0x04000520 RID: 1312
		private string _bannerText;

		// Token: 0x04000521 RID: 1313
		private string _nameText;

		// Token: 0x04000522 RID: 1314
		private string _influenceText;

		// Token: 0x04000523 RID: 1315
		private string _membersText;

		// Token: 0x04000524 RID: 1316
		private string _fiefsText;

		// Token: 0x04000525 RID: 1317
		private string _typeText;

		// Token: 0x04000526 RID: 1318
		private string _expelActionText;

		// Token: 0x04000527 RID: 1319
		private string _expelActionExplanationText;

		// Token: 0x04000528 RID: 1320
		private string _supportActionExplanationText;

		// Token: 0x04000529 RID: 1321
		private int _expelCost;

		// Token: 0x0400052A RID: 1322
		private string _supportText;

		// Token: 0x0400052B RID: 1323
		private int _supportCost;

		// Token: 0x0400052C RID: 1324
		private bool _canSupportCurrentClan;

		// Token: 0x0400052D RID: 1325
		private bool _canExpelCurrentClan;

		// Token: 0x0400052E RID: 1326
		private KingdomClanItemVM _currentSelectedClan;

		// Token: 0x0400052F RID: 1327
		private KingdomClanSortControllerVM _clanSortController;
	}
}
