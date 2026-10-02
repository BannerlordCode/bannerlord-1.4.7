using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting
{
	// Token: 0x020000FC RID: 252
	public class CraftingResourceItemVM : ViewModel
	{
		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x060016B5 RID: 5813 RVA: 0x0005823F File Offset: 0x0005643F
		// (set) Token: 0x060016B6 RID: 5814 RVA: 0x00058247 File Offset: 0x00056447
		public ItemObject ResourceItem { get; private set; }

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x060016B7 RID: 5815 RVA: 0x00058250 File Offset: 0x00056450
		// (set) Token: 0x060016B8 RID: 5816 RVA: 0x00058258 File Offset: 0x00056458
		public CraftingMaterials ResourceMaterial { get; private set; }

		// Token: 0x060016B9 RID: 5817 RVA: 0x00058264 File Offset: 0x00056464
		public CraftingResourceItemVM(CraftingMaterials material, int amount, int changeAmount = 0)
		{
			this.ResourceMaterial = material;
			Campaign campaign = Campaign.Current;
			ItemObject itemObject;
			if (campaign == null)
			{
				itemObject = null;
			}
			else
			{
				GameModels models = campaign.Models;
				if (models == null)
				{
					itemObject = null;
				}
				else
				{
					SmithingModel smithingModel = models.SmithingModel;
					itemObject = ((smithingModel != null) ? smithingModel.GetCraftingMaterialItem(material) : null);
				}
			}
			this.ResourceItem = itemObject;
			ItemObject resourceItem = this.ResourceItem;
			string text;
			if (resourceItem == null)
			{
				text = null;
			}
			else
			{
				TextObject name = resourceItem.Name;
				text = ((name != null) ? name.ToString() : null);
			}
			this.ResourceName = text ?? "none";
			this.ResourceHint = new HintViewModel(new TextObject("{=!}" + this.ResourceName, null), null);
			this.ResourceAmount = amount;
			ItemObject resourceItem2 = this.ResourceItem;
			this.ResourceItemStringId = ((resourceItem2 != null) ? resourceItem2.StringId : null) ?? "none";
			this.ResourceMaterialTypeAsStr = this.ResourceMaterial.ToString();
			this.ResourceChangeAmount = changeAmount;
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x060016BA RID: 5818 RVA: 0x0005834D File Offset: 0x0005654D
		// (set) Token: 0x060016BB RID: 5819 RVA: 0x00058355 File Offset: 0x00056555
		[DataSourceProperty]
		public string ResourceName
		{
			get
			{
				return this._resourceName;
			}
			set
			{
				if (value != this._resourceName)
				{
					this._resourceName = value;
					base.OnPropertyChangedWithValue<string>(value, "ResourceName");
				}
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x060016BC RID: 5820 RVA: 0x00058378 File Offset: 0x00056578
		// (set) Token: 0x060016BD RID: 5821 RVA: 0x00058380 File Offset: 0x00056580
		[DataSourceProperty]
		public HintViewModel ResourceHint
		{
			get
			{
				return this._resourceHint;
			}
			set
			{
				if (value != this._resourceHint)
				{
					this._resourceHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ResourceHint");
				}
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x060016BE RID: 5822 RVA: 0x0005839E File Offset: 0x0005659E
		// (set) Token: 0x060016BF RID: 5823 RVA: 0x000583A6 File Offset: 0x000565A6
		[DataSourceProperty]
		public string ResourceMaterialTypeAsStr
		{
			get
			{
				return this._resourceMaterialTypeAsStr;
			}
			set
			{
				if (value != this._resourceMaterialTypeAsStr)
				{
					this._resourceMaterialTypeAsStr = value;
					base.OnPropertyChangedWithValue<string>(value, "ResourceMaterialTypeAsStr");
				}
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x060016C0 RID: 5824 RVA: 0x000583C9 File Offset: 0x000565C9
		// (set) Token: 0x060016C1 RID: 5825 RVA: 0x000583D1 File Offset: 0x000565D1
		[DataSourceProperty]
		public int ResourceAmount
		{
			get
			{
				return this._resourceUsageAmount;
			}
			set
			{
				if (value != this._resourceUsageAmount)
				{
					this._resourceUsageAmount = value;
					base.OnPropertyChangedWithValue(value, "ResourceAmount");
				}
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x060016C2 RID: 5826 RVA: 0x000583EF File Offset: 0x000565EF
		// (set) Token: 0x060016C3 RID: 5827 RVA: 0x000583F7 File Offset: 0x000565F7
		[DataSourceProperty]
		public int ResourceChangeAmount
		{
			get
			{
				return this._resourceChangeAmount;
			}
			set
			{
				if (value != this._resourceChangeAmount)
				{
					this._resourceChangeAmount = value;
					base.OnPropertyChangedWithValue(value, "ResourceChangeAmount");
				}
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x060016C4 RID: 5828 RVA: 0x00058415 File Offset: 0x00056615
		// (set) Token: 0x060016C5 RID: 5829 RVA: 0x0005841D File Offset: 0x0005661D
		[DataSourceProperty]
		public string ResourceItemStringId
		{
			get
			{
				return this._resourceItemStringId;
			}
			set
			{
				if (value != this._resourceItemStringId)
				{
					this._resourceItemStringId = value;
					base.OnPropertyChangedWithValue<string>(value, "ResourceItemStringId");
				}
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x060016C6 RID: 5830 RVA: 0x00058440 File Offset: 0x00056640
		// (set) Token: 0x060016C7 RID: 5831 RVA: 0x00058448 File Offset: 0x00056648
		[DataSourceProperty]
		public bool IsResourceAvailable
		{
			get
			{
				return this._isResourceAvailable;
			}
			set
			{
				if (value != this._isResourceAvailable)
				{
					this._isResourceAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsResourceAvailable");
				}
			}
		}

		// Token: 0x04000A64 RID: 2660
		private string _resourceName;

		// Token: 0x04000A65 RID: 2661
		private string _resourceItemStringId;

		// Token: 0x04000A66 RID: 2662
		private int _resourceUsageAmount;

		// Token: 0x04000A67 RID: 2663
		private int _resourceChangeAmount;

		// Token: 0x04000A68 RID: 2664
		private string _resourceMaterialTypeAsStr;

		// Token: 0x04000A69 RID: 2665
		private HintViewModel _resourceHint;

		// Token: 0x04000A6A RID: 2666
		private bool _isResourceAvailable = true;
	}
}
