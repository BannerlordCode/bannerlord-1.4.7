using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000CE RID: 206
	[EncyclopediaViewModel(typeof(Clan))]
	public class EncyclopediaClanPageVM : EncyclopediaContentPageVM
	{
		// Token: 0x0600138A RID: 5002 RVA: 0x0004EB4C File Offset: 0x0004CD4C
		public EncyclopediaClanPageVM(EncyclopediaPageArgs args)
			: base(args)
		{
			this._faction = base.Obj as IFaction;
			this._clan = this._faction as Clan;
			this.Members = new MBBindingList<HeroVM>();
			this.Enemies = new MBBindingList<EncyclopediaFactionVM>();
			this.Settlements = new MBBindingList<EncyclopediaSettlementVM>();
			this.History = new MBBindingList<EncyclopediaHistoryEventVM>();
			this.ClanInfo = new MBBindingList<StringPairItemVM>();
			base.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(this._clan);
			this.RefreshValues();
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x0004EBE0 File Offset: 0x0004CDE0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.StrengthHint = new HintViewModel(GameTexts.FindText("str_strength", null), null);
			this.ProsperityHint = new HintViewModel(GameTexts.FindText("str_prosperity", null), null);
			this.MembersText = GameTexts.FindText("str_members", null).ToString();
			this.AlliesText = new TextObject("{=bfQLwMUp}Clans", null).ToString();
			this.EnemiesText = new TextObject("{=zZlWRZjO}Wars", null).ToString();
			this.SettlementsText = GameTexts.FindText("str_settlements", null).ToString();
			this.VillagesText = GameTexts.FindText("str_villages", null).ToString();
			this.DestroyedText = new TextObject("{=w8Yzf0F0}Destroyed", null).ToString();
			this.PartOfText = GameTexts.FindText("str_encyclopedia_clan_part_of_kingdom", null).ToString();
			this.LeaderText = GameTexts.FindText("str_leader", null).ToString();
			this.InfoText = GameTexts.FindText("str_info", null).ToString();
			base.UpdateBookmarkHintText();
			this.Refresh();
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x0004ECF4 File Offset: 0x0004CEF4
		public override void Refresh()
		{
			this.Members.Clear();
			this.Enemies.Clear();
			this.Settlements.Clear();
			this.History.Clear();
			this.ClanInfo.Clear();
			TextObject encyclopediaText = this._faction.EncyclopediaText;
			this.InformationText = ((encyclopediaText != null) ? encyclopediaText.ToString() : null);
			this.Leader = new HeroVM(this._faction.Leader, true);
			this.NameText = this._clan.Name.ToString();
			this.HasParentKingdom = this._clan.Kingdom != null;
			this.ParentKingdom = (this.HasParentKingdom ? new EncyclopediaFactionVM(((Clan)this._faction).Kingdom) : null);
			if (this._faction.IsKingdomFaction)
			{
				this.DescriptorText = GameTexts.FindText("str_kingdom_faction", null).ToString();
			}
			else if (this._faction.IsBanditFaction)
			{
				this.DescriptorText = GameTexts.FindText("str_bandit_faction", null).ToString();
			}
			else if (this._faction.IsMinorFaction)
			{
				this.DescriptorText = GameTexts.FindText("str_minor_faction", null).ToString();
			}
			int num = 0;
			float num2 = 0f;
			EncyclopediaPage pageOf = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Hero));
			IEnumerable<Hero> heroes = this._faction.Heroes;
			Clan clan = this._clan;
			foreach (Hero hero in heroes.Union<Hero>((clan != null) ? clan.Companions : null))
			{
				if (pageOf.IsValidEncyclopediaItem(hero))
				{
					if (hero != this.Leader.Hero)
					{
						this.Members.Add(new HeroVM(hero, true));
					}
					num += hero.Gold;
				}
			}
			this.Members.Sort(new HeroAgeComparer(false));
			this.Banner = new BannerImageIdentifierVM(this._faction.Banner, true);
			foreach (MobileParty mobileParty in MobileParty.AllLordParties)
			{
				if (mobileParty.MapFaction == this._faction && !mobileParty.IsDisbanding)
				{
					num2 += mobileParty.Party.CalculateCurrentStrength();
				}
			}
			this.ProsperityText = num.ToString();
			this.StrengthText = num2.ToString();
			MBObjectBase mbobjectBase = this._faction as MBObjectBase;
			for (int i = Campaign.Current.LogEntryHistory.GameActionLogs.Count - 1; i >= 0; i--)
			{
				IEncyclopediaLog encyclopediaLog;
				if ((encyclopediaLog = Campaign.Current.LogEntryHistory.GameActionLogs[i] as IEncyclopediaLog) != null && encyclopediaLog.IsVisibleInEncyclopediaPageOf(mbobjectBase))
				{
					this.History.Add(new EncyclopediaHistoryEventVM(encyclopediaLog));
				}
			}
			EncyclopediaPage pageOf2 = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Clan));
			foreach (IFaction faction in Campaign.Current.Factions.OrderBy<IFaction, bool>((IFaction x) => !x.IsKingdomFaction).ThenBy<IFaction, string>((IFaction f) => f.Name.ToString()))
			{
				IFaction mapFaction = faction.MapFaction;
				if (pageOf2.IsValidEncyclopediaItem(mapFaction) && mapFaction != this._faction.MapFaction && mapFaction != this._faction && !mapFaction.IsBanditFaction && FactionManager.IsAtWarAgainstFaction(this._faction.MapFaction, mapFaction) && !this.Enemies.Any<EncyclopediaFactionVM>((EncyclopediaFactionVM x) => x.Faction == mapFaction))
				{
					this.Enemies.Add(new EncyclopediaFactionVM(mapFaction));
				}
			}
			EncyclopediaPage pageOf3 = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Settlement));
			foreach (Settlement settlement in from s in Settlement.All
				orderby s.IsVillage, s.IsCastle, s.IsTown
				select s)
			{
				if ((settlement.MapFaction == this._faction || (settlement.OwnerClan == this._faction && settlement.OwnerClan.Leader != null)) && pageOf3.IsValidEncyclopediaItem(settlement) && (settlement.IsTown || settlement.IsCastle))
				{
					this.Settlements.Add(new EncyclopediaSettlementVM(settlement));
				}
			}
			GameTexts.SetVariable("LEFT", new TextObject("{=tTLvo8sM}Clan Tier", null).ToString());
			this.ClanInfo.Add(new StringPairItemVM(GameTexts.FindText("str_LEFT_colon", null).ToString(), this._clan.Tier.ToString(), null));
			GameTexts.SetVariable("LEFT", new TextObject("{=ODEnkg0o}Clan Strength", null).ToString());
			this.ClanInfo.Add(new StringPairItemVM(GameTexts.FindText("str_LEFT_colon", null).ToString(), this._clan.CurrentTotalStrength.ToString("F0"), null));
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_wealth", null).ToString());
			this.ClanInfo.Add(new StringPairItemVM(GameTexts.FindText("str_LEFT_colon", null).ToString(), CampaignUIHelper.GetClanWealthStatusText(this._clan), null));
			this.IsClanDestroyed = this._clan.IsEliminated;
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x0004F34C File Offset: 0x0004D54C
		public override string GetName()
		{
			return this._clan.Name.ToString();
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x0004F360 File Offset: 0x0004D560
		public override string GetNavigationBarURL()
		{
			return HyperlinkTexts.GetGenericHyperlinkText("Home", GameTexts.FindText("str_encyclopedia_home", null).ToString()) + " \\ " + HyperlinkTexts.GetGenericHyperlinkText("ListPage-Clans", GameTexts.FindText("str_encyclopedia_clans", null).ToString()) + " \\ " + this.GetName();
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x0004F3C8 File Offset: 0x0004D5C8
		public override void ExecuteSwitchBookmarkedState()
		{
			base.ExecuteSwitchBookmarkedState();
			if (base.IsBookmarked)
			{
				Campaign.Current.EncyclopediaManager.ViewDataTracker.AddEncyclopediaBookmarkToItem(this._clan);
				return;
			}
			Campaign.Current.EncyclopediaManager.ViewDataTracker.RemoveEncyclopediaBookmarkFromItem(this._clan);
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06001390 RID: 5008 RVA: 0x0004F418 File Offset: 0x0004D618
		// (set) Token: 0x06001391 RID: 5009 RVA: 0x0004F420 File Offset: 0x0004D620
		[DataSourceProperty]
		public MBBindingList<StringPairItemVM> ClanInfo
		{
			get
			{
				return this._clanInfo;
			}
			set
			{
				if (value != this._clanInfo)
				{
					this._clanInfo = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemVM>>(value, "ClanInfo");
				}
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06001392 RID: 5010 RVA: 0x0004F43E File Offset: 0x0004D63E
		// (set) Token: 0x06001393 RID: 5011 RVA: 0x0004F446 File Offset: 0x0004D646
		[DataSourceProperty]
		public MBBindingList<HeroVM> Members
		{
			get
			{
				return this._members;
			}
			set
			{
				if (value != this._members)
				{
					this._members = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "Members");
				}
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06001394 RID: 5012 RVA: 0x0004F464 File Offset: 0x0004D664
		// (set) Token: 0x06001395 RID: 5013 RVA: 0x0004F46C File Offset: 0x0004D66C
		[DataSourceProperty]
		public MBBindingList<EncyclopediaFactionVM> Enemies
		{
			get
			{
				return this._enemies;
			}
			set
			{
				if (value != this._enemies)
				{
					this._enemies = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaFactionVM>>(value, "Enemies");
				}
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06001396 RID: 5014 RVA: 0x0004F48A File Offset: 0x0004D68A
		// (set) Token: 0x06001397 RID: 5015 RVA: 0x0004F492 File Offset: 0x0004D692
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSettlementVM> Settlements
		{
			get
			{
				return this._settlements;
			}
			set
			{
				if (value != this._settlements)
				{
					this._settlements = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaSettlementVM>>(value, "Settlements");
				}
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06001398 RID: 5016 RVA: 0x0004F4B0 File Offset: 0x0004D6B0
		// (set) Token: 0x06001399 RID: 5017 RVA: 0x0004F4B8 File Offset: 0x0004D6B8
		[DataSourceProperty]
		public MBBindingList<EncyclopediaHistoryEventVM> History
		{
			get
			{
				return this._history;
			}
			set
			{
				if (value != this._history)
				{
					this._history = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaHistoryEventVM>>(value, "History");
				}
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x0600139A RID: 5018 RVA: 0x0004F4D6 File Offset: 0x0004D6D6
		// (set) Token: 0x0600139B RID: 5019 RVA: 0x0004F4DE File Offset: 0x0004D6DE
		[DataSourceProperty]
		public EncyclopediaFactionVM ParentKingdom
		{
			get
			{
				return this._parentKingdom;
			}
			set
			{
				if (value != this._parentKingdom)
				{
					this._parentKingdom = value;
					base.OnPropertyChangedWithValue<EncyclopediaFactionVM>(value, "ParentKingdom");
				}
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x0600139C RID: 5020 RVA: 0x0004F4FC File Offset: 0x0004D6FC
		// (set) Token: 0x0600139D RID: 5021 RVA: 0x0004F504 File Offset: 0x0004D704
		[DataSourceProperty]
		public bool HasParentKingdom
		{
			get
			{
				return this._hasParentKingdom;
			}
			set
			{
				if (value != this._hasParentKingdom)
				{
					this._hasParentKingdom = value;
					base.OnPropertyChangedWithValue(value, "HasParentKingdom");
				}
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x0600139E RID: 5022 RVA: 0x0004F522 File Offset: 0x0004D722
		// (set) Token: 0x0600139F RID: 5023 RVA: 0x0004F52A File Offset: 0x0004D72A
		[DataSourceProperty]
		public bool IsClanDestroyed
		{
			get
			{
				return this._isClanDestroyed;
			}
			set
			{
				if (value != this._isClanDestroyed)
				{
					this._isClanDestroyed = value;
					base.OnPropertyChangedWithValue(value, "IsClanDestroyed");
				}
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x060013A0 RID: 5024 RVA: 0x0004F548 File Offset: 0x0004D748
		// (set) Token: 0x060013A1 RID: 5025 RVA: 0x0004F550 File Offset: 0x0004D750
		[DataSourceProperty]
		public string DestroyedText
		{
			get
			{
				return this._destroyedText;
			}
			set
			{
				if (value != this._destroyedText)
				{
					this._destroyedText = value;
					base.OnPropertyChangedWithValue<string>(value, "DestroyedText");
				}
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x060013A2 RID: 5026 RVA: 0x0004F573 File Offset: 0x0004D773
		// (set) Token: 0x060013A3 RID: 5027 RVA: 0x0004F57B File Offset: 0x0004D77B
		[DataSourceProperty]
		public string PartOfText
		{
			get
			{
				return this._partOfText;
			}
			set
			{
				if (value != this._partOfText)
				{
					this._partOfText = value;
					base.OnPropertyChangedWithValue<string>(value, "PartOfText");
				}
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x060013A4 RID: 5028 RVA: 0x0004F59E File Offset: 0x0004D79E
		// (set) Token: 0x060013A5 RID: 5029 RVA: 0x0004F5A6 File Offset: 0x0004D7A6
		[DataSourceProperty]
		public string TierText
		{
			get
			{
				return this._tierText;
			}
			set
			{
				if (value != this._tierText)
				{
					this._tierText = value;
					base.OnPropertyChangedWithValue<string>(value, "TierText");
				}
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x060013A6 RID: 5030 RVA: 0x0004F5C9 File Offset: 0x0004D7C9
		// (set) Token: 0x060013A7 RID: 5031 RVA: 0x0004F5D1 File Offset: 0x0004D7D1
		[DataSourceProperty]
		public string InfoText
		{
			get
			{
				return this._infoText;
			}
			set
			{
				if (value != this._infoText)
				{
					this._infoText = value;
					base.OnPropertyChangedWithValue<string>(value, "InfoText");
				}
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x060013A8 RID: 5032 RVA: 0x0004F5F4 File Offset: 0x0004D7F4
		// (set) Token: 0x060013A9 RID: 5033 RVA: 0x0004F5FC File Offset: 0x0004D7FC
		[DataSourceProperty]
		public HeroVM Leader
		{
			get
			{
				return this._leader;
			}
			set
			{
				if (value != this._leader)
				{
					this._leader = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "Leader");
				}
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x060013AA RID: 5034 RVA: 0x0004F61A File Offset: 0x0004D81A
		// (set) Token: 0x060013AB RID: 5035 RVA: 0x0004F622 File Offset: 0x0004D822
		[DataSourceProperty]
		public BannerImageIdentifierVM Banner
		{
			get
			{
				return this._banner;
			}
			set
			{
				if (value != this._banner)
				{
					this._banner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Banner");
				}
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060013AC RID: 5036 RVA: 0x0004F640 File Offset: 0x0004D840
		// (set) Token: 0x060013AD RID: 5037 RVA: 0x0004F648 File Offset: 0x0004D848
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

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060013AE RID: 5038 RVA: 0x0004F66B File Offset: 0x0004D86B
		// (set) Token: 0x060013AF RID: 5039 RVA: 0x0004F673 File Offset: 0x0004D873
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

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x060013B0 RID: 5040 RVA: 0x0004F696 File Offset: 0x0004D896
		// (set) Token: 0x060013B1 RID: 5041 RVA: 0x0004F69E File Offset: 0x0004D89E
		[DataSourceProperty]
		public string EnemiesText
		{
			get
			{
				return this._enemiesText;
			}
			set
			{
				if (value != this._enemiesText)
				{
					this._enemiesText = value;
					base.OnPropertyChangedWithValue<string>(value, "EnemiesText");
				}
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x060013B2 RID: 5042 RVA: 0x0004F6C1 File Offset: 0x0004D8C1
		// (set) Token: 0x060013B3 RID: 5043 RVA: 0x0004F6C9 File Offset: 0x0004D8C9
		[DataSourceProperty]
		public string AlliesText
		{
			get
			{
				return this._alliesText;
			}
			set
			{
				if (value != this._alliesText)
				{
					this._alliesText = value;
					base.OnPropertyChangedWithValue<string>(value, "AlliesText");
				}
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060013B4 RID: 5044 RVA: 0x0004F6EC File Offset: 0x0004D8EC
		// (set) Token: 0x060013B5 RID: 5045 RVA: 0x0004F6F4 File Offset: 0x0004D8F4
		[DataSourceProperty]
		public string SettlementsText
		{
			get
			{
				return this._settlementsText;
			}
			set
			{
				if (value != this._settlementsText)
				{
					this._settlementsText = value;
					base.OnPropertyChangedWithValue<string>(value, "SettlementsText");
				}
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x060013B6 RID: 5046 RVA: 0x0004F717 File Offset: 0x0004D917
		// (set) Token: 0x060013B7 RID: 5047 RVA: 0x0004F71F File Offset: 0x0004D91F
		[DataSourceProperty]
		public string VillagesText
		{
			get
			{
				return this._villagesText;
			}
			set
			{
				if (value != this._villagesText)
				{
					this._villagesText = value;
					base.OnPropertyChangedWithValue<string>(value, "VillagesText");
				}
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060013B8 RID: 5048 RVA: 0x0004F742 File Offset: 0x0004D942
		// (set) Token: 0x060013B9 RID: 5049 RVA: 0x0004F74A File Offset: 0x0004D94A
		[DataSourceProperty]
		public string InformationText
		{
			get
			{
				return this._informationText;
			}
			set
			{
				if (value != this._informationText)
				{
					this._informationText = value;
					base.OnPropertyChangedWithValue<string>(value, "InformationText");
				}
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060013BA RID: 5050 RVA: 0x0004F76D File Offset: 0x0004D96D
		// (set) Token: 0x060013BB RID: 5051 RVA: 0x0004F775 File Offset: 0x0004D975
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

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x060013BC RID: 5052 RVA: 0x0004F798 File Offset: 0x0004D998
		// (set) Token: 0x060013BD RID: 5053 RVA: 0x0004F7A0 File Offset: 0x0004D9A0
		[DataSourceProperty]
		public string DescriptorText
		{
			get
			{
				return this._descriptorText;
			}
			set
			{
				if (value != this._descriptorText)
				{
					this._descriptorText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptorText");
				}
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060013BE RID: 5054 RVA: 0x0004F7C3 File Offset: 0x0004D9C3
		// (set) Token: 0x060013BF RID: 5055 RVA: 0x0004F7CB File Offset: 0x0004D9CB
		[DataSourceProperty]
		public string ProsperityText
		{
			get
			{
				return this._prosperityText;
			}
			set
			{
				if (value != this._prosperityText)
				{
					this._prosperityText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProsperityText");
				}
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060013C0 RID: 5056 RVA: 0x0004F7EE File Offset: 0x0004D9EE
		// (set) Token: 0x060013C1 RID: 5057 RVA: 0x0004F7F6 File Offset: 0x0004D9F6
		[DataSourceProperty]
		public string StrengthText
		{
			get
			{
				return this._strengthText;
			}
			set
			{
				if (value != this._strengthText)
				{
					this._strengthText = value;
					base.OnPropertyChangedWithValue<string>(value, "StrengthText");
				}
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x060013C2 RID: 5058 RVA: 0x0004F819 File Offset: 0x0004DA19
		// (set) Token: 0x060013C3 RID: 5059 RVA: 0x0004F821 File Offset: 0x0004DA21
		[DataSourceProperty]
		public HintViewModel ProsperityHint
		{
			get
			{
				return this._prosperityHint;
			}
			set
			{
				if (value != this._prosperityHint)
				{
					this._prosperityHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ProsperityHint");
				}
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060013C4 RID: 5060 RVA: 0x0004F83F File Offset: 0x0004DA3F
		// (set) Token: 0x060013C5 RID: 5061 RVA: 0x0004F847 File Offset: 0x0004DA47
		[DataSourceProperty]
		public HintViewModel StrengthHint
		{
			get
			{
				return this._strengthHint;
			}
			set
			{
				if (value != this._strengthHint)
				{
					this._strengthHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "StrengthHint");
				}
			}
		}

		// Token: 0x040008F0 RID: 2288
		private readonly IFaction _faction;

		// Token: 0x040008F1 RID: 2289
		private readonly Clan _clan;

		// Token: 0x040008F2 RID: 2290
		private MBBindingList<StringPairItemVM> _clanInfo;

		// Token: 0x040008F3 RID: 2291
		private MBBindingList<HeroVM> _members;

		// Token: 0x040008F4 RID: 2292
		private MBBindingList<EncyclopediaFactionVM> _enemies;

		// Token: 0x040008F5 RID: 2293
		private MBBindingList<EncyclopediaSettlementVM> _settlements;

		// Token: 0x040008F6 RID: 2294
		private MBBindingList<EncyclopediaHistoryEventVM> _history;

		// Token: 0x040008F7 RID: 2295
		private HeroVM _leader;

		// Token: 0x040008F8 RID: 2296
		private BannerImageIdentifierVM _banner;

		// Token: 0x040008F9 RID: 2297
		private string _membersText;

		// Token: 0x040008FA RID: 2298
		private string _enemiesText;

		// Token: 0x040008FB RID: 2299
		private string _alliesText;

		// Token: 0x040008FC RID: 2300
		private string _settlementsText;

		// Token: 0x040008FD RID: 2301
		private string _villagesText;

		// Token: 0x040008FE RID: 2302
		private string _leaderText;

		// Token: 0x040008FF RID: 2303
		private string _descriptorText;

		// Token: 0x04000900 RID: 2304
		private string _informationText;

		// Token: 0x04000901 RID: 2305
		private string _prosperityText;

		// Token: 0x04000902 RID: 2306
		private string _strengthText;

		// Token: 0x04000903 RID: 2307
		private string _destroyedText;

		// Token: 0x04000904 RID: 2308
		private string _partOfText;

		// Token: 0x04000905 RID: 2309
		private string _tierText;

		// Token: 0x04000906 RID: 2310
		private string _infoText;

		// Token: 0x04000907 RID: 2311
		private HintViewModel _prosperityHint;

		// Token: 0x04000908 RID: 2312
		private HintViewModel _strengthHint;

		// Token: 0x04000909 RID: 2313
		private EncyclopediaFactionVM _parentKingdom;

		// Token: 0x0400090A RID: 2314
		private string _nameText;

		// Token: 0x0400090B RID: 2315
		private bool _hasParentKingdom;

		// Token: 0x0400090C RID: 2316
		private bool _isClanDestroyed;
	}
}
