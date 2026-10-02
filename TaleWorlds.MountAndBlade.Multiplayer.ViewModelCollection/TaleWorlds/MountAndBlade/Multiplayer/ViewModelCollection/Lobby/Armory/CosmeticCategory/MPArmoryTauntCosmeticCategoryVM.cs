using System;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticCategory
{
	// Token: 0x02000083 RID: 131
	public class MPArmoryTauntCosmeticCategoryVM : MPArmoryCosmeticCategoryBaseVM
	{
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000D01 RID: 3329 RVA: 0x00028330 File Offset: 0x00026530
		// (remove) Token: 0x06000D02 RID: 3330 RVA: 0x00028364 File Offset: 0x00026564
		public static event Action<MPArmoryTauntCosmeticCategoryVM> OnSelected;

		// Token: 0x06000D03 RID: 3331 RVA: 0x00028397 File Offset: 0x00026597
		public MPArmoryTauntCosmeticCategoryVM(MPArmoryCosmeticsVM.TauntCategoryFlag tauntCategory)
			: base(CosmeticsManager.CosmeticType.Taunt)
		{
			this.TauntCategory = tauntCategory;
			base.CosmeticCategoryName = tauntCategory.ToString();
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x000283BA File Offset: 0x000265BA
		public override void RefreshValues()
		{
			base.RefreshValues();
			base.AvailableCosmetics.ApplyActionOnAllItems(delegate(MPArmoryCosmeticItemBaseVM c)
			{
				c.RefreshValues();
			});
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x000283EC File Offset: 0x000265EC
		protected override void ExecuteSelectCategory()
		{
			Action<MPArmoryTauntCosmeticCategoryVM> onSelected = MPArmoryTauntCosmeticCategoryVM.OnSelected;
			if (onSelected == null)
			{
				return;
			}
			onSelected(this);
		}

		// Token: 0x040005E4 RID: 1508
		public readonly MPArmoryCosmeticsVM.TauntCategoryFlag TauntCategory;
	}
}
