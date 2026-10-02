using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200008B RID: 139
	public struct ConversationSceneData
	{
		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x0600122E RID: 4654 RVA: 0x000535BA File Offset: 0x000517BA
		// (set) Token: 0x0600122F RID: 4655 RVA: 0x000535C2 File Offset: 0x000517C2
		public string SceneID { get; private set; }

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001230 RID: 4656 RVA: 0x000535CB File Offset: 0x000517CB
		// (set) Token: 0x06001231 RID: 4657 RVA: 0x000535D3 File Offset: 0x000517D3
		public TerrainType Terrain { get; private set; }

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001232 RID: 4658 RVA: 0x000535DC File Offset: 0x000517DC
		// (set) Token: 0x06001233 RID: 4659 RVA: 0x000535E4 File Offset: 0x000517E4
		public List<TerrainType> TerrainTypes { get; private set; }

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001234 RID: 4660 RVA: 0x000535ED File Offset: 0x000517ED
		// (set) Token: 0x06001235 RID: 4661 RVA: 0x000535F5 File Offset: 0x000517F5
		public ForestDensity ForestDensity { get; private set; }

		// Token: 0x06001236 RID: 4662 RVA: 0x000535FE File Offset: 0x000517FE
		public ConversationSceneData(string sceneID, TerrainType terrain, List<TerrainType> terrainTypes, ForestDensity forestDensity)
		{
			this.SceneID = sceneID;
			this.Terrain = terrain;
			this.TerrainTypes = terrainTypes;
			this.ForestDensity = forestDensity;
		}
	}
}
