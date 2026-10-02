using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x020003FA RID: 1018
	public abstract class ItemPickupModel : MBGameModel<ItemPickupModel>
	{
		// Token: 0x06003782 RID: 14210
		public abstract float GetItemScoreForAgent(SpawnedItemEntity item, Agent agent);

		// Token: 0x06003783 RID: 14211
		public abstract bool IsItemAvailableForAgent(SpawnedItemEntity item, Agent agent, EquipmentIndex slotToPickUp);

		// Token: 0x06003784 RID: 14212
		public abstract bool IsAgentEquipmentSuitableForPickUpAvailability(Agent agent);
	}
}
