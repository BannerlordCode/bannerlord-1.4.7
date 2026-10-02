using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x0200000D RID: 13
	internal class ScriptingInterfaceOfIConfig : IConfig
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x0000F35C File Offset: 0x0000D55C
		public void Apply(int texture_budget, int sharpen_amount, int hdr, int dof_mode, int motion_blur, int ssr, int size, int texture_filtering, int trail_amount, int dynamic_resolution_target)
		{
			ScriptingInterfaceOfIConfig.call_ApplyDelegate(texture_budget, sharpen_amount, hdr, dof_mode, motion_blur, ssr, size, texture_filtering, trail_amount, dynamic_resolution_target);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000F384 File Offset: 0x0000D584
		public void ApplyConfigChanges(bool resizeWindow)
		{
			ScriptingInterfaceOfIConfig.call_ApplyConfigChangesDelegate(resizeWindow);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000F391 File Offset: 0x0000D591
		public int AutoSaveInMinutes()
		{
			return ScriptingInterfaceOfIConfig.call_AutoSaveInMinutesDelegate();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000F39D File Offset: 0x0000D59D
		public bool CheckGFXSupportStatus(int enum_id)
		{
			return ScriptingInterfaceOfIConfig.call_CheckGFXSupportStatusDelegate(enum_id);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000F3AA File Offset: 0x0000D5AA
		public int GetAutoGFXQuality()
		{
			return ScriptingInterfaceOfIConfig.call_GetAutoGFXQualityDelegate();
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000F3B6 File Offset: 0x0000D5B6
		public int GetCharacterDetail()
		{
			return ScriptingInterfaceOfIConfig.call_GetCharacterDetailDelegate();
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000F3C2 File Offset: 0x0000D5C2
		public bool GetCheatMode()
		{
			return ScriptingInterfaceOfIConfig.call_GetCheatModeDelegate();
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000F3CE File Offset: 0x0000D5CE
		public int GetCurrentSoundDeviceIndex()
		{
			return ScriptingInterfaceOfIConfig.call_GetCurrentSoundDeviceIndexDelegate();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000F3DA File Offset: 0x0000D5DA
		public string GetDebugLoginPassword()
		{
			if (ScriptingInterfaceOfIConfig.call_GetDebugLoginPasswordDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000F3F0 File Offset: 0x0000D5F0
		public string GetDebugLoginUserName()
		{
			if (ScriptingInterfaceOfIConfig.call_GetDebugLoginUserNameDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000F406 File Offset: 0x0000D606
		public float GetDefaultRGLConfig(int type)
		{
			return ScriptingInterfaceOfIConfig.call_GetDefaultRGLConfigDelegate(type);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000F413 File Offset: 0x0000D613
		public void GetDesktopResolution(ref int width, ref int height)
		{
			ScriptingInterfaceOfIConfig.call_GetDesktopResolutionDelegate(ref width, ref height);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000F421 File Offset: 0x0000D621
		public bool GetDevelopmentMode()
		{
			return ScriptingInterfaceOfIConfig.call_GetDevelopmentModeDelegate();
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000F42D File Offset: 0x0000D62D
		public bool GetDisableGuiMessages()
		{
			return ScriptingInterfaceOfIConfig.call_GetDisableGuiMessagesDelegate();
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000F439 File Offset: 0x0000D639
		public bool GetDisableSound()
		{
			return ScriptingInterfaceOfIConfig.call_GetDisableSoundDelegate();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000F445 File Offset: 0x0000D645
		public int GetDlssOptionCount()
		{
			return ScriptingInterfaceOfIConfig.call_GetDlssOptionCountDelegate();
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000F451 File Offset: 0x0000D651
		public int GetDlssTechnique()
		{
			return ScriptingInterfaceOfIConfig.call_GetDlssTechniqueDelegate();
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000F45D File Offset: 0x0000D65D
		public bool GetDoLocalizationCheckAtStartup()
		{
			return ScriptingInterfaceOfIConfig.call_GetDoLocalizationCheckAtStartupDelegate();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000F469 File Offset: 0x0000D669
		public bool GetEnableClothSimulation()
		{
			return ScriptingInterfaceOfIConfig.call_GetEnableClothSimulationDelegate();
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000F475 File Offset: 0x0000D675
		public bool GetEnableEditMode()
		{
			return ScriptingInterfaceOfIConfig.call_GetEnableEditModeDelegate();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000F481 File Offset: 0x0000D681
		public bool GetInvertMouse()
		{
			return ScriptingInterfaceOfIConfig.call_GetInvertMouseDelegate();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000F48D File Offset: 0x0000D68D
		public string GetLastOpenedScene()
		{
			if (ScriptingInterfaceOfIConfig.call_GetLastOpenedSceneDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000F4A3 File Offset: 0x0000D6A3
		public bool GetLocalizationDebugMode()
		{
			return ScriptingInterfaceOfIConfig.call_GetLocalizationDebugModeDelegate();
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000F4AF File Offset: 0x0000D6AF
		public int GetMonitorDeviceCount()
		{
			return ScriptingInterfaceOfIConfig.call_GetMonitorDeviceCountDelegate();
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000F4BB File Offset: 0x0000D6BB
		public string GetMonitorDeviceName(int i)
		{
			if (ScriptingInterfaceOfIConfig.call_GetMonitorDeviceNameDelegate(i) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0000F4D2 File Offset: 0x0000D6D2
		public int GetRefreshRateAtIndex(int index)
		{
			return ScriptingInterfaceOfIConfig.call_GetRefreshRateAtIndexDelegate(index);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000F4DF File Offset: 0x0000D6DF
		public int GetRefreshRateCount()
		{
			return ScriptingInterfaceOfIConfig.call_GetRefreshRateCountDelegate();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000F4EB File Offset: 0x0000D6EB
		public void GetResolution(ref int width, ref int height)
		{
			ScriptingInterfaceOfIConfig.call_GetResolutionDelegate(ref width, ref height);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000F4F9 File Offset: 0x0000D6F9
		public Vec2 GetResolutionAtIndex(int index)
		{
			return ScriptingInterfaceOfIConfig.call_GetResolutionAtIndexDelegate(index);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000F506 File Offset: 0x0000D706
		public int GetResolutionCount()
		{
			return ScriptingInterfaceOfIConfig.call_GetResolutionCountDelegate();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000F512 File Offset: 0x0000D712
		public float GetRGLConfig(int type)
		{
			return ScriptingInterfaceOfIConfig.call_GetRGLConfigDelegate(type);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000F51F File Offset: 0x0000D71F
		public float GetRGLConfigForDefaultSettings(int type, int defaultSettings)
		{
			return ScriptingInterfaceOfIConfig.call_GetRGLConfigForDefaultSettingsDelegate(type, defaultSettings);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000F52D File Offset: 0x0000D72D
		public int GetSoundDeviceCount()
		{
			return ScriptingInterfaceOfIConfig.call_GetSoundDeviceCountDelegate();
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000F539 File Offset: 0x0000D739
		public string GetSoundDeviceName(int i)
		{
			if (ScriptingInterfaceOfIConfig.call_GetSoundDeviceNameDelegate(i) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000F550 File Offset: 0x0000D750
		public bool GetTableauCacheMode()
		{
			return ScriptingInterfaceOfIConfig.call_GetTableauCacheModeDelegate();
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000F55C File Offset: 0x0000D75C
		public bool GetUIDebugMode()
		{
			return ScriptingInterfaceOfIConfig.call_GetUIDebugModeDelegate();
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000F568 File Offset: 0x0000D768
		public bool GetUIDoNotUseGeneratedPrefabs()
		{
			return ScriptingInterfaceOfIConfig.call_GetUIDoNotUseGeneratedPrefabsDelegate();
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000F574 File Offset: 0x0000D774
		public int GetVideoDeviceCount()
		{
			return ScriptingInterfaceOfIConfig.call_GetVideoDeviceCountDelegate();
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000F580 File Offset: 0x0000D780
		public string GetVideoDeviceName(int i)
		{
			if (ScriptingInterfaceOfIConfig.call_GetVideoDeviceNameDelegate(i) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000F597 File Offset: 0x0000D797
		public bool Is120HzAvailable()
		{
			return ScriptingInterfaceOfIConfig.call_Is120HzAvailableDelegate();
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000F5A3 File Offset: 0x0000D7A3
		public bool IsDlssAvailable()
		{
			return ScriptingInterfaceOfIConfig.call_IsDlssAvailableDelegate();
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000F5AF File Offset: 0x0000D7AF
		public void ReadRGLConfigFiles()
		{
			ScriptingInterfaceOfIConfig.call_ReadRGLConfigFilesDelegate();
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000F5BB File Offset: 0x0000D7BB
		public void RefreshOptionsData()
		{
			ScriptingInterfaceOfIConfig.call_RefreshOptionsDataDelegate();
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000F5C7 File Offset: 0x0000D7C7
		public int SaveRGLConfig()
		{
			return ScriptingInterfaceOfIConfig.call_SaveRGLConfigDelegate();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000F5D3 File Offset: 0x0000D7D3
		public void SetAutoConfigWrtHardware()
		{
			ScriptingInterfaceOfIConfig.call_SetAutoConfigWrtHardwareDelegate();
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000F5DF File Offset: 0x0000D7DF
		public void SetBrightness(float brightness)
		{
			ScriptingInterfaceOfIConfig.call_SetBrightnessDelegate(brightness);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000F5EC File Offset: 0x0000D7EC
		public void SetCustomResolution(int width, int height)
		{
			ScriptingInterfaceOfIConfig.call_SetCustomResolutionDelegate(width, height);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000F5FA File Offset: 0x0000D7FA
		public void SetDefaultGameConfig()
		{
			ScriptingInterfaceOfIConfig.call_SetDefaultGameConfigDelegate();
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000F606 File Offset: 0x0000D806
		public void SetRGLConfig(int type, float value)
		{
			ScriptingInterfaceOfIConfig.call_SetRGLConfigDelegate(type, value);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000F614 File Offset: 0x0000D814
		public void SetSharpenAmount(float sharpen_amount)
		{
			ScriptingInterfaceOfIConfig.call_SetSharpenAmountDelegate(sharpen_amount);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000F621 File Offset: 0x0000D821
		public void SetSoundDevice(int i)
		{
			ScriptingInterfaceOfIConfig.call_SetSoundDeviceDelegate(i);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000F62E File Offset: 0x0000D82E
		public void SetSoundPreset(int i)
		{
			ScriptingInterfaceOfIConfig.call_SetSoundPresetDelegate(i);
		}

		// Token: 0x0400004A RID: 74
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400004B RID: 75
		public static ScriptingInterfaceOfIConfig.ApplyDelegate call_ApplyDelegate;

		// Token: 0x0400004C RID: 76
		public static ScriptingInterfaceOfIConfig.ApplyConfigChangesDelegate call_ApplyConfigChangesDelegate;

		// Token: 0x0400004D RID: 77
		public static ScriptingInterfaceOfIConfig.AutoSaveInMinutesDelegate call_AutoSaveInMinutesDelegate;

		// Token: 0x0400004E RID: 78
		public static ScriptingInterfaceOfIConfig.CheckGFXSupportStatusDelegate call_CheckGFXSupportStatusDelegate;

		// Token: 0x0400004F RID: 79
		public static ScriptingInterfaceOfIConfig.GetAutoGFXQualityDelegate call_GetAutoGFXQualityDelegate;

		// Token: 0x04000050 RID: 80
		public static ScriptingInterfaceOfIConfig.GetCharacterDetailDelegate call_GetCharacterDetailDelegate;

		// Token: 0x04000051 RID: 81
		public static ScriptingInterfaceOfIConfig.GetCheatModeDelegate call_GetCheatModeDelegate;

		// Token: 0x04000052 RID: 82
		public static ScriptingInterfaceOfIConfig.GetCurrentSoundDeviceIndexDelegate call_GetCurrentSoundDeviceIndexDelegate;

		// Token: 0x04000053 RID: 83
		public static ScriptingInterfaceOfIConfig.GetDebugLoginPasswordDelegate call_GetDebugLoginPasswordDelegate;

		// Token: 0x04000054 RID: 84
		public static ScriptingInterfaceOfIConfig.GetDebugLoginUserNameDelegate call_GetDebugLoginUserNameDelegate;

		// Token: 0x04000055 RID: 85
		public static ScriptingInterfaceOfIConfig.GetDefaultRGLConfigDelegate call_GetDefaultRGLConfigDelegate;

		// Token: 0x04000056 RID: 86
		public static ScriptingInterfaceOfIConfig.GetDesktopResolutionDelegate call_GetDesktopResolutionDelegate;

		// Token: 0x04000057 RID: 87
		public static ScriptingInterfaceOfIConfig.GetDevelopmentModeDelegate call_GetDevelopmentModeDelegate;

		// Token: 0x04000058 RID: 88
		public static ScriptingInterfaceOfIConfig.GetDisableGuiMessagesDelegate call_GetDisableGuiMessagesDelegate;

		// Token: 0x04000059 RID: 89
		public static ScriptingInterfaceOfIConfig.GetDisableSoundDelegate call_GetDisableSoundDelegate;

		// Token: 0x0400005A RID: 90
		public static ScriptingInterfaceOfIConfig.GetDlssOptionCountDelegate call_GetDlssOptionCountDelegate;

		// Token: 0x0400005B RID: 91
		public static ScriptingInterfaceOfIConfig.GetDlssTechniqueDelegate call_GetDlssTechniqueDelegate;

		// Token: 0x0400005C RID: 92
		public static ScriptingInterfaceOfIConfig.GetDoLocalizationCheckAtStartupDelegate call_GetDoLocalizationCheckAtStartupDelegate;

		// Token: 0x0400005D RID: 93
		public static ScriptingInterfaceOfIConfig.GetEnableClothSimulationDelegate call_GetEnableClothSimulationDelegate;

		// Token: 0x0400005E RID: 94
		public static ScriptingInterfaceOfIConfig.GetEnableEditModeDelegate call_GetEnableEditModeDelegate;

		// Token: 0x0400005F RID: 95
		public static ScriptingInterfaceOfIConfig.GetInvertMouseDelegate call_GetInvertMouseDelegate;

		// Token: 0x04000060 RID: 96
		public static ScriptingInterfaceOfIConfig.GetLastOpenedSceneDelegate call_GetLastOpenedSceneDelegate;

		// Token: 0x04000061 RID: 97
		public static ScriptingInterfaceOfIConfig.GetLocalizationDebugModeDelegate call_GetLocalizationDebugModeDelegate;

		// Token: 0x04000062 RID: 98
		public static ScriptingInterfaceOfIConfig.GetMonitorDeviceCountDelegate call_GetMonitorDeviceCountDelegate;

		// Token: 0x04000063 RID: 99
		public static ScriptingInterfaceOfIConfig.GetMonitorDeviceNameDelegate call_GetMonitorDeviceNameDelegate;

		// Token: 0x04000064 RID: 100
		public static ScriptingInterfaceOfIConfig.GetRefreshRateAtIndexDelegate call_GetRefreshRateAtIndexDelegate;

		// Token: 0x04000065 RID: 101
		public static ScriptingInterfaceOfIConfig.GetRefreshRateCountDelegate call_GetRefreshRateCountDelegate;

		// Token: 0x04000066 RID: 102
		public static ScriptingInterfaceOfIConfig.GetResolutionDelegate call_GetResolutionDelegate;

		// Token: 0x04000067 RID: 103
		public static ScriptingInterfaceOfIConfig.GetResolutionAtIndexDelegate call_GetResolutionAtIndexDelegate;

		// Token: 0x04000068 RID: 104
		public static ScriptingInterfaceOfIConfig.GetResolutionCountDelegate call_GetResolutionCountDelegate;

		// Token: 0x04000069 RID: 105
		public static ScriptingInterfaceOfIConfig.GetRGLConfigDelegate call_GetRGLConfigDelegate;

		// Token: 0x0400006A RID: 106
		public static ScriptingInterfaceOfIConfig.GetRGLConfigForDefaultSettingsDelegate call_GetRGLConfigForDefaultSettingsDelegate;

		// Token: 0x0400006B RID: 107
		public static ScriptingInterfaceOfIConfig.GetSoundDeviceCountDelegate call_GetSoundDeviceCountDelegate;

		// Token: 0x0400006C RID: 108
		public static ScriptingInterfaceOfIConfig.GetSoundDeviceNameDelegate call_GetSoundDeviceNameDelegate;

		// Token: 0x0400006D RID: 109
		public static ScriptingInterfaceOfIConfig.GetTableauCacheModeDelegate call_GetTableauCacheModeDelegate;

		// Token: 0x0400006E RID: 110
		public static ScriptingInterfaceOfIConfig.GetUIDebugModeDelegate call_GetUIDebugModeDelegate;

		// Token: 0x0400006F RID: 111
		public static ScriptingInterfaceOfIConfig.GetUIDoNotUseGeneratedPrefabsDelegate call_GetUIDoNotUseGeneratedPrefabsDelegate;

		// Token: 0x04000070 RID: 112
		public static ScriptingInterfaceOfIConfig.GetVideoDeviceCountDelegate call_GetVideoDeviceCountDelegate;

		// Token: 0x04000071 RID: 113
		public static ScriptingInterfaceOfIConfig.GetVideoDeviceNameDelegate call_GetVideoDeviceNameDelegate;

		// Token: 0x04000072 RID: 114
		public static ScriptingInterfaceOfIConfig.Is120HzAvailableDelegate call_Is120HzAvailableDelegate;

		// Token: 0x04000073 RID: 115
		public static ScriptingInterfaceOfIConfig.IsDlssAvailableDelegate call_IsDlssAvailableDelegate;

		// Token: 0x04000074 RID: 116
		public static ScriptingInterfaceOfIConfig.ReadRGLConfigFilesDelegate call_ReadRGLConfigFilesDelegate;

		// Token: 0x04000075 RID: 117
		public static ScriptingInterfaceOfIConfig.RefreshOptionsDataDelegate call_RefreshOptionsDataDelegate;

		// Token: 0x04000076 RID: 118
		public static ScriptingInterfaceOfIConfig.SaveRGLConfigDelegate call_SaveRGLConfigDelegate;

		// Token: 0x04000077 RID: 119
		public static ScriptingInterfaceOfIConfig.SetAutoConfigWrtHardwareDelegate call_SetAutoConfigWrtHardwareDelegate;

		// Token: 0x04000078 RID: 120
		public static ScriptingInterfaceOfIConfig.SetBrightnessDelegate call_SetBrightnessDelegate;

		// Token: 0x04000079 RID: 121
		public static ScriptingInterfaceOfIConfig.SetCustomResolutionDelegate call_SetCustomResolutionDelegate;

		// Token: 0x0400007A RID: 122
		public static ScriptingInterfaceOfIConfig.SetDefaultGameConfigDelegate call_SetDefaultGameConfigDelegate;

		// Token: 0x0400007B RID: 123
		public static ScriptingInterfaceOfIConfig.SetRGLConfigDelegate call_SetRGLConfigDelegate;

		// Token: 0x0400007C RID: 124
		public static ScriptingInterfaceOfIConfig.SetSharpenAmountDelegate call_SetSharpenAmountDelegate;

		// Token: 0x0400007D RID: 125
		public static ScriptingInterfaceOfIConfig.SetSoundDeviceDelegate call_SetSoundDeviceDelegate;

		// Token: 0x0400007E RID: 126
		public static ScriptingInterfaceOfIConfig.SetSoundPresetDelegate call_SetSoundPresetDelegate;

		// Token: 0x020000CC RID: 204
		// (Invoke) Token: 0x06000949 RID: 2377
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ApplyDelegate(int texture_budget, int sharpen_amount, int hdr, int dof_mode, int motion_blur, int ssr, int size, int texture_filtering, int trail_amount, int dynamic_resolution_target);

		// Token: 0x020000CD RID: 205
		// (Invoke) Token: 0x0600094D RID: 2381
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ApplyConfigChangesDelegate([MarshalAs(UnmanagedType.U1)] bool resizeWindow);

		// Token: 0x020000CE RID: 206
		// (Invoke) Token: 0x06000951 RID: 2385
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int AutoSaveInMinutesDelegate();

		// Token: 0x020000CF RID: 207
		// (Invoke) Token: 0x06000955 RID: 2389
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool CheckGFXSupportStatusDelegate(int enum_id);

		// Token: 0x020000D0 RID: 208
		// (Invoke) Token: 0x06000959 RID: 2393
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetAutoGFXQualityDelegate();

		// Token: 0x020000D1 RID: 209
		// (Invoke) Token: 0x0600095D RID: 2397
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetCharacterDetailDelegate();

		// Token: 0x020000D2 RID: 210
		// (Invoke) Token: 0x06000961 RID: 2401
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetCheatModeDelegate();

		// Token: 0x020000D3 RID: 211
		// (Invoke) Token: 0x06000965 RID: 2405
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetCurrentSoundDeviceIndexDelegate();

		// Token: 0x020000D4 RID: 212
		// (Invoke) Token: 0x06000969 RID: 2409
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetDebugLoginPasswordDelegate();

		// Token: 0x020000D5 RID: 213
		// (Invoke) Token: 0x0600096D RID: 2413
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetDebugLoginUserNameDelegate();

		// Token: 0x020000D6 RID: 214
		// (Invoke) Token: 0x06000971 RID: 2417
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetDefaultRGLConfigDelegate(int type);

		// Token: 0x020000D7 RID: 215
		// (Invoke) Token: 0x06000975 RID: 2421
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetDesktopResolutionDelegate(ref int width, ref int height);

		// Token: 0x020000D8 RID: 216
		// (Invoke) Token: 0x06000979 RID: 2425
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetDevelopmentModeDelegate();

		// Token: 0x020000D9 RID: 217
		// (Invoke) Token: 0x0600097D RID: 2429
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetDisableGuiMessagesDelegate();

		// Token: 0x020000DA RID: 218
		// (Invoke) Token: 0x06000981 RID: 2433
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetDisableSoundDelegate();

		// Token: 0x020000DB RID: 219
		// (Invoke) Token: 0x06000985 RID: 2437
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetDlssOptionCountDelegate();

		// Token: 0x020000DC RID: 220
		// (Invoke) Token: 0x06000989 RID: 2441
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetDlssTechniqueDelegate();

		// Token: 0x020000DD RID: 221
		// (Invoke) Token: 0x0600098D RID: 2445
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetDoLocalizationCheckAtStartupDelegate();

		// Token: 0x020000DE RID: 222
		// (Invoke) Token: 0x06000991 RID: 2449
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetEnableClothSimulationDelegate();

		// Token: 0x020000DF RID: 223
		// (Invoke) Token: 0x06000995 RID: 2453
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetEnableEditModeDelegate();

		// Token: 0x020000E0 RID: 224
		// (Invoke) Token: 0x06000999 RID: 2457
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetInvertMouseDelegate();

		// Token: 0x020000E1 RID: 225
		// (Invoke) Token: 0x0600099D RID: 2461
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetLastOpenedSceneDelegate();

		// Token: 0x020000E2 RID: 226
		// (Invoke) Token: 0x060009A1 RID: 2465
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetLocalizationDebugModeDelegate();

		// Token: 0x020000E3 RID: 227
		// (Invoke) Token: 0x060009A5 RID: 2469
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetMonitorDeviceCountDelegate();

		// Token: 0x020000E4 RID: 228
		// (Invoke) Token: 0x060009A9 RID: 2473
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetMonitorDeviceNameDelegate(int i);

		// Token: 0x020000E5 RID: 229
		// (Invoke) Token: 0x060009AD RID: 2477
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetRefreshRateAtIndexDelegate(int index);

		// Token: 0x020000E6 RID: 230
		// (Invoke) Token: 0x060009B1 RID: 2481
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetRefreshRateCountDelegate();

		// Token: 0x020000E7 RID: 231
		// (Invoke) Token: 0x060009B5 RID: 2485
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetResolutionDelegate(ref int width, ref int height);

		// Token: 0x020000E8 RID: 232
		// (Invoke) Token: 0x060009B9 RID: 2489
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec2 GetResolutionAtIndexDelegate(int index);

		// Token: 0x020000E9 RID: 233
		// (Invoke) Token: 0x060009BD RID: 2493
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetResolutionCountDelegate();

		// Token: 0x020000EA RID: 234
		// (Invoke) Token: 0x060009C1 RID: 2497
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetRGLConfigDelegate(int type);

		// Token: 0x020000EB RID: 235
		// (Invoke) Token: 0x060009C5 RID: 2501
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetRGLConfigForDefaultSettingsDelegate(int type, int defaultSettings);

		// Token: 0x020000EC RID: 236
		// (Invoke) Token: 0x060009C9 RID: 2505
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetSoundDeviceCountDelegate();

		// Token: 0x020000ED RID: 237
		// (Invoke) Token: 0x060009CD RID: 2509
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetSoundDeviceNameDelegate(int i);

		// Token: 0x020000EE RID: 238
		// (Invoke) Token: 0x060009D1 RID: 2513
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetTableauCacheModeDelegate();

		// Token: 0x020000EF RID: 239
		// (Invoke) Token: 0x060009D5 RID: 2517
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetUIDebugModeDelegate();

		// Token: 0x020000F0 RID: 240
		// (Invoke) Token: 0x060009D9 RID: 2521
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetUIDoNotUseGeneratedPrefabsDelegate();

		// Token: 0x020000F1 RID: 241
		// (Invoke) Token: 0x060009DD RID: 2525
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetVideoDeviceCountDelegate();

		// Token: 0x020000F2 RID: 242
		// (Invoke) Token: 0x060009E1 RID: 2529
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetVideoDeviceNameDelegate(int i);

		// Token: 0x020000F3 RID: 243
		// (Invoke) Token: 0x060009E5 RID: 2533
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool Is120HzAvailableDelegate();

		// Token: 0x020000F4 RID: 244
		// (Invoke) Token: 0x060009E9 RID: 2537
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsDlssAvailableDelegate();

		// Token: 0x020000F5 RID: 245
		// (Invoke) Token: 0x060009ED RID: 2541
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReadRGLConfigFilesDelegate();

		// Token: 0x020000F6 RID: 246
		// (Invoke) Token: 0x060009F1 RID: 2545
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RefreshOptionsDataDelegate();

		// Token: 0x020000F7 RID: 247
		// (Invoke) Token: 0x060009F5 RID: 2549
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int SaveRGLConfigDelegate();

		// Token: 0x020000F8 RID: 248
		// (Invoke) Token: 0x060009F9 RID: 2553
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAutoConfigWrtHardwareDelegate();

		// Token: 0x020000F9 RID: 249
		// (Invoke) Token: 0x060009FD RID: 2557
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetBrightnessDelegate(float brightness);

		// Token: 0x020000FA RID: 250
		// (Invoke) Token: 0x06000A01 RID: 2561
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetCustomResolutionDelegate(int width, int height);

		// Token: 0x020000FB RID: 251
		// (Invoke) Token: 0x06000A05 RID: 2565
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDefaultGameConfigDelegate();

		// Token: 0x020000FC RID: 252
		// (Invoke) Token: 0x06000A09 RID: 2569
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetRGLConfigDelegate(int type, float value);

		// Token: 0x020000FD RID: 253
		// (Invoke) Token: 0x06000A0D RID: 2573
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSharpenAmountDelegate(float sharpen_amount);

		// Token: 0x020000FE RID: 254
		// (Invoke) Token: 0x06000A11 RID: 2577
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSoundDeviceDelegate(int i);

		// Token: 0x020000FF RID: 255
		// (Invoke) Token: 0x06000A15 RID: 2581
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSoundPresetDelegate(int i);
	}
}
