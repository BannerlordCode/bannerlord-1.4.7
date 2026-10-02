using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000D4 RID: 212
	public static class TeamSideEnumExtensions
	{
		// Token: 0x06000B30 RID: 2864 RVA: 0x00024930 File Offset: 0x00022B30
		public static bool IsValid(this TeamSideEnum teamSide)
		{
			return teamSide >= TeamSideEnum.PlayerTeam && teamSide < TeamSideEnum.NumSides;
		}
	}
}
