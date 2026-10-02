using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000AC RID: 172
	public class TownManagementVM : ViewModel
	{
		// Token: 0x06001078 RID: 4216 RVA: 0x00043108 File Offset: 0x00041308
		public TownManagementVM()
		{
			this._settlement = Settlement.CurrentSettlement;
			Settlement settlement = this._settlement;
			if (((settlement != null) ? settlement.Town : null) == null)
			{
				Debug.FailedAssert("Town management initialized with null settlement and/or town!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\GameMenu\\TownManagement\\TownManagementVM.cs", ".ctor", 27);
				Debug.Print("Town management initialized with null settlement and/or town!", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			this.ProjectSelection = new SettlementProjectSelectionVM(this._settlement, new Action(this.OnChangeInBuildingQueue));
			this.GovernorSelection = new SettlementGovernorSelectionVM(this._settlement, new Action<Hero>(this.OnGovernorSelectionDone));
			this.ReserveControl = new TownManagementReserveControlVM(this._settlement, new Action(this.OnReserveUpdated));
			this.MiddleFirstTextList = new MBBindingList<TownManagementDescriptionItemVM>();
			this.MiddleSecondTextList = new MBBindingList<TownManagementDescriptionItemVM>();
			this.Shops = new MBBindingList<TownManagementShopItemVM>();
			this.Villages = new MBBindingList<TownManagementVillageItemVM>();
			this.Show = false;
			this.IsTown = this._settlement.IsTown;
			this.IsThereCurrentProject = this._settlement.Town.CurrentBuilding != null;
			this.CurrentGovernor = new HeroVM(this._settlement.Town.Governor ?? CampaignUIHelper.GetTeleportingGovernor(this._settlement, Campaign.Current.GetCampaignBehavior<ITeleportationCampaignBehavior>()), true);
			if (this.CurrentGovernor.Hero != null)
			{
				this.CurrentGovernorTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetHeroGovernorEffectsTooltip(this.CurrentGovernor.Hero, this._settlement));
			}
			else
			{
				this.CurrentGovernorTooltip = new BasicTooltipViewModel(() => this.GetAssignGovernorTooltip());
			}
			this.UpdateGovernorSelectionProperties();
			this.RefreshCurrentDevelopment();
			this.RefreshTownManagementStats();
			foreach (Workshop workshop in this._settlement.Town.Workshops)
			{
				WorkshopType workshopType = workshop.WorkshopType;
				if (workshopType != null && !workshopType.IsHidden)
				{
					this.Shops.Add(new TownManagementShopItemVM(workshop));
				}
			}
			foreach (Village village in this._settlement.BoundVillages)
			{
				this.Villages.Add(new TownManagementVillageItemVM(village));
			}
			this.ConsumptionTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetSettlementConsumptionTooltip(this._settlement));
			this.RefreshValues();
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x00043360 File Offset: 0x00041560
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CurrentProjectText = new TextObject("{=qBq70qDq}Current Project", null).ToString();
			this.CompletionText = new TextObject("{=Rkh2k1OA}Completion:", null).ToString();
			this.ManageText = new TextObject("{=XseYJYka}Manage", null).ToString();
			this.DoneText = new TextObject("{=WiNRdfsm}Done", null).ToString();
			this.WallsText = new TextObject("{=LsZEdD2z}Walls", null).ToString();
			this.VillagesText = GameTexts.FindText("str_bound_village", null).ToString();
			this.ShopsInSettlementText = GameTexts.FindText("str_shops_in_settlement", null).ToString();
			this.GovernorText = GameTexts.FindText("str_sort_by_governor_label", null).ToString();
			this.MiddleFirstTextList.ApplyActionOnAllItems(delegate(TownManagementDescriptionItemVM x)
			{
				x.RefreshValues();
			});
			this.MiddleSecondTextList.ApplyActionOnAllItems(delegate(TownManagementDescriptionItemVM x)
			{
				x.RefreshValues();
			});
			this.ProjectSelection.RefreshValues();
			this.GovernorSelection.RefreshValues();
			this.ReserveControl.RefreshValues();
			this.Shops.ApplyActionOnAllItems(delegate(TownManagementShopItemVM x)
			{
				x.RefreshValues();
			});
			this.Villages.ApplyActionOnAllItems(delegate(TownManagementVillageItemVM x)
			{
				x.RefreshValues();
			});
			this.CurrentGovernor.RefreshValues();
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x000434F8 File Offset: 0x000416F8
		private void RefreshTownManagementStats()
		{
			this.MiddleFirstTextList.Clear();
			this.MiddleSecondTextList.Clear();
			ExplainedNumber taxExplanation = Campaign.Current.Models.SettlementTaxModel.CalculateTownTax(this._settlement.Town, true);
			int taxValue = (int)taxExplanation.ResultNumber;
			BasicTooltipViewModel basicTooltipViewModel = new BasicTooltipViewModel(() => CampaignUIHelper.GetTooltipForAccumulatingPropertyWithResult(GameTexts.FindText("str_town_management_population_tax", null).ToString(), (float)taxValue, ref taxExplanation));
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_town_management_population_tax", null));
			this.MiddleFirstTextList.Add(new TownManagementDescriptionItemVM(GameTexts.FindText("str_LEFT_colon", null), taxValue, 0, TownManagementDescriptionItemVM.DescriptionType.Gold, basicTooltipViewModel));
			BasicTooltipViewModel basicTooltipViewModel2 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownDailyProductionTooltip(this._settlement.Town));
			this.MiddleFirstTextList.Add(new TownManagementDescriptionItemVM(GameTexts.FindText("str_town_management_daily_production", null), (int)Campaign.Current.Models.BuildingConstructionModel.CalculateDailyConstructionPower(this._settlement.Town, false).ResultNumber, 0, TownManagementDescriptionItemVM.DescriptionType.Production, basicTooltipViewModel2));
			BasicTooltipViewModel basicTooltipViewModel3 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownProsperityTooltip(this._settlement.Town));
			this.MiddleFirstTextList.Add(new TownManagementDescriptionItemVM(GameTexts.FindText("str_town_management_prosperity", null), (int)this._settlement.Town.Prosperity, MathF.Round(Campaign.Current.Models.SettlementProsperityModel.CalculateProsperityChange(this._settlement.Town, false).ResultNumber), TownManagementDescriptionItemVM.DescriptionType.Prosperity, basicTooltipViewModel3));
			BasicTooltipViewModel basicTooltipViewModel4 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownFoodTooltip(this._settlement.Town));
			this.MiddleFirstTextList.Add(new TownManagementDescriptionItemVM(GameTexts.FindText("str_town_management_food", null), (int)this._settlement.Town.FoodStocks, MathF.Round(Campaign.Current.Models.SettlementFoodModel.CalculateTownFoodStocksChange(this._settlement.Town, true, false).ResultNumber), TownManagementDescriptionItemVM.DescriptionType.Food, basicTooltipViewModel4));
			BasicTooltipViewModel basicTooltipViewModel5 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownLoyaltyTooltip(this._settlement.Town));
			this.MiddleSecondTextList.Add(new TownManagementDescriptionItemVM(GameTexts.FindText("str_town_management_loyalty", null), (int)this._settlement.Town.Loyalty, MathF.Round(Campaign.Current.Models.SettlementLoyaltyModel.CalculateLoyaltyChange(this._settlement.Town, false).ResultNumber), TownManagementDescriptionItemVM.DescriptionType.Loyalty, basicTooltipViewModel5));
			BasicTooltipViewModel basicTooltipViewModel6 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownSecurityTooltip(this._settlement.Town));
			this.MiddleSecondTextList.Add(new TownManagementDescriptionItemVM(GameTexts.FindText("str_town_management_security", null), (int)this._settlement.Town.Security, MathF.Round(Campaign.Current.Models.SettlementSecurityModel.CalculateSecurityChange(this._settlement.Town, false).ResultNumber), TownManagementDescriptionItemVM.DescriptionType.Security, basicTooltipViewModel6));
			BasicTooltipViewModel basicTooltipViewModel7 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownMilitiaTooltip(this._settlement.Town));
			this.MiddleSecondTextList.Add(new TownManagementDescriptionItemVM(GameTexts.FindText("str_town_management_militia", null), (int)this._settlement.Militia, MathF.Round(Campaign.Current.Models.SettlementMilitiaModel.CalculateMilitiaChange(this._settlement, false).ResultNumber), TownManagementDescriptionItemVM.DescriptionType.Militia, basicTooltipViewModel7));
			BasicTooltipViewModel basicTooltipViewModel8 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownGarrisonTooltip(this._settlement.Town));
			Collection<TownManagementDescriptionItemVM> middleSecondTextList = this.MiddleSecondTextList;
			TextObject textObject = GameTexts.FindText("str_town_management_garrison", null);
			MobileParty garrisonParty = this._settlement.Town.GarrisonParty;
			middleSecondTextList.Add(new TownManagementDescriptionItemVM(textObject, (garrisonParty != null) ? garrisonParty.Party.NumberOfAllMembers : 0, MathF.Round(SettlementHelper.GetGarrisonChangeExplainedNumber(this._settlement.Town).ResultNumber), TownManagementDescriptionItemVM.DescriptionType.Garrison, basicTooltipViewModel8));
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x0004389A File Offset: 0x00041A9A
		private void OnChangeInBuildingQueue()
		{
			this.OnProjectSelectionDone();
			this.RefreshTownManagementStats();
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x000438A8 File Offset: 0x00041AA8
		private void RefreshCurrentDevelopment()
		{
			if (this._settlement.Town.CurrentBuilding != null)
			{
				this.IsCurrentProjectDaily = this._settlement.Town.CurrentBuilding.BuildingType.IsDailyProject;
				if (!this.IsCurrentProjectDaily)
				{
					this.CurrentProjectProgress = (int)(BuildingHelper.GetProgressOfBuilding(this.ProjectSelection.CurrentSelectedProject.Building, this._settlement.Town) * 100f);
					this.ProjectSelection.CurrentSelectedProject.RefreshProductionText();
				}
			}
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x0004392C File Offset: 0x00041B2C
		private void OnProjectSelectionDone()
		{
			List<Building> localDevelopmentList = this.ProjectSelection.LocalDevelopmentList;
			Building building = this.ProjectSelection.CurrentDailyDefault.Building;
			if (localDevelopmentList != null)
			{
				BuildingHelper.ChangeCurrentBuildingQueue(localDevelopmentList, this._settlement.Town);
			}
			if (building != this._settlement.Town.Buildings.FirstOrDefault<Building>((Building k) => k.IsCurrentlyDefault) && building != null)
			{
				BuildingHelper.ChangeDefaultBuilding(building, this._settlement.Town);
			}
			this.RefreshCurrentDevelopment();
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x000439BC File Offset: 0x00041BBC
		private void OnGovernorSelectionDone(Hero selectedGovernor)
		{
			if (selectedGovernor != this.CurrentGovernor.Hero)
			{
				this.CurrentGovernor = new HeroVM(selectedGovernor, true);
				if (this.CurrentGovernor.Hero != null)
				{
					ChangeGovernorAction.Apply(this._settlement.Town, this.CurrentGovernor.Hero);
					this.CurrentGovernorTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetHeroGovernorEffectsTooltip(selectedGovernor, this._settlement));
				}
				else
				{
					ChangeGovernorAction.RemoveGovernorOfIfExists(this._settlement.Town);
					this.CurrentGovernorTooltip = new BasicTooltipViewModel(() => this.GetAssignGovernorTooltip());
				}
			}
			this.UpdateGovernorSelectionProperties();
			this.RefreshTownManagementStats();
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x00043A78 File Offset: 0x00041C78
		private void UpdateGovernorSelectionProperties()
		{
			this.HasGovernor = this.CurrentGovernor.Hero != null;
			TextObject textObject;
			this.IsGovernorSelectionEnabled = this.GetCanChangeGovernor(out textObject);
			this.GovernorSelectionDisabledHint = new HintViewModel(textObject, null);
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x00043AB4 File Offset: 0x00041CB4
		private bool GetCanChangeGovernor(out TextObject disabledReason)
		{
			HeroVM currentGovernor = this.CurrentGovernor;
			bool flag;
			if (currentGovernor == null)
			{
				flag = false;
			}
			else
			{
				Hero hero = currentGovernor.Hero;
				bool? flag2 = ((hero != null) ? new bool?(hero.IsTraveling) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			if (flag)
			{
				disabledReason = new TextObject("{=qbqimqMb}{GOVERNOR.NAME} is on the way to be the new governor of {SETTLEMENT_NAME}", null);
				if (this.CurrentGovernor.Hero.CharacterObject != null)
				{
					StringHelpers.SetCharacterProperties("GOVERNOR", this.CurrentGovernor.Hero.CharacterObject, disabledReason, false);
				}
				TextObject textObject = disabledReason;
				string text = "SETTLEMENT_NAME";
				TextObject name = this._settlement.Name;
				textObject.SetTextVariable(text, ((name != null) ? name.ToString() : null) ?? string.Empty);
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x00043B77 File Offset: 0x00041D77
		private void OnReserveUpdated()
		{
			this.RefreshCurrentDevelopment();
			this.RefreshTownManagementStats();
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x00043B85 File Offset: 0x00041D85
		public void ExecuteDone()
		{
			this.OnProjectSelectionDone();
			this.Show = false;
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x00043B94 File Offset: 0x00041D94
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.DoneInputKey.OnFinalize();
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x00043BA7 File Offset: 0x00041DA7
		private string GetAssignGovernorTooltip()
		{
			return GameTexts.FindText("str_clan_assign_governor", null).ToString();
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x00043BB9 File Offset: 0x00041DB9
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001086 RID: 4230 RVA: 0x00043BC8 File Offset: 0x00041DC8
		// (set) Token: 0x06001087 RID: 4231 RVA: 0x00043BD0 File Offset: 0x00041DD0
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

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001088 RID: 4232 RVA: 0x00043BEE File Offset: 0x00041DEE
		// (set) Token: 0x06001089 RID: 4233 RVA: 0x00043BF6 File Offset: 0x00041DF6
		[DataSourceProperty]
		public string CompletionText
		{
			get
			{
				return this._completionText;
			}
			set
			{
				if (value != this._completionText)
				{
					this._completionText = value;
					base.OnPropertyChangedWithValue<string>(value, "CompletionText");
				}
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x0600108A RID: 4234 RVA: 0x00043C19 File Offset: 0x00041E19
		// (set) Token: 0x0600108B RID: 4235 RVA: 0x00043C21 File Offset: 0x00041E21
		[DataSourceProperty]
		public string GovernorText
		{
			get
			{
				return this._governorText;
			}
			set
			{
				if (value != this._governorText)
				{
					this._governorText = value;
					base.OnPropertyChangedWithValue<string>(value, "GovernorText");
				}
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x0600108C RID: 4236 RVA: 0x00043C44 File Offset: 0x00041E44
		// (set) Token: 0x0600108D RID: 4237 RVA: 0x00043C4C File Offset: 0x00041E4C
		[DataSourceProperty]
		public string ManageText
		{
			get
			{
				return this._manageText;
			}
			set
			{
				if (value != this._manageText)
				{
					this._manageText = value;
					base.OnPropertyChangedWithValue<string>(value, "ManageText");
				}
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x0600108E RID: 4238 RVA: 0x00043C6F File Offset: 0x00041E6F
		// (set) Token: 0x0600108F RID: 4239 RVA: 0x00043C77 File Offset: 0x00041E77
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

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001090 RID: 4240 RVA: 0x00043C9A File Offset: 0x00041E9A
		// (set) Token: 0x06001091 RID: 4241 RVA: 0x00043CA2 File Offset: 0x00041EA2
		[DataSourceProperty]
		public string WallsText
		{
			get
			{
				return this._wallsText;
			}
			set
			{
				if (value != this._wallsText)
				{
					this._wallsText = value;
					base.OnPropertyChangedWithValue<string>(value, "WallsText");
				}
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001092 RID: 4242 RVA: 0x00043CC5 File Offset: 0x00041EC5
		// (set) Token: 0x06001093 RID: 4243 RVA: 0x00043CCD File Offset: 0x00041ECD
		[DataSourceProperty]
		public string CurrentProjectText
		{
			get
			{
				return this._currentProjectText;
			}
			set
			{
				if (value != this._currentProjectText)
				{
					this._currentProjectText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentProjectText");
				}
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001094 RID: 4244 RVA: 0x00043CF0 File Offset: 0x00041EF0
		// (set) Token: 0x06001095 RID: 4245 RVA: 0x00043CF8 File Offset: 0x00041EF8
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

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001096 RID: 4246 RVA: 0x00043D1B File Offset: 0x00041F1B
		// (set) Token: 0x06001097 RID: 4247 RVA: 0x00043D23 File Offset: 0x00041F23
		[DataSourceProperty]
		public bool HasGovernor
		{
			get
			{
				return this._hasGovernor;
			}
			set
			{
				if (value != this._hasGovernor)
				{
					this._hasGovernor = value;
					base.OnPropertyChangedWithValue(value, "HasGovernor");
				}
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001098 RID: 4248 RVA: 0x00043D41 File Offset: 0x00041F41
		// (set) Token: 0x06001099 RID: 4249 RVA: 0x00043D49 File Offset: 0x00041F49
		[DataSourceProperty]
		public bool IsGovernorSelectionEnabled
		{
			get
			{
				return this._isGovernorSelectionEnabled;
			}
			set
			{
				if (value != this._isGovernorSelectionEnabled)
				{
					this._isGovernorSelectionEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsGovernorSelectionEnabled");
				}
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x0600109A RID: 4250 RVA: 0x00043D67 File Offset: 0x00041F67
		// (set) Token: 0x0600109B RID: 4251 RVA: 0x00043D6F File Offset: 0x00041F6F
		[DataSourceProperty]
		public bool IsTown
		{
			get
			{
				return this._isTown;
			}
			set
			{
				if (value != this._isTown)
				{
					this._isTown = value;
					base.OnPropertyChangedWithValue(value, "IsTown");
				}
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x0600109C RID: 4252 RVA: 0x00043D8D File Offset: 0x00041F8D
		// (set) Token: 0x0600109D RID: 4253 RVA: 0x00043D95 File Offset: 0x00041F95
		[DataSourceProperty]
		public bool Show
		{
			get
			{
				return this._show;
			}
			set
			{
				if (value != this._show)
				{
					this._show = value;
					base.OnPropertyChangedWithValue(value, "Show");
				}
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x0600109E RID: 4254 RVA: 0x00043DB3 File Offset: 0x00041FB3
		// (set) Token: 0x0600109F RID: 4255 RVA: 0x00043DBB File Offset: 0x00041FBB
		[DataSourceProperty]
		public bool IsThereCurrentProject
		{
			get
			{
				return this._isThereCurrentProject;
			}
			set
			{
				if (value != this._isThereCurrentProject)
				{
					this._isThereCurrentProject = value;
					base.OnPropertyChangedWithValue(value, "IsThereCurrentProject");
				}
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x060010A0 RID: 4256 RVA: 0x00043DD9 File Offset: 0x00041FD9
		// (set) Token: 0x060010A1 RID: 4257 RVA: 0x00043DE1 File Offset: 0x00041FE1
		[DataSourceProperty]
		public bool IsSelectingGovernor
		{
			get
			{
				return this._isSelectingGovernor;
			}
			set
			{
				if (value != this._isSelectingGovernor)
				{
					this._isSelectingGovernor = value;
					base.OnPropertyChangedWithValue(value, "IsSelectingGovernor");
				}
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x060010A2 RID: 4258 RVA: 0x00043DFF File Offset: 0x00041FFF
		// (set) Token: 0x060010A3 RID: 4259 RVA: 0x00043E07 File Offset: 0x00042007
		[DataSourceProperty]
		public MBBindingList<TownManagementDescriptionItemVM> MiddleFirstTextList
		{
			get
			{
				return this._middleLeftTextList;
			}
			set
			{
				if (value != this._middleLeftTextList)
				{
					this._middleLeftTextList = value;
					base.OnPropertyChanged("MiddleLeftTextList");
				}
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x060010A4 RID: 4260 RVA: 0x00043E24 File Offset: 0x00042024
		// (set) Token: 0x060010A5 RID: 4261 RVA: 0x00043E2C File Offset: 0x0004202C
		[DataSourceProperty]
		public MBBindingList<TownManagementDescriptionItemVM> MiddleSecondTextList
		{
			get
			{
				return this._middleRightTextList;
			}
			set
			{
				if (value != this._middleRightTextList)
				{
					this._middleRightTextList = value;
					base.OnPropertyChanged("MiddleRightTextList");
				}
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x060010A6 RID: 4262 RVA: 0x00043E49 File Offset: 0x00042049
		// (set) Token: 0x060010A7 RID: 4263 RVA: 0x00043E51 File Offset: 0x00042051
		[DataSourceProperty]
		public MBBindingList<TownManagementShopItemVM> Shops
		{
			get
			{
				return this._shops;
			}
			set
			{
				if (value != this._shops)
				{
					this._shops = value;
					base.OnPropertyChangedWithValue<MBBindingList<TownManagementShopItemVM>>(value, "Shops");
				}
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x060010A8 RID: 4264 RVA: 0x00043E6F File Offset: 0x0004206F
		// (set) Token: 0x060010A9 RID: 4265 RVA: 0x00043E77 File Offset: 0x00042077
		[DataSourceProperty]
		public MBBindingList<TownManagementVillageItemVM> Villages
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
					base.OnPropertyChangedWithValue<MBBindingList<TownManagementVillageItemVM>>(value, "Villages");
				}
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x060010AA RID: 4266 RVA: 0x00043E95 File Offset: 0x00042095
		// (set) Token: 0x060010AB RID: 4267 RVA: 0x00043E9D File Offset: 0x0004209D
		[DataSourceProperty]
		public HintViewModel GovernorSelectionDisabledHint
		{
			get
			{
				return this._governorSelectionDisabledHint;
			}
			set
			{
				if (value != this._governorSelectionDisabledHint)
				{
					this._governorSelectionDisabledHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "GovernorSelectionDisabledHint");
				}
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x060010AC RID: 4268 RVA: 0x00043EBB File Offset: 0x000420BB
		// (set) Token: 0x060010AD RID: 4269 RVA: 0x00043EC3 File Offset: 0x000420C3
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

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x060010AE RID: 4270 RVA: 0x00043EE6 File Offset: 0x000420E6
		// (set) Token: 0x060010AF RID: 4271 RVA: 0x00043EEE File Offset: 0x000420EE
		[DataSourceProperty]
		public string ShopsInSettlementText
		{
			get
			{
				return this._shopsInSettlementText;
			}
			set
			{
				if (value != this._shopsInSettlementText)
				{
					this._shopsInSettlementText = value;
					base.OnPropertyChangedWithValue<string>(value, "ShopsInSettlementText");
				}
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x00043F11 File Offset: 0x00042111
		// (set) Token: 0x060010B1 RID: 4273 RVA: 0x00043F19 File Offset: 0x00042119
		[DataSourceProperty]
		public bool IsCurrentProjectDaily
		{
			get
			{
				return this._isCurrentProjectDaily;
			}
			set
			{
				if (value != this._isCurrentProjectDaily)
				{
					this._isCurrentProjectDaily = value;
					base.OnPropertyChangedWithValue(value, "IsCurrentProjectDaily");
				}
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x060010B2 RID: 4274 RVA: 0x00043F37 File Offset: 0x00042137
		// (set) Token: 0x060010B3 RID: 4275 RVA: 0x00043F3F File Offset: 0x0004213F
		[DataSourceProperty]
		public int CurrentProjectProgress
		{
			get
			{
				return this._currentProjectProgress;
			}
			set
			{
				if (value != this._currentProjectProgress)
				{
					this._currentProjectProgress = value;
					base.OnPropertyChangedWithValue(value, "CurrentProjectProgress");
				}
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x060010B4 RID: 4276 RVA: 0x00043F5D File Offset: 0x0004215D
		// (set) Token: 0x060010B5 RID: 4277 RVA: 0x00043F65 File Offset: 0x00042165
		[DataSourceProperty]
		public SettlementProjectSelectionVM ProjectSelection
		{
			get
			{
				return this._projectSelection;
			}
			set
			{
				if (value != this._projectSelection)
				{
					this._projectSelection = value;
					base.OnPropertyChangedWithValue<SettlementProjectSelectionVM>(value, "ProjectSelection");
				}
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x060010B6 RID: 4278 RVA: 0x00043F83 File Offset: 0x00042183
		// (set) Token: 0x060010B7 RID: 4279 RVA: 0x00043F8B File Offset: 0x0004218B
		[DataSourceProperty]
		public SettlementGovernorSelectionVM GovernorSelection
		{
			get
			{
				return this._governorSelection;
			}
			set
			{
				if (value != this._governorSelection)
				{
					this._governorSelection = value;
					base.OnPropertyChangedWithValue<SettlementGovernorSelectionVM>(value, "GovernorSelection");
				}
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x060010B8 RID: 4280 RVA: 0x00043FA9 File Offset: 0x000421A9
		// (set) Token: 0x060010B9 RID: 4281 RVA: 0x00043FB1 File Offset: 0x000421B1
		[DataSourceProperty]
		public TownManagementReserveControlVM ReserveControl
		{
			get
			{
				return this._reserveControl;
			}
			set
			{
				if (value != this._reserveControl)
				{
					this._reserveControl = value;
					base.OnPropertyChangedWithValue<TownManagementReserveControlVM>(value, "ReserveControl");
				}
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x060010BA RID: 4282 RVA: 0x00043FCF File Offset: 0x000421CF
		// (set) Token: 0x060010BB RID: 4283 RVA: 0x00043FD7 File Offset: 0x000421D7
		[DataSourceProperty]
		public BasicTooltipViewModel CurrentGovernorTooltip
		{
			get
			{
				return this._currentGovernorTooltip;
			}
			set
			{
				if (value != this._currentGovernorTooltip)
				{
					this._currentGovernorTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "CurrentGovernorTooltip");
				}
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x00043FF5 File Offset: 0x000421F5
		// (set) Token: 0x060010BD RID: 4285 RVA: 0x00043FFD File Offset: 0x000421FD
		[DataSourceProperty]
		public HeroVM CurrentGovernor
		{
			get
			{
				return this._currentGovernor;
			}
			set
			{
				if (value != this._currentGovernor)
				{
					this._currentGovernor = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "CurrentGovernor");
				}
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x060010BE RID: 4286 RVA: 0x0004401B File Offset: 0x0004221B
		// (set) Token: 0x060010BF RID: 4287 RVA: 0x00044023 File Offset: 0x00042223
		[DataSourceProperty]
		public BasicTooltipViewModel ConsumptionTooltip
		{
			get
			{
				return this._consumptionTooltip;
			}
			set
			{
				if (value != this._consumptionTooltip)
				{
					this._consumptionTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "ConsumptionTooltip");
				}
			}
		}

		// Token: 0x04000787 RID: 1927
		private readonly Settlement _settlement;

		// Token: 0x04000788 RID: 1928
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000789 RID: 1929
		private bool _isThereCurrentProject;

		// Token: 0x0400078A RID: 1930
		private bool _isSelectingGovernor;

		// Token: 0x0400078B RID: 1931
		private SettlementProjectSelectionVM _projectSelection;

		// Token: 0x0400078C RID: 1932
		private SettlementGovernorSelectionVM _governorSelection;

		// Token: 0x0400078D RID: 1933
		private TownManagementReserveControlVM _reserveControl;

		// Token: 0x0400078E RID: 1934
		private MBBindingList<TownManagementDescriptionItemVM> _middleLeftTextList;

		// Token: 0x0400078F RID: 1935
		private MBBindingList<TownManagementDescriptionItemVM> _middleRightTextList;

		// Token: 0x04000790 RID: 1936
		private MBBindingList<TownManagementShopItemVM> _shops;

		// Token: 0x04000791 RID: 1937
		private MBBindingList<TownManagementVillageItemVM> _villages;

		// Token: 0x04000792 RID: 1938
		private HintViewModel _governorSelectionDisabledHint;

		// Token: 0x04000793 RID: 1939
		private bool _show;

		// Token: 0x04000794 RID: 1940
		private bool _isTown;

		// Token: 0x04000795 RID: 1941
		private bool _hasGovernor;

		// Token: 0x04000796 RID: 1942
		private bool _isGovernorSelectionEnabled;

		// Token: 0x04000797 RID: 1943
		private string _titleText;

		// Token: 0x04000798 RID: 1944
		private bool _isCurrentProjectDaily;

		// Token: 0x04000799 RID: 1945
		private int _currentProjectProgress;

		// Token: 0x0400079A RID: 1946
		private string _currentProjectText;

		// Token: 0x0400079B RID: 1947
		private HeroVM _currentGovernor;

		// Token: 0x0400079C RID: 1948
		private BasicTooltipViewModel _currentGovernorTooltip;

		// Token: 0x0400079D RID: 1949
		private string _manageText;

		// Token: 0x0400079E RID: 1950
		private string _doneText;

		// Token: 0x0400079F RID: 1951
		private string _wallsText;

		// Token: 0x040007A0 RID: 1952
		private string _completionText;

		// Token: 0x040007A1 RID: 1953
		private string _villagesText;

		// Token: 0x040007A2 RID: 1954
		private string _shopsInSettlementText;

		// Token: 0x040007A3 RID: 1955
		private BasicTooltipViewModel _consumptionTooltip;

		// Token: 0x040007A4 RID: 1956
		private string _governorText;
	}
}
