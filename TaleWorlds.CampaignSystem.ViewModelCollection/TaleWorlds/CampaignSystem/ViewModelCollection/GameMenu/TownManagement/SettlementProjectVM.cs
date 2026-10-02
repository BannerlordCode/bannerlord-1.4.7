using System;
using Helpers;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000A7 RID: 167
	public abstract class SettlementProjectVM : ViewModel
	{
		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x0600101B RID: 4123 RVA: 0x00042443 File Offset: 0x00040643
		// (set) Token: 0x0600101C RID: 4124 RVA: 0x0004244B File Offset: 0x0004064B
		public bool IsDaily { get; protected set; }

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x0600101D RID: 4125 RVA: 0x00042454 File Offset: 0x00040654
		// (set) Token: 0x0600101E RID: 4126 RVA: 0x0004245C File Offset: 0x0004065C
		public Building Building
		{
			get
			{
				return this._building;
			}
			set
			{
				this._building = value;
				this.Name = ((value != null) ? value.Name.ToString() : "");
				this.Explanation = ((value != null) ? value.Explanation.ToString() : "");
				this.VisualCode = ((value != null) ? value.BuildingType.StringId.ToLower() : "");
				int constructionCost = this.Building.GetConstructionCost();
				TextObject textObject;
				if (constructionCost > 0)
				{
					textObject = new TextObject("{=tAwRIPiy}Construction Cost: {COST}", null);
					textObject.SetTextVariable("COST", constructionCost);
				}
				else
				{
					textObject = TextObject.GetEmpty();
				}
				this.ProductionCostText = ((value != null) ? textObject.ToString() : "");
				this.CurrentPositiveEffectText = ((value != null) ? value.GetBonusExplanation().ToString() : "");
			}
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x00042528 File Offset: 0x00040728
		protected SettlementProjectVM(Action<SettlementProjectVM, bool> onSelection, Action<SettlementProjectVM> onSetAsCurrent, Action onResetCurrent, Building building, Settlement settlement)
		{
			this._onSelection = onSelection;
			this._onSetAsCurrent = onSetAsCurrent;
			this._onResetCurrent = onResetCurrent;
			this.Building = building;
			this._settlement = settlement;
			this.Progress = (int)(BuildingHelper.GetProgressOfBuilding(building, this._settlement.Town) * 100f);
			this.RefreshValues();
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x000425B8 File Offset: 0x000407B8
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.Building.BuildingType.IsDailyProject)
			{
				this.CurrentPositiveEffectText = this.Building.BuildingType.GetExplanationAtLevel(this.Building.CurrentLevel).ToString();
				this.NextPositiveEffectText = "";
				return;
			}
			this.CurrentPositiveEffectText = this.GetBonusText(this.Building, this.Building.CurrentLevel);
			this.NextPositiveEffectText = this.GetBonusText(this.Building, this.Building.CurrentLevel + 1);
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0004264C File Offset: 0x0004084C
		private string GetBonusText(Building building, int level)
		{
			if (level == 0 || level == 4)
			{
				return "";
			}
			object obj = ((level == 1) ? this.L1BonusText : ((level == 2) ? this.L2BonusText : this.L3BonusText));
			TextObject bonusExplanationOfLevel = this.GetBonusExplanationOfLevel(level);
			object obj2 = obj;
			obj2.SetTextVariable("BONUS", bonusExplanationOfLevel);
			return obj2.ToString();
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0004269E File Offset: 0x0004089E
		private void ExecuteShowTooltip()
		{
			InformationManager.ShowTooltip(typeof(Building), new object[] { this._building });
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x000426BE File Offset: 0x000408BE
		private void ExecuteHideTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x000426C5 File Offset: 0x000408C5
		private TextObject GetBonusExplanationOfLevel(int level)
		{
			if (level >= 0 && level <= 3)
			{
				return this.Building.BuildingType.GetExplanationAtLevel(level);
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x000426E6 File Offset: 0x000408E6
		public virtual void RefreshProductionText()
		{
		}

		// Token: 0x06001026 RID: 4134
		public abstract void ExecuteAddRemoveToQueue();

		// Token: 0x06001027 RID: 4135
		public abstract void ExecuteSetAsActiveDevelopment();

		// Token: 0x06001028 RID: 4136
		public abstract void ExecuteSetAsCurrent();

		// Token: 0x06001029 RID: 4137
		public abstract void ExecuteResetCurrent();

		// Token: 0x0600102A RID: 4138
		public abstract void ExecuteToggleSelected();

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x0600102B RID: 4139 RVA: 0x000426E8 File Offset: 0x000408E8
		// (set) Token: 0x0600102C RID: 4140 RVA: 0x000426F0 File Offset: 0x000408F0
		[DataSourceProperty]
		public string VisualCode
		{
			get
			{
				return this._visualCode;
			}
			set
			{
				if (value != this._visualCode)
				{
					this._visualCode = value;
					base.OnPropertyChangedWithValue<string>(value, "VisualCode");
				}
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x0600102D RID: 4141 RVA: 0x00042713 File Offset: 0x00040913
		// (set) Token: 0x0600102E RID: 4142 RVA: 0x0004271B File Offset: 0x0004091B
		[DataSourceProperty]
		public string ProductionText
		{
			get
			{
				return this._productionText;
			}
			set
			{
				if (value != this._productionText)
				{
					this._productionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProductionText");
				}
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x0600102F RID: 4143 RVA: 0x0004273E File Offset: 0x0004093E
		// (set) Token: 0x06001030 RID: 4144 RVA: 0x00042746 File Offset: 0x00040946
		[DataSourceProperty]
		public string CurrentPositiveEffectText
		{
			get
			{
				return this._currentPositiveEffectText;
			}
			set
			{
				if (value != this._currentPositiveEffectText)
				{
					this._currentPositiveEffectText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentPositiveEffectText");
				}
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001031 RID: 4145 RVA: 0x00042769 File Offset: 0x00040969
		// (set) Token: 0x06001032 RID: 4146 RVA: 0x00042771 File Offset: 0x00040971
		[DataSourceProperty]
		public string NextPositiveEffectText
		{
			get
			{
				return this._nextPositiveEffectText;
			}
			set
			{
				if (value != this._nextPositiveEffectText)
				{
					this._nextPositiveEffectText = value;
					base.OnPropertyChangedWithValue<string>(value, "NextPositiveEffectText");
				}
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001033 RID: 4147 RVA: 0x00042794 File Offset: 0x00040994
		// (set) Token: 0x06001034 RID: 4148 RVA: 0x0004279C File Offset: 0x0004099C
		[DataSourceProperty]
		public string ProductionCostText
		{
			get
			{
				return this._productionCostText;
			}
			set
			{
				if (value != this._productionCostText)
				{
					this._productionCostText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProductionCostText");
				}
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x000427BF File Offset: 0x000409BF
		// (set) Token: 0x06001036 RID: 4150 RVA: 0x000427C7 File Offset: 0x000409C7
		[DataSourceProperty]
		public bool IsCurrentActiveProject
		{
			get
			{
				return this._isCurrentActiveProject;
			}
			set
			{
				if (value != this._isCurrentActiveProject)
				{
					this._isCurrentActiveProject = value;
					base.OnPropertyChangedWithValue(value, "IsCurrentActiveProject");
				}
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001037 RID: 4151 RVA: 0x000427E5 File Offset: 0x000409E5
		// (set) Token: 0x06001038 RID: 4152 RVA: 0x000427ED File Offset: 0x000409ED
		[DataSourceProperty]
		public int Progress
		{
			get
			{
				return this._progress;
			}
			set
			{
				if (value != this._progress)
				{
					this._progress = value;
					base.OnPropertyChangedWithValue(value, "Progress");
				}
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001039 RID: 4153 RVA: 0x0004280B File Offset: 0x00040A0B
		// (set) Token: 0x0600103A RID: 4154 RVA: 0x00042813 File Offset: 0x00040A13
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

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x0600103B RID: 4155 RVA: 0x00042836 File Offset: 0x00040A36
		// (set) Token: 0x0600103C RID: 4156 RVA: 0x0004283E File Offset: 0x00040A3E
		[DataSourceProperty]
		public string Explanation
		{
			get
			{
				return this._explanation;
			}
			set
			{
				if (value != this._explanation)
				{
					this._explanation = value;
					base.OnPropertyChangedWithValue<string>(value, "Explanation");
				}
			}
		}

		// Token: 0x04000759 RID: 1881
		public int Index;

		// Token: 0x0400075B RID: 1883
		private Building _building;

		// Token: 0x0400075C RID: 1884
		protected Action<SettlementProjectVM, bool> _onSelection;

		// Token: 0x0400075D RID: 1885
		protected Action<SettlementProjectVM> _onSetAsCurrent;

		// Token: 0x0400075E RID: 1886
		protected Action _onResetCurrent;

		// Token: 0x0400075F RID: 1887
		protected Settlement _settlement;

		// Token: 0x04000760 RID: 1888
		private readonly TextObject L1BonusText = new TextObject("{=PJZ8QYgA}L-I : {BONUS}", null);

		// Token: 0x04000761 RID: 1889
		private readonly TextObject L2BonusText = new TextObject("{=9i0wnjJK}L-II : {BONUS}", null);

		// Token: 0x04000762 RID: 1890
		private readonly TextObject L3BonusText = new TextObject("{=pRP2sOWP}L-III : {BONUS}", null);

		// Token: 0x04000763 RID: 1891
		private string _name;

		// Token: 0x04000764 RID: 1892
		private string _visualCode;

		// Token: 0x04000765 RID: 1893
		private string _explanation;

		// Token: 0x04000766 RID: 1894
		private string _currentPositiveEffectText;

		// Token: 0x04000767 RID: 1895
		private string _nextPositiveEffectText;

		// Token: 0x04000768 RID: 1896
		private string _productionCostText;

		// Token: 0x04000769 RID: 1897
		private int _progress;

		// Token: 0x0400076A RID: 1898
		private bool _isCurrentActiveProject;

		// Token: 0x0400076B RID: 1899
		private string _productionText;
	}
}
