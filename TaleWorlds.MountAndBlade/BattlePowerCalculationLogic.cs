using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200027C RID: 636
	public class BattlePowerCalculationLogic : MissionLogic, IBattlePowerCalculationLogic, IMissionBehavior
	{
		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06002365 RID: 9061 RVA: 0x0007DF35 File Offset: 0x0007C135
		// (set) Token: 0x06002366 RID: 9062 RVA: 0x0007DF3D File Offset: 0x0007C13D
		public bool IsTeamPowersCalculated { get; private set; }

		// Token: 0x06002367 RID: 9063 RVA: 0x0007DF48 File Offset: 0x0007C148
		public BattlePowerCalculationLogic()
		{
			this._sidePowerData = new Dictionary<Team, float>[2];
			for (int i = 0; i < 2; i++)
			{
				this._sidePowerData[i] = new Dictionary<Team, float>();
			}
			this.IsTeamPowersCalculated = false;
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x0007DF87 File Offset: 0x0007C187
		public float GetTotalTeamPower(Team team)
		{
			if (!this.IsTeamPowersCalculated)
			{
				this.CalculateTeamPowers();
			}
			return this._sidePowerData[(int)team.Side][team];
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x0007DFAC File Offset: 0x0007C1AC
		private void CalculateTeamPowers()
		{
			Mission.TeamCollection teams = base.Mission.Teams;
			foreach (Team team in teams)
			{
				this._sidePowerData[(int)team.Side].Add(team, 0f);
			}
			IMissionAgentSpawnLogic missionBehavior = base.Mission.GetMissionBehavior<IMissionAgentSpawnLogic>();
			for (int i = 0; i < 2; i++)
			{
				BattleSideEnum battleSideEnum = (BattleSideEnum)i;
				IEnumerable<IAgentOriginBase> allTroopsForSide = missionBehavior.GetAllTroopsForSide(battleSideEnum);
				Dictionary<Team, float> dictionary = this._sidePowerData[i];
				bool flag = base.Mission.PlayerTeam != null && base.Mission.PlayerTeam.Side == battleSideEnum;
				foreach (IAgentOriginBase agentOriginBase in allTroopsForSide)
				{
					Team agentTeam = Mission.GetAgentTeam(agentOriginBase, flag);
					BasicCharacterObject troop = agentOriginBase.Troop;
					Dictionary<Team, float> dictionary2 = dictionary;
					Team team2 = agentTeam;
					dictionary2[team2] += troop.GetPower();
				}
			}
			foreach (Team team3 in teams)
			{
				team3.QuerySystem.Expire();
			}
			this.IsTeamPowersCalculated = true;
		}

		// Token: 0x04000D9D RID: 3485
		private Dictionary<Team, float>[] _sidePowerData;
	}
}
