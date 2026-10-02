using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B7 RID: 439
	[ScriptingInterfaceBase]
	internal interface IMBWindowManager
	{
		// Token: 0x060018C8 RID: 6344
		[EngineMethod("erase_message_lines", false, null, false)]
		void EraseMessageLines();

		// Token: 0x060018C9 RID: 6345
		[EngineMethod("world_to_screen", false, null, true)]
		float WorldToScreen(UIntPtr cameraPointer, Vec3 worldSpacePosition, ref float screenX, ref float screenY, ref float w);

		// Token: 0x060018CA RID: 6346
		[EngineMethod("world_to_screen_with_fixed_z", false, null, false)]
		float WorldToScreenWithFixedZ(UIntPtr cameraPointer, Vec3 cameraPosition, Vec3 worldSpacePosition, ref float screenX, ref float screenY, ref float w);

		// Token: 0x060018CB RID: 6347
		[EngineMethod("dont_change_cursor_pos", false, null, false)]
		void DontChangeCursorPos();

		// Token: 0x060018CC RID: 6348
		[EngineMethod("pre_display", false, null, false)]
		void PreDisplay();

		// Token: 0x060018CD RID: 6349
		[EngineMethod("screen_to_world", false, null, false)]
		void ScreenToWorld(UIntPtr pointer, float screenX, float screenY, float z, ref Vec3 worldSpacePosition);

		// Token: 0x060018CE RID: 6350
		[EngineMethod("get_screen_resolution", false, null, false)]
		Vec2 GetScreenResolution();
	}
}
