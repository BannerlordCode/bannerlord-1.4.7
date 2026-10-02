using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine.Options;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Options.ManagedOptions;

namespace TaleWorlds.MountAndBlade.Options
{
	// Token: 0x02000398 RID: 920
	public static class OptionsProvider
	{
		// Token: 0x06003495 RID: 13461 RVA: 0x000D8F7F File Offset: 0x000D717F
		public static OptionCategory GetVideoOptionCategory(bool isMainMenu, Action onBrightnessClick, Action onExposureClick, Action onBenchmarkClick)
		{
			return new OptionCategory(OptionsProvider.GetVideoGeneralOptions(isMainMenu, onBrightnessClick, onExposureClick, onBenchmarkClick), OptionsProvider.GetVideoOptionGroups());
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x000D8F94 File Offset: 0x000D7194
		private static IEnumerable<IOptionData> GetVideoGeneralOptions(bool isMainMenu, Action onBrightnessClick, Action onExposureClick, Action onBenchmarkClick)
		{
			if (isMainMenu)
			{
				yield return new ActionOptionData("Benchmark", onBenchmarkClick);
			}
			yield return new ActionOptionData(NativeOptions.NativeOptionsType.Brightness, onBrightnessClick);
			yield return new ActionOptionData(NativeOptions.NativeOptionsType.ExposureCompensation, onExposureClick);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.SelectedMonitor);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.SelectedAdapter);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.DisplayMode);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.ScreenResolution);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.RefreshRate);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.VSync);
			yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.ForceVSyncInMenus);
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.FrameLimiter);
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.SharpenAmount);
			yield break;
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x000D8FB9 File Offset: 0x000D71B9
		private static IEnumerable<IOptionData> GetPerformanceGeneralOptions(bool isMultiplayer)
		{
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.OverAll);
			yield break;
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x000D8FC2 File Offset: 0x000D71C2
		private static IEnumerable<OptionGroup> GetVideoOptionGroups()
		{
			return null;
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x000D8FC5 File Offset: 0x000D71C5
		public static OptionCategory GetPerformanceOptionCategory(bool isMultiplayer)
		{
			return new OptionCategory(OptionsProvider.GetPerformanceGeneralOptions(isMultiplayer), OptionsProvider.GetPerformanceOptionGroups(isMultiplayer));
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x000D8FD8 File Offset: 0x000D71D8
		private static IEnumerable<OptionGroup> GetPerformanceOptionGroups(bool isMultiplayer)
		{
			yield return new OptionGroup(new TextObject("{=sRTd3RI5}Graphics", null), OptionsProvider.GetPerformanceGraphicsOptions(isMultiplayer));
			yield return new OptionGroup(new TextObject("{=vDMe8SCV}Resolution Scaling", null), OptionsProvider.GetPerformanceResolutionScalingOptions(isMultiplayer));
			yield return new OptionGroup(new TextObject("{=2zcrC0h1}Gameplay", null), OptionsProvider.GetPerformanceGameplayOptions(isMultiplayer));
			yield return new OptionGroup(new TextObject("{=xebFLnH2}Audio", null), OptionsProvider.GetPerformanceAudioOptions());
			yield break;
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x000D8FE8 File Offset: 0x000D71E8
		public static IEnumerable<IOptionData> GetPerformanceGraphicsOptions(bool isMultiplayer)
		{
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.Antialiasing);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.ShaderQuality);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.TextureBudget);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.TextureQuality);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.TextureFiltering);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.CharacterDetail);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.ShadowmapResolution);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.ShadowmapType);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.ShadowmapFiltering);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.ParticleDetail);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.ParticleQuality);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.FoliageQuality);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.TerrainQuality);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.EnvironmentDetail);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.Occlusion);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.DecalQuality);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.WaterQuality);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.SSRQuality);
			if (!isMultiplayer)
			{
				yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.NumberOfCorpses);
			}
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.NumberOfRagDolls);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.LightingQuality);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.ClothSimulation);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.SunShafts);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.Tesselation);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.InteractiveGrass);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.SSR);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.SSSSS);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.MotionBlur);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.DepthOfField);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.Bloom);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.FilmGrain);
			if (NativeOptions.CheckGFXSupportStatus(65))
			{
				yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.PostFXVignette);
			}
			if (NativeOptions.CheckGFXSupportStatus(64))
			{
				yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.PostFXChromaticAberration);
			}
			if (NativeOptions.CheckGFXSupportStatus(62))
			{
				yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.PostFXLensFlare);
			}
			if (NativeOptions.CheckGFXSupportStatus(66))
			{
				yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.PostFXHexagonVignette);
			}
			if (NativeOptions.CheckGFXSupportStatus(63))
			{
				yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.PostFXStreaks);
			}
			yield break;
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x000D8FF8 File Offset: 0x000D71F8
		public static IEnumerable<IOptionData> GetPerformanceResolutionScalingOptions(bool isMultiplayer)
		{
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.DLSS);
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.ResolutionScale);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.DynamicResolution);
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.DynamicResolutionTarget);
			yield break;
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x000D9001 File Offset: 0x000D7201
		public static IEnumerable<IOptionData> GetPerformanceGameplayOptions(bool isMultiplayer)
		{
			if (!isMultiplayer)
			{
				yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.BattleSize);
				yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.PhysicsTickRate);
			}
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.AnimationSamplingQuality);
			yield break;
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x000D9011 File Offset: 0x000D7211
		public static IEnumerable<IOptionData> GetPerformanceAudioOptions()
		{
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.MaxSimultaneousSoundEventCount);
			yield break;
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x000D901A File Offset: 0x000D721A
		private static IEnumerable<IOptionData> GetAudioGeneralOptions(bool isMultiplayer)
		{
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.MasterVolume);
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.SoundVolume);
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.MusicVolume);
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.VoiceOverVolume);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.SoundPreset);
			yield return new NativeSelectionOptionData(NativeOptions.NativeOptionsType.SoundDevice);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.KeepSoundInBackground);
			if (isMultiplayer)
			{
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableVoiceChat);
			}
			else
			{
				yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.SoundOcclusion);
			}
			yield break;
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x000D902A File Offset: 0x000D722A
		public static OptionCategory GetAudioOptionCategory(bool isMultiplayer)
		{
			return new OptionCategory(OptionsProvider.GetAudioGeneralOptions(isMultiplayer), OptionsProvider.GetAudioOptionGroups(isMultiplayer));
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x000D903D File Offset: 0x000D723D
		private static IEnumerable<OptionGroup> GetAudioOptionGroups(bool isMultiplayer)
		{
			return null;
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x000D9040 File Offset: 0x000D7240
		public static OptionCategory GetGameplayOptionCategory(bool isMainMenu, bool isMultiplayer)
		{
			return new OptionCategory(OptionsProvider.GetGameplayGeneralOptions(isMultiplayer), OptionsProvider.GetGameplayOptionGroups(isMainMenu, isMultiplayer));
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x000D9054 File Offset: 0x000D7254
		private static IEnumerable<IOptionData> GetGameplayGeneralOptions(bool isMultiplayer)
		{
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.Language);
			if (!isMultiplayer)
			{
				yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.VoiceLanguage);
				yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.PlayerReceivedDamageDifficulty);
			}
			yield break;
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x000D9064 File Offset: 0x000D7264
		private static IEnumerable<OptionGroup> GetGameplayOptionGroups(bool isMainMenu, bool isMultiplayer)
		{
			yield return new OptionGroup(new TextObject("{=m9KoYCv5}Controls", null), OptionsProvider.GetGameplayControlsOptions(isMainMenu, isMultiplayer));
			yield return new OptionGroup(new TextObject("{=uZ6q4Qs2}Visuals", null), OptionsProvider.GetGameplayVisualOptions(isMultiplayer));
			yield return new OptionGroup(new TextObject("{=gAfbULHM}Camera", null), OptionsProvider.GetGameplayCameraOptions(isMultiplayer));
			yield return new OptionGroup(new TextObject("{=WRMyiiYJ}User Interface", null), OptionsProvider.GetGameplayUIOptions(isMultiplayer));
			if (!isMultiplayer)
			{
				yield return new OptionGroup(new TextObject("{=ys9baYiQ}Campaign", null), OptionsProvider.GetGameplayCampaignOptions());
			}
			yield break;
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x000D907B File Offset: 0x000D727B
		private static IEnumerable<IOptionData> GetGameplayControlsOptions(bool isMainMenu, bool isMultiplayer)
		{
			bool isDualSense = Input.ControllerType.IsPlaystation();
			if (isDualSense)
			{
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.GyroOverrideForAttackDefend);
			}
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.ControlBlockDirection);
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.ControlAttackDirection);
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.MouseYMovementScale);
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.MouseSensitivity);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.InvertMouseYAxis);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.EnableVibration);
			yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.EnableAlternateAiming);
			if (isDualSense)
			{
				yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.EnableTouchpadMouse);
				yield return new NativeBooleanOptionData(NativeOptions.NativeOptionsType.EnableGyroAssistedAim);
				yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.GyroAimSensitivity);
			}
			if (!isMultiplayer)
			{
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.LockTarget);
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.SlowDownOnOrder);
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.StopGameOnFocusLost);
			}
			yield break;
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x000D908B File Offset: 0x000D728B
		private static IEnumerable<IOptionData> GetGameplayVisualOptions(bool isMultiplayer)
		{
			yield return new NativeNumericOptionData(NativeOptions.NativeOptionsType.TrailAmount);
			yield return new ManagedNumericOptionData(ManagedOptions.ManagedOptionsType.FriendlyTroopsBannerOpacity);
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.AlwaysShowFriendlyTroopBannersType);
			if (!isMultiplayer)
			{
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.ShowFormationDistances);
			}
			yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.ShowBlood);
			yield break;
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x000D909B File Offset: 0x000D729B
		private static IEnumerable<IOptionData> GetGameplayCameraOptions(bool isMultiplayer)
		{
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.TurnCameraWithHorseInFirstPerson);
			yield return new ManagedNumericOptionData(ManagedOptions.ManagedOptionsType.FirstPersonFov);
			yield return new ManagedNumericOptionData(ManagedOptions.ManagedOptionsType.CombatCameraDistance);
			yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableVerticalAimCorrection);
			yield return new ManagedNumericOptionData(ManagedOptions.ManagedOptionsType.ZoomSensitivityModifier);
			yield break;
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x000D90A4 File Offset: 0x000D72A4
		private static IEnumerable<IOptionData> GetGameplayUIOptions(bool isMultiplayer)
		{
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.CrosshairType);
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.OrderType);
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.OrderLayoutType);
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.ReportCasualtiesType);
			yield return new ManagedNumericOptionData(ManagedOptions.ManagedOptionsType.UIScale);
			yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.ShowAttackDirection);
			yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.ShowTargetingReticle);
			yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.ReportDamage);
			yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.ReportBark);
			if (!isMultiplayer)
			{
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.ReportExperience);
			}
			yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.ReportPersonalDamage);
			yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableDamageTakenVisuals);
			if (isMultiplayer)
			{
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableMultiplayerChatBox);
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableDeathIcon);
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableNetworkAlertIcons);
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableGenericAvatars);
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableGenericNames);
			}
			else
			{
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableSingleplayerChatBox);
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.HideBattleUI);
				yield return new ManagedBooleanOptionData(ManagedOptions.ManagedOptionsType.EnableTutorialHints);
			}
			yield break;
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x000D90B4 File Offset: 0x000D72B4
		private static IEnumerable<IOptionData> GetGameplayCampaignOptions()
		{
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.AutoTrackAttackedSettlements);
			yield return new ManagedNumericOptionData(ManagedOptions.ManagedOptionsType.AutoSaveInterval);
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.UnitSpawnPrioritization);
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.ReinforcementWaveCount);
			yield return new ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType.MapDoubleClickBehavior);
			yield break;
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x000D90BD File Offset: 0x000D72BD
		public static IEnumerable<string> GetGameKeyCategoriesList(bool isMultiplayer)
		{
			yield return GameKeyMainCategories.ActionCategory;
			yield return GameKeyMainCategories.OrderMenuCategory;
			if (!isMultiplayer)
			{
				yield return GameKeyMainCategories.ShipControlsCategory;
				yield return GameKeyMainCategories.CampaignMapCategory;
				yield return GameKeyMainCategories.MenuShortcutCategory;
				yield return GameKeyMainCategories.PhotoModeCategory;
			}
			else
			{
				yield return GameKeyMainCategories.PollCategory;
			}
			yield return GameKeyMainCategories.ChatCategory;
			yield break;
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x000D90CD File Offset: 0x000D72CD
		public static IEnumerable<int> GetHiddenGameKeys(bool isNavalModuleActive)
		{
			if (!isNavalModuleActive)
			{
				yield return 45;
			}
			yield break;
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x000D90DD File Offset: 0x000D72DD
		public static OptionCategory GetControllerOptionCategory()
		{
			return new OptionCategory(OptionsProvider.GetControllerBaseOptions(), OptionsProvider.GetControllerOptionGroups());
		}

		// Token: 0x060034AD RID: 13485 RVA: 0x000D90EE File Offset: 0x000D72EE
		private static IEnumerable<IOptionData> GetControllerBaseOptions()
		{
			return null;
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x000D90F1 File Offset: 0x000D72F1
		private static IEnumerable<OptionGroup> GetControllerOptionGroups()
		{
			return null;
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x000D90F4 File Offset: 0x000D72F4
		public static Dictionary<NativeOptions.NativeOptionsType, float[]> GetDefaultNativeOptions()
		{
			if (OptionsProvider._defaultNativeOptions == null)
			{
				OptionsProvider._defaultNativeOptions = new Dictionary<NativeOptions.NativeOptionsType, float[]>();
				foreach (NativeOptionData nativeOptionData in NativeOptions.VideoOptions.Union<NativeOptionData>(NativeOptions.GraphicsOptions))
				{
					float[] array = new float[OptionsProvider._overallConfigCount];
					bool flag = false;
					for (int i = 0; i < OptionsProvider._overallConfigCount; i++)
					{
						array[i] = NativeOptions.GetDefaultConfigForOverallSettings(nativeOptionData.Type, i);
						if (array[i] < 0f)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						OptionsProvider._defaultNativeOptions[nativeOptionData.Type] = array;
					}
				}
			}
			return OptionsProvider._defaultNativeOptions;
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x000D91B4 File Offset: 0x000D73B4
		public static Dictionary<ManagedOptions.ManagedOptionsType, float[]> GetDefaultManagedOptions()
		{
			if (OptionsProvider._defaultManagedOptions == null)
			{
				OptionsProvider._defaultManagedOptions = new Dictionary<ManagedOptions.ManagedOptionsType, float[]>();
				float[] array = new float[OptionsProvider._overallConfigCount];
				for (int i = 0; i < OptionsProvider._overallConfigCount; i++)
				{
					array[i] = (float)i;
				}
				OptionsProvider._defaultManagedOptions.Add(ManagedOptions.ManagedOptionsType.BattleSize, array);
				array = new float[OptionsProvider._overallConfigCount];
				for (int j = 0; j < OptionsProvider._overallConfigCount; j++)
				{
					array[j] = (float)j;
				}
				OptionsProvider._defaultManagedOptions.Add(ManagedOptions.ManagedOptionsType.NumberOfCorpses, array);
			}
			return OptionsProvider._defaultManagedOptions;
		}

		// Token: 0x0400165E RID: 5726
		private static readonly int _overallConfigCount = NativeSelectionOptionData.GetOptionsLimit(NativeOptions.NativeOptionsType.OverAll) - 1;

		// Token: 0x0400165F RID: 5727
		private static Dictionary<NativeOptions.NativeOptionsType, float[]> _defaultNativeOptions;

		// Token: 0x04001660 RID: 5728
		private static Dictionary<ManagedOptions.ManagedOptionsType, float[]> _defaultManagedOptions;
	}
}
