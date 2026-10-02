using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001BD RID: 445
	[ScriptingInterfaceBase]
	internal interface IMBBannerlordTableauManager
	{
		// Token: 0x06001907 RID: 6407
		[EngineMethod("request_character_tableau_render", false, null, false)]
		void RequestCharacterTableauRender(int characterCodeId, string path, UIntPtr poseEntity, UIntPtr cameraObject, int tableauType);

		// Token: 0x06001908 RID: 6408
		[EngineMethod("initialize_character_tableau_render_system", false, null, false)]
		void InitializeCharacterTableauRenderSystem();

		// Token: 0x06001909 RID: 6409
		[EngineMethod("get_number_of_pending_tableau_requests", false, null, false)]
		int GetNumberOfPendingTableauRequests();
	}
}
