using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Missions.AgentBehaviors;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

namespace SandBox.Objects
{
	// Token: 0x0200003E RID: 62
	public class StealthZone
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000D80C File Offset: 0x0000BA0C
		// (set) Token: 0x06000231 RID: 561 RVA: 0x0000D814 File Offset: 0x0000BA14
		public bool AreAgentsActive { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000232 RID: 562 RVA: 0x0000D81D File Offset: 0x0000BA1D
		// (set) Token: 0x06000233 RID: 563 RVA: 0x0000D825 File Offset: 0x0000BA25
		public bool UseVolumeBox { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0000D82E File Offset: 0x0000BA2E
		// (set) Token: 0x06000235 RID: 565 RVA: 0x0000D836 File Offset: 0x0000BA36
		public int EliminatedAgents { get; private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000D83F File Offset: 0x0000BA3F
		// (set) Token: 0x06000237 RID: 567 RVA: 0x0000D847 File Offset: 0x0000BA47
		private Timer ActivationTimer { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000D850 File Offset: 0x0000BA50
		// (set) Token: 0x06000239 RID: 569 RVA: 0x0000D858 File Offset: 0x0000BA58
		private Timer DeactivationTimer { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600023A RID: 570 RVA: 0x0000D861 File Offset: 0x0000BA61
		// (set) Token: 0x0600023B RID: 571 RVA: 0x0000D869 File Offset: 0x0000BA69
		private Timer ForceTargetTimer { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000D872 File Offset: 0x0000BA72
		// (set) Token: 0x0600023D RID: 573 RVA: 0x0000D87A File Offset: 0x0000BA7A
		public List<Agent> Agents { get; private set; }

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600023E RID: 574 RVA: 0x0000D884 File Offset: 0x0000BA84
		// (remove) Token: 0x0600023F RID: 575 RVA: 0x0000D8BC File Offset: 0x0000BABC
		public event Action OnActivated;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000240 RID: 576 RVA: 0x0000D8F4 File Offset: 0x0000BAF4
		// (remove) Token: 0x06000241 RID: 577 RVA: 0x0000D92C File Offset: 0x0000BB2C
		public event Action OnDisactivated;

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000D961 File Offset: 0x0000BB61
		// (set) Token: 0x06000243 RID: 579 RVA: 0x0000D969 File Offset: 0x0000BB69
		public VolumeBox VolumeBox { get; private set; }

		// Token: 0x06000244 RID: 580 RVA: 0x0000D974 File Offset: 0x0000BB74
		public StealthZone(Agent targetAgent, bool useVolumeBox)
		{
			this.TargetAgent = targetAgent;
			this.EliminatedAgents = 0;
			this.AreAgentsActive = false;
			this.UseVolumeBox = useVolumeBox;
			if (this.UseVolumeBox)
			{
				GameEntity gameEntity = Mission.Current.Scene.FindEntityWithTag("stealth_zone_volume");
				this.VolumeBox = ((gameEntity != null) ? gameEntity.GetFirstScriptOfType<VolumeBox>() : null);
			}
			this.IsZoneUsable = false;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000D9D8 File Offset: 0x0000BBD8
		public void SetStealthAgents(List<Agent> agents)
		{
			this.Agents = agents;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000D9E4 File Offset: 0x0000BBE4
		public void Tick()
		{
			bool flag = this.TargetAgent != null && this.TargetAgent.IsActive() && (!this.UseVolumeBox || this.VolumeBox.IsPointIn(this.TargetAgent.Position));
			if (this.IsZoneUsable)
			{
				if (!this.AreAgentsActive && flag && this.ActivationTimer == null)
				{
					this.ActivationTimer = new Timer(Mission.Current.CurrentTime, 2f, true);
				}
				if (this.AreAgentsActive && !flag && this.DeactivationTimer == null)
				{
					this.DeactivationTimer = new Timer(Mission.Current.CurrentTime, 2f, true);
				}
				if (!flag)
				{
					this.ActivationTimer = null;
				}
				else
				{
					this.DeactivationTimer = null;
				}
				if (this.AreAgentsActive && !flag && this.DeactivationTimer.Check(Mission.Current.CurrentTime))
				{
					this.DeactivateStealthZone();
				}
				if (flag && !this.AreAgentsActive && this.ActivationTimer.Check(Mission.Current.CurrentTime) && this.Agents != null && this.Agents.Count > 0)
				{
					this.ActivateStealthZone();
					return;
				}
			}
			else
			{
				if (flag && this.ForceTargetTimer == null)
				{
					this.ForceTargetTimer = new Timer(Mission.Current.CurrentTime, 5f, true);
				}
				else if (!flag)
				{
					this.ForceTargetTimer = null;
				}
				if (flag && this.ForceTargetTimer.Check(Mission.Current.CurrentTime))
				{
					StealthZone.StealthZoneEvent onTargetInZone = this.OnTargetInZone;
					if (onTargetInZone != null)
					{
						onTargetInZone();
					}
					this.ForceTargetTimer.Reset(Mission.Current.CurrentTime);
				}
			}
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000DB8C File Offset: 0x0000BD8C
		private void ActivateStealthZone()
		{
			this.AreAgentsActive = true;
			this.ActivationTimer = null;
			this.DeactivationTimer = null;
			this.ActivateStealthZoneInternal();
			this.OnActivated();
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000DBB4 File Offset: 0x0000BDB4
		private void DeactivateStealthZone()
		{
			this.AreAgentsActive = false;
			this.ActivationTimer = null;
			this.DeactivationTimer = null;
			this.DeactivateStealthZoneInternal();
			this.OnDisactivated();
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000DBDC File Offset: 0x0000BDDC
		private void ActivateStealthZoneInternal()
		{
			Mission.Current.SetMissionMode(MissionMode.Stealth, false);
			Mission.Current.IsInventoryAccessible = false;
			Mission.Current.IsQuestScreenAccessible = false;
			this.EliminatedAgents = 0;
			foreach (Agent agent in this.Agents)
			{
				if (agent.IsActive())
				{
					agent.SetAgentFlags(agent.GetAgentFlags() | AgentFlag.CanGetAlarmed);
					this.SetStealthMode(agent, true);
					agent.SetTeam(Mission.Current.PlayerEnemyTeam, true);
				}
			}
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000DC84 File Offset: 0x0000BE84
		private void DeactivateStealthZoneInternal()
		{
			Mission.Current.SetMissionMode(MissionMode.StartUp, false);
			Mission.Current.IsInventoryAccessible = true;
			Mission.Current.IsQuestScreenAccessible = true;
			foreach (Agent agent in this.Agents)
			{
				if (agent.IsActive())
				{
					agent.SetAgentFlags(agent.GetAgentFlags() & ~AgentFlag.CanGetAlarmed);
					this.SetStealthMode(agent, false);
					agent.Health = agent.HealthLimit;
					agent.SetTeam(Team.Invalid, true);
				}
			}
			this.EliminatedAgents = 0;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000DD34 File Offset: 0x0000BF34
		public void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent)
		{
			if (!affectedAgent.IsMainAgent || this.Agents == null || !this.Agents.Contains(affectorAgent))
			{
				if (affectedAgent.IsAIControlled && affectedAgent.Team == Mission.Current.PlayerEnemyTeam && this.Agents.Contains(affectedAgent))
				{
					int eliminatedAgents = this.EliminatedAgents;
					this.EliminatedAgents = eliminatedAgents + 1;
					this.Agents.Remove(affectedAgent);
				}
				return;
			}
			StealthZone.StealthZoneEvent onTargetEliminated = this.OnTargetEliminated;
			if (onTargetEliminated == null)
			{
				return;
			}
			onTargetEliminated();
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000DDB5 File Offset: 0x0000BFB5
		public bool IsAgentInside(Agent agent)
		{
			VolumeBox volumeBox = this.VolumeBox;
			return volumeBox != null && volumeBox.IsPointIn(agent.Position);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000DDCE File Offset: 0x0000BFCE
		public void OnPlayerFlees()
		{
			if (this.IsAnyoneAlarmed() || this.IsAgentInside(this.TargetAgent))
			{
				StealthZone.StealthZoneEvent onTargetFlees = this.OnTargetFlees;
				if (onTargetFlees == null)
				{
					return;
				}
				onTargetFlees();
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000DDF8 File Offset: 0x0000BFF8
		private void SetStealthMode(Agent agent, bool isActive)
		{
			CampaignAgentComponent component = agent.GetComponent<CampaignAgentComponent>();
			AlarmedBehaviorGroup alarmedBehaviorGroup = ((component != null) ? component.AgentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>() : null);
			if (alarmedBehaviorGroup != null)
			{
				alarmedBehaviorGroup.DoNotCheckForAlarmFactorIncrease = !isActive;
				alarmedBehaviorGroup.ResetAlarmFactor();
				if (isActive)
				{
					alarmedBehaviorGroup.DoNotIncreaseAlarmFactorDueToSeeingOrHearingTheEnemy = false;
				}
				alarmedBehaviorGroup.IsActive = isActive;
				agent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>().IsActive = !isActive;
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000DE5A File Offset: 0x0000C05A
		public void ResetEvents()
		{
			this.OnTargetInZone = null;
			this.OnTargetFlees = null;
			this.OnTargetEliminated = null;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000DE71 File Offset: 0x0000C071
		public void DisableAll()
		{
			this.SetStealthAgents(null);
			this.ResetEvents();
			this.IsZoneUsable = false;
			this.AreAgentsActive = false;
			this.TargetAgent = null;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000DE95 File Offset: 0x0000C095
		private bool IsAnyoneAlarmed()
		{
			if (this.Agents != null)
			{
				return this.Agents.Any<Agent>((Agent x) => x.IsAlarmed());
			}
			return false;
		}

		// Token: 0x040000DD RID: 221
		public const string VolumeBoxId = "stealth_zone_volume";

		// Token: 0x040000DF RID: 223
		public bool IsZoneUsable;

		// Token: 0x040000E6 RID: 230
		public StealthZone.StealthZoneEvent OnTargetFlees;

		// Token: 0x040000E7 RID: 231
		public StealthZone.StealthZoneEvent OnTargetEliminated;

		// Token: 0x040000E8 RID: 232
		public StealthZone.StealthZoneEvent OnTargetInZone;

		// Token: 0x040000EC RID: 236
		public Agent TargetAgent;

		// Token: 0x02000147 RID: 327
		// (Invoke) Token: 0x06000E1A RID: 3610
		public delegate void StealthZoneEvent();
	}
}
