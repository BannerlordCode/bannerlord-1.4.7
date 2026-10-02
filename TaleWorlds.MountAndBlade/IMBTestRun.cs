using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001A3 RID: 419
	[ScriptingInterfaceBase]
	internal interface IMBTestRun
	{
		// Token: 0x0600165E RID: 5726
		[EngineMethod("auto_continue", false, null, false)]
		int AutoContinue(int type);

		// Token: 0x0600165F RID: 5727
		[EngineMethod("get_fps", false, null, false)]
		int GetFPS();

		// Token: 0x06001660 RID: 5728
		[EngineMethod("enter_edit_mode", false, null, false)]
		bool EnterEditMode();

		// Token: 0x06001661 RID: 5729
		[EngineMethod("open_scene", false, null, false)]
		bool OpenScene(string sceneName);

		// Token: 0x06001662 RID: 5730
		[EngineMethod("close_scene", false, null, false)]
		bool CloseScene();

		// Token: 0x06001663 RID: 5731
		[EngineMethod("save_scene", false, null, false)]
		bool SaveScene();

		// Token: 0x06001664 RID: 5732
		[EngineMethod("open_default_scene", false, null, false)]
		bool OpenDefaultScene();

		// Token: 0x06001665 RID: 5733
		[EngineMethod("leave_edit_mode", false, null, false)]
		bool LeaveEditMode();

		// Token: 0x06001666 RID: 5734
		[EngineMethod("new_scene", false, null, false)]
		bool NewScene();

		// Token: 0x06001667 RID: 5735
		[EngineMethod("start_mission", false, null, false)]
		void StartMission();
	}
}
