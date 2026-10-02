using System;
using System.Collections.Generic;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200031C RID: 796
	public static class MultiplayerGlobalMutedPlayersManager
	{
		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06002D3E RID: 11582 RVA: 0x000AF838 File Offset: 0x000ADA38
		private static List<PlayerId> _mutedPlayers
		{
			get
			{
				if (MultiplayerGlobalMutedPlayersManager._mutedPlayersInternal == null)
				{
					MultiplayerGlobalMutedPlayersManager._mutedPlayersInternal = new List<PlayerId>();
				}
				return MultiplayerGlobalMutedPlayersManager._mutedPlayersInternal;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06002D3F RID: 11583 RVA: 0x000AF850 File Offset: 0x000ADA50
		public static List<PlayerId> MutedPlayers
		{
			get
			{
				return MultiplayerGlobalMutedPlayersManager._mutedPlayers;
			}
		}

		// Token: 0x06002D40 RID: 11584 RVA: 0x000AF857 File Offset: 0x000ADA57
		public static void MutePlayer(PlayerId playerId)
		{
			MultiplayerGlobalMutedPlayersManager._mutedPlayers.Add(playerId);
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x000AF864 File Offset: 0x000ADA64
		public static void UnmutePlayer(PlayerId playerId)
		{
			MultiplayerGlobalMutedPlayersManager._mutedPlayers.Remove(playerId);
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x000AF872 File Offset: 0x000ADA72
		public static bool IsUserMuted(PlayerId playerId)
		{
			return MultiplayerGlobalMutedPlayersManager._mutedPlayers.Contains(playerId);
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x000AF87F File Offset: 0x000ADA7F
		public static void ClearMutedPlayers()
		{
			MultiplayerGlobalMutedPlayersManager._mutedPlayers.Clear();
		}

		// Token: 0x040011D6 RID: 4566
		private static List<PlayerId> _mutedPlayersInternal;
	}
}
