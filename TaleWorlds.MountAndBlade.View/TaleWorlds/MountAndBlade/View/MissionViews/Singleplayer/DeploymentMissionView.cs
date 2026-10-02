using System;
using TaleWorlds.MountAndBlade.View.MissionViews.Order;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer
{
	// Token: 0x0200008B RID: 139
	public class DeploymentMissionView : MissionView
	{
		// Token: 0x06000530 RID: 1328 RVA: 0x00026512 File Offset: 0x00024712
		public override void AfterStart()
		{
			this._orderTroopPlacer = base.Mission.GetMissionBehavior<OrderTroopPlacer>();
			this._entitySelectionHandler = base.Mission.GetMissionBehavior<MissionEntitySelectionUIHandler>();
			this._deploymentBoundaryMarkerHandler = base.Mission.GetMissionBehavior<MissionDeploymentBoundaryMarker>();
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00026547 File Offset: 0x00024747
		public override void OnDeploymentPlanMade(Team team, bool isFirstPlan)
		{
			if (team == base.Mission.PlayerTeam && base.Mission.DeploymentPlan.HasDeploymentBoundaries(base.Mission.PlayerTeam))
			{
				OrderTroopPlacer orderTroopPlacer = this._orderTroopPlacer;
				if (orderTroopPlacer == null)
				{
					return;
				}
				orderTroopPlacer.RestrictOrdersToDeploymentBoundaries(true);
			}
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00026588 File Offset: 0x00024788
		public override void OnDeploymentFinished()
		{
			if (this._entitySelectionHandler != null)
			{
				base.Mission.RemoveMissionBehavior(this._entitySelectionHandler);
			}
			if (this._deploymentBoundaryMarkerHandler != null)
			{
				if (base.Mission.DeploymentPlan.HasDeploymentBoundaries(base.Mission.PlayerTeam))
				{
					OrderTroopPlacer orderTroopPlacer = this._orderTroopPlacer;
					if (orderTroopPlacer != null)
					{
						orderTroopPlacer.RestrictOrdersToDeploymentBoundaries(false);
					}
				}
				base.Mission.RemoveMissionBehavior(this._deploymentBoundaryMarkerHandler);
			}
			if (!base.Mission.HasMissionBehavior<MissionBoundaryWallView>())
			{
				MissionBoundaryWallView missionBoundaryWallView = new MissionBoundaryWallView();
				base.MissionScreen.AddMissionView(missionBoundaryWallView);
			}
		}

		// Token: 0x040002EA RID: 746
		protected OrderTroopPlacer _orderTroopPlacer;

		// Token: 0x040002EB RID: 747
		protected MissionDeploymentBoundaryMarker _deploymentBoundaryMarkerHandler;

		// Token: 0x040002EC RID: 748
		protected MissionEntitySelectionUIHandler _entitySelectionHandler;
	}
}
