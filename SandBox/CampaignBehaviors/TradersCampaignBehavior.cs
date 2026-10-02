using System;
using Helpers;
using SandBox.Missions.MissionLogics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000E5 RID: 229
	public class TradersCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06000B36 RID: 2870 RVA: 0x0005322A File Offset: 0x0005142A
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00053243 File Offset: 0x00051443
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x00053245 File Offset: 0x00051445
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x00053250 File Offset: 0x00051450
		protected void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("weaponsmith_talk_start_normal", "start", "weaponsmith_talk_player", "{=!}{TRADER_GREETING}", new ConversationSentence.OnConditionDelegate(this.conversation_weaponsmith_talk_start_normal_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("weaponsmith_talk_start_to_player_in_disguise", "start", "close_window", "{=1auLEn9y}Look, my good {?PLAYER.GENDER}woman{?}man{\\?}, these are hard times for sure, but I need you to move along. You'll scare away my customers.", new ConversationSentence.OnConditionDelegate(this.conversation_weaponsmith_talk_start_to_player_in_disguise_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("weaponsmith_talk_initial", "weaponsmith_begin", "weaponsmith_talk_player", "{=jxw54Ijt}Okay, is there anything more I can help with?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("weaponsmith_talk_player_1", "weaponsmith_talk_player", "merchant_response_1", "{=ExltvaKo}Let me see what you have for sale...", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("weaponsmith_talk_player_request_craft", "weaponsmith_talk_player", "merchant_response_crafting", "{=w1vzpCNi}I need you to craft a weapon for me", new ConversationSentence.OnConditionDelegate(this.conversation_open_crafting_on_condition), null, 100, null, null);
			campaignGameStarter.AddPlayerLine("weaponsmith_talk_player_3", "weaponsmith_talk_player", "merchant_response_3", "{=8hNYr2VX}I was just passing by.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("weaponsmith_talk_merchant_response_1", "merchant_response_1", "player_merchant_talk_close", "{=K5mG9nDv}With pleasure.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("weaponsmith_talk_merchant_response_2", "merchant_response_2", "player_merchant_talk_2", "{=5bRQ0gt7}How many men do you need for it? For each men I want 100{GOLD_ICON}.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("weaponsmith_talk_merchant_response_craft", "merchant_response_crafting", "player_merchant_craft_talk_close", "{=lF5HkBDy}As you wish.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("weaponsmith_talk_merchant_craft_opened", "player_merchant_craft_talk_close", "close_window", "{=TD8Jxn7U}Have a nice day my {?PLAYER.GENDER}lady{?}lord{\\?}.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_weaponsmith_craft_on_consequence), 100, null);
			campaignGameStarter.AddDialogLine("weaponsmith_talk_merchant_response_3", "merchant_response_3", "close_window", "{=FpNWdIaT}Yes, of course. Just ask me if there is anything you need.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("weaponsmith_talk_end", "player_merchant_talk_close", "close_window", "{=Yh0danUf}Thank you and good day my {?PLAYER.GENDER}lady{?}lord{\\?}.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_weaponsmith_talk_player_on_consequence), 100, null);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x00053417 File Offset: 0x00051617
		private bool conversation_open_crafting_on_condition()
		{
			return CharacterObject.OneToOneConversationCharacter != null && CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Blacksmith;
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00053430 File Offset: 0x00051630
		private bool conversation_weaponsmith_talk_start_normal_on_condition()
		{
			if (!this.IsTrader())
			{
				return false;
			}
			if (!Campaign.Current.IsMainHeroDisguised)
			{
				MBTextManager.SetTextVariable("TRADER_GREETING", "{=7IxFrati}Greetings my {?PLAYER.GENDER}lady{?}lord{\\?}, how may I help you?", false);
				return true;
			}
			if (Mission.Current.GetMissionBehavior<DisguiseMissionLogic>().ContactAlreadySetCommonCondition() && Hero.MainHero.GetPerkValue(DefaultPerks.Roguery.SmugglerConnections))
			{
				MBTextManager.SetTextVariable("TRADER_GREETING", "{=bqg2gS7i}Ah, a friend of a friend. How may I help you?", false);
				return true;
			}
			return false;
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x0005349A File Offset: 0x0005169A
		private bool conversation_weaponsmith_talk_start_to_player_in_disguise_on_condition()
		{
			return this.IsTrader() && !this.conversation_weaponsmith_talk_start_normal_on_condition();
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x000534AF File Offset: 0x000516AF
		private bool IsTrader()
		{
			return CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Weaponsmith || CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Armorer || CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.HorseTrader || CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.GoodsTrader;
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x000534EC File Offset: 0x000516EC
		private void conversation_weaponsmith_talk_player_on_consequence()
		{
			InventoryScreenHelper.InventoryCategoryType inventoryCategoryType = InventoryScreenHelper.InventoryCategoryType.None;
			Occupation occupation = CharacterObject.OneToOneConversationCharacter.Occupation;
			if (occupation != Occupation.GoodsTrader)
			{
				switch (occupation)
				{
				case Occupation.Weaponsmith:
					inventoryCategoryType = InventoryScreenHelper.InventoryCategoryType.Weapon;
					break;
				case Occupation.Armorer:
					inventoryCategoryType = InventoryScreenHelper.InventoryCategoryType.Armors;
					break;
				case Occupation.HorseTrader:
					inventoryCategoryType = InventoryScreenHelper.InventoryCategoryType.HorseCategory;
					break;
				default:
					if (occupation == Occupation.Blacksmith)
					{
						inventoryCategoryType = InventoryScreenHelper.InventoryCategoryType.Weapon;
					}
					break;
				}
			}
			else
			{
				inventoryCategoryType = InventoryScreenHelper.InventoryCategoryType.Goods;
			}
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (Mission.Current != null)
			{
				InventoryScreenHelper.OpenScreenAsTrade(currentSettlement.ItemRoster, currentSettlement.Town, inventoryCategoryType, new Action(this.OnInventoryScreenDone));
				return;
			}
			InventoryScreenHelper.OpenScreenAsTrade(currentSettlement.ItemRoster, currentSettlement.Town, inventoryCategoryType, null);
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00053577 File Offset: 0x00051777
		private void conversation_weaponsmith_craft_on_consequence()
		{
			CraftingHelper.OpenCrafting(CraftingTemplate.All[0], null);
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0005358C File Offset: 0x0005178C
		private void OnInventoryScreenDone()
		{
			foreach (Agent agent in Mission.Current.Agents)
			{
				CharacterObject characterObject = (CharacterObject)agent.Character;
				if (agent.IsHuman && characterObject != null && characterObject.IsHero && characterObject.HeroObject.PartyBelongedTo == MobileParty.MainParty && (!agent.IsMainAgent || !Campaign.Current.IsMainHeroDisguised))
				{
					agent.UpdateSpawnEquipmentAndRefreshVisuals(Mission.Current.DoesMissionRequireCivilianEquipment ? characterObject.FirstCivilianEquipment : characterObject.FirstBattleEquipment);
				}
			}
		}
	}
}
