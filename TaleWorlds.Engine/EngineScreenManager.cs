using System;
using TaleWorlds.DotNet;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.Engine
{
	// Token: 0x02000045 RID: 69
	internal class EngineScreenManager
	{
		// Token: 0x060006D3 RID: 1747 RVA: 0x000041BE File Offset: 0x000023BE
		[EngineCallback(null, false)]
		internal static void PreTick(float dt)
		{
			ScreenManager.EarlyUpdate(EngineApplicationInterface.IScreen.GetUsableAreaPercentages());
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x000041CF File Offset: 0x000023CF
		[EngineCallback(null, false)]
		public static void Tick(float dt)
		{
			ScreenManager.Tick(dt);
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x000041D7 File Offset: 0x000023D7
		[EngineCallback(null, false)]
		internal static void LateTick(float dt)
		{
			ScreenManager.LateTick(dt);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x000041DF File Offset: 0x000023DF
		[EngineCallback(null, false)]
		internal static void OnOnscreenKeyboardDone(string inputText)
		{
			ScreenManager.OnOnscreenKeyboardDone(inputText);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x000041E7 File Offset: 0x000023E7
		[EngineCallback(null, false)]
		internal static void OnOnscreenKeyboardCanceled()
		{
			ScreenManager.OnOnscreenKeyboardCanceled();
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x000041EE File Offset: 0x000023EE
		[EngineCallback(null, false)]
		internal static void OnGameWindowFocusChange(bool focusGained)
		{
			ScreenManager.OnGameWindowFocusChange(focusGained);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x000041F6 File Offset: 0x000023F6
		[EngineCallback(null, false)]
		internal static void Update()
		{
			ScreenManager.Update(EngineScreenManager._lastPressedKeys);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00004202 File Offset: 0x00002402
		[EngineCallback(null, false)]
		internal static void InitializeLastPressedKeys(NativeArray lastKeysPressed)
		{
			EngineScreenManager._lastPressedKeys = new NativeArrayEnumerator<int>(lastKeysPressed);
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0000420F File Offset: 0x0000240F
		internal static void Initialize()
		{
			ScreenManager.Initialize(new ScreenManagerEngineConnection());
		}

		// Token: 0x0400005A RID: 90
		private static NativeArrayEnumerator<int> _lastPressedKeys;
	}
}
