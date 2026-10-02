using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy
{
	// Token: 0x0200006C RID: 108
	public abstract class KingdomDiplomacyItemVM : KingdomItemVM
	{
		// Token: 0x060008B7 RID: 2231 RVA: 0x000271E8 File Offset: 0x000253E8
		protected KingdomDiplomacyItemVM(IFaction faction1, IFaction faction2)
		{
			this._playerKingdom = Hero.MainHero.MapFaction;
			if (faction1 == this._playerKingdom || faction2 == this._playerKingdom)
			{
				this.Faction1 = this._playerKingdom;
				this.Faction2 = ((faction1 != this._playerKingdom) ? faction1 : faction2);
			}
			else
			{
				this.Faction1 = faction1;
				this.Faction2 = faction2;
			}
			this._faction1Color = Color.FromUint(this.Faction1.Color).ToString();
			this._faction2Color = Color.FromUint(this.Faction2.Color).ToString();
			this.Stats = new MBBindingList<KingdomWarComparableStatVM>();
			this.PopulateSettlements();
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x000272A8 File Offset: 0x000254A8
		protected virtual void UpdateDiplomacyProperties()
		{
			this.Stats.Clear();
			this.Faction1Visual = new BannerImageIdentifierVM(this.Faction1.Banner, true);
			this.Faction2Visual = new BannerImageIdentifierVM(this.Faction2.Banner, true);
			StanceLink stanceWith = this._playerKingdom.GetStanceWith(this.Faction2);
			int dailyTributeToPay = stanceWith.GetDailyTributeToPay(this._playerKingdom);
			int remainingTributePaymentCount = stanceWith.GetRemainingTributePaymentCount();
			TextObject textObject = new TextObject("{=SDhQWonF}Paying {DENAR}{GOLD_ICON} as tribute per day, {TRIBUTE_PAYMENTS_REMAINING} days remaining.", null);
			textObject.SetTextVariable("DENAR", MathF.Abs(dailyTributeToPay));
			textObject.SetTextVariable("TRIBUTE_PAYMENTS_REMAINING", remainingTributePaymentCount);
			this.Faction1TributeText = ((dailyTributeToPay > 0) ? textObject.ToString() : string.Empty);
			this.Faction2TributeText = ((dailyTributeToPay < 0) ? textObject.ToString() : string.Empty);
			this.Faction1Name = this.Faction1.Name.ToString();
			this.Faction2Name = this.Faction2.Name.ToString();
			TextObject textObject2 = new TextObject("{=OyyJSyIX}{FACTION_1} is paying {DENAR}{GOLD_ICON} as tribute to {FACTION_2}, {TRIBUTE_PAYMENTS_REMAINING} days remaining.", null);
			TextObject textObject3 = textObject2.CopyTextObject();
			this.Faction1TributeHint = ((dailyTributeToPay > 0) ? new HintViewModel(textObject2.SetTextVariable("DENAR", MathF.Abs(dailyTributeToPay)).SetTextVariable("TRIBUTE_PAYMENTS_REMAINING", remainingTributePaymentCount).SetTextVariable("FACTION_1", this.Faction1Name)
				.SetTextVariable("FACTION_2", this.Faction2Name), null) : new HintViewModel());
			this.Faction2TributeHint = ((dailyTributeToPay < 0) ? new HintViewModel(textObject3.SetTextVariable("DENAR", MathF.Abs(dailyTributeToPay)).SetTextVariable("TRIBUTE_PAYMENTS_REMAINING", remainingTributePaymentCount).SetTextVariable("FACTION_1", this.Faction2Name)
				.SetTextVariable("FACTION_2", this.Faction1Name), null) : new HintViewModel());
			this.Faction1Leader = new HeroVM(this.Faction1.Leader, false);
			this.Faction2Leader = new HeroVM(this.Faction2.Leader, false);
			this.Faction1OwnedClans = new MBBindingList<KingdomDiplomacyFactionItemVM>();
			if (this.Faction1.IsKingdomFaction)
			{
				foreach (Clan clan in (this.Faction1 as Kingdom).Clans)
				{
					this.Faction1OwnedClans.Add(new KingdomDiplomacyFactionItemVM(clan));
				}
			}
			this.Faction2OwnedClans = new MBBindingList<KingdomDiplomacyFactionItemVM>();
			if (this.Faction2.IsKingdomFaction)
			{
				foreach (Clan clan2 in (this.Faction2 as Kingdom).Clans)
				{
					this.Faction2OwnedClans.Add(new KingdomDiplomacyFactionItemVM(clan2));
				}
			}
			this.Faction2OtherWars = new MBBindingList<KingdomDiplomacyFactionItemVM>();
			foreach (StanceLink stanceLink in FactionHelper.GetStances(this.Faction2))
			{
				if (stanceLink.IsAtWar && stanceLink.Faction1 != this.Faction1 && stanceLink.Faction2 != this.Faction1 && (stanceLink.Faction1.IsKingdomFaction || stanceLink.Faction1.Leader == Hero.MainHero) && (stanceLink.Faction2.IsKingdomFaction || stanceLink.Faction2.Leader == Hero.MainHero) && !stanceLink.Faction1.IsRebelClan && !stanceLink.Faction2.IsRebelClan && !stanceLink.Faction1.IsBanditFaction && !stanceLink.Faction2.IsBanditFaction)
				{
					this.Faction2OtherWars.Add(new KingdomDiplomacyFactionItemVM((stanceLink.Faction1 == this.Faction2) ? stanceLink.Faction2 : stanceLink.Faction1));
				}
			}
			this.IsFaction2OtherWarsVisible = this.Faction2OtherWars.Count > 0;
			this.Faction2OtherTradeAgreements = new MBBindingList<KingdomDiplomacyFactionItemVM>();
			foreach (IFaction faction in Campaign.Current.Factions)
			{
				if (faction != this.Faction1 && faction != this.Faction2 && faction.IsKingdomFaction)
				{
					ITradeAgreementsCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<ITradeAgreementsCampaignBehavior>();
					TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement;
					if (campaignBehavior != null && campaignBehavior.HasTradeAgreement(faction as Kingdom, this.Faction2 as Kingdom, out tradeAgreement))
					{
						this.Faction2OtherTradeAgreements.Add(new KingdomDiplomacyFactionItemVM(faction));
					}
				}
			}
			this.IsFaction2OtherTradeAgreementsVisible = this.Faction2OtherTradeAgreements.Count > 0;
			this.Faction2OtherAlliances = new MBBindingList<KingdomDiplomacyFactionItemVM>();
			foreach (IFaction faction2 in Campaign.Current.Factions)
			{
				if (faction2 != this.Faction1 && faction2 != this.Faction2 && DiplomacyHelper.HasAllianceWithFaction(faction2, this.Faction2))
				{
					this.Faction2OtherAlliances.Add(new KingdomDiplomacyFactionItemVM(faction2));
				}
			}
			this.IsFaction2OtherAlliancesVisible = this.Faction2OtherAlliances.Count > 0;
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x000277F4 File Offset: 0x000259F4
		private void PopulateSettlements()
		{
			this._faction1Towns = new List<Settlement>();
			this._faction1Castles = new List<Settlement>();
			this._faction2Towns = new List<Settlement>();
			this._faction2Castles = new List<Settlement>();
			foreach (Settlement settlement in this.Faction1.Settlements)
			{
				if (settlement.IsTown)
				{
					this._faction1Towns.Add(settlement);
				}
				else if (settlement.IsCastle)
				{
					this._faction1Castles.Add(settlement);
				}
			}
			foreach (Settlement settlement2 in this.Faction2.Settlements)
			{
				if (settlement2.IsTown)
				{
					this._faction2Towns.Add(settlement2);
				}
				else if (settlement2.IsCastle)
				{
					this._faction2Castles.Add(settlement2);
				}
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x00027908 File Offset: 0x00025B08
		// (set) Token: 0x060008BB RID: 2235 RVA: 0x00027910 File Offset: 0x00025B10
		[DataSourceProperty]
		public MBBindingList<KingdomDiplomacyFactionItemVM> Faction1OwnedClans
		{
			get
			{
				return this._faction1OwnedClans;
			}
			set
			{
				if (value != this._faction1OwnedClans)
				{
					this._faction1OwnedClans = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomDiplomacyFactionItemVM>>(value, "Faction1OwnedClans");
				}
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x0002792E File Offset: 0x00025B2E
		// (set) Token: 0x060008BD RID: 2237 RVA: 0x00027936 File Offset: 0x00025B36
		[DataSourceProperty]
		public MBBindingList<KingdomDiplomacyFactionItemVM> Faction2OwnedClans
		{
			get
			{
				return this._faction2OwnedClans;
			}
			set
			{
				if (value != this._faction2OwnedClans)
				{
					this._faction2OwnedClans = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomDiplomacyFactionItemVM>>(value, "Faction2OwnedClans");
				}
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x00027954 File Offset: 0x00025B54
		// (set) Token: 0x060008BF RID: 2239 RVA: 0x0002795C File Offset: 0x00025B5C
		[DataSourceProperty]
		public MBBindingList<KingdomDiplomacyFactionItemVM> Faction2OtherWars
		{
			get
			{
				return this._faction2OtherWars;
			}
			set
			{
				if (value != this._faction2OtherWars)
				{
					this._faction2OtherWars = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomDiplomacyFactionItemVM>>(value, "Faction2OtherWars");
				}
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x0002797A File Offset: 0x00025B7A
		// (set) Token: 0x060008C1 RID: 2241 RVA: 0x00027982 File Offset: 0x00025B82
		[DataSourceProperty]
		public MBBindingList<KingdomDiplomacyFactionItemVM> Faction2OtherTradeAgreements
		{
			get
			{
				return this._faction2OtherTradeAgreements;
			}
			set
			{
				if (value != this._faction2OtherTradeAgreements)
				{
					this._faction2OtherTradeAgreements = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomDiplomacyFactionItemVM>>(value, "Faction2OtherTradeAgreements");
				}
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x000279A0 File Offset: 0x00025BA0
		// (set) Token: 0x060008C3 RID: 2243 RVA: 0x000279A8 File Offset: 0x00025BA8
		[DataSourceProperty]
		public MBBindingList<KingdomDiplomacyFactionItemVM> Faction2OtherAlliances
		{
			get
			{
				return this._faction2OtherAlliances;
			}
			set
			{
				if (value != this._faction2OtherAlliances)
				{
					this._faction2OtherAlliances = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomDiplomacyFactionItemVM>>(value, "Faction2OtherAlliances");
				}
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x000279C6 File Offset: 0x00025BC6
		// (set) Token: 0x060008C5 RID: 2245 RVA: 0x000279CE File Offset: 0x00025BCE
		[DataSourceProperty]
		public MBBindingList<KingdomWarComparableStatVM> Stats
		{
			get
			{
				return this._stats;
			}
			set
			{
				if (value != this._stats)
				{
					this._stats = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomWarComparableStatVM>>(value, "Stats");
				}
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x000279EC File Offset: 0x00025BEC
		// (set) Token: 0x060008C7 RID: 2247 RVA: 0x000279F4 File Offset: 0x00025BF4
		[DataSourceProperty]
		public BannerImageIdentifierVM Faction1Visual
		{
			get
			{
				return this._faction1Visual;
			}
			set
			{
				if (value != this._faction1Visual)
				{
					this._faction1Visual = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Faction1Visual");
				}
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060008C8 RID: 2248 RVA: 0x00027A12 File Offset: 0x00025C12
		// (set) Token: 0x060008C9 RID: 2249 RVA: 0x00027A1A File Offset: 0x00025C1A
		[DataSourceProperty]
		public BannerImageIdentifierVM Faction2Visual
		{
			get
			{
				return this._faction2Visual;
			}
			set
			{
				if (value != this._faction2Visual)
				{
					this._faction2Visual = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Faction2Visual");
				}
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x00027A38 File Offset: 0x00025C38
		// (set) Token: 0x060008CB RID: 2251 RVA: 0x00027A40 File Offset: 0x00025C40
		[DataSourceProperty]
		public string Faction1Name
		{
			get
			{
				return this._faction1Name;
			}
			set
			{
				if (value != this._faction1Name)
				{
					this._faction1Name = value;
					base.OnPropertyChangedWithValue<string>(value, "Faction1Name");
				}
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x00027A63 File Offset: 0x00025C63
		// (set) Token: 0x060008CD RID: 2253 RVA: 0x00027A6B File Offset: 0x00025C6B
		[DataSourceProperty]
		public string Faction2Name
		{
			get
			{
				return this._faction2Name;
			}
			set
			{
				if (value != this._faction2Name)
				{
					this._faction2Name = value;
					base.OnPropertyChangedWithValue<string>(value, "Faction2Name");
				}
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x00027A8E File Offset: 0x00025C8E
		// (set) Token: 0x060008CF RID: 2255 RVA: 0x00027A96 File Offset: 0x00025C96
		[DataSourceProperty]
		public string Faction1TributeText
		{
			get
			{
				return this._faction1TributeText;
			}
			set
			{
				if (value != this._faction1TributeText)
				{
					this._faction1TributeText = value;
					base.OnPropertyChangedWithValue<string>(value, "Faction1TributeText");
				}
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x00027AB9 File Offset: 0x00025CB9
		// (set) Token: 0x060008D1 RID: 2257 RVA: 0x00027AC1 File Offset: 0x00025CC1
		[DataSourceProperty]
		public string Faction2TributeText
		{
			get
			{
				return this._faction2TributeText;
			}
			set
			{
				if (value != this._faction2TributeText)
				{
					this._faction2TributeText = value;
					base.OnPropertyChangedWithValue<string>(value, "Faction2TributeText");
				}
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x00027AE4 File Offset: 0x00025CE4
		// (set) Token: 0x060008D3 RID: 2259 RVA: 0x00027AEC File Offset: 0x00025CEC
		[DataSourceProperty]
		public HintViewModel Faction1TributeHint
		{
			get
			{
				return this._faction1TributeHint;
			}
			set
			{
				if (value != this._faction1TributeHint)
				{
					this._faction1TributeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Faction1TributeHint");
				}
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060008D4 RID: 2260 RVA: 0x00027B0A File Offset: 0x00025D0A
		// (set) Token: 0x060008D5 RID: 2261 RVA: 0x00027B12 File Offset: 0x00025D12
		[DataSourceProperty]
		public HintViewModel Faction2TributeHint
		{
			get
			{
				return this._faction2TributeHint;
			}
			set
			{
				if (value != this._faction2TributeHint)
				{
					this._faction2TributeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Faction2TributeHint");
				}
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060008D6 RID: 2262 RVA: 0x00027B30 File Offset: 0x00025D30
		// (set) Token: 0x060008D7 RID: 2263 RVA: 0x00027B38 File Offset: 0x00025D38
		[DataSourceProperty]
		public bool IsFaction2OtherWarsVisible
		{
			get
			{
				return this._isFaction2OtherWarsVisible;
			}
			set
			{
				if (value != this._isFaction2OtherWarsVisible)
				{
					this._isFaction2OtherWarsVisible = value;
					base.OnPropertyChangedWithValue(value, "IsFaction2OtherWarsVisible");
				}
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x00027B56 File Offset: 0x00025D56
		// (set) Token: 0x060008D9 RID: 2265 RVA: 0x00027B5E File Offset: 0x00025D5E
		[DataSourceProperty]
		public bool IsFaction2OtherTradeAgreementsVisible
		{
			get
			{
				return this._isFaction2OtherTradeAgreementsVisible;
			}
			set
			{
				if (value != this._isFaction2OtherTradeAgreementsVisible)
				{
					this._isFaction2OtherTradeAgreementsVisible = value;
					base.OnPropertyChangedWithValue(value, "IsFaction2OtherTradeAgreementsVisible");
				}
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x00027B7C File Offset: 0x00025D7C
		// (set) Token: 0x060008DB RID: 2267 RVA: 0x00027B84 File Offset: 0x00025D84
		[DataSourceProperty]
		public bool IsFaction2OtherAlliancesVisible
		{
			get
			{
				return this._isFaction2OtherAlliancesVisible;
			}
			set
			{
				if (value != this._isFaction2OtherAlliancesVisible)
				{
					this._isFaction2OtherAlliancesVisible = value;
					base.OnPropertyChangedWithValue(value, "IsFaction2OtherAlliancesVisible");
				}
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x00027BA2 File Offset: 0x00025DA2
		// (set) Token: 0x060008DD RID: 2269 RVA: 0x00027BAA File Offset: 0x00025DAA
		[DataSourceProperty]
		public HeroVM Faction1Leader
		{
			get
			{
				return this._faction1Leader;
			}
			set
			{
				if (value != this._faction1Leader)
				{
					this._faction1Leader = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "Faction1Leader");
				}
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x00027BC8 File Offset: 0x00025DC8
		// (set) Token: 0x060008DF RID: 2271 RVA: 0x00027BD0 File Offset: 0x00025DD0
		[DataSourceProperty]
		public HeroVM Faction2Leader
		{
			get
			{
				return this._faction2Leader;
			}
			set
			{
				if (value != this._faction2Leader)
				{
					this._faction2Leader = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "Faction2Leader");
				}
			}
		}

		// Token: 0x040003CE RID: 974
		public readonly IFaction Faction1;

		// Token: 0x040003CF RID: 975
		public readonly IFaction Faction2;

		// Token: 0x040003D0 RID: 976
		protected readonly string _faction1Color;

		// Token: 0x040003D1 RID: 977
		protected readonly string _faction2Color;

		// Token: 0x040003D2 RID: 978
		protected readonly IFaction _playerKingdom;

		// Token: 0x040003D3 RID: 979
		protected List<Settlement> _faction1Towns;

		// Token: 0x040003D4 RID: 980
		protected List<Settlement> _faction2Towns;

		// Token: 0x040003D5 RID: 981
		protected List<Settlement> _faction1Castles;

		// Token: 0x040003D6 RID: 982
		protected List<Settlement> _faction2Castles;

		// Token: 0x040003D7 RID: 983
		private MBBindingList<KingdomWarComparableStatVM> _stats;

		// Token: 0x040003D8 RID: 984
		private BannerImageIdentifierVM _faction1Visual;

		// Token: 0x040003D9 RID: 985
		private BannerImageIdentifierVM _faction2Visual;

		// Token: 0x040003DA RID: 986
		private HeroVM _faction1Leader;

		// Token: 0x040003DB RID: 987
		private HeroVM _faction2Leader;

		// Token: 0x040003DC RID: 988
		private string _faction1Name;

		// Token: 0x040003DD RID: 989
		private string _faction2Name;

		// Token: 0x040003DE RID: 990
		private string _faction1TributeText;

		// Token: 0x040003DF RID: 991
		private string _faction2TributeText;

		// Token: 0x040003E0 RID: 992
		private HintViewModel _faction1TributeHint;

		// Token: 0x040003E1 RID: 993
		private HintViewModel _faction2TributeHint;

		// Token: 0x040003E2 RID: 994
		private bool _isFaction2OtherWarsVisible;

		// Token: 0x040003E3 RID: 995
		private bool _isFaction2OtherTradeAgreementsVisible;

		// Token: 0x040003E4 RID: 996
		private bool _isFaction2OtherAlliancesVisible;

		// Token: 0x040003E5 RID: 997
		private MBBindingList<KingdomDiplomacyFactionItemVM> _faction1OwnedClans;

		// Token: 0x040003E6 RID: 998
		private MBBindingList<KingdomDiplomacyFactionItemVM> _faction2OwnedClans;

		// Token: 0x040003E7 RID: 999
		private MBBindingList<KingdomDiplomacyFactionItemVM> _faction2OtherWars;

		// Token: 0x040003E8 RID: 1000
		private MBBindingList<KingdomDiplomacyFactionItemVM> _faction2OtherTradeAgreements;

		// Token: 0x040003E9 RID: 1001
		private MBBindingList<KingdomDiplomacyFactionItemVM> _faction2OtherAlliances;
	}
}
