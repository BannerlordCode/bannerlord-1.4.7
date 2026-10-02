using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000062 RID: 98
	public static class MultiplayerReportPlayerManager
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060002EA RID: 746 RVA: 0x0000D69C File Offset: 0x0000B89C
		// (remove) Token: 0x060002EB RID: 747 RVA: 0x0000D6D0 File Offset: 0x0000B8D0
		public static event Action<string, PlayerId, string, bool> ReportHandlers;

		// Token: 0x060002EC RID: 748 RVA: 0x0000D703 File Offset: 0x0000B903
		public static void RequestReportPlayer(string gameId, PlayerId playerId, string playerName, bool isRequestedFromMission)
		{
			Action<string, PlayerId, string, bool> reportHandlers = MultiplayerReportPlayerManager.ReportHandlers;
			if (reportHandlers == null)
			{
				return;
			}
			reportHandlers(gameId, playerId, playerName, isRequestedFromMission);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000D718 File Offset: 0x0000B918
		public static void OnPlayerReported(PlayerId playerId)
		{
			MultiplayerReportPlayerManager.IncrementReportOfPlayer(playerId);
			NetworkCommunicator networkCommunicator = GameNetwork.NetworkPeers.Find((NetworkCommunicator x) => x.VirtualPlayer.Id == playerId);
			if (networkCommunicator != null)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (component != null)
				{
					component.SetMuted(true);
				}
			}
			Game.Current.GetGameHandler<ChatBox>().SetPlayerMuted(playerId, true);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000D780 File Offset: 0x0000B980
		public static bool IsPlayerReportedOverLimit(PlayerId player)
		{
			int num;
			return MultiplayerReportPlayerManager._reportsPerPlayer.TryGetValue(player, out num) && num == 3;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000D7A4 File Offset: 0x0000B9A4
		private static void IncrementReportOfPlayer(PlayerId player)
		{
			if (MultiplayerReportPlayerManager._reportsPerPlayer.ContainsKey(player))
			{
				Dictionary<PlayerId, int> reportsPerPlayer = MultiplayerReportPlayerManager._reportsPerPlayer;
				int num = reportsPerPlayer[player];
				reportsPerPlayer[player] = num + 1;
				return;
			}
			MultiplayerReportPlayerManager._reportsPerPlayer.Add(player, 1);
		}

		// Token: 0x040000EC RID: 236
		private static Dictionary<PlayerId, int> _reportsPerPlayer = new Dictionary<PlayerId, int>();

		// Token: 0x040000ED RID: 237
		private const int _maxReportsPerPlayer = 3;
	}
}
