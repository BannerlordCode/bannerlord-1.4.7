using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B4 RID: 436
	[ScriptingInterfaceBase]
	internal interface IMBTeam
	{
		// Token: 0x060018B8 RID: 6328
		[EngineMethod("is_enemy", false, null, true)]
		bool IsEnemy(UIntPtr missionPointer, int teamIndex, int otherTeamIndex);

		// Token: 0x060018B9 RID: 6329
		[EngineMethod("set_is_enemy", false, null, false)]
		void SetIsEnemy(UIntPtr missionPointer, int teamIndex, int otherTeamIndex, bool isEnemy);
	}
}
