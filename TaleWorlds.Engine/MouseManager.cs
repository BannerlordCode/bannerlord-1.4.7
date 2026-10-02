using System;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.Engine
{
	// Token: 0x0200006D RID: 109
	public static class MouseManager
	{
		// Token: 0x06000A38 RID: 2616 RVA: 0x0000A646 File Offset: 0x00008846
		public static void ActivateMouseCursor(CursorType mouseId)
		{
			EngineApplicationInterface.IMouseManager.ActivateMouseCursor((int)mouseId);
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x0000A653 File Offset: 0x00008853
		public static void SetMouseCursor(CursorType mouseId, string mousePath)
		{
			EngineApplicationInterface.IMouseManager.SetMouseCursor((int)mouseId, mousePath);
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0000A661 File Offset: 0x00008861
		public static void ShowCursor(bool show)
		{
			EngineApplicationInterface.IMouseManager.ShowCursor(show);
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0000A66E File Offset: 0x0000886E
		public static void LockCursorAtCurrentPosition(bool lockCursor)
		{
			EngineApplicationInterface.IMouseManager.LockCursorAtCurrentPosition(lockCursor);
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0000A67B File Offset: 0x0000887B
		public static void LockCursorAtPosition(float x, float y)
		{
			EngineApplicationInterface.IMouseManager.LockCursorAtPosition(x, y);
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0000A689 File Offset: 0x00008889
		public static void UnlockCursor()
		{
			EngineApplicationInterface.IMouseManager.UnlockCursor();
		}
	}
}
