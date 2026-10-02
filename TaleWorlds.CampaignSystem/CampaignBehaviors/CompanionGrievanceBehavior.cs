using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003E0 RID: 992
	public class CompanionGrievanceBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003CFB RID: 15611 RVA: 0x00106D40 File Offset: 0x00104F40
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.OnHourlyTick));
			CampaignEvents.VillageLooted.AddNonSerializedListener(this, new Action<Village>(this.OnVillageRaided));
			CampaignEvents.PlayerDesertedBattleEvent.AddNonSerializedListener(this, new Action<int>(this.OnPlayerDesertedBattle));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.OnDailyTick));
		}

		// Token: 0x06003CFC RID: 15612 RVA: 0x00106DC0 File Offset: 0x00104FC0
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<Hero, CompanionGrievanceBehavior.Grievance>>("_heroGrievances", ref this._heroGrievances);
			dataStore.SyncData<CompanionGrievanceBehavior.Grievance>("_currentGrievance", ref this._currentGrievance);
			dataStore.SyncData<CampaignTime[]>("_nextGrievableTimeForComplaintType", ref this._nextGrievableTimeForComplaintType);
		}

		// Token: 0x06003CFD RID: 15613 RVA: 0x00106DF8 File Offset: 0x00104FF8
		private void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("companion_start_grievance", "start", "grievance_received", "{=PVyZ9vNB}{TITLE}, there is something I wish to discuss with you.", new ConversationSentence.OnConditionDelegate(this.companion_start_grievance_condition), null, 120, null);
			campaignGameStarter.AddPlayerLine("grievance_requested", "grievance_received", "grievance_noticed", "{=m72wpzG2}Go on, I'm listening.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLineWithVariation("companion_continue_grievance_desert_softspoken", "grievance_noticed", "grievance_listened", new ConversationSentence.OnConditionDelegate(this.companion_grievance_desert_battle_condition), null, 100, "", "", "", "", null).Variation(new object[] { "{=7ldEMGn6}I don't like running away from a battle like that.", "DefaultTag", 0 }).Variation(new object[] { "{=EqLQtlca}The way we just ran from the enemy back there... I don't want to get a name for being a coward.", "PersonaSoftspokenTag", 1 })
				.Variation(new object[] { "{=sXwOLNo9}I don't like turning my back on the enemy like that. For me, death in battle is better than dishonor.", "PersonaCurtTag", 1 })
				.Variation(new object[] { "{=TnW3i5ul}We ran back there. I despise running. I prefer to be the wolf, not a rabbit.", "PersonaCurtTag", 1, "KhuzaitTag", 1 })
				.Variation(new object[] { "{=IDJbtGks}We ran back there. It is shameful to turn your back on the enemy.", "PersonaCurtTag", 1, "SturgianTag", 1 })
				.Variation(new object[] { "{=PTj1WtJx}The way we ran back there... It shames me to think of it. Next time, let us fight and die rather than let men call us cowards!", "PersonaEarnestTag", 1 })
				.Variation(new object[] { "{=UTRvswWE}As I recall, when you first hired me, it was to fight, not run away. Now, I'm sure what you did back there was sensible, but still, I've earned a bit of a reputation for bravery and I don't care to be called a coward. Those foes we can't beat - let's try to stay a little more clear of them next time, shall we?", "PersonaIronicTag", 1 })
				.Variation(new object[] { "{=v7OCHday}I suppose back there we had to run away to fight another day, as the hero Cathalac once did. If you remember the story, though, for the next three years he sat by himself in a bog, unable to look anyone else in the eye. So let's not do that too often, shall we?", "PersonaIronicTag", 1, "BattanianTag", 1 })
				.Variation(new object[] { "{=u9tAQLUf}We ran away back there. I hope word does not get around. Not looking forward to seeing the snickers and grins on people's faces the next time we walk into a tavern. Stings worse than arrows, that does.", "PersonaIronicTag", 1, "VlandianTag", 1 })
				.Variation(new object[] { "{=gfoqoGTn}The way we ran away back there... I may have told you that I hoped one day the poets would write odes about me. I had intended that they would praise my heroism, not my ability to scamper to safety.", "PersonaIronicTag", 1, "AseraiTag", 1 });
			campaignGameStarter.AddDialogLineWithVariation("companion_continue_grievance_wage_softspoken", "grievance_noticed", "grievance_listened", new ConversationSentence.OnConditionDelegate(this.companion_grievance_wage_condition), null, 100, "", "", "", "", null).Variation(new object[] { "{=yuqLzmL9}I should remind you that I expect to be paid as you had promised.", "DefaultTag", 1 }).Variation(new object[] { "{=zBfB5vw8}I hope you don't mind me saying this, but... Your men shed their blood for you. It bothers me to hear that their wages are late.", "PersonaSoftspokenTag", 1 })
				.Variation(new object[] { "{=Bhtt6XPv}Your men's wages are late. That's not the kind of company I like to fight in.", "PersonaCurtTag", 1 })
				.Variation(new object[] { "{=0mVwnCES}I must say something. Your men say their wages are late. We should take care that this doesn't happen.", "PersonaEarnestTag", 1 })
				.Variation(new object[] { "{=TzqxgiQl}The men say their wages are late. Best uphold your end of the bargain with them, or they might not keep theirs. It would especially be tricky if they chose to void their contract during a battle, wouldn't you say?", "PersonaIronicTag", 1 });
			campaignGameStarter.AddDialogLineWithVariation("companion_continue_grievance_starve_softspoken", "grievance_noticed", "grievance_listened", new ConversationSentence.OnConditionDelegate(this.companion_grievance_starve_condition), null, 100, "", "", "", "", null).Variation(new object[] { "{=IPLyqdVX}I hear we're running low on food. We should watch our stocks better.", "DefaultTag", 1 }).Variation(new object[] { "{=ITboR6C1}The men say we're running low on food. We should be more careful of that.", "PersonaSoftspokenTag", 1 })
				.Variation(new object[] { "{=HkjaCc44}Your men say there's little to eat. They march, they fight. They deserve to eat.", "PersonaCurtTag", 1 })
				.Variation(new object[] { "{=acOOsQaC}The food's running out. That's not fair to the men. We should take care that the food doesn't run out.", "PersonaEarnestTag", 1 })
				.Variation(new object[] { "{=6UKdUrPs}The men say the food's running out. We expect them to die for us if needed. Least we can do is let them die on a full belly.", "PersonaIronicTag", 1 })
				.Variation(new object[] { "{=gChji1JO}About the food... These men are ready to spill their blood for you, but there won't be much blood in their veins to shed if their bellies are empty.", "PersonaIronicTag", 1, "BattanianTag", 1 })
				.Variation(new object[] { "{=IgGQUms4}About our food situation... The general Aricaros used to say that an army marches on its stomach. Can't get far on an empty one.", "PersonaIronicTag", 1, "EmpireTag", 1 })
				.Variation(new object[] { "{=ZNaQrIaP}About our food situation... We shouldn't let the men go hungry. A man's courage comes from his stomach, they say.", "PersonaIronicTag", 1, "PersonaIronicTag", 1 });
			campaignGameStarter.AddDialogLineWithVariation("companion_continue_grievance_raid_softspoken", "grievance_noticed", "grievance_listened", new ConversationSentence.OnConditionDelegate(this.companion_grievance_raid_condition), null, 100, "", "", "", "", null).Variation(new object[] { "{=zNvjSFaC}Pillaging villages is not what I signed up for.", "DefaultTag", 1 }).Variation(new object[] { "{=bpXgcBCp}What we did to that village... I don't like it. Those farmers, they're a lot like my people. I want to know it won't happen again.", "PersonaSoftspokenTag", 1 })
				.Variation(new object[] { "{=4bkLDxIU}What we did back there, to that village... I don't do that. I want no part of it.", "PersonaCurtTag", 1 })
				.Variation(new object[] { "{=VldAzBo5}I need to say something. What we did to that village - it was wrong. They're innocent farmers and they shouldn't have their homes and fields ransacked and burned like that. I won't do that again.", "PersonaEarnestTag", 1 })
				.Variation(new object[] { "{=pDa7kOja}I know war is cruel, but I don't want to make it crueler than necessary. I'd rather not have the blood of innocents on my conscience, if you don't mind. Let's not raid villages like that.", "PersonaIronicTag", 1 });
			campaignGameStarter.AddPlayerLine("grievance_1", "grievance_listened", "close_window", "{=OVeSBrhv}Very well, I will consider this when taking such actions.", null, new ConversationSentence.OnConsequenceDelegate(this.companion_grievance_accepted_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("grievance_2", "grievance_listened", "close_window", "{=2wmKs6Is}As your leader I am able to decide the best course of action.", null, new ConversationSentence.OnConsequenceDelegate(this.companion_grievance_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("grievance_3", "grievance_listened", "close_window", "{=fzKFQuFT}Perhaps you are not suitable for this party after all.", null, new ConversationSentence.OnConsequenceDelegate(this.companion_grievance_rejected_consequence), 100, null, null);
			campaignGameStarter.AddDialogLine("companion_repeat_grievance", "start", "grievance_repeated", "{=baeO5Zkk}{TITLE}... {GRIEVANCE_SHORT_DESCRIPTION}", new ConversationSentence.OnConditionDelegate(this.companion_repeat_grievance_condition), null, 120, null);
			campaignGameStarter.AddDialogLine("companion_grievance_repetition_desert", "grievance_repeated", "close_window", "{=!}{GRIEVANCE_REPETITION}", new ConversationSentence.OnConditionDelegate(this.companion_grievance_desert_battle_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_grievance_consequence), 100, null);
			campaignGameStarter.AddDialogLine("companion_grievance_repetition_wage", "grievance_repeated", "close_window", "{=!}{GRIEVANCE_REPETITION}", new ConversationSentence.OnConditionDelegate(this.companion_grievance_wage_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_grievance_consequence), 100, null);
			campaignGameStarter.AddDialogLine("companion_grievance_repetition_starve", "grievance_repeated", "close_window", "{=!}{GRIEVANCE_REPETITION}", new ConversationSentence.OnConditionDelegate(this.companion_grievance_starve_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_grievance_consequence), 100, null);
			campaignGameStarter.AddDialogLine("companion_grievance_repetition_raid", "grievance_repeated", "close_window", "{=!}{GRIEVANCE_REPETITION}", new ConversationSentence.OnConditionDelegate(this.companion_grievance_raid_condition), new ConversationSentence.OnConsequenceDelegate(this.companion_grievance_consequence), 100, null);
		}

		// Token: 0x06003CFE RID: 15614 RVA: 0x00107538 File Offset: 0x00105738
		private bool companion_grievance_raid_condition()
		{
			return this._currentGrievance != null && this._currentGrievance.TypeOfGrievance == CompanionGrievanceBehavior.GrievanceType.VillageRaided;
		}

		// Token: 0x06003CFF RID: 15615 RVA: 0x00107552 File Offset: 0x00105752
		private bool companion_grievance_starve_condition()
		{
			return this._currentGrievance != null && this._currentGrievance.TypeOfGrievance == CompanionGrievanceBehavior.GrievanceType.Starvation;
		}

		// Token: 0x06003D00 RID: 15616 RVA: 0x0010756C File Offset: 0x0010576C
		private bool companion_grievance_desert_battle_condition()
		{
			return this._currentGrievance != null && this._currentGrievance.TypeOfGrievance == CompanionGrievanceBehavior.GrievanceType.DesertedBattle;
		}

		// Token: 0x06003D01 RID: 15617 RVA: 0x00107586 File Offset: 0x00105786
		private bool companion_grievance_wage_condition()
		{
			return this._currentGrievance != null && this._currentGrievance.TypeOfGrievance == CompanionGrievanceBehavior.GrievanceType.NoWage;
		}

		// Token: 0x06003D02 RID: 15618 RVA: 0x001075A0 File Offset: 0x001057A0
		private bool companion_start_grievance_condition()
		{
			MBTextManager.SetTextVariable("TITLE", Hero.MainHero.IsFemale ? GameTexts.FindText("str_my_lady", null) : GameTexts.FindText("str_my_lord", null), false);
			return this._currentGrievance != null && Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero == this._currentGrievance.GrievingHero && this._currentGrievance.Count <= 1 && !this._currentGrievance.HasBeenSettled;
		}

		// Token: 0x06003D03 RID: 15619 RVA: 0x0010761C File Offset: 0x0010581C
		private bool companion_repeat_grievance_condition()
		{
			if (this._currentGrievance != null && Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero == this._currentGrievance.GrievingHero)
			{
				MBTextManager.SetTextVariable("TITLE", ConversationHelper.HeroRefersToHero(Hero.OneToOneConversationHero, Hero.MainHero, true), false);
				MBTextManager.SetTextVariable("GRIEVANCE_SHORT_DESCRIPTION", "{=scJ2eVhS}What I said to you before... [default, should not appear]", false);
				if (this._currentGrievance.TypeOfGrievance == CompanionGrievanceBehavior.GrievanceType.DesertedBattle)
				{
					MBTextManager.SetTextVariable("GRIEVANCE_SHORT_DESCRIPTION", "{=1G5M9nn2}What I mentioned about running from battle...", false);
				}
				else if (this._currentGrievance.TypeOfGrievance == CompanionGrievanceBehavior.GrievanceType.NoWage)
				{
					MBTextManager.SetTextVariable("GRIEVANCE_SHORT_DESCRIPTION", "{=p78FaTqe}What I said about our wages being paid on time...", false);
				}
				else if (this._currentGrievance.TypeOfGrievance == CompanionGrievanceBehavior.GrievanceType.Starvation)
				{
					MBTextManager.SetTextVariable("GRIEVANCE_SHORT_DESCRIPTION", "{=zfPQlDbQ}What I said about our food...", false);
				}
				else if (this._currentGrievance.TypeOfGrievance == CompanionGrievanceBehavior.GrievanceType.VillageRaided)
				{
					MBTextManager.SetTextVariable("GRIEVANCE_SHORT_DESCRIPTION", "{=pQmUIjOQ}What I said about raiding villagers...", false);
				}
				MBTextManager.SetTextVariable("GRIEVANCE_REPETITION", "{=qNSOb7pJ}Once again, this is not something I'm happy with...", false);
				if (Hero.OneToOneConversationHero.CharacterObject.GetPersona() == DefaultTraits.PersonaEarnest)
				{
					MBTextManager.SetTextVariable("GRIEVANCE_REPETITION", "{=YWu5Xfgz}I don't feel you're taking my complaint seriously.", false);
				}
				else if (Hero.OneToOneConversationHero.CharacterObject.GetPersona() == DefaultTraits.PersonaIronic)
				{
					MBTextManager.SetTextVariable("GRIEVANCE_REPETITION", "{=wScKLt7F}Let me put things this way... It's not grown on me at all since the last time it happened.", false);
				}
				else if (Hero.OneToOneConversationHero.CharacterObject.GetPersona() == DefaultTraits.PersonaCurt)
				{
					MBTextManager.SetTextVariable("GRIEVANCE_REPETITION", "{=dpzbyUCa}I don't care for it any more than I did before.", false);
				}
				return Hero.OneToOneConversationHero == this._currentGrievance.GrievingHero && this._currentGrievance.Count > 1;
			}
			return false;
		}

		// Token: 0x06003D04 RID: 15620 RVA: 0x001077A8 File Offset: 0x001059A8
		private void companion_grievance_accepted_consequence()
		{
			CompanionGrievanceBehavior.Grievance value = this._heroGrievances.FirstOrDefault<KeyValuePair<Hero, CompanionGrievanceBehavior.Grievance>>((KeyValuePair<Hero, CompanionGrievanceBehavior.Grievance> t) => t.Value == this._currentGrievance && t.Key == this._currentGrievance.GrievingHero).Value;
			if (value != null)
			{
				value.HasBeenSettled = true;
				if (value.Count <= 1)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, this._currentGrievance.GrievingHero, 10, true);
				}
			}
			this._currentGrievance = null;
		}

		// Token: 0x06003D05 RID: 15621 RVA: 0x00107808 File Offset: 0x00105A08
		private void companion_grievance_consequence()
		{
			if (this._currentGrievance.Count > 1)
			{
				int num = (this._currentGrievance.HasBeenSettled ? (-5) : (-2));
				ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, this._currentGrievance.GrievingHero, num, true);
			}
			this._currentGrievance = null;
		}

		// Token: 0x06003D06 RID: 15622 RVA: 0x00107855 File Offset: 0x00105A55
		private void companion_grievance_rejected_consequence()
		{
			ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, this._currentGrievance.GrievingHero, -15, true);
			this._currentGrievance = null;
		}

		// Token: 0x06003D07 RID: 15623 RVA: 0x00107876 File Offset: 0x00105A76
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x06003D08 RID: 15624 RVA: 0x00107880 File Offset: 0x00105A80
		private void OnHourlyTick()
		{
			if (!MobileParty.MainParty.IsCurrentlyAtSea && GameStateManager.Current.ActiveState is MapState && MobileParty.MainParty.MapEvent == null && PlayerEncounter.Current == null)
			{
				foreach (KeyValuePair<Hero, CompanionGrievanceBehavior.Grievance> keyValuePair in this._heroGrievances)
				{
					CompanionGrievanceBehavior.Grievance value = keyValuePair.Value;
					if (value.GrievingHero.PartyBelongedTo == MobileParty.MainParty && value.HaveGrievance)
					{
						this._currentGrievance = value;
						ConversationCharacterData conversationCharacterData = new ConversationCharacterData(Hero.MainHero.CharacterObject, MobileParty.MainParty.Party, false, false, false, false, false, false);
						ConversationCharacterData conversationCharacterData2 = new ConversationCharacterData(value.GrievingHero.CharacterObject, null, false, false, false, false, false, false);
						if (PartyBase.MainParty.MobileParty.IsCurrentlyAtSea)
						{
							conversationCharacterData2.Party = MobileParty.MainParty.Party;
							CampaignMission.OpenConversationMission(conversationCharacterData, conversationCharacterData2, "", "", false);
						}
						else
						{
							CampaignMapConversation.OpenConversation(conversationCharacterData, conversationCharacterData2);
						}
						value.HaveGrievance = false;
						value.NextGrievanceTime = CampaignTime.DaysFromNow((float)(4 + MBRandom.RandomInt(4)));
						break;
					}
				}
			}
		}

		// Token: 0x06003D09 RID: 15625 RVA: 0x001079D4 File Offset: 0x00105BD4
		private void DecideCompanionGrievances(CompanionGrievanceBehavior.GrievanceType eventType)
		{
			if (this._nextGrievableTimeForComplaintType[(int)eventType].IsFuture || MobileParty.MainParty.IsCurrentlyAtSea)
			{
				return;
			}
			foreach (Hero hero in Hero.MainHero.CompanionsInParty)
			{
				CompanionGrievanceBehavior.Grievance grievance;
				this._heroGrievances.TryGetValue(hero, out grievance);
				if (grievance == null)
				{
					CompanionGrievanceBehavior.GrievanceType grievanceTypeForCompanion = this.GetGrievanceTypeForCompanion(hero, eventType);
					if (grievanceTypeForCompanion != CompanionGrievanceBehavior.GrievanceType.Invalid)
					{
						grievance = new CompanionGrievanceBehavior.Grievance(hero, CampaignTime.Now, grievanceTypeForCompanion);
						this._heroGrievances.Add(hero, grievance);
						this._nextGrievableTimeForComplaintType[(int)eventType] = CampaignTime.DaysFromNow(4f);
						break;
					}
				}
				if (grievance != null && grievance.TypeOfGrievance == eventType && !grievance.HaveGrievance && grievance.NextGrievanceTime.IsPast)
				{
					grievance.HaveGrievance = true;
					this._nextGrievableTimeForComplaintType[(int)eventType] = CampaignTime.DaysFromNow(4f);
					grievance.Count++;
					break;
				}
			}
		}

		// Token: 0x06003D0A RID: 15626 RVA: 0x00107AE4 File Offset: 0x00105CE4
		private CompanionGrievanceBehavior.GrievanceType GetGrievanceTypeForCompanion(Hero companionHero, CompanionGrievanceBehavior.GrievanceType type)
		{
			if ((type == CompanionGrievanceBehavior.GrievanceType.DesertedBattle && companionHero.GetTraitLevel(DefaultTraits.Valor) > 0) || (type == CompanionGrievanceBehavior.GrievanceType.Starvation && companionHero.GetTraitLevel(DefaultTraits.Generosity) > 0) || (type == CompanionGrievanceBehavior.GrievanceType.NoWage && companionHero.GetTraitLevel(DefaultTraits.Generosity) > 0) || (type == CompanionGrievanceBehavior.GrievanceType.VillageRaided && companionHero.GetTraitLevel(DefaultTraits.Mercy) > 0))
			{
				return type;
			}
			return CompanionGrievanceBehavior.GrievanceType.Invalid;
		}

		// Token: 0x06003D0B RID: 15627 RVA: 0x00107B3C File Offset: 0x00105D3C
		private void OnPlayerDesertedBattle(int sacrificedMenCount)
		{
			this.DecideCompanionGrievances(CompanionGrievanceBehavior.GrievanceType.DesertedBattle);
		}

		// Token: 0x06003D0C RID: 15628 RVA: 0x00107B48 File Offset: 0x00105D48
		private void OnVillageRaided(Village village)
		{
			PartyBase party = village.Settlement.Party;
			MapEvent mapEvent = ((party != null) ? party.MapEvent : null);
			if (mapEvent != null)
			{
				using (IEnumerator<PartyBase> enumerator = mapEvent.InvolvedParties.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == PartyBase.MainParty)
						{
							this.DecideCompanionGrievances(CompanionGrievanceBehavior.GrievanceType.VillageRaided);
						}
					}
				}
			}
		}

		// Token: 0x06003D0D RID: 15629 RVA: 0x00107BB8 File Offset: 0x00105DB8
		private void OnDailyTick()
		{
			if (PartyBase.MainParty.IsStarving)
			{
				this.DecideCompanionGrievances(CompanionGrievanceBehavior.GrievanceType.Starvation);
			}
			if (MobileParty.MainParty.HasUnpaidWages > 0f)
			{
				this.DecideCompanionGrievances(CompanionGrievanceBehavior.GrievanceType.NoWage);
			}
			foreach (KeyValuePair<Hero, CompanionGrievanceBehavior.Grievance> keyValuePair in this._heroGrievances)
			{
				CompanionGrievanceBehavior.Grievance value = keyValuePair.Value;
				if (value.NextGrievanceTime.ElapsedDaysUntilNow >= 56f)
				{
					value.HasBeenSettled = false;
					value.Count = 0;
				}
			}
		}

		// Token: 0x04001288 RID: 4744
		private Dictionary<Hero, CompanionGrievanceBehavior.Grievance> _heroGrievances = new Dictionary<Hero, CompanionGrievanceBehavior.Grievance>();

		// Token: 0x04001289 RID: 4745
		private CampaignTime[] _nextGrievableTimeForComplaintType = new CampaignTime[Enum.GetValues(typeof(CompanionGrievanceBehavior.GrievanceType)).Length];

		// Token: 0x0400128A RID: 4746
		private CompanionGrievanceBehavior.Grievance _currentGrievance;

		// Token: 0x0400128B RID: 4747
		private const int BaseGrievanceFrequencyInDays = 4;

		// Token: 0x0400128C RID: 4748
		private const int GrievanceObsolescenceDurationInDays = 56;

		// Token: 0x0400128D RID: 4749
		private const int GrievanceCooldownForSameTypeInDays = 1;

		// Token: 0x020007D4 RID: 2004
		public class CompanionGrievanceBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x0600639F RID: 25503 RVA: 0x001C2090 File Offset: 0x001C0290
			public CompanionGrievanceBehaviorTypeDefiner()
				: base(80000)
			{
			}

			// Token: 0x060063A0 RID: 25504 RVA: 0x001C209D File Offset: 0x001C029D
			protected override void DefineEnumTypes()
			{
				base.AddEnumDefinition(typeof(CompanionGrievanceBehavior.GrievanceType), 1, null);
			}

			// Token: 0x060063A1 RID: 25505 RVA: 0x001C20B1 File Offset: 0x001C02B1
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(CompanionGrievanceBehavior.Grievance), 10, null);
			}

			// Token: 0x060063A2 RID: 25506 RVA: 0x001C20C6 File Offset: 0x001C02C6
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(Dictionary<Hero, CompanionGrievanceBehavior.Grievance>));
			}
		}

		// Token: 0x020007D5 RID: 2005
		internal class Grievance
		{
			// Token: 0x17001540 RID: 5440
			// (get) Token: 0x060063A3 RID: 25507 RVA: 0x001C20D8 File Offset: 0x001C02D8
			// (set) Token: 0x060063A4 RID: 25508 RVA: 0x001C20E0 File Offset: 0x001C02E0
			[SaveableProperty(4)]
			public bool HaveGrievance { get; set; }

			// Token: 0x060063A5 RID: 25509 RVA: 0x001C20E9 File Offset: 0x001C02E9
			public Grievance(Hero hero, CampaignTime time, CompanionGrievanceBehavior.GrievanceType type)
			{
				this.GrievingHero = hero;
				this.NextGrievanceTime = time;
				this.TypeOfGrievance = type;
				this.HasBeenSettled = false;
				this.Count = 1;
				this.HaveGrievance = true;
			}

			// Token: 0x060063A6 RID: 25510 RVA: 0x001C211B File Offset: 0x001C031B
			internal static void AutoGeneratedStaticCollectObjectsGrievance(object o, List<object> collectedObjects)
			{
				((CompanionGrievanceBehavior.Grievance)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060063A7 RID: 25511 RVA: 0x001C2129 File Offset: 0x001C0329
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.GrievingHero);
				CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this.NextGrievanceTime, collectedObjects);
			}

			// Token: 0x060063A8 RID: 25512 RVA: 0x001C2148 File Offset: 0x001C0348
			internal static object AutoGeneratedGetMemberValueHaveGrievance(object o)
			{
				return ((CompanionGrievanceBehavior.Grievance)o).HaveGrievance;
			}

			// Token: 0x060063A9 RID: 25513 RVA: 0x001C215A File Offset: 0x001C035A
			internal static object AutoGeneratedGetMemberValueGrievingHero(object o)
			{
				return ((CompanionGrievanceBehavior.Grievance)o).GrievingHero;
			}

			// Token: 0x060063AA RID: 25514 RVA: 0x001C2167 File Offset: 0x001C0367
			internal static object AutoGeneratedGetMemberValueNextGrievanceTime(object o)
			{
				return ((CompanionGrievanceBehavior.Grievance)o).NextGrievanceTime;
			}

			// Token: 0x060063AB RID: 25515 RVA: 0x001C2179 File Offset: 0x001C0379
			internal static object AutoGeneratedGetMemberValueTypeOfGrievance(object o)
			{
				return ((CompanionGrievanceBehavior.Grievance)o).TypeOfGrievance;
			}

			// Token: 0x060063AC RID: 25516 RVA: 0x001C218B File Offset: 0x001C038B
			internal static object AutoGeneratedGetMemberValueHasBeenSettled(object o)
			{
				return ((CompanionGrievanceBehavior.Grievance)o).HasBeenSettled;
			}

			// Token: 0x060063AD RID: 25517 RVA: 0x001C219D File Offset: 0x001C039D
			internal static object AutoGeneratedGetMemberValueCount(object o)
			{
				return ((CompanionGrievanceBehavior.Grievance)o).Count;
			}

			// Token: 0x04001F8F RID: 8079
			[SaveableField(1)]
			public Hero GrievingHero;

			// Token: 0x04001F90 RID: 8080
			[SaveableField(2)]
			public CampaignTime NextGrievanceTime;

			// Token: 0x04001F91 RID: 8081
			[SaveableField(3)]
			public CompanionGrievanceBehavior.GrievanceType TypeOfGrievance;

			// Token: 0x04001F93 RID: 8083
			[SaveableField(5)]
			public bool HasBeenSettled;

			// Token: 0x04001F94 RID: 8084
			[SaveableField(6)]
			public int Count;
		}

		// Token: 0x020007D6 RID: 2006
		internal enum GrievanceType
		{
			// Token: 0x04001F96 RID: 8086
			Invalid,
			// Token: 0x04001F97 RID: 8087
			NoWage,
			// Token: 0x04001F98 RID: 8088
			Starvation,
			// Token: 0x04001F99 RID: 8089
			VillageRaided,
			// Token: 0x04001F9A RID: 8090
			DesertedBattle
		}
	}
}
