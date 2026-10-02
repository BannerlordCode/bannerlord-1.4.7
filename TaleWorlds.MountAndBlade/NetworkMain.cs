using System;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200031E RID: 798
	public static class NetworkMain
	{
		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06002D7C RID: 11644 RVA: 0x000AFECA File Offset: 0x000AE0CA
		// (set) Token: 0x06002D7D RID: 11645 RVA: 0x000AFED1 File Offset: 0x000AE0D1
		public static LobbyClient GameClient { get; private set; }

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06002D7E RID: 11646 RVA: 0x000AFED9 File Offset: 0x000AE0D9
		// (set) Token: 0x06002D7F RID: 11647 RVA: 0x000AFEE0 File Offset: 0x000AE0E0
		public static CommunityClient CommunityClient { get; private set; }

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06002D80 RID: 11648 RVA: 0x000AFEE8 File Offset: 0x000AE0E8
		// (set) Token: 0x06002D81 RID: 11649 RVA: 0x000AFEEF File Offset: 0x000AE0EF
		public static CustomBattleServer CustomBattleServer { get; private set; }

		// Token: 0x06002D82 RID: 11650 RVA: 0x000AFEF7 File Offset: 0x000AE0F7
		public static void SetPeers(LobbyClient gameClient, CommunityClient communityClient, CustomBattleServer customBattleServer)
		{
			NetworkMain.GameClient = gameClient;
			NetworkMain.CommunityClient = communityClient;
			NetworkMain.CustomBattleServer = customBattleServer;
		}
	}
}
