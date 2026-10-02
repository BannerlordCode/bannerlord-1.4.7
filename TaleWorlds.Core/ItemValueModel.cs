using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000073 RID: 115
	public abstract class ItemValueModel : MBGameModel<ItemValueModel>
	{
		// Token: 0x060007EE RID: 2030
		public abstract float GetEquipmentValueFromTier(float itemTierf);

		// Token: 0x060007EF RID: 2031
		public abstract float CalculateTier(ItemObject item);

		// Token: 0x060007F0 RID: 2032
		public abstract int CalculateValue(ItemObject item);

		// Token: 0x060007F1 RID: 2033
		public abstract bool GetIsTransferable(ItemObject item);
	}
}
