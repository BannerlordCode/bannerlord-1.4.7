using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StoryMode.Quests.PlayerClanQuests;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace StoryMode.GameComponents.CampaignBehaviors
{
	// Token: 0x02000050 RID: 80
	public class MainStorylineCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060004E8 RID: 1256 RVA: 0x0001B6A8 File Offset: 0x000198A8
		public override void RegisterEvents()
		{
			CampaignEvents.CanHeroDieEvent.AddNonSerializedListener(this, new ReferenceAction<Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.CanHeroDie));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
			CampaignEvents.HeroComesOfAgeEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroComesOfAge));
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001B711 File Offset: 0x00019911
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001B713 File Offset: 0x00019913
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (clan == Clan.PlayerClan && newKingdom != null && (detail == ChangeKingdomAction.ChangeKingdomActionDetail.CreateKingdom || detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinKingdom))
			{
				Clan.PlayerClan.IsNoble = true;
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001B738 File Offset: 0x00019938
		private void CanHeroDie(Hero hero, KillCharacterAction.KillCharacterActionDetail causeOfDeath, ref bool result)
		{
			if ((hero == StoryModeHeroes.Radagos && StoryModeManager.Current.MainStoryLine.TutorialPhase.IsCompleted && !Campaign.Current.QuestManager.IsThereActiveQuestWithType(typeof(RescueFamilyQuestBehavior.RescueFamilyQuest)) && !Campaign.Current.QuestManager.IsThereActiveQuestWithType(typeof(RebuildPlayerClanQuest)) && causeOfDeath == KillCharacterAction.KillCharacterActionDetail.Executed) || causeOfDeath == KillCharacterAction.KillCharacterActionDetail.ExecutionAfterMapEvent)
			{
				result = true;
				return;
			}
			if (hero.IsSpecial && hero != StoryModeHeroes.RadagosHenchman && !StoryModeManager.Current.MainStoryLine.IsCompleted)
			{
				result = false;
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001B7C9 File Offset: 0x000199C9
		private void OnHeroComesOfAge(Hero hero)
		{
			if (hero == StoryModeHeroes.LittleBrother || (hero == StoryModeHeroes.LittleSister && !ModuleHelper.IsModuleActive("NavalDLC")))
			{
				StoryModeHelpers.SetPlayerSiblingsSkillsIfNeeded(hero);
			}
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001B7F0 File Offset: 0x000199F0
		private void OnGameLoadFinished()
		{
			if (MBSaveLoad.IsUpdatingGameVersion)
			{
				if (MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.3.13.105456", 0))
				{
					if (Clan.PlayerClan.Kingdom != null && !Clan.PlayerClan.IsUnderMercenaryService && !Clan.PlayerClan.IsNoble)
					{
						Clan.PlayerClan.IsNoble = true;
					}
					bool flag = StoryModeManager.Current.MainStoryLine.FamilyRescued && !Campaign.Current.QuestManager.IsThereActiveQuestWithType(typeof(RescueFamilyQuestBehavior.RescueFamilyQuest));
					this.HandlePlayerSiblingsStatesOnLoad(StoryModeHeroes.LittleSister, flag);
					this.HandlePlayerSiblingsStatesOnLoad(StoryModeHeroes.LittleBrother, flag);
					if (flag)
					{
						this.CheckStoryModeHeroStateAndUpdateIfNeeded(StoryModeHeroes.ElderBrother);
						MainStorylineCampaignBehavior.CheckAndUpdateGovernorStatusOfStoryModeHero(StoryModeHeroes.ElderBrother);
					}
				}
				if (MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.2.0", 0))
				{
					FirstPhase instance = FirstPhase.Instance;
					if (instance != null && instance.AllPiecesCollected)
					{
						ItemObject @object = Campaign.Current.ObjectManager.GetObject<ItemObject>("dragon_banner");
						bool flag2 = false;
						foreach (ItemRosterElement itemRosterElement in MobileParty.MainParty.ItemRoster)
						{
							if (itemRosterElement.EquipmentElement.Item == @object)
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							FirstPhase firstPhase = StoryModeManager.Current.MainStoryLine.FirstPhase;
							if (firstPhase != null)
							{
								firstPhase.MergeDragonBanner();
							}
						}
					}
				}
				if (MBSaveLoad.LastLoadedGameVersion.IsOlderThan(ApplicationVersion.FromString("v1.2.9.35367", 0)))
				{
					List<EquipmentElement> list = new List<EquipmentElement>();
					foreach (ItemRosterElement itemRosterElement2 in MobileParty.MainParty.ItemRoster)
					{
						ItemObject item = itemRosterElement2.EquipmentElement.Item;
						string text = ((item != null) ? item.StringId : null);
						if (!itemRosterElement2.EquipmentElement.IsQuestItem && (text == "dragon_banner_center" || text == "dragon_banner_dragonhead" || text == "dragon_banner_handle" || text == "dragon_banner"))
						{
							list.Add(itemRosterElement2.EquipmentElement);
						}
					}
					if (list.Any<EquipmentElement>())
					{
						foreach (EquipmentElement equipmentElement in list)
						{
							MobileParty.MainParty.ItemRoster.AddToCounts(equipmentElement, -1);
							MobileParty.MainParty.ItemRoster.AddToCounts(new EquipmentElement(equipmentElement.Item, null, null, true), 1);
						}
					}
				}
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001BAB4 File Offset: 0x00019CB4
		private void HandlePlayerSiblingsStatesOnLoad(Hero hero, bool isPlayerFamilyRescued)
		{
			if (hero.IsAlive && (hero == StoryModeHeroes.LittleBrother || (hero == StoryModeHeroes.LittleSister && !ModuleHelper.IsModuleActive("NavalDLC"))))
			{
				AgingCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<AgingCampaignBehavior>();
				FieldInfo field = typeof(AgingCampaignBehavior).GetField("_heroesYoungerThanHeroComesOfAge", BindingFlags.Instance | BindingFlags.NonPublic);
				Dictionary<Hero, int> dictionary = ((campaignBehavior != null) ? ((Dictionary<Hero, int>)field.GetValue(campaignBehavior)) : null);
				if (hero.Age < (float)Campaign.Current.Models.AgeModel.HeroComesOfAge)
				{
					if (!hero.IsDisabled && !hero.IsNotSpawned)
					{
						if (isPlayerFamilyRescued)
						{
							hero.ChangeState(Hero.CharacterStates.NotSpawned);
						}
						else
						{
							DisableHeroAction.Apply(hero);
						}
					}
					if (!hero.IsDisabled && dictionary != null && !dictionary.ContainsKey(hero))
					{
						dictionary.Add(hero, (int)hero.Age);
						field.SetValue(campaignBehavior, dictionary);
					}
				}
				else if (isPlayerFamilyRescued)
				{
					if (dictionary != null && dictionary.ContainsKey(hero))
					{
						dictionary.Remove(hero);
					}
					this.CheckPlayerSiblingsEducationStages(hero);
					this.CheckStoryModeHeroStateAndUpdateIfNeeded(hero);
					StoryModeHelpers.SetPlayerSiblingsSkillsIfNeeded(hero);
				}
				else if (!hero.IsDisabled)
				{
					DisableHeroAction.Apply(hero);
					if (hero.GovernorOf != null)
					{
						ChangeGovernorAction.RemoveGovernorOf(hero);
					}
				}
				MainStorylineCampaignBehavior.CheckAndUpdateGovernorStatusOfStoryModeHero(hero);
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001BBE0 File Offset: 0x00019DE0
		private void CheckPlayerSiblingsEducationStages(Hero hero)
		{
			EducationCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<EducationCampaignBehavior>();
			if (campaignBehavior != null)
			{
				Type typeFromHandle = typeof(EducationCampaignBehavior);
				if (((Dictionary<Hero, short>)typeFromHandle.GetField("_previousEducations", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(campaignBehavior)).ContainsKey(hero) || !this.IsHeroAttributesInitialized(hero))
				{
					typeFromHandle.GetMethod("OnHeroComesOfAge", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(campaignBehavior, new object[] { hero });
				}
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001BC50 File Offset: 0x00019E50
		private void CheckStoryModeHeroStateAndUpdateIfNeeded(Hero hero)
		{
			if (hero.IsNotSpawned || hero.IsDisabled)
			{
				Settlement settlementToSpawnForPlayerRelative = this.GetSettlementToSpawnForPlayerRelative(hero);
				if (hero.BornSettlement == null)
				{
					hero.BornSettlement = settlementToSpawnForPlayerRelative;
				}
				TeleportHeroAction.ApplyImmediateTeleportToSettlement(hero, settlementToSpawnForPlayerRelative);
				if (!hero.IsActive)
				{
					hero.ChangeState(Hero.CharacterStates.Active);
				}
			}
			if (hero.Clan == null)
			{
				hero.Clan = Clan.PlayerClan;
				if (!hero.IsFugitive)
				{
					MakeHeroFugitiveAction.Apply(hero, false);
				}
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001BCBC File Offset: 0x00019EBC
		private static void CheckAndUpdateGovernorStatusOfStoryModeHero(Hero hero)
		{
			if (hero.GovernorOf != null && hero.CurrentSettlement != hero.GovernorOf.Settlement)
			{
				ChangeGovernorAction.RemoveGovernorOf(hero);
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001BCE0 File Offset: 0x00019EE0
		private bool IsHeroAttributesInitialized(Hero hero)
		{
			foreach (CharacterAttribute characterAttribute in Attributes.All)
			{
				if (hero.GetAttributeValue(characterAttribute) != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001BD3C File Offset: 0x00019F3C
		private Settlement GetSettlementToSpawnForPlayerRelative(Hero hero)
		{
			if (hero.GovernorOf != null)
			{
				return hero.GovernorOf.Settlement;
			}
			if (!hero.HomeSettlement.OwnerClan.IsAtWarWith(Clan.PlayerClan.MapFaction))
			{
				return hero.HomeSettlement;
			}
			if (!Clan.PlayerClan.MapFaction.Settlements.IsEmpty<Settlement>())
			{
				return Clan.PlayerClan.MapFaction.Settlements.GetRandomElement<Settlement>();
			}
			foreach (Settlement settlement in Settlement.All)
			{
				if (!settlement.MapFaction.IsAtWarWith(Clan.PlayerClan.MapFaction))
				{
					return settlement;
				}
			}
			return Village.All.GetRandomElement<Village>().Settlement;
		}
	}
}
