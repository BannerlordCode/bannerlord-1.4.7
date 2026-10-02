using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu
{
	// Token: 0x0200009D RID: 157
	public class GameMenuPlunderItemVM : ViewModel
	{
		// Token: 0x06000F44 RID: 3908 RVA: 0x0003F48F File Offset: 0x0003D68F
		public GameMenuPlunderItemVM(EquipmentElement item, int amount = 1)
		{
			this.Item = item;
			this.Amount = amount;
			this.Visual = new ItemImageIdentifierVM(item.Item, "");
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0003F4BC File Offset: 0x0003D6BC
		public void ExecuteBeginTooltip()
		{
			if (this.Item.Item != null)
			{
				InformationManager.ShowTooltip(typeof(ItemObject), new object[] { this.Item });
			}
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x0003F4FC File Offset: 0x0003D6FC
		public void ExecuteEndTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06000F47 RID: 3911 RVA: 0x0003F503 File Offset: 0x0003D703
		// (set) Token: 0x06000F48 RID: 3912 RVA: 0x0003F50B File Offset: 0x0003D70B
		[DataSourceProperty]
		public ItemImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<ItemImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06000F49 RID: 3913 RVA: 0x0003F529 File Offset: 0x0003D729
		// (set) Token: 0x06000F4A RID: 3914 RVA: 0x0003F531 File Offset: 0x0003D731
		[DataSourceProperty]
		public int Amount
		{
			get
			{
				return this._amount;
			}
			set
			{
				if (value != this._amount)
				{
					this._amount = value;
					base.OnPropertyChangedWithValue(value, "Amount");
				}
			}
		}

		// Token: 0x040006EC RID: 1772
		public readonly EquipmentElement Item;

		// Token: 0x040006ED RID: 1773
		private ItemImageIdentifierVM _visual;

		// Token: 0x040006EE RID: 1774
		private int _amount;
	}
}
