using System;
using System.Diagnostics;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Source.Missions
{
	// Token: 0x020003CE RID: 974
	public abstract class BaseBattleMissionController : MissionLogic
	{
		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x06003639 RID: 13881 RVA: 0x000E01C7 File Offset: 0x000DE3C7
		// (set) Token: 0x0600363A RID: 13882 RVA: 0x000E01CF File Offset: 0x000DE3CF
		private protected bool IsPlayerAttacker { protected get; private set; }

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x0600363B RID: 13883 RVA: 0x000E01D8 File Offset: 0x000DE3D8
		// (set) Token: 0x0600363C RID: 13884 RVA: 0x000E01E0 File Offset: 0x000DE3E0
		private protected int DeployedAttackerTroopCount { protected get; private set; }

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x0600363D RID: 13885 RVA: 0x000E01E9 File Offset: 0x000DE3E9
		// (set) Token: 0x0600363E RID: 13886 RVA: 0x000E01F1 File Offset: 0x000DE3F1
		private protected int DeployedDefenderTroopCount { protected get; private set; }

		// Token: 0x0600363F RID: 13887 RVA: 0x000E01FA File Offset: 0x000DE3FA
		protected BaseBattleMissionController(bool isPlayerAttacker)
		{
			this.IsPlayerAttacker = isPlayerAttacker;
			this.game = Game.Current;
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x000E0214 File Offset: 0x000DE414
		public override void EarlyStart()
		{
			this.EarlyStart();
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x000E021C File Offset: 0x000DE41C
		public override void AfterStart()
		{
			base.AfterStart();
			this.CreateTeams();
			base.Mission.SetMissionMode(MissionMode.Battle, true);
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x000E0237 File Offset: 0x000DE437
		protected virtual void SetupTeam(Team team)
		{
			if (team.Side == BattleSideEnum.Attacker)
			{
				this.CreateAttackerTroops();
			}
			else
			{
				this.CreateDefenderTroops();
			}
			if (team == base.Mission.PlayerTeam)
			{
				this.CreatePlayer();
			}
		}

		// Token: 0x06003643 RID: 13891
		protected abstract void CreateDefenderTroops();

		// Token: 0x06003644 RID: 13892
		protected abstract void CreateAttackerTroops();

		// Token: 0x06003645 RID: 13893 RVA: 0x000E0264 File Offset: 0x000DE464
		public virtual TeamAIComponent GetTeamAI(Team team, float thinkTimerTime = 5f, float applyTimerTime = 1f)
		{
			return new TeamAIGeneral(base.Mission, team, thinkTimerTime, applyTimerTime);
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x000E0274 File Offset: 0x000DE474
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x000E027D File Offset: 0x000DE47D
		[Conditional("DEBUG")]
		private void DebugTick()
		{
			if (Input.DebugInput.IsHotKeyPressed("SwapToEnemy"))
			{
				this.BecomeEnemy();
			}
			if (Input.DebugInput.IsHotKeyDown("BaseBattleMissionControllerHotkeyBecomePlayer"))
			{
				this.BecomePlayer();
			}
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x000E02AD File Offset: 0x000DE4AD
		protected bool IsPlayerDead()
		{
			return base.Mission.MainAgent == null || !base.Mission.MainAgent.IsActive();
		}

		// Token: 0x06003649 RID: 13897 RVA: 0x000E02D4 File Offset: 0x000DE4D4
		public override bool MissionEnded(ref MissionResult missionResult)
		{
			if (!base.Mission.IsDeploymentFinished)
			{
				return false;
			}
			if (this.IsPlayerDead())
			{
				missionResult = MissionResult.CreateDefeated(base.Mission);
				return true;
			}
			if (base.Mission.GetMemberCountOfSide(BattleSideEnum.Attacker) == 0)
			{
				missionResult = ((base.Mission.PlayerTeam.Side == BattleSideEnum.Attacker) ? MissionResult.CreateDefeated(base.Mission) : MissionResult.CreateSuccessful(base.Mission, false));
				return true;
			}
			if (base.Mission.GetMemberCountOfSide(BattleSideEnum.Defender) == 0)
			{
				missionResult = ((base.Mission.PlayerTeam.Side == BattleSideEnum.Attacker) ? MissionResult.CreateSuccessful(base.Mission, false) : MissionResult.CreateDefeated(base.Mission));
				return true;
			}
			return false;
		}

		// Token: 0x0600364A RID: 13898 RVA: 0x000E0384 File Offset: 0x000DE584
		public override InquiryData OnEndMissionRequest(out bool canPlayerLeave)
		{
			canPlayerLeave = true;
			if (!this.IsPlayerDead() && base.Mission.IsPlayerCloseToAnEnemy(5f))
			{
				canPlayerLeave = false;
				MBInformationManager.AddQuickInformation(GameTexts.FindText("str_can_not_retreat", null), 0, null, null, "");
			}
			else
			{
				MissionResult missionResult = null;
				if (!this.IsPlayerDead() && !this.MissionEnded(ref missionResult))
				{
					return new InquiryData("", GameTexts.FindText("str_retreat_question", null).ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action(base.Mission.OnEndMissionResult), null, "", 0f, null, null, null);
				}
			}
			return null;
		}

		// Token: 0x0600364B RID: 13899 RVA: 0x000E043C File Offset: 0x000DE63C
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
		}

		// Token: 0x0600364C RID: 13900 RVA: 0x000E0440 File Offset: 0x000DE640
		private void CreateTeams()
		{
			if (!base.Mission.Teams.IsEmpty<Team>())
			{
				throw new MBIllegalValueException("Number of teams is not 0.");
			}
			base.Mission.Teams.Add(BattleSideEnum.Defender, 4278190335U, 4278190335U, null, true, false, true);
			base.Mission.Teams.Add(BattleSideEnum.Attacker, 4278255360U, 4278255360U, null, true, false, true);
			if (this.IsPlayerAttacker)
			{
				base.Mission.PlayerTeam = base.Mission.AttackerTeam;
			}
			else
			{
				base.Mission.PlayerTeam = base.Mission.DefenderTeam;
			}
			TeamAIComponent teamAI = this.GetTeamAI(base.Mission.DefenderTeam, 5f, 1f);
			base.Mission.DefenderTeam.AddTeamAI(teamAI, false);
			TeamAIComponent teamAI2 = this.GetTeamAI(base.Mission.AttackerTeam, 5f, 1f);
			base.Mission.AttackerTeam.AddTeamAI(teamAI2, false);
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x000E053C File Offset: 0x000DE73C
		protected void IncrementDeploymedTroops(BattleSideEnum side)
		{
			int num;
			if (side == BattleSideEnum.Attacker)
			{
				num = this.DeployedAttackerTroopCount;
				this.DeployedAttackerTroopCount = num + 1;
				return;
			}
			num = this.DeployedDefenderTroopCount;
			this.DeployedDefenderTroopCount = num + 1;
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x000E0570 File Offset: 0x000DE770
		protected virtual void CreatePlayer()
		{
			this.game.PlayerTroop = Game.Current.ObjectManager.GetObject<BasicCharacterObject>("main_hero");
			FormationClass formationClass = base.Mission.GetFormationSpawnClass(base.Mission.PlayerTeam, FormationClass.NumberOfRegularFormations, false);
			if (formationClass != FormationClass.NumberOfRegularFormations)
			{
				formationClass = this.game.PlayerTroop.DefaultFormationClass;
			}
			WorldPosition worldPosition;
			Vec2 vec;
			base.Mission.GetFormationSpawnFrame(base.Mission.PlayerTeam, formationClass, false, out worldPosition, out vec, true);
			Mission mission = base.Mission;
			AgentBuildData agentBuildData = new AgentBuildData(this.game.PlayerTroop).Team(base.Mission.PlayerTeam);
			Vec3 groundVec = worldPosition.GetGroundVec3();
			Agent agent = mission.SpawnAgent(agentBuildData.InitialPosition(in groundVec).InitialDirection(in vec).Controller(AgentControllerType.Player), false);
			agent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
			base.Mission.MainAgent = agent;
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x000E0645 File Offset: 0x000DE845
		protected void BecomeEnemy()
		{
			base.Mission.MainAgent.Controller = AgentControllerType.AI;
			base.Mission.PlayerEnemyTeam.Leader.Controller = AgentControllerType.Player;
			this.SwapTeams();
		}

		// Token: 0x06003650 RID: 13904 RVA: 0x000E0674 File Offset: 0x000DE874
		protected void BecomePlayer()
		{
			base.Mission.MainAgent.Controller = AgentControllerType.Player;
			base.Mission.PlayerEnemyTeam.Leader.Controller = AgentControllerType.AI;
			this.SwapTeams();
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x000E06A3 File Offset: 0x000DE8A3
		protected void SwapTeams()
		{
			base.Mission.PlayerTeam = base.Mission.PlayerEnemyTeam;
			this.IsPlayerAttacker = !this.IsPlayerAttacker;
		}

		// Token: 0x04001750 RID: 5968
		protected readonly Game game;
	}
}
