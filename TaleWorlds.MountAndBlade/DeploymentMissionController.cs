using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000282 RID: 642
	public abstract class DeploymentMissionController : MissionLogic
	{
		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x060023CC RID: 9164 RVA: 0x000806B8 File Offset: 0x0007E8B8
		// (set) Token: 0x060023CD RID: 9165 RVA: 0x000806C0 File Offset: 0x0007E8C0
		public bool TeamSetupOver { get; private set; }

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x060023CE RID: 9166 RVA: 0x000806CC File Offset: 0x0007E8CC
		// (remove) Token: 0x060023CF RID: 9167 RVA: 0x00080704 File Offset: 0x0007E904
		public event Action OnAfterSetupTeams;

		// Token: 0x060023D0 RID: 9168 RVA: 0x00080739 File Offset: 0x0007E939
		public DeploymentMissionController(bool isPlayerAttacker)
		{
			this.IsPlayerAttacker = isPlayerAttacker;
			this.PlayerSide = (this.IsPlayerAttacker ? BattleSideEnum.Attacker : BattleSideEnum.Defender);
			this.EnemySide = (this.IsPlayerAttacker ? BattleSideEnum.Defender : BattleSideEnum.Attacker);
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x0008076C File Offset: 0x0007E96C
		public override void AfterStart()
		{
			base.Mission.AllowAiTicking = false;
			this.OnAfterStart();
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x00080780 File Offset: 0x0007E980
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			base.Mission.AreOrderGesturesEnabled_AdditionalCondition += this.AreOrderGesturesEnabled_AdditionalCondition;
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x000807A0 File Offset: 0x0007E9A0
		public void FinishDeployment()
		{
			this.BeforeDeploymentFinished();
			if (this.IsPlayerAttacker)
			{
				this.UnhideAgentsOfSide(BattleSideEnum.Defender);
			}
			Mission.Current.OnDeploymentFinished();
			foreach (Team team in base.Mission.Teams)
			{
				foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
				{
					if (formation.CountOfUnits > 0)
					{
						formation.ApplyActionOnEachUnit(delegate(Agent agent)
						{
							if (agent.IsAIControlled)
							{
								agent.SetAlarmState(Agent.AIStateFlag.Alarmed);
								agent.SetIsAIPaused(false);
								if (agent.GetAgentFlags().HasAnyFlag(AgentFlag.CanWieldWeapon))
								{
									agent.ResetEnemyCaches();
								}
								HumanAIComponent humanAIComponent = agent.HumanAIComponent;
								if (humanAIComponent == null)
								{
									return;
								}
								humanAIComponent.SyncBehaviorParamsIfNecessary();
							}
						}, null);
					}
				}
			}
			Agent initialPlayerAgent = base.Mission.InitialPlayerAgent;
			initialPlayerAgent.SetDetachableFromFormation(true);
			initialPlayerAgent.Controller = AgentControllerType.Player;
			base.Mission.AllowAiTicking = true;
			base.Mission.DisableDying = false;
			base.Mission.SetFallAvoidSystemActive(false);
			Mission.Current.OnAfterDeploymentFinished();
			this.AfterDeploymentFinished();
			base.Mission.RemoveMissionBehavior(this);
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x000808D0 File Offset: 0x0007EAD0
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (!this.TeamSetupOver && base.Mission.Scene != null)
			{
				this.SetupTeams();
				this.TeamSetupOver = true;
			}
			if (this.TeamSetupOver && !this.AfterSetupTeamsCalled)
			{
				Action onAfterSetupTeams = this.OnAfterSetupTeams;
				if (onAfterSetupTeams != null)
				{
					onAfterSetupTeams();
				}
				this.AfterSetupTeamsCalled = true;
			}
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x00080934 File Offset: 0x0007EB34
		protected void SetupAgentAIStatesForSide(BattleSideEnum battleSide)
		{
			foreach (Team team in Mission.GetTeamsOfSide(battleSide))
			{
				foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
				{
					if (formation.CountOfUnits > 0)
					{
						formation.ApplyActionOnEachUnit(delegate(Agent agent)
						{
							if (agent.IsAIControlled)
							{
								agent.SetAlarmState(Agent.AIStateFlag.None);
								agent.SetIsAIPaused(true);
							}
						}, null);
					}
				}
			}
		}

		// Token: 0x060023D6 RID: 9174
		protected abstract void OnAfterStart();

		// Token: 0x060023D7 RID: 9175
		protected abstract void OnSetupTeamsOfSide(BattleSideEnum side);

		// Token: 0x060023D8 RID: 9176
		protected abstract void OnSetupTeamsFinished();

		// Token: 0x060023D9 RID: 9177
		protected abstract void BeforeDeploymentFinished();

		// Token: 0x060023DA RID: 9178
		protected abstract void AfterDeploymentFinished();

		// Token: 0x060023DB RID: 9179 RVA: 0x000809E4 File Offset: 0x0007EBE4
		protected virtual void SetupAIOfEnemySide(BattleSideEnum enemySide)
		{
			Team team = ((enemySide == BattleSideEnum.Attacker) ? base.Mission.AttackerTeam : base.Mission.DefenderTeam);
			this.SetupAIOfEnemyTeam(team);
			Team team2 = ((enemySide == BattleSideEnum.Attacker) ? base.Mission.AttackerAllyTeam : base.Mission.DefenderAllyTeam);
			if (team2 != null)
			{
				this.SetupAIOfEnemyTeam(team2);
			}
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x00080A3C File Offset: 0x0007EC3C
		protected virtual void SetupAIOfEnemyTeam(Team team)
		{
			foreach (Formation formation in team.FormationsIncludingEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					formation.SetControlledByAI(true, false);
				}
			}
			team.QuerySystem.Expire();
			base.Mission.AllowAiTicking = true;
			base.Mission.ForceTickOccasionally = true;
			team.ResetTactic();
			bool isTeleportingAgents = Mission.Current.IsTeleportingAgents;
			base.Mission.IsTeleportingAgents = true;
			team.Tick(0f);
			base.Mission.IsTeleportingAgents = isTeleportingAgents;
			base.Mission.AllowAiTicking = false;
			base.Mission.ForceTickOccasionally = false;
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x00080B08 File Offset: 0x0007ED08
		private void SetupTeams()
		{
			Utilities.SetLoadingScreenPercentage(0.92f);
			base.Mission.DisableDying = true;
			base.Mission.SetFallAvoidSystemActive(true);
			this.OnSetupTeamsOfSide(this.EnemySide);
			this.SetupAIOfEnemySide(this.EnemySide);
			if (this.IsPlayerAttacker)
			{
				this.HideAgentsOfSide(BattleSideEnum.Defender);
			}
			this.OnSetupTeamsOfSide(this.PlayerSide);
			Agent initialPlayerAgent = base.Mission.InitialPlayerAgent;
			initialPlayerAgent.Controller = AgentControllerType.None;
			initialPlayerAgent.SetIsAIPaused(true);
			initialPlayerAgent.SetDetachableFromFormation(false);
			this.OnSetupTeamsFinished();
			base.Mission.AreOrderGesturesEnabled_AdditionalCondition -= this.AreOrderGesturesEnabled_AdditionalCondition;
			Utilities.SetLoadingScreenPercentage(0.96f);
			if (!MissionGameModels.Current.BattleInitializationModel.CanPlayerSideDeployWithOrderOfBattle())
			{
				this.FinishDeployment();
			}
		}

		// Token: 0x060023DE RID: 9182 RVA: 0x00080BC8 File Offset: 0x0007EDC8
		private void HideAgentsOfSide(BattleSideEnum side)
		{
			foreach (Agent agent in base.Mission.Agents)
			{
				if (agent.IsHuman && agent.Team != null && agent.Team.Side == side)
				{
					agent.SetRenderCheckEnabled(false);
					agent.AgentVisuals.SetVisible(false);
					Agent mountAgent = agent.MountAgent;
					if (mountAgent != null)
					{
						mountAgent.SetRenderCheckEnabled(false);
					}
					Agent mountAgent2 = agent.MountAgent;
					if (mountAgent2 != null)
					{
						mountAgent2.AgentVisuals.SetVisible(false);
					}
				}
			}
		}

		// Token: 0x060023DF RID: 9183 RVA: 0x00080C74 File Offset: 0x0007EE74
		private void UnhideAgentsOfSide(BattleSideEnum side)
		{
			foreach (Agent agent in base.Mission.Agents)
			{
				if (agent.IsHuman && agent.Team != null && agent.Team.Side == side)
				{
					agent.SetRenderCheckEnabled(true);
					agent.AgentVisuals.SetVisible(true);
					Agent mountAgent = agent.MountAgent;
					if (mountAgent != null)
					{
						mountAgent.SetRenderCheckEnabled(true);
					}
					Agent mountAgent2 = agent.MountAgent;
					if (mountAgent2 != null)
					{
						mountAgent2.AgentVisuals.SetVisible(true);
					}
				}
			}
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x00080D20 File Offset: 0x0007EF20
		private bool AreOrderGesturesEnabled_AdditionalCondition()
		{
			return false;
		}

		// Token: 0x04000DBE RID: 3518
		protected readonly bool IsPlayerAttacker;

		// Token: 0x04000DBF RID: 3519
		protected BattleSideEnum PlayerSide;

		// Token: 0x04000DC0 RID: 3520
		protected BattleSideEnum EnemySide;

		// Token: 0x04000DC1 RID: 3521
		protected bool AfterSetupTeamsCalled;
	}
}
