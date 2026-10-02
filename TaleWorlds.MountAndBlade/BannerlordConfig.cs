using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000197 RID: 407
	public static class BannerlordConfig
	{
		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x0600154B RID: 5451 RVA: 0x00050938 File Offset: 0x0004EB38
		public static int MinBattleSize
		{
			get
			{
				return BannerlordConfig._battleSizes[0];
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x0600154C RID: 5452 RVA: 0x00050941 File Offset: 0x0004EB41
		public static int MaxBattleSize
		{
			get
			{
				return BannerlordConfig._battleSizes[BannerlordConfig._battleSizes.Length - 1];
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x0600154D RID: 5453 RVA: 0x00050952 File Offset: 0x0004EB52
		public static int MinReinforcementWaveCount
		{
			get
			{
				return BannerlordConfig._reinforcementWaveCounts[0];
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x0005095B File Offset: 0x0004EB5B
		public static int MaxReinforcementWaveCount
		{
			get
			{
				return BannerlordConfig._reinforcementWaveCounts[BannerlordConfig._reinforcementWaveCounts.Length - 1];
			}
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x0005096C File Offset: 0x0004EB6C
		public static void Initialize()
		{
			string text = Utilities.LoadBannerlordConfigFile();
			if (string.IsNullOrEmpty(text))
			{
				BannerlordConfig.Save();
			}
			else
			{
				bool flag = false;
				string[] array = text.Split(new char[] { '\n' });
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(new char[] { '=' });
					PropertyInfo property = typeof(BannerlordConfig).GetProperty(array2[0]);
					if (property == null)
					{
						flag = true;
					}
					else
					{
						string text2 = array2[1];
						try
						{
							if (property.PropertyType == typeof(string))
							{
								string text3 = Regex.Replace(text2, "\\r", "");
								property.SetValue(null, text3);
							}
							else if (property.PropertyType == typeof(float))
							{
								float num;
								if (float.TryParse(text2, out num))
								{
									property.SetValue(null, num);
								}
								else
								{
									flag = true;
								}
							}
							else if (property.PropertyType == typeof(int))
							{
								int num2;
								if (int.TryParse(text2, out num2))
								{
									BannerlordConfig.ConfigPropertyInt customAttribute = property.GetCustomAttribute<BannerlordConfig.ConfigPropertyInt>();
									if (customAttribute == null || customAttribute.IsValidValue(num2))
									{
										property.SetValue(null, num2);
									}
									else
									{
										flag = true;
									}
								}
								else
								{
									flag = true;
								}
							}
							else if (property.PropertyType == typeof(bool))
							{
								bool flag2;
								if (bool.TryParse(text2, out flag2))
								{
									property.SetValue(null, flag2);
								}
								else
								{
									flag = true;
								}
							}
							else
							{
								flag = true;
								Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\BannerlordConfig.cs", "Initialize", 114);
							}
						}
						catch
						{
							flag = true;
						}
					}
				}
				if (flag)
				{
					BannerlordConfig.Save();
				}
				MBAPI.IMBBannerlordConfig.ValidateOptions();
			}
			MBTextManager.TryChangeVoiceLanguage(BannerlordConfig.VoiceLanguage);
			MBTextManager.ChangeLanguage(BannerlordConfig.Language);
			MBTextManager.LocalizationDebugMode = NativeConfig.LocalizationDebugMode;
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x00050B70 File Offset: 0x0004ED70
		public static SaveResult Save()
		{
			Dictionary<PropertyInfo, object> dictionary = new Dictionary<PropertyInfo, object>();
			foreach (PropertyInfo propertyInfo in typeof(BannerlordConfig).GetProperties())
			{
				if (propertyInfo.GetCustomAttribute<BannerlordConfig.ConfigProperty>() != null)
				{
					dictionary.Add(propertyInfo, propertyInfo.GetValue(null, null));
				}
			}
			string text = "";
			foreach (KeyValuePair<PropertyInfo, object> keyValuePair in dictionary)
			{
				text = string.Concat(new string[]
				{
					text,
					keyValuePair.Key.Name,
					"=",
					keyValuePair.Value.ToString(),
					"\n"
				});
			}
			SaveResult saveResult = Utilities.SaveConfigFile(text);
			MBAPI.IMBBannerlordConfig.ValidateOptions();
			return saveResult;
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00050C50 File Offset: 0x0004EE50
		public static float GetDamageToPlayerMultiplier()
		{
			switch (BannerlordConfig.PlayerReceivedDamageDifficulty)
			{
			case 0:
				return 0.25f;
			case 1:
				return 0.5f;
			case 2:
				return 1f;
			default:
				return 1f;
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001552 RID: 5458 RVA: 0x00050C8E File Offset: 0x0004EE8E
		public static string DefaultLanguage
		{
			get
			{
				return BannerlordConfig.GetDefaultLanguage();
			}
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x00050C95 File Offset: 0x0004EE95
		public static int GetRealBattleSize()
		{
			return BannerlordConfig._battleSizes[BannerlordConfig.BattleSize];
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x00050CA2 File Offset: 0x0004EEA2
		public static int GetRealBattleSizeForSiege()
		{
			return BannerlordConfig._siegeBattleSizes[BannerlordConfig.BattleSize];
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x00050CAF File Offset: 0x0004EEAF
		public static int GetRealBattleSizeForNaval()
		{
			return BannerlordConfig._battleSizes[BannerlordConfig.BattleSize];
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x00050CBC File Offset: 0x0004EEBC
		public static int GetReinforcementWaveCount()
		{
			return BannerlordConfig._reinforcementWaveCounts[BannerlordConfig.ReinforcementWaveCount];
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x00050CC9 File Offset: 0x0004EEC9
		public static int GetRealBattleSizeForSallyOut()
		{
			return BannerlordConfig._sallyOutBattleSizes[BannerlordConfig.BattleSize];
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x00050CD6 File Offset: 0x0004EED6
		private static string GetDefaultLanguage()
		{
			return LocalizedTextManager.GetLocalizationCodeOfISOLanguageCode(Utilities.GetSystemLanguage());
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001559 RID: 5465 RVA: 0x00050CE2 File Offset: 0x0004EEE2
		// (set) Token: 0x0600155A RID: 5466 RVA: 0x00050CEC File Offset: 0x0004EEEC
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static string Language
		{
			get
			{
				return BannerlordConfig._language;
			}
			set
			{
				if (BannerlordConfig._language != value)
				{
					if (MBTextManager.LanguageExistsInCurrentConfiguration(value, NativeConfig.IsDevelopmentMode) && MBTextManager.ChangeLanguage(value))
					{
						BannerlordConfig._language = value;
					}
					else if (MBTextManager.ChangeLanguage("English"))
					{
						BannerlordConfig._language = "English";
					}
					else
					{
						Debug.FailedAssert("Language cannot be set!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\BannerlordConfig.cs", "Language", 391);
					}
					MBTextManager.LocalizationDebugMode = NativeConfig.LocalizationDebugMode;
				}
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x0600155B RID: 5467 RVA: 0x00050D5E File Offset: 0x0004EF5E
		// (set) Token: 0x0600155C RID: 5468 RVA: 0x00050D68 File Offset: 0x0004EF68
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static string VoiceLanguage
		{
			get
			{
				return BannerlordConfig._voiceLanguage;
			}
			set
			{
				if (BannerlordConfig._voiceLanguage != value)
				{
					if (MBTextManager.LanguageExistsInCurrentConfiguration(value, NativeConfig.IsDevelopmentMode) && MBTextManager.TryChangeVoiceLanguage(value))
					{
						BannerlordConfig._voiceLanguage = value;
						return;
					}
					if (MBTextManager.TryChangeVoiceLanguage("English"))
					{
						BannerlordConfig._voiceLanguage = "English";
						return;
					}
					Debug.FailedAssert("Voice Language cannot be set!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\BannerlordConfig.cs", "VoiceLanguage", 418);
				}
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x0600155D RID: 5469 RVA: 0x00050DCE File Offset: 0x0004EFCE
		// (set) Token: 0x0600155E RID: 5470 RVA: 0x00050DD5 File Offset: 0x0004EFD5
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2 }, false)]
		public static int MapDoubleClickBehavior { get; set; } = BannerlordConfig.MapDoubleClickBehavior;

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x0600155F RID: 5471 RVA: 0x00050DDD File Offset: 0x0004EFDD
		// (set) Token: 0x06001560 RID: 5472 RVA: 0x00050DE4 File Offset: 0x0004EFE4
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2 }, false)]
		public static int PlayerReceivedDamageDifficulty { get; set; } = 0;

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001561 RID: 5473 RVA: 0x00050DEC File Offset: 0x0004EFEC
		// (set) Token: 0x06001562 RID: 5474 RVA: 0x00050DF3 File Offset: 0x0004EFF3
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool GyroOverrideForAttackDefend { get; set; } = false;

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001563 RID: 5475 RVA: 0x00050DFB File Offset: 0x0004EFFB
		// (set) Token: 0x06001564 RID: 5476 RVA: 0x00050E02 File Offset: 0x0004F002
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2 }, false)]
		public static int AttackDirectionControl { get; set; } = 1;

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001565 RID: 5477 RVA: 0x00050E0A File Offset: 0x0004F00A
		// (set) Token: 0x06001566 RID: 5478 RVA: 0x00050E11 File Offset: 0x0004F011
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2 }, false)]
		public static int DefendDirectionControl { get; set; } = 0;

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001567 RID: 5479 RVA: 0x00050E19 File Offset: 0x0004F019
		// (set) Token: 0x06001568 RID: 5480 RVA: 0x00050E20 File Offset: 0x0004F020
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2, 3, 4, 5 }, false)]
		public static int NumberOfCorpses
		{
			get
			{
				return BannerlordConfig._numberOfCorpses;
			}
			set
			{
				BannerlordConfig._numberOfCorpses = value;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001569 RID: 5481 RVA: 0x00050E28 File Offset: 0x0004F028
		// (set) Token: 0x0600156A RID: 5482 RVA: 0x00050E2F File Offset: 0x0004F02F
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool ShowBlood { get; set; } = true;

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x0600156B RID: 5483 RVA: 0x00050E37 File Offset: 0x0004F037
		// (set) Token: 0x0600156C RID: 5484 RVA: 0x00050E3E File Offset: 0x0004F03E
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool DisplayAttackDirection { get; set; } = true;

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x0600156D RID: 5485 RVA: 0x00050E46 File Offset: 0x0004F046
		// (set) Token: 0x0600156E RID: 5486 RVA: 0x00050E4D File Offset: 0x0004F04D
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool DisplayTargetingReticule { get; set; } = true;

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x0600156F RID: 5487 RVA: 0x00050E55 File Offset: 0x0004F055
		// (set) Token: 0x06001570 RID: 5488 RVA: 0x00050E5C File Offset: 0x0004F05C
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool ForceVSyncInMenus { get; set; } = true;

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001571 RID: 5489 RVA: 0x00050E64 File Offset: 0x0004F064
		// (set) Token: 0x06001572 RID: 5490 RVA: 0x00050E6B File Offset: 0x0004F06B
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2, 3, 4, 5, 6 }, false)]
		public static int BattleSize
		{
			get
			{
				return BannerlordConfig._battleSize;
			}
			set
			{
				BannerlordConfig._battleSize = value;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001573 RID: 5491 RVA: 0x00050E73 File Offset: 0x0004F073
		// (set) Token: 0x06001574 RID: 5492 RVA: 0x00050E7A File Offset: 0x0004F07A
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2, 3 }, false)]
		public static int ReinforcementWaveCount { get; set; } = 3;

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001575 RID: 5493 RVA: 0x00050E82 File Offset: 0x0004F082
		public static float CivilianAgentCount
		{
			get
			{
				return (float)BannerlordConfig.GetRealBattleSize() * 0.5f;
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001576 RID: 5494 RVA: 0x00050E90 File Offset: 0x0004F090
		// (set) Token: 0x06001577 RID: 5495 RVA: 0x00050E97 File Offset: 0x0004F097
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static float FirstPersonFov { get; set; } = 65f;

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001578 RID: 5496 RVA: 0x00050E9F File Offset: 0x0004F09F
		// (set) Token: 0x06001579 RID: 5497 RVA: 0x00050EA6 File Offset: 0x0004F0A6
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static float UIScale { get; set; } = 1f;

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x0600157A RID: 5498 RVA: 0x00050EAE File Offset: 0x0004F0AE
		// (set) Token: 0x0600157B RID: 5499 RVA: 0x00050EB5 File Offset: 0x0004F0B5
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static float CombatCameraDistance { get; set; } = 1f;

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x0600157C RID: 5500 RVA: 0x00050EBD File Offset: 0x0004F0BD
		// (set) Token: 0x0600157D RID: 5501 RVA: 0x00050EC4 File Offset: 0x0004F0C4
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2, 3 }, false)]
		public static int TurnCameraWithHorseInFirstPerson { get; set; } = 2;

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x0600157E RID: 5502 RVA: 0x00050ECC File Offset: 0x0004F0CC
		// (set) Token: 0x0600157F RID: 5503 RVA: 0x00050ED3 File Offset: 0x0004F0D3
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool ReportDamage { get; set; } = true;

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06001580 RID: 5504 RVA: 0x00050EDB File Offset: 0x0004F0DB
		// (set) Token: 0x06001581 RID: 5505 RVA: 0x00050EE2 File Offset: 0x0004F0E2
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool ReportBark { get; set; } = true;

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001582 RID: 5506 RVA: 0x00050EEA File Offset: 0x0004F0EA
		// (set) Token: 0x06001583 RID: 5507 RVA: 0x00050EF1 File Offset: 0x0004F0F1
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool LockTarget { get; set; } = false;

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001584 RID: 5508 RVA: 0x00050EF9 File Offset: 0x0004F0F9
		// (set) Token: 0x06001585 RID: 5509 RVA: 0x00050F00 File Offset: 0x0004F100
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableTutorialHints { get; set; } = true;

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001586 RID: 5510 RVA: 0x00050F08 File Offset: 0x0004F108
		// (set) Token: 0x06001587 RID: 5511 RVA: 0x00050F0F File Offset: 0x0004F10F
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static int AutoSaveInterval
		{
			get
			{
				return BannerlordConfig._autoSaveInterval;
			}
			set
			{
				if (value == 4)
				{
					BannerlordConfig._autoSaveInterval = -1;
					return;
				}
				BannerlordConfig._autoSaveInterval = value;
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001588 RID: 5512 RVA: 0x00050F22 File Offset: 0x0004F122
		// (set) Token: 0x06001589 RID: 5513 RVA: 0x00050F29 File Offset: 0x0004F129
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static float FriendlyTroopsBannerOpacity { get; set; } = 1f;

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x0600158A RID: 5514 RVA: 0x00050F31 File Offset: 0x0004F131
		// (set) Token: 0x0600158B RID: 5515 RVA: 0x00050F38 File Offset: 0x0004F138
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2 }, false)]
		public static int AlwaysShowFriendlyTroopBannersType { get; set; } = 1;

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x0600158C RID: 5516 RVA: 0x00050F40 File Offset: 0x0004F140
		// (set) Token: 0x0600158D RID: 5517 RVA: 0x00050F47 File Offset: 0x0004F147
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2 }, false)]
		public static int KillFeedVisualType { get; set; } = 1;

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x0600158E RID: 5518 RVA: 0x00050F4F File Offset: 0x0004F14F
		// (set) Token: 0x0600158F RID: 5519 RVA: 0x00050F56 File Offset: 0x0004F156
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool ShowFormationDistances { get; set; } = false;

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001590 RID: 5520 RVA: 0x00050F5E File Offset: 0x0004F15E
		// (set) Token: 0x06001591 RID: 5521 RVA: 0x00050F65 File Offset: 0x0004F165
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2 }, false)]
		public static int AutoTrackAttackedSettlements { get; set; } = 0;

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001592 RID: 5522 RVA: 0x00050F6D File Offset: 0x0004F16D
		// (set) Token: 0x06001593 RID: 5523 RVA: 0x00050F74 File Offset: 0x0004F174
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool ReportPersonalDamage { get; set; } = true;

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001594 RID: 5524 RVA: 0x00050F7C File Offset: 0x0004F17C
		// (set) Token: 0x06001595 RID: 5525 RVA: 0x00050F83 File Offset: 0x0004F183
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool SlowDownOnOrder { get; set; } = true;

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001596 RID: 5526 RVA: 0x00050F8B File Offset: 0x0004F18B
		// (set) Token: 0x06001597 RID: 5527 RVA: 0x00050F92 File Offset: 0x0004F192
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool StopGameOnFocusLost
		{
			get
			{
				return BannerlordConfig._stopGameOnFocusLost;
			}
			set
			{
				BannerlordConfig._stopGameOnFocusLost = value;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001598 RID: 5528 RVA: 0x00050F9A File Offset: 0x0004F19A
		// (set) Token: 0x06001599 RID: 5529 RVA: 0x00050FA1 File Offset: 0x0004F1A1
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool ReportExperience { get; set; } = true;

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x0600159A RID: 5530 RVA: 0x00050FA9 File Offset: 0x0004F1A9
		// (set) Token: 0x0600159B RID: 5531 RVA: 0x00050FB0 File Offset: 0x0004F1B0
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableDamageTakenVisuals { get; set; } = true;

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x0600159C RID: 5532 RVA: 0x00050FB8 File Offset: 0x0004F1B8
		// (set) Token: 0x0600159D RID: 5533 RVA: 0x00050FBF File Offset: 0x0004F1BF
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableVerticalAimCorrection { get; set; } = true;

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x0600159E RID: 5534 RVA: 0x00050FC7 File Offset: 0x0004F1C7
		// (set) Token: 0x0600159F RID: 5535 RVA: 0x00050FCE File Offset: 0x0004F1CE
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static float ZoomSensitivityModifier { get; set; } = 0.66666f;

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x060015A0 RID: 5536 RVA: 0x00050FD6 File Offset: 0x0004F1D6
		// (set) Token: 0x060015A1 RID: 5537 RVA: 0x00050FDD File Offset: 0x0004F1DD
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1 }, false)]
		public static int CrosshairType { get; set; } = 0;

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x060015A2 RID: 5538 RVA: 0x00050FE5 File Offset: 0x0004F1E5
		// (set) Token: 0x060015A3 RID: 5539 RVA: 0x00050FEC File Offset: 0x0004F1EC
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableGenericAvatars { get; set; } = false;

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x060015A4 RID: 5540 RVA: 0x00050FF4 File Offset: 0x0004F1F4
		// (set) Token: 0x060015A5 RID: 5541 RVA: 0x00050FFB File Offset: 0x0004F1FB
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableGenericNames { get; set; } = false;

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x060015A6 RID: 5542 RVA: 0x00051003 File Offset: 0x0004F203
		// (set) Token: 0x060015A7 RID: 5543 RVA: 0x0005100A File Offset: 0x0004F20A
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool HideFullServers { get; set; } = false;

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x00051012 File Offset: 0x0004F212
		// (set) Token: 0x060015A9 RID: 5545 RVA: 0x00051019 File Offset: 0x0004F219
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool HideEmptyServers { get; set; } = false;

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x060015AA RID: 5546 RVA: 0x00051021 File Offset: 0x0004F221
		// (set) Token: 0x060015AB RID: 5547 RVA: 0x00051028 File Offset: 0x0004F228
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool HidePasswordProtectedServers { get; set; } = false;

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x060015AC RID: 5548 RVA: 0x00051030 File Offset: 0x0004F230
		// (set) Token: 0x060015AD RID: 5549 RVA: 0x00051037 File Offset: 0x0004F237
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool HideUnofficialServers { get; set; } = false;

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x060015AE RID: 5550 RVA: 0x0005103F File Offset: 0x0004F23F
		// (set) Token: 0x060015AF RID: 5551 RVA: 0x00051046 File Offset: 0x0004F246
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool HideModuleIncompatibleServers { get; set; } = false;

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x060015B0 RID: 5552 RVA: 0x0005104E File Offset: 0x0004F24E
		// (set) Token: 0x060015B1 RID: 5553 RVA: 0x00051055 File Offset: 0x0004F255
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool ShowOnlyFavoriteServers { get; set; } = false;

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x060015B2 RID: 5554 RVA: 0x0005105D File Offset: 0x0004F25D
		// (set) Token: 0x060015B3 RID: 5555 RVA: 0x00051064 File Offset: 0x0004F264
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1 }, false)]
		public static int OrderType
		{
			get
			{
				return BannerlordConfig._orderType;
			}
			set
			{
				BannerlordConfig._orderType = value;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x060015B4 RID: 5556 RVA: 0x0005106C File Offset: 0x0004F26C
		// (set) Token: 0x060015B5 RID: 5557 RVA: 0x00051073 File Offset: 0x0004F273
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1 }, false)]
		public static int OrderLayoutType
		{
			get
			{
				return BannerlordConfig._orderLayoutType;
			}
			set
			{
				BannerlordConfig._orderLayoutType = value;
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x060015B6 RID: 5558 RVA: 0x0005107B File Offset: 0x0004F27B
		// (set) Token: 0x060015B7 RID: 5559 RVA: 0x00051082 File Offset: 0x0004F282
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableVoiceChat { get; set; } = true;

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x060015B8 RID: 5560 RVA: 0x0005108A File Offset: 0x0004F28A
		// (set) Token: 0x060015B9 RID: 5561 RVA: 0x00051091 File Offset: 0x0004F291
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableDeathIcon { get; set; } = true;

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x060015BA RID: 5562 RVA: 0x00051099 File Offset: 0x0004F299
		// (set) Token: 0x060015BB RID: 5563 RVA: 0x000510A0 File Offset: 0x0004F2A0
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableNetworkAlertIcons { get; set; } = true;

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x060015BC RID: 5564 RVA: 0x000510A8 File Offset: 0x0004F2A8
		// (set) Token: 0x060015BD RID: 5565 RVA: 0x000510AF File Offset: 0x0004F2AF
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableSingleplayerChatBox { get; set; } = true;

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x060015BE RID: 5566 RVA: 0x000510B7 File Offset: 0x0004F2B7
		// (set) Token: 0x060015BF RID: 5567 RVA: 0x000510BE File Offset: 0x0004F2BE
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableMultiplayerChatBox { get; set; } = true;

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x060015C0 RID: 5568 RVA: 0x000510C6 File Offset: 0x0004F2C6
		// (set) Token: 0x060015C1 RID: 5569 RVA: 0x000510CD File Offset: 0x0004F2CD
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static float ChatBoxSizeX { get; set; } = 495f;

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x060015C2 RID: 5570 RVA: 0x000510D5 File Offset: 0x0004F2D5
		// (set) Token: 0x060015C3 RID: 5571 RVA: 0x000510DC File Offset: 0x0004F2DC
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static float ChatBoxSizeY { get; set; } = 340f;

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x060015C4 RID: 5572 RVA: 0x000510E4 File Offset: 0x0004F2E4
		// (set) Token: 0x060015C5 RID: 5573 RVA: 0x000510EB File Offset: 0x0004F2EB
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static string LatestSaveGameName { get; set; } = string.Empty;

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x060015C6 RID: 5574 RVA: 0x000510F3 File Offset: 0x0004F2F3
		// (set) Token: 0x060015C7 RID: 5575 RVA: 0x000510FA File Offset: 0x0004F2FA
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool HideBattleUI { get; set; } = false;

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x060015C8 RID: 5576 RVA: 0x00051102 File Offset: 0x0004F302
		// (set) Token: 0x060015C9 RID: 5577 RVA: 0x00051109 File Offset: 0x0004F309
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2, 3 }, false)]
		public static int UnitSpawnPrioritization { get; set; } = 0;

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x060015CA RID: 5578 RVA: 0x00051111 File Offset: 0x0004F311
		// (set) Token: 0x060015CB RID: 5579 RVA: 0x00051118 File Offset: 0x0004F318
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool IAPNoticeConfirmed { get; set; } = false;

		// Token: 0x0400069C RID: 1692
		private static int[] _battleSizes = new int[] { 200, 300, 400, 500, 600, 800, 1000 };

		// Token: 0x0400069D RID: 1693
		private static int[] _siegeBattleSizes = new int[] { 150, 230, 320, 425, 540, 625, 1000 };

		// Token: 0x0400069E RID: 1694
		private static int[] _sallyOutBattleSizes = new int[] { 150, 200, 240, 280, 320, 360, 400 };

		// Token: 0x0400069F RID: 1695
		private static int[] _reinforcementWaveCounts = new int[] { 3, 4, 5, 0 };

		// Token: 0x040006A0 RID: 1696
		public const int MaxCorpseCount = 1021;

		// Token: 0x040006A1 RID: 1697
		public static double SiegeBattleSizeMultiplier = 0.8;

		// Token: 0x040006A2 RID: 1698
		public const int DefaultMapDoubleClickBehavior = 0;

		// Token: 0x040006A3 RID: 1699
		public const int DefaultPlayerReceviedDamageDifficulty = 0;

		// Token: 0x040006A4 RID: 1700
		public const bool DefaultGyroOverrideForAttackDefend = false;

		// Token: 0x040006A5 RID: 1701
		public const int DefaultAttackDirectionControl = 1;

		// Token: 0x040006A6 RID: 1702
		public const int DefaultDefendDirectionControl = 0;

		// Token: 0x040006A7 RID: 1703
		public const int DefaultNumberOfCorpses = 3;

		// Token: 0x040006A8 RID: 1704
		public const bool DefaultShowBlood = true;

		// Token: 0x040006A9 RID: 1705
		public const bool DefaultDisplayAttackDirection = true;

		// Token: 0x040006AA RID: 1706
		public const bool DefaultDisplayTargetingReticule = true;

		// Token: 0x040006AB RID: 1707
		public const bool DefaultForceVSyncInMenus = true;

		// Token: 0x040006AC RID: 1708
		public const int DefaultBattleSize = 2;

		// Token: 0x040006AD RID: 1709
		public const int DefaultReinforcementWaveCount = 3;

		// Token: 0x040006AE RID: 1710
		public const float DefaultBattleSizeMultiplier = 0.5f;

		// Token: 0x040006AF RID: 1711
		public const float DefaultFirstPersonFov = 65f;

		// Token: 0x040006B0 RID: 1712
		public const float DefaultUIScale = 1f;

		// Token: 0x040006B1 RID: 1713
		public const float DefaultCombatCameraDistance = 1f;

		// Token: 0x040006B2 RID: 1714
		public const int DefaultCombatAI = 0;

		// Token: 0x040006B3 RID: 1715
		public const int DefaultTurnCameraWithHorseInFirstPerson = 2;

		// Token: 0x040006B4 RID: 1716
		public const int DefaultAutoSaveInterval = 30;

		// Token: 0x040006B5 RID: 1717
		public const float DefaultFriendlyTroopsBannerOpacity = 1f;

		// Token: 0x040006B6 RID: 1718
		public const int DefaultAlwaysShowFriendlyTroopBannersType = 1;

		// Token: 0x040006B7 RID: 1719
		public const bool DefaultShowFormationDistances = false;

		// Token: 0x040006B8 RID: 1720
		public const bool DefaultReportDamage = true;

		// Token: 0x040006B9 RID: 1721
		public const bool DefaultReportBark = true;

		// Token: 0x040006BA RID: 1722
		public const bool DefaultEnableTutorialHints = true;

		// Token: 0x040006BB RID: 1723
		public const int DefaultKillFeedVisualType = 1;

		// Token: 0x040006BC RID: 1724
		public const int DefaultAutoTrackAttackedSettlements = 0;

		// Token: 0x040006BD RID: 1725
		public const bool DefaultReportPersonalDamage = true;

		// Token: 0x040006BE RID: 1726
		public const bool DefaultStopGameOnFocusLost = true;

		// Token: 0x040006BF RID: 1727
		public const bool DefaultSlowDownOnOrder = true;

		// Token: 0x040006C0 RID: 1728
		public const bool DefaultReportExperience = true;

		// Token: 0x040006C1 RID: 1729
		public const bool DefaultEnableDamageTakenVisuals = true;

		// Token: 0x040006C2 RID: 1730
		public const bool DefaultEnableVoiceChat = true;

		// Token: 0x040006C3 RID: 1731
		public const bool DefaultEnableDeathIcon = true;

		// Token: 0x040006C4 RID: 1732
		public const bool DefaultEnableNetworkAlertIcons = true;

		// Token: 0x040006C5 RID: 1733
		public const bool DefaultEnableVerticalAimCorrection = true;

		// Token: 0x040006C6 RID: 1734
		public const float DefaultZoomSensitivityModifier = 0.66666f;

		// Token: 0x040006C7 RID: 1735
		public const bool DefaultSingleplayerEnableChatBox = true;

		// Token: 0x040006C8 RID: 1736
		public const bool DefaultMultiplayerEnableChatBox = true;

		// Token: 0x040006C9 RID: 1737
		public const float DefaultChatBoxSizeX = 495f;

		// Token: 0x040006CA RID: 1738
		public const float DefaultChatBoxSizeY = 340f;

		// Token: 0x040006CB RID: 1739
		public const int DefaultCrosshairType = 0;

		// Token: 0x040006CC RID: 1740
		public const bool DefaultEnableGenericAvatars = false;

		// Token: 0x040006CD RID: 1741
		public const bool DefaultEnableGenericNames = false;

		// Token: 0x040006CE RID: 1742
		public const bool DefaultHideFullServers = false;

		// Token: 0x040006CF RID: 1743
		public const bool DefaultHideEmptyServers = false;

		// Token: 0x040006D0 RID: 1744
		public const bool DefaultHidePasswordProtectedServers = false;

		// Token: 0x040006D1 RID: 1745
		public const bool DefaultHideUnofficialServers = false;

		// Token: 0x040006D2 RID: 1746
		public const bool DefaultHideModuleIncompatibleServers = false;

		// Token: 0x040006D3 RID: 1747
		public const bool DefaultShowOnlyFavoriteServers = false;

		// Token: 0x040006D4 RID: 1748
		public const int DefaultOrderLayoutType = 0;

		// Token: 0x040006D5 RID: 1749
		public const bool DefaultHideBattleUI = false;

		// Token: 0x040006D6 RID: 1750
		public const int DefaultUnitSpawnPrioritization = 0;

		// Token: 0x040006D7 RID: 1751
		public const int DefaultOrderType = 0;

		// Token: 0x040006D8 RID: 1752
		public const bool DefaultLockTarget = false;

		// Token: 0x040006D9 RID: 1753
		private static string _language = BannerlordConfig.DefaultLanguage;

		// Token: 0x040006DA RID: 1754
		private static string _voiceLanguage = BannerlordConfig.DefaultLanguage;

		// Token: 0x040006E0 RID: 1760
		private static int _numberOfCorpses = 3;

		// Token: 0x040006E5 RID: 1765
		private static int _battleSize = 2;

		// Token: 0x040006EF RID: 1775
		private static int _autoSaveInterval = 30;

		// Token: 0x040006F7 RID: 1783
		private static bool _stopGameOnFocusLost = true;

		// Token: 0x04000705 RID: 1797
		private static int _orderType = 0;

		// Token: 0x04000706 RID: 1798
		private static int _orderLayoutType = 0;

		// Token: 0x020004E4 RID: 1252
		private interface IConfigPropertyBoundChecker<T>
		{
		}

		// Token: 0x020004E5 RID: 1253
		private abstract class ConfigProperty : Attribute
		{
		}

		// Token: 0x020004E6 RID: 1254
		private sealed class ConfigPropertyInt : BannerlordConfig.ConfigProperty
		{
			// Token: 0x06003B2B RID: 15147 RVA: 0x000ED644 File Offset: 0x000EB844
			public ConfigPropertyInt(int[] possibleValues, bool isRange = false)
			{
				this._possibleValues = possibleValues;
				this._isRange = isRange;
				bool isRange2 = this._isRange;
			}

			// Token: 0x06003B2C RID: 15148 RVA: 0x000ED664 File Offset: 0x000EB864
			public bool IsValidValue(int value)
			{
				if (this._isRange)
				{
					return value >= this._possibleValues[0] && value <= this._possibleValues[1];
				}
				int[] possibleValues = this._possibleValues;
				for (int i = 0; i < possibleValues.Length; i++)
				{
					if (possibleValues[i] == value)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x04001C4B RID: 7243
			private int[] _possibleValues;

			// Token: 0x04001C4C RID: 7244
			private bool _isRange;
		}

		// Token: 0x020004E7 RID: 1255
		private sealed class ConfigPropertyUnbounded : BannerlordConfig.ConfigProperty
		{
		}
	}
}
