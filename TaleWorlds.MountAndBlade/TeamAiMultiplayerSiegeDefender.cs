using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000186 RID: 390
	public class TeamAiMultiplayerSiegeDefender : TeamAISiegeComponent
	{
		// Token: 0x060014DE RID: 5342 RVA: 0x0004D86D File Offset: 0x0004BA6D
		public TeamAiMultiplayerSiegeDefender(Mission currentMission, Team currentTeam, float thinkTimerTime, float applyTimerTime)
			: base(currentMission, currentTeam, thinkTimerTime, applyTimerTime)
		{
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x0004D87A File Offset: 0x0004BA7A
		public override void OnUnitAddedToFormationForTheFirstTime(Formation formation)
		{
		}
	}
}
