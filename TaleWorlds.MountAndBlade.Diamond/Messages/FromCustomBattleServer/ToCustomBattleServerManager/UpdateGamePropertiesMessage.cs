using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromCustomBattleServer.ToCustomBattleServerManager
{
	// Token: 0x0200000E RID: 14
	[MessageDescription("CustomBattleServer", "CustomBattleServerManager", false)]
	[Serializable]
	public class UpdateGamePropertiesMessage : Message
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600006B RID: 107 RVA: 0x000025F0 File Offset: 0x000007F0
		// (set) Token: 0x0600006C RID: 108 RVA: 0x000025F8 File Offset: 0x000007F8
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00002601 File Offset: 0x00000801
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00002609 File Offset: 0x00000809
		[JsonProperty]
		public string Scene { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00002612 File Offset: 0x00000812
		// (set) Token: 0x06000070 RID: 112 RVA: 0x0000261A File Offset: 0x0000081A
		[JsonProperty]
		public string UniqueSceneId { get; private set; }

		// Token: 0x06000071 RID: 113 RVA: 0x00002623 File Offset: 0x00000823
		public UpdateGamePropertiesMessage()
		{
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000262B File Offset: 0x0000082B
		public UpdateGamePropertiesMessage(string gameType, string scene, string uniqueSceneId)
		{
			this.GameType = gameType;
			this.Scene = scene;
			this.UniqueSceneId = uniqueSceneId;
		}
	}
}
