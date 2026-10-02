using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Events;
using TaleWorlds.CampaignSystem.ViewModelCollection.Quests;
using TaleWorlds.Core;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Overlay
{
	// Token: 0x020000BD RID: 189
	[MenuOverlay("SettlementMenuOverlay")]
	public class SettlementMenuOverlayVM : GameMenuOverlay
	{
		// Token: 0x06001298 RID: 4760 RVA: 0x0004B5E4 File Offset: 0x000497E4
		public SettlementMenuOverlayVM(GameMenu.MenuOverlayType type)
		{
			this._type = type;
			this._overlayTalkItem = null;
			base.IsInitializationOver = false;
			this._settlement = Settlement.CurrentSettlement;
			this.CharacterList = new MBBindingList<GameMenuPartyItemVM>();
			this.PartyList = new MBBindingList<GameMenuPartyItemVM>();
			this.IssueList = new MBBindingList<StringItemWithHintVM>();
			base.CurrentOverlayType = 0;
			this.CardSelectionPopup = new ClanCardSelectionPopupVM();
			this.CardSelectionPopup.IsVisible = false;
			this.CrimeHint = new BasicTooltipViewModel(() => this.GetCrimeTooltip());
			if (Settlement.CurrentSettlement.IsFortification)
			{
				this.RemainingFoodHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownFoodTooltip(this._settlement.Town));
				this.LoyaltyHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownLoyaltyTooltip(this._settlement.Town));
				this.MilitasHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownMilitiaTooltip(this._settlement.Town));
				this.ProsperityHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownProsperityTooltip(this._settlement.Town));
				this.WallsHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownWallsTooltip(this._settlement.Town));
				this.GarrisonHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownGarrisonTooltip(this._settlement.Town));
				this.SecurityHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownSecurityTooltip(this._settlement.Town));
			}
			else
			{
				this.MilitasHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetVillageMilitiaTooltip(this._settlement.Village));
				this.LoyaltyHint = new BasicTooltipViewModel();
				this.WallsHint = new BasicTooltipViewModel();
				this.ProsperityHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetVillageProsperityTooltip(this._settlement.Village));
			}
			this.UpdateSettlementOwnerBanner();
			this._contextMenuItem = null;
			base.IsInitializationOver = true;
			CampaignEvents.AfterSettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.OnQuestCompletedEvent.AddNonSerializedListener(this, new Action<QuestBase, QuestBase.QuestCompleteDetails>(this.OnQuestCompleted));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnPeaceDeclared));
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.TownRebelliosStateChanged.AddNonSerializedListener(this, new Action<Town, bool>(this.OnTownRebelliousStateChanged));
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
			this.RefreshValues();
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x0004B844 File Offset: 0x00049A44
		private List<TooltipProperty> GetCrimeTooltip()
		{
			Game game = Game.Current;
			if (game != null)
			{
				game.EventManager.TriggerEvent<CrimeValueInspectedInSettlementOverlayEvent>(new CrimeValueInspectedInSettlementOverlayEvent());
			}
			return CampaignUIHelper.GetCrimeTooltip(this._settlement);
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x0004B86B File Offset: 0x00049A6B
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PartyFilterHint = new HintViewModel(GameTexts.FindText("str_parties", null), null);
			this.CharacterFilterHint = new HintViewModel(GameTexts.FindText("str_characters", null), null);
			this.Refresh();
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x0004B8A8 File Offset: 0x00049AA8
		protected override void ExecuteOnSetAsActiveContextMenuItem(GameMenuPartyItemVM troop)
		{
			base.ExecuteOnSetAsActiveContextMenuItem(troop);
			base.ContextList.Clear();
			this.IssueList.Clear();
			if (this._contextMenuItem.Character != null && (!this._contextMenuItem.Character.IsHero || !this._contextMenuItem.Character.HeroObject.IsPrisoner))
			{
				bool flag = true;
				TextObject textObject = TextObject.GetEmpty();
				this._mostRecentOverlayTalkPermission = null;
				Game.Current.EventManager.TriggerEvent<SettlementOverlayTalkPermissionEvent>(new SettlementOverlayTalkPermissionEvent(this._contextMenuItem.Character.HeroObject, new Action<bool, TextObject>(this.OnSettlementOverlayTalkPermissionResult)));
				if (this._mostRecentOverlayTalkPermission != null)
				{
					flag = this._mostRecentOverlayTalkPermission.Item1;
					textObject = this._mostRecentOverlayTalkPermission.Item2;
				}
				this._overlayTalkItem = new GameMenuOverlayActionVM(new Action<object>(base.ExecuteTroopAction), GameTexts.FindText("str_menu_overlay_context_list", "Conversation").ToString(), flag, GameMenuOverlay.MenuOverlayContextList.Conversation, textObject);
				base.ContextList.Add(this._overlayTalkItem);
				bool flag2 = true;
				TextObject textObject2 = TextObject.GetEmpty();
				this._mostRecentOverlayQuickTalkPermission = null;
				Game.Current.EventManager.TriggerEvent<SettlementOverylayQuickTalkPermissionEvent>(new SettlementOverylayQuickTalkPermissionEvent(this._contextMenuItem.Character.HeroObject, new Action<bool, TextObject>(this.OnSettlementOverlayQuickTalkPermissionResult)));
				if (this._mostRecentOverlayQuickTalkPermission != null)
				{
					flag2 = this._mostRecentOverlayQuickTalkPermission.Item1;
					textObject2 = this._mostRecentOverlayQuickTalkPermission.Item2;
				}
				this._overlayQuickTalkItem = new GameMenuOverlayActionVM(new Action<object>(base.ExecuteTroopAction), GameTexts.FindText("str_menu_overlay_context_list", "QuickConversation").ToString(), flag2, GameMenuOverlay.MenuOverlayContextList.QuickConversation, textObject2);
				base.ContextList.Add(this._overlayQuickTalkItem);
				foreach (QuestMarkerVM questMarkerVM in troop.Quests)
				{
					if (questMarkerVM.IssueQuestFlag != CampaignUIHelper.IssueQuestFlags.None)
					{
						GameTexts.SetVariable("STR2", questMarkerVM.QuestTitle);
						string text = string.Empty;
						if (questMarkerVM.IssueQuestFlag == CampaignUIHelper.IssueQuestFlags.ActiveIssue)
						{
							text = "{=!}<img src=\"General\\Icons\\icon_issue_active_square\" extend=\"4\">";
						}
						else if (questMarkerVM.IssueQuestFlag == CampaignUIHelper.IssueQuestFlags.AvailableIssue)
						{
							text = "{=!}<img src=\"General\\Icons\\icon_issue_available_square\" extend=\"4\">";
						}
						else if (questMarkerVM.IssueQuestFlag == CampaignUIHelper.IssueQuestFlags.ActiveStoryQuest)
						{
							text = "{=!}<img src=\"General\\Icons\\icon_story_quest_active_square\" extend=\"4\">";
						}
						else if (questMarkerVM.IssueQuestFlag == CampaignUIHelper.IssueQuestFlags.TrackedIssue)
						{
							text = "{=!}<img src=\"General\\Icons\\issue_target_icon\" extend=\"4\">";
						}
						else if (questMarkerVM.IssueQuestFlag == CampaignUIHelper.IssueQuestFlags.TrackedStoryQuest)
						{
							text = "{=!}<img src=\"General\\Icons\\quest_target_icon\" extend=\"4\">";
						}
						GameTexts.SetVariable("STR1", text);
						string text2 = GameTexts.FindText("str_STR1_STR2", null).ToString();
						this.IssueList.Add(new StringItemWithHintVM(text2, questMarkerVM.QuestHint.HintText));
					}
				}
				if (this._contextMenuItem.Character.IsHero)
				{
					MobileParty partyBelongedTo = this._contextMenuItem.Character.HeroObject.PartyBelongedTo;
					if (((partyBelongedTo != null) ? partyBelongedTo.Army : null) != null && this._contextMenuItem.Character.HeroObject.PartyBelongedTo.Army.LeaderParty == this._contextMenuItem.Character.HeroObject.PartyBelongedTo && MobileParty.MainParty.Army == null && DiplomacyHelper.IsSameFactionAndNotEliminated(this._contextMenuItem.Character.HeroObject.MapFaction, Hero.MainHero.MapFaction))
					{
						GameMenuOverlayActionVM gameMenuOverlayActionVM = new GameMenuOverlayActionVM(new Action<object>(base.ExecuteTroopAction), GameTexts.FindText("str_menu_overlay_context_list", "JoinArmy").ToString(), true, GameMenuOverlay.MenuOverlayContextList.JoinArmy, null);
						base.ContextList.Add(gameMenuOverlayActionVM);
					}
				}
				if (this._contextMenuItem.Character.IsHero && this._contextMenuItem.Character.HeroObject.PartyBelongedTo == null && this._contextMenuItem.Character.HeroObject.Clan == Clan.PlayerClan && this._contextMenuItem.Character.HeroObject.Age > (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && !Campaign.Current.GetCampaignBehavior<IAlleyCampaignBehavior>().IsHeroAlleyLeaderOfAnyPlayerAlley(this._contextMenuItem.Character.HeroObject))
				{
					GameMenuOverlayActionVM gameMenuOverlayActionVM2 = new GameMenuOverlayActionVM(new Action<object>(base.ExecuteTroopAction), GameTexts.FindText("str_menu_overlay_context_list", "TakeToParty").ToString(), true, GameMenuOverlay.MenuOverlayContextList.TakeToParty, null);
					base.ContextList.Add(gameMenuOverlayActionVM2);
				}
				CampaignEventDispatcher.Instance.OnCharacterPortraitPopUpOpened(this._contextMenuItem.Character);
				return;
			}
			if (this._contextMenuItem.Party != null)
			{
				Hero owner = this._contextMenuItem.Party.Owner;
				if (((owner != null) ? owner.Clan : null) == Hero.MainHero.Clan)
				{
					MobileParty mobileParty = this._contextMenuItem.Party.MobileParty;
					if (mobileParty != null && !mobileParty.IsMainParty)
					{
						MobileParty mobileParty2 = this._contextMenuItem.Party.MobileParty;
						if (mobileParty2 != null && mobileParty2.IsGarrison)
						{
							this._overlayTalkItem = new GameMenuOverlayActionVM(new Action<object>(base.ExecuteTroopAction), GameTexts.FindText("str_menu_overlay_context_list", "ManageGarrison").ToString(), true, GameMenuOverlay.MenuOverlayContextList.ManageGarrison, null);
							base.ContextList.Add(this._overlayTalkItem);
							goto IL_0685;
						}
					}
				}
				if (this._contextMenuItem.Party.MapFaction == Hero.MainHero.MapFaction)
				{
					MobileParty mobileParty3 = this._contextMenuItem.Party.MobileParty;
					if (mobileParty3 != null && !mobileParty3.IsMainParty && (this._contextMenuItem.Party.MobileParty == null || (!this._contextMenuItem.Party.MobileParty.IsVillager && !this._contextMenuItem.Party.MobileParty.IsCaravan && !this._contextMenuItem.Party.MobileParty.IsPatrolParty && !this._contextMenuItem.Party.MobileParty.IsMilitia)))
					{
						if (this._contextMenuItem.Party.MobileParty.ActualClan == Clan.PlayerClan)
						{
							this._overlayTalkItem = new GameMenuOverlayActionVM(new Action<object>(base.ExecuteTroopAction), GameTexts.FindText("str_menu_overlay_context_list", "ManageTroops").ToString(), true, GameMenuOverlay.MenuOverlayContextList.ManageTroops, null);
							base.ContextList.Add(this._overlayTalkItem);
						}
						else
						{
							this._overlayTalkItem = new GameMenuOverlayActionVM(new Action<object>(base.ExecuteTroopAction), GameTexts.FindText("str_menu_overlay_context_list", "DonateTroops").ToString(), true, GameMenuOverlay.MenuOverlayContextList.DonateTroops, null);
							base.ContextList.Add(this._overlayTalkItem);
						}
					}
				}
				IL_0685:
				if (this._contextMenuItem.Party.LeaderHero != null && this._contextMenuItem.Party.LeaderHero != Hero.MainHero)
				{
					bool flag3 = this.CharacterList.Any<GameMenuPartyItemVM>((GameMenuPartyItemVM c) => c.Character == this._contextMenuItem.Party.LeaderHero.CharacterObject);
					TextObject textObject3 = ((!flag3) ? GameTexts.FindText("str_menu_overlay_cant_talk_to_party_leader", null) : TextObject.GetEmpty());
					base.ContextList.Add(new StringItemWithEnabledAndHintVM(new Action<object>(base.ExecuteTroopAction), GameTexts.FindText("str_menu_overlay_context_list", "ConverseWithLeader").ToString(), flag3, GameMenuOverlay.MenuOverlayContextList.ConverseWithLeader, textObject3));
				}
				CharacterObject visualPartyLeader = CampaignUIHelper.GetVisualPartyLeader(this._contextMenuItem.Party);
				if (visualPartyLeader != null)
				{
					CampaignEventDispatcher.Instance.OnCharacterPortraitPopUpOpened(visualPartyLeader);
				}
			}
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x0004C004 File Offset: 0x0004A204
		private void OnSettlementOverlayTalkPermissionResult(bool isAvailable, TextObject reasonStr)
		{
			this._mostRecentOverlayTalkPermission = new Tuple<bool, TextObject>(isAvailable, reasonStr);
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x0004C013 File Offset: 0x0004A213
		private void OnSettlementOverlayQuickTalkPermissionResult(bool isAvailable, TextObject reasonStr)
		{
			this._mostRecentOverlayQuickTalkPermission = new Tuple<bool, TextObject>(isAvailable, reasonStr);
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x0004C022 File Offset: 0x0004A222
		private void OnSettlementOverlayLeaveCharacterPermissionResult(bool isAvailable, TextObject reasonStr)
		{
			this._mostRecentOverlayLeaveCharacterPermission = new Tuple<bool, TextObject>(isAvailable, reasonStr);
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x0004C031 File Offset: 0x0004A231
		public override void ExecuteOnOverlayClosed()
		{
			base.ExecuteOnOverlayClosed();
			this.InitLists();
			base.ContextList.Clear();
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x0004C04A File Offset: 0x0004A24A
		private void ExecuteCloseTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x0004C051 File Offset: 0x0004A251
		private void ExecuteOpenTooltip()
		{
			InformationManager.ShowTooltip(typeof(Settlement), new object[] { this._settlement, true });
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x0004C07A File Offset: 0x0004A27A
		private void ExecuteSettlementLink()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(this._settlement.EncyclopediaLink);
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x0004C098 File Offset: 0x0004A298
		private bool Contains(MBBindingList<GameMenuPartyItemVM> list, CharacterObject character)
		{
			using (IEnumerator<GameMenuPartyItemVM> enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Character == character)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x0004C0E8 File Offset: 0x0004A2E8
		public override void UpdateOverlayType(GameMenu.MenuOverlayType newType)
		{
			this._type = newType;
			base.UpdateOverlayType(newType);
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x0004C0F8 File Offset: 0x0004A2F8
		private void InitLists()
		{
			this.UpdateCharacterList();
			this.UpdatePartyList();
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x0004C108 File Offset: 0x0004A308
		private void UpdateCharacterList()
		{
			if (this._type == GameMenu.MenuOverlayType.SettlementWithCharacters || this._type == GameMenu.MenuOverlayType.SettlementWithBoth)
			{
				Dictionary<Hero, bool> dictionary = new Dictionary<Hero, bool>();
				foreach (LocationCharacter locationCharacter in Campaign.Current.GameMenuManager.MenuLocations.SelectMany<Location, LocationCharacter>((Location l) => l.GetCharacterList()))
				{
					if (Campaign.Current.Models.HeroAgentLocationModel.WillBeListedInOverlay(locationCharacter) && !dictionary.ContainsKey(locationCharacter.Character.HeroObject))
					{
						dictionary.Add(locationCharacter.Character.HeroObject, locationCharacter.UseCivilianEquipment);
					}
				}
				for (int i = this.CharacterList.Count - 1; i >= 0; i--)
				{
					GameMenuPartyItemVM gameMenuPartyItemVM = this.CharacterList[i];
					if (!dictionary.ContainsKey(gameMenuPartyItemVM.Character.HeroObject))
					{
						this.CharacterList.RemoveAt(i);
					}
				}
				using (Dictionary<Hero, bool>.Enumerator enumerator2 = dictionary.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<Hero, bool> heroKvp = enumerator2.Current;
						if (!this.CharacterList.Any<GameMenuPartyItemVM>((GameMenuPartyItemVM x) => x.Character == heroKvp.Key.CharacterObject))
						{
							GameMenuPartyItemVM gameMenuPartyItemVM2 = new GameMenuPartyItemVM(new Action<GameMenuPartyItemVM>(this.ExecuteOnSetAsActiveContextMenuItem), heroKvp.Key.CharacterObject, heroKvp.Value);
							this.CharacterList.Add(gameMenuPartyItemVM2);
						}
					}
				}
				this.CharacterList.Sort(new SettlementMenuOverlayVM.CharacterComparer());
				return;
			}
			this.CharacterList.Clear();
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x0004C2D8 File Offset: 0x0004A4D8
		private void UpdatePartyList()
		{
			if (this._type == GameMenu.MenuOverlayType.SettlementWithBoth || this._type == GameMenu.MenuOverlayType.SettlementWithParties)
			{
				SettlementMenuOverlayVM.<>c__DisplayClass22_0 CS$<>8__locals1 = new SettlementMenuOverlayVM.<>c__DisplayClass22_0();
				Settlement settlement = MobileParty.MainParty.CurrentSettlement ?? MobileParty.MainParty.LastVisitedSettlement;
				CS$<>8__locals1.partiesInSettlement = new List<MobileParty>();
				foreach (MobileParty mobileParty in settlement.Parties)
				{
					if (this.WillBeListed(mobileParty))
					{
						CS$<>8__locals1.partiesInSettlement.Add(mobileParty);
					}
				}
				for (int j = this.PartyList.Count - 1; j >= 0; j--)
				{
					GameMenuPartyItemVM gameMenuPartyItemVM = this.PartyList[j];
					if (!CS$<>8__locals1.partiesInSettlement.Contains(gameMenuPartyItemVM.Party.MobileParty))
					{
						this.PartyList.RemoveAt(j);
					}
				}
				int i2;
				int i;
				for (i = 0; i < CS$<>8__locals1.partiesInSettlement.Count; i = i2 + 1)
				{
					if (!this.PartyList.Any<GameMenuPartyItemVM>((GameMenuPartyItemVM x) => x.Party == CS$<>8__locals1.partiesInSettlement[i].Party))
					{
						GameMenuPartyItemVM gameMenuPartyItemVM2 = new GameMenuPartyItemVM(new Action<GameMenuPartyItemVM>(this.ExecuteOnSetAsActiveContextMenuItem), CS$<>8__locals1.partiesInSettlement[i].Party, false);
						this.PartyList.Add(gameMenuPartyItemVM2);
					}
					i2 = i;
				}
				this.PartyList.Sort(new SettlementMenuOverlayVM.PartyComparer());
				return;
			}
			this.PartyList.Clear();
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x0004C480 File Offset: 0x0004A680
		private void UpdateList<TListItem, TElement>(MBBindingList<TListItem> listToUpdate, IEnumerable<TElement> listInSettlement, IComparer<TListItem> comparer, Func<TListItem, TElement> getElementFromListItem, Func<TElement, bool> doesSettlementHasElement, Func<TElement, TListItem> createListItem)
		{
			HashSet<TElement> hashSet = new HashSet<TElement>();
			for (int i = 0; i < listToUpdate.Count; i++)
			{
				TListItem tlistItem = listToUpdate[i];
				TElement telement = getElementFromListItem(tlistItem);
				if (doesSettlementHasElement(telement))
				{
					hashSet.Add(telement);
				}
				else
				{
					listToUpdate.RemoveAt(i);
					i--;
				}
			}
			foreach (TElement telement2 in listInSettlement)
			{
				if (!hashSet.Contains(telement2))
				{
					listToUpdate.Add(createListItem(telement2));
					hashSet.Add(telement2);
				}
			}
			listToUpdate.Sort(comparer);
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x0004C538 File Offset: 0x0004A738
		private bool WillBeListed(MobileParty mobileParty)
		{
			return mobileParty != null && mobileParty.IsActive;
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x0004C548 File Offset: 0x0004A748
		private bool WillBeListed(CharacterObject character)
		{
			Settlement settlement = ((MobileParty.MainParty.CurrentSettlement != null) ? MobileParty.MainParty.CurrentSettlement : MobileParty.MainParty.LastVisitedSettlement);
			return character.IsHero && character.HeroObject.PartyBelongedTo != MobileParty.MainParty && character.HeroObject.CurrentSettlement == settlement;
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x0004C5A4 File Offset: 0x0004A7A4
		private void UpdateSettlementOwnerBanner()
		{
			Banner banner = null;
			IFaction mapFaction = this._settlement.MapFaction;
			if (mapFaction != null && mapFaction.IsKingdomFaction && ((Kingdom)this._settlement.MapFaction).RulingClan == this._settlement.OwnerClan)
			{
				banner = this._settlement.OwnerClan.Kingdom.Banner;
			}
			else
			{
				Clan ownerClan = this._settlement.OwnerClan;
				if (((ownerClan != null) ? ownerClan.Banner : null) != null)
				{
					banner = this._settlement.OwnerClan.Banner;
				}
			}
			if (banner != null)
			{
				this.SettlementOwnerBanner = new BannerImageIdentifierVM(banner, true);
				return;
			}
			this.SettlementOwnerBanner = new BannerImageIdentifierVM(null, false);
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x0004C650 File Offset: 0x0004A850
		private void UpdateProperties()
		{
			Settlement currentSettlement = ((MobileParty.MainParty.CurrentSettlement != null) ? MobileParty.MainParty.CurrentSettlement : MobileParty.MainParty.LastVisitedSettlement);
			this.IsFortification = currentSettlement.IsFortification;
			IFaction mapFaction = currentSettlement.MapFaction;
			this.IsCrimeEnabled = mapFaction != null && mapFaction.MainHeroCrimeRating > 0f;
			IFaction mapFaction2 = currentSettlement.MapFaction;
			this.CrimeLbl = ((int)((mapFaction2 != null) ? new float?(mapFaction2.MainHeroCrimeRating) : null).Value).ToString();
			IFaction mapFaction3 = currentSettlement.MapFaction;
			this.CrimeChangeAmount = (int)((mapFaction3 != null) ? new float?(mapFaction3.DailyCrimeRatingChange) : null).Value;
			this.RemainingFoodText = (currentSettlement.IsFortification ? ((int)currentSettlement.Town.FoodStocks).ToString() : "-");
			this.FoodChangeAmount = ((currentSettlement.Town != null) ? ((int)currentSettlement.Town.FoodChange) : 0);
			this.MilitasLbl = ((int)currentSettlement.Militia).ToString();
			Town town = currentSettlement.Town;
			int num;
			if (town == null)
			{
				Village village = currentSettlement.Village;
				num = (int)((village != null) ? village.MilitiaChange : 0f);
			}
			else
			{
				num = (int)town.MilitiaChange;
			}
			this.MilitiaChangeAmount = num;
			this.IsLoyaltyRebellionWarning = currentSettlement.IsTown && currentSettlement.Town.Loyalty < (float)Campaign.Current.Models.SettlementLoyaltyModel.RebelliousStateStartLoyaltyThreshold;
			if (currentSettlement.IsFortification)
			{
				this.ProsperityLbl = ((int)currentSettlement.Town.Prosperity).ToString();
				this.ProsperityChangeAmount = (int)currentSettlement.Town.ProsperityChange;
				MobileParty garrisonParty = currentSettlement.Town.GarrisonParty;
				this.GarrisonLbl = ((garrisonParty != null) ? garrisonParty.Party.NumberOfAllMembers.ToString() : null) ?? "0";
				this.GarrisonChangeAmount = (int)SettlementHelper.GetGarrisonChangeExplainedNumber(currentSettlement.Town).ResultNumber;
				MobileParty garrisonParty2 = currentSettlement.Town.GarrisonParty;
				this.GarrisonAmount = ((garrisonParty2 != null) ? garrisonParty2.Party.NumberOfAllMembers : 0);
				this.IsNoGarrisonWarning = this.GarrisonAmount < 1;
				this.WallsLbl = currentSettlement.Town.GetWallLevel().ToString();
				this.WallsLevel = currentSettlement.Town.GetWallLevel();
				this.LoyaltyLbl = ((int)currentSettlement.Town.Loyalty).ToString();
				this.LoyaltyChangeAmount = (int)currentSettlement.Town.LoyaltyChange;
				this.SecurityLbl = ((int)currentSettlement.Town.Security).ToString();
				this.SecurityChangeAmount = (int)currentSettlement.Town.SecurityChange;
			}
			else
			{
				this.GarrisonLbl = "-";
				this.GarrisonChangeAmount = 0;
				this.WallsLbl = "-";
				this.WallsLevel = 1;
				this.LoyaltyLbl = "-";
				this.LoyaltyChangeAmount = 0;
				this.SecurityLbl = "-";
				this.SecurityChangeAmount = 0;
				if (currentSettlement.IsVillage)
				{
					this.ProsperityLbl = ((int)currentSettlement.Village.Hearth).ToString();
					this.ProsperityChangeAmount = (int)currentSettlement.Village.HearthChange;
				}
			}
			this.SettlementNameLbl = currentSettlement.Name + ((currentSettlement.IsVillage && currentSettlement.Village.VillageState != Village.VillageStates.Normal) ? ("(" + currentSettlement.Village.VillageState.ToString() + ")") : "");
			Game.Current.EventManager.TriggerEvent<SettlementOverlayLeaveCharacterPermissionEvent>(new SettlementOverlayLeaveCharacterPermissionEvent(new Action<bool, TextObject>(this.OnSettlementOverlayLeaveCharacterPermissionResult)));
			if (currentSettlement.IsVillage)
			{
				this.CanAssignMembers = false;
				this.AssignMembersHint = new HintViewModel(new TextObject("{=4p0M9T0N}Cannot assign members in a village.", null), null);
				return;
			}
			if (this._mostRecentOverlayLeaveCharacterPermission != null)
			{
				this.CanAssignMembers = this._mostRecentOverlayLeaveCharacterPermission.Item1;
				this.AssignMembersHint = (this.CanAssignMembers ? new HintViewModel(new TextObject("{=GSiE1FPj}Assing Member(s)", null), null) : new HintViewModel(this._mostRecentOverlayLeaveCharacterPermission.Item2, null));
				return;
			}
			this.CanAssignMembers = Clan.PlayerClan.Heroes.Any<Hero>((Hero hero) => currentSettlement == hero.StayingInSettlement || (!hero.CharacterObject.IsPlayerCharacter && MobileParty.MainParty.MemberRoster.Contains(hero.CharacterObject)));
			if (!this.CanAssignMembers)
			{
				this.AssignMembersHint = new HintViewModel(new TextObject("{=zbyqlOX3}Assign members. Need at least 1 companion.", null), null);
				return;
			}
			this.AssignMembersHint = new HintViewModel(new TextObject("{=ahp6nVg0}Assign Member(s)", null), null);
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x0004CB74 File Offset: 0x0004AD74
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent obj)
		{
			this._latestTutorialElementID = obj.NewNotificationElementID;
			if (this._latestTutorialElementID != null)
			{
				if (this._latestTutorialElementID != "")
				{
					if (this._latestTutorialElementID == "ApplicapleCompanion" && !this._isCompanionHighlightApplied)
					{
						this._isCompanionHighlightApplied = this.SetPartyItemHighlightState(this._latestTutorialElementID, true);
					}
					else if (this._latestTutorialElementID != "ApplicapleCompanion" && this._isCompanionHighlightApplied)
					{
						this._isCompanionHighlightApplied = this.SetPartyItemHighlightState("ApplicapleCompanion", false);
					}
					if (this._latestTutorialElementID == "ApplicableQuestGivers" && !this._isQuestGiversHighlightApplied)
					{
						this._isQuestGiversHighlightApplied = this.SetPartyItemHighlightState(this._latestTutorialElementID, true);
					}
					else if (this._latestTutorialElementID != "ApplicableQuestGivers" && this._isQuestGiversHighlightApplied)
					{
						this._isCompanionHighlightApplied = this.SetPartyItemHighlightState("ApplicableQuestGivers", false);
					}
					if (this._latestTutorialElementID == "ApplicableNotable" && !this._isNotableHighlightApplied)
					{
						this._isNotableHighlightApplied = this.SetPartyItemHighlightState(this._latestTutorialElementID, true);
					}
					else if (this._latestTutorialElementID != "ApplicableNotable" && this._isNotableHighlightApplied)
					{
						this._isNotableHighlightApplied = this.SetPartyItemHighlightState("ApplicableNotable", false);
					}
					if (this._latestTutorialElementID == "CrimeLabel" && !this.IsCrimeLabelHighlightEnabled)
					{
						this.IsCrimeLabelHighlightEnabled = true;
					}
					else if (this._latestTutorialElementID != "CrimeLabel" && this.IsCrimeLabelHighlightEnabled)
					{
						this.IsCrimeLabelHighlightEnabled = false;
					}
					if (this._latestTutorialElementID == "OverlayTalkButton" && !this._isTalkItemHighlightApplied)
					{
						if (this._overlayTalkItem != null)
						{
							this._overlayTalkItem.IsHiglightEnabled = true;
							this._isTalkItemHighlightApplied = true;
							return;
						}
					}
					else if (this._latestTutorialElementID != "OverlayTalkButton" && this._isTalkItemHighlightApplied && this._overlayTalkItem != null)
					{
						this._overlayTalkItem.IsHiglightEnabled = false;
						this._isTalkItemHighlightApplied = true;
						return;
					}
				}
				else
				{
					if (this._isCompanionHighlightApplied)
					{
						this._isCompanionHighlightApplied = !this.SetPartyItemHighlightState("ApplicapleCompanion", false);
					}
					if (this._isNotableHighlightApplied)
					{
						this._isNotableHighlightApplied = !this.SetPartyItemHighlightState("ApplicableNotable", false);
					}
					if (this._isQuestGiversHighlightApplied)
					{
						this._isQuestGiversHighlightApplied = !this.SetPartyItemHighlightState("ApplicableQuestGivers", false);
					}
					if (this.IsCrimeLabelHighlightEnabled)
					{
						this.IsCrimeLabelHighlightEnabled = false;
					}
					if (this._isTalkItemHighlightApplied && this._overlayTalkItem != null)
					{
						this._overlayTalkItem.IsHiglightEnabled = false;
						this._isTalkItemHighlightApplied = false;
						return;
					}
				}
			}
			else
			{
				if (this._isCompanionHighlightApplied)
				{
					this._isCompanionHighlightApplied = !this.SetPartyItemHighlightState("ApplicapleCompanion", false);
				}
				if (this._isNotableHighlightApplied)
				{
					this._isNotableHighlightApplied = !this.SetPartyItemHighlightState("ApplicableNotable", false);
				}
				if (this._isQuestGiversHighlightApplied)
				{
					this._isQuestGiversHighlightApplied = !this.SetPartyItemHighlightState("ApplicableQuestGivers", false);
				}
				if (this._isTalkItemHighlightApplied && this._overlayTalkItem != null)
				{
					this._overlayTalkItem.IsHiglightEnabled = false;
					this._isTalkItemHighlightApplied = false;
				}
				if (this.IsCrimeLabelHighlightEnabled)
				{
					this.IsCrimeLabelHighlightEnabled = false;
				}
			}
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x0004CE94 File Offset: 0x0004B094
		private bool SetPartyItemHighlightState(string condition, bool state)
		{
			bool flag = false;
			foreach (GameMenuPartyItemVM gameMenuPartyItemVM in this.CharacterList)
			{
				if (condition == "ApplicapleCompanion" && gameMenuPartyItemVM.Character.IsHero && gameMenuPartyItemVM.Character.HeroObject.IsWanderer && !gameMenuPartyItemVM.Character.HeroObject.IsPlayerCompanion)
				{
					gameMenuPartyItemVM.IsHighlightEnabled = state;
					flag = true;
				}
				else if (condition == "ApplicableNotable" && gameMenuPartyItemVM.Character.IsHero && gameMenuPartyItemVM.Character.HeroObject.IsNotable && !gameMenuPartyItemVM.Character.HeroObject.IsPlayerCompanion)
				{
					gameMenuPartyItemVM.IsHighlightEnabled = state;
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x0004CF74 File Offset: 0x0004B174
		public override void Refresh()
		{
			base.IsInitializationOver = false;
			this.InitLists();
			this.UpdateProperties();
			foreach (GameMenuPartyItemVM gameMenuPartyItemVM in this.CharacterList)
			{
				gameMenuPartyItemVM.RefreshProperties();
			}
			foreach (GameMenuPartyItemVM gameMenuPartyItemVM2 in this.PartyList)
			{
				gameMenuPartyItemVM2.RefreshProperties();
			}
			base.IsInitializationOver = true;
			base.Refresh();
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x0004D018 File Offset: 0x0004B218
		public void ExecuteAddCompanion()
		{
			Settlement settlement = this._settlement;
			if (((settlement != null) ? settlement.Town : null) != null)
			{
				TextObject textObject = GameTexts.FindText("str_send_members", null);
				textObject.SetTextVariable("SETTLEMENT_NAME", this._settlement.Name);
				ClanCardSelectionInfo clanCardSelectionInfo = new ClanCardSelectionInfo(textObject, this.GetSendMembersCandidates(), new Action<List<object>, Action>(this.OnAssignMembersToSettlement), true, 1, 0);
				ClanCardSelectionPopupVM cardSelectionPopup = this.CardSelectionPopup;
				if (cardSelectionPopup == null)
				{
					return;
				}
				cardSelectionPopup.Open(clanCardSelectionInfo);
			}
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x0004D08A File Offset: 0x0004B28A
		private IEnumerable<ClanCardSelectionItemInfo> GetSendMembersCandidates()
		{
			foreach (Hero hero in from m in MobileParty.MainParty.MemberRoster.GetTroopRoster()
				where m.Character.IsHero && !m.Character.IsPlayerCharacter && m.Character.HeroObject.CanMoveToSettlement()
				select m.Character.HeroObject)
			{
				SkillObject charm = DefaultSkills.Charm;
				int skillValue = hero.GetSkillValue(charm);
				CharacterImageIdentifier characterImageIdentifier = new CharacterImageIdentifier(CampaignUIHelper.GetCharacterCode(hero.CharacterObject, false));
				yield return new ClanCardSelectionItemInfo(hero, hero.Name, characterImageIdentifier, CardSelectionItemSpriteType.Skill, charm.StringId.ToLower(), skillValue.ToString(), this.GetSendMembersCandidateProperties(hero), false, null, null);
			}
			IEnumerator<Hero> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x0004D09A File Offset: 0x0004B29A
		private IEnumerable<ClanCardSelectionItemPropertyInfo> GetSendMembersCandidateProperties(Hero hero)
		{
			TextObject textObject = new TextObject("{=otaUtXMX}+{AMOUNT} relation chance with notables per day.", null);
			int emissaryRelationBonusForMainClan = Campaign.Current.Models.EmissaryModel.EmissaryRelationBonusForMainClan;
			textObject.SetTextVariable("AMOUNT", emissaryRelationBonusForMainClan);
			yield return new ClanCardSelectionItemPropertyInfo(textObject);
			yield break;
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x0004D0A4 File Offset: 0x0004B2A4
		private void OnAssignMembersToSettlement(List<object> leftMembers, Action closePopup)
		{
			Settlement settlement = ((MobileParty.MainParty.CurrentSettlement != null) ? MobileParty.MainParty.CurrentSettlement : MobileParty.MainParty.LastVisitedSettlement);
			if (closePopup != null)
			{
				closePopup();
			}
			foreach (object obj in leftMembers)
			{
				if (obj is Hero)
				{
					Hero hero = obj as Hero;
					PartyBase.MainParty.MemberRoster.RemoveTroop(hero.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
					if (hero.CharacterObject.IsHero && !settlement.HeroesWithoutParty.Contains(hero.CharacterObject.HeroObject))
					{
						EnterSettlementAction.ApplyForCharacterOnly(hero.CharacterObject.HeroObject, settlement);
					}
				}
			}
			if (leftMembers.Count > 0)
			{
				this.InitLists();
			}
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x0004D18C File Offset: 0x0004B38C
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
			Game.Current.EventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x0004D1BC File Offset: 0x0004B3BC
		private void OnSettlementEntered(MobileParty arg1, Settlement arg2, Hero arg3)
		{
			Settlement settlement = ((MobileParty.MainParty.CurrentSettlement != null) ? MobileParty.MainParty.CurrentSettlement : MobileParty.MainParty.LastVisitedSettlement);
			if (arg2 == settlement)
			{
				this.InitLists();
			}
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x0004D1F8 File Offset: 0x0004B3F8
		private void OnSettlementLeft(MobileParty arg1, Settlement arg2)
		{
			Settlement settlement = ((MobileParty.MainParty.CurrentSettlement != null) ? MobileParty.MainParty.CurrentSettlement : MobileParty.MainParty.LastVisitedSettlement);
			if (arg2 == settlement)
			{
				this.InitLists();
			}
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x0004D234 File Offset: 0x0004B434
		private void OnQuestCompleted(QuestBase arg1, QuestBase.QuestCompleteDetails arg2)
		{
			Settlement settlement = ((MobileParty.MainParty.CurrentSettlement != null) ? MobileParty.MainParty.CurrentSettlement : MobileParty.MainParty.LastVisitedSettlement);
			Hero questGiver = arg1.QuestGiver;
			if (((questGiver != null) ? questGiver.CurrentSettlement : null) != null && arg1.QuestGiver.CurrentSettlement == settlement)
			{
				this.Refresh();
			}
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x0004D28C File Offset: 0x0004B48C
		private void OnPeaceDeclared(IFaction faction1, IFaction faction2, MakePeaceAction.MakePeaceDetail detail)
		{
			this.OnPeaceOrWarDeclared(faction1, faction2);
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x0004D296 File Offset: 0x0004B496
		private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail reason)
		{
			this.OnPeaceOrWarDeclared(faction1, faction2);
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x0004D2A0 File Offset: 0x0004B4A0
		private void OnPeaceOrWarDeclared(IFaction arg1, IFaction arg2)
		{
			Hero mainHero = Hero.MainHero;
			bool flag;
			if (mainHero == null)
			{
				flag = null != null;
			}
			else
			{
				Settlement currentSettlement = mainHero.CurrentSettlement;
				flag = ((currentSettlement != null) ? currentSettlement.MapFaction : null) != null;
			}
			bool flag2;
			if (flag)
			{
				Hero mainHero2 = Hero.MainHero;
				if (((mainHero2 != null) ? mainHero2.CurrentSettlement.MapFaction : null) != arg1)
				{
					Hero mainHero3 = Hero.MainHero;
					flag2 = ((mainHero3 != null) ? mainHero3.CurrentSettlement.MapFaction : null) == arg2;
				}
				else
				{
					flag2 = true;
				}
			}
			else
			{
				flag2 = false;
			}
			Hero mainHero4 = Hero.MainHero;
			bool flag3;
			if (((mainHero4 != null) ? mainHero4.MapFaction : null) != arg1)
			{
				Hero mainHero5 = Hero.MainHero;
				flag3 = ((mainHero5 != null) ? mainHero5.MapFaction : null) == arg2;
			}
			else
			{
				flag3 = true;
			}
			bool flag4 = flag3;
			if (flag2 || flag4)
			{
				this.InitLists();
			}
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x0004D33A File Offset: 0x0004B53A
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero previousOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if (settlement == this._settlement || (this._settlement.IsVillage && settlement.BoundVillages.Contains(this._settlement.Village)))
			{
				this.UpdateSettlementOwnerBanner();
			}
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0004D370 File Offset: 0x0004B570
		private void OnTownRebelliousStateChanged(Town town, bool isRebellious)
		{
			if (this._settlement.IsTown && this._settlement.Town == town)
			{
				this.IsLoyaltyRebellionWarning = isRebellious || town.Loyalty < (float)Campaign.Current.Models.SettlementLoyaltyModel.RebelliousStateStartLoyaltyThreshold;
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x060012BD RID: 4797 RVA: 0x0004D3C1 File Offset: 0x0004B5C1
		// (set) Token: 0x060012BE RID: 4798 RVA: 0x0004D3C9 File Offset: 0x0004B5C9
		[DataSourceProperty]
		public ClanCardSelectionPopupVM CardSelectionPopup
		{
			get
			{
				return this._cardSelectionPopup;
			}
			set
			{
				if (value != this._cardSelectionPopup)
				{
					this._cardSelectionPopup = value;
					base.OnPropertyChangedWithValue<ClanCardSelectionPopupVM>(value, "CardSelectionPopup");
				}
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x060012BF RID: 4799 RVA: 0x0004D3E7 File Offset: 0x0004B5E7
		// (set) Token: 0x060012C0 RID: 4800 RVA: 0x0004D3EF File Offset: 0x0004B5EF
		[DataSourceProperty]
		public string RemainingFoodText
		{
			get
			{
				return this._remainingFoodText;
			}
			set
			{
				if (value != this._remainingFoodText)
				{
					this._remainingFoodText = value;
					base.OnPropertyChangedWithValue<string>(value, "RemainingFoodText");
				}
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x060012C1 RID: 4801 RVA: 0x0004D412 File Offset: 0x0004B612
		// (set) Token: 0x060012C2 RID: 4802 RVA: 0x0004D41A File Offset: 0x0004B61A
		[DataSourceProperty]
		public int ProsperityChangeAmount
		{
			get
			{
				return this._prosperityChangeAmount;
			}
			set
			{
				if (value != this._prosperityChangeAmount)
				{
					this._prosperityChangeAmount = value;
					base.OnPropertyChangedWithValue(value, "ProsperityChangeAmount");
				}
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x060012C3 RID: 4803 RVA: 0x0004D438 File Offset: 0x0004B638
		// (set) Token: 0x060012C4 RID: 4804 RVA: 0x0004D440 File Offset: 0x0004B640
		[DataSourceProperty]
		public int MilitiaChangeAmount
		{
			get
			{
				return this._militiaChangeAmount;
			}
			set
			{
				if (value != this._militiaChangeAmount)
				{
					this._militiaChangeAmount = value;
					base.OnPropertyChangedWithValue(value, "MilitiaChangeAmount");
				}
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x060012C5 RID: 4805 RVA: 0x0004D45E File Offset: 0x0004B65E
		// (set) Token: 0x060012C6 RID: 4806 RVA: 0x0004D466 File Offset: 0x0004B666
		[DataSourceProperty]
		public int GarrisonChangeAmount
		{
			get
			{
				return this._garrisonChangeAmount;
			}
			set
			{
				if (value != this._garrisonChangeAmount)
				{
					this._garrisonChangeAmount = value;
					base.OnPropertyChangedWithValue(value, "GarrisonChangeAmount");
				}
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x060012C7 RID: 4807 RVA: 0x0004D484 File Offset: 0x0004B684
		// (set) Token: 0x060012C8 RID: 4808 RVA: 0x0004D48C File Offset: 0x0004B68C
		[DataSourceProperty]
		public int GarrisonAmount
		{
			get
			{
				return this._garrisonAmount;
			}
			set
			{
				if (value != this._garrisonAmount)
				{
					this._garrisonAmount = value;
					base.OnPropertyChangedWithValue(value, "GarrisonAmount");
				}
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x060012C9 RID: 4809 RVA: 0x0004D4AA File Offset: 0x0004B6AA
		// (set) Token: 0x060012CA RID: 4810 RVA: 0x0004D4B2 File Offset: 0x0004B6B2
		[DataSourceProperty]
		public int CrimeChangeAmount
		{
			get
			{
				return this._crimeChangeAmount;
			}
			set
			{
				if (value != this._crimeChangeAmount)
				{
					this._crimeChangeAmount = value;
					base.OnPropertyChangedWithValue(value, "CrimeChangeAmount");
				}
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x060012CB RID: 4811 RVA: 0x0004D4D0 File Offset: 0x0004B6D0
		// (set) Token: 0x060012CC RID: 4812 RVA: 0x0004D4D8 File Offset: 0x0004B6D8
		[DataSourceProperty]
		public int LoyaltyChangeAmount
		{
			get
			{
				return this._loyaltyChangeAmount;
			}
			set
			{
				if (value != this._loyaltyChangeAmount)
				{
					this._loyaltyChangeAmount = value;
					base.OnPropertyChangedWithValue(value, "LoyaltyChangeAmount");
				}
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x060012CD RID: 4813 RVA: 0x0004D4F6 File Offset: 0x0004B6F6
		// (set) Token: 0x060012CE RID: 4814 RVA: 0x0004D4FE File Offset: 0x0004B6FE
		[DataSourceProperty]
		public int SecurityChangeAmount
		{
			get
			{
				return this._securityChangeAmount;
			}
			set
			{
				if (value != this._securityChangeAmount)
				{
					this._securityChangeAmount = value;
					base.OnPropertyChangedWithValue(value, "SecurityChangeAmount");
				}
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x060012CF RID: 4815 RVA: 0x0004D51C File Offset: 0x0004B71C
		// (set) Token: 0x060012D0 RID: 4816 RVA: 0x0004D524 File Offset: 0x0004B724
		[DataSourceProperty]
		public int FoodChangeAmount
		{
			get
			{
				return this._foodChangeAmount;
			}
			set
			{
				if (value != this._foodChangeAmount)
				{
					this._foodChangeAmount = value;
					base.OnPropertyChangedWithValue(value, "FoodChangeAmount");
				}
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x060012D1 RID: 4817 RVA: 0x0004D542 File Offset: 0x0004B742
		// (set) Token: 0x060012D2 RID: 4818 RVA: 0x0004D54A File Offset: 0x0004B74A
		[DataSourceProperty]
		public BasicTooltipViewModel RemainingFoodHint
		{
			get
			{
				return this._remainingFoodHint;
			}
			set
			{
				if (value != this._remainingFoodHint)
				{
					this._remainingFoodHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "RemainingFoodHint");
				}
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x060012D3 RID: 4819 RVA: 0x0004D568 File Offset: 0x0004B768
		// (set) Token: 0x060012D4 RID: 4820 RVA: 0x0004D570 File Offset: 0x0004B770
		[DataSourceProperty]
		public BasicTooltipViewModel SecurityHint
		{
			get
			{
				return this._securityHint;
			}
			set
			{
				if (value != this._securityHint)
				{
					this._securityHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "SecurityHint");
				}
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x060012D5 RID: 4821 RVA: 0x0004D58E File Offset: 0x0004B78E
		// (set) Token: 0x060012D6 RID: 4822 RVA: 0x0004D596 File Offset: 0x0004B796
		[DataSourceProperty]
		public HintViewModel PartyFilterHint
		{
			get
			{
				return this._partyFilterHint;
			}
			set
			{
				if (value != this._partyFilterHint)
				{
					this._partyFilterHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PartyFilterHint");
				}
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x060012D7 RID: 4823 RVA: 0x0004D5B4 File Offset: 0x0004B7B4
		// (set) Token: 0x060012D8 RID: 4824 RVA: 0x0004D5BC File Offset: 0x0004B7BC
		[DataSourceProperty]
		public HintViewModel CharacterFilterHint
		{
			get
			{
				return this._characterFilterHint;
			}
			set
			{
				if (value != this._characterFilterHint)
				{
					this._characterFilterHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CharacterFilterHint");
				}
			}
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x060012D9 RID: 4825 RVA: 0x0004D5DA File Offset: 0x0004B7DA
		// (set) Token: 0x060012DA RID: 4826 RVA: 0x0004D5E2 File Offset: 0x0004B7E2
		[DataSourceProperty]
		public BasicTooltipViewModel MilitasHint
		{
			get
			{
				return this._militasHint;
			}
			set
			{
				if (value != this._militasHint)
				{
					this._militasHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "MilitasHint");
				}
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x060012DB RID: 4827 RVA: 0x0004D600 File Offset: 0x0004B800
		// (set) Token: 0x060012DC RID: 4828 RVA: 0x0004D608 File Offset: 0x0004B808
		[DataSourceProperty]
		public BasicTooltipViewModel GarrisonHint
		{
			get
			{
				return this._garrisonHint;
			}
			set
			{
				if (value != this._garrisonHint)
				{
					this._garrisonHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "GarrisonHint");
				}
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x060012DD RID: 4829 RVA: 0x0004D626 File Offset: 0x0004B826
		// (set) Token: 0x060012DE RID: 4830 RVA: 0x0004D62E File Offset: 0x0004B82E
		[DataSourceProperty]
		public BasicTooltipViewModel ProsperityHint
		{
			get
			{
				return this._prosperityHint;
			}
			set
			{
				if (value != this._prosperityHint)
				{
					this._prosperityHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "ProsperityHint");
				}
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x060012DF RID: 4831 RVA: 0x0004D64C File Offset: 0x0004B84C
		// (set) Token: 0x060012E0 RID: 4832 RVA: 0x0004D654 File Offset: 0x0004B854
		[DataSourceProperty]
		public BasicTooltipViewModel LoyaltyHint
		{
			get
			{
				return this._loyaltyHint;
			}
			set
			{
				if (value != this._loyaltyHint)
				{
					this._loyaltyHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "LoyaltyHint");
				}
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x060012E1 RID: 4833 RVA: 0x0004D672 File Offset: 0x0004B872
		// (set) Token: 0x060012E2 RID: 4834 RVA: 0x0004D67A File Offset: 0x0004B87A
		[DataSourceProperty]
		public BasicTooltipViewModel WallsHint
		{
			get
			{
				return this._wallsHint;
			}
			set
			{
				if (value != this._wallsHint)
				{
					this._wallsHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "WallsHint");
				}
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x060012E3 RID: 4835 RVA: 0x0004D698 File Offset: 0x0004B898
		// (set) Token: 0x060012E4 RID: 4836 RVA: 0x0004D6A0 File Offset: 0x0004B8A0
		[DataSourceProperty]
		public BasicTooltipViewModel CrimeHint
		{
			get
			{
				return this._crimeHint;
			}
			set
			{
				if (value != this._crimeHint)
				{
					this._crimeHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "CrimeHint");
				}
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x060012E5 RID: 4837 RVA: 0x0004D6BE File Offset: 0x0004B8BE
		// (set) Token: 0x060012E6 RID: 4838 RVA: 0x0004D6C6 File Offset: 0x0004B8C6
		[DataSourceProperty]
		public HintViewModel AssignMembersHint
		{
			get
			{
				return this._assignMembersHint;
			}
			set
			{
				if (value != this._assignMembersHint)
				{
					this._assignMembersHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AssignMembersHint");
				}
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x060012E7 RID: 4839 RVA: 0x0004D6E4 File Offset: 0x0004B8E4
		// (set) Token: 0x060012E8 RID: 4840 RVA: 0x0004D6EC File Offset: 0x0004B8EC
		[DataSourceProperty]
		public BannerImageIdentifierVM SettlementOwnerBanner
		{
			get
			{
				return this._settlementOwnerBanner;
			}
			set
			{
				if (value != this._settlementOwnerBanner)
				{
					this._settlementOwnerBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "SettlementOwnerBanner");
				}
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x060012E9 RID: 4841 RVA: 0x0004D70A File Offset: 0x0004B90A
		// (set) Token: 0x060012EA RID: 4842 RVA: 0x0004D712 File Offset: 0x0004B912
		[DataSourceProperty]
		public MBBindingList<GameMenuPartyItemVM> CharacterList
		{
			get
			{
				return this._characterList;
			}
			set
			{
				if (value != this._characterList)
				{
					this._characterList = value;
					base.OnPropertyChangedWithValue<MBBindingList<GameMenuPartyItemVM>>(value, "CharacterList");
				}
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x060012EB RID: 4843 RVA: 0x0004D730 File Offset: 0x0004B930
		// (set) Token: 0x060012EC RID: 4844 RVA: 0x0004D738 File Offset: 0x0004B938
		[DataSourceProperty]
		public MBBindingList<GameMenuPartyItemVM> PartyList
		{
			get
			{
				return this._partyList;
			}
			set
			{
				if (value != this._partyList)
				{
					this._partyList = value;
					base.OnPropertyChangedWithValue<MBBindingList<GameMenuPartyItemVM>>(value, "PartyList");
				}
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x060012ED RID: 4845 RVA: 0x0004D756 File Offset: 0x0004B956
		// (set) Token: 0x060012EE RID: 4846 RVA: 0x0004D75E File Offset: 0x0004B95E
		[DataSourceProperty]
		public MBBindingList<StringItemWithHintVM> IssueList
		{
			get
			{
				return this._issueList;
			}
			set
			{
				if (value != this._issueList)
				{
					this._issueList = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringItemWithHintVM>>(value, "IssueList");
				}
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x060012EF RID: 4847 RVA: 0x0004D77C File Offset: 0x0004B97C
		// (set) Token: 0x060012F0 RID: 4848 RVA: 0x0004D784 File Offset: 0x0004B984
		[DataSourceProperty]
		public string MilitasLbl
		{
			get
			{
				return this._militasLbl;
			}
			set
			{
				if (value != this._militasLbl)
				{
					this._militasLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "MilitasLbl");
				}
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x060012F1 RID: 4849 RVA: 0x0004D7A7 File Offset: 0x0004B9A7
		// (set) Token: 0x060012F2 RID: 4850 RVA: 0x0004D7AF File Offset: 0x0004B9AF
		[DataSourceProperty]
		public string GarrisonLbl
		{
			get
			{
				return this._garrisonLbl;
			}
			set
			{
				if (value != this._garrisonLbl)
				{
					this._garrisonLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "GarrisonLbl");
				}
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x060012F3 RID: 4851 RVA: 0x0004D7D2 File Offset: 0x0004B9D2
		// (set) Token: 0x060012F4 RID: 4852 RVA: 0x0004D7DA File Offset: 0x0004B9DA
		[DataSourceProperty]
		public string CrimeLbl
		{
			get
			{
				return this._crimeLbl;
			}
			set
			{
				if (value != this._crimeLbl)
				{
					this._crimeLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "CrimeLbl");
				}
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x060012F5 RID: 4853 RVA: 0x0004D7FD File Offset: 0x0004B9FD
		// (set) Token: 0x060012F6 RID: 4854 RVA: 0x0004D805 File Offset: 0x0004BA05
		[DataSourceProperty]
		public bool CanAssignMembers
		{
			get
			{
				return this._canAssignMembers;
			}
			set
			{
				if (value != this._canAssignMembers)
				{
					this._canAssignMembers = value;
					base.OnPropertyChangedWithValue(value, "CanAssignMembers");
				}
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x060012F7 RID: 4855 RVA: 0x0004D823 File Offset: 0x0004BA23
		// (set) Token: 0x060012F8 RID: 4856 RVA: 0x0004D82B File Offset: 0x0004BA2B
		[DataSourceProperty]
		public string ProsperityLbl
		{
			get
			{
				return this._prosperityLbl;
			}
			set
			{
				if (value != this._prosperityLbl)
				{
					this._prosperityLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "ProsperityLbl");
				}
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x060012F9 RID: 4857 RVA: 0x0004D84E File Offset: 0x0004BA4E
		// (set) Token: 0x060012FA RID: 4858 RVA: 0x0004D856 File Offset: 0x0004BA56
		[DataSourceProperty]
		public string LoyaltyLbl
		{
			get
			{
				return this._loyaltyLbl;
			}
			set
			{
				if (value != this._loyaltyLbl)
				{
					this._loyaltyLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "LoyaltyLbl");
				}
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x060012FB RID: 4859 RVA: 0x0004D879 File Offset: 0x0004BA79
		// (set) Token: 0x060012FC RID: 4860 RVA: 0x0004D881 File Offset: 0x0004BA81
		[DataSourceProperty]
		public string SecurityLbl
		{
			get
			{
				return this._securityLbl;
			}
			set
			{
				if (value != this._securityLbl)
				{
					this._securityLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "SecurityLbl");
				}
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x060012FD RID: 4861 RVA: 0x0004D8A4 File Offset: 0x0004BAA4
		// (set) Token: 0x060012FE RID: 4862 RVA: 0x0004D8AC File Offset: 0x0004BAAC
		[DataSourceProperty]
		public string WallsLbl
		{
			get
			{
				return this._wallsLbl;
			}
			set
			{
				if (value != this._wallsLbl)
				{
					this._wallsLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "WallsLbl");
				}
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x060012FF RID: 4863 RVA: 0x0004D8CF File Offset: 0x0004BACF
		// (set) Token: 0x06001300 RID: 4864 RVA: 0x0004D8D7 File Offset: 0x0004BAD7
		[DataSourceProperty]
		public int WallsLevel
		{
			get
			{
				return this._wallsLevel;
			}
			set
			{
				if (value != this._wallsLevel)
				{
					this._wallsLevel = value;
					base.OnPropertyChangedWithValue(value, "WallsLevel");
				}
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06001301 RID: 4865 RVA: 0x0004D8F5 File Offset: 0x0004BAF5
		// (set) Token: 0x06001302 RID: 4866 RVA: 0x0004D8FD File Offset: 0x0004BAFD
		[DataSourceProperty]
		public string SettlementNameLbl
		{
			get
			{
				return this._settlementNameLbl;
			}
			set
			{
				if (value != this._settlementNameLbl)
				{
					this._settlementNameLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "SettlementNameLbl");
				}
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06001303 RID: 4867 RVA: 0x0004D920 File Offset: 0x0004BB20
		// (set) Token: 0x06001304 RID: 4868 RVA: 0x0004D928 File Offset: 0x0004BB28
		[DataSourceProperty]
		public bool IsFortification
		{
			get
			{
				return this._isFortification;
			}
			set
			{
				if (value != this._isFortification)
				{
					this._isFortification = value;
					base.OnPropertyChangedWithValue(value, "IsFortification");
				}
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06001305 RID: 4869 RVA: 0x0004D946 File Offset: 0x0004BB46
		// (set) Token: 0x06001306 RID: 4870 RVA: 0x0004D94E File Offset: 0x0004BB4E
		[DataSourceProperty]
		public bool IsCrimeEnabled
		{
			get
			{
				return this._isCrimeEnabled;
			}
			set
			{
				if (value != this._isCrimeEnabled)
				{
					this._isCrimeEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsCrimeEnabled");
				}
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06001307 RID: 4871 RVA: 0x0004D96C File Offset: 0x0004BB6C
		// (set) Token: 0x06001308 RID: 4872 RVA: 0x0004D974 File Offset: 0x0004BB74
		[DataSourceProperty]
		public bool IsNoGarrisonWarning
		{
			get
			{
				return this._isNoGarrisonWarning;
			}
			set
			{
				if (value != this._isNoGarrisonWarning)
				{
					this._isNoGarrisonWarning = value;
					base.OnPropertyChangedWithValue(value, "IsNoGarrisonWarning");
				}
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06001309 RID: 4873 RVA: 0x0004D992 File Offset: 0x0004BB92
		// (set) Token: 0x0600130A RID: 4874 RVA: 0x0004D99A File Offset: 0x0004BB9A
		[DataSourceProperty]
		public bool IsCrimeLabelHighlightEnabled
		{
			get
			{
				return this._isCrimeLabelHighlightEnabled;
			}
			set
			{
				if (value != this._isCrimeLabelHighlightEnabled)
				{
					this._isCrimeLabelHighlightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsCrimeLabelHighlightEnabled");
				}
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x0600130B RID: 4875 RVA: 0x0004D9B8 File Offset: 0x0004BBB8
		// (set) Token: 0x0600130C RID: 4876 RVA: 0x0004D9C0 File Offset: 0x0004BBC0
		[DataSourceProperty]
		public bool IsLoyaltyRebellionWarning
		{
			get
			{
				return this._isLoyaltyRebellionWarning;
			}
			set
			{
				if (value != this._isLoyaltyRebellionWarning)
				{
					this._isLoyaltyRebellionWarning = value;
					base.OnPropertyChangedWithValue(value, "IsLoyaltyRebellionWarning");
				}
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x0600130D RID: 4877 RVA: 0x0004D9DE File Offset: 0x0004BBDE
		// (set) Token: 0x0600130E RID: 4878 RVA: 0x0004D9E6 File Offset: 0x0004BBE6
		[DataSourceProperty]
		public bool IsShipyardEnabled
		{
			get
			{
				return this._isShipyardEnabled;
			}
			set
			{
				if (value != this._isShipyardEnabled)
				{
					this._isShipyardEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsShipyardEnabled");
				}
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x0600130F RID: 4879 RVA: 0x0004DA04 File Offset: 0x0004BC04
		// (set) Token: 0x06001310 RID: 4880 RVA: 0x0004DA0C File Offset: 0x0004BC0C
		[DataSourceProperty]
		public string ShipyardLbl
		{
			get
			{
				return this._shipyardLbl;
			}
			set
			{
				if (value != this._shipyardLbl)
				{
					this._shipyardLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "ShipyardLbl");
				}
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06001311 RID: 4881 RVA: 0x0004DA2F File Offset: 0x0004BC2F
		// (set) Token: 0x06001312 RID: 4882 RVA: 0x0004DA37 File Offset: 0x0004BC37
		[DataSourceProperty]
		public BasicTooltipViewModel ShipyardHint
		{
			get
			{
				return this._shipyardHint;
			}
			set
			{
				if (value != this._shipyardHint)
				{
					this._shipyardHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "ShipyardHint");
				}
			}
		}

		// Token: 0x0400087B RID: 2171
		protected readonly Settlement _settlement;

		// Token: 0x0400087C RID: 2172
		private GameMenu.MenuOverlayType _type;

		// Token: 0x0400087D RID: 2173
		private GameMenuOverlayActionVM _overlayTalkItem;

		// Token: 0x0400087E RID: 2174
		private GameMenuOverlayActionVM _overlayQuickTalkItem;

		// Token: 0x0400087F RID: 2175
		private Tuple<bool, TextObject> _mostRecentOverlayTalkPermission;

		// Token: 0x04000880 RID: 2176
		private Tuple<bool, TextObject> _mostRecentOverlayQuickTalkPermission;

		// Token: 0x04000881 RID: 2177
		private Tuple<bool, TextObject> _mostRecentOverlayLeaveCharacterPermission;

		// Token: 0x04000882 RID: 2178
		private string _latestTutorialElementID;

		// Token: 0x04000883 RID: 2179
		private bool _isCompanionHighlightApplied;

		// Token: 0x04000884 RID: 2180
		private bool _isQuestGiversHighlightApplied;

		// Token: 0x04000885 RID: 2181
		private bool _isNotableHighlightApplied;

		// Token: 0x04000886 RID: 2182
		private bool _isTalkItemHighlightApplied;

		// Token: 0x04000887 RID: 2183
		private string _militasLbl;

		// Token: 0x04000888 RID: 2184
		private string _garrisonLbl;

		// Token: 0x04000889 RID: 2185
		private bool _isNoGarrisonWarning;

		// Token: 0x0400088A RID: 2186
		private bool _isLoyaltyRebellionWarning;

		// Token: 0x0400088B RID: 2187
		private bool _isCrimeLabelHighlightEnabled;

		// Token: 0x0400088C RID: 2188
		private string _crimeLbl;

		// Token: 0x0400088D RID: 2189
		private string _prosperityLbl;

		// Token: 0x0400088E RID: 2190
		private string _loyaltyLbl;

		// Token: 0x0400088F RID: 2191
		private string _securityLbl;

		// Token: 0x04000890 RID: 2192
		private string _wallsLbl;

		// Token: 0x04000891 RID: 2193
		private string _settlementNameLbl;

		// Token: 0x04000892 RID: 2194
		private string _remainingFoodText = "";

		// Token: 0x04000893 RID: 2195
		private int _wallsLevel;

		// Token: 0x04000894 RID: 2196
		private int _prosperityChangeAmount;

		// Token: 0x04000895 RID: 2197
		private int _militiaChangeAmount;

		// Token: 0x04000896 RID: 2198
		private int _garrisonChangeAmount;

		// Token: 0x04000897 RID: 2199
		private int _garrisonAmount;

		// Token: 0x04000898 RID: 2200
		private int _crimeChangeAmount;

		// Token: 0x04000899 RID: 2201
		private int _loyaltyChangeAmount;

		// Token: 0x0400089A RID: 2202
		private int _securityChangeAmount;

		// Token: 0x0400089B RID: 2203
		private int _foodChangeAmount;

		// Token: 0x0400089C RID: 2204
		private MBBindingList<GameMenuPartyItemVM> _characterList;

		// Token: 0x0400089D RID: 2205
		private MBBindingList<GameMenuPartyItemVM> _partyList;

		// Token: 0x0400089E RID: 2206
		private MBBindingList<StringItemWithHintVM> _issueList;

		// Token: 0x0400089F RID: 2207
		private bool _isFortification;

		// Token: 0x040008A0 RID: 2208
		private bool _isCrimeEnabled;

		// Token: 0x040008A1 RID: 2209
		private bool _canAssignMembers;

		// Token: 0x040008A2 RID: 2210
		private BasicTooltipViewModel _remainingFoodHint;

		// Token: 0x040008A3 RID: 2211
		private BasicTooltipViewModel _militasHint;

		// Token: 0x040008A4 RID: 2212
		private BasicTooltipViewModel _garrisonHint;

		// Token: 0x040008A5 RID: 2213
		private BasicTooltipViewModel _prosperityHint;

		// Token: 0x040008A6 RID: 2214
		private BasicTooltipViewModel _loyaltyHint;

		// Token: 0x040008A7 RID: 2215
		private BasicTooltipViewModel _securityHint;

		// Token: 0x040008A8 RID: 2216
		private BasicTooltipViewModel _wallsHint;

		// Token: 0x040008A9 RID: 2217
		private BasicTooltipViewModel _crimeHint;

		// Token: 0x040008AA RID: 2218
		private HintViewModel _characterFilterHint;

		// Token: 0x040008AB RID: 2219
		private HintViewModel _partyFilterHint;

		// Token: 0x040008AC RID: 2220
		private HintViewModel _assignMembersHint;

		// Token: 0x040008AD RID: 2221
		private BannerImageIdentifierVM _settlementOwnerBanner;

		// Token: 0x040008AE RID: 2222
		private ClanCardSelectionPopupVM _cardSelectionPopup;

		// Token: 0x040008AF RID: 2223
		private bool _isShipyardEnabled;

		// Token: 0x040008B0 RID: 2224
		private string _shipyardLbl;

		// Token: 0x040008B1 RID: 2225
		private BasicTooltipViewModel _shipyardHint;

		// Token: 0x02000236 RID: 566
		private class CharacterComparer : IComparer<GameMenuPartyItemVM>
		{
			// Token: 0x060024E3 RID: 9443 RVA: 0x00081032 File Offset: 0x0007F232
			public int Compare(GameMenuPartyItemVM x, GameMenuPartyItemVM y)
			{
				return CampaignUIHelper.GetHeroCompareSortIndex(x.Character.HeroObject, y.Character.HeroObject);
			}
		}

		// Token: 0x02000237 RID: 567
		private class PartyComparer : IComparer<GameMenuPartyItemVM>
		{
			// Token: 0x060024E5 RID: 9445 RVA: 0x00081057 File Offset: 0x0007F257
			public int Compare(GameMenuPartyItemVM x, GameMenuPartyItemVM y)
			{
				return CampaignUIHelper.MobilePartyPrecedenceComparerInstance.Compare(x.Party.MobileParty, y.Party.MobileParty);
			}
		}
	}
}
