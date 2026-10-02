using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x02000092 RID: 146
	public class ItemPreviewVM : ViewModel
	{
		// Token: 0x06000CCC RID: 3276 RVA: 0x000364F1 File Offset: 0x000346F1
		public ItemPreviewVM(Action onClosed)
		{
			this._onClosed = onClosed;
			this.ItemTableau = new ItemCollectionElementViewModel();
			this.RefreshValues();
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x00036511 File Offset: 0x00034711
		public override void OnFinalize()
		{
			this.ItemTableau.OnFinalize();
			this.ItemTableau = null;
			base.OnFinalize();
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x0003652B File Offset: 0x0003472B
		public void Open(EquipmentElement item)
		{
			this.ItemTableau.FillFrom(item, Clan.PlayerClan.Banner);
			this.ItemName = item.Item.Name.ToString();
			this.IsSelected = true;
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x00036561 File Offset: 0x00034761
		public void ExecuteClose()
		{
			this.Close();
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x00036569 File Offset: 0x00034769
		public void Close()
		{
			this._onClosed();
			this.IsSelected = false;
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x0003657D File Offset: 0x0003477D
		// (set) Token: 0x06000CD2 RID: 3282 RVA: 0x00036585 File Offset: 0x00034785
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
					base.OnPropertyChanged("IsSelected");
				}
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x000365A2 File Offset: 0x000347A2
		// (set) Token: 0x06000CD4 RID: 3284 RVA: 0x000365AA File Offset: 0x000347AA
		[DataSourceProperty]
		public string ItemName
		{
			get
			{
				return this._itemName;
			}
			set
			{
				if (value != this._itemName)
				{
					this._itemName = value;
					base.OnPropertyChanged("ItemName");
				}
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x000365CC File Offset: 0x000347CC
		// (set) Token: 0x06000CD6 RID: 3286 RVA: 0x000365D4 File Offset: 0x000347D4
		[DataSourceProperty]
		public ItemCollectionElementViewModel ItemTableau
		{
			get
			{
				return this._itemTableau;
			}
			set
			{
				if (value != this._itemTableau)
				{
					this._itemTableau = value;
					base.OnPropertyChangedWithValue<ItemCollectionElementViewModel>(value, "ItemTableau");
				}
			}
		}

		// Token: 0x040005D8 RID: 1496
		private Action _onClosed;

		// Token: 0x040005D9 RID: 1497
		private bool _isSelected;

		// Token: 0x040005DA RID: 1498
		private string _itemName;

		// Token: 0x040005DB RID: 1499
		private ItemCollectionElementViewModel _itemTableau;
	}
}
