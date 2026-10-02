using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000DB RID: 219
	public class HeirSelectionCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06000A37 RID: 2615 RVA: 0x0004D68C File Offset: 0x0004B88C
		public override void RegisterEvents()
		{
			CampaignEvents.OnBeforeMainCharacterDiedEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnBeforeMainCharacterDied));
			CampaignEvents.OnBeforePlayerCharacterChangedEvent.AddNonSerializedListener(this, new Action<Hero, Hero>(this.OnBeforePlayerCharacterChanged));
			CampaignEvents.OnPlayerCharacterChangedEvent.AddNonSerializedListener(this, new Action<Hero, Hero, MobileParty, bool>(this.OnPlayerCharacterChanged));
			CampaignEvents.OnHeirSelectionOverEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeirSelectionOver));
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x0004D6F5 File Offset: 0x0004B8F5
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x0004D6F8 File Offset: 0x0004B8F8
		private void OnBeforePlayerCharacterChanged(Hero oldPlayer, Hero newPlayer)
		{
			foreach (ItemRosterElement itemRosterElement in MobileParty.MainParty.ItemRoster)
			{
				this._itemsThatWillBeInherited.Add(itemRosterElement);
			}
			for (int i = 0; i < 12; i++)
			{
				if (!oldPlayer.BattleEquipment[i].IsEmpty)
				{
					this._equipmentsThatWillBeInherited.AddToCounts(oldPlayer.BattleEquipment[i], 1);
				}
				if (!oldPlayer.CivilianEquipment[i].IsEmpty)
				{
					this._equipmentsThatWillBeInherited.AddToCounts(oldPlayer.CivilianEquipment[i], 1);
				}
			}
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0004D7BC File Offset: 0x0004B9BC
		private void OnPlayerCharacterChanged(Hero oldPlayer, Hero newPlayer, MobileParty newMainParty, bool isMainPartyChanged)
		{
			foreach (Alley alley in oldPlayer.OwnedAlleys.ToList<Alley>())
			{
				alley.SetOwner(newPlayer);
			}
			if (isMainPartyChanged)
			{
				newMainParty.ItemRoster.Add(this._itemsThatWillBeInherited);
			}
			newMainParty.ItemRoster.Add(this._equipmentsThatWillBeInherited);
			this._itemsThatWillBeInherited.Clear();
			this._equipmentsThatWillBeInherited.Clear();
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0004D850 File Offset: 0x0004BA50
		private void OnBeforeMainCharacterDied(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			Dictionary<Hero, int> heirApparents = Hero.MainHero.Clan.GetHeirApparents();
			Hero.MainHero.AddDeathMark(killer, detail);
			if (heirApparents.Count == 0)
			{
				if (PlayerEncounter.Current != null && (PlayerEncounter.Battle == null || !PlayerEncounter.Battle.IsFinalized))
				{
					PlayerEncounter.Finish(true);
				}
				Dictionary<TroopRosterElement, int> dictionary = new Dictionary<TroopRosterElement, int>();
				foreach (TroopRosterElement troopRosterElement in MobileParty.MainParty.Party.MemberRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character != CharacterObject.PlayerCharacter)
					{
						dictionary.Add(troopRosterElement, troopRosterElement.Number);
					}
				}
				foreach (KeyValuePair<TroopRosterElement, int> keyValuePair in dictionary)
				{
					MobileParty.MainParty.Party.MemberRoster.RemoveTroop(keyValuePair.Key.Character, keyValuePair.Value, default(UniqueTroopDescriptor), 0);
				}
				CampaignEventDispatcher.Instance.OnGameOver();
				this.GameOverCleanup();
				this.ShowGameStatistics();
				Campaign.Current.OnGameOver();
			}
			else
			{
				if (Hero.MainHero.IsPrisoner)
				{
					EndCaptivityAction.ApplyByDeath(Hero.MainHero);
				}
				if (PlayerEncounter.Current != null && (PlayerEncounter.Battle == null || !PlayerEncounter.Battle.IsFinalized))
				{
					PlayerEncounter.Finish(true);
				}
				CampaignEventDispatcher.Instance.OnHeirSelectionRequested(heirApparents);
			}
			if (Campaign.Current.CurrentMenuContext != null)
			{
				GameMenu.ExitToLast();
			}
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0004D9F0 File Offset: 0x0004BBF0
		private void OnHeirSelectionOver(Hero selectedHeir)
		{
			ApplyHeirSelectionAction.ApplyByDeath(selectedHeir);
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0004D9F8 File Offset: 0x0004BBF8
		private void ShowGameStatistics()
		{
			object obj = new TextObject("{=oxb2FVz5}Clan Destroyed", null);
			TextObject textObject = new TextObject("{=T2GbF6lK}With no suitable heirs, the {CLAN_NAME} clan is no more. Your journey ends here.", null);
			textObject.SetTextVariable("CLAN_NAME", Clan.PlayerClan.Name);
			TextObject textObject2 = new TextObject("{=DM6luo3c}Continue", null);
			InformationManager.ShowInquiry(new InquiryData(obj.ToString(), textObject.ToString(), true, false, textObject2.ToString(), "", delegate
			{
				GameOverState gameOverState = Game.Current.GameStateManager.CreateState<GameOverState>(new object[] { GameOverState.GameOverReason.ClanDestroyed });
				Game.Current.GameStateManager.CleanAndPushState(gameOverState, 0);
			}, null, "", 0f, null, null, null), true, false);
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x0004DA90 File Offset: 0x0004BC90
		private void GameOverCleanup()
		{
			GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, Hero.MainHero.Gold, true);
			Campaign.Current.MainParty.Party.ItemRoster.Clear();
			Campaign.Current.MainParty.Party.MemberRoster.Clear();
			Campaign.Current.MainParty.Party.PrisonRoster.Clear();
			Campaign.Current.MainParty.IsVisible = false;
			Campaign.Current.CameraFollowParty = null;
			Campaign.Current.MainParty.IsActive = false;
			PartyBase.MainParty.SetVisualAsDirty();
			if (Hero.MainHero.MapFaction.IsKingdomFaction && Clan.PlayerClan.Kingdom.Leader == Hero.MainHero)
			{
				DestroyKingdomAction.ApplyByKingdomLeaderDeath(Clan.PlayerClan.Kingdom);
			}
		}

		// Token: 0x0400049E RID: 1182
		private readonly ItemRoster _itemsThatWillBeInherited = new ItemRoster();

		// Token: 0x0400049F RID: 1183
		private readonly ItemRoster _equipmentsThatWillBeInherited = new ItemRoster();
	}
}
