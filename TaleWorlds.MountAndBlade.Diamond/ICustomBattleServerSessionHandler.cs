using System;
using System.Threading.Tasks;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200011E RID: 286
	public interface ICustomBattleServerSessionHandler
	{
		// Token: 0x06000668 RID: 1640
		void OnConnected();

		// Token: 0x06000669 RID: 1641
		void OnCantConnect();

		// Token: 0x0600066A RID: 1642
		void OnDisconnected();

		// Token: 0x0600066B RID: 1643
		void OnStateChanged(CustomBattleServer.State state);

		// Token: 0x0600066C RID: 1644
		void OnSuccessfulGameRegister();

		// Token: 0x0600066D RID: 1645
		Task<PlayerJoinGameResponseDataFromHost[]> OnClientWantsToConnectCustomGame(PlayerJoinGameData[] playerJoinData);

		// Token: 0x0600066E RID: 1646
		void OnClientQuitFromCustomGame(PlayerId playerId);

		// Token: 0x0600066F RID: 1647
		void OnGameFinished();

		// Token: 0x06000670 RID: 1648
		void OnChatFilterListsReceived(string[] profanityList, string[] allowList);

		// Token: 0x06000671 RID: 1649
		void OnPlayerKickRequested(PlayerId playerID, bool isBanning);
	}
}
