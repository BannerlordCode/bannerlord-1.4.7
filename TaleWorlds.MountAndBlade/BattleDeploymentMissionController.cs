using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Handlers;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000275 RID: 629
	public class BattleDeploymentMissionController : DeploymentMissionController
	{
		// Token: 0x06002339 RID: 9017 RVA: 0x0007CEEB File Offset: 0x0007B0EB
		public BattleDeploymentMissionController(bool isPlayerAttacker)
			: base(isPlayerAttacker)
		{
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x0007CEF4 File Offset: 0x0007B0F4
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._battleDeploymentHandler = base.Mission.GetMissionBehavior<BattleDeploymentHandler>();
			this.MissionAgentSpawnLogic = base.Mission.GetMissionBehavior<DefaultBattleMissionAgentSpawnLogic>();
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x0007CF1E File Offset: 0x0007B11E
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x0007CF28 File Offset: 0x0007B128
		protected override void OnAfterStart()
		{
			for (int i = 0; i < 2; i++)
			{
				this.MissionAgentSpawnLogic.SetSpawnTroops((BattleSideEnum)i, false, false);
			}
			this.MissionAgentSpawnLogic.SetReinforcementsSpawnEnabled(false, true);
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x0007CF5C File Offset: 0x0007B15C
		protected override void OnSetupTeamsOfSide(BattleSideEnum battleSide)
		{
			this.MissionAgentSpawnLogic.SetSpawnTroops(battleSide, true, true);
			base.SetupAgentAIStatesForSide(battleSide);
			this.MissionAgentSpawnLogic.OnSideDeploymentOver(battleSide);
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x0007CF80 File Offset: 0x0007B180
		protected override void OnSetupTeamsFinished()
		{
			base.Mission.IsTeleportingAgents = true;
			foreach (Team team in base.Mission.Teams)
			{
				if (team.GeneralAgent != null)
				{
					WorldPosition worldPosition;
					Vec2 vec;
					base.Mission.GetFormationSpawnFrame(team, FormationClass.NumberOfRegularFormations, false, out worldPosition, out vec, true);
					if (worldPosition.GetNavMesh() != UIntPtr.Zero && worldPosition.IsValid)
					{
						team.GeneralAgent.TrySetFormationFrame(in worldPosition, in vec);
					}
				}
			}
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x0007D024 File Offset: 0x0007B224
		protected override void BeforeDeploymentFinished()
		{
			base.Mission.IsTeleportingAgents = false;
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x0007D032 File Offset: 0x0007B232
		protected override void AfterDeploymentFinished()
		{
			this.MissionAgentSpawnLogic.SetReinforcementsSpawnEnabled(true, true);
			base.Mission.RemoveMissionBehavior(this._battleDeploymentHandler);
		}

		// Token: 0x04000D81 RID: 3457
		protected DefaultBattleMissionAgentSpawnLogic MissionAgentSpawnLogic;

		// Token: 0x04000D82 RID: 3458
		private BattleDeploymentHandler _battleDeploymentHandler;
	}
}
