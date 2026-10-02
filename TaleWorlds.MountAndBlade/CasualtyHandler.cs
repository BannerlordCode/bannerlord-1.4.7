using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200027E RID: 638
	public class CasualtyHandler : MissionLogic
	{
		// Token: 0x06002372 RID: 9074 RVA: 0x0007E425 File Offset: 0x0007C625
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			this.RegisterCasualty(affectedAgent);
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x0007E42E File Offset: 0x0007C62E
		public override void OnAgentFleeing(Agent affectedAgent)
		{
			this.RegisterCasualty(affectedAgent);
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x0007E438 File Offset: 0x0007C638
		public int GetCasualtyCountOfFormation(Formation formation)
		{
			int num;
			if (!this._casualtyCounts.TryGetValue(formation, out num))
			{
				num = 0;
				this._casualtyCounts[formation] = 0;
			}
			return num;
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x0007E468 File Offset: 0x0007C668
		public float GetCasualtyPowerLossOfFormation(Formation formation)
		{
			float num;
			if (!this._powerLoss.TryGetValue(formation, out num))
			{
				num = 0f;
				this._powerLoss[formation] = 0f;
			}
			return num;
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x0007E4A0 File Offset: 0x0007C6A0
		private void RegisterCasualty(Agent agent)
		{
			Formation formation = agent.Formation;
			if (formation != null)
			{
				if (this._casualtyCounts.ContainsKey(formation))
				{
					Dictionary<Formation, int> casualtyCounts = this._casualtyCounts;
					Formation formation2 = formation;
					int num = casualtyCounts[formation2];
					casualtyCounts[formation2] = num + 1;
				}
				else
				{
					this._casualtyCounts[formation] = 1;
				}
				if (this._powerLoss.ContainsKey(formation))
				{
					Dictionary<Formation, float> powerLoss = this._powerLoss;
					Formation formation2 = formation;
					powerLoss[formation2] += agent.Character.GetPower();
					return;
				}
				this._powerLoss[formation] = agent.Character.GetPower();
			}
		}

		// Token: 0x04000DA1 RID: 3489
		private readonly Dictionary<Formation, int> _casualtyCounts = new Dictionary<Formation, int>();

		// Token: 0x04000DA2 RID: 3490
		private readonly Dictionary<Formation, float> _powerLoss = new Dictionary<Formation, float>();
	}
}
