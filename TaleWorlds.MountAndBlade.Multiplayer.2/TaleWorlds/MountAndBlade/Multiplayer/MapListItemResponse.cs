using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000056 RID: 86
	[Serializable]
	public class MapListItemResponse
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0000BD8E File Offset: 0x00009F8E
		// (set) Token: 0x060002BC RID: 700 RVA: 0x0000BD96 File Offset: 0x00009F96
		public string Name { get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0000BD9F File Offset: 0x00009F9F
		// (set) Token: 0x060002BE RID: 702 RVA: 0x0000BDA7 File Offset: 0x00009FA7
		public string UniqueToken { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0000BDB0 File Offset: 0x00009FB0
		// (set) Token: 0x060002C0 RID: 704 RVA: 0x0000BDB8 File Offset: 0x00009FB8
		public string Revision { get; private set; }

		// Token: 0x060002C1 RID: 705 RVA: 0x0000BDC1 File Offset: 0x00009FC1
		[JsonConstructor]
		public MapListItemResponse(string name, string uniqueToken, string revision)
		{
			this.Name = name;
			this.UniqueToken = uniqueToken;
			this.Revision = revision;
		}
	}
}
