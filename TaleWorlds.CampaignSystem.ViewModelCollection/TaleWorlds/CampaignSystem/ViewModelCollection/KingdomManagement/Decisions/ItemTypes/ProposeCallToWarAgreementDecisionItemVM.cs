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
	// Token: 0x02000081 RID: 129
	public class ProposeCallToWarAgreementDecisionItemVM : DecisionItemBaseVM
	{
		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x0002DEA2 File Offset: 0x0002C0A2
		private Kingdom _calledKingdom
		{
			get
			{
				return (this._decision as ProposeCallToWarAgreementDecision).CalledKingdom;
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x0002DEB4 File Offset: 0x0002C0B4
		public IFaction TargetFaction
		{
			get
			{
				return (this._decision as ProposeCallToWarAgreementDecision).KingdomToCallToWarAgainst;
			}
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0002DEC6 File Offset: 0x0002C0C6
		public ProposeCallToWarAgreementDecisionItemVM(ProposeCallToWarAgreementDecision decision, Action onDecisionOver)
			: base(decision, onDecisionOver)
		{
			this._proposeCallToWarAgreementDecision = decision;
			base.DecisionType = 9;
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0002DEE0 File Offset: 0x0002C0E0
		protected override void InitValues()
		{
			base.InitValues();
			TextObject textObject = GameTexts.FindText("str_kingdom_decision_propose_call_to_war_agreement", null);
			this.NameText = textObject.ToString();
			TextObject textObject2 = GameTexts.FindText("str_kingdom_decision_propose_call_to_war_agreement_desc", null);
			textObject2.SetTextVariable("CALLED_KINGDOM", this._calledKingdom.Name);
			textObject2.SetTextVariable("KINGDOM_TO_CALL_TO_WAR_AGAINST", this.TargetFaction.Name);
			this.ProposeCallToWarAgreementDescriptionText = textObject2.ToString();
			this.SourceFactionBanner = new BannerImageIdentifierVM(this._calledKingdom.Banner, true);
			this.TargetFactionBanner = new BannerImageIdentifierVM(this.TargetFaction.Banner, true);
			this.LeaderText = GameTexts.FindText("str_leader", null).ToString();
			this.SourceFactionLeader = new HeroVM(this._calledKingdom.Leader, false);
			this.TargetFactionLeader = new HeroVM(this.TargetFaction.Leader, false);
			this.ComparedStats = new MBBindingList<KingdomWarComparableStatVM>();
			Kingdom kingdom = this.TargetFaction as Kingdom;
			string text = Color.FromUint(this._calledKingdom.Color).ToString();
			string text2 = Color.FromUint(kingdom.Color).ToString();
			KingdomWarComparableStatVM kingdomWarComparableStatVM = new KingdomWarComparableStatVM((int)this._calledKingdom.CurrentTotalStrength, (int)kingdom.CurrentTotalStrength, GameTexts.FindText("str_strength", null), text, text2, 10000, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM);
			this.TargetFactionOtherWars = new MBBindingList<KingdomDiplomacyFactionItemVM>();
			foreach (StanceLink stanceLink in FactionHelper.GetStances(this.TargetFaction))
			{
				if (stanceLink.IsAtWar && stanceLink.Faction1 != this._calledKingdom && stanceLink.Faction2 != this._calledKingdom && (stanceLink.Faction1.IsKingdomFaction || stanceLink.Faction1.Leader == Hero.MainHero) && (stanceLink.Faction2.IsKingdomFaction || stanceLink.Faction2.Leader == Hero.MainHero) && !stanceLink.Faction1.IsRebelClan && !stanceLink.Faction2.IsRebelClan && !stanceLink.Faction1.IsBanditFaction && !stanceLink.Faction2.IsBanditFaction)
				{
					this.TargetFactionOtherWars.Add(new KingdomDiplomacyFactionItemVM((stanceLink.Faction1 == this.TargetFaction) ? stanceLink.Faction2 : stanceLink.Faction1));
				}
			}
			this.IsTargetFactionOtherWarsVisible = this.TargetFactionOtherWars.Count > 0;
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x0002E198 File Offset: 0x0002C398
		// (set) Token: 0x06000AA7 RID: 2727 RVA: 0x0002E1A0 File Offset: 0x0002C3A0
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

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x0002E1C3 File Offset: 0x0002C3C3
		// (set) Token: 0x06000AA9 RID: 2729 RVA: 0x0002E1CB File Offset: 0x0002C3CB
		[DataSourceProperty]
		public string ProposeCallToWarAgreementDescriptionText
		{
			get
			{
				return this._proposeCallToWarAgreementDescriptionText;
			}
			set
			{
				if (value != this._proposeCallToWarAgreementDescriptionText)
				{
					this._proposeCallToWarAgreementDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProposeCallToWarAgreementDescriptionText");
				}
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000AAA RID: 2730 RVA: 0x0002E1EE File Offset: 0x0002C3EE
		// (set) Token: 0x06000AAB RID: 2731 RVA: 0x0002E1F6 File Offset: 0x0002C3F6
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

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000AAC RID: 2732 RVA: 0x0002E214 File Offset: 0x0002C414
		// (set) Token: 0x06000AAD RID: 2733 RVA: 0x0002E21C File Offset: 0x0002C41C
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

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x0002E23A File Offset: 0x0002C43A
		// (set) Token: 0x06000AAF RID: 2735 RVA: 0x0002E242 File Offset: 0x0002C442
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

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x0002E260 File Offset: 0x0002C460
		// (set) Token: 0x06000AB1 RID: 2737 RVA: 0x0002E268 File Offset: 0x0002C468
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

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x0002E28B File Offset: 0x0002C48B
		// (set) Token: 0x06000AB3 RID: 2739 RVA: 0x0002E293 File Offset: 0x0002C493
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

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x0002E2B1 File Offset: 0x0002C4B1
		// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x0002E2B9 File Offset: 0x0002C4B9
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

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x0002E2D7 File Offset: 0x0002C4D7
		// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x0002E2DF File Offset: 0x0002C4DF
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

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x0002E2FD File Offset: 0x0002C4FD
		// (set) Token: 0x06000AB9 RID: 2745 RVA: 0x0002E305 File Offset: 0x0002C505
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

		// Token: 0x040004B9 RID: 1209
		private readonly ProposeCallToWarAgreementDecision _proposeCallToWarAgreementDecision;

		// Token: 0x040004BA RID: 1210
		private string _nameText;

		// Token: 0x040004BB RID: 1211
		private string _proposeCallToWarAgreementDescriptionText;

		// Token: 0x040004BC RID: 1212
		private BannerImageIdentifierVM _sourceFactionBanner;

		// Token: 0x040004BD RID: 1213
		private BannerImageIdentifierVM _targetFactionBanner;

		// Token: 0x040004BE RID: 1214
		private string _leaderText;

		// Token: 0x040004BF RID: 1215
		private HeroVM _sourceFactionLeader;

		// Token: 0x040004C0 RID: 1216
		private HeroVM _targetFactionLeader;

		// Token: 0x040004C1 RID: 1217
		private MBBindingList<KingdomWarComparableStatVM> _comparedStats;

		// Token: 0x040004C2 RID: 1218
		private bool _isTargetFactionOtherWarsVisible;

		// Token: 0x040004C3 RID: 1219
		private MBBindingList<KingdomDiplomacyFactionItemVM> _targetFactionOtherWars;
	}
}
