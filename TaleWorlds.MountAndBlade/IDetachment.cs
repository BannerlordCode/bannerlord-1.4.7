using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200014A RID: 330
	public interface IDetachment
	{
		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06001111 RID: 4369
		MBReadOnlyList<Formation> UserFormations { get; }

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06001112 RID: 4370
		bool IsLoose { get; }

		// Token: 0x06001113 RID: 4371
		bool IsAgentUsingOrInterested(Agent agent);

		// Token: 0x06001114 RID: 4372
		float? GetWeightOfNextSlot(BattleSideEnum side);

		// Token: 0x06001115 RID: 4373
		float GetDetachmentWeight(BattleSideEnum side);

		// Token: 0x06001116 RID: 4374
		float ComputeAndCacheDetachmentWeight(BattleSideEnum side);

		// Token: 0x06001117 RID: 4375
		float GetDetachmentWeightFromCache();

		// Token: 0x06001118 RID: 4376
		void GetSlotIndexWeightTuples(List<ValueTuple<int, float>> slotIndexWeightTuples);

		// Token: 0x06001119 RID: 4377
		bool IsSlotAtIndexAvailableForAgent(int slotIndex, Agent agent);

		// Token: 0x0600111A RID: 4378
		bool IsAgentEligible(Agent agent);

		// Token: 0x0600111B RID: 4379
		void AddAgentAtSlotIndex(Agent agent, int slotIndex);

		// Token: 0x0600111C RID: 4380
		Agent GetMovingAgentAtSlotIndex(int slotIndex);

		// Token: 0x0600111D RID: 4381
		void MarkSlotAtIndex(int slotIndex);

		// Token: 0x0600111E RID: 4382
		bool IsDetachmentRecentlyEvaluated();

		// Token: 0x0600111F RID: 4383
		void UnmarkDetachment();

		// Token: 0x06001120 RID: 4384
		float? GetWeightOfAgentAtNextSlot(List<Agent> candidates, out Agent match);

		// Token: 0x06001121 RID: 4385
		float? GetWeightOfAgentAtNextSlot(List<ValueTuple<Agent, float>> agentTemplateScores, out Agent match);

		// Token: 0x06001122 RID: 4386
		float GetTemplateWeightOfAgent(Agent candidate);

		// Token: 0x06001123 RID: 4387
		List<float> GetTemplateCostsOfAgent(Agent candidate, List<float> oldValue);

		// Token: 0x06001124 RID: 4388
		float GetExactCostOfAgentAtSlot(Agent candidate, int slotIndex);

		// Token: 0x06001125 RID: 4389
		float GetWeightOfOccupiedSlot(Agent detachedAgent);

		// Token: 0x06001126 RID: 4390
		float? GetWeightOfAgentAtOccupiedSlot(Agent detachedAgent, List<Agent> candidates, out Agent match);

		// Token: 0x06001127 RID: 4391
		bool IsStandingPointAvailableForAgent(Agent agent);

		// Token: 0x06001128 RID: 4392
		void AddAgent(Agent agent, int slotIndex = -1, Agent.AIScriptedFrameFlags customFlags = Agent.AIScriptedFrameFlags.None);

		// Token: 0x06001129 RID: 4393
		void RemoveAgent(Agent detachedAgent);

		// Token: 0x0600112A RID: 4394
		int GetNumberOfUsableSlots();

		// Token: 0x0600112B RID: 4395
		void FormationStartUsing(Formation formation);

		// Token: 0x0600112C RID: 4396
		void FormationStopUsing(Formation formation);

		// Token: 0x0600112D RID: 4397
		bool IsUsedByFormation(Formation formation);

		// Token: 0x0600112E RID: 4398
		WorldFrame? GetAgentFrame(Agent detachedAgent);

		// Token: 0x0600112F RID: 4399
		void ResetEvaluation();

		// Token: 0x06001130 RID: 4400
		bool IsEvaluated();

		// Token: 0x06001131 RID: 4401
		void SetAsEvaluated();

		// Token: 0x06001132 RID: 4402
		void OnFormationLeave(Formation formation);
	}
}
