using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x0200007E RID: 126
	public class RetirementMissionLogic : MissionLogic
	{
		// Token: 0x06000524 RID: 1316 RVA: 0x00022984 File Offset: 0x00020B84
		public override void AfterStart()
		{
			base.AfterStart();
			this.SpawnHermit();
			LeaveMissionLogic leaveMissionLogic = (LeaveMissionLogic)base.Mission.MissionLogics.FirstOrDefault<MissionLogic>((MissionLogic x) => x is LeaveMissionLogic);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x000229D4 File Offset: 0x00020BD4
		private void SpawnHermit()
		{
			List<GameEntity> list = base.Mission.Scene.FindEntitiesWithTag("sp_hermit").ToList<GameEntity>();
			MatrixFrame globalFrame = list[MBRandom.RandomInt(list.Count<GameEntity>())].GetGlobalFrame();
			CharacterObject @object = Campaign.Current.ObjectManager.GetObject<CharacterObject>("sp_hermit");
			AgentBuildData agentBuildData = new AgentBuildData(@object).TroopOrigin(new SimpleAgentOrigin(@object, -1, null, default(UniqueTroopDescriptor))).Team(base.Mission.SpectatorTeam).InitialPosition(in globalFrame.origin);
			Vec2 vec = globalFrame.rotation.f.AsVec2;
			vec = vec.Normalized();
			AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).CivilianEquipment(true).NoHorses(true)
				.NoWeapons(true)
				.ClothingColor1(base.Mission.PlayerTeam.Color)
				.ClothingColor2(base.Mission.PlayerTeam.Color2);
			base.Mission.SpawnAgent(agentBuildData2, false).SetMortalityState(Agent.MortalityState.Invulnerable);
		}
	}
}
