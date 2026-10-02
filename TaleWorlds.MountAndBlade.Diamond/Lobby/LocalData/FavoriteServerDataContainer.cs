using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData
{
	// Token: 0x02000172 RID: 370
	public class FavoriteServerDataContainer : MultiplayerLocalDataContainer<FavoriteServerData>
	{
		// Token: 0x06000A4B RID: 2635 RVA: 0x000109BC File Offset: 0x0000EBBC
		protected override string GetSaveDirectoryName()
		{
			return "Data";
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x000109C3 File Offset: 0x0000EBC3
		protected override string GetSaveFileName()
		{
			return "FavoriteServers.json";
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x000109CC File Offset: 0x0000EBCC
		public bool TryGetServerData(GameServerEntry serverEntry, out FavoriteServerData favoriteServerData)
		{
			favoriteServerData = null;
			MBReadOnlyList<FavoriteServerData> entries = base.GetEntries();
			for (int i = 0; i < entries.Count; i++)
			{
				FavoriteServerData favoriteServerData2 = entries[i];
				if (favoriteServerData2.HasSameContentWith(serverEntry))
				{
					favoriteServerData = favoriteServerData2;
					return true;
				}
			}
			return false;
		}
	}
}
