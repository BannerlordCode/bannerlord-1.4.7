using System;
using Helpers;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions.ItemTypes
{
	// Token: 0x0200007F RID: 127
	public class MakePeaceDecisionItemVM : DecisionItemBaseVM
	{
		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x0002D6F9 File Offset: 0x0002B8F9
		private Kingdom _sourceFaction
		{
			get
			{
				return Hero.MainHero.Clan.Kingdom;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x0002D70A File Offset: 0x0002B90A
		public IFaction TargetFaction
		{
			get
			{
				return (this._decision as MakePeaceKingdomDecision).FactionToMakePeaceWith;
			}
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x0002D71C File Offset: 0x0002B91C
		public MakePeaceDecisionItemVM(MakePeaceKingdomDecision decision, Action onDecisionOver)
			: base(decision, onDecisionOver)
		{
			this._makePeaceDecision = decision;
			base.DecisionType = 5;
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0002D734 File Offset: 0x0002B934
		protected override void InitValues()
		{
			base.InitValues();
			TextObject textObject = GameTexts.FindText("str_kingdom_decision_make_peace", null);
			this.NameText = textObject.ToString();
			TextObject textObject2 = GameTexts.FindText("str_kingdom_decision_make_peace_desc", null);
			textObject2.SetTextVariable("FACTION", this.TargetFaction.Name);
			this.PeaceDescriptionText = textObject2.ToString();
			this.SourceFactionBanner = new BannerImageIdentifierVM(this._sourceFaction.Banner, true);
			this.TargetFactionBanner = new BannerImageIdentifierVM(this.TargetFaction.Banner, true);
			this.LeaderText = GameTexts.FindText("str_leader", null).ToString();
			this.SourceFactionLeader = new HeroVM(this._sourceFaction.Leader, false);
			this.TargetFactionLeader = new HeroVM(this.TargetFaction.Leader, false);
			this.ComparedStats = new MBBindingList<KingdomWarComparableStatVM>();
			Kingdom targetFaction = this.TargetFaction as Kingdom;
			string text = Color.FromUint(this._sourceFaction.Color).ToString();
			string text2 = Color.FromUint(targetFaction.Color).ToString();
			StanceLink stanceWith = this._sourceFaction.GetStanceWith(this.TargetFaction);
			KingdomWarComparableStatVM kingdomWarComparableStatVM = new KingdomWarComparableStatVM((int)this._sourceFaction.CurrentTotalStrength, (int)targetFaction.CurrentTotalStrength, GameTexts.FindText("str_strength", null), text, text2, 10000, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM);
			KingdomWarComparableStatVM kingdomWarComparableStatVM2 = new KingdomWarComparableStatVM(stanceWith.GetCasualties(targetFaction), stanceWith.GetCasualties(this._sourceFaction), GameTexts.FindText("str_war_casualties_inflicted", null), text, text2, 10000, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM2);
			KingdomWarComparableStatVM kingdomWarComparableStatVM3 = new KingdomWarComparableStatVM(stanceWith.GetSuccessfulSieges(this._sourceFaction), stanceWith.GetSuccessfulSieges(targetFaction), GameTexts.FindText("str_war_successful_sieges", null), text, text2, 5, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM3);
			KingdomWarComparableStatVM kingdomWarComparableStatVM4 = new KingdomWarComparableStatVM(stanceWith.GetSuccessfulRaids(this._sourceFaction), stanceWith.GetSuccessfulRaids(targetFaction), GameTexts.FindText("str_war_successful_raids", null), text, text2, 10, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM4);
			ExplainedNumber warProgressOfFaction1 = Campaign.Current.Models.DiplomacyModel.GetWarProgressScore(this._sourceFaction, targetFaction, true);
			ExplainedNumber warProgressOfFaction2 = Campaign.Current.Models.DiplomacyModel.GetWarProgressScore(targetFaction, this._sourceFaction, true);
			int num = (int)(warProgressOfFaction1.ResultNumber * 100f / warProgressOfFaction1.LimitMaxValue);
			int num2 = (int)(warProgressOfFaction2.ResultNumber * 100f / warProgressOfFaction2.LimitMaxValue);
			int num3 = MathF.Max(0, num - num2);
			int num4 = MathF.Max(0, num2 - num);
			KingdomWarComparableStatVM kingdomWarComparableStatVM5 = new KingdomWarComparableStatVM(num3, num4, new TextObject("{=8qbkS5D2}War Progress", null), text, text2, 100, new BasicTooltipViewModel(() => CampaignUIHelper.GetNormalizedWarProgressTooltip(warProgressOfFaction1, warProgressOfFaction2, warProgressOfFaction1.LimitMaxValue, this._sourceFaction.Name, targetFaction.Name)), new BasicTooltipViewModel(() => CampaignUIHelper.GetNormalizedWarProgressTooltip(warProgressOfFaction2, warProgressOfFaction1, warProgressOfFaction2.LimitMaxValue, targetFaction.Name, this._sourceFaction.Name)));
			this.ComparedStats.Add(kingdomWarComparableStatVM5);
			this.TargetFactionOtherWars = new MBBindingList<KingdomDiplomacyFactionItemVM>();
			foreach (StanceLink stanceLink in FactionHelper.GetStances(this.TargetFaction))
			{
				if (stanceLink.IsAtWar && stanceLink.Faction1 != this._sourceFaction && stanceLink.Faction2 != this._sourceFaction && (stanceLink.Faction1.IsKingdomFaction || stanceLink.Faction1.Leader == Hero.MainHero) && (stanceLink.Faction2.IsKingdomFaction || stanceLink.Faction2.Leader == Hero.MainHero) && !stanceLink.Faction1.IsRebelClan && !stanceLink.Faction2.IsRebelClan && !stanceLink.Faction1.IsBanditFaction && !stanceLink.Faction2.IsBanditFaction)
				{
					this.TargetFactionOtherWars.Add(new KingdomDiplomacyFactionItemVM((stanceLink.Faction1 == this.TargetFaction) ? stanceLink.Faction2 : stanceLink.Faction1));
				}
			}
			this.IsTargetFactionOtherWarsVisible = this.TargetFactionOtherWars.Count > 0;
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x0002DBB4 File Offset: 0x0002BDB4
		// (set) Token: 0x06000A85 RID: 2693 RVA: 0x0002DBBC File Offset: 0x0002BDBC
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

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x0002DBDF File Offset: 0x0002BDDF
		// (set) Token: 0x06000A87 RID: 2695 RVA: 0x0002DBE7 File Offset: 0x0002BDE7
		[DataSourceProperty]
		public string PeaceDescriptionText
		{
			get
			{
				return this._peaceDescriptionText;
			}
			set
			{
				if (value != this._peaceDescriptionText)
				{
					this._peaceDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "PeaceDescriptionText");
				}
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x0002DC0A File Offset: 0x0002BE0A
		// (set) Token: 0x06000A89 RID: 2697 RVA: 0x0002DC12 File Offset: 0x0002BE12
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

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x0002DC30 File Offset: 0x0002BE30
		// (set) Token: 0x06000A8B RID: 2699 RVA: 0x0002DC38 File Offset: 0x0002BE38
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

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000A8C RID: 2700 RVA: 0x0002DC56 File Offset: 0x0002BE56
		// (set) Token: 0x06000A8D RID: 2701 RVA: 0x0002DC5E File Offset: 0x0002BE5E
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

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x0002DC7C File Offset: 0x0002BE7C
		// (set) Token: 0x06000A8F RID: 2703 RVA: 0x0002DC84 File Offset: 0x0002BE84
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

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x0002DCA7 File Offset: 0x0002BEA7
		// (set) Token: 0x06000A91 RID: 2705 RVA: 0x0002DCAF File Offset: 0x0002BEAF
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

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x0002DCCD File Offset: 0x0002BECD
		// (set) Token: 0x06000A93 RID: 2707 RVA: 0x0002DCD5 File Offset: 0x0002BED5
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

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x0002DCF3 File Offset: 0x0002BEF3
		// (set) Token: 0x06000A95 RID: 2709 RVA: 0x0002DCFB File Offset: 0x0002BEFB
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

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x0002DD19 File Offset: 0x0002BF19
		// (set) Token: 0x06000A97 RID: 2711 RVA: 0x0002DD21 File Offset: 0x0002BF21
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

		// Token: 0x040004AA RID: 1194
		private readonly MakePeaceKingdomDecision _makePeaceDecision;

		// Token: 0x040004AB RID: 1195
		private string _nameText;

		// Token: 0x040004AC RID: 1196
		private string _peaceDescriptionText;

		// Token: 0x040004AD RID: 1197
		private BannerImageIdentifierVM _sourceFactionBanner;

		// Token: 0x040004AE RID: 1198
		private BannerImageIdentifierVM _targetFactionBanner;

		// Token: 0x040004AF RID: 1199
		private string _leaderText;

		// Token: 0x040004B0 RID: 1200
		private HeroVM _sourceFactionLeader;

		// Token: 0x040004B1 RID: 1201
		private HeroVM _targetFactionLeader;

		// Token: 0x040004B2 RID: 1202
		private MBBindingList<KingdomWarComparableStatVM> _comparedStats;

		// Token: 0x040004B3 RID: 1203
		private bool _isTargetFactionOtherWarsVisible;

		// Token: 0x040004B4 RID: 1204
		private MBBindingList<KingdomDiplomacyFactionItemVM> _targetFactionOtherWars;
	}
}
