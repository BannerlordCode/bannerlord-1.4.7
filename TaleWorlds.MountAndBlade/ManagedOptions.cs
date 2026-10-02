using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000383 RID: 899
	public static class ManagedOptions
	{
		// Token: 0x060033C2 RID: 13250 RVA: 0x000D50C4 File Offset: 0x000D32C4
		public static float GetConfig(ManagedOptions.ManagedOptionsType type)
		{
			switch (type)
			{
			case ManagedOptions.ManagedOptionsType.Language:
				return (float)LocalizedTextManager.GetLanguageIds(NativeConfig.IsDevelopmentMode).IndexOf(BannerlordConfig.Language);
			case ManagedOptions.ManagedOptionsType.GyroOverrideForAttackDefend:
				return (float)(BannerlordConfig.GyroOverrideForAttackDefend ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.ControlBlockDirection:
				return (float)BannerlordConfig.DefendDirectionControl;
			case ManagedOptions.ManagedOptionsType.ControlAttackDirection:
				return (float)BannerlordConfig.AttackDirectionControl;
			case ManagedOptions.ManagedOptionsType.NumberOfCorpses:
				return (float)BannerlordConfig.NumberOfCorpses;
			case ManagedOptions.ManagedOptionsType.BattleSize:
				return (float)BannerlordConfig.BattleSize;
			case ManagedOptions.ManagedOptionsType.ReinforcementWaveCount:
				return (float)BannerlordConfig.ReinforcementWaveCount;
			case ManagedOptions.ManagedOptionsType.TurnCameraWithHorseInFirstPerson:
				return (float)BannerlordConfig.TurnCameraWithHorseInFirstPerson;
			case ManagedOptions.ManagedOptionsType.ShowBlood:
				return (float)(BannerlordConfig.ShowBlood ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.ShowAttackDirection:
				return (float)(BannerlordConfig.DisplayAttackDirection ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.ShowTargetingReticle:
				return (float)(BannerlordConfig.DisplayTargetingReticule ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.AutoSaveInterval:
				return (float)BannerlordConfig.AutoSaveInterval;
			case ManagedOptions.ManagedOptionsType.FriendlyTroopsBannerOpacity:
				return BannerlordConfig.FriendlyTroopsBannerOpacity;
			case ManagedOptions.ManagedOptionsType.AlwaysShowFriendlyTroopBannersType:
				return (float)BannerlordConfig.AlwaysShowFriendlyTroopBannersType;
			case ManagedOptions.ManagedOptionsType.ShowFormationDistances:
				return (float)(BannerlordConfig.ShowFormationDistances ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.ReportDamage:
				return (float)(BannerlordConfig.ReportDamage ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.ReportBark:
				return (float)(BannerlordConfig.ReportBark ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.LockTarget:
				return (float)(BannerlordConfig.LockTarget ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.EnableTutorialHints:
				return (float)(BannerlordConfig.EnableTutorialHints ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.ReportCasualtiesType:
				return (float)BannerlordConfig.KillFeedVisualType;
			case ManagedOptions.ManagedOptionsType.ReportExperience:
				return (float)(BannerlordConfig.ReportExperience ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.ReportPersonalDamage:
				return (float)(BannerlordConfig.ReportPersonalDamage ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.FirstPersonFov:
				return BannerlordConfig.FirstPersonFov;
			case ManagedOptions.ManagedOptionsType.CombatCameraDistance:
				return BannerlordConfig.CombatCameraDistance;
			case ManagedOptions.ManagedOptionsType.EnableDamageTakenVisuals:
				return (float)(BannerlordConfig.EnableDamageTakenVisuals ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.EnableVoiceChat:
				return (float)(BannerlordConfig.EnableVoiceChat ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.EnableDeathIcon:
				return (float)(BannerlordConfig.EnableDeathIcon ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.EnableNetworkAlertIcons:
				return (float)(BannerlordConfig.EnableNetworkAlertIcons ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.ForceVSyncInMenus:
				return (float)(BannerlordConfig.ForceVSyncInMenus ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.EnableVerticalAimCorrection:
				return (float)(BannerlordConfig.EnableVerticalAimCorrection ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.ZoomSensitivityModifier:
				return BannerlordConfig.ZoomSensitivityModifier;
			case ManagedOptions.ManagedOptionsType.UIScale:
				return BannerlordConfig.UIScale;
			case ManagedOptions.ManagedOptionsType.CrosshairType:
				return (float)BannerlordConfig.CrosshairType;
			case ManagedOptions.ManagedOptionsType.EnableGenericAvatars:
				return (float)(BannerlordConfig.EnableGenericAvatars ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.EnableGenericNames:
				return (float)(BannerlordConfig.EnableGenericNames ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.OrderType:
				return (float)BannerlordConfig.OrderType;
			case ManagedOptions.ManagedOptionsType.OrderLayoutType:
				return (float)BannerlordConfig.OrderLayoutType;
			case ManagedOptions.ManagedOptionsType.AutoTrackAttackedSettlements:
				return (float)BannerlordConfig.AutoTrackAttackedSettlements;
			case ManagedOptions.ManagedOptionsType.StopGameOnFocusLost:
				return (float)(BannerlordConfig.StopGameOnFocusLost ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.SlowDownOnOrder:
				return (float)(BannerlordConfig.SlowDownOnOrder ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.HideFullServers:
				return (float)(BannerlordConfig.HideFullServers ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.HideEmptyServers:
				return (float)(BannerlordConfig.HideEmptyServers ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.HidePasswordProtectedServers:
				return (float)(BannerlordConfig.HidePasswordProtectedServers ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.HideUnofficialServers:
				return (float)(BannerlordConfig.HideUnofficialServers ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.HideModuleIncompatibleServers:
				return (float)(BannerlordConfig.HideModuleIncompatibleServers ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.HideBattleUI:
				return (float)(BannerlordConfig.HideBattleUI ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.UnitSpawnPrioritization:
				return (float)BannerlordConfig.UnitSpawnPrioritization;
			case ManagedOptions.ManagedOptionsType.EnableSingleplayerChatBox:
				return (float)(BannerlordConfig.EnableSingleplayerChatBox ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.EnableMultiplayerChatBox:
				return (float)(BannerlordConfig.EnableMultiplayerChatBox ? 1 : 0);
			case ManagedOptions.ManagedOptionsType.VoiceLanguage:
				return (float)LocalizedVoiceManager.GetVoiceLanguageIds().IndexOf(BannerlordConfig.VoiceLanguage);
			case ManagedOptions.ManagedOptionsType.PlayerReceivedDamageDifficulty:
				return (float)BannerlordConfig.PlayerReceivedDamageDifficulty;
			case ManagedOptions.ManagedOptionsType.MapDoubleClickBehavior:
				return (float)BannerlordConfig.MapDoubleClickBehavior;
			default:
				Debug.FailedAssert("ManagedOptionsType not found", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Options\\ManagedOptions\\ManagedOptions.cs", "GetConfig", 180);
				return 0f;
			}
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x000D53F8 File Offset: 0x000D35F8
		public static float GetDefaultConfig(ManagedOptions.ManagedOptionsType type)
		{
			switch (type)
			{
			case ManagedOptions.ManagedOptionsType.Language:
				return (float)LocalizedTextManager.GetLanguageIds(NativeConfig.IsDevelopmentMode).IndexOf(BannerlordConfig.DefaultLanguage);
			case ManagedOptions.ManagedOptionsType.GyroOverrideForAttackDefend:
				return 0f;
			case ManagedOptions.ManagedOptionsType.ControlBlockDirection:
				return 0f;
			case ManagedOptions.ManagedOptionsType.ControlAttackDirection:
				return 1f;
			case ManagedOptions.ManagedOptionsType.NumberOfCorpses:
				return 3f;
			case ManagedOptions.ManagedOptionsType.BattleSize:
				return 2f;
			case ManagedOptions.ManagedOptionsType.ReinforcementWaveCount:
				return 3f;
			case ManagedOptions.ManagedOptionsType.TurnCameraWithHorseInFirstPerson:
				return 2f;
			case ManagedOptions.ManagedOptionsType.ShowBlood:
				return 1f;
			case ManagedOptions.ManagedOptionsType.ShowAttackDirection:
				return 1f;
			case ManagedOptions.ManagedOptionsType.ShowTargetingReticle:
				return 1f;
			case ManagedOptions.ManagedOptionsType.AutoSaveInterval:
				return 30f;
			case ManagedOptions.ManagedOptionsType.FriendlyTroopsBannerOpacity:
				return 1f;
			case ManagedOptions.ManagedOptionsType.AlwaysShowFriendlyTroopBannersType:
				return 1f;
			case ManagedOptions.ManagedOptionsType.ShowFormationDistances:
				return 0f;
			case ManagedOptions.ManagedOptionsType.ReportDamage:
				return 1f;
			case ManagedOptions.ManagedOptionsType.ReportBark:
				return 1f;
			case ManagedOptions.ManagedOptionsType.LockTarget:
				return 0f;
			case ManagedOptions.ManagedOptionsType.EnableTutorialHints:
				return 1f;
			case ManagedOptions.ManagedOptionsType.ReportCasualtiesType:
				return 1f;
			case ManagedOptions.ManagedOptionsType.ReportExperience:
				return 1f;
			case ManagedOptions.ManagedOptionsType.ReportPersonalDamage:
				return 1f;
			case ManagedOptions.ManagedOptionsType.FirstPersonFov:
				return 65f;
			case ManagedOptions.ManagedOptionsType.CombatCameraDistance:
				return 1f;
			case ManagedOptions.ManagedOptionsType.EnableDamageTakenVisuals:
				return 1f;
			case ManagedOptions.ManagedOptionsType.EnableVoiceChat:
				return 1f;
			case ManagedOptions.ManagedOptionsType.EnableDeathIcon:
				return 1f;
			case ManagedOptions.ManagedOptionsType.EnableNetworkAlertIcons:
				return 1f;
			case ManagedOptions.ManagedOptionsType.ForceVSyncInMenus:
				return 1f;
			case ManagedOptions.ManagedOptionsType.EnableVerticalAimCorrection:
				return 1f;
			case ManagedOptions.ManagedOptionsType.ZoomSensitivityModifier:
				return 0.66666f;
			case ManagedOptions.ManagedOptionsType.UIScale:
				return 1f;
			case ManagedOptions.ManagedOptionsType.CrosshairType:
				return 0f;
			case ManagedOptions.ManagedOptionsType.EnableGenericAvatars:
				return 0f;
			case ManagedOptions.ManagedOptionsType.EnableGenericNames:
				return 0f;
			case ManagedOptions.ManagedOptionsType.OrderType:
				return 0f;
			case ManagedOptions.ManagedOptionsType.OrderLayoutType:
				return 0f;
			case ManagedOptions.ManagedOptionsType.AutoTrackAttackedSettlements:
				return 0f;
			case ManagedOptions.ManagedOptionsType.StopGameOnFocusLost:
				return 1f;
			case ManagedOptions.ManagedOptionsType.SlowDownOnOrder:
				return 1f;
			case ManagedOptions.ManagedOptionsType.HideFullServers:
				return 0f;
			case ManagedOptions.ManagedOptionsType.HideEmptyServers:
				return 0f;
			case ManagedOptions.ManagedOptionsType.HidePasswordProtectedServers:
				return 0f;
			case ManagedOptions.ManagedOptionsType.HideUnofficialServers:
				return 0f;
			case ManagedOptions.ManagedOptionsType.HideModuleIncompatibleServers:
				return 0f;
			case ManagedOptions.ManagedOptionsType.HideBattleUI:
				return 0f;
			case ManagedOptions.ManagedOptionsType.UnitSpawnPrioritization:
				return 0f;
			case ManagedOptions.ManagedOptionsType.EnableSingleplayerChatBox:
				return 1f;
			case ManagedOptions.ManagedOptionsType.EnableMultiplayerChatBox:
				return 1f;
			case ManagedOptions.ManagedOptionsType.VoiceLanguage:
				return (float)LocalizedVoiceManager.GetVoiceLanguageIds().IndexOf(BannerlordConfig.VoiceLanguage);
			case ManagedOptions.ManagedOptionsType.PlayerReceivedDamageDifficulty:
				return 0f;
			case ManagedOptions.ManagedOptionsType.MapDoubleClickBehavior:
				return 0f;
			default:
				Debug.FailedAssert("ManagedOptionsType not found", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Options\\ManagedOptions\\ManagedOptions.cs", "GetDefaultConfig", 293);
				return 0f;
			}
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x000D5651 File Offset: 0x000D3851
		[MBCallback(null, true)]
		internal static int GetConfigCount()
		{
			return 52;
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x000D5655 File Offset: 0x000D3855
		[MBCallback(null, true)]
		internal static float GetConfigValue(int type)
		{
			return ManagedOptions.GetConfig((ManagedOptions.ManagedOptionsType)type);
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x000D5660 File Offset: 0x000D3860
		public static void SetConfig(ManagedOptions.ManagedOptionsType type, float value)
		{
			switch (type)
			{
			case ManagedOptions.ManagedOptionsType.Language:
			{
				List<string> list = LocalizedTextManager.GetLanguageIds(NativeConfig.IsDevelopmentMode);
				if (value >= 0f && value < (float)list.Count)
				{
					BannerlordConfig.Language = list[(int)value];
				}
				else
				{
					Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Options\\ManagedOptions\\ManagedOptions.cs", "SetConfig", 459);
					BannerlordConfig.Language = list[0];
				}
				break;
			}
			case ManagedOptions.ManagedOptionsType.GyroOverrideForAttackDefend:
				BannerlordConfig.GyroOverrideForAttackDefend = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.ControlBlockDirection:
				BannerlordConfig.DefendDirectionControl = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.ControlAttackDirection:
				BannerlordConfig.AttackDirectionControl = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.NumberOfCorpses:
				BannerlordConfig.NumberOfCorpses = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.BattleSize:
				BannerlordConfig.BattleSize = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.ReinforcementWaveCount:
				BannerlordConfig.ReinforcementWaveCount = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.TurnCameraWithHorseInFirstPerson:
				BannerlordConfig.TurnCameraWithHorseInFirstPerson = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.ShowBlood:
				BannerlordConfig.ShowBlood = (double)value != 0.0;
				break;
			case ManagedOptions.ManagedOptionsType.ShowAttackDirection:
				BannerlordConfig.DisplayAttackDirection = (double)value != 0.0;
				break;
			case ManagedOptions.ManagedOptionsType.ShowTargetingReticle:
				BannerlordConfig.DisplayTargetingReticule = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.AutoSaveInterval:
				BannerlordConfig.AutoSaveInterval = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.FriendlyTroopsBannerOpacity:
				BannerlordConfig.FriendlyTroopsBannerOpacity = value;
				break;
			case ManagedOptions.ManagedOptionsType.AlwaysShowFriendlyTroopBannersType:
				BannerlordConfig.AlwaysShowFriendlyTroopBannersType = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.ShowFormationDistances:
				BannerlordConfig.ShowFormationDistances = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.ReportDamage:
				BannerlordConfig.ReportDamage = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.ReportBark:
				BannerlordConfig.ReportBark = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.LockTarget:
				BannerlordConfig.LockTarget = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.EnableTutorialHints:
				BannerlordConfig.EnableTutorialHints = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.ReportCasualtiesType:
				BannerlordConfig.KillFeedVisualType = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.ReportExperience:
				BannerlordConfig.ReportExperience = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.ReportPersonalDamage:
				BannerlordConfig.ReportPersonalDamage = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.FirstPersonFov:
				BannerlordConfig.FirstPersonFov = value;
				break;
			case ManagedOptions.ManagedOptionsType.CombatCameraDistance:
				BannerlordConfig.CombatCameraDistance = value;
				break;
			case ManagedOptions.ManagedOptionsType.EnableDamageTakenVisuals:
				BannerlordConfig.EnableDamageTakenVisuals = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.EnableVoiceChat:
				BannerlordConfig.EnableVoiceChat = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.EnableDeathIcon:
				BannerlordConfig.EnableDeathIcon = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.EnableNetworkAlertIcons:
				BannerlordConfig.EnableNetworkAlertIcons = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.ForceVSyncInMenus:
				BannerlordConfig.ForceVSyncInMenus = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.EnableVerticalAimCorrection:
				BannerlordConfig.EnableVerticalAimCorrection = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.ZoomSensitivityModifier:
				BannerlordConfig.ZoomSensitivityModifier = value;
				break;
			case ManagedOptions.ManagedOptionsType.UIScale:
				BannerlordConfig.UIScale = value;
				break;
			case ManagedOptions.ManagedOptionsType.CrosshairType:
				BannerlordConfig.CrosshairType = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.EnableGenericAvatars:
				BannerlordConfig.EnableGenericAvatars = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.EnableGenericNames:
				BannerlordConfig.EnableGenericNames = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.OrderType:
				BannerlordConfig.OrderType = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.OrderLayoutType:
				BannerlordConfig.OrderLayoutType = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.AutoTrackAttackedSettlements:
				BannerlordConfig.AutoTrackAttackedSettlements = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.StopGameOnFocusLost:
				BannerlordConfig.StopGameOnFocusLost = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.SlowDownOnOrder:
				BannerlordConfig.SlowDownOnOrder = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.HideFullServers:
				BannerlordConfig.HideFullServers = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.HideEmptyServers:
				BannerlordConfig.HideEmptyServers = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.HidePasswordProtectedServers:
				BannerlordConfig.HidePasswordProtectedServers = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.HideUnofficialServers:
				BannerlordConfig.HideUnofficialServers = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.HideModuleIncompatibleServers:
				BannerlordConfig.HideModuleIncompatibleServers = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.HideBattleUI:
				BannerlordConfig.HideBattleUI = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.UnitSpawnPrioritization:
				BannerlordConfig.UnitSpawnPrioritization = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.EnableSingleplayerChatBox:
				BannerlordConfig.EnableSingleplayerChatBox = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.EnableMultiplayerChatBox:
				BannerlordConfig.EnableMultiplayerChatBox = value != 0f;
				break;
			case ManagedOptions.ManagedOptionsType.VoiceLanguage:
			{
				List<string> list = LocalizedVoiceManager.GetVoiceLanguageIds();
				if (value >= 0f && value < (float)list.Count)
				{
					BannerlordConfig.VoiceLanguage = list[(int)value];
				}
				else
				{
					Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Options\\ManagedOptions\\ManagedOptions.cs", "SetConfig", 477);
					BannerlordConfig.VoiceLanguage = list[0];
				}
				break;
			}
			case ManagedOptions.ManagedOptionsType.PlayerReceivedDamageDifficulty:
				BannerlordConfig.PlayerReceivedDamageDifficulty = (int)value;
				break;
			case ManagedOptions.ManagedOptionsType.MapDoubleClickBehavior:
				BannerlordConfig.MapDoubleClickBehavior = (int)value;
				break;
			}
			ManagedOptions.OnManagedOptionChangedDelegate onManagedOptionChanged = ManagedOptions.OnManagedOptionChanged;
			if (onManagedOptionChanged == null)
			{
				return;
			}
			onManagedOptionChanged(type);
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x000D5B50 File Offset: 0x000D3D50
		public static SaveResult SaveConfig()
		{
			return BannerlordConfig.Save();
		}

		// Token: 0x040015E7 RID: 5607
		public static ManagedOptions.OnManagedOptionChangedDelegate OnManagedOptionChanged;

		// Token: 0x02000659 RID: 1625
		public enum ManagedOptionsType
		{
			// Token: 0x04002168 RID: 8552
			Language,
			// Token: 0x04002169 RID: 8553
			GyroOverrideForAttackDefend,
			// Token: 0x0400216A RID: 8554
			ControlBlockDirection,
			// Token: 0x0400216B RID: 8555
			ControlAttackDirection,
			// Token: 0x0400216C RID: 8556
			NumberOfCorpses,
			// Token: 0x0400216D RID: 8557
			BattleSize,
			// Token: 0x0400216E RID: 8558
			ReinforcementWaveCount,
			// Token: 0x0400216F RID: 8559
			TurnCameraWithHorseInFirstPerson,
			// Token: 0x04002170 RID: 8560
			ShowBlood,
			// Token: 0x04002171 RID: 8561
			ShowAttackDirection,
			// Token: 0x04002172 RID: 8562
			ShowTargetingReticle,
			// Token: 0x04002173 RID: 8563
			AutoSaveInterval,
			// Token: 0x04002174 RID: 8564
			FriendlyTroopsBannerOpacity,
			// Token: 0x04002175 RID: 8565
			AlwaysShowFriendlyTroopBannersType,
			// Token: 0x04002176 RID: 8566
			ShowFormationDistances,
			// Token: 0x04002177 RID: 8567
			ReportDamage,
			// Token: 0x04002178 RID: 8568
			ReportBark,
			// Token: 0x04002179 RID: 8569
			LockTarget,
			// Token: 0x0400217A RID: 8570
			EnableTutorialHints,
			// Token: 0x0400217B RID: 8571
			ReportCasualtiesType,
			// Token: 0x0400217C RID: 8572
			ReportExperience,
			// Token: 0x0400217D RID: 8573
			ReportPersonalDamage,
			// Token: 0x0400217E RID: 8574
			FirstPersonFov,
			// Token: 0x0400217F RID: 8575
			CombatCameraDistance,
			// Token: 0x04002180 RID: 8576
			EnableDamageTakenVisuals,
			// Token: 0x04002181 RID: 8577
			EnableVoiceChat,
			// Token: 0x04002182 RID: 8578
			EnableDeathIcon,
			// Token: 0x04002183 RID: 8579
			EnableNetworkAlertIcons,
			// Token: 0x04002184 RID: 8580
			ForceVSyncInMenus,
			// Token: 0x04002185 RID: 8581
			EnableVerticalAimCorrection,
			// Token: 0x04002186 RID: 8582
			ZoomSensitivityModifier,
			// Token: 0x04002187 RID: 8583
			UIScale,
			// Token: 0x04002188 RID: 8584
			CrosshairType,
			// Token: 0x04002189 RID: 8585
			EnableGenericAvatars,
			// Token: 0x0400218A RID: 8586
			EnableGenericNames,
			// Token: 0x0400218B RID: 8587
			OrderType,
			// Token: 0x0400218C RID: 8588
			OrderLayoutType,
			// Token: 0x0400218D RID: 8589
			AutoTrackAttackedSettlements,
			// Token: 0x0400218E RID: 8590
			StopGameOnFocusLost,
			// Token: 0x0400218F RID: 8591
			SlowDownOnOrder,
			// Token: 0x04002190 RID: 8592
			HideFullServers,
			// Token: 0x04002191 RID: 8593
			HideEmptyServers,
			// Token: 0x04002192 RID: 8594
			HidePasswordProtectedServers,
			// Token: 0x04002193 RID: 8595
			HideUnofficialServers,
			// Token: 0x04002194 RID: 8596
			HideModuleIncompatibleServers,
			// Token: 0x04002195 RID: 8597
			HideBattleUI,
			// Token: 0x04002196 RID: 8598
			UnitSpawnPrioritization,
			// Token: 0x04002197 RID: 8599
			EnableSingleplayerChatBox,
			// Token: 0x04002198 RID: 8600
			EnableMultiplayerChatBox,
			// Token: 0x04002199 RID: 8601
			VoiceLanguage,
			// Token: 0x0400219A RID: 8602
			PlayerReceivedDamageDifficulty,
			// Token: 0x0400219B RID: 8603
			MapDoubleClickBehavior,
			// Token: 0x0400219C RID: 8604
			ManagedOptionTypeCount
		}

		// Token: 0x0200065A RID: 1626
		// (Invoke) Token: 0x06004062 RID: 16482
		public delegate void OnManagedOptionChangedDelegate(ManagedOptions.ManagedOptionsType changedManagedOptionsType);
	}
}
