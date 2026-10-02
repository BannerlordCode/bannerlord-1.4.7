using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.Refinement
{
	// Token: 0x02000114 RID: 276
	public class RefinementActionItemVM : ViewModel
	{
		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06001959 RID: 6489 RVA: 0x000606C0 File Offset: 0x0005E8C0
		public Crafting.RefiningFormula RefineFormula { get; }

		// Token: 0x0600195A RID: 6490 RVA: 0x000606C8 File Offset: 0x0005E8C8
		public RefinementActionItemVM(Crafting.RefiningFormula refineFormula, Action<RefinementActionItemVM> onSelect)
		{
			this._onSelect = onSelect;
			this.RefineFormula = refineFormula;
			this.InputMaterials = new MBBindingList<CraftingResourceItemVM>();
			this.OutputMaterials = new MBBindingList<CraftingResourceItemVM>();
			SmithingModel smithingModel = Campaign.Current.Models.SmithingModel;
			if (this.RefineFormula.Input1Count > 0)
			{
				this.InputMaterials.Add(new CraftingResourceItemVM(this.RefineFormula.Input1, this.RefineFormula.Input1Count, 0));
			}
			if (this.RefineFormula.Input2Count > 0)
			{
				this.InputMaterials.Add(new CraftingResourceItemVM(this.RefineFormula.Input2, this.RefineFormula.Input2Count, 0));
			}
			if (this.RefineFormula.OutputCount > 0)
			{
				this.OutputMaterials.Add(new CraftingResourceItemVM(this.RefineFormula.Output, this.RefineFormula.OutputCount, 0));
			}
			if (this.RefineFormula.Output2Count > 0)
			{
				this.OutputMaterials.Add(new CraftingResourceItemVM(this.RefineFormula.Output2, this.RefineFormula.Output2Count, 0));
			}
			this.RefreshDynamicProperties();
			this.RefreshValues();
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x000607F0 File Offset: 0x0005E9F0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.InputMaterials.ApplyActionOnAllItems(delegate(CraftingResourceItemVM m)
			{
				m.RefreshValues();
			});
			this.OutputMaterials.ApplyActionOnAllItems(delegate(CraftingResourceItemVM m)
			{
				m.RefreshValues();
			});
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x00060857 File Offset: 0x0005EA57
		public void RefreshDynamicProperties()
		{
			this.IsEnabled = this.UpdateInputAvailabilities();
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x00060868 File Offset: 0x0005EA68
		private bool UpdateInputAvailabilities()
		{
			bool flag = true;
			ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
			foreach (CraftingResourceItemVM craftingResourceItemVM in this.InputMaterials)
			{
				if (itemRoster.GetItemNumber(craftingResourceItemVM.ResourceItem) < craftingResourceItemVM.ResourceAmount)
				{
					flag = false;
					craftingResourceItemVM.IsResourceAvailable = false;
				}
				else
				{
					craftingResourceItemVM.IsResourceAvailable = true;
				}
			}
			return flag;
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x000608E4 File Offset: 0x0005EAE4
		public void ExecuteSelectAction()
		{
			this._onSelect(this);
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x0600195F RID: 6495 RVA: 0x000608F2 File Offset: 0x0005EAF2
		// (set) Token: 0x06001960 RID: 6496 RVA: 0x000608FA File Offset: 0x0005EAFA
		[DataSourceProperty]
		public MBBindingList<CraftingResourceItemVM> InputMaterials
		{
			get
			{
				return this._inputMaterials;
			}
			set
			{
				if (value != this._inputMaterials)
				{
					this._inputMaterials = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingResourceItemVM>>(value, "InputMaterials");
				}
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06001961 RID: 6497 RVA: 0x00060918 File Offset: 0x0005EB18
		// (set) Token: 0x06001962 RID: 6498 RVA: 0x00060920 File Offset: 0x0005EB20
		[DataSourceProperty]
		public MBBindingList<CraftingResourceItemVM> OutputMaterials
		{
			get
			{
				return this._outputMaterials;
			}
			set
			{
				if (value != this._outputMaterials)
				{
					this._outputMaterials = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingResourceItemVM>>(value, "OutputMaterials");
				}
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06001963 RID: 6499 RVA: 0x0006093E File Offset: 0x0005EB3E
		// (set) Token: 0x06001964 RID: 6500 RVA: 0x00060946 File Offset: 0x0005EB46
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

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06001965 RID: 6501 RVA: 0x00060964 File Offset: 0x0005EB64
		// (set) Token: 0x06001966 RID: 6502 RVA: 0x0006096C File Offset: 0x0005EB6C
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

		// Token: 0x04000BA6 RID: 2982
		private readonly Action<RefinementActionItemVM> _onSelect;

		// Token: 0x04000BA8 RID: 2984
		private MBBindingList<CraftingResourceItemVM> _inputMaterials;

		// Token: 0x04000BA9 RID: 2985
		private MBBindingList<CraftingResourceItemVM> _outputMaterials;

		// Token: 0x04000BAA RID: 2986
		private bool _isSelected;

		// Token: 0x04000BAB RID: 2987
		private bool _isEnabled;
	}
}
