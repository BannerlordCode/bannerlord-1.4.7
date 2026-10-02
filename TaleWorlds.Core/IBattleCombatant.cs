using System;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x0200007D RID: 125
	public interface IBattleCombatant
	{
		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000871 RID: 2161
		TextObject Name { get; }

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000872 RID: 2162
		BattleSideEnum Side { get; }

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000873 RID: 2163
		BasicCultureObject BasicCulture { get; }

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000874 RID: 2164
		BasicCharacterObject General { get; }

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000875 RID: 2165
		Tuple<uint, uint> PrimaryColorPair { get; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000876 RID: 2166
		Banner Banner { get; }

		// Token: 0x06000877 RID: 2167
		int GetTacticsSkillAmount();

		// Token: 0x06000878 RID: 2168
		int GetNumberOfMissionReadyTroops();

		// Token: 0x06000879 RID: 2169
		bool IsUnderPlayersCommand(BattleSideEnum playerSide);
	}
}
