using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200008A RID: 138
	public struct SingleplayerBattleSceneData
	{
		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001221 RID: 4641 RVA: 0x00053525 File Offset: 0x00051725
		// (set) Token: 0x06001222 RID: 4642 RVA: 0x0005352D File Offset: 0x0005172D
		public string SceneID { get; private set; }

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x00053536 File Offset: 0x00051736
		// (set) Token: 0x06001224 RID: 4644 RVA: 0x0005353E File Offset: 0x0005173E
		public TerrainType Terrain { get; private set; }

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001225 RID: 4645 RVA: 0x00053547 File Offset: 0x00051747
		// (set) Token: 0x06001226 RID: 4646 RVA: 0x0005354F File Offset: 0x0005174F
		public List<TerrainType> TerrainTypes { get; private set; }

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001227 RID: 4647 RVA: 0x00053558 File Offset: 0x00051758
		// (set) Token: 0x06001228 RID: 4648 RVA: 0x00053560 File Offset: 0x00051760
		public ForestDensity ForestDensity { get; private set; }

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001229 RID: 4649 RVA: 0x00053569 File Offset: 0x00051769
		// (set) Token: 0x0600122A RID: 4650 RVA: 0x00053571 File Offset: 0x00051771
		public List<int> MapIndices { get; private set; }

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x0600122B RID: 4651 RVA: 0x0005357A File Offset: 0x0005177A
		// (set) Token: 0x0600122C RID: 4652 RVA: 0x00053582 File Offset: 0x00051782
		public bool IsNaval { get; private set; }

		// Token: 0x0600122D RID: 4653 RVA: 0x0005358B File Offset: 0x0005178B
		public SingleplayerBattleSceneData(string sceneID, TerrainType terrain, List<TerrainType> terrainTypes, ForestDensity forestDensity, List<int> mapIndices, bool isNaval)
		{
			this.SceneID = sceneID;
			this.Terrain = terrain;
			this.TerrainTypes = terrainTypes;
			this.ForestDensity = forestDensity;
			this.MapIndices = mapIndices;
			this.IsNaval = isNaval;
		}
	}
}
