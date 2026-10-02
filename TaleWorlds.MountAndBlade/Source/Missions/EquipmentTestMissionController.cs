using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.Source.Missions
{
	// Token: 0x020003D3 RID: 979
	public class EquipmentTestMissionController : MissionLogic
	{
		// Token: 0x0600365B RID: 13915 RVA: 0x000E1178 File Offset: 0x000DF378
		public override void AfterStart()
		{
			base.AfterStart();
			WeakGameEntity weakGameEntity = base.Mission.Scene.FindWeakEntityWithTag("spawnpoint_player");
			base.Mission.SpawnAgent(new AgentBuildData(Game.Current.PlayerTroop).Team(base.Mission.AttackerTeam).InitialFrameFromSpawnPointEntity(weakGameEntity).CivilianEquipment(false)
				.Controller(AgentControllerType.Player), false);
		}
	}
}
