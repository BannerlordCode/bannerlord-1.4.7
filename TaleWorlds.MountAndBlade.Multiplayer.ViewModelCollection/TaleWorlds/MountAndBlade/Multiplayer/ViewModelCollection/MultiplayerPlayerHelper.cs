using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x02000014 RID: 20
	public static class MultiplayerPlayerHelper
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00005982 File Offset: 0x00003B82
		private static IReadOnlyCollection<PlayerId> PlatformBlocks
		{
			get
			{
				return PlatformServices.Instance.BlockedUsers;
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000598E File Offset: 0x00003B8E
		public static bool IsBlocked(PlayerId playerID)
		{
			return PermaMuteList.IsPlayerMuted(playerID) || (MultiplayerPlayerHelper.PlatformBlocks != null && MultiplayerPlayerHelper.PlatformBlocks.Contains(playerID));
		}
	}
}
