using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000EB RID: 235
	[Serializable]
	public class AvailableCustomGames
	{
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x0000519C File Offset: 0x0000339C
		// (set) Token: 0x0600047C RID: 1148 RVA: 0x000051A4 File Offset: 0x000033A4
		[JsonProperty]
		public List<GameServerEntry> CustomGameServerInfos { get; private set; }

		// Token: 0x0600047D RID: 1149 RVA: 0x000051AD File Offset: 0x000033AD
		public AvailableCustomGames()
		{
			this.CustomGameServerInfos = new List<GameServerEntry>();
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x000051C0 File Offset: 0x000033C0
		public AvailableCustomGames GetCustomGamesByPermission(int playerPermission)
		{
			AvailableCustomGames availableCustomGames = new AvailableCustomGames();
			foreach (GameServerEntry gameServerEntry in this.CustomGameServerInfos)
			{
				if (gameServerEntry.Permission <= playerPermission)
				{
					availableCustomGames.CustomGameServerInfos.Add(gameServerEntry);
				}
			}
			return availableCustomGames;
		}
	}
}
