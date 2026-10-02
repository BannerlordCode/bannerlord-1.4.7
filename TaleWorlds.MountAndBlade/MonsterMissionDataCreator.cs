using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002DD RID: 733
	public class MonsterMissionDataCreator : IMonsterMissionDataCreator
	{
		// Token: 0x06002A9A RID: 10906 RVA: 0x000A3F5A File Offset: 0x000A215A
		IMonsterMissionData IMonsterMissionDataCreator.CreateMonsterMissionData(Monster monster)
		{
			return new MonsterMissionData(monster);
		}
	}
}
