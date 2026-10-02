using System;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001E3 RID: 483
	public abstract class TournamentModel : MBGameModel<TournamentModel>
	{
		// Token: 0x06001ED6 RID: 7894
		public abstract float GetTournamentStartChance(Town town);

		// Token: 0x06001ED7 RID: 7895
		public abstract TournamentGame CreateTournament(Town town);

		// Token: 0x06001ED8 RID: 7896
		public abstract float GetTournamentEndChance(TournamentGame tournament);

		// Token: 0x06001ED9 RID: 7897
		public abstract int GetNumLeaderboardVictoriesAtGameStart();

		// Token: 0x06001EDA RID: 7898
		public abstract float GetTournamentSimulationScore(CharacterObject character);

		// Token: 0x06001EDB RID: 7899
		public abstract int GetRenownReward(Hero winner, Town town);

		// Token: 0x06001EDC RID: 7900
		public abstract int GetInfluenceReward(Hero winner, Town town);

		// Token: 0x06001EDD RID: 7901
		[return: TupleElementNames(new string[] { "skill", "xp" })]
		public abstract ValueTuple<SkillObject, int> GetSkillXpGainFromTournament(Town town);

		// Token: 0x06001EDE RID: 7902
		public abstract Equipment GetParticipantArmor(CharacterObject participant);

		// Token: 0x06001EDF RID: 7903
		public abstract MBList<ItemObject> GetRegularRewardItems(Town town, int regularRewardMinValue, int regularRewardMaxValue);

		// Token: 0x06001EE0 RID: 7904
		public abstract MBList<ItemObject> GetEliteRewardItems(Town town, int regularRewardMinValue, int regularRewardMaxValue);
	}
}
