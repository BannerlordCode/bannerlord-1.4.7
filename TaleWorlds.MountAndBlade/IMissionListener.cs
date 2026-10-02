using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200025E RID: 606
	public interface IMissionListener
	{
		// Token: 0x06002257 RID: 8791
		void OnEquipItemsFromSpawnEquipmentBegin(Agent agent, Agent.CreationType creationType);

		// Token: 0x06002258 RID: 8792
		void OnEquipItemsFromSpawnEquipment(Agent agent, Agent.CreationType creationType);

		// Token: 0x06002259 RID: 8793
		void OnEndMission();

		// Token: 0x0600225A RID: 8794
		void OnMissionModeChange(MissionMode oldMissionMode, bool atStart);

		// Token: 0x0600225B RID: 8795
		void OnConversationCharacterChanged();

		// Token: 0x0600225C RID: 8796
		void OnResetMission();

		// Token: 0x0600225D RID: 8797
		void OnDeploymentPlanMade(Team team, bool isFirstPlan);
	}
}
