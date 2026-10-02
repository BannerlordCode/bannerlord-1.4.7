using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200031B RID: 795
	public class MultiplayerGameTypeInfo
	{
		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06002D37 RID: 11575 RVA: 0x000AF7E4 File Offset: 0x000AD9E4
		// (set) Token: 0x06002D38 RID: 11576 RVA: 0x000AF7EC File Offset: 0x000AD9EC
		public string GameModule { get; private set; }

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06002D39 RID: 11577 RVA: 0x000AF7F5 File Offset: 0x000AD9F5
		// (set) Token: 0x06002D3A RID: 11578 RVA: 0x000AF7FD File Offset: 0x000AD9FD
		public string GameType { get; private set; }

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06002D3B RID: 11579 RVA: 0x000AF806 File Offset: 0x000ADA06
		// (set) Token: 0x06002D3C RID: 11580 RVA: 0x000AF80E File Offset: 0x000ADA0E
		public List<string> Scenes { get; private set; }

		// Token: 0x06002D3D RID: 11581 RVA: 0x000AF817 File Offset: 0x000ADA17
		public MultiplayerGameTypeInfo(string gameModule, string gameType)
		{
			this.GameModule = gameModule;
			this.GameType = gameType;
			this.Scenes = new List<string>();
		}
	}
}
