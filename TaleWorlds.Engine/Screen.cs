using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000085 RID: 133
	public static class Screen
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000C11 RID: 3089 RVA: 0x0000D4A3 File Offset: 0x0000B6A3
		// (set) Token: 0x06000C12 RID: 3090 RVA: 0x0000D4AA File Offset: 0x0000B6AA
		public static float RealScreenResolutionWidth { get; private set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000C13 RID: 3091 RVA: 0x0000D4B2 File Offset: 0x0000B6B2
		// (set) Token: 0x06000C14 RID: 3092 RVA: 0x0000D4B9 File Offset: 0x0000B6B9
		public static float RealScreenResolutionHeight { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000C15 RID: 3093 RVA: 0x0000D4C1 File Offset: 0x0000B6C1
		public static Vec2 RealScreenResolution
		{
			get
			{
				return new Vec2(Screen.RealScreenResolutionWidth, Screen.RealScreenResolutionHeight);
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000C16 RID: 3094 RVA: 0x0000D4D2 File Offset: 0x0000B6D2
		// (set) Token: 0x06000C17 RID: 3095 RVA: 0x0000D4D9 File Offset: 0x0000B6D9
		public static float AspectRatio { get; private set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000C18 RID: 3096 RVA: 0x0000D4E1 File Offset: 0x0000B6E1
		// (set) Token: 0x06000C19 RID: 3097 RVA: 0x0000D4E8 File Offset: 0x0000B6E8
		public static Vec2 DesktopResolution { get; private set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000C1A RID: 3098 RVA: 0x0000D4F0 File Offset: 0x0000B6F0
		// (set) Token: 0x06000C1B RID: 3099 RVA: 0x0000D4F7 File Offset: 0x0000B6F7
		public static Vec2 ScreenScale { get; private set; }

		// Token: 0x06000C1C RID: 3100 RVA: 0x0000D500 File Offset: 0x0000B700
		internal static void Update()
		{
			Screen.RealScreenResolutionWidth = EngineApplicationInterface.IScreen.GetRealScreenResolutionWidth();
			Screen.RealScreenResolutionHeight = EngineApplicationInterface.IScreen.GetRealScreenResolutionHeight();
			Screen.AspectRatio = EngineApplicationInterface.IScreen.GetAspectRatio();
			Screen.DesktopResolution = new Vec2(EngineApplicationInterface.IScreen.GetDesktopWidth(), EngineApplicationInterface.IScreen.GetDesktopHeight());
			Screen.ScreenScale = new Vec2(Screen.RealScreenResolutionWidth / Screen.DesktopResolution.x, Screen.RealScreenResolutionHeight / Screen.DesktopResolution.y);
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x0000D582 File Offset: 0x0000B782
		public static bool GetMouseVisible()
		{
			return EngineApplicationInterface.IScreen.GetMouseVisible();
		}
	}
}
