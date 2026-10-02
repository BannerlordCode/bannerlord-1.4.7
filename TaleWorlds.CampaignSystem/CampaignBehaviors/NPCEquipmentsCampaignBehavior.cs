using System;
using Helpers;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000420 RID: 1056
	public class NPCEquipmentsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600437C RID: 17276 RVA: 0x0014787B File Offset: 0x00145A7B
		public override void RegisterEvents()
		{
			CampaignEvents.RulingClanChanged.AddNonSerializedListener(this, new Action<Kingdom, Clan>(this.OnRulingClanChanged));
		}

		// Token: 0x0600437D RID: 17277 RVA: 0x00147894 File Offset: 0x00145A94
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600437E RID: 17278 RVA: 0x00147898 File Offset: 0x00145A98
		private void OnRulingClanChanged(Kingdom kingdom, Clan oldRulingClan)
		{
			Hero leader = kingdom.Leader;
			Hero hero = ((oldRulingClan != null) ? oldRulingClan.Leader : null);
			ValueTuple<Equipment, Equipment> equipmentsForChangingRuler = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentsForChangingRuler(leader, hero, Equipment.EquipmentType.Civilian);
			ValueTuple<Equipment, Equipment> equipmentsForChangingRuler2 = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentsForChangingRuler(leader, hero, Equipment.EquipmentType.Battle);
			if (leader != Hero.MainHero)
			{
				EquipmentHelper.AssignHeroEquipmentFromEquipment(leader, equipmentsForChangingRuler2.Item1);
				EquipmentHelper.AssignHeroEquipmentFromEquipment(leader, equipmentsForChangingRuler.Item1);
			}
			if (hero != null && hero.IsActive && hero != Hero.MainHero)
			{
				EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, equipmentsForChangingRuler2.Item2);
				EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, equipmentsForChangingRuler.Item2);
			}
		}
	}
}
