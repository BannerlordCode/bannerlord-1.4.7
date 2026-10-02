using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000088 RID: 136
	public class ResourceDepotFile
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x0001222E File Offset: 0x0001042E
		// (set) Token: 0x060004F3 RID: 1267 RVA: 0x00012236 File Offset: 0x00010436
		public ResourceDepotLocation ResourceDepotLocation { get; private set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x0001223F File Offset: 0x0001043F
		public string BasePath
		{
			get
			{
				return this.ResourceDepotLocation.BasePath;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x0001224C File Offset: 0x0001044C
		public string Location
		{
			get
			{
				return this.ResourceDepotLocation.Path;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x00012259 File Offset: 0x00010459
		// (set) Token: 0x060004F7 RID: 1271 RVA: 0x00012261 File Offset: 0x00010461
		public string FileName { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x0001226A File Offset: 0x0001046A
		// (set) Token: 0x060004F9 RID: 1273 RVA: 0x00012272 File Offset: 0x00010472
		public string FullPath { get; private set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x0001227B File Offset: 0x0001047B
		// (set) Token: 0x060004FB RID: 1275 RVA: 0x00012283 File Offset: 0x00010483
		public string FullPathLowerCase { get; private set; }

		// Token: 0x060004FC RID: 1276 RVA: 0x0001228C File Offset: 0x0001048C
		public ResourceDepotFile(ResourceDepotLocation resourceDepotLocation, string fileName, string fullPath)
		{
			this.ResourceDepotLocation = resourceDepotLocation;
			this.FileName = fileName;
			this.FullPath = fullPath;
			this.FullPathLowerCase = fullPath.ToLower();
		}
	}
}
