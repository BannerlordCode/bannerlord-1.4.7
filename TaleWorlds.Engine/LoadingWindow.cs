using System;

namespace TaleWorlds.Engine
{
	// Token: 0x02000056 RID: 86
	public static class LoadingWindow
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x00006FB0 File Offset: 0x000051B0
		// (set) Token: 0x060008B8 RID: 2232 RVA: 0x00006FB7 File Offset: 0x000051B7
		public static bool IsLoadingWindowActive { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x00006FBF File Offset: 0x000051BF
		// (set) Token: 0x060008BA RID: 2234 RVA: 0x00006FC6 File Offset: 0x000051C6
		public static ILoadingWindowManager LoadingWindowManager { get; private set; }

		// Token: 0x060008BC RID: 2236 RVA: 0x00006FD0 File Offset: 0x000051D0
		public static void InitializeWith<T>() where T : class, ILoadingWindowManager, new()
		{
			LoadingWindow.Destroy();
			LoadingWindow.LoadingWindowManager = new T();
			LoadingWindow.LoadingWindowManager.Initialize();
			if (LoadingWindow.IsLoadingWindowActive)
			{
				LoadingWindow.LoadingWindowManager.EnableLoadingWindow();
			}
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00007001 File Offset: 0x00005201
		public static void Destroy()
		{
			ILoadingWindowManager loadingWindowManager = LoadingWindow.LoadingWindowManager;
			if (loadingWindowManager != null)
			{
				loadingWindowManager.DisableLoadingWindow();
			}
			ILoadingWindowManager loadingWindowManager2 = LoadingWindow.LoadingWindowManager;
			if (loadingWindowManager2 != null)
			{
				loadingWindowManager2.Destroy();
			}
			LoadingWindow.LoadingWindowManager = null;
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00007029 File Offset: 0x00005229
		public static void DisableGlobalLoadingWindow()
		{
			if (LoadingWindow.LoadingWindowManager == null)
			{
				return;
			}
			if (LoadingWindow.IsLoadingWindowActive)
			{
				LoadingWindow.LoadingWindowManager.DisableLoadingWindow();
				Utilities.DisableGlobalLoadingWindow();
				Utilities.OnLoadingWindowDisabled();
			}
			LoadingWindow.IsLoadingWindowActive = false;
			Utilities.DebugSetGlobalLoadingWindowState(false);
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0000705A File Offset: 0x0000525A
		public static void EnableGlobalLoadingWindow()
		{
			if (LoadingWindow.LoadingWindowManager == null)
			{
				return;
			}
			LoadingWindow.IsLoadingWindowActive = true;
			Utilities.DebugSetGlobalLoadingWindowState(true);
			if (LoadingWindow.IsLoadingWindowActive)
			{
				LoadingWindow.LoadingWindowManager.EnableLoadingWindow();
				Utilities.OnLoadingWindowEnabled();
			}
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x00007086 File Offset: 0x00005286
		public static void SetCurrentModeIsMultiplayer(bool isMultiplayer)
		{
			ILoadingWindowManager loadingWindowManager = LoadingWindow.LoadingWindowManager;
			if (loadingWindowManager == null)
			{
				return;
			}
			loadingWindowManager.SetCurrentModeIsMultiplayer(isMultiplayer);
		}
	}
}
