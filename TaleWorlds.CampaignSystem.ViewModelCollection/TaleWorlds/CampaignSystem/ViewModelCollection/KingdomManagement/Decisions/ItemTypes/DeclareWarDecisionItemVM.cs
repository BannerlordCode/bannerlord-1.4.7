using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions.ItemTypes
{
	// Token: 0x0200007B RID: 123
	public class DeclareWarDecisionItemVM : DecisionItemBaseVM
	{
		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x0002C5E9 File Offset: 0x0002A7E9
		private Kingdom _sourceFaction
		{
			get
			{
				return Hero.MainHero.Clan.Kingdom;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000A30 RID: 2608 RVA: 0x0002C5FA File Offset: 0x0002A7FA
		public IFaction TargetFaction
		{
			get
			{
				return (this._decision as DeclareWarDecision).FactionToDeclareWarOn;
			}
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0002C60C File Offset: 0x0002A80C
		public DeclareWarDecisionItemVM(DeclareWarDecision decision, Action onDecisionOver)
			: base(decision, onDecisionOver)
		{
			this._declareWarDecision = decision;
			base.DecisionType = 4;
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0002C624 File Offset: 0x0002A824
		protected override void InitValues()
		{
			base.InitValues();
			TextObject textObject = GameTexts.FindText("str_kingdom_decision_declare_war", null);
			this.NameText = textObject.ToString();
			TextObject textObject2 = GameTexts.FindText("str_kingdom_decision_declare_war_desc", null);
			textObject2.SetTextVariable("FACTION", this.TargetFaction.Name);
			this.WarDescriptionText = textObject2.ToString();
			this.SourceFactionBanner = new BannerImageIdentifierVM(this._sourceFaction.Banner, true);
			this.TargetFactionBanner = new BannerImageIdentifierVM(this.TargetFaction.Banner, true);
			this.LeaderText = GameTexts.FindText("str_leader", null).ToString();
			this.SourceFactionLeader = new HeroVM(this._sourceFaction.Leader, false);
			this.TargetFactionLeader = new HeroVM(this.TargetFaction.Leader, false);
			this.ComparedStats = new MBBindingList<KingdomWarComparableStatVM>();
			Kingdom kingdom = this.TargetFaction as Kingdom;
			string text = Color.FromUint(this._sourceFaction.Color).ToString();
			string text2 = Color.FromUint(kingdom.Color).ToString();
			KingdomWarComparableStatVM kingdomWarComparableStatVM = new KingdomWarComparableStatVM((int)this._sourceFaction.CurrentTotalStrength, (int)kingdom.CurrentTotalStrength, GameTexts.FindText("str_strength", null), text, text2, 10000, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM);
			KingdomWarComparableStatVM kingdomWarComparableStatVM2 = new KingdomWarComparableStatVM(this._sourceFaction.Armies.Count, kingdom.Armies.Count, GameTexts.FindText("str_armies", null), text, text2, 5, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM2);
			int num = this._sourceFaction.Settlements.Count<Settlement>((Settlement settlement) => settlement.IsTown);
			int num2 = kingdom.Settlements.Count<Settlement>((Settlement settlement) => settlement.IsTown);
			KingdomWarComparableStatVM kingdomWarComparableStatVM3 = new KingdomWarComparableStatVM(num, num2, GameTexts.FindText("str_towns", null), text, text2, 50, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM3);
			int num3 = this._sourceFaction.Settlements.Count<Settlement>((Settlement settlement) => settlement.IsCastle);
			int num4 = this.TargetFaction.Settlements.Count<Settlement>((Settlement settlement) => settlement.IsCastle);
			KingdomWarComparableStatVM kingdomWarComparableStatVM4 = new KingdomWarComparableStatVM(num3, num4, GameTexts.FindText("str_castles", null), text, text2, 50, null, null);
			this.ComparedStats.Add(kingdomWarComparableStatVM4);
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

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x0002CA10 File Offset: 0x0002AC10
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x0002CA18 File Offset: 0x0002AC18
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

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x0002CA3B File Offset: 0x0002AC3B
		// (set) Token: 0x06000A36 RID: 2614 RVA: 0x0002CA43 File Offset: 0x0002AC43
		[DataSourceProperty]
		public string WarDescriptionText
		{
			get
			{
				return this._warDescriptionText;
			}
			set
			{
				if (value != this._warDescriptionText)
				{
					this._warDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "WarDescriptionText");
				}
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0002CA66 File Offset: 0x0002AC66
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x0002CA6E File Offset: 0x0002AC6E
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

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x0002CA8C File Offset: 0x0002AC8C
		// (set) Token: 0x06000A3A RID: 2618 RVA: 0x0002CA94 File Offset: 0x0002AC94
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

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x0002CAB2 File Offset: 0x0002ACB2
		// (set) Token: 0x06000A3C RID: 2620 RVA: 0x0002CABA File Offset: 0x0002ACBA
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

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x0002CAD8 File Offset: 0x0002ACD8
		// (set) Token: 0x06000A3E RID: 2622 RVA: 0x0002CAE0 File Offset: 0x0002ACE0
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

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x0002CB03 File Offset: 0x0002AD03
		// (set) Token: 0x06000A40 RID: 2624 RVA: 0x0002CB0B File Offset: 0x0002AD0B
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

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x0002CB29 File Offset: 0x0002AD29
		// (set) Token: 0x06000A42 RID: 2626 RVA: 0x0002CB31 File Offset: 0x0002AD31
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

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000A43 RID: 2627 RVA: 0x0002CB4F File Offset: 0x0002AD4F
		// (set) Token: 0x06000A44 RID: 2628 RVA: 0x0002CB57 File Offset: 0x0002AD57
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

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x0002CB75 File Offset: 0x0002AD75
		// (set) Token: 0x06000A46 RID: 2630 RVA: 0x0002CB7D File Offset: 0x0002AD7D
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

		// Token: 0x04000486 RID: 1158
		private readonly DeclareWarDecision _declareWarDecision;

		// Token: 0x04000487 RID: 1159
		private string _nameText;

		// Token: 0x04000488 RID: 1160
		private string _warDescriptionText;

		// Token: 0x04000489 RID: 1161
		private BannerImageIdentifierVM _sourceFactionBanner;

		// Token: 0x0400048A RID: 1162
		private BannerImageIdentifierVM _targetFactionBanner;

		// Token: 0x0400048B RID: 1163
		private string _leaderText;

		// Token: 0x0400048C RID: 1164
		private HeroVM _sourceFactionLeader;

		// Token: 0x0400048D RID: 1165
		private HeroVM _targetFactionLeader;

		// Token: 0x0400048E RID: 1166
		private MBBindingList<KingdomWarComparableStatVM> _comparedStats;

		// Token: 0x0400048F RID: 1167
		private bool _isTargetFactionOtherWarsVisible;

		// Token: 0x04000490 RID: 1168
		private MBBindingList<KingdomDiplomacyFactionItemVM> _targetFactionOtherWars;
	}
}
