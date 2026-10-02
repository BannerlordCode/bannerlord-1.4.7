using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
	// Token: 0x02000010 RID: 16
	public struct CustomBattleSceneData
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x000084E6 File Offset: 0x000066E6
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x000084EE File Offset: 0x000066EE
		public string SceneID { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x000084F7 File Offset: 0x000066F7
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x000084FF File Offset: 0x000066FF
		public TextObject Name { get; private set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00008508 File Offset: 0x00006708
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00008510 File Offset: 0x00006710
		public TerrainType Terrain { get; private set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00008519 File Offset: 0x00006719
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00008521 File Offset: 0x00006721
		public List<TerrainType> TerrainTypes { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000852A File Offset: 0x0000672A
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00008532 File Offset: 0x00006732
		public ForestDensity ForestDensity { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000853B File Offset: 0x0000673B
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00008543 File Offset: 0x00006743
		public bool IsSiegeMap { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000FE RID: 254 RVA: 0x0000854C File Offset: 0x0000674C
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00008554 File Offset: 0x00006754
		public bool IsVillageMap { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000100 RID: 256 RVA: 0x0000855D File Offset: 0x0000675D
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00008565 File Offset: 0x00006765
		public bool IsLordsHallMap { get; private set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000856E File Offset: 0x0000676E
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00008576 File Offset: 0x00006776
		public string ForcedSceneLevel { get; private set; }

		// Token: 0x06000104 RID: 260 RVA: 0x00008580 File Offset: 0x00006780
		public CustomBattleSceneData(string sceneID, TextObject name, TerrainType terrain, List<TerrainType> terrainTypes, ForestDensity forestDensity, bool isSiegeMap, bool isVillageMap, bool isLordsHallMap, string forcedSceneLevel)
		{
			this.SceneID = sceneID;
			this.Name = name;
			this.Terrain = terrain;
			this.TerrainTypes = terrainTypes;
			this.ForestDensity = forestDensity;
			this.IsSiegeMap = isSiegeMap;
			this.IsVillageMap = isVillageMap;
			this.IsLordsHallMap = isLordsHallMap;
			this.ForcedSceneLevel = forcedSceneLevel;
		}
	}
}
