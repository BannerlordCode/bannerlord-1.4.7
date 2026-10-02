using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Options;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Options.ManagedOptions
{
	// Token: 0x0200039C RID: 924
	public class ManagedSelectionOptionData : ManagedOptionData, ISelectionOptionData, IOptionData
	{
		// Token: 0x060034C3 RID: 13507 RVA: 0x000D9428 File Offset: 0x000D7628
		public ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType type)
			: base(type)
		{
			this._selectableOptionsLimit = ManagedSelectionOptionData.GetOptionsLimit(type);
			this._selectableOptionNames = ManagedSelectionOptionData.GetOptionNames(type);
		}

		// Token: 0x060034C4 RID: 13508 RVA: 0x000D9449 File Offset: 0x000D7649
		public int GetSelectableOptionsLimit()
		{
			return this._selectableOptionsLimit;
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x000D9451 File Offset: 0x000D7651
		public IEnumerable<SelectionData> GetSelectableOptionNames()
		{
			return this._selectableOptionNames;
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x000D945C File Offset: 0x000D765C
		public static int GetOptionsLimit(ManagedOptions.ManagedOptionsType optionType)
		{
			if (optionType <= ManagedOptions.ManagedOptionsType.ReportCasualtiesType)
			{
				switch (optionType)
				{
				case ManagedOptions.ManagedOptionsType.Language:
					return LocalizedTextManager.GetLanguageIds(NativeConfig.IsDevelopmentMode).Count;
				case ManagedOptions.ManagedOptionsType.GyroOverrideForAttackDefend:
				case ManagedOptions.ManagedOptionsType.ShowBlood:
				case ManagedOptions.ManagedOptionsType.ShowAttackDirection:
				case ManagedOptions.ManagedOptionsType.ShowTargetingReticle:
				case ManagedOptions.ManagedOptionsType.AutoSaveInterval:
				case ManagedOptions.ManagedOptionsType.FriendlyTroopsBannerOpacity:
					break;
				case ManagedOptions.ManagedOptionsType.ControlBlockDirection:
					return 3;
				case ManagedOptions.ManagedOptionsType.ControlAttackDirection:
					return 3;
				case ManagedOptions.ManagedOptionsType.NumberOfCorpses:
					return 6;
				case ManagedOptions.ManagedOptionsType.BattleSize:
					return 7;
				case ManagedOptions.ManagedOptionsType.ReinforcementWaveCount:
					return 4;
				case ManagedOptions.ManagedOptionsType.TurnCameraWithHorseInFirstPerson:
					return 4;
				case ManagedOptions.ManagedOptionsType.AlwaysShowFriendlyTroopBannersType:
					return 3;
				default:
					if (optionType == ManagedOptions.ManagedOptionsType.ReportCasualtiesType)
					{
						return 3;
					}
					break;
				}
			}
			else
			{
				switch (optionType)
				{
				case ManagedOptions.ManagedOptionsType.CrosshairType:
					return 2;
				case ManagedOptions.ManagedOptionsType.EnableGenericAvatars:
				case ManagedOptions.ManagedOptionsType.EnableGenericNames:
					break;
				case ManagedOptions.ManagedOptionsType.OrderType:
					return 2;
				case ManagedOptions.ManagedOptionsType.OrderLayoutType:
					return 2;
				case ManagedOptions.ManagedOptionsType.AutoTrackAttackedSettlements:
					return 3;
				default:
					switch (optionType)
					{
					case ManagedOptions.ManagedOptionsType.UnitSpawnPrioritization:
						return 4;
					case ManagedOptions.ManagedOptionsType.VoiceLanguage:
						return LocalizedVoiceManager.GetVoiceLanguageIds().Count;
					case ManagedOptions.ManagedOptionsType.PlayerReceivedDamageDifficulty:
						return 3;
					case ManagedOptions.ManagedOptionsType.MapDoubleClickBehavior:
						return 3;
					}
					break;
				}
			}
			return 0;
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x000D9531 File Offset: 0x000D7731
		private static IEnumerable<SelectionData> GetOptionNames(ManagedOptions.ManagedOptionsType type)
		{
			if (type == ManagedOptions.ManagedOptionsType.Language)
			{
				List<string> languageIds = LocalizedTextManager.GetLanguageIds(NativeConfig.IsDevelopmentMode);
				int num;
				for (int i = 0; i < languageIds.Count; i = num + 1)
				{
					yield return new SelectionData(false, LocalizedTextManager.GetLanguageTitle(languageIds[i]));
					num = i;
				}
				languageIds = null;
			}
			else if (type == ManagedOptions.ManagedOptionsType.VoiceLanguage)
			{
				List<string> languageIds = LocalizedVoiceManager.GetVoiceLanguageIds();
				int num;
				for (int i = 0; i < languageIds.Count; i = num + 1)
				{
					yield return new SelectionData(false, LocalizedTextManager.GetLanguageTitle(languageIds[i]));
					num = i;
				}
				languageIds = null;
			}
			else
			{
				int i = ManagedSelectionOptionData.GetOptionsLimit(type);
				string typeName = type.ToString();
				int num;
				for (int j = 0; j < i; j = num + 1)
				{
					yield return new SelectionData(true, "str_options_type_" + typeName + "_" + j.ToString());
					num = j;
				}
				typeName = null;
			}
			yield break;
		}

		// Token: 0x04001665 RID: 5733
		private readonly int _selectableOptionsLimit;

		// Token: 0x04001666 RID: 5734
		private readonly IEnumerable<SelectionData> _selectableOptionNames;
	}
}
