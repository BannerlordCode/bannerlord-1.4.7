using System;
using Helpers;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000A9 RID: 169
	public class TownManagementReserveControlVM : ViewModel
	{
		// Token: 0x0600104C RID: 4172 RVA: 0x00042A2C File Offset: 0x00040C2C
		public TownManagementReserveControlVM(Settlement settlement, Action onReserveUpdated)
		{
			this._settlement = settlement;
			this._onReserveUpdated = onReserveUpdated;
			if (((settlement != null) ? settlement.Town : null) != null)
			{
				this.CurrentReserveAmount = Settlement.CurrentSettlement.Town.BoostBuildingProcess;
				this.CurrentGivenAmount = 0;
				this.MaxReserveAmount = MathF.Min(Hero.MainHero.Gold, 10000);
				this.AddGoldToReserveHint = new HintViewModel(new TextObject("{=TeZ3QzN6}Add gold to the reserve", null), null);
			}
			this.RefreshValues();
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x00042AB0 File Offset: 0x00040CB0
		public override void RefreshValues()
		{
			base.RefreshValues();
			Settlement settlement = this._settlement;
			if (((settlement != null) ? settlement.Town : null) != null)
			{
				this.ReserveText = new TextObject("{=2ckyCKR7}Reserve", null).ToString();
				GameTexts.SetVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
				this.UpdateReserveText();
			}
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x00042B04 File Offset: 0x00040D04
		private void UpdateReserveText()
		{
			TextObject textObject = GameTexts.FindText("str_town_management_reserve_explanation", null);
			textObject.SetTextVariable("BOOST", Campaign.Current.Models.BuildingConstructionModel.GetBoostAmount(this._settlement.Town));
			textObject.SetTextVariable("COST", Campaign.Current.Models.BuildingConstructionModel.GetBoostCost(this._settlement.Town));
			this.ReserveBonusText = textObject.ToString();
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x00042B80 File Offset: 0x00040D80
		public void ExecuteConfirm()
		{
			this.IsEnabled = false;
			BuildingHelper.BoostBuildingProcessWithGold(this.CurrentReserveAmount + this.CurrentGivenAmount, Settlement.CurrentSettlement.Town);
			this.CurrentGivenAmount = 0;
			GameTexts.SetVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			this.UpdateReserveText();
			this.MaxReserveAmount = MathF.Min(Hero.MainHero.Gold, 10000);
			this.CurrentReserveAmount = Settlement.CurrentSettlement.Town.BoostBuildingProcess;
			Action onReserveUpdated = this._onReserveUpdated;
			if (onReserveUpdated == null)
			{
				return;
			}
			onReserveUpdated();
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x00042C0B File Offset: 0x00040E0B
		public void ExecuteCancel()
		{
			this.IsEnabled = false;
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001051 RID: 4177 RVA: 0x00042C14 File Offset: 0x00040E14
		// (set) Token: 0x06001052 RID: 4178 RVA: 0x00042C1C File Offset: 0x00040E1C
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001053 RID: 4179 RVA: 0x00042C3A File Offset: 0x00040E3A
		// (set) Token: 0x06001054 RID: 4180 RVA: 0x00042C44 File Offset: 0x00040E44
		[DataSourceProperty]
		public int CurrentReserveAmount
		{
			get
			{
				return this._currentReserveAmount;
			}
			set
			{
				if (value != this._currentReserveAmount)
				{
					this._currentReserveAmount = value;
					base.OnPropertyChangedWithValue(value, "CurrentReserveAmount");
					this.CurrentReserveText = (this.CurrentGivenAmount + value).ToString();
				}
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001055 RID: 4181 RVA: 0x00042C83 File Offset: 0x00040E83
		// (set) Token: 0x06001056 RID: 4182 RVA: 0x00042C8B File Offset: 0x00040E8B
		[DataSourceProperty]
		public int CurrentGivenAmount
		{
			get
			{
				return this._currentGivenAmount;
			}
			set
			{
				if (value != this._currentGivenAmount)
				{
					this._currentGivenAmount = value;
					base.OnPropertyChangedWithValue(value, "CurrentGivenAmount");
				}
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001057 RID: 4183 RVA: 0x00042CA9 File Offset: 0x00040EA9
		// (set) Token: 0x06001058 RID: 4184 RVA: 0x00042CB1 File Offset: 0x00040EB1
		[DataSourceProperty]
		public int MaxReserveAmount
		{
			get
			{
				return this._maxReserveAmount;
			}
			set
			{
				if (value != this._maxReserveAmount)
				{
					this._maxReserveAmount = value;
					base.OnPropertyChangedWithValue(value, "MaxReserveAmount");
				}
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001059 RID: 4185 RVA: 0x00042CCF File Offset: 0x00040ECF
		// (set) Token: 0x0600105A RID: 4186 RVA: 0x00042CD7 File Offset: 0x00040ED7
		[DataSourceProperty]
		public string ReserveBonusText
		{
			get
			{
				return this._reserveBonusText;
			}
			set
			{
				if (value != this._reserveBonusText)
				{
					this._reserveBonusText = value;
					base.OnPropertyChangedWithValue<string>(value, "ReserveBonusText");
				}
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x0600105B RID: 4187 RVA: 0x00042CFA File Offset: 0x00040EFA
		// (set) Token: 0x0600105C RID: 4188 RVA: 0x00042D02 File Offset: 0x00040F02
		[DataSourceProperty]
		public string ReserveText
		{
			get
			{
				return this._reserveText;
			}
			set
			{
				if (value != this._reserveText)
				{
					this._reserveText = value;
					base.OnPropertyChangedWithValue<string>(value, "ReserveText");
				}
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x00042D25 File Offset: 0x00040F25
		// (set) Token: 0x0600105E RID: 4190 RVA: 0x00042D2D File Offset: 0x00040F2D
		[DataSourceProperty]
		public string CurrentReserveText
		{
			get
			{
				return this._currentReserveText;
			}
			set
			{
				if (value != this._currentReserveText)
				{
					this._currentReserveText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentReserveText");
				}
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x00042D50 File Offset: 0x00040F50
		// (set) Token: 0x06001060 RID: 4192 RVA: 0x00042D58 File Offset: 0x00040F58
		[DataSourceProperty]
		public HintViewModel AddGoldToReserveHint
		{
			get
			{
				return this._addGoldToReserveHint;
			}
			set
			{
				if (value != this._addGoldToReserveHint)
				{
					this._addGoldToReserveHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AddGoldToReserveHint");
				}
			}
		}

		// Token: 0x04000773 RID: 1907
		private readonly Action _onReserveUpdated;

		// Token: 0x04000774 RID: 1908
		private readonly Settlement _settlement;

		// Token: 0x04000775 RID: 1909
		private const int MaxOneTimeAmount = 10000;

		// Token: 0x04000776 RID: 1910
		private bool _isEnabled;

		// Token: 0x04000777 RID: 1911
		private string _reserveText;

		// Token: 0x04000778 RID: 1912
		private int _currentReserveAmount;

		// Token: 0x04000779 RID: 1913
		private int _currentGivenAmount;

		// Token: 0x0400077A RID: 1914
		private int _maxReserveAmount;

		// Token: 0x0400077B RID: 1915
		private string _reserveBonusText;

		// Token: 0x0400077C RID: 1916
		private string _currentReserveText;

		// Token: 0x0400077D RID: 1917
		private HintViewModel _addGoldToReserveHint;
	}
}
