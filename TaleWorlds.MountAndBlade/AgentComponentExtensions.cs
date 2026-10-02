using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000107 RID: 263
	public static class AgentComponentExtensions
	{
		// Token: 0x06000D4A RID: 3402 RVA: 0x00018374 File Offset: 0x00016574
		public static float GetMorale(this Agent agent)
		{
			CommonAIComponent commonAIComponent = agent.CommonAIComponent;
			if (commonAIComponent != null)
			{
				return commonAIComponent.Morale;
			}
			return -1f;
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00018398 File Offset: 0x00016598
		public static void SetMorale(this Agent agent, float morale)
		{
			CommonAIComponent commonAIComponent = agent.CommonAIComponent;
			if (commonAIComponent != null)
			{
				commonAIComponent.Morale = morale;
			}
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x000183B8 File Offset: 0x000165B8
		public static void ChangeMorale(this Agent agent, float delta)
		{
			CommonAIComponent commonAIComponent = agent.CommonAIComponent;
			if (commonAIComponent != null)
			{
				commonAIComponent.Morale += delta;
			}
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x000183E0 File Offset: 0x000165E0
		public static bool IsRetreating(this Agent agent, bool isComponentAssured = true)
		{
			CommonAIComponent commonAIComponent = agent.CommonAIComponent;
			return commonAIComponent != null && commonAIComponent.IsRetreating;
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x000183FF File Offset: 0x000165FF
		public static void Retreat(this Agent agent, bool useCachingSystem = false)
		{
			CommonAIComponent commonAIComponent = agent.CommonAIComponent;
			if (commonAIComponent == null)
			{
				return;
			}
			commonAIComponent.Retreat(useCachingSystem);
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00018412 File Offset: 0x00016612
		public static void StopRetreatingMoraleComponent(this Agent agent)
		{
			CommonAIComponent commonAIComponent = agent.CommonAIComponent;
			if (commonAIComponent == null)
			{
				return;
			}
			commonAIComponent.StopRetreating();
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00018424 File Offset: 0x00016624
		public static void SetBehaviorValueSet(this Agent agent, HumanAIComponent.BehaviorValueSet behaviorValueSet)
		{
			HumanAIComponent humanAIComponent = agent.HumanAIComponent;
			if (humanAIComponent == null)
			{
				return;
			}
			humanAIComponent.SetBehaviorValueSet(behaviorValueSet);
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00018437 File Offset: 0x00016637
		public static void RefreshBehaviorValues(this Agent agent, MovementOrder.MovementOrderEnum movementOrder, ArrangementOrder.ArrangementOrderEnum arrangementOrder)
		{
			HumanAIComponent humanAIComponent = agent.HumanAIComponent;
			if (humanAIComponent == null)
			{
				return;
			}
			humanAIComponent.RefreshBehaviorValues(movementOrder, arrangementOrder);
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0001844B File Offset: 0x0001664B
		public static void SetAIBehaviorValues(this Agent agent, HumanAIComponent.AISimpleBehaviorKind behavior, float y1, float x2, float y2, float x3, float y3)
		{
			HumanAIComponent humanAIComponent = agent.HumanAIComponent;
			if (humanAIComponent == null)
			{
				return;
			}
			humanAIComponent.OverrideBehaviorParams(behavior, y1, x2, y2, x3, y3);
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00018466 File Offset: 0x00016666
		public static void AIMoveToGameObjectEnable(this Agent agent, UsableMissionObject usedObject, IDetachment detachment, Agent.AIScriptedFrameFlags scriptedFrameFlags = Agent.AIScriptedFrameFlags.NoAttack)
		{
			agent.HumanAIComponent.MoveToUsableGameObject(usedObject, detachment, scriptedFrameFlags);
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00018476 File Offset: 0x00016676
		public static void AIMoveToGameObjectDisable(this Agent agent)
		{
			agent.HumanAIComponent.MoveToClear();
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00018483 File Offset: 0x00016683
		public static bool AIMoveToGameObjectIsEnabled(this Agent agent)
		{
			return agent.AIStateFlags.HasAnyFlag(Agent.AIStateFlag.UseObjectMoving);
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00018492 File Offset: 0x00016692
		public static void AIDefendGameObjectEnable(this Agent agent, UsableMissionObject usedObject, IDetachment detachment)
		{
			agent.HumanAIComponent.StartDefendingGameObject(usedObject, detachment);
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x000184A1 File Offset: 0x000166A1
		public static void AIDefendGameObjectDisable(this Agent agent)
		{
			agent.HumanAIComponent.StopDefendingGameObject();
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x000184AE File Offset: 0x000166AE
		public static bool AIDefendGameObjectIsEnabled(this Agent agent)
		{
			return agent.HumanAIComponent.IsDefending;
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x000184BB File Offset: 0x000166BB
		public static bool AIInterestedInAnyGameObject(this Agent agent)
		{
			return agent.HumanAIComponent.IsInterestedInAnyGameObject();
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x000184C8 File Offset: 0x000166C8
		public static bool AIInterestedInGameObject(this Agent agent, UsableMissionObject usableMissionObject)
		{
			return agent.HumanAIComponent.IsInterestedInGameObject(usableMissionObject);
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x000184D6 File Offset: 0x000166D6
		public static void AIUseGameObjectEnable(this Agent agent)
		{
			agent.AIStateFlags |= Agent.AIStateFlag.UseObjectUsing;
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x000184E7 File Offset: 0x000166E7
		public static void AIUseGameObjectDisable(this Agent agent)
		{
			agent.AIStateFlags &= ~Agent.AIStateFlag.UseObjectUsing;
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x000184F8 File Offset: 0x000166F8
		public static bool AIUseGameObjectIsEnabled(this Agent agent)
		{
			return agent.AIStateFlags.HasAnyFlag(Agent.AIStateFlag.UseObjectUsing);
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x00018507 File Offset: 0x00016707
		public static Agent GetFollowedUnit(this Agent agent)
		{
			HumanAIComponent humanAIComponent = agent.HumanAIComponent;
			if (humanAIComponent == null)
			{
				return null;
			}
			return humanAIComponent.FollowedAgent;
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0001851A File Offset: 0x0001671A
		public static void SetFollowedUnit(this Agent agent, Agent followedUnit)
		{
			agent.HumanAIComponent.FollowAgent(followedUnit);
		}
	}
}
