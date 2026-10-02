using System;
using System.Collections.Generic;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002E9 RID: 745
	public static class CustomGameBannedPlayerManager
	{
		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06002AD8 RID: 10968 RVA: 0x000A4EB5 File Offset: 0x000A30B5
		private static Dictionary<PlayerId, CustomGameBannedPlayerManager.BannedPlayer> _bannedPlayers
		{
			get
			{
				if (CustomGameBannedPlayerManager._bannedPlayersInternal == null)
				{
					CustomGameBannedPlayerManager._bannedPlayersInternal = new Dictionary<PlayerId, CustomGameBannedPlayerManager.BannedPlayer>();
				}
				return CustomGameBannedPlayerManager._bannedPlayersInternal;
			}
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x000A4ED0 File Offset: 0x000A30D0
		public static void AddBannedPlayer(PlayerId playerId, int banDueTime)
		{
			CustomGameBannedPlayerManager._bannedPlayers[playerId] = new CustomGameBannedPlayerManager.BannedPlayer
			{
				PlayerId = playerId,
				BanDueTime = banDueTime
			};
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x000A4F04 File Offset: 0x000A3104
		public static bool IsUserBanned(PlayerId playerId)
		{
			return CustomGameBannedPlayerManager._bannedPlayers.ContainsKey(playerId) && CustomGameBannedPlayerManager._bannedPlayers[playerId].BanDueTime > Environment.TickCount;
		}

		// Token: 0x04001099 RID: 4249
		private static Dictionary<PlayerId, CustomGameBannedPlayerManager.BannedPlayer> _bannedPlayersInternal;

		// Token: 0x020005CA RID: 1482
		private struct BannedPlayer
		{
			// Token: 0x17000A81 RID: 2689
			// (get) Token: 0x06003E6E RID: 15982 RVA: 0x000F5200 File Offset: 0x000F3400
			// (set) Token: 0x06003E6F RID: 15983 RVA: 0x000F5208 File Offset: 0x000F3408
			public PlayerId PlayerId { get; set; }

			// Token: 0x17000A82 RID: 2690
			// (get) Token: 0x06003E70 RID: 15984 RVA: 0x000F5211 File Offset: 0x000F3411
			// (set) Token: 0x06003E71 RID: 15985 RVA: 0x000F5219 File Offset: 0x000F3419
			public int BanDueTime { get; set; }
		}
	}
}
