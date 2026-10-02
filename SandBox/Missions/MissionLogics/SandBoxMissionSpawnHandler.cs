using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000083 RID: 131
	public class SandBoxMissionSpawnHandler : MissionLogic
	{
		// Token: 0x06000530 RID: 1328 RVA: 0x00022DB7 File Offset: 0x00020FB7
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionAgentSpawnLogic = base.Mission.GetMissionBehavior<DefaultBattleMissionAgentSpawnLogic>();
			this._mapEvent = MapEvent.PlayerMapEvent;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00022DDC File Offset: 0x00020FDC
		protected static MissionSpawnSettings CreateSandBoxBattleWaveSpawnSettings()
		{
			int reinforcementWaveCount = BannerlordConfig.GetReinforcementWaveCount();
			return new MissionSpawnSettings(MissionSpawnSettings.InitialSpawnMethod.BattleSizeAllocating, MissionSpawnSettings.ReinforcementTimingMethod.GlobalTimer, MissionSpawnSettings.ReinforcementSpawnMethod.Wave, 3f, 0f, 0f, 0.5f, reinforcementWaveCount, 0f, 0f, 1f, 0.75f);
		}

		// Token: 0x040002C4 RID: 708
		protected DefaultBattleMissionAgentSpawnLogic _missionAgentSpawnLogic;

		// Token: 0x040002C5 RID: 709
		protected MapEvent _mapEvent;
	}
}
