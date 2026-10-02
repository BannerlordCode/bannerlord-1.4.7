using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000121 RID: 289
	public class ItemData
	{
		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x00008304 File Offset: 0x00006504
		// (set) Token: 0x06000673 RID: 1651 RVA: 0x0000830C File Offset: 0x0000650C
		public string TypeId { get; set; }

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x00008315 File Offset: 0x00006515
		// (set) Token: 0x06000675 RID: 1653 RVA: 0x0000831D File Offset: 0x0000651D
		public string ModifierId { get; set; }

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x00008326 File Offset: 0x00006526
		// (set) Token: 0x06000677 RID: 1655 RVA: 0x0000832E File Offset: 0x0000652E
		public int? Index { get; set; }

		// Token: 0x06000678 RID: 1656 RVA: 0x00008337 File Offset: 0x00006537
		public void CopyItemData(ItemData itemdata)
		{
			this.TypeId = itemdata.TypeId;
			this.ModifierId = itemdata.ModifierId;
			this.Index = itemdata.Index;
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x0000835D File Offset: 0x0000655D
		private ItemType ItemType
		{
			get
			{
				return ItemList.GetItemTypeOf(this.TypeId);
			}
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0000836C File Offset: 0x0000656C
		private static int GetInventoryItemTypeOfItem(ItemType itemType)
		{
			switch (itemType)
			{
			case ItemType.Horse:
				return 64;
			case ItemType.OneHandedWeapon:
				return 1;
			case ItemType.TwoHandedWeapon:
				return 1;
			case ItemType.Polearm:
				return 1;
			case ItemType.Arrows:
				return 1;
			case ItemType.Bolts:
				return 1;
			case ItemType.Shield:
				return 2;
			case ItemType.Bow:
				return 1;
			case ItemType.Crossbow:
				return 1;
			case ItemType.Thrown:
				return 1;
			case ItemType.Goods:
				return 256;
			case ItemType.HeadArmor:
				return 4;
			case ItemType.BodyArmor:
				return 8;
			case ItemType.LegArmor:
				return 16;
			case ItemType.HandArmor:
				return 32;
			case ItemType.Pistol:
				return 1;
			case ItemType.Musket:
				return 1;
			case ItemType.Bullets:
				return 1;
			case ItemType.Animal:
				return 1024;
			case ItemType.Book:
				return 512;
			case ItemType.Cape:
				return 2048;
			case ItemType.HorseHarness:
				return 128;
			}
			return 0;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00008423 File Offset: 0x00006623
		public bool CanItemToEquipmentDragPossible(int equipmentIndex)
		{
			return ItemData.CanItemToEquipmentDragPossible(this.TypeId, equipmentIndex);
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00008434 File Offset: 0x00006634
		public static bool CanItemToEquipmentDragPossible(string itemTypeId, int equipmentIndex)
		{
			InventoryItemType inventoryItemTypeOfItem = (InventoryItemType)ItemData.GetInventoryItemTypeOfItem(ItemList.GetItemTypeOf(itemTypeId));
			bool flag = false;
			if (equipmentIndex == 0 || equipmentIndex == 1 || equipmentIndex == 2 || equipmentIndex == 3)
			{
				flag = inventoryItemTypeOfItem == InventoryItemType.Weapon || inventoryItemTypeOfItem == InventoryItemType.Shield;
			}
			else if (equipmentIndex == 5)
			{
				flag = inventoryItemTypeOfItem == InventoryItemType.HeadArmor;
			}
			else if (equipmentIndex == 6)
			{
				flag = inventoryItemTypeOfItem == InventoryItemType.BodyArmor;
			}
			else if (equipmentIndex == 7)
			{
				flag = inventoryItemTypeOfItem == InventoryItemType.LegArmor;
			}
			else if (equipmentIndex == 8)
			{
				flag = inventoryItemTypeOfItem == InventoryItemType.HandArmor;
			}
			else if (equipmentIndex == 9)
			{
				flag = inventoryItemTypeOfItem == InventoryItemType.Cape;
			}
			else if (equipmentIndex == 10)
			{
				flag = inventoryItemTypeOfItem == InventoryItemType.Horse;
			}
			else if (equipmentIndex == 11)
			{
				flag = inventoryItemTypeOfItem == InventoryItemType.HorseHarness;
			}
			return flag;
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x000084C8 File Offset: 0x000066C8
		public int Price
		{
			get
			{
				return ItemData.GetPriceOf(this.TypeId, this.ModifierId);
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x000084DB File Offset: 0x000066DB
		public bool IsValid
		{
			get
			{
				return ItemData.IsItemValid(this.TypeId, this.ModifierId);
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x000084EE File Offset: 0x000066EE
		public string ItemKey
		{
			get
			{
				return this.TypeId + "|" + this.ModifierId;
			}
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00008506 File Offset: 0x00006706
		public static int GetPriceOf(string itemId, string modifierId)
		{
			return ItemList.GetPriceOf(itemId, modifierId);
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0000850F File Offset: 0x0000670F
		public static bool IsItemValid(string itemId, string modifierId)
		{
			return ItemList.IsItemValid(itemId, modifierId);
		}
	}
}
