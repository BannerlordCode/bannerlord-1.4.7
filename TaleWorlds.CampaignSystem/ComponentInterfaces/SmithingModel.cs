using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200019F RID: 415
	public abstract class SmithingModel : MBGameModel<SmithingModel>
	{
		// Token: 0x06001C9B RID: 7323
		public abstract int GetCraftingPartDifficulty(CraftingPiece craftingPiece);

		// Token: 0x06001C9C RID: 7324
		public abstract int CalculateWeaponDesignDifficulty(WeaponDesign weaponDesign);

		// Token: 0x06001C9D RID: 7325
		public abstract ItemModifier GetCraftedWeaponModifier(WeaponDesign weaponDesign, Hero weaponsmith);

		// Token: 0x06001C9E RID: 7326
		public abstract IEnumerable<Crafting.RefiningFormula> GetRefiningFormulas(Hero weaponsmith);

		// Token: 0x06001C9F RID: 7327
		public abstract ItemObject GetCraftingMaterialItem(CraftingMaterials craftingMaterial);

		// Token: 0x06001CA0 RID: 7328
		public abstract int[] GetSmeltingOutputForItem(ItemObject item);

		// Token: 0x06001CA1 RID: 7329
		public abstract int GetSkillXpForRefining(ref Crafting.RefiningFormula refineFormula);

		// Token: 0x06001CA2 RID: 7330
		public abstract int GetSkillXpForSmelting(ItemObject item);

		// Token: 0x06001CA3 RID: 7331
		public abstract int GetSkillXpForSmithingInFreeBuildMode(ItemObject item);

		// Token: 0x06001CA4 RID: 7332
		public abstract int GetSkillXpForSmithingInCraftingOrderMode(ItemObject item);

		// Token: 0x06001CA5 RID: 7333
		public abstract int[] GetSmithingCostsForWeaponDesign(WeaponDesign weaponDesign);

		// Token: 0x06001CA6 RID: 7334
		public abstract int GetEnergyCostForRefining(ref Crafting.RefiningFormula refineFormula, Hero hero);

		// Token: 0x06001CA7 RID: 7335
		public abstract int GetEnergyCostForSmithing(ItemObject item, Hero hero);

		// Token: 0x06001CA8 RID: 7336
		public abstract int GetEnergyCostForSmelting(ItemObject item, Hero hero);

		// Token: 0x06001CA9 RID: 7337
		public abstract float ResearchPointsNeedForNewPart(int totalPartCount, int openedPartCount);

		// Token: 0x06001CAA RID: 7338
		public abstract int GetPartResearchGainForSmeltingItem(ItemObject item, Hero hero);

		// Token: 0x06001CAB RID: 7339
		public abstract int GetPartResearchGainForSmithingItem(ItemObject item, Hero hero, bool isFreeBuildMode);
	}
}
