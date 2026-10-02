using System;
using SandBox.Tournaments.MissionLogics;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics.Arena
{
	// Token: 0x02000099 RID: 153
	public class ArenaDuelMissionBehavior : MissionLogic
	{
		// Token: 0x06000657 RID: 1623 RVA: 0x0002AEF2 File Offset: 0x000290F2
		public override void AfterStart()
		{
			TournamentBehavior.DeleteTournamentSetsExcept(base.Mission.Scene.FindEntityWithTag("tournament_fight"));
		}
	}
}
