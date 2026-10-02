using System;
using System.IO;

namespace TaleWorlds.Library
{
	// Token: 0x02000089 RID: 137
	public class ResourceDepotLocation
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x000122B5 File Offset: 0x000104B5
		// (set) Token: 0x060004FE RID: 1278 RVA: 0x000122BD File Offset: 0x000104BD
		public string BasePath { get; private set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x000122C6 File Offset: 0x000104C6
		// (set) Token: 0x06000500 RID: 1280 RVA: 0x000122CE File Offset: 0x000104CE
		public string Path { get; private set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x000122D7 File Offset: 0x000104D7
		// (set) Token: 0x06000502 RID: 1282 RVA: 0x000122DF File Offset: 0x000104DF
		public string FullPath { get; private set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x000122E8 File Offset: 0x000104E8
		// (set) Token: 0x06000504 RID: 1284 RVA: 0x000122F0 File Offset: 0x000104F0
		public FileSystemWatcher Watcher { get; private set; }

		// Token: 0x06000505 RID: 1285 RVA: 0x000122F9 File Offset: 0x000104F9
		public ResourceDepotLocation(string basePath, string path, string fullPath)
		{
			this.BasePath = basePath;
			this.Path = path;
			this.FullPath = fullPath;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00012318 File Offset: 0x00010518
		public void StartWatchingChanges(FileSystemEventHandler onChangeEvent, RenamedEventHandler onRenameEvent)
		{
			this.Watcher = new FileSystemWatcher
			{
				Path = this.FullPath,
				NotifyFilter = (NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite | NotifyFilters.CreationTime),
				Filter = "*.*",
				IncludeSubdirectories = true,
				EnableRaisingEvents = true
			};
			this.Watcher.Changed += onChangeEvent;
			this.Watcher.Created += onChangeEvent;
			this.Watcher.Deleted += onChangeEvent;
			this.Watcher.Renamed += onRenameEvent;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001238D File Offset: 0x0001058D
		public void StopWatchingChanges()
		{
			this.Watcher.Dispose();
		}
	}
}
