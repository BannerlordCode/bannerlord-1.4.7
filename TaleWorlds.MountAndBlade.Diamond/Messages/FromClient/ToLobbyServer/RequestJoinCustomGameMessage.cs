using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C0 RID: 192
	[MessageDescription("Client", "LobbyServer", false)]
	[Serializable]
	public class RequestJoinCustomGameMessage : Message
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000371 RID: 881 RVA: 0x000045B6 File Offset: 0x000027B6
		// (set) Token: 0x06000372 RID: 882 RVA: 0x000045BE File Offset: 0x000027BE
		[JsonProperty]
		public CustomBattleId CustomBattleId { get; private set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000373 RID: 883 RVA: 0x000045C7 File Offset: 0x000027C7
		// (set) Token: 0x06000374 RID: 884 RVA: 0x000045CF File Offset: 0x000027CF
		[JsonProperty]
		public string Password { get; private set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000375 RID: 885 RVA: 0x000045D8 File Offset: 0x000027D8
		// (set) Token: 0x06000376 RID: 886 RVA: 0x000045E0 File Offset: 0x000027E0
		[JsonProperty]
		public bool IsJoinAsAdminOnly { get; private set; }

		// Token: 0x06000377 RID: 887 RVA: 0x000045E9 File Offset: 0x000027E9
		public RequestJoinCustomGameMessage()
		{
		}

		// Token: 0x06000378 RID: 888 RVA: 0x000045F1 File Offset: 0x000027F1
		public RequestJoinCustomGameMessage(CustomBattleId customBattleId, string password = "", bool isJoinAsAdminOnly = false)
		{
			this.CustomBattleId = customBattleId;
			this.Password = password;
			this.IsJoinAsAdminOnly = isJoinAsAdminOnly;
		}
	}
}
