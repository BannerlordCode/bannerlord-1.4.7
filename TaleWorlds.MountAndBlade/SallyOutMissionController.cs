using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000297 RID: 663
	public abstract class SallyOutMissionController : MissionLogic
	{
		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x060024AD RID: 9389 RVA: 0x00084F42 File Offset: 0x00083142
		private float BesiegedDeploymentDuration
		{
			get
			{
				return 55f;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x060024AE RID: 9390 RVA: 0x00084F49 File Offset: 0x00083149
		private float BesiegerActivationDuration
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x060024AF RID: 9391 RVA: 0x00084F50 File Offset: 0x00083150
		public MBReadOnlyList<SiegeWeapon> BesiegerSiegeEngines
		{
			get
			{
				return this._besiegerSiegeEngines;
			}
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x00084F58 File Offset: 0x00083158
		public SallyOutMissionController(bool isSallyOutAmbush)
		{
			this._isSallyOutAmbush = isSallyOutAmbush;
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x00084F67 File Offset: 0x00083167
		public override void OnBehaviorInitialize()
		{
			this.MissionAgentSpawnLogic = base.Mission.GetMissionBehavior<DefaultBattleMissionAgentSpawnLogic>();
			this._sallyOutNotificationsHandler = new SallyOutMissionNotificationsHandler(this.MissionAgentSpawnLogic, this);
			Mission.Current.GetOverriddenFleePositionForAgent += this.GetSallyOutFleePositionForAgent;
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x00084FA4 File Offset: 0x000831A4
		public override void AfterStart()
		{
			this._sallyOutNotificationsHandler.OnAfterStart();
			int num;
			int num2;
			this.GetInitialTroopCounts(out num, out num2);
			this.SetupInitialSpawn(num, num2);
			this._castleGates = base.Mission.MissionObjects.FindAllWithType<CastleGate>().ToList<CastleGate>();
			this._besiegedDeploymentTimer = new BasicMissionTimer();
			TeamAIComponent teamAI = base.Mission.DefenderTeam.TeamAI;
			teamAI.OnNotifyTacticalDecision = (TeamAIComponent.TacticalDecisionDelegate)Delegate.Combine(teamAI.OnNotifyTacticalDecision, new TeamAIComponent.TacticalDecisionDelegate(this.OnDefenderTeamTacticalDecision));
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x00085025 File Offset: 0x00083225
		public override void OnMissionTick(float dt)
		{
			this._sallyOutNotificationsHandler.OnMissionTick(dt);
			this.UpdateTimers();
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x0008503C File Offset: 0x0008323C
		public override void OnDeploymentFinished()
		{
			this._besiegerSiegeEngines = SallyOutMissionController.GetBesiegerSiegeEngines();
			SallyOutMissionController.DisableSiegeEngines();
			if (this._isSallyOutAmbush)
			{
				Mission.Current.AddMissionBehavior(new SallyOutEndLogic());
			}
			this._sallyOutNotificationsHandler.OnDeploymentFinished();
			this._besiegerActivationTimer = new BasicMissionTimer();
			this.DeactivateBesiegers();
			this.ActivateDefenders();
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x00085092 File Offset: 0x00083292
		protected override void OnEndMission()
		{
			this._sallyOutNotificationsHandler.OnMissionEnd();
			Mission.Current.GetOverriddenFleePositionForAgent -= this.GetSallyOutFleePositionForAgent;
		}

		// Token: 0x060024B6 RID: 9398
		protected abstract void GetInitialTroopCounts(out int besiegedTotalTroopCount, out int besiegerTotalTroopCount);

		// Token: 0x060024B7 RID: 9399 RVA: 0x000850B8 File Offset: 0x000832B8
		private void UpdateTimers()
		{
			if (this._besiegedDeploymentTimer != null)
			{
				if (this._besiegedDeploymentTimer.ElapsedTime >= this.BesiegedDeploymentDuration)
				{
					foreach (CastleGate castleGate in this._castleGates)
					{
						castleGate.SetAutoOpenState(true);
					}
					this._besiegedDeploymentTimer = null;
					goto IL_012B;
				}
				using (List<CastleGate>.Enumerator enumerator = this._castleGates.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CastleGate castleGate2 = enumerator.Current;
						if (!castleGate2.IsDestroyed && !castleGate2.IsGateOpen)
						{
							castleGate2.OpenDoor();
						}
					}
					goto IL_012B;
				}
			}
			Agent mainAgent = base.Mission.MainAgent;
			if (mainAgent != null && mainAgent.IsActive())
			{
				Vec3 eyeGlobalPosition = mainAgent.GetEyeGlobalPosition();
				foreach (CastleGate castleGate3 in this._castleGates)
				{
					if (!castleGate3.IsDestroyed && !castleGate3.IsGateOpen && eyeGlobalPosition.DistanceSquared(castleGate3.GameEntity.GlobalPosition) <= 25f)
					{
						castleGate3.OpenDoor();
					}
				}
			}
			IL_012B:
			if (this._besiegerActivationTimer != null && this._besiegerActivationTimer.ElapsedTime >= this.BesiegerActivationDuration)
			{
				this.ActivateBesiegers();
				this._besiegerActivationTimer = null;
			}
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x00085240 File Offset: 0x00083440
		private void ActivateDefenders()
		{
			if (base.Mission.DefenderAllyTeam != null)
			{
				foreach (Agent agent in base.Mission.DefenderAllyTeam.ActiveAgents.ToList<Agent>())
				{
					FormationClass formationIndex = agent.Formation.FormationIndex;
					agent.SetTeam(base.Mission.DefenderTeam, true);
					agent.Formation = base.Mission.DefenderTeam.GetFormation(formationIndex);
				}
			}
			foreach (Formation formation in base.Mission.DefenderTeam.FormationsIncludingSpecialAndEmpty)
			{
				formation.SetMovementOrder(MovementOrder.MovementOrderCharge);
			}
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x0008532C File Offset: 0x0008352C
		private void AdjustTotalTroopCounts(ref int besiegedTotalTroopCount, ref int besiegerTotalTroopCount)
		{
			float num = 0.25f;
			float num2 = 1f - num;
			int num3 = (int)((float)this.MissionAgentSpawnLogic.BattleSize * num);
			int num4 = (int)((float)this.MissionAgentSpawnLogic.BattleSize * num2);
			besiegedTotalTroopCount = MathF.Min(besiegedTotalTroopCount, num3);
			besiegerTotalTroopCount = MathF.Min(besiegerTotalTroopCount, num4);
			float num5 = num2 / num;
			if ((float)besiegerTotalTroopCount / (float)besiegedTotalTroopCount <= num5)
			{
				int num6 = (int)((float)besiegerTotalTroopCount / num5);
				besiegedTotalTroopCount = MathF.Min(num6, besiegedTotalTroopCount);
				return;
			}
			int num7 = (int)((float)besiegedTotalTroopCount * num5);
			besiegerTotalTroopCount = MathF.Min(num7, besiegerTotalTroopCount);
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x000853B4 File Offset: 0x000835B4
		private void SetupInitialSpawn(int besiegedTotalTroopCount, int besiegerTotalTroopCount)
		{
			this.AdjustTotalTroopCounts(ref besiegedTotalTroopCount, ref besiegerTotalTroopCount);
			int num = besiegedTotalTroopCount + besiegerTotalTroopCount;
			int num2 = MathF.Min(besiegedTotalTroopCount, MathF.Ceiling((float)num * 0.1f));
			int num3 = MathF.Min(besiegerTotalTroopCount, MathF.Ceiling((float)num * 0.1f));
			this.MissionAgentSpawnLogic.SetSpawnHorses(BattleSideEnum.Defender, true);
			this.MissionAgentSpawnLogic.SetSpawnHorses(BattleSideEnum.Attacker, false);
			MissionSpawnSettings missionSpawnSettings = SallyOutMissionController.CreateSallyOutSpawnSettings(0.01f, 0.1f);
			this.MissionAgentSpawnLogic.InitWithSinglePhase(besiegedTotalTroopCount, besiegerTotalTroopCount, num2, num3, false, false, in missionSpawnSettings);
			this.MissionAgentSpawnLogic.SetCustomReinforcementSpawnTimer(new SallyOutReinforcementSpawnTimer(1f, 90f, 15f, 5));
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x00085454 File Offset: 0x00083654
		private WorldPosition? GetSallyOutFleePositionForAgent(Agent agent)
		{
			if (!agent.IsHuman)
			{
				return null;
			}
			Formation formation = agent.Formation;
			if (formation == null || formation.Team.Side == BattleSideEnum.Attacker)
			{
				return null;
			}
			bool flag = !agent.HasMount;
			bool isRangedCached = agent.IsRangedCached;
			FormationClass formationClass;
			if (flag)
			{
				formationClass = (isRangedCached ? FormationClass.Ranged : FormationClass.Infantry);
			}
			else
			{
				formationClass = (isRangedCached ? FormationClass.HorseArcher : FormationClass.Cavalry);
			}
			return new WorldPosition?(Mission.Current.DeploymentPlan.GetFormationPlan(formation.Team, formationClass, false).CreateNewDeploymentWorldPosition(WorldPosition.WorldPositionEnforcedCache.GroundVec3));
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x000854DC File Offset: 0x000836DC
		private static MissionSpawnSettings CreateSallyOutSpawnSettings(float besiegedReinforcementPercentage, float besiegerReinforcementPercentage)
		{
			return new MissionSpawnSettings(MissionSpawnSettings.InitialSpawnMethod.FreeAllocation, MissionSpawnSettings.ReinforcementTimingMethod.CustomTimer, MissionSpawnSettings.ReinforcementSpawnMethod.Fixed, 0f, 0f, 0f, 0f, 0, besiegedReinforcementPercentage, besiegerReinforcementPercentage, 1f, 0.75f);
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x00085514 File Offset: 0x00083714
		private void OnDefenderTeamTacticalDecision(in TacticalDecision decision)
		{
			TacticalDecision tacticalDecision = decision;
			if (tacticalDecision.DecisionCode == 31)
			{
				this._sallyOutNotificationsHandler.OnBesiegedSideFallsbackToKeep();
			}
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x00085540 File Offset: 0x00083740
		private void DeactivateBesiegers()
		{
			foreach (Formation formation in base.Mission.AttackerTeam.FormationsIncludingSpecialAndEmpty)
			{
				formation.SetMovementOrder(MovementOrder.MovementOrderStop);
				formation.SetFiringOrder(FiringOrder.FiringOrderHoldYourFire);
				formation.SetControlledByAI(false, false);
			}
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x000855B4 File Offset: 0x000837B4
		private void ActivateBesiegers()
		{
			Team attackerTeam = base.Mission.AttackerTeam;
			foreach (Formation formation in base.Mission.AttackerTeam.FormationsIncludingSpecialAndEmpty)
			{
				formation.SetControlledByAI(true, false);
			}
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x0008561C File Offset: 0x0008381C
		public static MBReadOnlyList<SiegeWeapon> GetBesiegerSiegeEngines()
		{
			MBList<SiegeWeapon> mblist = new MBList<SiegeWeapon>();
			using (List<MissionObject>.Enumerator enumerator = Mission.Current.ActiveMissionObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SiegeWeapon siegeWeapon;
					if ((siegeWeapon = enumerator.Current as SiegeWeapon) != null && siegeWeapon.DestructionComponent != null && siegeWeapon.Side == BattleSideEnum.Attacker)
					{
						mblist.Add(siegeWeapon);
					}
				}
			}
			return mblist;
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x00085694 File Offset: 0x00083894
		public static void DisableSiegeEngines()
		{
			for (int i = Mission.Current.ActiveMissionObjects.Count - 1; i >= 0; i--)
			{
				SiegeWeapon siegeWeapon;
				if ((siegeWeapon = Mission.Current.ActiveMissionObjects[i] as SiegeWeapon) != null && siegeWeapon.DestructionComponent != null && !siegeWeapon.IsDeactivated)
				{
					siegeWeapon.Disable();
					siegeWeapon.Deactivate();
				}
			}
		}

		// Token: 0x04000E28 RID: 3624
		private const float BesiegedTotalTroopRatio = 0.25f;

		// Token: 0x04000E29 RID: 3625
		private const float BesiegedInitialTroopRatio = 0.1f;

		// Token: 0x04000E2A RID: 3626
		private const float BesiegedReinforcementRatio = 0.01f;

		// Token: 0x04000E2B RID: 3627
		private const float BesiegerInitialTroopRatio = 0.1f;

		// Token: 0x04000E2C RID: 3628
		private const float BesiegerReinforcementRatio = 0.1f;

		// Token: 0x04000E2D RID: 3629
		private const float BesiegedInitialInterval = 1f;

		// Token: 0x04000E2E RID: 3630
		private const float BesiegerInitialInterval = 90f;

		// Token: 0x04000E2F RID: 3631
		private const float BesiegerIntervalChange = 15f;

		// Token: 0x04000E30 RID: 3632
		private const int BesiegerIntervalChangeCount = 5;

		// Token: 0x04000E31 RID: 3633
		private const float PlayerToGateSquaredDistanceThreshold = 25f;

		// Token: 0x04000E32 RID: 3634
		private SallyOutMissionNotificationsHandler _sallyOutNotificationsHandler;

		// Token: 0x04000E33 RID: 3635
		private List<CastleGate> _castleGates;

		// Token: 0x04000E34 RID: 3636
		private BasicMissionTimer _besiegedDeploymentTimer;

		// Token: 0x04000E35 RID: 3637
		private BasicMissionTimer _besiegerActivationTimer;

		// Token: 0x04000E36 RID: 3638
		private MBReadOnlyList<SiegeWeapon> _besiegerSiegeEngines;

		// Token: 0x04000E37 RID: 3639
		protected DefaultBattleMissionAgentSpawnLogic MissionAgentSpawnLogic;

		// Token: 0x04000E38 RID: 3640
		private bool _isSallyOutAmbush;
	}
}
