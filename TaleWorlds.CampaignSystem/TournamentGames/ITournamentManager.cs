using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.TournamentGames
{
	// Token: 0x020002D2 RID: 722
	public interface ITournamentManager
	{
		// Token: 0x0600274F RID: 10063
		void AddTournament(TournamentGame game);

		// Token: 0x06002750 RID: 10064
		TournamentGame GetTournamentGame(Town town);

		// Token: 0x06002751 RID: 10065
		void OnPlayerJoinMatch(Type gameType);

		// Token: 0x06002752 RID: 10066
		void OnPlayerJoinTournament(Type gameType, Settlement settlement);

		// Token: 0x06002753 RID: 10067
		void OnPlayerWatchTournament(Type gameType, Settlement settlement);

		// Token: 0x06002754 RID: 10068
		void OnPlayerWinMatch(Type gameType);

		// Token: 0x06002755 RID: 10069
		void OnPlayerWinTournament(Type gameType);

		// Token: 0x06002756 RID: 10070
		void InitializeLeaderboardEntry(Hero hero, int initialVictories = 0);

		// Token: 0x06002757 RID: 10071
		void AddLeaderboardEntry(Hero hero);

		// Token: 0x06002758 RID: 10072
		void GivePrizeToWinner(TournamentGame tournament, Hero winner, bool isPlayerParticipated);

		// Token: 0x06002759 RID: 10073
		void DeleteLeaderboardEntry(Hero hero);

		// Token: 0x0600275A RID: 10074
		List<KeyValuePair<Hero, int>> GetLeaderboard();

		// Token: 0x0600275B RID: 10075
		int GetLeaderBoardRank(Hero hero);

		// Token: 0x0600275C RID: 10076
		Hero GetLeaderBoardLeader();

		// Token: 0x0600275D RID: 10077
		void ResolveTournament(TournamentGame tournament, Town town);
	}
}
