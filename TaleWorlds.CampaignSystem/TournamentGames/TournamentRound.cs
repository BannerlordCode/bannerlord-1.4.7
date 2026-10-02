using System;

namespace TaleWorlds.CampaignSystem.TournamentGames
{
	// Token: 0x020002D6 RID: 726
	public class TournamentRound
	{
		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06002798 RID: 10136 RVA: 0x000A56FF File Offset: 0x000A38FF
		// (set) Token: 0x06002799 RID: 10137 RVA: 0x000A5707 File Offset: 0x000A3907
		public TournamentMatch[] Matches { get; private set; }

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x0600279A RID: 10138 RVA: 0x000A5710 File Offset: 0x000A3910
		// (set) Token: 0x0600279B RID: 10139 RVA: 0x000A5718 File Offset: 0x000A3918
		public int CurrentMatchIndex { get; private set; }

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x0600279C RID: 10140 RVA: 0x000A5721 File Offset: 0x000A3921
		public TournamentMatch CurrentMatch
		{
			get
			{
				if (this.CurrentMatchIndex >= this.Matches.Length)
				{
					return null;
				}
				return this.Matches[this.CurrentMatchIndex];
			}
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x000A5744 File Offset: 0x000A3944
		public TournamentRound(int participantCount, int numberOfMatches, int numberOfTeamsPerMatch, int numberOfWinnerParticipants, TournamentGame.QualificationMode qualificationMode)
		{
			this.Matches = new TournamentMatch[numberOfMatches];
			this.CurrentMatchIndex = 0;
			int num = participantCount / numberOfMatches;
			for (int i = 0; i < numberOfMatches; i++)
			{
				this.Matches[i] = new TournamentMatch(num, numberOfTeamsPerMatch, numberOfWinnerParticipants / numberOfMatches, qualificationMode);
			}
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x000A5790 File Offset: 0x000A3990
		public void OnMatchEnded()
		{
			int currentMatchIndex = this.CurrentMatchIndex;
			this.CurrentMatchIndex = currentMatchIndex + 1;
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x000A57B0 File Offset: 0x000A39B0
		public void EndMatch()
		{
			this.CurrentMatch.End();
			int currentMatchIndex = this.CurrentMatchIndex;
			this.CurrentMatchIndex = currentMatchIndex + 1;
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x000A57D8 File Offset: 0x000A39D8
		public void AddParticipant(TournamentParticipant participant, bool firstTime = false)
		{
			foreach (TournamentMatch tournamentMatch in this.Matches)
			{
				if (tournamentMatch.IsParticipantRequired())
				{
					tournamentMatch.AddParticipant(participant, firstTime);
					return;
				}
			}
		}
	}
}
