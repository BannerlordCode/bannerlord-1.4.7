using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000D1 RID: 209
	public class CheckpointCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06000931 RID: 2353 RVA: 0x00043020 File Offset: 0x00041220
		public override void RegisterEvents()
		{
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x00043022 File Offset: 0x00041222
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<int>("LastUsedMissionCheckpointId", ref this.LastUsedMissionCheckpointId);
			dataStore.SyncData<List<AgentSaveData>>("CorpseList", ref this.CorpseList);
		}

		// Token: 0x0400047A RID: 1146
		public int LastUsedMissionCheckpointId = -1;

		// Token: 0x0400047B RID: 1147
		public List<AgentSaveData> CorpseList = new List<AgentSaveData>();
	}
}
