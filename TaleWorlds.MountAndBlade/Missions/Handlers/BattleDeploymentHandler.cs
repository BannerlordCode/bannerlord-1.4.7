using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Missions.Handlers
{
	// Token: 0x020003EC RID: 1004
	public class BattleDeploymentHandler : DeploymentHandler
	{
		// Token: 0x06003712 RID: 14098 RVA: 0x000E3A7A File Offset: 0x000E1C7A
		public BattleDeploymentHandler(bool isPlayerAttacker)
			: base(isPlayerAttacker)
		{
		}

		// Token: 0x06003713 RID: 14099 RVA: 0x000E3A83 File Offset: 0x000E1C83
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
			if (base.PlayerTeam != null)
			{
				base.PlayerTeam.OnOrderIssued -= this.OrderController_OnOrderIssued;
			}
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x000E3AAA File Offset: 0x000E1CAA
		public override void AfterStart()
		{
			base.AfterStart();
			base.PlayerTeam.OnOrderIssued += this.OrderController_OnOrderIssued;
		}

		// Token: 0x06003715 RID: 14101 RVA: 0x000E3ACC File Offset: 0x000E1CCC
		public override void AutoDeployTeamUsingDeploymentPlan(Team team)
		{
			List<Formation> list = team.FormationsIncludingEmpty.ToList<Formation>();
			if (list.Count > 0)
			{
				bool isTeleportingAgents = base.Mission.IsTeleportingAgents;
				base.Mission.IsTeleportingAgents = true;
				OrderController orderController = (team.IsPlayerTeam ? team.PlayerOrderController : team.MasterOrderController);
				orderController.SelectAllFormations(false);
				this.SetDefaultFormationOrders(orderController);
				orderController.ClearSelectedFormations();
				IMissionDeploymentPlan deploymentPlan = base.Mission.DeploymentPlan;
				if (deploymentPlan.IsPlanMade(team))
				{
					using (List<Formation>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Formation formation = enumerator.Current;
							IFormationDeploymentPlan formationPlan = deploymentPlan.GetFormationPlan(team, formation.FormationIndex, false);
							WorldPosition worldPosition;
							Vec2 vec;
							base.Mission.GetFormationSpawnFrame(formation.Team, formation.FormationIndex, false, out worldPosition, out vec, true);
							if (formationPlan.HasDimensions)
							{
								formation.SetFormOrder(FormOrder.FormOrderCustom(formationPlan.PlannedWidth), true);
							}
							formation.SetMovementOrder(MovementOrder.MovementOrderMove(worldPosition));
							formation.SetFacingOrder(FacingOrder.FacingOrderLookAtDirection(vec));
							formation.SetPositioning(new WorldPosition?(worldPosition), new Vec2?(vec), new int?(formation.ArrangementOrder.GetUnitSpacing()));
							formation.ApplyActionOnEachUnit(delegate(Agent agent)
							{
								agent.ForceUpdateCachedAndFormationValues(true, false);
							}, null);
							formation.SetHasPendingUnitPositions(false);
							formation.SetMovementOrder(MovementOrder.MovementOrderStop);
						}
						goto IL_0189;
					}
				}
				Debug.FailedAssert("Failed to deploy team. Initial deployment plan is not made yet.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\MissionLogics\\BattleDeploymentHandler.cs", "AutoDeployTeamUsingDeploymentPlan", 84);
				IL_0189:
				foreach (Formation formation2 in list)
				{
					formation2.ApplyActionOnEachUnit(delegate(Agent agent)
					{
						agent.ForceUpdateCachedAndFormationValues(true, false);
					}, null);
					formation2.SetHasPendingUnitPositions(false);
				}
				base.Mission.IsTeleportingAgents = isTeleportingAgents;
			}
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x000E3CE0 File Offset: 0x000E1EE0
		public override void ForceUpdateAllUnits()
		{
			DeploymentHandler.OrderController_OnOrderIssued_Aux(OrderType.Move, base.PlayerTeam.FormationsIncludingSpecialAndEmpty, null, Array.Empty<object>());
		}

		// Token: 0x06003717 RID: 14103 RVA: 0x000E3CFC File Offset: 0x000E1EFC
		public void SetDefaultFormationOrders(OrderController orderController)
		{
			orderController.SetOrder(OrderType.AIControlOff);
			orderController.SetFormationUpdateEnabledAfterSetOrder(false);
			orderController.SetOrder(OrderType.Mount);
			orderController.SetOrder(OrderType.FireAtWill);
			orderController.SetOrder(OrderType.ArrangementLine);
			orderController.SetOrder(OrderType.StandYourGround);
			orderController.SetOrder((base.Mission.IsSiegeBattle || base.Mission.IsSallyOutBattle) ? OrderType.AIControlOn : OrderType.AIControlOff);
			orderController.SetFormationUpdateEnabledAfterSetOrder(true);
		}

		// Token: 0x06003718 RID: 14104 RVA: 0x000E3D64 File Offset: 0x000E1F64
		private void OrderController_OnOrderIssued(OrderType orderType, MBReadOnlyList<Formation> appliedFormations, OrderController orderController, params object[] delegateParams)
		{
			DeploymentHandler.OrderController_OnOrderIssued_Aux(orderType, appliedFormations, orderController, delegateParams);
		}
	}
}
