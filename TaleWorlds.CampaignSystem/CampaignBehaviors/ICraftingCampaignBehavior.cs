using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003FC RID: 1020
	public interface ICraftingCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x17000E38 RID: 3640
		// (get) Token: 0x0600404A RID: 16458
		IReadOnlyDictionary<Town, CraftingCampaignBehavior.CraftingOrderSlots> CraftingOrders { get; }

		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x0600404B RID: 16459
		IReadOnlyCollection<WeaponDesign> CraftingHistory { get; }

		// Token: 0x0600404C RID: 16460
		void CompleteOrder(Town town, CraftingOrder craftingOrder, ItemObject craftedItem, Hero completerHero);

		// Token: 0x0600404D RID: 16461
		ItemModifier GetCurrentItemModifier();

		// Token: 0x0600404E RID: 16462
		void SetCurrentItemModifier(ItemModifier modifier);

		// Token: 0x0600404F RID: 16463
		void SetCraftedWeaponName(ItemObject craftedWeaponItem, TextObject name);

		// Token: 0x06004050 RID: 16464
		void GetOrderResult(CraftingOrder craftingOrder, ItemObject craftedItem, out bool isSucceed, out TextObject orderRemark, out TextObject orderResult, out int finalPrice);

		// Token: 0x06004051 RID: 16465
		int GetCraftingDifficulty(WeaponDesign weaponDesign);

		// Token: 0x06004052 RID: 16466
		int GetHeroCraftingStamina(Hero hero);

		// Token: 0x06004053 RID: 16467
		void SetHeroCraftingStamina(Hero hero, int value);

		// Token: 0x06004054 RID: 16468
		int GetMaxHeroCraftingStamina(Hero hero);

		// Token: 0x06004055 RID: 16469
		void DoRefinement(Hero hero, Crafting.RefiningFormula refineFormula);

		// Token: 0x06004056 RID: 16470
		void DoSmelting(Hero currentCraftingHero, EquipmentElement equipmentElement);

		// Token: 0x06004057 RID: 16471
		ItemObject CreateCraftedWeaponInFreeBuildMode(Hero hero, WeaponDesign weaponDesign, ItemModifier weaponModifier = null);

		// Token: 0x06004058 RID: 16472
		ItemObject CreateCraftedWeaponInCraftingOrderMode(Hero crafterHero, CraftingOrder craftingOrder, WeaponDesign weaponDesign);

		// Token: 0x06004059 RID: 16473
		bool IsOpened(CraftingPiece craftingPiece, CraftingTemplate craftingTemplate);

		// Token: 0x0600405A RID: 16474
		CraftingOrder CreateCustomOrderForHero(Hero orderOwner, float orderDifficulty = -1f, WeaponDesign weaponDesign = null, CraftingTemplate craftingTemplate = null);

		// Token: 0x0600405B RID: 16475
		void CancelCustomOrder(Town town, CraftingOrder craftingOrder);

		// Token: 0x0600405C RID: 16476
		Hero GetActiveCraftingHero();

		// Token: 0x0600405D RID: 16477
		void SetActiveCraftingHero(Hero hero);
	}
}
