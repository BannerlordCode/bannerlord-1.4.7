using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticCategory
{
	// Token: 0x02000081 RID: 129
	public class MPArmoryClothingCosmeticCategoryVM : MPArmoryCosmeticCategoryBaseVM
	{
		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000CEB RID: 3307 RVA: 0x00027F14 File Offset: 0x00026114
		// (remove) Token: 0x06000CEC RID: 3308 RVA: 0x00027F48 File Offset: 0x00026148
		public static event Action<MPArmoryClothingCosmeticCategoryVM> OnSelected;

		// Token: 0x06000CED RID: 3309 RVA: 0x00027F7B File Offset: 0x0002617B
		public MPArmoryClothingCosmeticCategoryVM(MPArmoryCosmeticsVM.ClothingCategory clothingCategory)
			: base(CosmeticsManager.CosmeticType.Clothing)
		{
			this._defaultCosmeticIDs = new List<string>();
			this.ClothingCategory = clothingCategory;
			base.CosmeticCategoryName = clothingCategory.ToString();
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00027FA9 File Offset: 0x000261A9
		public override void RefreshValues()
		{
			base.RefreshValues();
			base.AvailableCosmetics.ApplyActionOnAllItems(delegate(MPArmoryCosmeticItemBaseVM c)
			{
				c.RefreshValues();
			});
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x00027FDB File Offset: 0x000261DB
		protected override void ExecuteSelectCategory()
		{
			Action<MPArmoryClothingCosmeticCategoryVM> onSelected = MPArmoryClothingCosmeticCategoryVM.OnSelected;
			if (onSelected == null)
			{
				return;
			}
			onSelected(this);
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00027FF0 File Offset: 0x000261F0
		private void AddDefaultItem(ItemObject item)
		{
			MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM = new MPArmoryCosmeticClothingItemVM(new ClothingCosmeticElement(item.StringId, CosmeticsManager.CosmeticRarity.Default, 0, new List<string>(), new List<Tuple<string, string>>()), string.Empty)
			{
				IsUnlocked = true,
				IsUnequippable = false
			};
			ItemObject.ItemTypeEnum itemTypeEnum = this.ClothingCategory.ToItemTypeEnum();
			if (itemTypeEnum == ItemObject.ItemTypeEnum.Invalid || itemTypeEnum == item.ItemType)
			{
				base.AvailableCosmetics.Add(mparmoryCosmeticClothingItemVM);
			}
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00028054 File Offset: 0x00026254
		public void SetDefaultEquipments(Equipment equipment)
		{
			base.AvailableCosmetics.Clear();
			this._defaultCosmeticIDs.Clear();
			if (equipment != null)
			{
				for (EquipmentIndex equipmentIndex = EquipmentIndex.NumAllWeaponSlots; equipmentIndex < EquipmentIndex.ArmorItemEndSlot; equipmentIndex++)
				{
					ItemObject item = equipment[equipmentIndex].Item;
					if (item != null)
					{
						this._defaultCosmeticIDs.Add(equipment[equipmentIndex].Item.StringId);
						this.AddDefaultItem(item);
					}
				}
			}
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x000280C0 File Offset: 0x000262C0
		public void ReplaceCosmeticWithDefaultItem(MPArmoryCosmeticClothingItemVM cosmetic, MPArmoryCosmeticsVM.ClothingCategory clothingCategory, MultiplayerClassDivisions.MPHeroClass selectedClass, List<string> ownedCosmetics)
		{
			bool flag = cosmetic.ClothingCategory == clothingCategory || clothingCategory == MPArmoryCosmeticsVM.ClothingCategory.ClothingCategoriesBegin;
			ClothingCosmeticElement clothingCosmeticElement;
			bool flag2 = (clothingCosmeticElement = cosmetic.Cosmetic as ClothingCosmeticElement) != null && (clothingCosmeticElement.ReplaceItemsId.Any<string>((string c) => this._defaultCosmeticIDs.Contains(c)) || clothingCosmeticElement.ReplaceItemless.Any<Tuple<string, string>>((Tuple<string, string> r) => r.Item1 == selectedClass.StringId)) && !base.AvailableCosmetics.Contains(cosmetic);
			if (flag && flag2)
			{
				base.AvailableCosmetics.Add(cosmetic);
				cosmetic.IsUnlocked = (ownedCosmetics != null && ownedCosmetics.Contains(cosmetic.CosmeticID)) || cosmetic.Cosmetic.IsFree;
			}
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0002817C File Offset: 0x0002637C
		public void OnEquipmentRefreshed(EquipmentIndex equipmentIndex)
		{
			foreach (MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM in base.AvailableCosmetics)
			{
				MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM;
				if ((mparmoryCosmeticClothingItemVM = mparmoryCosmeticItemBaseVM as MPArmoryCosmeticClothingItemVM) != null && mparmoryCosmeticClothingItemVM.EquipmentElement.Item.GetCosmeticEquipmentIndex() == equipmentIndex)
				{
					mparmoryCosmeticItemBaseVM.IsUsed = false;
				}
			}
		}

		// Token: 0x040005DC RID: 1500
		public readonly MPArmoryCosmeticsVM.ClothingCategory ClothingCategory;

		// Token: 0x040005DD RID: 1501
		private List<string> _defaultCosmeticIDs;
	}
}
