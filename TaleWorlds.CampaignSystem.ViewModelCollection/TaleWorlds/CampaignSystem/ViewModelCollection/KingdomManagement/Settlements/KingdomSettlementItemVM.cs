using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Helpers;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Armies;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Settlements
{
	// Token: 0x02000067 RID: 103
	public class KingdomSettlementItemVM : KingdomItemVM
	{
		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x00024BDB File Offset: 0x00022DDB
		// (set) Token: 0x060007F8 RID: 2040 RVA: 0x00024BE3 File Offset: 0x00022DE3
		public int Garrison { get; private set; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x00024BEC File Offset: 0x00022DEC
		// (set) Token: 0x060007FA RID: 2042 RVA: 0x00024BF4 File Offset: 0x00022DF4
		public int Militia { get; private set; }

		// Token: 0x060007FB RID: 2043 RVA: 0x00024C00 File Offset: 0x00022E00
		public KingdomSettlementItemVM(Settlement settlement, Action<KingdomSettlementItemVM> onSelect)
		{
			this.Settlement = settlement;
			this._onSelect = onSelect;
			this.Name = settlement.Name.ToString();
			this.Villages = new MBBindingList<KingdomSettlementVillageItemVM>();
			SettlementComponent settlementComponent = settlement.SettlementComponent;
			this.SettlementImagePath = ((settlementComponent == null) ? "placeholder" : (settlementComponent.BackgroundMeshName + "_t"));
			this.ItemProperties = new MBBindingList<SelectableFiefItemPropertyVM>();
			this.ImageName = ((settlementComponent != null) ? settlementComponent.WaitMeshName : "");
			this.Owner = new HeroVM(settlement.OwnerClan.Leader, false);
			this.OwnerClanBanner = new BannerImageIdentifierVM(this.Settlement.OwnerClan.Banner, false);
			this.OwnerClanBanner_9 = new BannerImageIdentifierVM(this.Settlement.OwnerClan.Banner, true);
			Town town = settlement.Town;
			this.WallLevel = ((town == null) ? (-1) : town.GetWallLevel());
			if (town != null)
			{
				this.Prosperity = MathF.Round(town.Prosperity);
				this.IconPath = town.BackgroundMeshName;
			}
			else if (settlement.IsCastle)
			{
				this.Prosperity = MathF.Round(settlement.Town.Prosperity);
				this.IconPath = "";
			}
			foreach (Village village in this.Settlement.BoundVillages)
			{
				this.Villages.Add(new KingdomSettlementVillageItemVM(village));
			}
			int num;
			if (!this.Settlement.IsFortification)
			{
				num = (int)this.Settlement.Militia;
			}
			else
			{
				MobileParty garrisonParty = this.Settlement.Town.GarrisonParty;
				num = ((garrisonParty != null) ? garrisonParty.Party.NumberOfAllMembers : 0);
			}
			this.Defenders = num;
			this.RefreshValues();
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00024DD8 File Offset: 0x00022FD8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Villages.ApplyActionOnAllItems(delegate(KingdomSettlementVillageItemVM x)
			{
				x.RefreshValues();
			});
			this.UpdateProperties();
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00024E10 File Offset: 0x00023010
		protected virtual void UpdateProperties()
		{
			this.ItemProperties.Clear();
			if (this.Settlement.Town != null)
			{
				BasicTooltipViewModel basicTooltipViewModel = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownWallsTooltip(this.Settlement.Town));
				this.ItemProperties.Add(new SelectableFiefItemPropertyVM(GameTexts.FindText("str_walls", null).ToString(), this.Settlement.Town.GetWallLevel().ToString(), 0, SelectableItemPropertyVM.PropertyType.Wall, basicTooltipViewModel, false));
				BasicTooltipViewModel basicTooltipViewModel2 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownGarrisonTooltip(this.Settlement.Town));
				int num = (int)SettlementHelper.GetGarrisonChangeExplainedNumber(this.Settlement.Town).ResultNumber;
				Collection<SelectableFiefItemPropertyVM> itemProperties = this.ItemProperties;
				string text = GameTexts.FindText("str_garrison", null).ToString();
				MobileParty garrisonParty = this.Settlement.Town.GarrisonParty;
				itemProperties.Add(new SelectableFiefItemPropertyVM(text, ((garrisonParty != null) ? garrisonParty.Party.NumberOfAllMembers.ToString() : null) ?? "0", num, SelectableItemPropertyVM.PropertyType.Garrison, basicTooltipViewModel2, false));
			}
			int num2 = (int)this.Settlement.Militia;
			List<TooltipProperty> militiaHint = (this.Settlement.IsVillage ? CampaignUIHelper.GetVillageMilitiaTooltip(this.Settlement.Village) : CampaignUIHelper.GetTownMilitiaTooltip(this.Settlement.Town));
			int num3 = ((this.Settlement.Town != null) ? ((int)this.Settlement.Town.MilitiaChange) : ((int)this.Settlement.Village.MilitiaChange));
			this.ItemProperties.Add(new SelectableFiefItemPropertyVM(GameTexts.FindText("str_militia", null).ToString(), num2.ToString(), num3, SelectableItemPropertyVM.PropertyType.Militia, new BasicTooltipViewModel(() => militiaHint), false));
			if (this.Settlement.Town != null)
			{
				BasicTooltipViewModel basicTooltipViewModel3 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownFoodTooltip(this.Settlement.Town));
				int num4 = (int)this.Settlement.Town.FoodChange;
				this.ItemProperties.Add(new SelectableFiefItemPropertyVM(GameTexts.FindText("str_food_stocks", null).ToString(), ((int)this.Settlement.Town.FoodStocks).ToString(), num4, SelectableItemPropertyVM.PropertyType.Food, basicTooltipViewModel3, false));
			}
			int num5 = ((this.Settlement.Town != null) ? ((int)this.Settlement.Town.ProsperityChange) : ((int)this.Settlement.Village.HearthChange));
			if (this.Settlement.IsFortification)
			{
				BasicTooltipViewModel basicTooltipViewModel4;
				if (this.Settlement.Town != null)
				{
					basicTooltipViewModel4 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownProsperityTooltip(this.Settlement.Town));
				}
				else
				{
					basicTooltipViewModel4 = new BasicTooltipViewModel(() => CampaignUIHelper.GetVillageProsperityTooltip(this.Settlement.Village));
				}
				this.ItemProperties.Add(new SelectableFiefItemPropertyVM(GameTexts.FindText("str_prosperity", null).ToString(), string.Format("{0:0.#}", this.Settlement.Town.Prosperity), num5, SelectableItemPropertyVM.PropertyType.Prosperity, basicTooltipViewModel4, false));
			}
			if (this.Settlement.Town != null)
			{
				BasicTooltipViewModel basicTooltipViewModel5 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownLoyaltyTooltip(this.Settlement.Town));
				int num6 = (int)this.Settlement.Town.LoyaltyChange;
				bool flag = this.Settlement.IsTown && this.Settlement.Town.Loyalty < (float)Campaign.Current.Models.SettlementLoyaltyModel.RebelliousStateStartLoyaltyThreshold;
				this.ItemProperties.Add(new SelectableFiefItemPropertyVM(GameTexts.FindText("str_loyalty", null).ToString(), string.Format("{0:0.#}", this.Settlement.Town.Loyalty), num6, SelectableItemPropertyVM.PropertyType.Loyalty, basicTooltipViewModel5, flag));
				BasicTooltipViewModel basicTooltipViewModel6 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownSecurityTooltip(this.Settlement.Town));
				int num7 = (int)this.Settlement.Town.SecurityChange;
				this.ItemProperties.Add(new SelectableFiefItemPropertyVM(GameTexts.FindText("str_security", null).ToString(), string.Format("{0:0.#}", this.Settlement.Town.Security), num7, SelectableItemPropertyVM.PropertyType.Security, basicTooltipViewModel6, false));
			}
			if (this.Settlement.IsTown)
			{
				BasicTooltipViewModel basicTooltipViewModel7 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownPatrolTooltip(this.Settlement.Town));
				this.ItemProperties.Add(new SelectableFiefItemPropertyVM(GameTexts.FindText("str_patrol", null).ToString(), Campaign.Current.GetCampaignBehavior<IPatrolPartiesCampaignBehavior>().GetSettlementPatrolStatus(this.Settlement).ToString(), 0, SelectableItemPropertyVM.PropertyType.Patrol, basicTooltipViewModel7, false));
			}
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00025278 File Offset: 0x00023478
		protected override void OnSelect()
		{
			base.OnSelect();
			this._onSelect(this);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0002528C File Offset: 0x0002348C
		private void ExecuteBeginHint()
		{
			InformationManager.ShowTooltip(typeof(Settlement), new object[] { this.Settlement, true });
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x000252B5 File Offset: 0x000234B5
		private void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x000252BC File Offset: 0x000234BC
		public void ExecuteLink()
		{
			if (this.Settlement != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this.Settlement.EncyclopediaLink);
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x000252E0 File Offset: 0x000234E0
		// (set) Token: 0x06000803 RID: 2051 RVA: 0x000252E8 File Offset: 0x000234E8
		[DataSourceProperty]
		public MBBindingList<SelectableFiefItemPropertyVM> ItemProperties
		{
			get
			{
				return this._itemProperties;
			}
			set
			{
				if (value != this._itemProperties)
				{
					this._itemProperties = value;
					base.OnPropertyChangedWithValue<MBBindingList<SelectableFiefItemPropertyVM>>(value, "ItemProperties");
				}
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x00025306 File Offset: 0x00023506
		// (set) Token: 0x06000805 RID: 2053 RVA: 0x0002530E File Offset: 0x0002350E
		[DataSourceProperty]
		public MBBindingList<KingdomSettlementVillageItemVM> Villages
		{
			get
			{
				return this._villages;
			}
			set
			{
				if (value != this._villages)
				{
					this._villages = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomSettlementVillageItemVM>>(value, "Villages");
				}
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x0002532C File Offset: 0x0002352C
		// (set) Token: 0x06000807 RID: 2055 RVA: 0x00025334 File Offset: 0x00023534
		[DataSourceProperty]
		public string IconPath
		{
			get
			{
				return this._iconPath;
			}
			set
			{
				if (value != this._iconPath)
				{
					this._iconPath = value;
					base.OnPropertyChangedWithValue<string>(value, "IconPath");
				}
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x00025357 File Offset: 0x00023557
		// (set) Token: 0x06000809 RID: 2057 RVA: 0x0002535F File Offset: 0x0002355F
		[DataSourceProperty]
		public int Defenders
		{
			get
			{
				return this._defenders;
			}
			set
			{
				if (value != this._defenders)
				{
					this._defenders = value;
					base.OnPropertyChangedWithValue(value, "Defenders");
				}
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x0002537D File Offset: 0x0002357D
		// (set) Token: 0x0600080B RID: 2059 RVA: 0x00025385 File Offset: 0x00023585
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x000253A8 File Offset: 0x000235A8
		// (set) Token: 0x0600080D RID: 2061 RVA: 0x000253B0 File Offset: 0x000235B0
		[DataSourceProperty]
		public string ImageName
		{
			get
			{
				return this._imageName;
			}
			set
			{
				if (value != this._imageName)
				{
					this._imageName = value;
					base.OnPropertyChangedWithValue<string>(value, "ImageName");
				}
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x000253D3 File Offset: 0x000235D3
		// (set) Token: 0x0600080F RID: 2063 RVA: 0x000253DB File Offset: 0x000235DB
		[DataSourceProperty]
		public string SettlementImagePath
		{
			get
			{
				return this._settlementImagePath;
			}
			set
			{
				if (value != this._settlementImagePath)
				{
					this._settlementImagePath = value;
					base.OnPropertyChangedWithValue<string>(value, "SettlementImagePath");
				}
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x000253FE File Offset: 0x000235FE
		// (set) Token: 0x06000811 RID: 2065 RVA: 0x00025406 File Offset: 0x00023606
		[DataSourceProperty]
		public string GovernorName
		{
			get
			{
				return this._governorName;
			}
			set
			{
				if (value != this._governorName)
				{
					this._governorName = value;
					base.OnPropertyChangedWithValue<string>(value, "GovernorName");
				}
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x00025429 File Offset: 0x00023629
		// (set) Token: 0x06000813 RID: 2067 RVA: 0x00025431 File Offset: 0x00023631
		[DataSourceProperty]
		public BannerImageIdentifierVM OwnerClanBanner
		{
			get
			{
				return this._ownerClanBanner;
			}
			set
			{
				if (value != this._ownerClanBanner)
				{
					this._ownerClanBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "OwnerClanBanner");
				}
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x0002544F File Offset: 0x0002364F
		// (set) Token: 0x06000815 RID: 2069 RVA: 0x00025457 File Offset: 0x00023657
		[DataSourceProperty]
		public BannerImageIdentifierVM OwnerClanBanner_9
		{
			get
			{
				return this._ownerClanBanner_9;
			}
			set
			{
				if (value != this._ownerClanBanner_9)
				{
					this._ownerClanBanner_9 = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "OwnerClanBanner_9");
				}
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x00025475 File Offset: 0x00023675
		// (set) Token: 0x06000817 RID: 2071 RVA: 0x0002547D File Offset: 0x0002367D
		[DataSourceProperty]
		public HeroVM Owner
		{
			get
			{
				return this._owner;
			}
			set
			{
				if (value != this._owner)
				{
					this._owner = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "Owner");
				}
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x0002549B File Offset: 0x0002369B
		// (set) Token: 0x06000819 RID: 2073 RVA: 0x000254A3 File Offset: 0x000236A3
		[DataSourceProperty]
		public int WallLevel
		{
			get
			{
				return this._wallLevel;
			}
			set
			{
				if (value != this._wallLevel)
				{
					this._wallLevel = value;
					base.OnPropertyChangedWithValue(value, "WallLevel");
				}
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x000254C1 File Offset: 0x000236C1
		// (set) Token: 0x0600081B RID: 2075 RVA: 0x000254C9 File Offset: 0x000236C9
		[DataSourceProperty]
		public int Prosperity
		{
			get
			{
				return this._prosperity;
			}
			set
			{
				if (value != this._prosperity)
				{
					this._prosperity = value;
					base.OnPropertyChangedWithValue(value, "Prosperity");
				}
			}
		}

		// Token: 0x04000371 RID: 881
		private readonly Action<KingdomSettlementItemVM> _onSelect;

		// Token: 0x04000372 RID: 882
		public readonly Settlement Settlement;

		// Token: 0x04000375 RID: 885
		private string _iconPath;

		// Token: 0x04000376 RID: 886
		private string _name;

		// Token: 0x04000377 RID: 887
		private string _imageName;

		// Token: 0x04000378 RID: 888
		private string _settlementImagePath;

		// Token: 0x04000379 RID: 889
		private string _governorName;

		// Token: 0x0400037A RID: 890
		private BannerImageIdentifierVM _ownerClanBanner;

		// Token: 0x0400037B RID: 891
		private BannerImageIdentifierVM _ownerClanBanner_9;

		// Token: 0x0400037C RID: 892
		private HeroVM _owner;

		// Token: 0x0400037D RID: 893
		private MBBindingList<SelectableFiefItemPropertyVM> _itemProperties;

		// Token: 0x0400037E RID: 894
		private MBBindingList<KingdomSettlementVillageItemVM> _villages;

		// Token: 0x0400037F RID: 895
		private int _wallLevel;

		// Token: 0x04000380 RID: 896
		private int _prosperity;

		// Token: 0x04000381 RID: 897
		private int _defenders;
	}
}
