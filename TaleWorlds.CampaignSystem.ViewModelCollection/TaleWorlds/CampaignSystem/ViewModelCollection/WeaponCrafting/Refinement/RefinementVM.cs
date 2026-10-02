using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.Refinement
{
	// Token: 0x02000115 RID: 277
	public class RefinementVM : ViewModel
	{
		// Token: 0x06001967 RID: 6503 RVA: 0x0006098C File Offset: 0x0005EB8C
		public RefinementVM(Action onRefinementSelectionChange, Func<CraftingAvailableHeroItemVM> getCurrentHero)
		{
			this._onRefinementSelectionChange = onRefinementSelectionChange;
			this._craftingBehavior = Campaign.Current.GetCampaignBehavior<ICraftingCampaignBehavior>();
			this._getCurrentHero = getCurrentHero;
			this.AvailableRefinementActions = new MBBindingList<RefinementActionItemVM>();
			this.SetupRefinementActionsList(this._getCurrentHero().Hero);
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x000609DE File Offset: 0x0005EBDE
		private void SetupRefinementActionsList(Hero craftingHero)
		{
			this.UpdateRefinementFormulas(craftingHero);
			this.RefreshRefinementActionsList(craftingHero);
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x000609EE File Offset: 0x0005EBEE
		internal void OnCraftingHeroChanged(CraftingAvailableHeroItemVM newHero)
		{
			this.SetupRefinementActionsList(this._getCurrentHero().Hero);
			this.SelectDefaultAction();
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x00060A0C File Offset: 0x0005EC0C
		private void UpdateRefinementFormulas(Hero hero)
		{
			this.AvailableRefinementActions.Clear();
			foreach (Crafting.RefiningFormula refiningFormula in Campaign.Current.Models.SmithingModel.GetRefiningFormulas(hero))
			{
				this.AvailableRefinementActions.Add(new RefinementActionItemVM(refiningFormula, new Action<RefinementActionItemVM>(this.OnSelectAction)));
			}
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x00060A8C File Offset: 0x0005EC8C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RefinementText = new TextObject("{=p7raHA9x}Refinement", null).ToString();
			this.AvailableRefinementActions.ApplyActionOnAllItems(delegate(RefinementActionItemVM x)
			{
				x.RefreshValues();
			});
			RefinementActionItemVM currentSelectedAction = this.CurrentSelectedAction;
			if (currentSelectedAction == null)
			{
				return;
			}
			currentSelectedAction.RefreshValues();
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x00060AF0 File Offset: 0x0005ECF0
		public void ExecuteSelectedRefinement(Hero currentCraftingHero)
		{
			if (this.CurrentSelectedAction != null)
			{
				ICraftingCampaignBehavior craftingBehavior = this._craftingBehavior;
				if (craftingBehavior != null)
				{
					craftingBehavior.DoRefinement(currentCraftingHero, this.CurrentSelectedAction.RefineFormula);
				}
				this.RefreshRefinementActionsList(currentCraftingHero);
				if (!this.CurrentSelectedAction.IsEnabled)
				{
					this.OnSelectAction(null);
				}
			}
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x00060B40 File Offset: 0x0005ED40
		public void RefreshRefinementActionsList(Hero craftingHero)
		{
			foreach (RefinementActionItemVM refinementActionItemVM in this.AvailableRefinementActions)
			{
				refinementActionItemVM.RefreshDynamicProperties();
			}
			if (this.CurrentSelectedAction == null)
			{
				this.SelectDefaultAction();
			}
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x00060B98 File Offset: 0x0005ED98
		private void SelectDefaultAction()
		{
			RefinementActionItemVM refinementActionItemVM = this.AvailableRefinementActions.FirstOrDefault<RefinementActionItemVM>((RefinementActionItemVM a) => a.IsEnabled);
			if (refinementActionItemVM != null)
			{
				this.OnSelectAction(refinementActionItemVM);
			}
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x00060BDA File Offset: 0x0005EDDA
		private void OnSelectAction(RefinementActionItemVM selectedAction)
		{
			if (this.CurrentSelectedAction != null)
			{
				this.CurrentSelectedAction.IsSelected = false;
			}
			this.CurrentSelectedAction = selectedAction;
			this._onRefinementSelectionChange();
			if (this.CurrentSelectedAction != null)
			{
				this.CurrentSelectedAction.IsSelected = true;
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06001970 RID: 6512 RVA: 0x00060C16 File Offset: 0x0005EE16
		// (set) Token: 0x06001971 RID: 6513 RVA: 0x00060C1E File Offset: 0x0005EE1E
		[DataSourceProperty]
		public RefinementActionItemVM CurrentSelectedAction
		{
			get
			{
				return this._currentSelectedAction;
			}
			set
			{
				if (value != this._currentSelectedAction)
				{
					this._currentSelectedAction = value;
					base.OnPropertyChangedWithValue<RefinementActionItemVM>(value, "CurrentSelectedAction");
					this.IsValidRefinementActionSelected = value != null;
				}
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06001972 RID: 6514 RVA: 0x00060C46 File Offset: 0x0005EE46
		// (set) Token: 0x06001973 RID: 6515 RVA: 0x00060C4E File Offset: 0x0005EE4E
		[DataSourceProperty]
		public bool IsValidRefinementActionSelected
		{
			get
			{
				return this._isValidRefinementActionSelected;
			}
			set
			{
				if (value != this._isValidRefinementActionSelected)
				{
					this._isValidRefinementActionSelected = value;
					base.OnPropertyChangedWithValue(value, "IsValidRefinementActionSelected");
				}
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06001974 RID: 6516 RVA: 0x00060C6C File Offset: 0x0005EE6C
		// (set) Token: 0x06001975 RID: 6517 RVA: 0x00060C74 File Offset: 0x0005EE74
		[DataSourceProperty]
		public MBBindingList<RefinementActionItemVM> AvailableRefinementActions
		{
			get
			{
				return this._availableRefinementActions;
			}
			set
			{
				if (value != this._availableRefinementActions)
				{
					this._availableRefinementActions = value;
					base.OnPropertyChangedWithValue<MBBindingList<RefinementActionItemVM>>(value, "AvailableRefinementActions");
				}
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06001976 RID: 6518 RVA: 0x00060C92 File Offset: 0x0005EE92
		// (set) Token: 0x06001977 RID: 6519 RVA: 0x00060C9A File Offset: 0x0005EE9A
		[DataSourceProperty]
		public string RefinementText
		{
			get
			{
				return this._refinementText;
			}
			set
			{
				if (value != this._refinementText)
				{
					this._refinementText = value;
					base.OnPropertyChangedWithValue<string>(value, "RefinementText");
				}
			}
		}

		// Token: 0x04000BAC RID: 2988
		private readonly Action _onRefinementSelectionChange;

		// Token: 0x04000BAD RID: 2989
		private readonly ICraftingCampaignBehavior _craftingBehavior;

		// Token: 0x04000BAE RID: 2990
		private readonly Func<CraftingAvailableHeroItemVM> _getCurrentHero;

		// Token: 0x04000BAF RID: 2991
		private RefinementActionItemVM _currentSelectedAction;

		// Token: 0x04000BB0 RID: 2992
		private bool _isValidRefinementActionSelected;

		// Token: 0x04000BB1 RID: 2993
		private MBBindingList<RefinementActionItemVM> _availableRefinementActions;

		// Token: 0x04000BB2 RID: 2994
		private string _refinementText;
	}
}
