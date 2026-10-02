using System;
using TaleWorlds.Engine.InputSystem;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.Engine
{
	// Token: 0x02000042 RID: 66
	public static class EngineController
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060006B4 RID: 1716 RVA: 0x00003C60 File Offset: 0x00001E60
		// (remove) Token: 0x060006B5 RID: 1717 RVA: 0x00003C94 File Offset: 0x00001E94
		public static event Action ConfigChange;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060006B6 RID: 1718 RVA: 0x00003CC8 File Offset: 0x00001EC8
		// (remove) Token: 0x060006B7 RID: 1719 RVA: 0x00003CFC File Offset: 0x00001EFC
		public static event Action<bool> OnConstrainedStateChanged;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060006B8 RID: 1720 RVA: 0x00003D30 File Offset: 0x00001F30
		// (remove) Token: 0x060006B9 RID: 1721 RVA: 0x00003D64 File Offset: 0x00001F64
		public static event Action OnDLCInstalledCallback;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060006BA RID: 1722 RVA: 0x00003D98 File Offset: 0x00001F98
		// (remove) Token: 0x060006BB RID: 1723 RVA: 0x00003DCC File Offset: 0x00001FCC
		public static event Action OnDLCLoadedCallback;

		// Token: 0x060006BC RID: 1724 RVA: 0x00003DFF File Offset: 0x00001FFF
		internal static void OnApplicationTick(float dt)
		{
			Input.Update();
			Screen.Update();
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00003E0C File Offset: 0x0000200C
		[EngineCallback(null, false)]
		internal static void Initialize()
		{
			IInputContext inputContext = null;
			Input.Initialize(new EngineInputManager(), inputContext);
			Common.PlatformFileHelper = new PlatformFileHelperPC(Utilities.GetApplicationName());
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00003E35 File Offset: 0x00002035
		[EngineCallback(null, false)]
		internal static void OnConfigChange()
		{
			NativeConfig.OnConfigChanged();
			if (EngineController.ConfigChange != null)
			{
				EngineController.ConfigChange();
			}
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00003E4D File Offset: 0x0000204D
		[EngineCallback(null, false)]
		internal static void OnConstrainedStateChange(bool isConstrained)
		{
			Action<bool> onConstrainedStateChanged = EngineController.OnConstrainedStateChanged;
			if (onConstrainedStateChanged == null)
			{
				return;
			}
			onConstrainedStateChanged(isConstrained);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x00003E5F File Offset: 0x0000205F
		[EngineCallback(null, false)]
		internal static void OnDLCInstalled()
		{
			Action onDLCInstalledCallback = EngineController.OnDLCInstalledCallback;
			if (onDLCInstalledCallback == null)
			{
				return;
			}
			onDLCInstalledCallback();
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00003E70 File Offset: 0x00002070
		[EngineCallback(null, false)]
		internal static void OnDLCLoaded()
		{
			Action onDLCLoadedCallback = EngineController.OnDLCLoadedCallback;
			if (onDLCLoadedCallback == null)
			{
				return;
			}
			onDLCLoadedCallback();
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00003E84 File Offset: 0x00002084
		[EngineCallback(null, false)]
		public static string GetVersionStr()
		{
			return ApplicationVersion.FromParametersFile(null).ToString();
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00003EA8 File Offset: 0x000020A8
		[EngineCallback(null, false)]
		public static string GetApplicationPlatformName()
		{
			return ApplicationPlatform.CurrentPlatform.ToString();
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00003EC8 File Offset: 0x000020C8
		[EngineCallback(null, false)]
		public static string GetModulesVersionStr()
		{
			string text = "";
			foreach (ModuleInfo moduleInfo in ModuleHelper.GetModules(null))
			{
				text = string.Concat(new object[] { text, moduleInfo.Name, "#", moduleInfo.Version, "\n" });
			}
			return text;
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00003F54 File Offset: 0x00002154
		[EngineCallback(null, false)]
		internal static void OnControllerDisconnection()
		{
			ScreenManager.OnControllerDisconnect();
		}
	}
}
