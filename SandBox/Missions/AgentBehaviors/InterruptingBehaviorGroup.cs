using System;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000AD RID: 173
	public class InterruptingBehaviorGroup : AgentBehaviorGroup
	{
		// Token: 0x06000741 RID: 1857 RVA: 0x0003170F File Offset: 0x0002F90F
		public InterruptingBehaviorGroup(AgentNavigator navigator, Mission mission)
			: base(navigator, mission)
		{
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0003171C File Offset: 0x0002F91C
		public override void Tick(float dt, bool isSimulation)
		{
			if (base.IsActive)
			{
				if (base.ScriptedBehavior != null)
				{
					if (!base.ScriptedBehavior.IsActive)
					{
						base.DisableAllBehaviors();
						base.ScriptedBehavior.IsActive = true;
					}
				}
				else
				{
					int bestBehaviorIndex = this.GetBestBehaviorIndex(isSimulation);
					if (bestBehaviorIndex != -1 && !this.Behaviors[bestBehaviorIndex].IsActive)
					{
						base.DisableAllBehaviors();
						this.Behaviors[bestBehaviorIndex].IsActive = true;
					}
				}
				this.TickActiveBehaviors(dt, isSimulation);
			}
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0003179C File Offset: 0x0002F99C
		private void TickActiveBehaviors(float dt, bool isSimulation)
		{
			for (int i = this.Behaviors.Count - 1; i >= 0; i--)
			{
				AgentBehavior agentBehavior = this.Behaviors[i];
				if (agentBehavior.IsActive)
				{
					agentBehavior.Tick(dt, isSimulation);
				}
			}
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x000317DE File Offset: 0x0002F9DE
		public override float GetScore(bool isSimulation)
		{
			if (this.GetBestBehaviorIndex(isSimulation) != -1)
			{
				return 0.75f;
			}
			return 0f;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x000317F8 File Offset: 0x0002F9F8
		private int GetBestBehaviorIndex(bool isSimulation)
		{
			float num = 0f;
			int num2 = -1;
			for (int i = 0; i < this.Behaviors.Count; i++)
			{
				float availability = this.Behaviors[i].GetAvailability(isSimulation);
				if (availability > num)
				{
					num = availability;
					num2 = i;
				}
			}
			return num2;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0003183F File Offset: 0x0002FA3F
		public override void ForceThink(float inSeconds)
		{
			this.Navigator.RefreshBehaviorGroups(false);
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00031850 File Offset: 0x0002FA50
		public override void ConversationTick()
		{
			foreach (AgentBehavior agentBehavior in this.Behaviors)
			{
				if (agentBehavior.IsActive)
				{
					agentBehavior.ConversationTick();
				}
			}
		}
	}
}
