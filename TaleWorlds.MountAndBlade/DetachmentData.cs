using System;
using System.Collections.Generic;
using TaleWorlds.LinQuick;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200014B RID: 331
	public class DetachmentData
	{
		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06001133 RID: 4403 RVA: 0x00031383 File Offset: 0x0002F583
		public int AgentCount
		{
			get
			{
				return this.joinedFormations.SumQ<Formation>((Formation f) => f.CountOfDetachableNonPlayerUnits) + this.MovingAgentCount + this.DefendingAgentCount;
			}
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x000313C0 File Offset: 0x0002F5C0
		public bool IsPrecalculated()
		{
			int count = this.agentScores.Count;
			return count > 0 && count >= this.AgentCount;
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x000313EB File Offset: 0x0002F5EB
		public DetachmentData()
		{
			this.firstTime = MBCommon.GetTotalMissionTime();
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00031414 File Offset: 0x0002F614
		public void RemoveScoreOfAgent(Agent agent)
		{
			for (int i = this.agentScores.Count - 1; i >= 0; i--)
			{
				if (this.agentScores[i].Item1 == agent)
				{
					this.agentScores.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x040003F7 RID: 1015
		public List<Formation> joinedFormations = new List<Formation>();

		// Token: 0x040003F8 RID: 1016
		public List<ValueTuple<Agent, List<float>>> agentScores = new List<ValueTuple<Agent, List<float>>>();

		// Token: 0x040003F9 RID: 1017
		public int MovingAgentCount;

		// Token: 0x040003FA RID: 1018
		public int DefendingAgentCount;

		// Token: 0x040003FB RID: 1019
		public float firstTime;
	}
}
