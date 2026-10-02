using System;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001D0 RID: 464
	public class MBMultiplayerData
	{
		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001BBC RID: 7100 RVA: 0x000605A4 File Offset: 0x0005E7A4
		// (set) Token: 0x06001BBD RID: 7101 RVA: 0x000605AB File Offset: 0x0005E7AB
		public static Guid ServerId { get; set; }

		// Token: 0x06001BBE RID: 7102 RVA: 0x000605B4 File Offset: 0x0005E7B4
		[MBCallback(null, false)]
		public static string GetServerId()
		{
			return MBMultiplayerData.ServerId.ToString();
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x000605D4 File Offset: 0x0005E7D4
		[MBCallback(null, false)]
		public static string GetServerName()
		{
			return MBMultiplayerData.ServerName;
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x000605DB File Offset: 0x0005E7DB
		[MBCallback(null, false)]
		public static string GetGameModule()
		{
			return MBMultiplayerData.GameModule;
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x000605E2 File Offset: 0x0005E7E2
		[MBCallback(null, false)]
		public static string GetGameType()
		{
			return MBMultiplayerData.GameType;
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x000605E9 File Offset: 0x0005E7E9
		[MBCallback(null, false)]
		public static string GetMap()
		{
			return MBMultiplayerData.Map;
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x000605F0 File Offset: 0x0005E7F0
		[MBCallback(null, false)]
		public static int GetCurrentPlayerCount()
		{
			return GameNetwork.NetworkPeerCount;
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x000605F7 File Offset: 0x0005E7F7
		[MBCallback(null, false)]
		public static int GetPlayerCountLimit()
		{
			return MBMultiplayerData.PlayerCountLimit;
		}

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06001BC5 RID: 7109 RVA: 0x00060600 File Offset: 0x0005E800
		// (remove) Token: 0x06001BC6 RID: 7110 RVA: 0x00060634 File Offset: 0x0005E834
		public static event MBMultiplayerData.GameServerInfoReceivedDelegate GameServerInfoReceived;

		// Token: 0x06001BC7 RID: 7111 RVA: 0x00060668 File Offset: 0x0005E868
		[MBCallback(null, false)]
		public static void UpdateGameServerInfo(string id, string gameServer, string gameModule, string gameType, string map, int currentPlayerCount, int maxPlayerCount, string address, int port)
		{
			if (MBMultiplayerData.GameServerInfoReceived != null)
			{
				MBMultiplayerData.GameServerInfoReceived(new CustomBattleId(Guid.Parse(id)), gameServer, gameModule, gameType, map, currentPlayerCount, maxPlayerCount, address, port);
			}
		}

		// Token: 0x0400091A RID: 2330
		public static string ServerName;

		// Token: 0x0400091B RID: 2331
		public static string GameModule;

		// Token: 0x0400091C RID: 2332
		public static string GameType;

		// Token: 0x0400091D RID: 2333
		public static string Map;

		// Token: 0x0400091E RID: 2334
		public static int PlayerCountLimit;

		// Token: 0x0200050A RID: 1290
		// (Invoke) Token: 0x06003BB2 RID: 15282
		public delegate void GameServerInfoReceivedDelegate(CustomBattleId id, string gameServer, string gameModule, string gameType, string map, int currentPlayerCount, int maxPlayerCount, string address, int port);
	}
}
