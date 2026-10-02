using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200032E RID: 814
	public class DefencePoint : ScriptComponentBehavior
	{
		// Token: 0x06002DF2 RID: 11762 RVA: 0x000B1337 File Offset: 0x000AF537
		public void AddDefender(Agent defender)
		{
			this.defenders.Add(defender);
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x000B1345 File Offset: 0x000AF545
		public bool RemoveDefender(Agent defender)
		{
			return this.defenders.Remove(defender);
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06002DF4 RID: 11764 RVA: 0x000B1353 File Offset: 0x000AF553
		public IEnumerable<Agent> Defenders
		{
			get
			{
				return this.defenders;
			}
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x000B135C File Offset: 0x000AF55C
		public void PurgeInactiveDefenders()
		{
			foreach (Agent agent in this.defenders.Where<Agent>((Agent d) => !d.IsActive()).ToList<Agent>())
			{
				this.RemoveDefender(agent);
			}
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x000B13DC File Offset: 0x000AF5DC
		private MatrixFrame GetPosition(int index)
		{
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			Vec3 f = globalFrame.rotation.f;
			f.Normalize();
			globalFrame.origin -= f * (float)index * ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BipedalRadius) * 2f * 1.5f;
			return globalFrame;
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x000B144C File Offset: 0x000AF64C
		public MatrixFrame GetVacantPosition(Agent a)
		{
			Mission mission = Mission.Current;
			Team team = mission.Teams.First<Team>((Team t) => t.Side == this.Side);
			for (int i = 0; i < 100; i++)
			{
				MatrixFrame position = this.GetPosition(i);
				Agent closestAllyAgent = mission.GetClosestAllyAgent(team, position.origin, ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BipedalRadius));
				if (closestAllyAgent == null || closestAllyAgent == a)
				{
					return position;
				}
			}
			Debug.FailedAssert("Couldn't find a vacant position", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\DefencePoint.cs", "GetVacantPosition", 73);
			return MatrixFrame.Identity;
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x000B14CC File Offset: 0x000AF6CC
		public int CountOccupiedDefenderPositions()
		{
			Mission mission = Mission.Current;
			Team team = mission.Teams.First<Team>((Team t) => t.Side == this.Side);
			for (int i = 0; i < 100; i++)
			{
				MatrixFrame position = this.GetPosition(i);
				if (mission.GetClosestAllyAgent(team, position.origin, ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BipedalRadius)) == null)
				{
					return i;
				}
			}
			return 100;
		}

		// Token: 0x0400122C RID: 4652
		private List<Agent> defenders = new List<Agent>();

		// Token: 0x0400122D RID: 4653
		public BattleSideEnum Side;
	}
}
