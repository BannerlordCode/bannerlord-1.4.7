using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Source.Missions
{
	// Token: 0x020003D7 RID: 983
	public class SimpleMountedPlayerMissionController : MissionLogic
	{
		// Token: 0x06003670 RID: 13936 RVA: 0x000E1600 File Offset: 0x000DF800
		public override void EarlyStart()
		{
			base.EarlyStart();
			foreach (MissionObject missionObject in base.Mission.ActiveMissionObjects.ToList<MissionObject>())
			{
				missionObject.SetDisabled(true);
			}
		}

		// Token: 0x06003671 RID: 13937 RVA: 0x000E1664 File Offset: 0x000DF864
		public override void AfterStart()
		{
			BasicCharacterObject @object = this._game.ObjectManager.GetObject<BasicCharacterObject>("aserai_tribal_horseman");
			WeakGameEntity weakGameEntity = Mission.Current.Scene.FindWeakEntityWithTag("spawnpoint_player_test");
			if (!weakGameEntity.IsValid)
			{
				weakGameEntity = Mission.Current.Scene.FindWeakEntityWithTag("spawnpoint_player");
			}
			MatrixFrame matrixFrame = (weakGameEntity.IsValid ? weakGameEntity.GetGlobalFrame() : MatrixFrame.Identity);
			AgentBuildData agentBuildData = new AgentBuildData(new BasicBattleAgentOrigin(@object));
			AgentBuildData agentBuildData2 = agentBuildData.InitialPosition(in matrixFrame.origin);
			Vec2 vec = matrixFrame.rotation.f.AsVec2;
			vec = vec.Normalized();
			agentBuildData2.InitialDirection(in vec).Controller(AgentControllerType.Player);
			base.Mission.SpawnAgent(agentBuildData, false).WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x000E1725 File Offset: 0x000DF925
		public override bool MissionEnded(ref MissionResult missionResult)
		{
			return base.Mission.InputManager.IsGameKeyPressed(4);
		}

		// Token: 0x0400176A RID: 5994
		private const string TestPlayerSpawnPoint = "spawnpoint_player_test";

		// Token: 0x0400176B RID: 5995
		private const string PlayerSpawnPoint = "spawnpoint_player";

		// Token: 0x0400176C RID: 5996
		private readonly Game _game = Game.Current;
	}
}
