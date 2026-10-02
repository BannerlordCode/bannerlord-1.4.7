using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000A7 RID: 167
	public class DailyBehaviorGroup : AgentBehaviorGroup
	{
		// Token: 0x060006FD RID: 1789 RVA: 0x0002F3BC File Offset: 0x0002D5BC
		public DailyBehaviorGroup(AgentNavigator navigator, Mission mission)
			: base(navigator, mission)
		{
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0002F3C8 File Offset: 0x0002D5C8
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
				else if (this.CheckBehaviorTimer == null || this.CheckBehaviorTimer.Check(base.Mission.CurrentTime))
				{
					this.Think(isSimulation);
				}
				this.TickActiveBehaviors(dt, isSimulation);
			}
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0002F438 File Offset: 0x0002D638
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

		// Token: 0x06000700 RID: 1792 RVA: 0x0002F494 File Offset: 0x0002D694
		private void Think(bool isSimulation)
		{
			float num = 0f;
			float[] array = new float[this.Behaviors.Count];
			for (int i = 0; i < this.Behaviors.Count; i++)
			{
				array[i] = this.Behaviors[i].GetAvailability(isSimulation);
				num += array[i];
			}
			if (num > 0f)
			{
				float num2 = MBRandom.RandomFloat * num;
				int j = 0;
				while (j < array.Length)
				{
					float num3 = array[j];
					num2 -= num3;
					if (num2 < 0f)
					{
						if (!this.Behaviors[j].IsActive)
						{
							base.DisableAllBehaviors();
							this.Behaviors[j].IsActive = true;
							this.CheckBehaviorTime = this.Behaviors[j].CheckTime;
							this.SetCheckBehaviorTimer(this.CheckBehaviorTime);
							return;
						}
						break;
					}
					else
					{
						j++;
					}
				}
			}
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0002F570 File Offset: 0x0002D770
		private void TickActiveBehaviors(float dt, bool isSimulation)
		{
			foreach (AgentBehavior agentBehavior in this.Behaviors)
			{
				if (agentBehavior.IsActive)
				{
					agentBehavior.Tick(dt, isSimulation);
				}
			}
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0002F5CC File Offset: 0x0002D7CC
		private void SetCheckBehaviorTimer(float time)
		{
			if (this.CheckBehaviorTimer == null)
			{
				this.CheckBehaviorTimer = new Timer(base.Mission.CurrentTime, time, true);
				return;
			}
			this.CheckBehaviorTimer.Reset(base.Mission.CurrentTime, time);
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0002F606 File Offset: 0x0002D806
		public override float GetScore(bool isSimulation)
		{
			return 0.5f;
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0002F610 File Offset: 0x0002D810
		public override void OnAgentRemoved(Agent agent)
		{
			if (base.IsActive)
			{
				foreach (AgentBehavior agentBehavior in this.Behaviors)
				{
					if (agentBehavior.IsActive)
					{
						agentBehavior.OnAgentRemoved(agent);
					}
				}
			}
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0002F674 File Offset: 0x0002D874
		protected override void OnActivate()
		{
			if (CampaignMission.Current.Location != null)
			{
				LocationCharacter locationCharacter = CampaignMission.Current.Location.GetLocationCharacter(base.OwnerAgent.Origin);
				if (locationCharacter != null && locationCharacter.ActionSetCode != locationCharacter.AlarmedActionSetCode)
				{
					AnimationSystemData animationSystemData = locationCharacter.GetAgentBuildData().AgentMonster.FillAnimationSystemData(MBGlobals.GetActionSet(locationCharacter.ActionSetCode), locationCharacter.Character.GetStepSize(), false);
					base.OwnerAgent.SetActionSet(ref animationSystemData);
				}
			}
			this.Navigator.SetItemsVisibility(true);
			this.Navigator.SetSpecialItem();
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0002F70A File Offset: 0x0002D90A
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			this.CheckBehaviorTimer = null;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0002F719 File Offset: 0x0002D919
		public override void ForceThink(float inSeconds)
		{
			if (MathF.Abs(inSeconds) < 1E-45f)
			{
				this.Think(false);
				return;
			}
			this.SetCheckBehaviorTimer(inSeconds);
		}
	}
}
