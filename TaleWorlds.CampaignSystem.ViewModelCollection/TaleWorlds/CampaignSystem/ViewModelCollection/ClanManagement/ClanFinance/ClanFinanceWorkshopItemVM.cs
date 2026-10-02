using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanFinance
{
	// Token: 0x02000136 RID: 310
	public class ClanFinanceWorkshopItemVM : ClanFinanceIncomeItemBaseVM
	{
		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06001CD6 RID: 7382 RVA: 0x0006AB7F File Offset: 0x00068D7F
		// (set) Token: 0x06001CD7 RID: 7383 RVA: 0x0006AB87 File Offset: 0x00068D87
		public Workshop Workshop { get; private set; }

		// Token: 0x06001CD8 RID: 7384 RVA: 0x0006AB90 File Offset: 0x00068D90
		public ClanFinanceWorkshopItemVM(Workshop workshop, Action<ClanFinanceWorkshopItemVM> onSelection, Action onRefresh, Action<ClanCardSelectionInfo> openCardSelectionPopup)
			: base(null, onRefresh)
		{
			this._workshopWarehouseBehavior = Campaign.Current.GetCampaignBehavior<IWorkshopWarehouseCampaignBehavior>();
			this.Workshop = workshop;
			this._openCardSelectionPopup = openCardSelectionPopup;
			this._workshopModel = Campaign.Current.Models.WorkshopModel;
			base.IncomeTypeAsEnum = IncomeTypes.Workshop;
			this._onSelection = new Action<ClanFinanceIncomeItemBaseVM>(this.tempOnSelection);
			this._onSelectionT = onSelection;
			SettlementComponent settlementComponent = this.Workshop.Settlement.SettlementComponent;
			base.ImageName = ((settlementComponent != null) ? settlementComponent.WaitMeshName : "");
			this.ManageWorkshopHint = new HintViewModel(new TextObject("{=LxWVtDF0}Manage Workshop", null), null);
			this.UseWarehouseAsInputHint = new HintViewModel(new TextObject("{=a4oqWgUi}If there are no raw materials in the warehouse, the workshop will buy raw materials from the market until the warehouse is restocked", null), null);
			this.StoreOutputPercentageHint = new HintViewModel(new TextObject("{=NVUi4bB9}When the warehouse is full, the workshop will sell the products to the town market", null), null);
			this.InputWarehouseCountsTooltip = new BasicTooltipViewModel();
			this.OutputWarehouseCountsTooltip = new BasicTooltipViewModel();
			this.ReceiveInputFromWarehouse = this._workshopWarehouseBehavior.IsGettingInputsFromWarehouse(workshop);
			this.WarehousePercentageSelector = new SelectorVM<WorkshopPercentageSelectorItemVM>(0, new Action<SelectorVM<WorkshopPercentageSelectorItemVM>>(this.OnStoreOutputInWarehousePercentageUpdated));
			this.RefreshStoragePercentages();
			float currentPercentage = this._workshopWarehouseBehavior.GetStockProductionInWarehouseRatio(workshop);
			WorkshopPercentageSelectorItemVM workshopPercentageSelectorItemVM = this.WarehousePercentageSelector.ItemList.FirstOrDefault<WorkshopPercentageSelectorItemVM>((WorkshopPercentageSelectorItemVM x) => x.Percentage.ApproximatelyEqualsTo(currentPercentage, 0.1f));
			this.WarehousePercentageSelector.SelectedIndex = ((workshopPercentageSelectorItemVM != null) ? this.WarehousePercentageSelector.ItemList.IndexOf(workshopPercentageSelectorItemVM) : 0);
			this.RefreshValues();
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x0006AD5D File Offset: 0x00068F5D
		private void tempOnSelection(ClanFinanceIncomeItemBaseVM temp)
		{
			this._onSelectionT(this);
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x0006AD6C File Offset: 0x00068F6C
		public override void RefreshValues()
		{
			base.RefreshValues();
			base.Name = this.Workshop.WorkshopType.Name.ToString();
			this.WorkshopTypeId = this.Workshop.WorkshopType.StringId;
			base.Location = this.Workshop.Settlement.Name.ToString();
			base.Income = (int)((float)this.Workshop.ProfitMade * (1f / Campaign.Current.Models.ClanFinanceModel.RevenueSmoothenFraction()));
			base.IncomeValueText = base.DetermineIncomeText(base.Income);
			this.InputsText = GameTexts.FindText("str_clan_workshop_inputs", null).ToString();
			this.OutputsText = GameTexts.FindText("str_clan_workshop_outputs", null).ToString();
			this.StoreOutputPercentageText = new TextObject("{=y6qCNFQj}Store Outputs in the Warehouse", null).ToString();
			this.UseWarehouseAsInputText = new TextObject("{=88WPmTKH}Get Input from the Warehouse", null).ToString();
			this.WarehouseCapacityText = new TextObject("{=X6eG4Q5V}Warehouse Capacity", null).ToString();
			float warehouseItemRosterWeight = this._workshopWarehouseBehavior.GetWarehouseItemRosterWeight(this.Workshop.Settlement);
			int warehouseCapacity = Campaign.Current.Models.WorkshopModel.WarehouseCapacity;
			this.WarehouseCapacityValue = GameTexts.FindText("str_LEFT_over_RIGHT", null).SetTextVariable("LEFT", warehouseItemRosterWeight, 2).SetTextVariable("RIGHT", warehouseCapacity)
				.ToString();
			this.WarehouseInputAmount = this._workshopWarehouseBehavior.GetInputCount(this.Workshop);
			this.WarehouseOutputAmount = this._workshopWarehouseBehavior.GetOutputCount(this.Workshop);
			this._inputDetails = this._workshopWarehouseBehavior.GetInputDailyChange(this.Workshop);
			this._outputDetails = this._workshopWarehouseBehavior.GetOutputDailyChange(this.Workshop);
			this.InputWarehouseCountsTooltip.SetToolipCallback(() => this.GetWarehouseInputOutputTooltip(true));
			this.OutputWarehouseCountsTooltip.SetToolipCallback(() => this.GetWarehouseInputOutputTooltip(false));
			base.ItemProperties.Clear();
			this.PopulateStatsList();
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x0006AF70 File Offset: 0x00069170
		private List<TooltipProperty> GetWarehouseInputOutputTooltip(bool isInput)
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			ExplainedNumber explainedNumber = (isInput ? this._inputDetails : this._outputDetails);
			if (!explainedNumber.ResultNumber.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				list.Add(new TooltipProperty(new TextObject("{=Y9egTJg0}Daily Change", null).ToString(), "", 1, false, TooltipProperty.TooltipPropertyFlags.Title));
				list.Add(new TooltipProperty("", "", 0, false, TooltipProperty.TooltipPropertyFlags.RundownSeperator));
				foreach (ValueTuple<string, float> valueTuple in explainedNumber.GetLines())
				{
					string text = GameTexts.FindText("str_clan_workshop_material_daily_Change", null).SetTextVariable("CHANGE", MathF.Abs(valueTuple.Item2).ToString("F1")).SetTextVariable("IS_POSITIVE", (valueTuple.Item2 > 0f) ? 1 : 0)
						.ToString();
					list.Add(new TooltipProperty(valueTuple.Item1, text, 0, false, TooltipProperty.TooltipPropertyFlags.None));
				}
			}
			return list;
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x0006B09C File Offset: 0x0006929C
		private void RefreshStoragePercentages()
		{
			this.WarehousePercentageSelector.ItemList.Clear();
			TextObject textObject = GameTexts.FindText("str_NUMBER_percent", null);
			textObject.SetTextVariable("NUMBER", 0);
			this.WarehousePercentageSelector.AddItem(new WorkshopPercentageSelectorItemVM(textObject.ToString(), 0f));
			textObject.SetTextVariable("NUMBER", 25);
			this.WarehousePercentageSelector.AddItem(new WorkshopPercentageSelectorItemVM(textObject.ToString(), 0.25f));
			textObject.SetTextVariable("NUMBER", 50);
			this.WarehousePercentageSelector.AddItem(new WorkshopPercentageSelectorItemVM(textObject.ToString(), 0.5f));
			textObject.SetTextVariable("NUMBER", 75);
			this.WarehousePercentageSelector.AddItem(new WorkshopPercentageSelectorItemVM(textObject.ToString(), 0.75f));
			textObject.SetTextVariable("NUMBER", 100);
			this.WarehousePercentageSelector.AddItem(new WorkshopPercentageSelectorItemVM(textObject.ToString(), 1f));
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x0006B191 File Offset: 0x00069391
		public void ExecuteToggleWarehouseUsage()
		{
			this.ReceiveInputFromWarehouse = !this.ReceiveInputFromWarehouse;
			this._workshopWarehouseBehavior.SetIsGettingInputsFromWarehouse(this.Workshop, this.ReceiveInputFromWarehouse);
			base.ItemProperties.Clear();
			this.PopulateStatsList();
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x0006B1CC File Offset: 0x000693CC
		protected override void PopulateStatsList()
		{
			ValueTuple<TextObject, bool, BasicTooltipViewModel> workshopStatus = this.GetWorkshopStatus(this.Workshop);
			if (!TextObject.IsNullOrEmpty(workshopStatus.Item1))
			{
				base.ItemProperties.Add(new SelectableItemPropertyVM(new TextObject("{=DXczLzml}Status", null).ToString(), workshopStatus.Item1.ToString(), workshopStatus.Item2, workshopStatus.Item3));
			}
			SelectableItemPropertyVM currentCapitalProperty = this.GetCurrentCapitalProperty();
			base.ItemProperties.Add(currentCapitalProperty);
			base.ItemProperties.Add(new SelectableItemPropertyVM(new TextObject("{=CaRbMaZY}Daily Wage", null).ToString(), this.Workshop.Expense.ToString(), false, null));
			TextObject textObject;
			TextObject textObject2;
			ClanFinanceWorkshopItemVM.GetWorkshopTypeProductionTexts(this.Workshop.WorkshopType, out textObject, out textObject2);
			this.InputProducts = textObject.ToString();
			this.OutputProducts = textObject2.ToString();
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x0006B2A0 File Offset: 0x000694A0
		private SelectableItemPropertyVM GetCurrentCapitalProperty()
		{
			string text3 = new TextObject("{=Ra17aK4e}Current Capital", null).ToString();
			string text2 = this.Workshop.Capital.ToString();
			bool flag = false;
			BasicTooltipViewModel basicTooltipViewModel;
			if (this.Workshop.Capital < this._workshopModel.CapitalLowLimit)
			{
				flag = true;
				basicTooltipViewModel = new BasicTooltipViewModel(() => new TextObject("{=Qu5clctb}The workshop is losing money. The expenses are being paid from your treasury because the workshop's capital is below {LOWER_THRESHOLD} denars", null).SetTextVariable("LOWER_THRESHOLD", this._workshopModel.CapitalLowLimit).ToString());
			}
			else
			{
				TextObject text = new TextObject("{=dEMUqz2Y}This workshop will send 20% of its profits above {INITIAL_CAPITAL} capital to your treasury", null);
				text.SetTextVariable("INITIAL_CAPITAL", Campaign.Current.Models.WorkshopModel.InitialCapital);
				basicTooltipViewModel = new BasicTooltipViewModel(() => text.ToString());
			}
			return new SelectableItemPropertyVM(text3, text2, flag, basicTooltipViewModel);
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x0006B358 File Offset: 0x00069558
		[return: TupleElementNames(new string[] { "Status", "IsWarning", "Hint" })]
		private ValueTuple<TextObject, bool, BasicTooltipViewModel> GetWorkshopStatus(Workshop workshop)
		{
			bool flag = false;
			BasicTooltipViewModel basicTooltipViewModel = null;
			TextObject textObject;
			if (workshop.LastRunCampaignTime.ElapsedDaysUntilNow >= 1f)
			{
				textObject = this._haltedText;
				flag = true;
				TextObject tooltipText = TextObject.GetEmpty();
				if (workshop.Settlement.Town.InRebelliousState)
				{
					tooltipText = this._townRebellionText;
				}
				else if (!this._workshopWarehouseBehavior.IsRawMaterialsSufficientInTownMarket(workshop))
				{
					tooltipText = this._noRawMaterialsText;
				}
				else if (this.WarehousePercentageSelector.SelectedItem.Percentage < 1f)
				{
					tooltipText = this._noProfitText;
				}
				int num = (int)workshop.LastRunCampaignTime.ElapsedDaysUntilNow;
				tooltipText.SetTextVariable("DAY", num);
				tooltipText.SetTextVariable("PLURAL_DAYS", (num == 1) ? "0" : "1");
				basicTooltipViewModel = new BasicTooltipViewModel(() => tooltipText.ToString());
			}
			else
			{
				textObject = this._runningText;
			}
			return new ValueTuple<TextObject, bool, BasicTooltipViewModel>(textObject, flag, basicTooltipViewModel);
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x0006B46C File Offset: 0x0006966C
		private static void GetWorkshopTypeProductionTexts(WorkshopType workshopType, out TextObject inputsText, out TextObject outputsText)
		{
			CampaignUIHelper.ProductInputOutputEqualityComparer productInputOutputEqualityComparer = new CampaignUIHelper.ProductInputOutputEqualityComparer();
			IEnumerable<TextObject> enumerable = from x in workshopType.Productions.SelectMany<WorkshopType.Production, ValueTuple<ItemCategory, int>>((WorkshopType.Production p) => p.Inputs).Distinct<ValueTuple<ItemCategory, int>>(productInputOutputEqualityComparer)
				select x.Item1.GetName();
			IEnumerable<TextObject> enumerable2 = from x in workshopType.Productions.SelectMany<WorkshopType.Production, ValueTuple<ItemCategory, int>>((WorkshopType.Production p) => p.Outputs).Distinct<ValueTuple<ItemCategory, int>>(productInputOutputEqualityComparer)
				select x.Item1.GetName();
			inputsText = CampaignUIHelper.GetCommaSeparatedText(null, enumerable);
			outputsText = CampaignUIHelper.GetCommaSeparatedText(null, enumerable2);
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x0006B53B File Offset: 0x0006973B
		public void ExecuteBeginWorkshopHint()
		{
			if (this.Workshop.WorkshopType != null)
			{
				InformationManager.ShowTooltip(typeof(Workshop), new object[] { this.Workshop });
			}
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x0006B568 File Offset: 0x00069768
		public void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x0006B570 File Offset: 0x00069770
		public void OnStoreOutputInWarehousePercentageUpdated(SelectorVM<WorkshopPercentageSelectorItemVM> selector)
		{
			if (selector.SelectedIndex != -1)
			{
				this._workshopWarehouseBehavior.SetStockProductionInWarehouseRatio(this.Workshop, selector.SelectedItem.Percentage);
				this._inputDetails = this._workshopWarehouseBehavior.GetInputDailyChange(this.Workshop);
				this._outputDetails = this._workshopWarehouseBehavior.GetOutputDailyChange(this.Workshop);
			}
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x0006B5D0 File Offset: 0x000697D0
		public void ExecuteManageWorkshop()
		{
			TextObject textObject = new TextObject("{=LxWVtDF0}Manage Workshop", null);
			ClanCardSelectionInfo clanCardSelectionInfo = new ClanCardSelectionInfo(textObject, this.GetManageWorkshopItems(), new Action<List<object>, Action>(this.OnManageWorkshopDone), false, 1, 0);
			Action<ClanCardSelectionInfo> openCardSelectionPopup = this._openCardSelectionPopup;
			if (openCardSelectionPopup == null)
			{
				return;
			}
			openCardSelectionPopup(clanCardSelectionInfo);
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x0006B617 File Offset: 0x00069817
		private IEnumerable<ClanCardSelectionItemInfo> GetManageWorkshopItems()
		{
			int costForNotable = this._workshopModel.GetCostForNotable(this.Workshop);
			TextObject textObject = new TextObject("{=ysireFjT}Sell This Workshop for {GOLD_AMOUNT}{GOLD_ICON}", null);
			textObject.SetTextVariable("GOLD_AMOUNT", costForNotable);
			textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			yield return new ClanCardSelectionItemInfo(textObject, false, null, ClanCardSelectionItemPropertyInfo.CreateActionGoldChangeText(costForNotable));
			foreach (WorkshopType workshopType in WorkshopType.All)
			{
				if (this.Workshop.WorkshopType != workshopType && !workshopType.IsHidden)
				{
					TextObject name = workshopType.Name;
					int convertProductionCost = this._workshopModel.GetConvertProductionCost(workshopType);
					TextObject textObject2 = new TextObject("{=av51ur2M}You need at least {REQUIRED_AMOUNT} denars to change the production type of this workshop.", null);
					textObject2.SetTextVariable("REQUIRED_AMOUNT", convertProductionCost);
					bool flag = convertProductionCost <= Hero.MainHero.Gold;
					yield return new ClanCardSelectionItemInfo(workshopType, name, null, CardSelectionItemSpriteType.Workshop, workshopType.StringId, null, this.GetWorkshopItemProperties(workshopType), !flag, textObject2, ClanCardSelectionItemPropertyInfo.CreateActionGoldChangeText(-convertProductionCost));
				}
			}
			List<WorkshopType>.Enumerator enumerator = default(List<WorkshopType>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001CE7 RID: 7399 RVA: 0x0006B627 File Offset: 0x00069827
		private IEnumerable<ClanCardSelectionItemPropertyInfo> GetWorkshopItemProperties(WorkshopType workshopType)
		{
			Workshop workshop = this.Workshop;
			int? num;
			if (workshop == null)
			{
				num = null;
			}
			else
			{
				Settlement settlement = workshop.Settlement;
				if (settlement == null)
				{
					num = null;
				}
				else
				{
					Town town = settlement.Town;
					if (town == null)
					{
						num = null;
					}
					else
					{
						Workshop[] workshops = town.Workshops;
						num = ((workshops != null) ? new int?(workshops.Count<Workshop>((Workshop x) => ((x != null) ? x.WorkshopType : null) == workshopType)) : null);
					}
				}
			}
			int num2 = num ?? 0;
			TextObject textObject = ((num2 == 0) ? new TextObject("{=gu5xmV0E}No other {WORKSHOP_NAME} in this town.", null) : new TextObject("{=lhIpaGt9}There {?(COUNT > 1)}are{?}is{\\?} {COUNT} more {?(COUNT > 1)}{PLURAL(WORKSHOP_NAME)}{?}{WORKSHOP_NAME}{\\?} in this town.", null));
			textObject.SetTextVariable("WORKSHOP_NAME", workshopType.Name);
			textObject.SetTextVariable("COUNT", num2);
			TextObject inputsText;
			TextObject outputsText;
			ClanFinanceWorkshopItemVM.GetWorkshopTypeProductionTexts(workshopType, out inputsText, out outputsText);
			yield return new ClanCardSelectionItemPropertyInfo(textObject);
			yield return new ClanCardSelectionItemPropertyInfo(ClanCardSelectionItemPropertyInfo.CreateLabeledValueText(new TextObject("{=XCz81XYm}Inputs", null), inputsText));
			yield return new ClanCardSelectionItemPropertyInfo(ClanCardSelectionItemPropertyInfo.CreateLabeledValueText(new TextObject("{=ErnykQEH}Outputs", null), outputsText));
			yield break;
		}

		// Token: 0x06001CE8 RID: 7400 RVA: 0x0006B640 File Offset: 0x00069840
		private void OnManageWorkshopDone(List<object> selectedItems, Action closePopup)
		{
			if (closePopup != null)
			{
				closePopup();
			}
			if (selectedItems.Count == 1)
			{
				WorkshopType workshopType = (WorkshopType)selectedItems[0];
				if (workshopType == null)
				{
					if (this.Workshop.Settlement.Town.Workshops.Count<Workshop>((Workshop x) => x.Owner == Hero.MainHero) == 1)
					{
						bool flag = Hero.MainHero.CurrentSettlement == this.Workshop.Settlement;
						InformationManager.ShowInquiry(new InquiryData(new TextObject("{=HiJTlBgF}Sell Workshop", null).ToString(), flag ? new TextObject("{=s06mScpJ}If you have goods in the warehouse, they will be transferred to your party. Are you sure?", null).ToString() : new TextObject("{=yuxBDKgM}If you have goods in the warehouse, they will be lost! Are you sure?", null).ToString(), true, true, new TextObject("{=aeouhelq}Yes", null).ToString(), new TextObject("{=8OkPHu4f}No", null).ToString(), new Action(this.ExecuteSellWorkshop), null, "", 0f, null, null, null), false, false);
					}
					else
					{
						this.ExecuteSellWorkshop();
					}
				}
				else
				{
					ChangeProductionTypeOfWorkshopAction.Apply(this.Workshop, workshopType, false);
				}
				Action onRefresh = this._onRefresh;
				if (onRefresh == null)
				{
					return;
				}
				onRefresh();
			}
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x0006B774 File Offset: 0x00069974
		private void ExecuteSellWorkshop()
		{
			Hero notableOwnerForWorkshop = Campaign.Current.Models.WorkshopModel.GetNotableOwnerForWorkshop(this.Workshop);
			ChangeOwnerOfWorkshopAction.ApplyByPlayerSelling(this.Workshop, notableOwnerForWorkshop, this.Workshop.WorkshopType);
			Action onRefresh = this._onRefresh;
			if (onRefresh == null)
			{
				return;
			}
			onRefresh();
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06001CEA RID: 7402 RVA: 0x0006B7C3 File Offset: 0x000699C3
		// (set) Token: 0x06001CEB RID: 7403 RVA: 0x0006B7CB File Offset: 0x000699CB
		[DataSourceProperty]
		public HintViewModel UseWarehouseAsInputHint
		{
			get
			{
				return this._useWarehouseAsInputHint;
			}
			set
			{
				if (value != this._useWarehouseAsInputHint)
				{
					this._useWarehouseAsInputHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "UseWarehouseAsInputHint");
				}
			}
		}

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06001CEC RID: 7404 RVA: 0x0006B7E9 File Offset: 0x000699E9
		// (set) Token: 0x06001CED RID: 7405 RVA: 0x0006B7F1 File Offset: 0x000699F1
		[DataSourceProperty]
		public HintViewModel StoreOutputPercentageHint
		{
			get
			{
				return this._storeOutputPercentageHint;
			}
			set
			{
				if (value != this._storeOutputPercentageHint)
				{
					this._storeOutputPercentageHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "StoreOutputPercentageHint");
				}
			}
		}

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x06001CEE RID: 7406 RVA: 0x0006B80F File Offset: 0x00069A0F
		// (set) Token: 0x06001CEF RID: 7407 RVA: 0x0006B817 File Offset: 0x00069A17
		[DataSourceProperty]
		public HintViewModel ManageWorkshopHint
		{
			get
			{
				return this._manageWorkshopHint;
			}
			set
			{
				if (value != this._manageWorkshopHint)
				{
					this._manageWorkshopHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ManageWorkshopHint");
				}
			}
		}

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x06001CF0 RID: 7408 RVA: 0x0006B835 File Offset: 0x00069A35
		// (set) Token: 0x06001CF1 RID: 7409 RVA: 0x0006B83D File Offset: 0x00069A3D
		[DataSourceProperty]
		public BasicTooltipViewModel InputWarehouseCountsTooltip
		{
			get
			{
				return this._inputWarehouseCountsTooltip;
			}
			set
			{
				if (value != this._inputWarehouseCountsTooltip)
				{
					this._inputWarehouseCountsTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "InputWarehouseCountsTooltip");
				}
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x06001CF2 RID: 7410 RVA: 0x0006B85B File Offset: 0x00069A5B
		// (set) Token: 0x06001CF3 RID: 7411 RVA: 0x0006B863 File Offset: 0x00069A63
		[DataSourceProperty]
		public BasicTooltipViewModel OutputWarehouseCountsTooltip
		{
			get
			{
				return this._outputWarehouseCountsTooltip;
			}
			set
			{
				if (value != this._outputWarehouseCountsTooltip)
				{
					this._outputWarehouseCountsTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "OutputWarehouseCountsTooltip");
				}
			}
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06001CF4 RID: 7412 RVA: 0x0006B881 File Offset: 0x00069A81
		// (set) Token: 0x06001CF5 RID: 7413 RVA: 0x0006B889 File Offset: 0x00069A89
		public string WorkshopTypeId
		{
			get
			{
				return this._workshopTypeId;
			}
			set
			{
				if (value != this._workshopTypeId)
				{
					this._workshopTypeId = value;
					base.OnPropertyChangedWithValue<string>(value, "WorkshopTypeId");
				}
			}
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x0006B8AC File Offset: 0x00069AAC
		// (set) Token: 0x06001CF7 RID: 7415 RVA: 0x0006B8B4 File Offset: 0x00069AB4
		public string InputsText
		{
			get
			{
				return this._inputsText;
			}
			set
			{
				if (value != this._inputsText)
				{
					this._inputsText = value;
					base.OnPropertyChangedWithValue<string>(value, "InputsText");
				}
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x0006B8D7 File Offset: 0x00069AD7
		// (set) Token: 0x06001CF9 RID: 7417 RVA: 0x0006B8DF File Offset: 0x00069ADF
		public string OutputsText
		{
			get
			{
				return this._outputsText;
			}
			set
			{
				if (value != this._outputsText)
				{
					this._outputsText = value;
					base.OnPropertyChangedWithValue<string>(value, "OutputsText");
				}
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x06001CFA RID: 7418 RVA: 0x0006B902 File Offset: 0x00069B02
		// (set) Token: 0x06001CFB RID: 7419 RVA: 0x0006B90A File Offset: 0x00069B0A
		public string InputProducts
		{
			get
			{
				return this._inputProducts;
			}
			set
			{
				if (value != this._inputProducts)
				{
					this._inputProducts = value;
					base.OnPropertyChangedWithValue<string>(value, "InputProducts");
				}
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06001CFC RID: 7420 RVA: 0x0006B92D File Offset: 0x00069B2D
		// (set) Token: 0x06001CFD RID: 7421 RVA: 0x0006B935 File Offset: 0x00069B35
		public string OutputProducts
		{
			get
			{
				return this._outputProducts;
			}
			set
			{
				if (value != this._outputProducts)
				{
					this._outputProducts = value;
					base.OnPropertyChangedWithValue<string>(value, "OutputProducts");
				}
			}
		}

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x06001CFE RID: 7422 RVA: 0x0006B958 File Offset: 0x00069B58
		// (set) Token: 0x06001CFF RID: 7423 RVA: 0x0006B960 File Offset: 0x00069B60
		public string UseWarehouseAsInputText
		{
			get
			{
				return this._useWarehouseAsInputText;
			}
			set
			{
				if (value != this._useWarehouseAsInputText)
				{
					this._useWarehouseAsInputText = value;
					base.OnPropertyChangedWithValue<string>(value, "UseWarehouseAsInputText");
				}
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x06001D00 RID: 7424 RVA: 0x0006B983 File Offset: 0x00069B83
		// (set) Token: 0x06001D01 RID: 7425 RVA: 0x0006B98B File Offset: 0x00069B8B
		public string StoreOutputPercentageText
		{
			get
			{
				return this._storeOutputPercentageText;
			}
			set
			{
				if (value != this._storeOutputPercentageText)
				{
					this._storeOutputPercentageText = value;
					base.OnPropertyChangedWithValue<string>(value, "StoreOutputPercentageText");
				}
			}
		}

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x06001D02 RID: 7426 RVA: 0x0006B9AE File Offset: 0x00069BAE
		// (set) Token: 0x06001D03 RID: 7427 RVA: 0x0006B9B6 File Offset: 0x00069BB6
		public string WarehouseCapacityText
		{
			get
			{
				return this._warehouseCapacityText;
			}
			set
			{
				if (value != this._warehouseCapacityText)
				{
					this._warehouseCapacityText = value;
					base.OnPropertyChangedWithValue<string>(value, "WarehouseCapacityText");
				}
			}
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x06001D04 RID: 7428 RVA: 0x0006B9D9 File Offset: 0x00069BD9
		// (set) Token: 0x06001D05 RID: 7429 RVA: 0x0006B9E1 File Offset: 0x00069BE1
		public string WarehouseCapacityValue
		{
			get
			{
				return this._warehouseCapacityValue;
			}
			set
			{
				if (value != this._warehouseCapacityValue)
				{
					this._warehouseCapacityValue = value;
					base.OnPropertyChangedWithValue<string>(value, "WarehouseCapacityValue");
				}
			}
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06001D06 RID: 7430 RVA: 0x0006BA04 File Offset: 0x00069C04
		// (set) Token: 0x06001D07 RID: 7431 RVA: 0x0006BA0C File Offset: 0x00069C0C
		public bool ReceiveInputFromWarehouse
		{
			get
			{
				return this._receiveInputFromWarehouse;
			}
			set
			{
				if (value != this._receiveInputFromWarehouse)
				{
					this._receiveInputFromWarehouse = value;
					base.OnPropertyChangedWithValue(value, "ReceiveInputFromWarehouse");
				}
			}
		}

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x06001D08 RID: 7432 RVA: 0x0006BA2A File Offset: 0x00069C2A
		// (set) Token: 0x06001D09 RID: 7433 RVA: 0x0006BA32 File Offset: 0x00069C32
		public int WarehouseInputAmount
		{
			get
			{
				return this._warehouseInputAmount;
			}
			set
			{
				if (value != this._warehouseInputAmount)
				{
					this._warehouseInputAmount = value;
					base.OnPropertyChangedWithValue(value, "WarehouseInputAmount");
				}
			}
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06001D0A RID: 7434 RVA: 0x0006BA50 File Offset: 0x00069C50
		// (set) Token: 0x06001D0B RID: 7435 RVA: 0x0006BA58 File Offset: 0x00069C58
		public int WarehouseOutputAmount
		{
			get
			{
				return this._warehouseOutputAmount;
			}
			set
			{
				if (value != this._warehouseOutputAmount)
				{
					this._warehouseOutputAmount = value;
					base.OnPropertyChangedWithValue(value, "WarehouseOutputAmount");
				}
			}
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06001D0C RID: 7436 RVA: 0x0006BA76 File Offset: 0x00069C76
		// (set) Token: 0x06001D0D RID: 7437 RVA: 0x0006BA7E File Offset: 0x00069C7E
		public SelectorVM<WorkshopPercentageSelectorItemVM> WarehousePercentageSelector
		{
			get
			{
				return this._warehousePercentageSelector;
			}
			set
			{
				if (value != this._warehousePercentageSelector)
				{
					this._warehousePercentageSelector = value;
					base.OnPropertyChangedWithValue<SelectorVM<WorkshopPercentageSelectorItemVM>>(value, "WarehousePercentageSelector");
				}
			}
		}

		// Token: 0x04000D71 RID: 3441
		private readonly TextObject _runningText = new TextObject("{=iuKvbKJ7}Running", null);

		// Token: 0x04000D72 RID: 3442
		private readonly TextObject _haltedText = new TextObject("{=zgnEagTJ}Halted", null);

		// Token: 0x04000D73 RID: 3443
		private readonly TextObject _noRawMaterialsText = new TextObject("{=JRKC4ed4}This workshop has not been producing for {DAY} {?PLURAL_DAYS}days{?}day{\\?} due to lack of raw materials in the town market.", null);

		// Token: 0x04000D74 RID: 3444
		private readonly TextObject _noProfitText = new TextObject("{=no0chrAH}This workshop has not been running for {DAY} {?PLURAL_DAYS}days{?}day{\\?} because the production has not been profitable", null);

		// Token: 0x04000D75 RID: 3445
		private readonly TextObject _townRebellionText = new TextObject("{=pDAuV918}This workshop has not been producing for {DAY} {?PLURAL_DAYS}days{?}day{\\?} due to rebel activity in the town.", null);

		// Token: 0x04000D76 RID: 3446
		private readonly IWorkshopWarehouseCampaignBehavior _workshopWarehouseBehavior;

		// Token: 0x04000D77 RID: 3447
		private readonly WorkshopModel _workshopModel;

		// Token: 0x04000D78 RID: 3448
		private readonly Action<ClanCardSelectionInfo> _openCardSelectionPopup;

		// Token: 0x04000D79 RID: 3449
		private readonly Action<ClanFinanceWorkshopItemVM> _onSelectionT;

		// Token: 0x04000D7A RID: 3450
		private ExplainedNumber _inputDetails;

		// Token: 0x04000D7B RID: 3451
		private ExplainedNumber _outputDetails;

		// Token: 0x04000D7C RID: 3452
		private HintViewModel _useWarehouseAsInputHint;

		// Token: 0x04000D7D RID: 3453
		private HintViewModel _storeOutputPercentageHint;

		// Token: 0x04000D7E RID: 3454
		private HintViewModel _manageWorkshopHint;

		// Token: 0x04000D7F RID: 3455
		private BasicTooltipViewModel _inputWarehouseCountsTooltip;

		// Token: 0x04000D80 RID: 3456
		private BasicTooltipViewModel _outputWarehouseCountsTooltip;

		// Token: 0x04000D81 RID: 3457
		private string _workshopTypeId;

		// Token: 0x04000D82 RID: 3458
		private string _inputsText;

		// Token: 0x04000D83 RID: 3459
		private string _outputsText;

		// Token: 0x04000D84 RID: 3460
		private string _inputProducts;

		// Token: 0x04000D85 RID: 3461
		private string _outputProducts;

		// Token: 0x04000D86 RID: 3462
		private string _useWarehouseAsInputText;

		// Token: 0x04000D87 RID: 3463
		private string _storeOutputPercentageText;

		// Token: 0x04000D88 RID: 3464
		private string _warehouseCapacityText;

		// Token: 0x04000D89 RID: 3465
		private string _warehouseCapacityValue;

		// Token: 0x04000D8A RID: 3466
		private bool _receiveInputFromWarehouse;

		// Token: 0x04000D8B RID: 3467
		private int _warehouseInputAmount;

		// Token: 0x04000D8C RID: 3468
		private int _warehouseOutputAmount;

		// Token: 0x04000D8D RID: 3469
		private SelectorVM<WorkshopPercentageSelectorItemVM> _warehousePercentageSelector;
	}
}
