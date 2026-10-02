using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.TournamentGames
{
	// Token: 0x020002D5 RID: 725
	public class TournamentParticipant
	{
		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06002787 RID: 10119 RVA: 0x000A5620 File Offset: 0x000A3820
		// (set) Token: 0x06002788 RID: 10120 RVA: 0x000A5628 File Offset: 0x000A3828
		public int Score { get; private set; }

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06002789 RID: 10121 RVA: 0x000A5631 File Offset: 0x000A3831
		// (set) Token: 0x0600278A RID: 10122 RVA: 0x000A5639 File Offset: 0x000A3839
		public CharacterObject Character { get; private set; }

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x0600278B RID: 10123 RVA: 0x000A5642 File Offset: 0x000A3842
		// (set) Token: 0x0600278C RID: 10124 RVA: 0x000A564A File Offset: 0x000A384A
		public UniqueTroopDescriptor Descriptor { get; private set; }

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x0600278D RID: 10125 RVA: 0x000A5653 File Offset: 0x000A3853
		// (set) Token: 0x0600278E RID: 10126 RVA: 0x000A565B File Offset: 0x000A385B
		public TournamentTeam Team { get; private set; }

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x0600278F RID: 10127 RVA: 0x000A5664 File Offset: 0x000A3864
		// (set) Token: 0x06002790 RID: 10128 RVA: 0x000A566C File Offset: 0x000A386C
		public Equipment MatchEquipment { get; set; }

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06002791 RID: 10129 RVA: 0x000A5675 File Offset: 0x000A3875
		// (set) Token: 0x06002792 RID: 10130 RVA: 0x000A567D File Offset: 0x000A387D
		public bool IsAssigned { get; set; }

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06002793 RID: 10131 RVA: 0x000A5686 File Offset: 0x000A3886
		public bool IsPlayer
		{
			get
			{
				CharacterObject character = this.Character;
				return character != null && character.IsPlayerCharacter;
			}
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x000A5699 File Offset: 0x000A3899
		public TournamentParticipant(CharacterObject character, UniqueTroopDescriptor descriptor = default(UniqueTroopDescriptor))
		{
			this.Character = character;
			this.Descriptor = (descriptor.IsValid ? descriptor : new UniqueTroopDescriptor(Game.Current.NextUniqueTroopSeed));
			this.Team = null;
			this.IsAssigned = false;
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x000A56D7 File Offset: 0x000A38D7
		public void SetTeam(TournamentTeam team)
		{
			this.Team = team;
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x000A56E0 File Offset: 0x000A38E0
		public int AddScore(int score)
		{
			this.Score += score;
			return this.Score;
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x000A56F6 File Offset: 0x000A38F6
		public void ResetScore()
		{
			this.Score = 0;
		}
	}
}
