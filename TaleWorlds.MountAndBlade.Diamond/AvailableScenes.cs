using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000EC RID: 236
	[Serializable]
	public class AvailableScenes
	{
		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x00005228 File Offset: 0x00003428
		// (set) Token: 0x06000480 RID: 1152 RVA: 0x0000522F File Offset: 0x0000342F
		public static AvailableScenes Empty { get; private set; } = new AvailableScenes(new Dictionary<string, string[]>());

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x00005248 File Offset: 0x00003448
		// (set) Token: 0x06000483 RID: 1155 RVA: 0x00005250 File Offset: 0x00003450
		public Dictionary<string, string[]> ScenesByGameTypes { get; set; }

		// Token: 0x06000484 RID: 1156 RVA: 0x00005259 File Offset: 0x00003459
		public AvailableScenes()
		{
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00005261 File Offset: 0x00003461
		public AvailableScenes(Dictionary<string, string[]> scenesByGameTypes)
		{
			this.ScenesByGameTypes = scenesByGameTypes;
		}
	}
}
