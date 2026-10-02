using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanFinance;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Supporters;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories
{
	// Token: 0x0200013B RID: 315
	public class ClanIncomeVM : ViewModel
	{
		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x06001D79 RID: 7545 RVA: 0x0006CFBE File Offset: 0x0006B1BE
		// (set) Token: 0x06001D7A RID: 7546 RVA: 0x0006CFC6 File Offset: 0x0006B1C6
		public int TotalIncome { get; private set; }

		// Token: 0x06001D7B RID: 7547 RVA: 0x0006CFD0 File Offset: 0x0006B1D0
		public ClanIncomeVM(Action onRefresh, Action<ClanCardSelectionInfo> openCardSelectionPopup)
		{
			this._onRefresh = onRefresh;
			this._openCardSelectionPopup = openCardSelectionPopup;
			this.Incomes = new MBBindingList<ClanFinanceWorkshopItemVM>();
			this.SupporterGroups = new MBBindingList<ClanSupporterGroupVM>();
			this.Alleys = new MBBindingList<ClanFinanceAlleyItemVM>();
			this.SortController = new ClanIncomeSortControllerVM(this._incomes, this._supporterGroups, this._alleys);
			this.RefreshList();
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x0006D038 File Offset: 0x0006B238
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NameText = GameTexts.FindText("str_sort_by_name_label", null).ToString();
			this.IncomeText = GameTexts.FindText("str_income", null).ToString();
			this.LocationText = GameTexts.FindText("str_tooltip_label_location", null).ToString();
			this.NoAdditionalIncomesText = GameTexts.FindText("str_clan_no_additional_incomes", null).ToString();
			this.Incomes.ApplyActionOnAllItems(delegate(ClanFinanceWorkshopItemVM x)
			{
				x.RefreshValues();
			});
			ClanFinanceWorkshopItemVM currentSelectedIncome = this.CurrentSelectedIncome;
			if (currentSelectedIncome != null)
			{
				currentSelectedIncome.RefreshValues();
			}
			this.SortController.RefreshValues();
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x0006D0EC File Offset: 0x0006B2EC
		public void RefreshList()
		{
			this.Incomes.Clear();
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsTown)
				{
					foreach (Workshop workshop in settlement.Town.Workshops)
					{
						if (workshop.Owner == Hero.MainHero)
						{
							this.Incomes.Add(new ClanFinanceWorkshopItemVM(workshop, new Action<ClanFinanceWorkshopItemVM>(this.OnIncomeSelection), new Action(this.OnRefresh), this._openCardSelectionPopup));
						}
					}
				}
			}
			this.RefreshSupporters();
			this.RefreshAlleys();
			this.SortController.ResetAllStates();
			GameTexts.SetVariable("STR1", GameTexts.FindText("str_clan_workshops", null));
			GameTexts.SetVariable("LEFT", Hero.MainHero.OwnedWorkshops.Count);
			GameTexts.SetVariable("RIGHT", Campaign.Current.Models.WorkshopModel.GetMaxWorkshopCountForClanTier(Clan.PlayerClan.Tier));
			GameTexts.SetVariable("STR2", GameTexts.FindText("str_LEFT_over_RIGHT_in_paranthesis", null));
			this.WorkshopText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			int num = 0;
			foreach (ClanSupporterGroupVM clanSupporterGroupVM in this.SupporterGroups)
			{
				num += clanSupporterGroupVM.Supporters.Count;
			}
			GameTexts.SetVariable("RANK", new TextObject("{=RzFyGnWJ}Supporters", null).ToString());
			GameTexts.SetVariable("NUMBER", num);
			this.SupportersText = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null).ToString();
			GameTexts.SetVariable("RANK", new TextObject("{=7tKjfMSb}Alleys", null).ToString());
			GameTexts.SetVariable("NUMBER", this.Alleys.Count);
			this.AlleysText = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null).ToString();
			this.RefreshTotalIncome();
			this.OnIncomeSelection(this.GetDefaultIncome());
			this.RefreshValues();
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x0006D328 File Offset: 0x0006B528
		private void RefreshSupporters()
		{
			foreach (ClanSupporterGroupVM clanSupporterGroupVM in this.SupporterGroups)
			{
				clanSupporterGroupVM.Supporters.Clear();
			}
			this.SupporterGroups.Clear();
			Dictionary<float, List<Hero>> dictionary = new Dictionary<float, List<Hero>>();
			NotablePowerModel notablePowerModel = Campaign.Current.Models.NotablePowerModel;
			foreach (Hero hero in Clan.PlayerClan.SupporterNotables.OrderBy<Hero, float>((Hero x) => x.Power))
			{
				if (hero.CurrentSettlement != null)
				{
					float influenceBonusToClan = notablePowerModel.GetInfluenceBonusToClan(hero);
					List<Hero> list;
					if (dictionary.TryGetValue(influenceBonusToClan, out list))
					{
						list.Add(hero);
					}
					else
					{
						dictionary.Add(influenceBonusToClan, new List<Hero> { hero });
					}
				}
			}
			foreach (KeyValuePair<float, List<Hero>> keyValuePair in dictionary)
			{
				if (keyValuePair.Value.Count > 0)
				{
					ClanSupporterGroupVM clanSupporterGroupVM2 = new ClanSupporterGroupVM(notablePowerModel.GetPowerRankName(keyValuePair.Value.FirstOrDefault<Hero>()), keyValuePair.Key, new Action<ClanSupporterGroupVM>(this.OnSupporterSelection));
					foreach (Hero hero2 in keyValuePair.Value)
					{
						clanSupporterGroupVM2.AddSupporter(hero2);
					}
					this.SupporterGroups.Add(clanSupporterGroupVM2);
				}
			}
			foreach (ClanSupporterGroupVM clanSupporterGroupVM3 in this.SupporterGroups)
			{
				clanSupporterGroupVM3.Refresh();
			}
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x0006D53C File Offset: 0x0006B73C
		private void RefreshAlleys()
		{
			this.Alleys.Clear();
			foreach (Alley alley in Hero.MainHero.OwnedAlleys)
			{
				this.Alleys.Add(new ClanFinanceAlleyItemVM(alley, this._openCardSelectionPopup, new Action<ClanFinanceAlleyItemVM>(this.OnAlleySelection), new Action(this.OnRefresh)));
			}
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x0006D5C8 File Offset: 0x0006B7C8
		private ClanFinanceWorkshopItemVM GetDefaultIncome()
		{
			return this.Incomes.FirstOrDefault<ClanFinanceWorkshopItemVM>();
		}

		// Token: 0x06001D81 RID: 7553 RVA: 0x0006D5D8 File Offset: 0x0006B7D8
		public void SelectWorkshop(Workshop workshop)
		{
			foreach (ClanFinanceWorkshopItemVM clanFinanceWorkshopItemVM in this.Incomes)
			{
				if (clanFinanceWorkshopItemVM != null)
				{
					ClanFinanceWorkshopItemVM clanFinanceWorkshopItemVM2 = clanFinanceWorkshopItemVM;
					if (clanFinanceWorkshopItemVM2.Workshop == workshop)
					{
						this.OnIncomeSelection(clanFinanceWorkshopItemVM2);
						break;
					}
				}
			}
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x0006D638 File Offset: 0x0006B838
		public void SelectAlley(Alley alley)
		{
			for (int i = 0; i < this.Alleys.Count; i++)
			{
				if (this.Alleys[i].Alley == alley)
				{
					this.OnAlleySelection(this.Alleys[i]);
					return;
				}
			}
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x0006D684 File Offset: 0x0006B884
		private void OnAlleySelection(ClanFinanceAlleyItemVM alley)
		{
			if (alley == null)
			{
				if (this.CurrentSelectedAlley != null)
				{
					this.CurrentSelectedAlley.IsSelected = false;
				}
				this.CurrentSelectedAlley = null;
				return;
			}
			this.OnIncomeSelection(null);
			this.OnSupporterSelection(null);
			if (this.CurrentSelectedAlley != null)
			{
				this.CurrentSelectedAlley.IsSelected = false;
			}
			this.CurrentSelectedAlley = alley;
			if (alley != null)
			{
				alley.IsSelected = true;
			}
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x0006D6E4 File Offset: 0x0006B8E4
		private void OnIncomeSelection(ClanFinanceWorkshopItemVM income)
		{
			if (income == null)
			{
				if (this.CurrentSelectedIncome != null)
				{
					this.CurrentSelectedIncome.IsSelected = false;
				}
				this.CurrentSelectedIncome = null;
				return;
			}
			this.OnSupporterSelection(null);
			this.OnAlleySelection(null);
			if (this.CurrentSelectedIncome != null)
			{
				this.CurrentSelectedIncome.IsSelected = false;
			}
			this.CurrentSelectedIncome = income;
			if (income != null)
			{
				income.IsSelected = true;
			}
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x0006D744 File Offset: 0x0006B944
		private void OnSupporterSelection(ClanSupporterGroupVM supporter)
		{
			if (supporter == null)
			{
				if (this.CurrentSelectedSupporterGroup != null)
				{
					this.CurrentSelectedSupporterGroup.IsSelected = false;
				}
				this.CurrentSelectedSupporterGroup = null;
				return;
			}
			this.OnIncomeSelection(null);
			this.OnAlleySelection(null);
			if (this.CurrentSelectedSupporterGroup != null)
			{
				this.CurrentSelectedSupporterGroup.IsSelected = false;
			}
			this.CurrentSelectedSupporterGroup = supporter;
			if (this.CurrentSelectedSupporterGroup != null)
			{
				this.CurrentSelectedSupporterGroup.IsSelected = true;
			}
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x0006D7AD File Offset: 0x0006B9AD
		public void RefreshTotalIncome()
		{
			this.TotalIncome = this.Incomes.Sum<ClanFinanceWorkshopItemVM>((ClanFinanceWorkshopItemVM i) => i.Income);
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x0006D7DF File Offset: 0x0006B9DF
		public void OnRefresh()
		{
			Action onRefresh = this._onRefresh;
			if (onRefresh == null)
			{
				return;
			}
			onRefresh();
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x06001D88 RID: 7560 RVA: 0x0006D7F1 File Offset: 0x0006B9F1
		// (set) Token: 0x06001D89 RID: 7561 RVA: 0x0006D7F9 File Offset: 0x0006B9F9
		[DataSourceProperty]
		public ClanFinanceAlleyItemVM CurrentSelectedAlley
		{
			get
			{
				return this._currentSelectedAlley;
			}
			set
			{
				if (value != this._currentSelectedAlley)
				{
					this._currentSelectedAlley = value;
					base.OnPropertyChangedWithValue<ClanFinanceAlleyItemVM>(value, "CurrentSelectedAlley");
					this.IsAnyValidAlleySelected = value != null;
					this.IsAnyValidIncomeSelected = false;
					this.IsAnyValidSupporterSelected = false;
				}
			}
		}

		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x06001D8A RID: 7562 RVA: 0x0006D82F File Offset: 0x0006BA2F
		// (set) Token: 0x06001D8B RID: 7563 RVA: 0x0006D837 File Offset: 0x0006BA37
		[DataSourceProperty]
		public ClanFinanceWorkshopItemVM CurrentSelectedIncome
		{
			get
			{
				return this._currentSelectedIncome;
			}
			set
			{
				if (value != this._currentSelectedIncome)
				{
					this._currentSelectedIncome = value;
					base.OnPropertyChangedWithValue<ClanFinanceWorkshopItemVM>(value, "CurrentSelectedIncome");
					this.IsAnyValidIncomeSelected = value != null;
					this.IsAnyValidSupporterSelected = false;
					this.IsAnyValidAlleySelected = false;
				}
			}
		}

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x06001D8C RID: 7564 RVA: 0x0006D86D File Offset: 0x0006BA6D
		// (set) Token: 0x06001D8D RID: 7565 RVA: 0x0006D875 File Offset: 0x0006BA75
		[DataSourceProperty]
		public ClanSupporterGroupVM CurrentSelectedSupporterGroup
		{
			get
			{
				return this._currentSelectedSupporterGroup;
			}
			set
			{
				if (value != this._currentSelectedSupporterGroup)
				{
					this._currentSelectedSupporterGroup = value;
					base.OnPropertyChangedWithValue<ClanSupporterGroupVM>(value, "CurrentSelectedSupporterGroup");
					this.IsAnyValidSupporterSelected = value != null;
					this.IsAnyValidIncomeSelected = false;
					this.IsAnyValidAlleySelected = false;
				}
			}
		}

		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x06001D8E RID: 7566 RVA: 0x0006D8AB File Offset: 0x0006BAAB
		// (set) Token: 0x06001D8F RID: 7567 RVA: 0x0006D8B3 File Offset: 0x0006BAB3
		[DataSourceProperty]
		public bool IsAnyValidAlleySelected
		{
			get
			{
				return this._isAnyValidAlleySelected;
			}
			set
			{
				if (value != this._isAnyValidAlleySelected)
				{
					this._isAnyValidAlleySelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyValidAlleySelected");
				}
			}
		}

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x06001D90 RID: 7568 RVA: 0x0006D8D1 File Offset: 0x0006BAD1
		// (set) Token: 0x06001D91 RID: 7569 RVA: 0x0006D8D9 File Offset: 0x0006BAD9
		[DataSourceProperty]
		public bool IsAnyValidIncomeSelected
		{
			get
			{
				return this._isAnyValidIncomeSelected;
			}
			set
			{
				if (value != this._isAnyValidIncomeSelected)
				{
					this._isAnyValidIncomeSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyValidIncomeSelected");
				}
			}
		}

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x06001D92 RID: 7570 RVA: 0x0006D8F7 File Offset: 0x0006BAF7
		// (set) Token: 0x06001D93 RID: 7571 RVA: 0x0006D8FF File Offset: 0x0006BAFF
		[DataSourceProperty]
		public bool IsAnyValidSupporterSelected
		{
			get
			{
				return this._isAnyValidSupporterSelected;
			}
			set
			{
				if (value != this._isAnyValidSupporterSelected)
				{
					this._isAnyValidSupporterSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyValidSupporterSelected");
				}
			}
		}

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x06001D94 RID: 7572 RVA: 0x0006D91D File Offset: 0x0006BB1D
		// (set) Token: 0x06001D95 RID: 7573 RVA: 0x0006D925 File Offset: 0x0006BB25
		[DataSourceProperty]
		public string IncomeText
		{
			get
			{
				return this._incomeText;
			}
			set
			{
				if (value != this._incomeText)
				{
					this._incomeText = value;
					base.OnPropertyChangedWithValue<string>(value, "IncomeText");
				}
			}
		}

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x06001D96 RID: 7574 RVA: 0x0006D948 File Offset: 0x0006BB48
		// (set) Token: 0x06001D97 RID: 7575 RVA: 0x0006D950 File Offset: 0x0006BB50
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x06001D98 RID: 7576 RVA: 0x0006D96E File Offset: 0x0006BB6E
		// (set) Token: 0x06001D99 RID: 7577 RVA: 0x0006D976 File Offset: 0x0006BB76
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

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x06001D9A RID: 7578 RVA: 0x0006D999 File Offset: 0x0006BB99
		// (set) Token: 0x06001D9B RID: 7579 RVA: 0x0006D9A1 File Offset: 0x0006BBA1
		[DataSourceProperty]
		public string LocationText
		{
			get
			{
				return this._locationText;
			}
			set
			{
				if (value != this._locationText)
				{
					this._locationText = value;
					base.OnPropertyChangedWithValue<string>(value, "LocationText");
				}
			}
		}

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x06001D9C RID: 7580 RVA: 0x0006D9C4 File Offset: 0x0006BBC4
		// (set) Token: 0x06001D9D RID: 7581 RVA: 0x0006D9CC File Offset: 0x0006BBCC
		[DataSourceProperty]
		public string WorkshopText
		{
			get
			{
				return this._workshopsText;
			}
			set
			{
				if (value != this._workshopsText)
				{
					this._workshopsText = value;
					base.OnPropertyChangedWithValue<string>(value, "WorkshopText");
				}
			}
		}

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x06001D9E RID: 7582 RVA: 0x0006D9EF File Offset: 0x0006BBEF
		// (set) Token: 0x06001D9F RID: 7583 RVA: 0x0006D9F7 File Offset: 0x0006BBF7
		[DataSourceProperty]
		public string SupportersText
		{
			get
			{
				return this._supportersText;
			}
			set
			{
				if (value != this._supportersText)
				{
					this._supportersText = value;
					base.OnPropertyChangedWithValue<string>(value, "SupportersText");
				}
			}
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x06001DA0 RID: 7584 RVA: 0x0006DA1A File Offset: 0x0006BC1A
		// (set) Token: 0x06001DA1 RID: 7585 RVA: 0x0006DA22 File Offset: 0x0006BC22
		[DataSourceProperty]
		public string AlleysText
		{
			get
			{
				return this._alleysText;
			}
			set
			{
				if (value != this._alleysText)
				{
					this._alleysText = value;
					base.OnPropertyChangedWithValue<string>(value, "AlleysText");
				}
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x06001DA2 RID: 7586 RVA: 0x0006DA45 File Offset: 0x0006BC45
		// (set) Token: 0x06001DA3 RID: 7587 RVA: 0x0006DA4D File Offset: 0x0006BC4D
		[DataSourceProperty]
		public string NoAdditionalIncomesText
		{
			get
			{
				return this._noAdditionalIncomesText;
			}
			set
			{
				if (this._noAdditionalIncomesText != value)
				{
					this._noAdditionalIncomesText = value;
					base.OnPropertyChangedWithValue<string>(value, "NoAdditionalIncomesText");
				}
			}
		}

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x06001DA4 RID: 7588 RVA: 0x0006DA70 File Offset: 0x0006BC70
		// (set) Token: 0x06001DA5 RID: 7589 RVA: 0x0006DA78 File Offset: 0x0006BC78
		[DataSourceProperty]
		public MBBindingList<ClanFinanceWorkshopItemVM> Incomes
		{
			get
			{
				return this._incomes;
			}
			set
			{
				if (value != this._incomes)
				{
					this._incomes = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanFinanceWorkshopItemVM>>(value, "Incomes");
				}
			}
		}

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x06001DA6 RID: 7590 RVA: 0x0006DA96 File Offset: 0x0006BC96
		// (set) Token: 0x06001DA7 RID: 7591 RVA: 0x0006DA9E File Offset: 0x0006BC9E
		[DataSourceProperty]
		public MBBindingList<ClanSupporterGroupVM> SupporterGroups
		{
			get
			{
				return this._supporterGroups;
			}
			set
			{
				if (value != this._supporterGroups)
				{
					this._supporterGroups = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanSupporterGroupVM>>(value, "SupporterGroups");
				}
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x06001DA8 RID: 7592 RVA: 0x0006DABC File Offset: 0x0006BCBC
		// (set) Token: 0x06001DA9 RID: 7593 RVA: 0x0006DAC4 File Offset: 0x0006BCC4
		[DataSourceProperty]
		public MBBindingList<ClanFinanceAlleyItemVM> Alleys
		{
			get
			{
				return this._alleys;
			}
			set
			{
				if (value != this._alleys)
				{
					this._alleys = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanFinanceAlleyItemVM>>(value, "Alleys");
				}
			}
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x06001DAA RID: 7594 RVA: 0x0006DAE2 File Offset: 0x0006BCE2
		// (set) Token: 0x06001DAB RID: 7595 RVA: 0x0006DAEA File Offset: 0x0006BCEA
		[DataSourceProperty]
		public ClanIncomeSortControllerVM SortController
		{
			get
			{
				return this._sortController;
			}
			set
			{
				if (value != this._sortController)
				{
					this._sortController = value;
					base.OnPropertyChangedWithValue<ClanIncomeSortControllerVM>(value, "SortController");
				}
			}
		}

		// Token: 0x04000DC6 RID: 3526
		private readonly Action _onRefresh;

		// Token: 0x04000DC7 RID: 3527
		private readonly Action<ClanCardSelectionInfo> _openCardSelectionPopup;

		// Token: 0x04000DC9 RID: 3529
		private MBBindingList<ClanFinanceWorkshopItemVM> _incomes;

		// Token: 0x04000DCA RID: 3530
		private MBBindingList<ClanSupporterGroupVM> _supporterGroups;

		// Token: 0x04000DCB RID: 3531
		private MBBindingList<ClanFinanceAlleyItemVM> _alleys;

		// Token: 0x04000DCC RID: 3532
		private ClanFinanceAlleyItemVM _currentSelectedAlley;

		// Token: 0x04000DCD RID: 3533
		private ClanFinanceWorkshopItemVM _currentSelectedIncome;

		// Token: 0x04000DCE RID: 3534
		private ClanSupporterGroupVM _currentSelectedSupporterGroup;

		// Token: 0x04000DCF RID: 3535
		private bool _isSelected;

		// Token: 0x04000DD0 RID: 3536
		private string _nameText;

		// Token: 0x04000DD1 RID: 3537
		private string _incomeText;

		// Token: 0x04000DD2 RID: 3538
		private string _locationText;

		// Token: 0x04000DD3 RID: 3539
		private string _workshopsText;

		// Token: 0x04000DD4 RID: 3540
		private string _supportersText;

		// Token: 0x04000DD5 RID: 3541
		private string _alleysText;

		// Token: 0x04000DD6 RID: 3542
		private string _noAdditionalIncomesText;

		// Token: 0x04000DD7 RID: 3543
		private bool _isAnyValidAlleySelected;

		// Token: 0x04000DD8 RID: 3544
		private bool _isAnyValidIncomeSelected;

		// Token: 0x04000DD9 RID: 3545
		private bool _isAnyValidSupporterSelected;

		// Token: 0x04000DDA RID: 3546
		private ClanIncomeSortControllerVM _sortController;
	}
}
