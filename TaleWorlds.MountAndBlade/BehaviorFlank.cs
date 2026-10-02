using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000118 RID: 280
	public class BehaviorFlank : BehaviorComponent
	{
		// Token: 0x06000E21 RID: 3617 RVA: 0x0001F689 File Offset: 0x0001D889
		public BehaviorFlank(Formation formation)
			: base(formation)
		{
			base.BehaviorCoherence = 0.5f;
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0001F6A4 File Offset: 0x0001D8A4
		protected override void CalculateCurrentOrder()
		{
			WorldPosition worldPosition = ((base.Formation.AI.Side == FormationAI.BehaviorSide.Right) ? base.Formation.QuerySystem.Team.RightFlankEdgePosition : base.Formation.QuerySystem.Team.LeftFlankEdgePosition);
			Vec2 vec = (worldPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized();
			base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x0001F729 File Offset: 0x0001D929
		public override void TickOccasionally()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0001F754 File Offset: 0x0001D954
		public override TextObject GetBehaviorString()
		{
			TextObject behaviorString = base.GetBehaviorString();
			behaviorString.SetTextVariable("IS_GENERAL_SIDE", "0");
			TextObject textObject = GameTexts.FindText("str_formation_ai_side_strings", base.Formation.AI.Side.ToString());
			behaviorString.SetTextVariable("SIDE_STRING", textObject);
			return behaviorString;
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0001F7B0 File Offset: 0x0001D9B0
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0001F818 File Offset: 0x0001DA18
		protected override float GetAiWeight()
		{
			FormationQuerySystem querySystem = base.Formation.QuerySystem;
			FormationQuerySystem cachedClosestEnemyFormation = base.Formation.CachedClosestEnemyFormation;
			if (cachedClosestEnemyFormation == null || cachedClosestEnemyFormation.Formation.CachedClosestEnemyFormation == querySystem)
			{
				return 0f;
			}
			Vec2 vec = (cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized();
			Vec2 vec2 = (cachedClosestEnemyFormation.Formation.CachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2 - cachedClosestEnemyFormation.Formation.CachedMedianPosition.AsVec2).Normalized();
			if (vec.DotProduct(vec2) > -0.5f)
			{
				return 0f;
			}
			if (Mission.Current.MissionTeamAIType != Mission.MissionTeamAITypeEnum.FieldBattle)
			{
				Vec3 navMeshVec = ((base.Formation.AI.Side == FormationAI.BehaviorSide.Right) ? base.Formation.QuerySystem.Team.RightFlankEdgePosition : base.Formation.QuerySystem.Team.LeftFlankEdgePosition).GetNavMeshVec3();
				int num;
				Mission.Current.Scene.GetNavigationMeshForPosition(in navMeshVec, out num, 1.5f, false);
				if (num >= 0)
				{
					Agent medianAgent = base.Formation.GetMedianAgent(true, true, base.Formation.CachedAveragePosition);
					if ((medianAgent != null && medianAgent.GetCurrentNavigationFaceId() % 10 == 1) == (num % 10 == 1))
					{
						goto IL_0166;
					}
				}
				return 0f;
			}
			IL_0166:
			return 1.2f;
		}
	}
}
