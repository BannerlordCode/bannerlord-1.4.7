using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Barter
{
	// Token: 0x02000159 RID: 345
	public class BarterItemVM : EncyclopediaLinkVM
	{
		// Token: 0x06002084 RID: 8324 RVA: 0x00076940 File Offset: 0x00074B40
		public BarterItemVM(Barterable barterable, BarterItemVM.BarterTransferEventDelegate OnTransfer, Action onAmountChange, bool isFixed = false)
		{
			this.Barterable = barterable;
			base.ActiveLink = barterable.GetEncyclopediaLink();
			this._onTransfer = OnTransfer;
			this._onAmountChange = onAmountChange;
			this._isFixed = isFixed;
			this.IsItemTransferrable = !isFixed;
			this.BarterableType = this.Barterable.StringID;
			ImageIdentifier visualIdentifier = this.Barterable.GetVisualIdentifier();
			this.HasVisualIdentifier = visualIdentifier != null;
			if (visualIdentifier != null)
			{
				this.VisualIdentifier = new GenericImageIdentifierVM(visualIdentifier);
			}
			else
			{
				this.VisualIdentifier = null;
				FiefBarterable fiefBarterable;
				if ((fiefBarterable = this.Barterable as FiefBarterable) != null)
				{
					this.FiefFileName = fiefBarterable.TargetSettlement.SettlementComponent.BackgroundMeshName;
				}
			}
			this.TotalItemCount = this.Barterable.MaxAmount;
			this.CurrentOfferedAmount = 1;
			this.IsMultiple = this.TotalItemCount > 1;
			this.IsOffered = this.Barterable.IsOffered;
			this.RefreshValues();
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x00076A46 File Offset: 0x00074C46
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ItemLbl = this.Barterable.Name.ToString();
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x00076A64 File Offset: 0x00074C64
		public void RefreshCompabilityWithItem(BarterItemVM item, bool isItemGotOffered)
		{
			if (isItemGotOffered && !item.Barterable.IsCompatible(this.Barterable))
			{
				this._incompatibleItems.Add(item.Barterable);
			}
			else if (!isItemGotOffered && this._incompatibleItems.Contains(item.Barterable))
			{
				this._incompatibleItems.Remove(item.Barterable);
			}
			this.IsItemTransferrable = this._incompatibleItems.Count <= 0;
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x00076ADC File Offset: 0x00074CDC
		public void ExecuteAddOffered()
		{
			int num = (BarterItemVM.IsEntireStackModifierActive ? this.TotalItemCount : (this.CurrentOfferedAmount + (BarterItemVM.IsFiveStackModifierActive ? 5 : 1)));
			this.CurrentOfferedAmount = ((num < this.TotalItemCount) ? num : this.TotalItemCount);
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x00076B24 File Offset: 0x00074D24
		public void ExecuteRemoveOffered()
		{
			int num = (BarterItemVM.IsEntireStackModifierActive ? 1 : (this.CurrentOfferedAmount - (BarterItemVM.IsFiveStackModifierActive ? 5 : 1)));
			this.CurrentOfferedAmount = ((num > 1) ? num : 1);
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x00076B5C File Offset: 0x00074D5C
		public void ExecuteAction()
		{
			if (this.IsItemTransferrable)
			{
				this._onTransfer(this, false);
			}
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x0600208A RID: 8330 RVA: 0x00076B73 File Offset: 0x00074D73
		// (set) Token: 0x0600208B RID: 8331 RVA: 0x00076B7B File Offset: 0x00074D7B
		[DataSourceProperty]
		public int TotalItemCount
		{
			get
			{
				return this._totalItemCount;
			}
			set
			{
				if (this._totalItemCount != value)
				{
					this._totalItemCount = value;
					base.OnPropertyChangedWithValue(value, "TotalItemCount");
					this.TotalItemCountText = CampaignUIHelper.GetAbbreviatedValueTextFromValue(value);
				}
			}
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x0600208C RID: 8332 RVA: 0x00076BA5 File Offset: 0x00074DA5
		// (set) Token: 0x0600208D RID: 8333 RVA: 0x00076BAD File Offset: 0x00074DAD
		[DataSourceProperty]
		public string TotalItemCountText
		{
			get
			{
				return this._totalItemCountText;
			}
			set
			{
				if (this._totalItemCountText != value)
				{
					this._totalItemCountText = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalItemCountText");
				}
			}
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x0600208E RID: 8334 RVA: 0x00076BD0 File Offset: 0x00074DD0
		// (set) Token: 0x0600208F RID: 8335 RVA: 0x00076BD8 File Offset: 0x00074DD8
		[DataSourceProperty]
		public int CurrentOfferedAmount
		{
			get
			{
				return this._currentOfferedAmount;
			}
			set
			{
				if (this._currentOfferedAmount != value)
				{
					this.Barterable.CurrentAmount = value;
					Action onAmountChange = this._onAmountChange;
					if (onAmountChange != null)
					{
						onAmountChange();
					}
					this._currentOfferedAmount = value;
					base.OnPropertyChangedWithValue(value, "CurrentOfferedAmount");
					this.CurrentOfferedAmountText = CampaignUIHelper.GetAbbreviatedValueTextFromValue(value);
				}
			}
		}

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06002090 RID: 8336 RVA: 0x00076C2A File Offset: 0x00074E2A
		// (set) Token: 0x06002091 RID: 8337 RVA: 0x00076C32 File Offset: 0x00074E32
		[DataSourceProperty]
		public string CurrentOfferedAmountText
		{
			get
			{
				return this._currentOfferedAmountText;
			}
			set
			{
				if (this._currentOfferedAmountText != value)
				{
					this._currentOfferedAmountText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentOfferedAmountText");
				}
			}
		}

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06002092 RID: 8338 RVA: 0x00076C55 File Offset: 0x00074E55
		// (set) Token: 0x06002093 RID: 8339 RVA: 0x00076C5D File Offset: 0x00074E5D
		[DataSourceProperty]
		public string BarterableType
		{
			get
			{
				return this._barterableType;
			}
			set
			{
				if (this._barterableType != value)
				{
					this._barterableType = value;
					base.OnPropertyChangedWithValue<string>(value, "BarterableType");
				}
			}
		}

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x06002094 RID: 8340 RVA: 0x00076C80 File Offset: 0x00074E80
		// (set) Token: 0x06002095 RID: 8341 RVA: 0x00076C88 File Offset: 0x00074E88
		[DataSourceProperty]
		public bool HasVisualIdentifier
		{
			get
			{
				return this._hasVisualIdentifier;
			}
			set
			{
				if (this._hasVisualIdentifier != value)
				{
					this._hasVisualIdentifier = value;
					base.OnPropertyChangedWithValue(value, "HasVisualIdentifier");
				}
			}
		}

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x06002096 RID: 8342 RVA: 0x00076CA6 File Offset: 0x00074EA6
		// (set) Token: 0x06002097 RID: 8343 RVA: 0x00076CAE File Offset: 0x00074EAE
		[DataSourceProperty]
		public bool IsMultiple
		{
			get
			{
				return this._isMultiple;
			}
			set
			{
				if (this._isMultiple != value)
				{
					this._isMultiple = value;
					base.OnPropertyChangedWithValue(value, "IsMultiple");
				}
			}
		}

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x06002098 RID: 8344 RVA: 0x00076CCC File Offset: 0x00074ECC
		// (set) Token: 0x06002099 RID: 8345 RVA: 0x00076CD4 File Offset: 0x00074ED4
		[DataSourceProperty]
		public bool IsSelectorActive
		{
			get
			{
				return this._isSelectorActive;
			}
			set
			{
				if (this._isSelectorActive != value)
				{
					this._isSelectorActive = value;
					base.OnPropertyChangedWithValue(value, "IsSelectorActive");
				}
			}
		}

		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x0600209A RID: 8346 RVA: 0x00076CF2 File Offset: 0x00074EF2
		// (set) Token: 0x0600209B RID: 8347 RVA: 0x00076CFA File Offset: 0x00074EFA
		[DataSourceProperty]
		public ImageIdentifierVM VisualIdentifier
		{
			get
			{
				return this._visualIdentifier;
			}
			set
			{
				if (this._visualIdentifier != value)
				{
					this._visualIdentifier = value;
					base.OnPropertyChangedWithValue<ImageIdentifierVM>(value, "VisualIdentifier");
				}
			}
		}

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x0600209C RID: 8348 RVA: 0x00076D18 File Offset: 0x00074F18
		// (set) Token: 0x0600209D RID: 8349 RVA: 0x00076D20 File Offset: 0x00074F20
		[DataSourceProperty]
		public string ItemLbl
		{
			get
			{
				return this._itemLbl;
			}
			set
			{
				this._itemLbl = value;
				base.OnPropertyChangedWithValue<string>(value, "ItemLbl");
			}
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x0600209E RID: 8350 RVA: 0x00076D35 File Offset: 0x00074F35
		// (set) Token: 0x0600209F RID: 8351 RVA: 0x00076D3D File Offset: 0x00074F3D
		[DataSourceProperty]
		public string FiefFileName
		{
			get
			{
				return this._fiefFileName;
			}
			set
			{
				this._fiefFileName = value;
				base.OnPropertyChangedWithValue<string>(value, "FiefFileName");
			}
		}

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x060020A0 RID: 8352 RVA: 0x00076D52 File Offset: 0x00074F52
		// (set) Token: 0x060020A1 RID: 8353 RVA: 0x00076D5A File Offset: 0x00074F5A
		[DataSourceProperty]
		public bool IsItemTransferrable
		{
			get
			{
				return this._isItemTransferrable;
			}
			set
			{
				if (this._isFixed)
				{
					value = false;
				}
				if (this._isItemTransferrable != value)
				{
					this._isItemTransferrable = value;
					base.OnPropertyChangedWithValue(value, "IsItemTransferrable");
				}
			}
		}

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x060020A2 RID: 8354 RVA: 0x00076D83 File Offset: 0x00074F83
		// (set) Token: 0x060020A3 RID: 8355 RVA: 0x00076D8B File Offset: 0x00074F8B
		[DataSourceProperty]
		public bool IsOffered
		{
			get
			{
				return this._isOffered;
			}
			set
			{
				if (value != this._isOffered)
				{
					this._isOffered = value;
					base.OnPropertyChangedWithValue(value, "IsOffered");
				}
			}
		}

		// Token: 0x04000F1C RID: 3868
		public static bool IsEntireStackModifierActive;

		// Token: 0x04000F1D RID: 3869
		public static bool IsFiveStackModifierActive;

		// Token: 0x04000F1E RID: 3870
		private readonly BarterItemVM.BarterTransferEventDelegate _onTransfer;

		// Token: 0x04000F1F RID: 3871
		private readonly Action _onAmountChange;

		// Token: 0x04000F20 RID: 3872
		private bool _isFixed;

		// Token: 0x04000F21 RID: 3873
		private List<Barterable> _incompatibleItems = new List<Barterable>();

		// Token: 0x04000F22 RID: 3874
		public Barterable Barterable;

		// Token: 0x04000F23 RID: 3875
		public bool _isOffered;

		// Token: 0x04000F24 RID: 3876
		private bool _isItemTransferrable = true;

		// Token: 0x04000F25 RID: 3877
		private string _itemLbl;

		// Token: 0x04000F26 RID: 3878
		private string _fiefFileName;

		// Token: 0x04000F27 RID: 3879
		private string _barterableType = "NULL";

		// Token: 0x04000F28 RID: 3880
		private string _currentOfferedAmountText;

		// Token: 0x04000F29 RID: 3881
		private ImageIdentifierVM _visualIdentifier;

		// Token: 0x04000F2A RID: 3882
		private bool _isSelectorActive;

		// Token: 0x04000F2B RID: 3883
		private bool _hasVisualIdentifier;

		// Token: 0x04000F2C RID: 3884
		private bool _isMultiple;

		// Token: 0x04000F2D RID: 3885
		private int _totalItemCount;

		// Token: 0x04000F2E RID: 3886
		private string _totalItemCountText;

		// Token: 0x04000F2F RID: 3887
		private int _currentOfferedAmount;

		// Token: 0x020002E5 RID: 741
		// (Invoke) Token: 0x0600273A RID: 10042
		public delegate void BarterTransferEventDelegate(BarterItemVM itemVM, bool transferAll);
	}
}
