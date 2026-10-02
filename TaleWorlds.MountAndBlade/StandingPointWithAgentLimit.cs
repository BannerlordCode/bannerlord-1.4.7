using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000359 RID: 857
	public class StandingPointWithAgentLimit : StandingPoint
	{
		// Token: 0x06003135 RID: 12597 RVA: 0x000C7E7F File Offset: 0x000C607F
		public void AddValidAgent(Agent agent)
		{
			if (agent != null)
			{
				this._validAgents.Add(agent);
			}
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x000C7E90 File Offset: 0x000C6090
		public void ClearValidAgents()
		{
			this._validAgents.Clear();
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x000C7E9D File Offset: 0x000C609D
		public override bool IsDisabledForAgent(Agent agent)
		{
			return !this._validAgents.Contains(agent) || base.IsDisabledForAgent(agent);
		}

		// Token: 0x040014A4 RID: 5284
		private readonly List<Agent> _validAgents = new List<Agent>();
	}
}
