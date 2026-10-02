using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000037 RID: 55
	[ApplicationInterfaceBase]
	internal interface IMouseManager
	{
		// Token: 0x06000577 RID: 1399
		[EngineMethod("activate_mouse_cursor", false, null, false)]
		void ActivateMouseCursor(int id);

		// Token: 0x06000578 RID: 1400
		[EngineMethod("set_mouse_cursor", false, null, false)]
		void SetMouseCursor(int id, string mousePath);

		// Token: 0x06000579 RID: 1401
		[EngineMethod("show_cursor", false, null, false)]
		void ShowCursor(bool show);

		// Token: 0x0600057A RID: 1402
		[EngineMethod("lock_cursor_at_current_pos", false, null, false)]
		void LockCursorAtCurrentPosition(bool lockCursor);

		// Token: 0x0600057B RID: 1403
		[EngineMethod("lock_cursor_at_position", false, null, false)]
		void LockCursorAtPosition(float x, float y);

		// Token: 0x0600057C RID: 1404
		[EngineMethod("unlock_cursor", false, null, false)]
		void UnlockCursor();
	}
}
