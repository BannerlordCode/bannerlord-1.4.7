using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Overlay
{
	// Token: 0x020000B7 RID: 183
	public class GameMenuOverlay : ViewModel
	{
		// Token: 0x06001230 RID: 4656 RVA: 0x00049986 File Offset: 0x00047B86
		public GameMenuOverlay()
		{
			this.ContextList = new MBBindingList<StringItemWithEnabledAndHintVM>();
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x000499A7 File Offset: 0x00047BA7
		public override void RefreshValues()
		{
			base.RefreshValues();
			GameMenuPartyItemVM contextMenuItem = this._contextMenuItem;
			if (contextMenuItem == null)
			{
				return;
			}
			contextMenuItem.RefreshValues();
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x000499BF File Offset: 0x00047BBF
		protected virtual void ExecuteOnSetAsActiveContextMenuItem(GameMenuPartyItemVM troop)
		{
			this._contextMenuItem = troop;
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x000499C8 File Offset: 0x00047BC8
		public virtual void ExecuteOnOverlayClosed()
		{
			if (!this._closedHandled)
			{
				CampaignEventDispatcher.Instance.OnCharacterPortraitPopUpClosed();
				this._closedHandled = true;
			}
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x000499E3 File Offset: 0x00047BE3
		public virtual void ExecuteOnOverlayOpened()
		{
			this._closedHandled = false;
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x000499EC File Offset: 0x00047BEC
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (!this._closedHandled)
			{
				this.ExecuteOnOverlayClosed();
			}
			InputKeyItemVM exitInputKey = this.ExitInputKey;
			if (exitInputKey == null)
			{
				return;
			}
			exitInputKey.OnFinalize();
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00049A14 File Offset: 0x00047C14
		protected void ExecuteTroopAction(object o)
		{
			switch ((GameMenuOverlay.MenuOverlayContextList)o)
			{
			case GameMenuOverlay.MenuOverlayContextList.Encyclopedia:
				if (this._contextMenuItem.Character != null)
				{
					if (this._contextMenuItem.Character.IsHero)
					{
						Campaign.Current.EncyclopediaManager.GoToLink(this._contextMenuItem.Character.HeroObject.EncyclopediaLink);
					}
					else
					{
						Debug.FailedAssert("Character object in menu overlay", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\GameMenu\\Overlay\\GameMenuOverlay.cs", "ExecuteTroopAction", 101);
						Campaign.Current.EncyclopediaManager.GoToLink(this._contextMenuItem.Character.EncyclopediaLink);
					}
				}
				else if (this._contextMenuItem.Party != null)
				{
					CharacterObject visualPartyLeader = CampaignUIHelper.GetVisualPartyLeader(this._contextMenuItem.Party);
					if (visualPartyLeader != null)
					{
						Campaign.Current.EncyclopediaManager.GoToLink(visualPartyLeader.EncyclopediaLink);
					}
				}
				else if (this._contextMenuItem.Settlement != null)
				{
					Campaign.Current.EncyclopediaManager.GoToLink(this._contextMenuItem.Settlement.EncyclopediaLink);
				}
				break;
			case GameMenuOverlay.MenuOverlayContextList.Conversation:
				if (this._contextMenuItem.Character != null)
				{
					if (this._contextMenuItem.Character.IsHero)
					{
						if (PlayerEncounter.Current != null || LocationComplex.Current != null || Campaign.Current.CurrentMenuContext != null)
						{
							Location location = LocationComplex.Current.GetLocationOfCharacter(this._contextMenuItem.Character.HeroObject);
							if (location.StringId == "alley")
							{
								location = LocationComplex.Current.GetLocationWithId("center");
							}
							CampaignEventDispatcher.Instance.OnPlayerStartTalkFromMenu(this._contextMenuItem.Character.HeroObject);
							PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(location, null, this._contextMenuItem.Character, null);
						}
						else
						{
							EncounterManager.StartPartyEncounter(PartyBase.MainParty, this._contextMenuItem.Character.HeroObject.PartyBelongedTo.Party);
						}
					}
					else
					{
						Debug.FailedAssert("Character object in menu overlay", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\GameMenu\\Overlay\\GameMenuOverlay.cs", "ExecuteTroopAction", 145);
					}
				}
				break;
			case GameMenuOverlay.MenuOverlayContextList.QuickConversation:
				if (this._contextMenuItem.Character != null)
				{
					if (this._contextMenuItem.Character.IsHero)
					{
						if (PlayerEncounter.Current != null || LocationComplex.Current != null || Campaign.Current.CurrentMenuContext != null)
						{
							CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, false, false, false, true, false, false), new ConversationCharacterData(this._contextMenuItem.Character, null, false, false, false, true, false, false));
						}
						else
						{
							EncounterManager.StartPartyEncounter(PartyBase.MainParty, this._contextMenuItem.Character.HeroObject.PartyBelongedTo.Party);
						}
					}
					else
					{
						Debug.FailedAssert("Character object in menu overlay", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\GameMenu\\Overlay\\GameMenuOverlay.cs", "ExecuteTroopAction", 168);
					}
				}
				break;
			case GameMenuOverlay.MenuOverlayContextList.ConverseWithLeader:
			{
				PartyBase party = this._contextMenuItem.Party;
				if (((party != null) ? party.LeaderHero : null) != null)
				{
					if (Settlement.CurrentSettlement != null || LocationComplex.Current != null || Campaign.Current.CurrentMenuContext != null)
					{
						this.ConverseWithLeader(PartyBase.MainParty, this._contextMenuItem.Party);
					}
					else
					{
						EncounterManager.StartPartyEncounter(PartyBase.MainParty, this._contextMenuItem.Party);
					}
				}
				break;
			}
			case GameMenuOverlay.MenuOverlayContextList.ArmyDismiss:
			{
				PartyBase party2 = this._contextMenuItem.Party;
				if (((party2 != null) ? party2.MobileParty.Army : null) != null && this._contextMenuItem.Party.MapEvent == null && this._contextMenuItem.Party.MobileParty.Army.LeaderParty != this._contextMenuItem.Party.MobileParty)
				{
					if (this._contextMenuItem.Party.MobileParty.Army.LeaderParty == MobileParty.MainParty && this._contextMenuItem.Party.MobileParty.Army.Parties.Count <= 2)
					{
						DisbandArmyAction.ApplyByNotEnoughParty(this._contextMenuItem.Party.MobileParty.Army);
					}
					else
					{
						this._contextMenuItem.Party.MobileParty.Army = null;
						this._contextMenuItem.Party.MobileParty.SetMoveModeHold();
					}
				}
				break;
			}
			case GameMenuOverlay.MenuOverlayContextList.ManageGarrison:
				if (this._contextMenuItem.Party != null)
				{
					PartyScreenHelper.OpenScreenAsManageTroops(this._contextMenuItem.Party.MobileParty);
				}
				break;
			case GameMenuOverlay.MenuOverlayContextList.DonateTroops:
				if (this._contextMenuItem.Party != null)
				{
					if (this._contextMenuItem.Party.MobileParty.IsGarrison)
					{
						PartyScreenHelper.OpenScreenAsDonateGarrisonWithCurrentSettlement();
					}
					else
					{
						PartyScreenHelper.OpenScreenAsDonateTroops(this._contextMenuItem.Party.MobileParty);
					}
				}
				break;
			case GameMenuOverlay.MenuOverlayContextList.JoinArmy:
			{
				CharacterObject character = this._contextMenuItem.Character;
				if (character != null && character.IsHero && this._contextMenuItem.Character.HeroObject.PartyBelongedTo != null)
				{
					MobileParty.MainParty.Army = this._contextMenuItem.Character.HeroObject.PartyBelongedTo.Army;
					MobileParty.MainParty.Army.AddPartyToMergedParties(MobileParty.MainParty);
					MenuContext currentMenuContext = Campaign.Current.CurrentMenuContext;
					if (currentMenuContext != null)
					{
						currentMenuContext.Refresh();
					}
				}
				break;
			}
			case GameMenuOverlay.MenuOverlayContextList.TakeToParty:
			{
				CharacterObject character2 = this._contextMenuItem.Character;
				if (character2 != null && character2.IsHero && this._contextMenuItem.Character.HeroObject.PartyBelongedTo == null)
				{
					Settlement currentSettlement = this._contextMenuItem.Character.HeroObject.CurrentSettlement;
					bool flag;
					if (currentSettlement == null)
					{
						flag = false;
					}
					else
					{
						MBReadOnlyList<Hero> notables = currentSettlement.Notables;
						bool? flag2 = ((notables != null) ? new bool?(notables.Contains(this._contextMenuItem.Character.HeroObject)) : null);
						bool flag3 = true;
						flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
					}
					if (flag)
					{
						LeaveSettlementAction.ApplyForCharacterOnly(this._contextMenuItem.Character.HeroObject);
					}
					AddHeroToPartyAction.Apply(this._contextMenuItem.Character.HeroObject, MobileParty.MainParty, true);
				}
				break;
			}
			case GameMenuOverlay.MenuOverlayContextList.ManageTroops:
			{
				PartyBase party3 = this._contextMenuItem.Party;
				if (((party3 != null) ? party3.MobileParty : null) != null && this._contextMenuItem.Party.MobileParty.ActualClan == Clan.PlayerClan)
				{
					PartyScreenHelper.OpenScreenAsManageTroopsAndPrisoners(this._contextMenuItem.Party.MobileParty, new PartyScreenClosedDelegate(PartyScreenHelper.OpenScreenAsManagePlayerClanPartyClosed));
				}
				break;
			}
			}
			if (!this._closedHandled)
			{
				CampaignEventDispatcher.Instance.OnCharacterPortraitPopUpClosed();
				this._closedHandled = true;
			}
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0004A0A8 File Offset: 0x000482A8
		private void ConverseWithLeader(PartyBase mainParty1, PartyBase party2)
		{
			bool flag;
			if (mainParty1.Side != BattleSideEnum.Attacker)
			{
				PlayerEncounter playerEncounter = PlayerEncounter.Current;
				flag = playerEncounter != null && playerEncounter.PlayerSide == BattleSideEnum.Attacker;
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (LocationComplex.Current != null && !flag2)
			{
				Location locationOfCharacter = LocationComplex.Current.GetLocationOfCharacter(party2.LeaderHero);
				CampaignEventDispatcher.Instance.OnPlayerStartTalkFromMenu(party2.LeaderHero);
				PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(locationOfCharacter, null, party2.LeaderHero.CharacterObject, null);
				return;
			}
			ConversationCharacterData conversationCharacterData = new ConversationCharacterData(CharacterObject.PlayerCharacter, mainParty1, false, false, false, false, false, false);
			ConversationCharacterData conversationCharacterData2 = new ConversationCharacterData(ConversationHelper.GetConversationCharacterPartyLeader(party2), party2, false, false, false, false, false, false);
			if (PartyBase.MainParty.MobileParty.IsCurrentlyAtSea)
			{
				CampaignMission.OpenConversationMission(conversationCharacterData, conversationCharacterData2, "", "", false);
				return;
			}
			CampaignMapConversation.OpenConversation(conversationCharacterData, conversationCharacterData2);
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0004A170 File Offset: 0x00048370
		public virtual void Refresh()
		{
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x0004A172 File Offset: 0x00048372
		public virtual void UpdateOverlayType(GameMenu.MenuOverlayType newType)
		{
			this.Refresh();
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x0004A17A File Offset: 0x0004837A
		public virtual void OnFrameTick(float dt)
		{
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x0004A17C File Offset: 0x0004837C
		public void HourlyTick()
		{
			this.Refresh();
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x0600123C RID: 4668 RVA: 0x0004A184 File Offset: 0x00048384
		// (set) Token: 0x0600123D RID: 4669 RVA: 0x0004A18C File Offset: 0x0004838C
		[DataSourceProperty]
		public bool IsContextMenuEnabled
		{
			get
			{
				return this._isContextMenuEnabled;
			}
			set
			{
				this._isContextMenuEnabled = value;
				base.OnPropertyChangedWithValue(value, "IsContextMenuEnabled");
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x0600123E RID: 4670 RVA: 0x0004A1A1 File Offset: 0x000483A1
		// (set) Token: 0x0600123F RID: 4671 RVA: 0x0004A1A9 File Offset: 0x000483A9
		[DataSourceProperty]
		public bool IsInitializationOver
		{
			get
			{
				return this._isInitializationOver;
			}
			set
			{
				this._isInitializationOver = value;
				base.OnPropertyChangedWithValue(value, "IsInitializationOver");
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001240 RID: 4672 RVA: 0x0004A1BE File Offset: 0x000483BE
		// (set) Token: 0x06001241 RID: 4673 RVA: 0x0004A1C6 File Offset: 0x000483C6
		[DataSourceProperty]
		public bool IsInfoBarExtended
		{
			get
			{
				return this._isInfoBarExtended;
			}
			set
			{
				this._isInfoBarExtended = value;
				base.OnPropertyChangedWithValue(value, "IsInfoBarExtended");
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001242 RID: 4674 RVA: 0x0004A1DB File Offset: 0x000483DB
		// (set) Token: 0x06001243 RID: 4675 RVA: 0x0004A1E3 File Offset: 0x000483E3
		[DataSourceProperty]
		public MBBindingList<StringItemWithEnabledAndHintVM> ContextList
		{
			get
			{
				return this._contextList;
			}
			set
			{
				if (value != this._contextList)
				{
					this._contextList = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringItemWithEnabledAndHintVM>>(value, "ContextList");
				}
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001244 RID: 4676 RVA: 0x0004A201 File Offset: 0x00048401
		// (set) Token: 0x06001245 RID: 4677 RVA: 0x0004A209 File Offset: 0x00048409
		[DataSourceProperty]
		public int CurrentOverlayType
		{
			get
			{
				return this._currentOverlayType;
			}
			set
			{
				if (value != this._currentOverlayType)
				{
					this._currentOverlayType = value;
					base.OnPropertyChangedWithValue(value, "CurrentOverlayType");
				}
			}
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x0004A227 File Offset: 0x00048427
		public void SetExitInputKey(HotKey hotKey)
		{
			this.ExitInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001247 RID: 4679 RVA: 0x0004A236 File Offset: 0x00048436
		// (set) Token: 0x06001248 RID: 4680 RVA: 0x0004A23E File Offset: 0x0004843E
		[DataSourceProperty]
		public InputKeyItemVM ExitInputKey
		{
			get
			{
				return this._exitInputKey;
			}
			set
			{
				if (value != this._exitInputKey)
				{
					this._exitInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ExitInputKey");
				}
			}
		}

		// Token: 0x04000850 RID: 2128
		public string GameMenuOverlayName;

		// Token: 0x04000851 RID: 2129
		private bool _closedHandled = true;

		// Token: 0x04000852 RID: 2130
		private bool _isContextMenuEnabled;

		// Token: 0x04000853 RID: 2131
		private int _currentOverlayType = -1;

		// Token: 0x04000854 RID: 2132
		private bool _isInfoBarExtended;

		// Token: 0x04000855 RID: 2133
		private bool _isInitializationOver;

		// Token: 0x04000856 RID: 2134
		private MBBindingList<StringItemWithEnabledAndHintVM> _contextList;

		// Token: 0x04000857 RID: 2135
		protected GameMenuPartyItemVM _contextMenuItem;

		// Token: 0x04000858 RID: 2136
		private InputKeyItemVM _exitInputKey;

		// Token: 0x02000232 RID: 562
		protected internal enum MenuOverlayContextList
		{
			// Token: 0x0400122B RID: 4651
			Encyclopedia,
			// Token: 0x0400122C RID: 4652
			Conversation,
			// Token: 0x0400122D RID: 4653
			QuickConversation,
			// Token: 0x0400122E RID: 4654
			ConverseWithLeader,
			// Token: 0x0400122F RID: 4655
			ArmyDismiss,
			// Token: 0x04001230 RID: 4656
			ManageGarrison,
			// Token: 0x04001231 RID: 4657
			DonateTroops,
			// Token: 0x04001232 RID: 4658
			JoinArmy,
			// Token: 0x04001233 RID: 4659
			TakeToParty,
			// Token: 0x04001234 RID: 4660
			ManageTroops
		}
	}
}
