using System;
using Helpers;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions.ItemTypes
{
	// Token: 0x02000079 RID: 121
	public class AcceptingCallToWarAgreementDecisionItemVM : DecisionItemBaseVM
	{
		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x060009E2 RID: 2530 RVA: 0x0002B2E7 File Offset: 0x000294E7
		private Kingdom _callingKingdom
		{
			get
			{
				return (this._decision as AcceptCallToWarAgreementDecision).CallingKingdom;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x0002B2F9 File Offset: 0x000294F9
		public IFaction TargetFaction
		{
			get
			{
				return (this._decision as AcceptCallToWarAgreementDecision).KingdomToCallToWarAgainst;
			}
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0002B30B File Offset: 0x0002950B
		public AcceptingCallToWarAgreementDecisionItemVM(AcceptCallToWarAgreementDecision decision, Action onDecisionOver)
			: base(decision, onDecisionOver)
		{
			this._callToWarAgreementDecision = decision;
			base.DecisionType = 8;
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0002B324 File Offset: 0x00029524
		protected override void InitValues()
		{
			base.InitValues();
			TextObject textObject = GameTexts.FindText("str_kingdom_decision_accept_call_to_war_agreement", null);
			this.NameText = textObject.ToString();
			TextObject textObject2 = GameTexts.FindText("str_kingdom_decision_accept_call_to_war_agreement_desc", null);
			textObject2.SetTextVariable("CALLING_KINGDOM", this._callingKingdom.Name);
			textObject2.SetTextVariable("KINGDOM_TO_CALL_TO_WAR_AGAINST", this.TargetFaction.Name);
			this.AcceptCallToWarAgreementDescriptionText = textObject2.ToString();
			this.SourceFactionBanner = new BannerImageIdentifierVM(this._callingKingdom.Banner, true);
			this.TargetFactionBanner = new BannerImageIdentifierVM(this.TargetFaction.Banner, true);
			this.LeaderText = GameTexts.FindText("str_leader", null).ToString();
			this.SourceFactionLeader = new HeroVM(this._callingKingdom.Leader, false);
			this.TargetFactionLeader = new HeroVM(this.TargetFaction.Leader, false);
			this.ComparedStats = new MBBindingList<KingdomWarComparableStatVM>();
			Kingdom kingdom = this.TargetFaction as Kingdom;
			string text = Color.FromUint(this._callingKingdom.Color).ToString();
			string text2 = Color.FromUint(kingdom.Color).ToString();
			KingdomWarComparableStatVM kingdomWarComparableStatVM = new KingdomWarComparableStatVM((int)this._callingKingdom.CurrentTotalStrength, (int)kingdom.CurrentTotalStrength, GameTexts.FindText("str_strength", null), text, text2, 10000, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM);
			this.TargetFactionOtherWars = new MBBindingList<KingdomDiplomacyFactionItemVM>();
			foreach (StanceLink stanceLink in FactionHelper.GetStances(this.TargetFaction))
			{
				if (stanceLink.IsAtWar && stanceLink.Faction1 != this._callingKingdom && stanceLink.Faction2 != this._callingKingdom && (stanceLink.Faction1.IsKingdomFaction || stanceLink.Faction1.Leader == Hero.MainHero) && (stanceLink.Faction2.IsKingdomFaction || stanceLink.Faction2.Leader == Hero.MainHero) && !stanceLink.Faction1.IsRebelClan && !stanceLink.Faction2.IsRebelClan && !stanceLink.Faction1.IsBanditFaction && !stanceLink.Faction2.IsBanditFaction)
				{
					this.TargetFactionOtherWars.Add(new KingdomDiplomacyFactionItemVM((stanceLink.Faction1 == this.TargetFaction) ? stanceLink.Faction2 : stanceLink.Faction1));
				}
			}
			this.IsTargetFactionOtherWarsVisible = this.TargetFactionOtherWars.Count > 0;
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0002B5DC File Offset: 0x000297DC
		// (set) Token: 0x060009E7 RID: 2535 RVA: 0x0002B5E4 File Offset: 0x000297E4
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

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0002B607 File Offset: 0x00029807
		// (set) Token: 0x060009E9 RID: 2537 RVA: 0x0002B60F File Offset: 0x0002980F
		[DataSourceProperty]
		public string AcceptCallToWarAgreementDescriptionText
		{
			get
			{
				return this._callToWarAgreementDescriptionText;
			}
			set
			{
				if (value != this._callToWarAgreementDescriptionText)
				{
					this._callToWarAgreementDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "AcceptCallToWarAgreementDescriptionText");
				}
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x060009EA RID: 2538 RVA: 0x0002B632 File Offset: 0x00029832
		// (set) Token: 0x060009EB RID: 2539 RVA: 0x0002B63A File Offset: 0x0002983A
		[DataSourceProperty]
		public BannerImageIdentifierVM SourceFactionBanner
		{
			get
			{
				return this._sourceFactionBanner;
			}
			set
			{
				if (value != this._sourceFactionBanner)
				{
					this._sourceFactionBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "SourceFactionBanner");
				}
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x060009EC RID: 2540 RVA: 0x0002B658 File Offset: 0x00029858
		// (set) Token: 0x060009ED RID: 2541 RVA: 0x0002B660 File Offset: 0x00029860
		[DataSourceProperty]
		public BannerImageIdentifierVM TargetFactionBanner
		{
			get
			{
				return this._targetFactionBanner;
			}
			set
			{
				if (value != this._targetFactionBanner)
				{
					this._targetFactionBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "TargetFactionBanner");
				}
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x060009EE RID: 2542 RVA: 0x0002B67E File Offset: 0x0002987E
		// (set) Token: 0x060009EF RID: 2543 RVA: 0x0002B686 File Offset: 0x00029886
		[DataSourceProperty]
		public MBBindingList<KingdomWarComparableStatVM> ComparedStats
		{
			get
			{
				return this._comparedStats;
			}
			set
			{
				if (value != this._comparedStats)
				{
					this._comparedStats = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomWarComparableStatVM>>(value, "ComparedStats");
				}
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x060009F0 RID: 2544 RVA: 0x0002B6A4 File Offset: 0x000298A4
		// (set) Token: 0x060009F1 RID: 2545 RVA: 0x0002B6AC File Offset: 0x000298AC
		[DataSourceProperty]
		public string LeaderText
		{
			get
			{
				return this._leaderText;
			}
			set
			{
				if (value != this._leaderText)
				{
					this._leaderText = value;
					base.OnPropertyChangedWithValue<string>(value, "LeaderText");
				}
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x060009F2 RID: 2546 RVA: 0x0002B6CF File Offset: 0x000298CF
		// (set) Token: 0x060009F3 RID: 2547 RVA: 0x0002B6D7 File Offset: 0x000298D7
		[DataSourceProperty]
		public HeroVM SourceFactionLeader
		{
			get
			{
				return this._sourceFactionLeader;
			}
			set
			{
				if (value != this._sourceFactionLeader)
				{
					this._sourceFactionLeader = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "SourceFactionLeader");
				}
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x060009F4 RID: 2548 RVA: 0x0002B6F5 File Offset: 0x000298F5
		// (set) Token: 0x060009F5 RID: 2549 RVA: 0x0002B6FD File Offset: 0x000298FD
		[DataSourceProperty]
		public HeroVM TargetFactionLeader
		{
			get
			{
				return this._targetFactionLeader;
			}
			set
			{
				if (value != this._targetFactionLeader)
				{
					this._targetFactionLeader = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "TargetFactionLeader");
				}
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x060009F6 RID: 2550 RVA: 0x0002B71B File Offset: 0x0002991B
		// (set) Token: 0x060009F7 RID: 2551 RVA: 0x0002B723 File Offset: 0x00029923
		[DataSourceProperty]
		public bool IsTargetFactionOtherWarsVisible
		{
			get
			{
				return this._isTargetFactionOtherWarsVisible;
			}
			set
			{
				if (value != this._isTargetFactionOtherWarsVisible)
				{
					this._isTargetFactionOtherWarsVisible = value;
					base.OnPropertyChangedWithValue(value, "IsTargetFactionOtherWarsVisible");
				}
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x060009F8 RID: 2552 RVA: 0x0002B741 File Offset: 0x00029941
		// (set) Token: 0x060009F9 RID: 2553 RVA: 0x0002B749 File Offset: 0x00029949
		[DataSourceProperty]
		public MBBindingList<KingdomDiplomacyFactionItemVM> TargetFactionOtherWars
		{
			get
			{
				return this._targetFactionOtherWars;
			}
			set
			{
				if (value != this._targetFactionOtherWars)
				{
					this._targetFactionOtherWars = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomDiplomacyFactionItemVM>>(value, "TargetFactionOtherWars");
				}
			}
		}

		// Token: 0x04000464 RID: 1124
		private readonly AcceptCallToWarAgreementDecision _callToWarAgreementDecision;

		// Token: 0x04000465 RID: 1125
		private string _nameText;

		// Token: 0x04000466 RID: 1126
		private string _callToWarAgreementDescriptionText;

		// Token: 0x04000467 RID: 1127
		private BannerImageIdentifierVM _sourceFactionBanner;

		// Token: 0x04000468 RID: 1128
		private BannerImageIdentifierVM _targetFactionBanner;

		// Token: 0x04000469 RID: 1129
		private string _leaderText;

		// Token: 0x0400046A RID: 1130
		private HeroVM _sourceFactionLeader;

		// Token: 0x0400046B RID: 1131
		private HeroVM _targetFactionLeader;

		// Token: 0x0400046C RID: 1132
		private MBBindingList<KingdomWarComparableStatVM> _comparedStats;

		// Token: 0x0400046D RID: 1133
		private bool _isTargetFactionOtherWarsVisible;

		// Token: 0x0400046E RID: 1134
		private MBBindingList<KingdomDiplomacyFactionItemVM> _targetFactionOtherWars;
	}
}
