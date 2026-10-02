using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem
{
	// Token: 0x0200007D RID: 125
	public class MPArmoryCosmeticClothingItemVM : MPArmoryCosmeticItemBaseVM
	{
		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06000C7B RID: 3195 RVA: 0x00026E84 File Offset: 0x00025084
		public EquipmentElement EquipmentElement { get; }

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06000C7C RID: 3196 RVA: 0x00026E8C File Offset: 0x0002508C
		public MPArmoryCosmeticsVM.ClothingCategory ClothingCategory { get; }

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x00026E94 File Offset: 0x00025094
		public ClothingCosmeticElement ClothingCosmeticElement { get; }

		// Token: 0x06000C7E RID: 3198 RVA: 0x00026E9C File Offset: 0x0002509C
		public MPArmoryCosmeticClothingItemVM(CosmeticElement cosmetic, string cosmeticID)
			: base(cosmetic, cosmeticID, CosmeticsManager.CosmeticType.Clothing)
		{
			ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>(cosmetic.Id);
			this.EquipmentElement = new EquipmentElement(@object, null, null, false);
			base.Icon = new ItemImageIdentifierVM(@object, "");
			this.ClothingCategory = this.GetCosmeticCategory();
			this.ClothingCosmeticElement = cosmetic as ClothingCosmeticElement;
			base.ItemType = (int)this.EquipmentElement.Item.ItemType;
			this.RefreshValues();
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00026F1C File Offset: 0x0002511C
		public override void RefreshValues()
		{
			base.RefreshValues();
			ItemObject item = this.EquipmentElement.Item;
			base.Name = ((item != null) ? item.Name.ToString() : null);
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x00026F54 File Offset: 0x00025154
		private MPArmoryCosmeticsVM.ClothingCategory GetCosmeticCategory()
		{
			ItemObject.ItemTypeEnum type = this.EquipmentElement.Item.Type;
			switch (type)
			{
			case ItemObject.ItemTypeEnum.HeadArmor:
				return MPArmoryCosmeticsVM.ClothingCategory.HeadArmor;
			case ItemObject.ItemTypeEnum.BodyArmor:
				return MPArmoryCosmeticsVM.ClothingCategory.BodyArmor;
			case ItemObject.ItemTypeEnum.LegArmor:
				return MPArmoryCosmeticsVM.ClothingCategory.LegArmor;
			case ItemObject.ItemTypeEnum.HandArmor:
				return MPArmoryCosmeticsVM.ClothingCategory.HandArmor;
			default:
				if (type != ItemObject.ItemTypeEnum.Cape)
				{
					return MPArmoryCosmeticsVM.ClothingCategory.Invalid;
				}
				return MPArmoryCosmeticsVM.ClothingCategory.Cape;
			}
		}
	}
}
