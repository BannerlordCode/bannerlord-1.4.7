using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F8 RID: 504
	public abstract class EquipmentSelectionModel : MBGameModel<EquipmentSelectionModel>
	{
		// Token: 0x06001F74 RID: 8052
		public abstract Equipment GetEquipmentForHeroComeOfAge(Hero hero, Equipment.EquipmentType equipmentType);

		// Token: 0x06001F75 RID: 8053
		public abstract Equipment GetEquipmentForHeroReachesTeenAge(Hero hero);

		// Token: 0x06001F76 RID: 8054
		public abstract Equipment GetEquipmentForInitialChildrenGeneration(Hero hero);

		// Token: 0x06001F77 RID: 8055
		public abstract Equipment GetEquipmentForDeliveredOffspring(Hero hero);

		// Token: 0x06001F78 RID: 8056
		public abstract ValueTuple<Equipment, Equipment> GetEquipmentsForChangingRuler(Hero newRuler, Hero oldRuler, Equipment.EquipmentType equipmentType);

		// Token: 0x06001F79 RID: 8057
		public abstract Equipment GetEquipmentForCompanionWhenTurningToLord(Hero companionHero, Equipment.EquipmentType equipmentType);
	}
}
