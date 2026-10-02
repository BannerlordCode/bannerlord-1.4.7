using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000103 RID: 259
	public class AgentProximityMap
	{
		// Token: 0x06000D2D RID: 3373 RVA: 0x00017A84 File Offset: 0x00015C84
		public static bool CanSearchRadius(float searchRadius)
		{
			float num = Mission.Current.ProximityMapMaxSearchRadius();
			return searchRadius <= num;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x00017AA4 File Offset: 0x00015CA4
		public static AgentProximityMap.ProximityMapSearchStruct BeginSearch(Mission mission, Vec2 searchPos, float searchRadius, bool extendRangeByBiggestAgentCollisionPadding = false)
		{
			if (extendRangeByBiggestAgentCollisionPadding)
			{
				searchRadius += mission.GetBiggestAgentCollisionPadding() + 1f;
			}
			AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = default(AgentProximityMap.ProximityMapSearchStruct);
			float num = mission.ProximityMapMaxSearchRadius();
			proximityMapSearchStruct.LoopAllAgents = searchRadius > num;
			if (proximityMapSearchStruct.LoopAllAgents)
			{
				proximityMapSearchStruct.SearchStructInternal.SearchPos = searchPos;
				proximityMapSearchStruct.SearchStructInternal.SearchDistSq = searchRadius * searchRadius;
				proximityMapSearchStruct.LastAgentLoopIndex = 0;
				proximityMapSearchStruct.LastFoundAgent = null;
				AgentReadOnlyList agents = mission.Agents;
				while (agents.Count > proximityMapSearchStruct.LastAgentLoopIndex)
				{
					Agent agent = agents[proximityMapSearchStruct.LastAgentLoopIndex];
					if (agent.Position.AsVec2.DistanceSquared(searchPos) <= proximityMapSearchStruct.SearchStructInternal.SearchDistSq)
					{
						proximityMapSearchStruct.LastFoundAgent = agent;
						break;
					}
					proximityMapSearchStruct.LastAgentLoopIndex++;
				}
			}
			else
			{
				proximityMapSearchStruct.SearchStructInternal = mission.ProximityMapBeginSearch(searchPos, searchRadius);
				proximityMapSearchStruct.RefreshLastFoundAgent(mission);
			}
			return proximityMapSearchStruct;
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x00017B90 File Offset: 0x00015D90
		public static void FindNext(Mission mission, ref AgentProximityMap.ProximityMapSearchStruct searchStruct)
		{
			if (searchStruct.LoopAllAgents)
			{
				searchStruct.LastAgentLoopIndex++;
				searchStruct.LastFoundAgent = null;
				AgentReadOnlyList agents = mission.Agents;
				while (agents.Count > searchStruct.LastAgentLoopIndex)
				{
					Agent agent = agents[searchStruct.LastAgentLoopIndex];
					if (agent.Position.AsVec2.DistanceSquared(searchStruct.SearchStructInternal.SearchPos) <= searchStruct.SearchStructInternal.SearchDistSq)
					{
						searchStruct.LastFoundAgent = agent;
						return;
					}
					searchStruct.LastAgentLoopIndex++;
				}
				return;
			}
			mission.ProximityMapFindNext(ref searchStruct.SearchStructInternal);
			searchStruct.RefreshLastFoundAgent(mission);
		}

		// Token: 0x02000427 RID: 1063
		public struct ProximityMapSearchStruct
		{
			// Token: 0x17000A18 RID: 2584
			// (get) Token: 0x06003802 RID: 14338 RVA: 0x000E5BFB File Offset: 0x000E3DFB
			// (set) Token: 0x06003803 RID: 14339 RVA: 0x000E5C03 File Offset: 0x000E3E03
			public Agent LastFoundAgent { get; internal set; }

			// Token: 0x06003804 RID: 14340 RVA: 0x000E5C0C File Offset: 0x000E3E0C
			internal void RefreshLastFoundAgent(Mission mission)
			{
				this.LastFoundAgent = this.SearchStructInternal.GetCurrentAgent(mission);
			}

			// Token: 0x0400190E RID: 6414
			internal AgentProximityMap.ProximityMapSearchStructInternal SearchStructInternal;

			// Token: 0x0400190F RID: 6415
			internal bool LoopAllAgents;

			// Token: 0x04001910 RID: 6416
			internal int LastAgentLoopIndex;
		}

		// Token: 0x02000428 RID: 1064
		[EngineStruct("Managed_proximity_map_search_struct", false, null)]
		[Serializable]
		internal struct ProximityMapSearchStructInternal
		{
			// Token: 0x06003805 RID: 14341 RVA: 0x000E5C20 File Offset: 0x000E3E20
			internal Agent GetCurrentAgent(Mission mission)
			{
				return mission.FindAgentWithIndex(this.CurrentElementIndex);
			}

			// Token: 0x04001912 RID: 6418
			internal int CurrentElementIndex;

			// Token: 0x04001913 RID: 6419
			internal Vec2i Loc;

			// Token: 0x04001914 RID: 6420
			internal Vec2i GridMin;

			// Token: 0x04001915 RID: 6421
			internal Vec2i GridMax;

			// Token: 0x04001916 RID: 6422
			internal Vec2 SearchPos;

			// Token: 0x04001917 RID: 6423
			internal float SearchDistSq;
		}
	}
}
