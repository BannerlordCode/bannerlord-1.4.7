using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200011D RID: 285
	public interface IBattleServerSessionHandler
	{
		// Token: 0x06000660 RID: 1632
		void OnConnected();

		// Token: 0x06000661 RID: 1633
		void OnCantConnect();

		// Token: 0x06000662 RID: 1634
		void OnDisconnected();

		// Token: 0x06000663 RID: 1635
		void OnNewPlayer(BattlePeer peer);

		// Token: 0x06000664 RID: 1636
		void OnStartGame(string sceneName, string gameType, string faction1, string faction2, int minRequiredPlayerCountToStartBattle, int battleSize, string[] profanityList, string[] allowList);

		// Token: 0x06000665 RID: 1637
		void OnPlayerFledBattle(BattlePeer peer, out BattleResult battleResult, bool isQuitFromBattle);

		// Token: 0x06000666 RID: 1638
		void OnEndMission();

		// Token: 0x06000667 RID: 1639
		void OnStopServer();
	}
}
