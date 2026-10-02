using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanFinance
{
	// Token: 0x02000135 RID: 309
	public class ClanFinanceTownItemVM : ClanFinanceIncomeItemBaseVM
	{
		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x06001CC5 RID: 7365 RVA: 0x0006A8DA File Offset: 0x00068ADA
		// (set) Token: 0x06001CC6 RID: 7366 RVA: 0x0006A8E2 File Offset: 0x00068AE2
		public Settlement Settlement { get; private set; }

		// Token: 0x06001CC7 RID: 7367 RVA: 0x0006A8EC File Offset: 0x00068AEC
		public ClanFinanceTownItemVM(Settlement settlement, TaxType taxType, Action<ClanFinanceIncomeItemBaseVM> onSelection, Action onRefresh)
			: base(onSelection, onRefresh)
		{
			base.IncomeTypeAsEnum = IncomeTypes.Settlement;
			this.Settlement = settlement;
			MBTextManager.SetTextVariable("SETTLEMENT_NAME", settlement.Name.ToString(), false);
			base.Name = ((taxType == TaxType.ProsperityTax) ? GameTexts.FindText("str_prosperity_tax", null).ToString() : GameTexts.FindText("str_trade_tax", null).ToString());
			this.IsUnderSiege = settlement.IsUnderSiege;
			this.IsUnderSiegeHint = new HintViewModel(new TextObject("{=!}PLACEHOLDER | THIS SETTLEMENT IS UNDER SIEGE", null), null);
			this.IsUnderRebellion = settlement.IsUnderRebellionAttack();
			this.IsUnderRebellionHint = new HintViewModel(new TextObject("{=!}PLACEHOLDER | THIS SETTLEMENT IS UNDER REBELLION", null), null);
			if (taxType == TaxType.ProsperityTax && settlement.Town != null)
			{
				float resultNumber = Campaign.Current.Models.SettlementTaxModel.CalculateTownTax(settlement.Town, false).ResultNumber;
				base.Income = (this.IsUnderRebellion ? 0 : ((int)resultNumber));
			}
			else if (taxType == TaxType.TradeTax)
			{
				if (settlement.Town != null)
				{
					base.Income = (int)((float)settlement.Town.TradeTaxAccumulated / Campaign.Current.Models.ClanFinanceModel.RevenueSmoothenFraction());
				}
				else if (settlement.Village != null)
				{
					base.Income = ((settlement.Village.VillageState == Village.VillageStates.Looted || settlement.Village.VillageState == Village.VillageStates.BeingRaided) ? 0 : ((int)((float)settlement.Village.TradeTaxAccumulated / Campaign.Current.Models.ClanFinanceModel.RevenueSmoothenFraction())));
				}
			}
			base.IncomeValueText = base.DetermineIncomeText(base.Income);
			this.HasGovernor = settlement.IsTown && settlement.Town.Governor != null;
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x0006AA97 File Offset: 0x00068C97
		protected override void PopulateActionList()
		{
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x0006AA99 File Offset: 0x00068C99
		protected override void PopulateStatsList()
		{
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06001CCA RID: 7370 RVA: 0x0006AA9B File Offset: 0x00068C9B
		// (set) Token: 0x06001CCB RID: 7371 RVA: 0x0006AAA3 File Offset: 0x00068CA3
		[DataSourceProperty]
		public bool IsUnderSiege
		{
			get
			{
				return this._isUnderSiege;
			}
			set
			{
				if (value != this._isUnderSiege)
				{
					this._isUnderSiege = value;
					base.OnPropertyChangedWithValue(value, "IsUnderSiege");
				}
			}
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06001CCC RID: 7372 RVA: 0x0006AAC1 File Offset: 0x00068CC1
		// (set) Token: 0x06001CCD RID: 7373 RVA: 0x0006AAC9 File Offset: 0x00068CC9
		[DataSourceProperty]
		public bool IsUnderRebellion
		{
			get
			{
				return this._isUnderRebellion;
			}
			set
			{
				if (value != this._isUnderRebellion)
				{
					this._isUnderRebellion = value;
					base.OnPropertyChangedWithValue(value, "IsUnderRebellion");
				}
			}
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06001CCE RID: 7374 RVA: 0x0006AAE7 File Offset: 0x00068CE7
		// (set) Token: 0x06001CCF RID: 7375 RVA: 0x0006AAEF File Offset: 0x00068CEF
		[DataSourceProperty]
		public HintViewModel IsUnderSiegeHint
		{
			get
			{
				return this._isUnderSiegeHint;
			}
			set
			{
				if (value != this._isUnderSiegeHint)
				{
					this._isUnderSiegeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "IsUnderSiegeHint");
				}
			}
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06001CD0 RID: 7376 RVA: 0x0006AB0D File Offset: 0x00068D0D
		// (set) Token: 0x06001CD1 RID: 7377 RVA: 0x0006AB15 File Offset: 0x00068D15
		[DataSourceProperty]
		public HintViewModel IsUnderRebellionHint
		{
			get
			{
				return this._isUnderRebellionHint;
			}
			set
			{
				if (value != this._isUnderRebellionHint)
				{
					this._isUnderRebellionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "IsUnderRebellionHint");
				}
			}
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x06001CD2 RID: 7378 RVA: 0x0006AB33 File Offset: 0x00068D33
		// (set) Token: 0x06001CD3 RID: 7379 RVA: 0x0006AB3B File Offset: 0x00068D3B
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

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06001CD4 RID: 7380 RVA: 0x0006AB59 File Offset: 0x00068D59
		// (set) Token: 0x06001CD5 RID: 7381 RVA: 0x0006AB61 File Offset: 0x00068D61
		[DataSourceProperty]
		public HintViewModel GovernorHint
		{
			get
			{
				return this._governorHint;
			}
			set
			{
				if (value != this._governorHint)
				{
					this._governorHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "GovernorHint");
				}
			}
		}

		// Token: 0x04000D6A RID: 3434
		private bool _isUnderSiege;

		// Token: 0x04000D6B RID: 3435
		private bool _isUnderRebellion;

		// Token: 0x04000D6C RID: 3436
		private HintViewModel _isUnderSiegeHint;

		// Token: 0x04000D6D RID: 3437
		private HintViewModel _isUnderRebellionHint;

		// Token: 0x04000D6E RID: 3438
		private HintViewModel _governorHint;

		// Token: 0x04000D6F RID: 3439
		private bool _hasGovernor;
	}
}
