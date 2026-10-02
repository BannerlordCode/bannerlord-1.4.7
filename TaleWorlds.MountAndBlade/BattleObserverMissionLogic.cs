using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200027B RID: 635
	public class BattleObserverMissionLogic : MissionLogic
	{
		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x0600235B RID: 9051 RVA: 0x0007DC1B File Offset: 0x0007BE1B
		// (set) Token: 0x0600235C RID: 9052 RVA: 0x0007DC23 File Offset: 0x0007BE23
		public IBattleObserver BattleObserver { get; private set; }

		// Token: 0x0600235D RID: 9053 RVA: 0x0007DC2C File Offset: 0x0007BE2C
		public void SetObserver(IBattleObserver observer)
		{
			this.BattleObserver = observer;
			foreach (Agent agent in this._onAgentBuildCache)
			{
				this.BattleObserver.TroopNumberChanged(agent.Team.Side, agent.Origin.BattleCombatant, agent.Character, 1, 0, 0, 0, 0, 0);
				this._builtAgentCountForSides[(int)agent.Team.Side]++;
			}
			this._onAgentBuildCache.Clear();
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x0007DCD4 File Offset: 0x0007BED4
		public override void EarlyStart()
		{
			base.EarlyStart();
			this._builtAgentCountForSides = new int[2];
			this._removedAgentCountForSides = new int[2];
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x0007DCF4 File Offset: 0x0007BEF4
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			if (agent.IsHuman)
			{
				if (this.BattleObserver != null && agent.Team != Team.Invalid)
				{
					BattleSideEnum side = agent.Team.Side;
					this.BattleObserver.TroopNumberChanged(side, agent.Origin.BattleCombatant, agent.Character, 1, 0, 0, 0, 0, 0);
					this._builtAgentCountForSides[(int)side]++;
					return;
				}
				this._onAgentBuildCache.Add(agent);
			}
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x0007DD6C File Offset: 0x0007BF6C
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (affectedAgent.IsHuman && affectedAgent.Team != Team.Invalid)
			{
				BattleSideEnum side = affectedAgent.Team.Side;
				switch (agentState)
				{
				case AgentState.Routed:
					this.BattleObserver.TroopNumberChanged(side, affectedAgent.Origin.BattleCombatant, affectedAgent.Character, -1, 0, 0, 1, 0, 0);
					break;
				case AgentState.Unconscious:
					this.BattleObserver.TroopNumberChanged(side, affectedAgent.Origin.BattleCombatant, affectedAgent.Character, -1, 0, 1, 0, 0, 0);
					break;
				case AgentState.Killed:
					this.BattleObserver.TroopNumberChanged(side, affectedAgent.Origin.BattleCombatant, affectedAgent.Character, -1, 1, 0, 0, 0, 0);
					break;
				default:
					throw new ArgumentOutOfRangeException("agentState", agentState, null);
				}
				this._removedAgentCountForSides[(int)side]++;
				if (affectorAgent != null && affectorAgent.IsHuman && (agentState == AgentState.Unconscious || agentState == AgentState.Killed))
				{
					this.BattleObserver.TroopNumberChanged(affectorAgent.Team.Side, affectorAgent.Origin.BattleCombatant, affectorAgent.Character, 0, 0, 0, 0, 1, 0);
				}
			}
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x0007DE88 File Offset: 0x0007C088
		public override void OnAgentTeamChanged(Team prevTeam, Team newTeam, Agent agent)
		{
			if (prevTeam == Team.Invalid && agent.IsHuman && newTeam != null && newTeam != Team.Invalid)
			{
				this.BattleObserver.TroopNumberChanged(agent.Team.Side, agent.Origin.BattleCombatant, agent.Character, 1, 0, 0, 0, 0, 0);
				this._builtAgentCountForSides[(int)agent.Team.Side]++;
			}
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x0007DEF8 File Offset: 0x0007C0F8
		public override void OnMissionResultReady(MissionResult missionResult)
		{
			if (missionResult.PlayerVictory)
			{
				this.BattleObserver.BattleResultsReady();
			}
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x0007DF0D File Offset: 0x0007C10D
		public float GetDeathToBuiltAgentRatioForSide(BattleSideEnum side)
		{
			return (float)this._removedAgentCountForSides[(int)side] / (float)this._builtAgentCountForSides[(int)side];
		}

		// Token: 0x04000D99 RID: 3481
		private int[] _builtAgentCountForSides;

		// Token: 0x04000D9A RID: 3482
		private int[] _removedAgentCountForSides;

		// Token: 0x04000D9B RID: 3483
		private List<Agent> _onAgentBuildCache = new List<Agent>();
	}
}
