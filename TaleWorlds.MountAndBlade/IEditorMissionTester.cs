using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002D8 RID: 728
	public interface IEditorMissionTester
	{
		// Token: 0x06002A87 RID: 10887
		void StartMissionForEditor(string missionName, string sceneName, string levels);

		// Token: 0x06002A88 RID: 10888
		void StartMissionForReplayEditor(string missionName, string sceneName, string levels, string fileName, bool record, float startTime, float endTime);
	}
}
