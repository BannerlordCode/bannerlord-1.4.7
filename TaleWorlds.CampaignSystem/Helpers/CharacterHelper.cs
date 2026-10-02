using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace Helpers
{
	// Token: 0x02000014 RID: 20
	public static class CharacterHelper
	{
		// Token: 0x060000A3 RID: 163 RVA: 0x00009128 File Offset: 0x00007328
		public static TextObject GetDeathNotification(Hero victimHero, Hero killer, KillCharacterAction.KillCharacterActionDetail detail)
		{
			TextObject textObject = TextObject.GetEmpty();
			if (detail == KillCharacterAction.KillCharacterActionDetail.DiedInLabor || detail == KillCharacterAction.KillCharacterActionDetail.Murdered || detail == KillCharacterAction.KillCharacterActionDetail.DiedInBattle || detail == KillCharacterAction.KillCharacterActionDetail.DiedOfOldAge)
			{
				textObject = GameTexts.FindText("str_on_hero_killed", detail.ToString());
			}
			else if ((detail == KillCharacterAction.KillCharacterActionDetail.Executed || detail == KillCharacterAction.KillCharacterActionDetail.ExecutionAfterMapEvent) && killer != null)
			{
				textObject = GameTexts.FindText("str_on_hero_killed", detail.ToString());
				StringHelpers.SetCharacterProperties("KILLER", killer.CharacterObject, textObject, false);
			}
			else if (detail == KillCharacterAction.KillCharacterActionDetail.Lost)
			{
				textObject = GameTexts.FindText("str_on_hero_killed", detail.ToString());
				StringHelpers.SetCharacterProperties("VICTIM", victimHero.CharacterObject, textObject, false);
			}
			else
			{
				textObject = GameTexts.FindText("str_on_hero_killed", "Default");
			}
			StringHelpers.SetCharacterProperties("HERO", victimHero.CharacterObject, textObject, false);
			return textObject;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000091F4 File Offset: 0x000073F4
		public static DynamicBodyProperties GetDynamicBodyPropertiesBetweenMinMaxRange(CharacterObject character)
		{
			BodyProperties bodyPropertyMin = character.BodyPropertyRange.BodyPropertyMin;
			BodyProperties bodyPropertyMax = character.BodyPropertyRange.BodyPropertyMax;
			float num = ((bodyPropertyMin.Age < bodyPropertyMax.Age) ? bodyPropertyMin.Age : bodyPropertyMax.Age);
			float num2 = ((bodyPropertyMin.Age > bodyPropertyMax.Age) ? bodyPropertyMin.Age : bodyPropertyMax.Age);
			float num3 = ((bodyPropertyMin.Weight < bodyPropertyMax.Weight) ? bodyPropertyMin.Weight : bodyPropertyMax.Weight);
			float num4 = ((bodyPropertyMin.Weight > bodyPropertyMax.Weight) ? bodyPropertyMin.Weight : bodyPropertyMax.Weight);
			float num5 = ((bodyPropertyMin.Build < bodyPropertyMax.Build) ? bodyPropertyMin.Build : bodyPropertyMax.Build);
			float num6 = ((bodyPropertyMin.Build > bodyPropertyMax.Build) ? bodyPropertyMin.Build : bodyPropertyMax.Build);
			float num7 = MBRandom.RandomFloatRanged(num, num2);
			float num8 = MBRandom.RandomFloatRanged(num3, num4);
			float num9 = MBRandom.RandomFloatRanged(num5, num6);
			return new DynamicBodyProperties(num7, num8, num9);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00009308 File Offset: 0x00007508
		public static TextObject GetReputationDescription(CharacterObject character)
		{
			TextObject textObject = new TextObject("{=!}{REPUTATION_SUMMARY}", null);
			TextObject textObject2 = Campaign.Current.ConversationManager.FindMatchingTextOrNull("reputation", character);
			StringHelpers.SetCharacterProperties("NOTABLE", character, textObject2, false);
			textObject.SetTextVariable("REPUTATION_SUMMARY", textObject2);
			return textObject;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00009354 File Offset: 0x00007554
		[return: TupleElementNames(new string[] { "color1", "color2" })]
		public static ValueTuple<uint, uint> GetDeterministicColorsForCharacter(CharacterObject character, PartyBase partyBelongsTo)
		{
			if (!character.IsHero)
			{
				uint? num;
				if (partyBelongsTo == null)
				{
					num = null;
				}
				else
				{
					IFaction mapFaction = partyBelongsTo.MapFaction;
					num = ((mapFaction != null) ? new uint?(mapFaction.Color) : null);
				}
				uint num2 = num ?? 4291609515U;
				uint? num3;
				if (partyBelongsTo == null)
				{
					num3 = null;
				}
				else
				{
					IFaction mapFaction2 = partyBelongsTo.MapFaction;
					num3 = ((mapFaction2 != null) ? new uint?(mapFaction2.Color2) : null);
				}
				uint num4 = num3 ?? 4291609515U;
				return new ValueTuple<uint, uint>(num2, num4);
			}
			if (character.Occupation == Occupation.Lord)
			{
				IFaction mapFaction3 = character.HeroObject.MapFaction;
				uint num5 = ((mapFaction3 != null) ? mapFaction3.Color : 4291609515U);
				IFaction mapFaction4 = character.HeroObject.MapFaction;
				return new ValueTuple<uint, uint>(num5, (mapFaction4 != null) ? mapFaction4.Color2 : 4291609515U);
			}
			if (character.HeroObject.MapFaction == null)
			{
				return new ValueTuple<uint, uint>(4291609515U, 4291609515U);
			}
			string stringId = character.HeroObject.MapFaction.Culture.StringId;
			uint[] array;
			if (!(stringId == "empire"))
			{
				if (!(stringId == "sturgia"))
				{
					if (!(stringId == "aserai"))
					{
						if (!(stringId == "vlandia"))
						{
							if (!(stringId == "battania"))
							{
								if (!(stringId == "khuzait"))
								{
									array = CampaignData.EmpireHeroClothColors;
								}
								else
								{
									array = CampaignData.KhuzaitHeroClothColors;
								}
							}
							else
							{
								array = CampaignData.BattaniaHeroClothColors;
							}
						}
						else
						{
							array = CampaignData.VlandiaHeroClothColors;
						}
					}
					else
					{
						array = CampaignData.AseraiHeroClothColors;
					}
				}
				else
				{
					array = CampaignData.SturgiaHeroClothColors;
				}
			}
			else
			{
				array = CampaignData.EmpireHeroClothColors;
			}
			return new ValueTuple<uint, uint>(character.HeroObject.MapFaction.Color, CharacterHelper.GetDeterministicColorFromListForHero(character.HeroObject, array));
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00009521 File Offset: 0x00007721
		private static uint GetDeterministicColorFromListForHero(Hero hero, uint[] colors)
		{
			return colors.ElementAt<uint>(hero.RandomIntWithSeed(39U) % colors.Length);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00009535 File Offset: 0x00007735
		public static IFaceGeneratorCustomFilter GetFaceGeneratorFilter()
		{
			IFacegenCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IFacegenCampaignBehavior>();
			if (campaignBehavior == null)
			{
				return null;
			}
			return campaignBehavior.GetFaceGenFilter();
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000954C File Offset: 0x0000774C
		public static string GetNonconversationPose(CharacterObject character)
		{
			if (character.HeroObject.IsGangLeader)
			{
				return "aggressive";
			}
			if (!character.HeroObject.IsNoncombatant && character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) <= 0 && character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) < 0)
			{
				return "aggressive2";
			}
			if (!character.HeroObject.IsNoncombatant && character.HeroObject.IsLord && character.GetPersona() == DefaultTraits.PersonaCurt && character.HeroObject.GetTraitLevel(DefaultTraits.Honor) > 0)
			{
				return "warrior2";
			}
			if (character.HeroObject.Clan != null && character.HeroObject.Clan.IsNoble && character.GetPersona() == DefaultTraits.PersonaEarnest && character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) >= 0 && character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) >= 0)
			{
				return "hip2";
			}
			if (character.IsFemale && character.GetPersona() == DefaultTraits.PersonaSoftspoken)
			{
				return "demure";
			}
			if (character.IsFemale && character.GetPersona() == DefaultTraits.PersonaIronic)
			{
				return "confident3";
			}
			if (character.GetPersona() == DefaultTraits.PersonaCurt)
			{
				return "closed2";
			}
			if (character.GetPersona() == DefaultTraits.PersonaSoftspoken)
			{
				return "demure2";
			}
			if (character.GetPersona() == DefaultTraits.PersonaIronic)
			{
				return "confident";
			}
			if (character.GetPersona() == DefaultTraits.PersonaEarnest)
			{
				return "normal2";
			}
			return "normal";
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000096C4 File Offset: 0x000078C4
		public static string GetNonconversationFacialIdle(CharacterObject character)
		{
			string text = "convo_normal";
			string text2 = "convo_bemused";
			string text3 = "convo_mocking_teasing";
			string text4 = "convo_mocking_revenge";
			string text5 = "convo_delighted";
			string text6 = "convo_approving";
			string text7 = "convo_thinking";
			string text8 = "convo_focused_happy";
			string text9 = "convo_calm_friendly";
			string text10 = "convo_annoyed";
			string text11 = "convo_undecided_closed";
			string text12 = "convo_bored";
			string text13 = "convo_grave";
			string text14 = "convo_predatory";
			string text15 = "convo_confused_annoyed";
			if (character.HeroObject.IsGangLeader)
			{
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) <= 0 && character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) < 0)
				{
					return text14;
				}
				return text15;
			}
			else if (character.GetPersona() == DefaultTraits.PersonaCurt)
			{
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) < 0)
				{
					return text12;
				}
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Honor) > 0)
				{
					return text11;
				}
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) < 0)
				{
					return text10;
				}
				return text13;
			}
			else if (character.GetPersona() == DefaultTraits.PersonaEarnest)
			{
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) > 0)
				{
					return text8;
				}
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) < 0)
				{
					return text12;
				}
				return text5;
			}
			else if (character.IsFemale && character.GetPersona() == DefaultTraits.PersonaSoftspoken)
			{
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) > 0)
				{
					return text9;
				}
				if (!character.HeroObject.IsNoncombatant)
				{
					return text7;
				}
				return text6;
			}
			else
			{
				if (character.GetPersona() != DefaultTraits.PersonaIronic)
				{
					return text;
				}
				if (!character.HeroObject.IsNoncombatant && character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) < 0)
				{
					return text4;
				}
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) < 0)
				{
					return text3;
				}
				return text2;
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00009884 File Offset: 0x00007A84
		public static string GetStandingBodyIdle(CharacterObject character, PartyBase party)
		{
			HeroHelper.WillLordAttack();
			string text = "normal";
			TraitObject persona = character.GetPersona();
			bool flag = Settlement.CurrentSettlement != null;
			if (character.IsHero)
			{
				if (character.HeroObject.IsWounded)
				{
					return (MBRandom.RandomFloat <= 0.7f) ? "weary" : "weary2";
				}
				bool flag2 = !character.HeroObject.IsHumanPlayerCharacter;
				int superiorityState = CharacterHelper.GetSuperiorityState(character);
				if (flag2)
				{
					int relation = Hero.MainHero.GetRelation(character.HeroObject);
					bool flag3 = CharacterHelper.MorePowerThanPlayer(character);
					if (character.IsFemale && character.HeroObject.IsNoncombatant)
					{
						if (relation < 0)
						{
							text = "closed";
						}
						else if (persona == DefaultTraits.PersonaIronic)
						{
							text = ((MBRandom.RandomFloat <= 0.5f) ? "confident" : "confident2");
						}
						else if (persona == DefaultTraits.PersonaCurt)
						{
							text = ((MBRandom.RandomFloat <= 0.5f) ? "closed" : "confident");
						}
						else if (persona == DefaultTraits.PersonaEarnest || persona == DefaultTraits.PersonaSoftspoken)
						{
							text = ((MBRandom.RandomFloat <= 0.7f) ? "demure" : "confident");
						}
					}
					else if (relation <= -20)
					{
						if (superiorityState >= 0)
						{
							if (persona == DefaultTraits.PersonaSoftspoken)
							{
								text = (character.IsFemale ? "closed" : "warrior2");
							}
							else if (persona == DefaultTraits.PersonaIronic)
							{
								text = (character.IsFemale ? "confident2" : "aggressive");
							}
							else
							{
								text = (character.IsFemale ? "confident2" : "warrior");
							}
						}
						else if (superiorityState == -1)
						{
							if (persona == DefaultTraits.PersonaSoftspoken)
							{
								if (flag3)
								{
									text = "closed";
								}
								else
								{
									text = (character.IsFemale ? "closed" : "normal");
								}
							}
							else if (persona == DefaultTraits.PersonaIronic)
							{
								if (flag3)
								{
									text = ((MBRandom.RandomFloat <= 0.5f) ? "closed" : "warrior");
								}
								else
								{
									text = "closed";
								}
							}
							else
							{
								text = (character.IsFemale ? "closed" : "warrior2");
							}
						}
					}
					else if (superiorityState >= 0)
					{
						if (persona == DefaultTraits.PersonaIronic)
						{
							if (flag)
							{
								if (flag3)
								{
									text = ((MBRandom.RandomFloat <= 0.7f) ? "confident2" : "normal");
								}
								else
								{
									text = ((MBRandom.RandomFloat <= 0.5f) ? "hip" : "normal");
								}
							}
							else
							{
								text = "confident2";
							}
						}
						else if (persona == DefaultTraits.PersonaSoftspoken)
						{
							if (flag)
							{
								if (character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) + character.HeroObject.GetTraitLevel(DefaultTraits.Honor) > 0)
								{
									text = ((MBRandom.RandomFloat <= 0.5f) ? "normal2" : "demure2");
								}
								else if (flag3)
								{
									text = ((MBRandom.RandomFloat <= 0.5f) ? "normal" : "closed");
								}
								else
								{
									text = ((MBRandom.RandomFloat <= 0.5f) ? "normal" : "demure");
								}
							}
							else
							{
								text = "normal";
							}
						}
						else if (persona == DefaultTraits.PersonaCurt)
						{
							if (flag)
							{
								if (character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) + character.HeroObject.GetTraitLevel(DefaultTraits.Honor) > 0)
								{
									text = "demure2";
								}
								else if (flag3)
								{
									text = ((MBRandom.RandomFloat <= 0.6f) ? "normal" : "closed2");
								}
								else
								{
									text = ((MBRandom.RandomFloat <= 0.4f) ? "warrior2" : "closed");
								}
							}
							else
							{
								text = "normal";
							}
						}
						else if (persona == DefaultTraits.PersonaEarnest)
						{
							if (flag)
							{
								if (flag3)
								{
									text = ((MBRandom.RandomFloat <= 0.6f) ? "normal" : "confident");
								}
								else
								{
									text = ((MBRandom.RandomFloat <= 0.2f) ? "normal" : "confident");
								}
							}
							else
							{
								text = "normal";
							}
						}
					}
				}
			}
			if (party != null)
			{
				MobileParty mobileParty = party.MobileParty;
				if (mobileParty != null && mobileParty.IsCurrentlyAtSea && party != PartyBase.MainParty)
				{
					return "naval";
				}
			}
			if (character.Occupation == Occupation.Bandit || character.Occupation == Occupation.Gangster)
			{
				text = ((MBRandom.RandomFloat <= 0.7f) ? "aggressive" : "hip");
			}
			if (character.Occupation == Occupation.Guard || character.Occupation == Occupation.PrisonGuard || character.Occupation == Occupation.Soldier)
			{
				text = "normal";
			}
			return text;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00009CEC File Offset: 0x00007EEC
		public static string GetDefaultFaceIdle(CharacterObject character)
		{
			string text = "convo_normal";
			string text2 = "convo_bemused";
			string text3 = "convo_mocking_aristocratic";
			string text4 = "convo_mocking_teasing";
			string text5 = "convo_mocking_revenge";
			string text6 = "convo_contemptuous";
			string text7 = "convo_delighted";
			string text8 = "convo_approving";
			string text9 = "convo_relaxed_happy";
			string text10 = "convo_nonchalant";
			string text11 = "convo_thinking";
			string text12 = "convo_undecided_closed";
			string text13 = "convo_bored";
			string text14 = "convo_bored2";
			string text15 = "convo_grave";
			string text16 = "convo_stern";
			string text17 = "convo_very_stern";
			string text18 = "convo_beaten";
			string text19 = "convo_predatory";
			string text20 = "convo_confused_annoyed";
			bool flag = false;
			bool flag2 = false;
			if (character.IsHero)
			{
				flag = character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) + character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) > 0;
				flag2 = character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) + character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) < 0;
			}
			bool flag3 = Hero.MainHero.Clan.Renown < 0f;
			bool flag4 = false;
			if (PlayerEncounter.Current != null && PlayerEncounter.Current.PlayerSide == BattleSideEnum.Defender && (PlayerEncounter.EncounteredMobileParty == null || PlayerEncounter.EncounteredMobileParty.Ai.DoNotAttackMainPartyUntil.IsPast) && PlayerEncounter.EncounteredParty.Owner != null && FactionManager.IsAtWarAgainstFaction(PlayerEncounter.EncounteredParty.MapFaction, Hero.MainHero.MapFaction))
			{
				flag4 = true;
			}
			if (Campaign.Current.CurrentConversationContext == ConversationContext.CapturedLord && character.IsHero && character.HeroObject.MapFaction == PlayerEncounter.EncounteredParty.MapFaction)
			{
				return text16;
			}
			if (character.HeroObject != null)
			{
				int relation = character.HeroObject.GetRelation(Hero.MainHero);
				if (character.HeroObject != null && character.GetPersona() == DefaultTraits.PersonaIronic)
				{
					if (relation > 4)
					{
						return text4;
					}
					if (relation < -10)
					{
						return text5;
					}
					if (character.Occupation == Occupation.GangLeader && character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) < 0)
					{
						return text10;
					}
					if (character.Occupation == Occupation.GangLeader && flag3)
					{
						return text10;
					}
					Clan clan = character.HeroObject.Clan;
					if (clan == null || !clan.IsNoble)
					{
						return text3;
					}
					if (character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) + character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) < 0)
					{
						return text13;
					}
					return text2;
				}
				else if (character.HeroObject != null && character.GetPersona() == DefaultTraits.PersonaCurt)
				{
					if (relation > 4)
					{
						return text7;
					}
					if (relation < -20)
					{
						return text4;
					}
					if (character.Occupation == Occupation.GangLeader && flag3)
					{
						return text19;
					}
					if (flag2)
					{
						return text15;
					}
					return text14;
				}
				else if (character.HeroObject != null && character.GetPersona() == DefaultTraits.PersonaSoftspoken)
				{
					if (relation > 4)
					{
						return text7;
					}
					if (relation < -20)
					{
						return text20;
					}
					Clan clan2 = character.HeroObject.Clan;
					if ((clan2 == null || !clan2.IsNoble) && flag3 && !character.IsFemale && flag2)
					{
						return text6;
					}
					Clan clan3 = character.HeroObject.Clan;
					if (clan3 != null && clan3.IsNoble && flag3 && !character.IsFemale && flag2)
					{
						return text12;
					}
					if (flag)
					{
						return text8;
					}
					return text11;
				}
				else if (character.HeroObject != null && character.GetPersona() == DefaultTraits.PersonaEarnest)
				{
					if (relation > 4)
					{
						return text7;
					}
					if (relation < -40)
					{
						return text17;
					}
					if (relation < -20)
					{
						return text16;
					}
					Clan clan4 = character.HeroObject.Clan;
					if (clan4 != null && clan4.IsNoble && flag2)
					{
						return text10;
					}
					if (flag)
					{
						return text8;
					}
					return text;
				}
			}
			else if (character.Occupation == Occupation.Villager || character.Occupation == Occupation.Townsfolk)
			{
				int deterministicHashCode = character.StringId.GetDeterministicHashCode();
				if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.Town != null && Settlement.CurrentSettlement.Town.Prosperity < (float)(200 * (Settlement.CurrentSettlement.IsTown ? 5 : 1)) && deterministicHashCode % 2 == 0)
				{
					return text18;
				}
				if (deterministicHashCode % 2 == 1)
				{
					return text9;
				}
			}
			else if (flag4 && character.Occupation == Occupation.Bandit)
			{
				return text16;
			}
			return text;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000A0FC File Offset: 0x000082FC
		private static int GetSuperiorityState(CharacterObject character)
		{
			if (Hero.MainHero.MapFaction != null && Hero.MainHero.MapFaction.Leader == Hero.MainHero && character.HeroObject.MapFaction == Hero.MainHero.MapFaction)
			{
				return -1;
			}
			if (character.IsHero && character.HeroObject.MapFaction != null && character.HeroObject.MapFaction.IsKingdomFaction)
			{
				Clan clan = character.HeroObject.Clan;
				if (clan != null && clan.IsNoble)
				{
					return 1;
				}
			}
			if (character.Occupation == Occupation.Villager || character.Occupation == Occupation.Townsfolk || character.Occupation == Occupation.Bandit || character.Occupation == Occupation.Gangster || character.Occupation == Occupation.Wanderer)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000A1BC File Offset: 0x000083BC
		private static bool MorePowerThanPlayer(CharacterObject otherCharacter)
		{
			float num;
			if (otherCharacter.HeroObject.PartyBelongedTo != null)
			{
				num = otherCharacter.HeroObject.PartyBelongedTo.Party.CalculateCurrentStrength();
			}
			else
			{
				num = otherCharacter.HeroObject.Power;
			}
			float num2 = MobileParty.MainParty.Party.CalculateCurrentStrength();
			return num > num2;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000A214 File Offset: 0x00008414
		public static CharacterObject FindUpgradeRootOf(CharacterObject character)
		{
			foreach (CharacterObject characterObject in CharacterObject.All)
			{
				if (characterObject.IsBasicTroop && CharacterHelper.UpgradeTreeContains(characterObject, characterObject, character))
				{
					return characterObject;
				}
			}
			return character;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000A278 File Offset: 0x00008478
		private static bool UpgradeTreeContains(CharacterObject rootTroop, CharacterObject baseTroop, CharacterObject character)
		{
			if (baseTroop == character)
			{
				return true;
			}
			for (int i = 0; i < baseTroop.UpgradeTargets.Length; i++)
			{
				if (baseTroop.UpgradeTargets[i] == rootTroop)
				{
					return false;
				}
				if (CharacterHelper.UpgradeTreeContains(rootTroop, baseTroop.UpgradeTargets[i], character))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000A2C0 File Offset: 0x000084C0
		public static ItemObject GetDefaultWeapon(CharacterObject affectorCharacter)
		{
			for (int i = 0; i <= 4; i++)
			{
				EquipmentElement equipmentFromSlot = affectorCharacter.Equipment.GetEquipmentFromSlot((EquipmentIndex)i);
				ItemObject item = equipmentFromSlot.Item;
				if (((item != null) ? item.PrimaryWeapon : null) != null && equipmentFromSlot.Item.PrimaryWeapon.WeaponFlags.HasAnyFlag(WeaponFlags.WeaponMask))
				{
					return equipmentFromSlot.Item;
				}
			}
			return null;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000A320 File Offset: 0x00008520
		public static bool CanUseItem(BasicCharacterObject currentCharacter, EquipmentElement itemRosterElement)
		{
			TextObject textObject;
			return CharacterHelper.CanUseItem(currentCharacter, itemRosterElement, out textObject);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000A338 File Offset: 0x00008538
		public static bool CanUseItem(BasicCharacterObject currentCharacter, EquipmentElement itemRosterElement, out TextObject reason)
		{
			bool flag = true;
			reason = null;
			ItemObject item = itemRosterElement.Item;
			SkillObject relevantSkill = item.RelevantSkill;
			if (relevantSkill != null && currentCharacter.GetSkillValue(relevantSkill) < item.Difficulty)
			{
				reason = new TextObject("{=rgqA29b8}You don't have enough {SKILL_NAME} skill to equip this item", null);
				reason.SetTextVariable("SKILL_NAME", itemRosterElement.Item.RelevantSkill.Name);
				flag = false;
			}
			int num = (((!currentCharacter.IsFemale || !item.ItemFlags.HasAnyFlag(ItemFlags.NotUsableByFemale)) && (currentCharacter.IsFemale || !item.ItemFlags.HasAnyFlag(ItemFlags.NotUsableByMale))) ? 1 : 0);
			bool flag2 = item.StringId.Equals("dragon_banner_center") || item.StringId.Equals("dragon_banner_dragonhead") || item.StringId.Equals("dragon_banner_handle");
			if (num == 0 || flag2 || (itemRosterElement.Item.HasHorseComponent && !itemRosterElement.Item.HorseComponent.IsRideable))
			{
				reason = new TextObject("{=ITKb4cKv}{ITEM_NAME} is not equippable.", null);
				reason.SetTextVariable("ITEM_NAME", itemRosterElement.GetModifiedItemName());
				flag = false;
			}
			return flag;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000A458 File Offset: 0x00008658
		public static int GetPartyMemberFaceSeed(PartyBase party, BasicCharacterObject character, int rank)
		{
			int num = party.Index * 171 + character.StringId.GetDeterministicHashCode() * 6791 + rank * 197;
			return ((num >= 0) ? num : (-num)) % 2000;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000A49B File Offset: 0x0000869B
		public static int GetDefaultFaceSeed(BasicCharacterObject character, int rank)
		{
			return character.GetDefaultFaceSeed(rank);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000A4A4 File Offset: 0x000086A4
		public static bool SearchForFormationInTroopTree(CharacterObject baseTroop, FormationClass formation)
		{
			if (baseTroop.UpgradeTargets.Length == 0 && baseTroop.DefaultFormationClass == formation)
			{
				return true;
			}
			foreach (CharacterObject characterObject in baseTroop.UpgradeTargets)
			{
				if (characterObject.Level > baseTroop.Level && CharacterHelper.SearchForFormationInTroopTree(characterObject, formation))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000A4F8 File Offset: 0x000086F8
		public static IEnumerable<CharacterObject> GetTroopTree(CharacterObject baseTroop, float minTier = -1f, float maxTier = 3.4028235E+38f)
		{
			MBQueue<CharacterObject> queue = new MBQueue<CharacterObject>();
			queue.Enqueue(baseTroop);
			while (queue.Count > 0)
			{
				CharacterObject character = queue.Dequeue();
				if ((float)character.Tier >= minTier && (float)character.Tier <= maxTier)
				{
					yield return character;
				}
				foreach (CharacterObject characterObject in character.UpgradeTargets)
				{
					queue.Enqueue(characterObject);
				}
				character = null;
			}
			yield break;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000A518 File Offset: 0x00008718
		public static void DeleteQuestCharacter(CharacterObject character, Settlement questSettlement)
		{
			if (questSettlement != null)
			{
				IList<LocationCharacter> listOfCharacters = questSettlement.LocationComplex.GetListOfCharacters();
				if (listOfCharacters.Any<LocationCharacter>((LocationCharacter x) => x.Character == character))
				{
					LocationCharacter locationCharacter = listOfCharacters.First<LocationCharacter>((LocationCharacter x) => x.Character == character);
					questSettlement.LocationComplex.RemoveCharacterIfExists(locationCharacter);
				}
			}
			Game.Current.ObjectManager.UnregisterObject(character);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000A58C File Offset: 0x0000878C
		public static CharacterObject GetRandomCompanionTemplateWithPredicate(Func<CharacterObject, bool> predicate = null)
		{
			if (predicate == null)
			{
				return MBObjectManager.Instance.GetObjectTypeList<CharacterObject>().GetRandomElementWithPredicate<CharacterObject>((CharacterObject x) => x.IsTemplate && x.Occupation == Occupation.Wanderer);
			}
			return MBObjectManager.Instance.GetObjectTypeList<CharacterObject>().GetRandomElementWithPredicate<CharacterObject>((CharacterObject x) => x.IsTemplate && x.Occupation == Occupation.Wanderer && predicate(x));
		}
	}
}
