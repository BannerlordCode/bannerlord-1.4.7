using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000185 RID: 389
	public class TeamAiMultiplayerSiegeAttacker : TeamAISiegeComponent
	{
		// Token: 0x060014DC RID: 5340 RVA: 0x0004D85E File Offset: 0x0004BA5E
		public TeamAiMultiplayerSiegeAttacker(Mission currentMission, Team currentTeam, float thinkTimerTime, float applyTimerTime)
			: base(currentMission, currentTeam, thinkTimerTime, applyTimerTime)
		{
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x0004D86B File Offset: 0x0004BA6B
		public override void OnUnitAddedToFormationForTheFirstTime(Formation formation)
		{
		}
	}
}
