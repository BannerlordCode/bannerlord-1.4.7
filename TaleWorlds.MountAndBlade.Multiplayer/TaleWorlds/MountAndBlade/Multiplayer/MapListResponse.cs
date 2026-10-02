using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000055 RID: 85
	[Serializable]
	public class MapListResponse
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000BD56 File Offset: 0x00009F56
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x0000BD5E File Offset: 0x00009F5E
		public string CurrentlyPlaying { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000BD67 File Offset: 0x00009F67
		// (set) Token: 0x060002B9 RID: 697 RVA: 0x0000BD6F File Offset: 0x00009F6F
		public List<MapListItemResponse> Maps { get; private set; }

		// Token: 0x060002BA RID: 698 RVA: 0x0000BD78 File Offset: 0x00009F78
		[JsonConstructor]
		public MapListResponse(string currentlyPlaying, List<MapListItemResponse> maps)
		{
			this.CurrentlyPlaying = currentlyPlaying;
			this.Maps = maps;
		}
	}
}
