using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.Engine.Options
{
	// Token: 0x020000AB RID: 171
	public class NativeOptions
	{
		// Token: 0x06000F5A RID: 3930 RVA: 0x000121E0 File Offset: 0x000103E0
		public static string GetGFXPresetName(NativeOptions.ConfigQuality presetIndex)
		{
			switch (presetIndex)
			{
			case NativeOptions.ConfigQuality.GFXVeryLow:
				return "1";
			case NativeOptions.ConfigQuality.GFXLow:
				return "2";
			case NativeOptions.ConfigQuality.GFXMedium:
				return "3";
			case NativeOptions.ConfigQuality.GFXHigh:
				return "4";
			case NativeOptions.ConfigQuality.GFXVeryHigh:
				return "5";
			case NativeOptions.ConfigQuality.GFXCustom:
				return "Custom";
			default:
				return "Unknown";
			}
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x00012236 File Offset: 0x00010436
		public static bool IsGFXOptionChangeable(NativeOptions.ConfigQuality config)
		{
			return config < NativeOptions.ConfigQuality.GFXCustom;
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x0001223C File Offset: 0x0001043C
		private static void CorrectSelection(List<NativeOptionData> audioOptions)
		{
			foreach (NativeOptionData nativeOptionData in audioOptions)
			{
				if (nativeOptionData.Type == NativeOptions.NativeOptionsType.SoundDevice)
				{
					int num = 0;
					for (int i = 0; i < NativeOptions.GetSoundDeviceCount(); i++)
					{
						if (NativeOptions.GetSoundDeviceName(i) != "")
						{
							num = i;
						}
					}
					if (nativeOptionData.GetValue(false) > (float)num)
					{
						NativeOptions.SetConfig(NativeOptions.NativeOptionsType.SoundDevice, 0f);
						nativeOptionData.SetValue(0f);
					}
				}
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000F5D RID: 3933 RVA: 0x000122D4 File Offset: 0x000104D4
		// (remove) Token: 0x06000F5E RID: 3934 RVA: 0x00012308 File Offset: 0x00010508
		public static event Action OnNativeOptionsApplied;

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000F5F RID: 3935 RVA: 0x0001233C File Offset: 0x0001053C
		public static List<NativeOptionData> VideoOptions
		{
			get
			{
				if (NativeOptions._videoOptions == null)
				{
					NativeOptions._videoOptions = new List<NativeOptionData>();
					for (NativeOptions.NativeOptionsType nativeOptionsType = NativeOptions.NativeOptionsType.None; nativeOptionsType < NativeOptions.NativeOptionsType.TotalOptions; nativeOptionsType++)
					{
						if (nativeOptionsType - NativeOptions.NativeOptionsType.DisplayMode <= 7 || nativeOptionsType == NativeOptions.NativeOptionsType.SharpenAmount)
						{
							NativeOptions._videoOptions.Add(new NativeNumericOptionData(nativeOptionsType));
						}
					}
				}
				return NativeOptions._videoOptions;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000F60 RID: 3936 RVA: 0x00012388 File Offset: 0x00010588
		public static List<NativeOptionData> GraphicsOptions
		{
			get
			{
				if (NativeOptions._graphicsOptions == null)
				{
					NativeOptions._graphicsOptions = new List<NativeOptionData>();
					for (NativeOptions.NativeOptionsType nativeOptionsType = NativeOptions.NativeOptionsType.None; nativeOptionsType < NativeOptions.NativeOptionsType.TotalOptions; nativeOptionsType++)
					{
						switch (nativeOptionsType)
						{
						case NativeOptions.NativeOptionsType.MaxSimultaneousSoundEventCount:
						case NativeOptions.NativeOptionsType.OverAll:
						case NativeOptions.NativeOptionsType.ShaderQuality:
						case NativeOptions.NativeOptionsType.TextureBudget:
						case NativeOptions.NativeOptionsType.TextureQuality:
						case NativeOptions.NativeOptionsType.ShadowmapResolution:
						case NativeOptions.NativeOptionsType.ShadowmapType:
						case NativeOptions.NativeOptionsType.ShadowmapFiltering:
						case NativeOptions.NativeOptionsType.ParticleDetail:
						case NativeOptions.NativeOptionsType.ParticleQuality:
						case NativeOptions.NativeOptionsType.FoliageQuality:
						case NativeOptions.NativeOptionsType.CharacterDetail:
						case NativeOptions.NativeOptionsType.EnvironmentDetail:
						case NativeOptions.NativeOptionsType.TerrainQuality:
						case NativeOptions.NativeOptionsType.NumberOfRagDolls:
						case NativeOptions.NativeOptionsType.AnimationSamplingQuality:
						case NativeOptions.NativeOptionsType.Occlusion:
						case NativeOptions.NativeOptionsType.TextureFiltering:
						case NativeOptions.NativeOptionsType.WaterQuality:
						case NativeOptions.NativeOptionsType.SSRQuality:
						case NativeOptions.NativeOptionsType.Antialiasing:
						case NativeOptions.NativeOptionsType.LightingQuality:
						case NativeOptions.NativeOptionsType.DecalQuality:
						case NativeOptions.NativeOptionsType.PhysicsTickRate:
							NativeOptions._graphicsOptions.Add(new NativeSelectionOptionData(nativeOptionsType));
							break;
						case NativeOptions.NativeOptionsType.DLSS:
							if (NativeOptions.GetIsDLSSAvailable())
							{
								NativeOptions._graphicsOptions.Add(new NativeSelectionOptionData(nativeOptionsType));
							}
							break;
						case NativeOptions.NativeOptionsType.DepthOfField:
						case NativeOptions.NativeOptionsType.SSR:
						case NativeOptions.NativeOptionsType.ClothSimulation:
						case NativeOptions.NativeOptionsType.InteractiveGrass:
						case NativeOptions.NativeOptionsType.SunShafts:
						case NativeOptions.NativeOptionsType.SSSSS:
						case NativeOptions.NativeOptionsType.Tesselation:
						case NativeOptions.NativeOptionsType.Bloom:
						case NativeOptions.NativeOptionsType.FilmGrain:
						case NativeOptions.NativeOptionsType.MotionBlur:
						case NativeOptions.NativeOptionsType.DynamicResolution:
							NativeOptions._graphicsOptions.Add(new NativeBooleanOptionData(nativeOptionsType));
							break;
						case NativeOptions.NativeOptionsType.PostFXLensFlare:
							if (EngineApplicationInterface.IConfig.CheckGFXSupportStatus(62))
							{
								NativeOptions._graphicsOptions.Add(new NativeBooleanOptionData(nativeOptionsType));
							}
							break;
						case NativeOptions.NativeOptionsType.PostFXStreaks:
							if (EngineApplicationInterface.IConfig.CheckGFXSupportStatus(63))
							{
								NativeOptions._graphicsOptions.Add(new NativeBooleanOptionData(nativeOptionsType));
							}
							break;
						case NativeOptions.NativeOptionsType.PostFXChromaticAberration:
							if (EngineApplicationInterface.IConfig.CheckGFXSupportStatus(64))
							{
								NativeOptions._graphicsOptions.Add(new NativeBooleanOptionData(nativeOptionsType));
							}
							break;
						case NativeOptions.NativeOptionsType.PostFXVignette:
							if (EngineApplicationInterface.IConfig.CheckGFXSupportStatus(65))
							{
								NativeOptions._graphicsOptions.Add(new NativeBooleanOptionData(nativeOptionsType));
							}
							break;
						case NativeOptions.NativeOptionsType.PostFXHexagonVignette:
							if (EngineApplicationInterface.IConfig.CheckGFXSupportStatus(66))
							{
								NativeOptions._graphicsOptions.Add(new NativeBooleanOptionData(nativeOptionsType));
							}
							break;
						case NativeOptions.NativeOptionsType.DynamicResolutionTarget:
							NativeOptions._graphicsOptions.Add(new NativeNumericOptionData(nativeOptionsType));
							break;
						}
					}
				}
				return NativeOptions._graphicsOptions;
			}
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x000125E4 File Offset: 0x000107E4
		public static void ReadRGLConfigFiles()
		{
			EngineApplicationInterface.IConfig.ReadRGLConfigFiles();
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x000125F0 File Offset: 0x000107F0
		public static float GetConfig(NativeOptions.NativeOptionsType type)
		{
			return EngineApplicationInterface.IConfig.GetRGLConfig((int)type);
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x000125FD File Offset: 0x000107FD
		public static float GetDefaultConfig(NativeOptions.NativeOptionsType type)
		{
			return EngineApplicationInterface.IConfig.GetDefaultRGLConfig((int)type);
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0001260A File Offset: 0x0001080A
		public static float GetDefaultConfigForOverallSettings(NativeOptions.NativeOptionsType type, int config)
		{
			return EngineApplicationInterface.IConfig.GetRGLConfigForDefaultSettings((int)type, config);
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x00012618 File Offset: 0x00010818
		public static int GetGameKeys(int keyType, int i)
		{
			Debug.FailedAssert("This is not implemented. Changed from Exception to not cause crash.", "C:\\BuildAgent\\work\\mb3\\Source\\Engine\\TaleWorlds.Engine\\Options\\NativeOptions\\NativeOptions.cs", "GetGameKeys", 330);
			return 0;
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x00012634 File Offset: 0x00010834
		public static string GetSoundDeviceName(int i)
		{
			return EngineApplicationInterface.IConfig.GetSoundDeviceName(i);
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x00012641 File Offset: 0x00010841
		public static string GetMonitorDeviceName(int i)
		{
			return EngineApplicationInterface.IConfig.GetMonitorDeviceName(i);
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x0001264E File Offset: 0x0001084E
		public static string GetVideoDeviceName(int i)
		{
			return EngineApplicationInterface.IConfig.GetVideoDeviceName(i);
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x0001265B File Offset: 0x0001085B
		public static int GetSoundDeviceCount()
		{
			return EngineApplicationInterface.IConfig.GetSoundDeviceCount();
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x00012667 File Offset: 0x00010867
		public static int GetMonitorDeviceCount()
		{
			return EngineApplicationInterface.IConfig.GetMonitorDeviceCount();
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x00012673 File Offset: 0x00010873
		public static int GetVideoDeviceCount()
		{
			return EngineApplicationInterface.IConfig.GetVideoDeviceCount();
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x0001267F File Offset: 0x0001087F
		public static int GetResolutionCount()
		{
			return EngineApplicationInterface.IConfig.GetResolutionCount();
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0001268B File Offset: 0x0001088B
		public static void RefreshOptionsData()
		{
			EngineApplicationInterface.IConfig.RefreshOptionsData();
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x00012697 File Offset: 0x00010897
		public static int GetRefreshRateCount()
		{
			return EngineApplicationInterface.IConfig.GetRefreshRateCount();
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x000126A3 File Offset: 0x000108A3
		public static int GetRefreshRateAtIndex(int index)
		{
			return EngineApplicationInterface.IConfig.GetRefreshRateAtIndex(index);
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x000126B0 File Offset: 0x000108B0
		public static void SetCustomResolution(int width, int height)
		{
			EngineApplicationInterface.IConfig.SetCustomResolution(width, height);
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x000126BE File Offset: 0x000108BE
		public static void GetResolution(ref int width, ref int height)
		{
			EngineApplicationInterface.IConfig.GetDesktopResolution(ref width, ref height);
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x000126CC File Offset: 0x000108CC
		public static void GetDesktopResolution(ref int width, ref int height)
		{
			EngineApplicationInterface.IConfig.GetDesktopResolution(ref width, ref height);
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x000126DA File Offset: 0x000108DA
		public static Vec2 GetResolutionAtIndex(int index)
		{
			return EngineApplicationInterface.IConfig.GetResolutionAtIndex(index);
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x000126E7 File Offset: 0x000108E7
		public static int GetDLSSTechnique()
		{
			return EngineApplicationInterface.IConfig.GetDlssTechnique();
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x000126F3 File Offset: 0x000108F3
		public static bool Is120HzAvailable()
		{
			return EngineApplicationInterface.IConfig.Is120HzAvailable();
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x000126FF File Offset: 0x000108FF
		public static int GetDLSSOptionCount()
		{
			return EngineApplicationInterface.IConfig.GetDlssOptionCount();
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0001270B File Offset: 0x0001090B
		public static bool GetIsDLSSAvailable()
		{
			return EngineApplicationInterface.IConfig.IsDlssAvailable();
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x00012717 File Offset: 0x00010917
		public static bool CheckGFXSupportStatus(int enumType)
		{
			return EngineApplicationInterface.IConfig.CheckGFXSupportStatus(enumType);
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x00012724 File Offset: 0x00010924
		public static void SetConfig(NativeOptions.NativeOptionsType type, float value)
		{
			EngineApplicationInterface.IConfig.SetRGLConfig((int)type, value);
			NativeOptions.OnNativeOptionChangedDelegate onNativeOptionChanged = NativeOptions.OnNativeOptionChanged;
			if (onNativeOptionChanged == null)
			{
				return;
			}
			onNativeOptionChanged(type);
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x00012742 File Offset: 0x00010942
		public static void ApplyConfigChanges(bool resizeWindow)
		{
			EngineApplicationInterface.IConfig.ApplyConfigChanges(resizeWindow);
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0001274F File Offset: 0x0001094F
		public static void SetGameKeys(int keyType, int index, int key)
		{
			Debug.FailedAssert("This is not implemented. Changed from Exception to not cause crash.", "C:\\BuildAgent\\work\\mb3\\Source\\Engine\\TaleWorlds.Engine\\Options\\NativeOptions\\NativeOptions.cs", "SetGameKeys", 443);
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0001276C File Offset: 0x0001096C
		public static void Apply(int texture_budget, int sharpen_amount, int hdr, int dof_mode, int motion_blur, int ssr, int size, int texture_filtering, int trail_amount, int dynamic_resolution_target)
		{
			EngineApplicationInterface.IConfig.Apply(texture_budget, sharpen_amount, hdr, dof_mode, motion_blur, ssr, size, texture_filtering, trail_amount, dynamic_resolution_target);
			Action onNativeOptionsApplied = NativeOptions.OnNativeOptionsApplied;
			if (onNativeOptionsApplied == null)
			{
				return;
			}
			onNativeOptionsApplied();
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x000127A2 File Offset: 0x000109A2
		public static SaveResult SaveConfig()
		{
			return (SaveResult)EngineApplicationInterface.IConfig.SaveRGLConfig();
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x000127AE File Offset: 0x000109AE
		public static void SetBrightness(float gamma)
		{
			EngineApplicationInterface.IConfig.SetBrightness(gamma);
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x000127BB File Offset: 0x000109BB
		public static void SetDefaultGameKeys()
		{
			Debug.FailedAssert("This is not implemented. Changed from Exception to not cause crash.", "C:\\BuildAgent\\work\\mb3\\Source\\Engine\\TaleWorlds.Engine\\Options\\NativeOptions\\NativeOptions.cs", "SetDefaultGameKeys", 468);
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x000127D6 File Offset: 0x000109D6
		public static void SetDefaultGameConfig()
		{
			EngineApplicationInterface.IConfig.SetDefaultGameConfig();
		}

		// Token: 0x04000220 RID: 544
		public static NativeOptions.OnNativeOptionChangedDelegate OnNativeOptionChanged;

		// Token: 0x04000222 RID: 546
		private static List<NativeOptionData> _videoOptions;

		// Token: 0x04000223 RID: 547
		private static List<NativeOptionData> _graphicsOptions;

		// Token: 0x020000E2 RID: 226
		public enum ConfigQuality
		{
			// Token: 0x040004E1 RID: 1249
			GFXVeryLow,
			// Token: 0x040004E2 RID: 1250
			GFXLow,
			// Token: 0x040004E3 RID: 1251
			GFXMedium,
			// Token: 0x040004E4 RID: 1252
			GFXHigh,
			// Token: 0x040004E5 RID: 1253
			GFXVeryHigh,
			// Token: 0x040004E6 RID: 1254
			GFXCustom
		}

		// Token: 0x020000E3 RID: 227
		public enum NativeOptionsType
		{
			// Token: 0x040004E8 RID: 1256
			None = -1,
			// Token: 0x040004E9 RID: 1257
			MasterVolume,
			// Token: 0x040004EA RID: 1258
			SoundVolume,
			// Token: 0x040004EB RID: 1259
			MusicVolume,
			// Token: 0x040004EC RID: 1260
			VoiceChatVolume,
			// Token: 0x040004ED RID: 1261
			VoiceOverVolume,
			// Token: 0x040004EE RID: 1262
			SoundDevice,
			// Token: 0x040004EF RID: 1263
			MaxSimultaneousSoundEventCount,
			// Token: 0x040004F0 RID: 1264
			SoundPreset,
			// Token: 0x040004F1 RID: 1265
			KeepSoundInBackground,
			// Token: 0x040004F2 RID: 1266
			SoundOcclusion,
			// Token: 0x040004F3 RID: 1267
			MouseSensitivity,
			// Token: 0x040004F4 RID: 1268
			InvertMouseYAxis,
			// Token: 0x040004F5 RID: 1269
			MouseYMovementScale,
			// Token: 0x040004F6 RID: 1270
			TrailAmount,
			// Token: 0x040004F7 RID: 1271
			EnableVibration,
			// Token: 0x040004F8 RID: 1272
			EnableGyroAssistedAim,
			// Token: 0x040004F9 RID: 1273
			GyroAimSensitivity,
			// Token: 0x040004FA RID: 1274
			EnableTouchpadMouse,
			// Token: 0x040004FB RID: 1275
			EnableAlternateAiming,
			// Token: 0x040004FC RID: 1276
			DisplayMode,
			// Token: 0x040004FD RID: 1277
			SelectedMonitor,
			// Token: 0x040004FE RID: 1278
			SelectedAdapter,
			// Token: 0x040004FF RID: 1279
			ScreenResolution,
			// Token: 0x04000500 RID: 1280
			RefreshRate,
			// Token: 0x04000501 RID: 1281
			ResolutionScale,
			// Token: 0x04000502 RID: 1282
			FrameLimiter,
			// Token: 0x04000503 RID: 1283
			VSync,
			// Token: 0x04000504 RID: 1284
			Brightness,
			// Token: 0x04000505 RID: 1285
			OverAll,
			// Token: 0x04000506 RID: 1286
			ShaderQuality,
			// Token: 0x04000507 RID: 1287
			TextureBudget,
			// Token: 0x04000508 RID: 1288
			TextureQuality,
			// Token: 0x04000509 RID: 1289
			ShadowmapResolution,
			// Token: 0x0400050A RID: 1290
			ShadowmapType,
			// Token: 0x0400050B RID: 1291
			ShadowmapFiltering,
			// Token: 0x0400050C RID: 1292
			ParticleDetail,
			// Token: 0x0400050D RID: 1293
			ParticleQuality,
			// Token: 0x0400050E RID: 1294
			FoliageQuality,
			// Token: 0x0400050F RID: 1295
			CharacterDetail,
			// Token: 0x04000510 RID: 1296
			EnvironmentDetail,
			// Token: 0x04000511 RID: 1297
			TerrainQuality,
			// Token: 0x04000512 RID: 1298
			NumberOfRagDolls,
			// Token: 0x04000513 RID: 1299
			AnimationSamplingQuality,
			// Token: 0x04000514 RID: 1300
			Occlusion,
			// Token: 0x04000515 RID: 1301
			TextureFiltering,
			// Token: 0x04000516 RID: 1302
			WaterQuality,
			// Token: 0x04000517 RID: 1303
			SSRQuality,
			// Token: 0x04000518 RID: 1304
			Antialiasing,
			// Token: 0x04000519 RID: 1305
			DLSS,
			// Token: 0x0400051A RID: 1306
			LightingQuality,
			// Token: 0x0400051B RID: 1307
			DecalQuality,
			// Token: 0x0400051C RID: 1308
			DepthOfField,
			// Token: 0x0400051D RID: 1309
			SSR,
			// Token: 0x0400051E RID: 1310
			ClothSimulation,
			// Token: 0x0400051F RID: 1311
			InteractiveGrass,
			// Token: 0x04000520 RID: 1312
			SunShafts,
			// Token: 0x04000521 RID: 1313
			SSSSS,
			// Token: 0x04000522 RID: 1314
			Tesselation,
			// Token: 0x04000523 RID: 1315
			Bloom,
			// Token: 0x04000524 RID: 1316
			FilmGrain,
			// Token: 0x04000525 RID: 1317
			MotionBlur,
			// Token: 0x04000526 RID: 1318
			SharpenAmount,
			// Token: 0x04000527 RID: 1319
			PostFXLensFlare,
			// Token: 0x04000528 RID: 1320
			PostFXStreaks,
			// Token: 0x04000529 RID: 1321
			PostFXChromaticAberration,
			// Token: 0x0400052A RID: 1322
			PostFXVignette,
			// Token: 0x0400052B RID: 1323
			PostFXHexagonVignette,
			// Token: 0x0400052C RID: 1324
			BrightnessMin,
			// Token: 0x0400052D RID: 1325
			BrightnessMax,
			// Token: 0x0400052E RID: 1326
			BrightnessCalibrated,
			// Token: 0x0400052F RID: 1327
			ExposureCompensation,
			// Token: 0x04000530 RID: 1328
			DynamicResolution,
			// Token: 0x04000531 RID: 1329
			DynamicResolutionTarget,
			// Token: 0x04000532 RID: 1330
			FSR,
			// Token: 0x04000533 RID: 1331
			PhysicsTickRate,
			// Token: 0x04000534 RID: 1332
			NumOfOptionTypes,
			// Token: 0x04000535 RID: 1333
			TotalOptions
		}

		// Token: 0x020000E4 RID: 228
		// (Invoke) Token: 0x06001060 RID: 4192
		public delegate void OnNativeOptionChangedDelegate(NativeOptions.NativeOptionsType changedNativeOptionsType);
	}
}
