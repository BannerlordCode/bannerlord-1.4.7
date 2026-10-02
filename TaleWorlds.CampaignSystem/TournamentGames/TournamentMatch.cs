using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.TournamentGames
{
	// Token: 0x020002D4 RID: 724
	public class TournamentMatch
	{
		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06002778 RID: 10104 RVA: 0x000A512B File Offset: 0x000A332B
		public IEnumerable<TournamentTeam> Teams
		{
			get
			{
				return this._teams.AsEnumerable<TournamentTeam>();
			}
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x06002779 RID: 10105 RVA: 0x000A5138 File Offset: 0x000A3338
		public IEnumerable<TournamentParticipant> Participants
		{
			get
			{
				return this._participants.AsEnumerable<TournamentParticipant>();
			}
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x0600277A RID: 10106 RVA: 0x000A5145 File Offset: 0x000A3345
		// (set) Token: 0x0600277B RID: 10107 RVA: 0x000A514D File Offset: 0x000A334D
		public TournamentMatch.MatchState State { get; private set; }

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x0600277C RID: 10108 RVA: 0x000A5156 File Offset: 0x000A3356
		public IEnumerable<TournamentParticipant> Winners
		{
			get
			{
				return this._winners.AsEnumerable<TournamentParticipant>();
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x0600277D RID: 10109 RVA: 0x000A5163 File Offset: 0x000A3363
		public bool IsReady
		{
			get
			{
				return this.State == TournamentMatch.MatchState.Ready;
			}
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x000A5170 File Offset: 0x000A3370
		public TournamentMatch(int participantCount, int numberOfTeamsPerMatch, int numberOfWinnerParticipants, TournamentGame.QualificationMode qualificationMode)
		{
			this._participants = new List<TournamentParticipant>();
			this._participantCount = participantCount;
			this._teams = new TournamentTeam[numberOfTeamsPerMatch];
			this._winners = new List<TournamentParticipant>();
			this._numberOfWinnerParticipants = numberOfWinnerParticipants;
			this.QualificationMode = qualificationMode;
			this._teamSize = participantCount / numberOfTeamsPerMatch;
			int[] array = new int[] { 119, 118, 120, 121 };
			int num = 0;
			for (int i = 0; i < numberOfTeamsPerMatch; i++)
			{
				this._teams[i] = new TournamentTeam(this._teamSize, BannerManager.GetColor(array[num]), Banner.CreateOneColoredEmptyBanner(array[num]));
				num++;
				num %= 4;
			}
			this.State = TournamentMatch.MatchState.Ready;
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x000A5216 File Offset: 0x000A3416
		public void End()
		{
			this.State = TournamentMatch.MatchState.Finished;
			this._winners = this.GetWinners();
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x000A522C File Offset: 0x000A342C
		public void Start()
		{
			if (this.State != TournamentMatch.MatchState.Started)
			{
				this.State = TournamentMatch.MatchState.Started;
				foreach (TournamentParticipant tournamentParticipant in this.Participants)
				{
					tournamentParticipant.ResetScore();
				}
			}
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x000A5288 File Offset: 0x000A3488
		public TournamentParticipant GetParticipant(int uniqueSeed)
		{
			return this._participants.FirstOrDefault<TournamentParticipant>((TournamentParticipant p) => p.Descriptor.CompareTo(uniqueSeed) == 0);
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x000A52B9 File Offset: 0x000A34B9
		public bool IsParticipantRequired()
		{
			return this._participants.Count < this._participantCount;
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x000A52D0 File Offset: 0x000A34D0
		public void AddParticipant(TournamentParticipant participant, bool firstTime)
		{
			this._participants.Add(participant);
			foreach (TournamentTeam tournamentTeam in this.Teams)
			{
				if (tournamentTeam.IsParticipantRequired() && ((participant.Team != null && participant.Team.TeamColor == tournamentTeam.TeamColor) || firstTime))
				{
					tournamentTeam.AddParticipant(participant);
					return;
				}
			}
			foreach (TournamentTeam tournamentTeam2 in this.Teams)
			{
				if (tournamentTeam2.IsParticipantRequired())
				{
					tournamentTeam2.AddParticipant(participant);
					break;
				}
			}
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x000A539C File Offset: 0x000A359C
		public bool IsPlayerParticipating()
		{
			return this.Participants.Any<TournamentParticipant>((TournamentParticipant x) => x.Character == CharacterObject.PlayerCharacter);
		}

		// Token: 0x06002785 RID: 10117 RVA: 0x000A53C8 File Offset: 0x000A35C8
		public bool IsPlayerWinner()
		{
			if (this.IsPlayerParticipating())
			{
				return this.GetWinners().Any<TournamentParticipant>((TournamentParticipant x) => x.Character == CharacterObject.PlayerCharacter);
			}
			return false;
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x000A5400 File Offset: 0x000A3600
		private List<TournamentParticipant> GetWinners()
		{
			List<TournamentParticipant> list = new List<TournamentParticipant>();
			if (this.QualificationMode == TournamentGame.QualificationMode.IndividualScore)
			{
				List<TournamentParticipant> list2 = this._participants.OrderByDescending<TournamentParticipant, int>((TournamentParticipant x) => x.Score).Take<TournamentParticipant>(this._numberOfWinnerParticipants).ToList<TournamentParticipant>();
				using (List<TournamentParticipant>.Enumerator enumerator = this._participants.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TournamentParticipant tournamentParticipant = enumerator.Current;
						if (list2.Contains(tournamentParticipant))
						{
							tournamentParticipant.IsAssigned = false;
							list.Add(tournamentParticipant);
						}
					}
					return list;
				}
			}
			if (this.QualificationMode == TournamentGame.QualificationMode.TeamScore)
			{
				IOrderedEnumerable<TournamentTeam> orderedEnumerable = this._teams.OrderByDescending<TournamentTeam, int>((TournamentTeam x) => x.Score);
				List<TournamentTeam> list3 = orderedEnumerable.Take<TournamentTeam>(this._numberOfWinnerParticipants / this._teamSize).ToList<TournamentTeam>();
				foreach (TournamentTeam tournamentTeam in this._teams)
				{
					if (list3.Contains(tournamentTeam))
					{
						foreach (TournamentParticipant tournamentParticipant2 in tournamentTeam.Participants)
						{
							tournamentParticipant2.IsAssigned = false;
							list.Add(tournamentParticipant2);
						}
					}
				}
				foreach (TournamentTeam tournamentTeam2 in orderedEnumerable)
				{
					int num = this._numberOfWinnerParticipants - list.Count;
					if (tournamentTeam2.Participants.Count<TournamentParticipant>() >= num)
					{
						IOrderedEnumerable<TournamentParticipant> orderedEnumerable2 = tournamentTeam2.Participants.OrderByDescending<TournamentParticipant, int>((TournamentParticipant x) => x.Score);
						list.AddRange(orderedEnumerable2.Take<TournamentParticipant>(num));
						break;
					}
					list.AddRange(tournamentTeam2.Participants);
				}
			}
			return list;
		}

		// Token: 0x04000B92 RID: 2962
		private readonly int _numberOfWinnerParticipants;

		// Token: 0x04000B93 RID: 2963
		public readonly TournamentGame.QualificationMode QualificationMode;

		// Token: 0x04000B94 RID: 2964
		private readonly TournamentTeam[] _teams;

		// Token: 0x04000B95 RID: 2965
		private readonly List<TournamentParticipant> _participants;

		// Token: 0x04000B97 RID: 2967
		private List<TournamentParticipant> _winners;

		// Token: 0x04000B98 RID: 2968
		private readonly int _participantCount;

		// Token: 0x04000B99 RID: 2969
		private int _teamSize;

		// Token: 0x02000683 RID: 1667
		public enum MatchState
		{
			// Token: 0x04001A5E RID: 6750
			Ready,
			// Token: 0x04001A5F RID: 6751
			Started,
			// Token: 0x04001A60 RID: 6752
			Finished
		}
	}
}
