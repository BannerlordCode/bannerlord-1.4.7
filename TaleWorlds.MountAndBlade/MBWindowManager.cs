using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E2 RID: 482
	public class MBWindowManager
	{
		// Token: 0x06001C58 RID: 7256 RVA: 0x000611C2 File Offset: 0x0005F3C2
		public static float WorldToScreen(Camera camera, Vec3 worldSpacePosition, ref float screenX, ref float screenY, ref float w)
		{
			return MBAPI.IMBWindowManager.WorldToScreen(camera.Pointer, worldSpacePosition, ref screenX, ref screenY, ref w);
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x000611DC File Offset: 0x0005F3DC
		public static float WorldToScreenInsideUsableArea(Camera camera, Vec3 worldSpacePosition, ref float screenX, ref float screenY, ref float w)
		{
			float num = MBAPI.IMBWindowManager.WorldToScreen(camera.Pointer, worldSpacePosition, ref screenX, ref screenY, ref w);
			screenX -= (Screen.RealScreenResolutionWidth - ScreenManager.UsableArea.X * Screen.RealScreenResolutionWidth) / 2f;
			screenY -= (Screen.RealScreenResolutionHeight - ScreenManager.UsableArea.Y * Screen.RealScreenResolutionHeight) / 2f;
			return num;
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x00061246 File Offset: 0x0005F446
		public static float WorldToScreenWithFixedZ(Camera camera, Vec3 cameraPosition, Vec3 worldSpacePosition, ref float screenX, ref float screenY, ref float w)
		{
			return MBAPI.IMBWindowManager.WorldToScreenWithFixedZ(camera.Pointer, cameraPosition, worldSpacePosition, ref screenX, ref screenY, ref w);
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x0006125F File Offset: 0x0005F45F
		public static void ScreenToWorld(Camera camera, float screenX, float screenY, float w, ref Vec3 worldSpacePosition)
		{
			MBAPI.IMBWindowManager.ScreenToWorld(camera.Pointer, screenX, screenY, w, ref worldSpacePosition);
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x00061276 File Offset: 0x0005F476
		public static Vec2 GetScreenResolution()
		{
			return MBAPI.IMBWindowManager.GetScreenResolution();
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x00061282 File Offset: 0x0005F482
		public static void PreDisplay()
		{
			MBAPI.IMBWindowManager.PreDisplay();
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x0006128E File Offset: 0x0005F48E
		public static void DontChangeCursorPos()
		{
			MBAPI.IMBWindowManager.DontChangeCursorPos();
		}
	}
}
