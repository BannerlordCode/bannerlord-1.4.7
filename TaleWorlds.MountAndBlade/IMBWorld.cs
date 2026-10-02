using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B5 RID: 437
	[ScriptingInterfaceBase]
	internal interface IMBWorld
	{
		// Token: 0x060018BA RID: 6330
		[EngineMethod("get_global_time", false, null, false)]
		float GetGlobalTime(MBCommon.TimeType timeType);

		// Token: 0x060018BB RID: 6331
		[EngineMethod("get_last_messages", false, null, false)]
		string GetLastMessages();

		// Token: 0x060018BC RID: 6332
		[EngineMethod("get_game_type", false, null, false)]
		int GetGameType();

		// Token: 0x060018BD RID: 6333
		[EngineMethod("set_game_type", false, null, false)]
		void SetGameType(int gameType);

		// Token: 0x060018BE RID: 6334
		[EngineMethod("pause_game", false, null, false)]
		void PauseGame();

		// Token: 0x060018BF RID: 6335
		[EngineMethod("unpause_game", false, null, false)]
		void UnpauseGame();

		// Token: 0x060018C0 RID: 6336
		[EngineMethod("set_mesh_used", false, null, false)]
		void SetMeshUsed(string meshName);

		// Token: 0x060018C1 RID: 6337
		[EngineMethod("set_material_used", false, null, false)]
		void SetMaterialUsed(string materialName);

		// Token: 0x060018C2 RID: 6338
		[EngineMethod("set_body_used", false, null, false)]
		void SetBodyUsed(string bodyName);

		// Token: 0x060018C3 RID: 6339
		[EngineMethod("fix_skeletons", false, null, false)]
		void FixSkeletons();

		// Token: 0x060018C4 RID: 6340
		[EngineMethod("check_resource_modifications", false, null, false)]
		void CheckResourceModifications();
	}
}
