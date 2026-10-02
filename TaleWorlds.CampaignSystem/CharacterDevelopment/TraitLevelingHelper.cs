using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x020003AE RID: 942
	public static class TraitLevelingHelper
	{
		// Token: 0x060036E8 RID: 14056 RVA: 0x000E49F4 File Offset: 0x000E2BF4
		public static void UpdateTraitXPAccordingToTraitLevels()
		{
			foreach (TraitObject traitObject in TraitObject.All)
			{
				int traitLevel = Hero.MainHero.GetTraitLevel(traitObject);
				if (traitLevel != 0)
				{
					int traitXpRequiredForTraitLevel = Campaign.Current.Models.CharacterDevelopmentModel.GetTraitXpRequiredForTraitLevel(traitObject, traitLevel);
					Campaign.Current.PlayerTraitDeveloper.SetPropertyValue(traitObject, traitXpRequiredForTraitLevel);
				}
			}
		}

		// Token: 0x060036E9 RID: 14057 RVA: 0x000E4A78 File Offset: 0x000E2C78
		public static void OnBattleWon(MapEvent mapEvent, float contribution)
		{
			float strengthRatio = mapEvent.GetMapEventSide(PlayerEncounter.Current.PlayerSide).StrengthRatio;
			if (strengthRatio > 9f)
			{
				int num = (int)(MBMath.Map(strengthRatio, 9f, 10f, 5f, 20f) * contribution);
				if (num > 0)
				{
					TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Valor, num, ActionNotes.BattleValor, null);
				}
			}
		}

		// Token: 0x060036EA RID: 14058 RVA: 0x000E4AD3 File Offset: 0x000E2CD3
		public static void OnTroopsSacrificed()
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Valor, -30, ActionNotes.SacrificedTroops, null);
		}

		// Token: 0x060036EB RID: 14059 RVA: 0x000E4AE4 File Offset: 0x000E2CE4
		public static void OnLordExecuted()
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Honor, -1000, ActionNotes.SacrificedTroops, null);
		}

		// Token: 0x060036EC RID: 14060 RVA: 0x000E4AF8 File Offset: 0x000E2CF8
		public static void OnTradeAgreementBroken()
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Honor, -1000, ActionNotes.DishonestBusinessQuarrel, null);
		}

		// Token: 0x060036ED RID: 14061 RVA: 0x000E4B0B File Offset: 0x000E2D0B
		public static void OnVillageRaided()
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Mercy, -30, ActionNotes.VillageRaid, null);
		}

		// Token: 0x060036EE RID: 14062 RVA: 0x000E4B1C File Offset: 0x000E2D1C
		public static void OnHostileAction(int amount)
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Honor, amount, ActionNotes.HostileAction, null);
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Mercy, amount, ActionNotes.HostileAction, null);
		}

		// Token: 0x060036EF RID: 14063 RVA: 0x000E4B3A File Offset: 0x000E2D3A
		public static void OnPartyTreatedWell()
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Generosity, 20, ActionNotes.PartyTakenCareOf, null);
		}

		// Token: 0x060036F0 RID: 14064 RVA: 0x000E4B4B File Offset: 0x000E2D4B
		public static void OnPartyStarved()
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Generosity, -20, ActionNotes.PartyHungry, null);
		}

		// Token: 0x060036F1 RID: 14065 RVA: 0x000E4B5C File Offset: 0x000E2D5C
		public static void OnIssueFailed(Hero targetHero, Tuple<TraitObject, int>[] effectedTraits)
		{
			foreach (Tuple<TraitObject, int> tuple in effectedTraits)
			{
				TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(tuple.Item1, tuple.Item2, ActionNotes.QuestFailed, targetHero);
			}
		}

		// Token: 0x060036F2 RID: 14066 RVA: 0x000E4B94 File Offset: 0x000E2D94
		public static void OnIssueSolvedThroughQuest(Hero targetHero, Tuple<TraitObject, int>[] effectedTraits)
		{
			foreach (Tuple<TraitObject, int> tuple in effectedTraits)
			{
				TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(tuple.Item1, tuple.Item2, ActionNotes.QuestSuccess, targetHero);
			}
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x000E4BC9 File Offset: 0x000E2DC9
		public static void OnIssueSolvedThroughQuest(Hero targetHero, TraitObject trait, int xp)
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(trait, xp, ActionNotes.QuestSuccess, targetHero);
		}

		// Token: 0x060036F4 RID: 14068 RVA: 0x000E4BD8 File Offset: 0x000E2DD8
		public static void OnIssueSolvedThroughAlternativeSolution(Hero targetHero, Tuple<TraitObject, int>[] effectedTraits)
		{
			foreach (Tuple<TraitObject, int> tuple in effectedTraits)
			{
				TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(tuple.Item1, tuple.Item2, ActionNotes.QuestSuccess, targetHero);
			}
		}

		// Token: 0x060036F5 RID: 14069 RVA: 0x000E4C10 File Offset: 0x000E2E10
		public static void OnIssueSolvedThroughBetrayal(Hero targetHero, Tuple<TraitObject, int>[] effectedTraits)
		{
			foreach (Tuple<TraitObject, int> tuple in effectedTraits)
			{
				TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(tuple.Item1, tuple.Item2, ActionNotes.QuestBetrayal, targetHero);
			}
		}

		// Token: 0x060036F6 RID: 14070 RVA: 0x000E4C45 File Offset: 0x000E2E45
		public static void OnLordFreed(Hero targetHero)
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Calculating, 20, ActionNotes.NPCFreed, targetHero);
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x000E4C56 File Offset: 0x000E2E56
		public static void OnPersuasionDefection(Hero targetHero)
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Calculating, 20, ActionNotes.PersuadedToDefect, targetHero);
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x000E4C68 File Offset: 0x000E2E68
		public static void OnSiegeAftermathApplied(Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType, TraitObject[] effectedTraits)
		{
			foreach (TraitObject traitObject in effectedTraits)
			{
				TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(traitObject, Campaign.Current.Models.SiegeAftermathModel.GetSiegeAftermathTraitXpChangeForPlayer(traitObject, settlement, aftermathType), ActionNotes.SiegeAftermath, null);
			}
		}

		// Token: 0x060036F9 RID: 14073 RVA: 0x000E4CA9 File Offset: 0x000E2EA9
		public static void OnIncidentResolved(TraitObject trait, int xpValue)
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(trait, xpValue, ActionNotes.DefaultNote, Hero.MainHero);
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x000E4CB8 File Offset: 0x000E2EB8
		public static void OnAllianceBrokenThroughHostility()
		{
			TraitLevelingHelper.AddPlayerTraitXPAndLogEntry(DefaultTraits.Honor, -1000, ActionNotes.DishonestBusinessQuarrel, null);
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x000E4CCC File Offset: 0x000E2ECC
		private static void AddPlayerTraitXPAndLogEntry(TraitObject trait, int xpValue, ActionNotes context, Hero referenceHero)
		{
			int traitLevel = Hero.MainHero.GetTraitLevel(trait);
			TraitLevelingHelper.AddTraitXp(trait, xpValue);
			if (traitLevel != Hero.MainHero.GetTraitLevel(trait))
			{
				CampaignEventDispatcher.Instance.OnPlayerTraitChanged(trait, traitLevel);
			}
			if (MathF.Abs(xpValue) >= 10)
			{
				LogEntry.AddLogEntry(new PlayerReputationChangesLogEntry(trait, referenceHero, context));
			}
		}

		// Token: 0x060036FC RID: 14076 RVA: 0x000E4D20 File Offset: 0x000E2F20
		private static void AddTraitXp(TraitObject trait, int xpAmount)
		{
			xpAmount += Campaign.Current.PlayerTraitDeveloper.GetPropertyValue(trait);
			int num;
			int num2;
			Campaign.Current.Models.CharacterDevelopmentModel.GetTraitLevelForTraitXp(Hero.MainHero, trait, xpAmount, out num, out num2);
			Campaign.Current.PlayerTraitDeveloper.SetPropertyValue(trait, num2);
			if (num != Hero.MainHero.GetTraitLevel(trait))
			{
				Hero.MainHero.SetTraitLevel(trait, num);
			}
		}

		// Token: 0x04001113 RID: 4371
		private const int LordExecutedHonorPenalty = -1000;

		// Token: 0x04001114 RID: 4372
		private const int TradeAgreementBrokenPenalty = -1000;

		// Token: 0x04001115 RID: 4373
		private const int AllianceBrokenHonorPenalty = -1000;

		// Token: 0x04001116 RID: 4374
		private const int TroopsSacrificedValorPenalty = -30;

		// Token: 0x04001117 RID: 4375
		private const int VillageRaidedMercyPenalty = -30;

		// Token: 0x04001118 RID: 4376
		private const int PartyStarvingGenerosityPenalty = -20;

		// Token: 0x04001119 RID: 4377
		private const int PartyTreatedWellGenerosityBonus = 20;

		// Token: 0x0400111A RID: 4378
		private const int LordFreedCalculatingBonus = 20;

		// Token: 0x0400111B RID: 4379
		private const int PersuasionDefectionCalculatingBonus = 20;
	}
}
