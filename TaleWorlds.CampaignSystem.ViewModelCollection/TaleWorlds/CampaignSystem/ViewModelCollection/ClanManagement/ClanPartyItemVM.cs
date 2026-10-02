using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x02000129 RID: 297
	public class ClanPartyItemVM : ViewModel
	{
		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06001B89 RID: 7049 RVA: 0x00066462 File Offset: 0x00064662
		// (set) Token: 0x06001B8A RID: 7050 RVA: 0x0006646A File Offset: 0x0006466A
		public int Expense { get; private set; }

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06001B8B RID: 7051 RVA: 0x00066473 File Offset: 0x00064673
		// (set) Token: 0x06001B8C RID: 7052 RVA: 0x0006647B File Offset: 0x0006467B
		public int Income { get; private set; }

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06001B8D RID: 7053 RVA: 0x00066484 File Offset: 0x00064684
		public PartyBase Party { get; }

		// Token: 0x06001B8E RID: 7054 RVA: 0x0006648C File Offset: 0x0006468C
		public ClanPartyItemVM(PartyBase party, Action<ClanPartyItemVM> onAssignment, Action onExpenseChange, Action onShowChangeLeaderPopup, ClanPartyItemVM.ClanPartyType type, IDisbandPartyCampaignBehavior disbandBehavior, ITeleportationCampaignBehavior teleportationBehavior)
		{
			this.Party = party;
			this._type = type;
			this._disbandBehavior = disbandBehavior;
			this._leader = CampaignUIHelper.GetVisualPartyLeader(this.Party);
			this.HasHeroMembers = party.IsMobile;
			if (this._leader == null)
			{
				TroopRosterElement troopRosterElement = this.Party.MemberRoster.GetTroopRoster().FirstOrDefault<TroopRosterElement>();
				if (!troopRosterElement.Equals(default(TroopRosterElement)))
				{
					this._leader = troopRosterElement.Character;
				}
				else
				{
					IFaction mapFaction = this.Party.MapFaction;
					this._leader = ((mapFaction != null) ? mapFaction.BasicTroop : null);
				}
			}
			CharacterObject leader = this._leader;
			if ((leader == null || !leader.IsHero) && party.IsMobile && (this._type == ClanPartyItemVM.ClanPartyType.Member || this._type == ClanPartyItemVM.ClanPartyType.Caravan))
			{
				Hero teleportingLeaderHero = CampaignUIHelper.GetTeleportingLeaderHero(party.MobileParty, teleportationBehavior);
				this._leader = ((teleportingLeaderHero != null) ? teleportingLeaderHero.CharacterObject : null);
				this._isLeaderTeleporting = this._leader != null;
			}
			if (this._leader != null)
			{
				CharacterCode characterCode = ClanPartyItemVM.GetCharacterCode(this._leader);
				this.LeaderVisual = new CharacterImageIdentifierVM(characterCode);
				this.CharacterModel = new CharacterViewModel(CharacterViewModel.StanceTypes.None);
				CharacterViewModel characterModel = this.CharacterModel;
				BasicCharacterObject leader2 = this._leader;
				int num = -1;
				Banner banner = this.Party.Banner;
				characterModel.FillFrom(leader2, num, (banner != null) ? banner.BannerCode : null);
				CharacterViewModel characterModel2 = this.CharacterModel;
				IFaction mapFaction2 = this.Party.MapFaction;
				characterModel2.ArmorColor1 = ((mapFaction2 != null) ? mapFaction2.Color : 0U);
				CharacterViewModel characterModel3 = this.CharacterModel;
				IFaction mapFaction3 = this.Party.MapFaction;
				characterModel3.ArmorColor2 = ((mapFaction3 != null) ? mapFaction3.Color2 : 0U);
			}
			else
			{
				this.LeaderVisual = new CharacterImageIdentifierVM(null);
				this.CharacterModel = new CharacterViewModel();
			}
			this._onAssignment = onAssignment;
			this._onExpenseChange = onExpenseChange;
			this._onShowChangeLeaderPopup = onShowChangeLeaderPopup;
			bool flag;
			if (!this.Party.MobileParty.IsDisbanding)
			{
				IDisbandPartyCampaignBehavior disbandBehavior2 = this._disbandBehavior;
				flag = disbandBehavior2 != null && disbandBehavior2.IsPartyWaitingForDisband(party.MobileParty);
			}
			else
			{
				flag = true;
			}
			this.IsDisbanding = flag;
			bool flag2 = !party.MobileParty.IsMilitia && !party.MobileParty.IsVillager && party.MobileParty.IsActive && !this.IsDisbanding;
			this.ShouldPartyHaveExpense = flag2 && (type == ClanPartyItemVM.ClanPartyType.Garrison || type == ClanPartyItemVM.ClanPartyType.Member);
			this.IsCaravan = type == ClanPartyItemVM.ClanPartyType.Caravan;
			TextObject empty = TextObject.GetEmpty();
			this.IsChangeLeaderVisible = type == ClanPartyItemVM.ClanPartyType.Caravan || type == ClanPartyItemVM.ClanPartyType.Member;
			this.IsChangeLeaderEnabled = this.IsChangeLeaderVisible && CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out empty);
			this.ChangeLeaderHint = new HintViewModel(this.IsChangeLeaderEnabled ? this._changeLeaderHintText : empty, null);
			if (this.ShouldPartyHaveExpense)
			{
				if (party.MobileParty != null)
				{
					this.ExpenseItem = new ClanFinanceExpenseItemVM(party.MobileParty);
					this.OnExpenseChange();
				}
				else
				{
					Debug.FailedAssert("This party should have expense info but it doesn't", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\ClanManagement\\ClanPartyItemVM.cs", ".ctor", 116);
				}
			}
			if (this.IsCaravan)
			{
				this.Income = Campaign.Current.Models.ClanFinanceModel.CalculateOwnerIncomeFromCaravan(party.MobileParty);
			}
			this.AutoRecruitmentHint = new HintViewModel(GameTexts.FindText("str_clan_auto_recruitment_hint", null), null);
			this.IsAutoRecruitmentVisible = party.MobileParty.IsGarrison;
			this.AutoRecruitmentValue = party.MobileParty.IsGarrison && this.Party.MobileParty.CurrentSettlement.Town.GarrisonAutoRecruitmentIsEnabled;
			this.HeroMembers = new MBBindingList<ClanPartyMemberItemVM>();
			this.Roles = new MBBindingList<ClanRoleItemVM>();
			this.InfantryHint = new BasicTooltipViewModel(() => this.GetPartyTroopInfo(this.Party, FormationClass.Infantry));
			this.CavalryHint = new BasicTooltipViewModel(() => this.GetPartyTroopInfo(this.Party, FormationClass.Cavalry));
			this.RangedHint = new BasicTooltipViewModel(() => this.GetPartyTroopInfo(this.Party, FormationClass.Ranged));
			this.HorseArcherHint = new BasicTooltipViewModel(() => this.GetPartyTroopInfo(this.Party, FormationClass.HorseArcher));
			this.ActionsDisabledHint = new HintViewModel();
			this.InArmyHint = new HintViewModel();
			this.RefreshValues();
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x00066891 File Offset: 0x00064A91
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.UpdateProperties();
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x000668A0 File Offset: 0x00064AA0
		public void UpdateProperties()
		{
			this.MembersText = GameTexts.FindText("str_members", null).ToString();
			this.AssigneesText = GameTexts.FindText("str_clan_assignee_title", null).ToString();
			this.RolesText = GameTexts.FindText("str_clan_role_title", null).ToString();
			this.PartyLeaderRoleEffectsText = GameTexts.FindText("str_clan_party_leader_roles_and_effects", null).ToString();
			this.AutoRecruitmentText = GameTexts.FindText("str_clan_auto_recruitment", null).ToString();
			PartyBase party = this.Party;
			this.IsPartyBehaviorEnabled = ((party != null) ? party.LeaderHero : null) != null && this.Party.LeaderHero.Clan.Leader != this.Party.LeaderHero && !this.Party.MobileParty.IsCaravan && !this.IsDisbanding;
			if (this.Party == PartyBase.MainParty && Hero.MainHero.IsPrisoner)
			{
				TextObject textObject = new TextObject("{=shL0WElC}{TROOP.NAME}{.o} Party", null);
				textObject.SetCharacterProperties("TROOP", Hero.MainHero.CharacterObject, false);
				this.Name = textObject.ToString();
			}
			else if (this._isLeaderTeleporting)
			{
				TextObject textObject2 = new TextObject("{=P5YtNXHR}{LEADER.NAME}{.o} Party", null);
				StringHelpers.SetCharacterProperties("LEADER", this._leader, textObject2, false);
				this.Name = textObject2.ToString();
			}
			else
			{
				this.Name = this.Party.Name.ToString();
			}
			this.IsMainHeroParty = this._type == ClanPartyItemVM.ClanPartyType.Main;
			this.PartyLocationText = CampaignUIHelper.GetPartyLocationText(this.Party.MobileParty);
			GameTexts.SetVariable("LEFT", this.Party.MobileParty.MemberRoster.TotalManCount);
			GameTexts.SetVariable("RIGHT", this.Party.MobileParty.Party.PartySizeLimit);
			string text = GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString();
			string text2 = GameTexts.FindText("str_party_morale_party_size", null).ToString();
			this.PartySizeText = text;
			GameTexts.SetVariable("LEFT", text2);
			GameTexts.SetVariable("RIGHT", text);
			this.PartySizeSubTitleText = GameTexts.FindText("str_LEFT_colon_RIGHT", null).ToString();
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_party_wage", null));
			GameTexts.SetVariable("RIGHT", this.Party.MobileParty.TotalWage);
			this.PartyWageSubTitleText = GameTexts.FindText("str_LEFT_colon_RIGHT", null).ToString();
			this.InArmyText = "";
			if (this.Party.MobileParty.Army != null)
			{
				this.IsInArmy = true;
				TextObject textObject3 = GameTexts.FindText("str_clan_in_army_hint", null);
				TextObject textObject4 = textObject3;
				string text3 = "ARMY_LEADER";
				MobileParty leaderParty = this.Party.MobileParty.Army.LeaderParty;
				string text4;
				if (leaderParty == null)
				{
					text4 = null;
				}
				else
				{
					Hero leaderHero = leaderParty.LeaderHero;
					text4 = ((leaderHero != null) ? leaderHero.Name.ToString() : null);
				}
				textObject4.SetTextVariable(text3, text4 ?? string.Empty);
				this.InArmyHint = new HintViewModel(textObject3, null);
				this.InArmyText = GameTexts.FindText("str_in_army", null).ToString();
			}
			this.DisbandingText = "";
			this.IsMembersAndRolesVisible = !this.IsDisbanding && this._type != ClanPartyItemVM.ClanPartyType.Garrison;
			if (this.IsDisbanding)
			{
				this.DisbandingText = GameTexts.FindText("str_disbanding", null).ToString();
			}
			this.PartyBehaviorText = "";
			if (this.IsPartyBehaviorEnabled)
			{
				this.PartyBehaviorSelector = new ClanPartyBehaviorSelectorVM(0, new Action<SelectorVM<SelectorItemVM>>(this.UpdatePartyBehaviorSelectionUpdate));
				for (int i = 0; i < 3; i++)
				{
					string text5 = GameTexts.FindText("str_clan_party_objective", i.ToString()).ToString();
					TextObject textObject5 = GameTexts.FindText("str_clan_party_objective_hint", i.ToString());
					this.PartyBehaviorSelector.AddItem(new SelectorItemVM(text5, textObject5));
				}
				this.PartyBehaviorSelector.SelectedIndex = (int)this.Party.MobileParty.Objective;
				this.PartyBehaviorText = GameTexts.FindText("str_clan_party_behavior", null).ToString();
			}
			if (this._leader != null)
			{
				CharacterViewModel characterModel = this.CharacterModel;
				BasicCharacterObject leader = this._leader;
				int num = -1;
				Banner banner = this.Party.Banner;
				characterModel.FillFrom(leader, num, (banner != null) ? banner.BannerCode : null);
				CharacterViewModel characterModel2 = this.CharacterModel;
				IFaction mapFaction = this.Party.MapFaction;
				characterModel2.ArmorColor1 = ((mapFaction != null) ? mapFaction.Color : 0U);
				CharacterViewModel characterModel3 = this.CharacterModel;
				IFaction mapFaction2 = this.Party.MapFaction;
				characterModel3.ArmorColor2 = ((mapFaction2 != null) ? mapFaction2.Color2 : 0U);
			}
			this.HeroMembers.Clear();
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			foreach (TroopRosterElement troopRosterElement in this.Party.MemberRoster.GetTroopRoster())
			{
				Hero heroObject = troopRosterElement.Character.HeroObject;
				if (heroObject != null && heroObject.Clan == Clan.PlayerClan && heroObject.GovernorOf == null)
				{
					ClanPartyMemberItemVM clanPartyMemberItemVM = new ClanPartyMemberItemVM(troopRosterElement.Character.HeroObject, this.Party.MobileParty);
					this.HeroMembers.Add(clanPartyMemberItemVM);
					if (clanPartyMemberItemVM.IsLeader)
					{
						this.LeaderMember = clanPartyMemberItemVM;
					}
				}
				else if (troopRosterElement.Character.DefaultFormationClass.Equals(FormationClass.Infantry))
				{
					num2 += troopRosterElement.Number;
				}
				else if (troopRosterElement.Character.DefaultFormationClass.Equals(FormationClass.Ranged))
				{
					num3 += troopRosterElement.Number;
				}
				else if (troopRosterElement.Character.DefaultFormationClass.Equals(FormationClass.Cavalry))
				{
					num4 += troopRosterElement.Number;
				}
				else if (troopRosterElement.Character.DefaultFormationClass.Equals(FormationClass.HorseArcher))
				{
					num5 += troopRosterElement.Number;
				}
			}
			if (this._isLeaderTeleporting)
			{
				ClanPartyMemberItemVM clanPartyMemberItemVM2 = new ClanPartyMemberItemVM(this._leader.HeroObject, this.Party.MobileParty);
				this.LeaderMember = clanPartyMemberItemVM2;
				this.HeroMembers.Insert(0, clanPartyMemberItemVM2);
			}
			this.HasCompanion = this.HeroMembers.Count > 1;
			if (this.IsMembersAndRolesVisible)
			{
				this.Roles.ApplyActionOnAllItems(delegate(ClanRoleItemVM x)
				{
					x.OnFinalize();
				});
				this.Roles.Clear();
				foreach (PartyRole partyRole in Campaign.Current.Models.ClanMemberPartyRoleModel.GetAssignablePartyRoles())
				{
					this.Roles.Add(new ClanRoleItemVM(this.Party.MobileParty, partyRole, this.HeroMembers, new Action<ClanRoleItemVM>(this.OnRoleSelectionToggled), new Action(this.OnRoleAssigned)));
				}
			}
			this.InfantryCount = num2;
			this.RangedCount = num3;
			this.CavalryCount = num4;
			this.HorseArcherCount = num5;
			TextObject textObject6;
			this.CanUseActions = CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject6);
			this.ActionsDisabledHint.HintText = (this.CanUseActions ? TextObject.GetEmpty() : textObject6);
			if (!this.CanUseActions)
			{
				this.AutoRecruitmentHint.HintText = this.ActionsDisabledHint.HintText;
				if (this.ExpenseItem != null)
				{
					this.ExpenseItem.IsEnabled = this.CanUseActions;
					this.ExpenseItem.WageLimitHint.HintText = this.ActionsDisabledHint.HintText;
				}
				foreach (ClanRoleItemVM clanRoleItemVM in this.Roles)
				{
					clanRoleItemVM.SetEnabled(false, this.ActionsDisabledHint.HintText);
				}
			}
			if (this.PartyBehaviorSelector != null)
			{
				this.PartyBehaviorSelector.CanUseActions = this.CanUseActions;
				this.PartyBehaviorSelector.ActionsDisabledHint.HintText = this.ActionsDisabledHint.HintText;
			}
			this.ShipCount = this.Party.Ships.Count;
			this.ShipCountText = GameTexts.FindText("str_LEFT_colon_RIGHT", null).SetTextVariable("LEFT", new TextObject("{=URbKirPS}Ship Count", null).ToString()).SetTextVariable("RIGHT", this.ShipCount)
				.ToString();
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x00067150 File Offset: 0x00065350
		private void OnExpenseChange()
		{
			this._onExpenseChange();
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x00067160 File Offset: 0x00065360
		public void OnPartySelection()
		{
			int num = (this.IsPartyBehaviorEnabled ? this.PartyBehaviorSelector.SelectedIndex : (-1));
			this._onAssignment(this);
			if (this.IsPartyBehaviorEnabled)
			{
				this.PartyBehaviorSelector.SelectedIndex = num;
			}
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x000671A4 File Offset: 0x000653A4
		public void ExecuteChangeLeader()
		{
			Action onShowChangeLeaderPopup = this._onShowChangeLeaderPopup;
			if (onShowChangeLeaderPopup == null)
			{
				return;
			}
			onShowChangeLeaderPopup();
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x000671B6 File Offset: 0x000653B6
		private void OnRoleAssigned()
		{
			this.Roles.ApplyActionOnAllItems(delegate(ClanRoleItemVM x)
			{
				x.Refresh();
			});
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x000671E2 File Offset: 0x000653E2
		private void ExecuteLocationLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x000671F4 File Offset: 0x000653F4
		private void UpdatePartyBehaviorSelectionUpdate(SelectorVM<SelectorItemVM> s)
		{
			if (s.SelectedIndex != (int)this.Party.MobileParty.Objective)
			{
				this.Party.MobileParty.SetPartyObjective((MobileParty.PartyObjective)s.SelectedIndex);
			}
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x00067224 File Offset: 0x00065424
		private void OnAutoRecruitChanged(bool value)
		{
			if (this.Party.IsMobile && this.Party.MobileParty.IsGarrison)
			{
				Settlement homeSettlement = this.Party.MobileParty.HomeSettlement;
				if (((homeSettlement != null) ? homeSettlement.Town : null) != null)
				{
					this.Party.MobileParty.HomeSettlement.Town.GarrisonAutoRecruitmentIsEnabled = value;
				}
			}
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x00067289 File Offset: 0x00065489
		private void OnRoleSelectionToggled(ClanRoleItemVM role)
		{
			this.LastOpenedRoleSelection = role;
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x00067294 File Offset: 0x00065494
		private static CharacterCode GetCharacterCode(CharacterObject character)
		{
			if (character.IsHero)
			{
				return CampaignUIHelper.GetCharacterCode(character, false);
			}
			uint color = Hero.MainHero.MapFaction.Color;
			uint color2 = Hero.MainHero.MapFaction.Color2;
			Equipment equipment = character.Equipment;
			string text = ((equipment != null) ? equipment.CalculateEquipmentCode() : null);
			BodyProperties bodyProperties = character.GetBodyProperties(character.Equipment, -1);
			return CharacterCode.CreateFrom(text, bodyProperties, character.IsFemale, character.IsHero, color, color2, character.DefaultFormationClass, character.Race);
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x00067314 File Offset: 0x00065514
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.HeroMembers.ApplyActionOnAllItems(delegate(ClanPartyMemberItemVM h)
			{
				h.OnFinalize();
			});
			this.Roles.ApplyActionOnAllItems(delegate(ClanRoleItemVM x)
			{
				x.OnFinalize();
			});
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x06001B9B RID: 7067 RVA: 0x0006737B File Offset: 0x0006557B
		// (set) Token: 0x06001B9C RID: 7068 RVA: 0x00067383 File Offset: 0x00065583
		[DataSourceProperty]
		public CharacterViewModel CharacterModel
		{
			get
			{
				return this._characterModel;
			}
			set
			{
				if (value != this._characterModel)
				{
					this._characterModel = value;
					base.OnPropertyChangedWithValue<CharacterViewModel>(value, "CharacterModel");
				}
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06001B9D RID: 7069 RVA: 0x000673A1 File Offset: 0x000655A1
		// (set) Token: 0x06001B9E RID: 7070 RVA: 0x000673A9 File Offset: 0x000655A9
		[DataSourceProperty]
		public ClanPartyBehaviorSelectorVM PartyBehaviorSelector
		{
			get
			{
				return this._partyBehaviorSelector;
			}
			set
			{
				if (value != this._partyBehaviorSelector)
				{
					this._partyBehaviorSelector = value;
					base.OnPropertyChangedWithValue<ClanPartyBehaviorSelectorVM>(value, "PartyBehaviorSelector");
				}
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06001B9F RID: 7071 RVA: 0x000673C7 File Offset: 0x000655C7
		// (set) Token: 0x06001BA0 RID: 7072 RVA: 0x000673CF File Offset: 0x000655CF
		[DataSourceProperty]
		public CharacterImageIdentifierVM LeaderVisual
		{
			get
			{
				return this._leaderVisual;
			}
			set
			{
				if (value != this._leaderVisual)
				{
					this._leaderVisual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "LeaderVisual");
				}
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06001BA1 RID: 7073 RVA: 0x000673ED File Offset: 0x000655ED
		// (set) Token: 0x06001BA2 RID: 7074 RVA: 0x000673F5 File Offset: 0x000655F5
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06001BA3 RID: 7075 RVA: 0x00067413 File Offset: 0x00065613
		// (set) Token: 0x06001BA4 RID: 7076 RVA: 0x0006741B File Offset: 0x0006561B
		[DataSourceProperty]
		public bool HasHeroMembers
		{
			get
			{
				return this._hasHeroMembers;
			}
			set
			{
				if (value != this._hasHeroMembers)
				{
					this._hasHeroMembers = value;
					base.OnPropertyChangedWithValue(value, "HasHeroMembers");
				}
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06001BA5 RID: 7077 RVA: 0x00067439 File Offset: 0x00065639
		// (set) Token: 0x06001BA6 RID: 7078 RVA: 0x00067441 File Offset: 0x00065641
		[DataSourceProperty]
		public bool IsClanRoleSelectionHighlightEnabled
		{
			get
			{
				return this._isClanRoleSelectionHighlightEnabled;
			}
			set
			{
				if (value != this._isClanRoleSelectionHighlightEnabled)
				{
					this._isClanRoleSelectionHighlightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsClanRoleSelectionHighlightEnabled");
				}
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06001BA7 RID: 7079 RVA: 0x0006745F File Offset: 0x0006565F
		// (set) Token: 0x06001BA8 RID: 7080 RVA: 0x00067467 File Offset: 0x00065667
		[DataSourceProperty]
		public bool IsRoleSelectionPopupVisible
		{
			get
			{
				return this._isRoleSelectionPopupVisible;
			}
			set
			{
				if (value != this._isRoleSelectionPopupVisible)
				{
					this._isRoleSelectionPopupVisible = value;
					base.OnPropertyChangedWithValue(value, "IsRoleSelectionPopupVisible");
				}
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06001BA9 RID: 7081 RVA: 0x00067485 File Offset: 0x00065685
		// (set) Token: 0x06001BAA RID: 7082 RVA: 0x0006748D File Offset: 0x0006568D
		[DataSourceProperty]
		public bool IsDisbanding
		{
			get
			{
				return this._isDisbanding;
			}
			set
			{
				if (value != this._isDisbanding)
				{
					this._isDisbanding = value;
					base.OnPropertyChangedWithValue(value, "IsDisbanding");
				}
			}
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06001BAB RID: 7083 RVA: 0x000674AB File Offset: 0x000656AB
		// (set) Token: 0x06001BAC RID: 7084 RVA: 0x000674B3 File Offset: 0x000656B3
		[DataSourceProperty]
		public bool IsInArmy
		{
			get
			{
				return this._isInArmy;
			}
			set
			{
				if (value != this._isInArmy)
				{
					this._isInArmy = value;
					base.OnPropertyChangedWithValue(value, "IsInArmy");
				}
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06001BAD RID: 7085 RVA: 0x000674D1 File Offset: 0x000656D1
		// (set) Token: 0x06001BAE RID: 7086 RVA: 0x000674D9 File Offset: 0x000656D9
		[DataSourceProperty]
		public bool CanUseActions
		{
			get
			{
				return this._canUseActions;
			}
			set
			{
				if (value != this._canUseActions)
				{
					this._canUseActions = value;
					base.OnPropertyChangedWithValue(value, "CanUseActions");
				}
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06001BAF RID: 7087 RVA: 0x000674F7 File Offset: 0x000656F7
		// (set) Token: 0x06001BB0 RID: 7088 RVA: 0x000674FF File Offset: 0x000656FF
		[DataSourceProperty]
		public bool IsChangeLeaderVisible
		{
			get
			{
				return this._isChangeLeaderVisible;
			}
			set
			{
				if (value != this._isChangeLeaderVisible)
				{
					this._isChangeLeaderVisible = value;
					base.OnPropertyChangedWithValue(value, "IsChangeLeaderVisible");
				}
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06001BB1 RID: 7089 RVA: 0x0006751D File Offset: 0x0006571D
		// (set) Token: 0x06001BB2 RID: 7090 RVA: 0x00067525 File Offset: 0x00065725
		[DataSourceProperty]
		public bool IsChangeLeaderEnabled
		{
			get
			{
				return this._isChangeLeaderEnabled;
			}
			set
			{
				if (value != this._isChangeLeaderEnabled)
				{
					this._isChangeLeaderEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsChangeLeaderEnabled");
				}
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06001BB3 RID: 7091 RVA: 0x00067543 File Offset: 0x00065743
		// (set) Token: 0x06001BB4 RID: 7092 RVA: 0x0006754B File Offset: 0x0006574B
		[DataSourceProperty]
		public HintViewModel ActionsDisabledHint
		{
			get
			{
				return this._actionsDisabledHint;
			}
			set
			{
				if (value != this._actionsDisabledHint)
				{
					this._actionsDisabledHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ActionsDisabledHint");
				}
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06001BB5 RID: 7093 RVA: 0x00067569 File Offset: 0x00065769
		// (set) Token: 0x06001BB6 RID: 7094 RVA: 0x00067571 File Offset: 0x00065771
		[DataSourceProperty]
		public bool IsCaravan
		{
			get
			{
				return this._isCaravan;
			}
			set
			{
				if (value != this._isCaravan)
				{
					this._isCaravan = value;
					base.OnPropertyChangedWithValue(value, "IsCaravan");
				}
			}
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06001BB7 RID: 7095 RVA: 0x0006758F File Offset: 0x0006578F
		// (set) Token: 0x06001BB8 RID: 7096 RVA: 0x00067597 File Offset: 0x00065797
		[DataSourceProperty]
		public bool ShouldPartyHaveExpense
		{
			get
			{
				return this._shouldPartyHaveExpense;
			}
			set
			{
				if (value != this._shouldPartyHaveExpense)
				{
					this._shouldPartyHaveExpense = value;
					base.OnPropertyChangedWithValue(value, "ShouldPartyHaveExpense");
				}
			}
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06001BB9 RID: 7097 RVA: 0x000675B5 File Offset: 0x000657B5
		// (set) Token: 0x06001BBA RID: 7098 RVA: 0x000675BD File Offset: 0x000657BD
		[DataSourceProperty]
		public bool HasCompanion
		{
			get
			{
				return this._hasCompanion;
			}
			set
			{
				if (value != this._hasCompanion)
				{
					this._hasCompanion = value;
					base.OnPropertyChangedWithValue(value, "HasCompanion");
				}
			}
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06001BBB RID: 7099 RVA: 0x000675DB File Offset: 0x000657DB
		// (set) Token: 0x06001BBC RID: 7100 RVA: 0x000675E3 File Offset: 0x000657E3
		[DataSourceProperty]
		public bool IsAutoRecruitmentVisible
		{
			get
			{
				return this._isAutoRecruitmentVisible;
			}
			set
			{
				if (value != this._isAutoRecruitmentVisible)
				{
					this._isAutoRecruitmentVisible = value;
					base.OnPropertyChangedWithValue(value, "IsAutoRecruitmentVisible");
				}
			}
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06001BBD RID: 7101 RVA: 0x00067601 File Offset: 0x00065801
		// (set) Token: 0x06001BBE RID: 7102 RVA: 0x00067609 File Offset: 0x00065809
		[DataSourceProperty]
		public bool AutoRecruitmentValue
		{
			get
			{
				return this._autoRecruitmentValue;
			}
			set
			{
				if (value != this._autoRecruitmentValue)
				{
					this._autoRecruitmentValue = value;
					base.OnPropertyChangedWithValue(value, "AutoRecruitmentValue");
					this.OnAutoRecruitChanged(value);
				}
			}
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06001BBF RID: 7103 RVA: 0x0006762E File Offset: 0x0006582E
		// (set) Token: 0x06001BC0 RID: 7104 RVA: 0x00067636 File Offset: 0x00065836
		[DataSourceProperty]
		public bool IsPartyBehaviorEnabled
		{
			get
			{
				return this._isPartyBehaviorEnabled;
			}
			set
			{
				if (value != this._isPartyBehaviorEnabled)
				{
					this._isPartyBehaviorEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsPartyBehaviorEnabled");
				}
			}
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06001BC1 RID: 7105 RVA: 0x00067654 File Offset: 0x00065854
		// (set) Token: 0x06001BC2 RID: 7106 RVA: 0x0006765C File Offset: 0x0006585C
		[DataSourceProperty]
		public bool IsMembersAndRolesVisible
		{
			get
			{
				return this._isMembersAndRolesVisible;
			}
			set
			{
				if (value != this._isMembersAndRolesVisible)
				{
					this._isMembersAndRolesVisible = value;
					base.OnPropertyChangedWithValue(value, "IsMembersAndRolesVisible");
				}
			}
		}

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06001BC3 RID: 7107 RVA: 0x0006767A File Offset: 0x0006587A
		// (set) Token: 0x06001BC4 RID: 7108 RVA: 0x00067682 File Offset: 0x00065882
		[DataSourceProperty]
		public bool IsMainHeroParty
		{
			get
			{
				return this._isMainHeroParty;
			}
			set
			{
				if (value != this._isMainHeroParty)
				{
					this._isMainHeroParty = value;
					base.OnPropertyChangedWithValue(value, "IsMainHeroParty");
				}
			}
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06001BC5 RID: 7109 RVA: 0x000676A0 File Offset: 0x000658A0
		// (set) Token: 0x06001BC6 RID: 7110 RVA: 0x000676A8 File Offset: 0x000658A8
		[DataSourceProperty]
		public ClanFinanceExpenseItemVM ExpenseItem
		{
			get
			{
				return this._expenseItem;
			}
			set
			{
				if (value != this._expenseItem)
				{
					this._expenseItem = value;
					base.OnPropertyChangedWithValue<ClanFinanceExpenseItemVM>(value, "ExpenseItem");
				}
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x06001BC7 RID: 7111 RVA: 0x000676C6 File Offset: 0x000658C6
		// (set) Token: 0x06001BC8 RID: 7112 RVA: 0x000676CE File Offset: 0x000658CE
		[DataSourceProperty]
		public ClanRoleItemVM LastOpenedRoleSelection
		{
			get
			{
				return this._lastOpenedRoleSelection;
			}
			set
			{
				if (value != this._lastOpenedRoleSelection)
				{
					this._lastOpenedRoleSelection = value;
					base.OnPropertyChangedWithValue<ClanRoleItemVM>(value, "LastOpenedRoleSelection");
				}
			}
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x000676EC File Offset: 0x000658EC
		// (set) Token: 0x06001BCA RID: 7114 RVA: 0x000676F4 File Offset: 0x000658F4
		[DataSourceProperty]
		public ClanPartyMemberItemVM LeaderMember
		{
			get
			{
				return this._leaderMember;
			}
			set
			{
				if (value != this._leaderMember)
				{
					this._leaderMember = value;
					base.OnPropertyChangedWithValue<ClanPartyMemberItemVM>(value, "LeaderMember");
				}
			}
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x06001BCB RID: 7115 RVA: 0x00067712 File Offset: 0x00065912
		// (set) Token: 0x06001BCC RID: 7116 RVA: 0x0006771A File Offset: 0x0006591A
		[DataSourceProperty]
		public string PartySizeText
		{
			get
			{
				return this._partySizeText;
			}
			set
			{
				if (value != this._partySizeText)
				{
					this._partySizeText = value;
					base.OnPropertyChanged("PartyStrengthText");
				}
			}
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x06001BCD RID: 7117 RVA: 0x0006773C File Offset: 0x0006593C
		// (set) Token: 0x06001BCE RID: 7118 RVA: 0x00067744 File Offset: 0x00065944
		[DataSourceProperty]
		public string ShipCountText
		{
			get
			{
				return this._shipCountText;
			}
			set
			{
				if (value != this._shipCountText)
				{
					this._shipCountText = value;
					base.OnPropertyChangedWithValue<string>(value, "ShipCountText");
				}
			}
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x06001BCF RID: 7119 RVA: 0x00067767 File Offset: 0x00065967
		// (set) Token: 0x06001BD0 RID: 7120 RVA: 0x0006776F File Offset: 0x0006596F
		[DataSourceProperty]
		public string MembersText
		{
			get
			{
				return this._membersText;
			}
			set
			{
				if (value != null)
				{
					this._membersText = value;
					base.OnPropertyChangedWithValue<string>(value, "MembersText");
				}
			}
		}

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x06001BD1 RID: 7121 RVA: 0x00067787 File Offset: 0x00065987
		// (set) Token: 0x06001BD2 RID: 7122 RVA: 0x0006778F File Offset: 0x0006598F
		[DataSourceProperty]
		public string AssigneesText
		{
			get
			{
				return this._assigneesText;
			}
			set
			{
				if (value != this._assigneesText)
				{
					this._assigneesText = value;
					base.OnPropertyChangedWithValue<string>(value, "AssigneesText");
				}
			}
		}

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06001BD3 RID: 7123 RVA: 0x000677B2 File Offset: 0x000659B2
		// (set) Token: 0x06001BD4 RID: 7124 RVA: 0x000677BA File Offset: 0x000659BA
		[DataSourceProperty]
		public string RolesText
		{
			get
			{
				return this._rolesText;
			}
			set
			{
				if (value != this._rolesText)
				{
					this._rolesText = value;
					base.OnPropertyChangedWithValue<string>(value, "RolesText");
				}
			}
		}

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06001BD5 RID: 7125 RVA: 0x000677DD File Offset: 0x000659DD
		// (set) Token: 0x06001BD6 RID: 7126 RVA: 0x000677E5 File Offset: 0x000659E5
		[DataSourceProperty]
		public string PartyLeaderRoleEffectsText
		{
			get
			{
				return this._partyLeaderRoleEffectsText;
			}
			set
			{
				if (value != this._partyLeaderRoleEffectsText)
				{
					this._partyLeaderRoleEffectsText = value;
					base.OnPropertyChangedWithValue<string>(value, "PartyLeaderRoleEffectsText");
				}
			}
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06001BD7 RID: 7127 RVA: 0x00067808 File Offset: 0x00065A08
		// (set) Token: 0x06001BD8 RID: 7128 RVA: 0x00067810 File Offset: 0x00065A10
		[DataSourceProperty]
		public string PartyLocationText
		{
			get
			{
				return this._partyLocationText;
			}
			set
			{
				if (value != this._partyLocationText)
				{
					this._partyLocationText = value;
					base.OnPropertyChangedWithValue<string>(value, "PartyLocationText");
				}
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06001BD9 RID: 7129 RVA: 0x00067833 File Offset: 0x00065A33
		// (set) Token: 0x06001BDA RID: 7130 RVA: 0x0006783B File Offset: 0x00065A3B
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x06001BDB RID: 7131 RVA: 0x0006785E File Offset: 0x00065A5E
		// (set) Token: 0x06001BDC RID: 7132 RVA: 0x00067866 File Offset: 0x00065A66
		[DataSourceProperty]
		public string PartySizeSubTitleText
		{
			get
			{
				return this._partySizeSubTitleText;
			}
			set
			{
				if (value != this._partySizeSubTitleText)
				{
					this._partySizeSubTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "PartySizeSubTitleText");
				}
			}
		}

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06001BDD RID: 7133 RVA: 0x00067889 File Offset: 0x00065A89
		// (set) Token: 0x06001BDE RID: 7134 RVA: 0x00067891 File Offset: 0x00065A91
		[DataSourceProperty]
		public string PartyWageSubTitleText
		{
			get
			{
				return this._partyWageSubTitleText;
			}
			set
			{
				if (value != this._partyWageSubTitleText)
				{
					this._partyWageSubTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "PartyWageSubTitleText");
				}
			}
		}

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06001BDF RID: 7135 RVA: 0x000678B4 File Offset: 0x00065AB4
		// (set) Token: 0x06001BE0 RID: 7136 RVA: 0x000678BC File Offset: 0x00065ABC
		[DataSourceProperty]
		public string PartyBehaviorText
		{
			get
			{
				return this._partyBehaviorText;
			}
			set
			{
				if (value != this._partyBehaviorText)
				{
					this._partyBehaviorText = value;
					base.OnPropertyChangedWithValue<string>(value, "PartyBehaviorText");
				}
			}
		}

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06001BE1 RID: 7137 RVA: 0x000678DF File Offset: 0x00065ADF
		// (set) Token: 0x06001BE2 RID: 7138 RVA: 0x000678E7 File Offset: 0x00065AE7
		[DataSourceProperty]
		public int InfantryCount
		{
			get
			{
				return this._infantryCount;
			}
			set
			{
				if (value != this._infantryCount)
				{
					this._infantryCount = value;
					base.OnPropertyChangedWithValue(value, "InfantryCount");
				}
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06001BE3 RID: 7139 RVA: 0x00067905 File Offset: 0x00065B05
		// (set) Token: 0x06001BE4 RID: 7140 RVA: 0x0006790D File Offset: 0x00065B0D
		[DataSourceProperty]
		public int RangedCount
		{
			get
			{
				return this._rangedCount;
			}
			set
			{
				if (value != this._rangedCount)
				{
					this._rangedCount = value;
					base.OnPropertyChangedWithValue(value, "RangedCount");
				}
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06001BE5 RID: 7141 RVA: 0x0006792B File Offset: 0x00065B2B
		// (set) Token: 0x06001BE6 RID: 7142 RVA: 0x00067933 File Offset: 0x00065B33
		[DataSourceProperty]
		public int CavalryCount
		{
			get
			{
				return this._cavalryCount;
			}
			set
			{
				if (value != this._cavalryCount)
				{
					this._cavalryCount = value;
					base.OnPropertyChangedWithValue(value, "CavalryCount");
				}
			}
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06001BE7 RID: 7143 RVA: 0x00067951 File Offset: 0x00065B51
		// (set) Token: 0x06001BE8 RID: 7144 RVA: 0x00067959 File Offset: 0x00065B59
		[DataSourceProperty]
		public int HorseArcherCount
		{
			get
			{
				return this._horseArcherCount;
			}
			set
			{
				if (value != this._horseArcherCount)
				{
					this._horseArcherCount = value;
					base.OnPropertyChangedWithValue(value, "HorseArcherCount");
				}
			}
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06001BE9 RID: 7145 RVA: 0x00067977 File Offset: 0x00065B77
		// (set) Token: 0x06001BEA RID: 7146 RVA: 0x0006797F File Offset: 0x00065B7F
		[DataSourceProperty]
		public int ShipCount
		{
			get
			{
				return this._shipCount;
			}
			set
			{
				if (value != this._shipCount)
				{
					this._shipCount = value;
					base.OnPropertyChangedWithValue(value, "ShipCount");
				}
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06001BEB RID: 7147 RVA: 0x0006799D File Offset: 0x00065B9D
		// (set) Token: 0x06001BEC RID: 7148 RVA: 0x000679A5 File Offset: 0x00065BA5
		[DataSourceProperty]
		public string InArmyText
		{
			get
			{
				return this._inArmyText;
			}
			set
			{
				if (value != this._inArmyText)
				{
					this._inArmyText = value;
					base.OnPropertyChangedWithValue<string>(value, "InArmyText");
				}
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06001BED RID: 7149 RVA: 0x000679C8 File Offset: 0x00065BC8
		// (set) Token: 0x06001BEE RID: 7150 RVA: 0x000679D0 File Offset: 0x00065BD0
		[DataSourceProperty]
		public string DisbandingText
		{
			get
			{
				return this._disbandingText;
			}
			set
			{
				if (value != this._disbandingText)
				{
					this._disbandingText = value;
					base.OnPropertyChangedWithValue<string>(value, "DisbandingText");
				}
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06001BEF RID: 7151 RVA: 0x000679F3 File Offset: 0x00065BF3
		// (set) Token: 0x06001BF0 RID: 7152 RVA: 0x000679FB File Offset: 0x00065BFB
		[DataSourceProperty]
		public string AutoRecruitmentText
		{
			get
			{
				return this._autoRecruitmentText;
			}
			set
			{
				if (value != this._autoRecruitmentText)
				{
					this._autoRecruitmentText = value;
					base.OnPropertyChangedWithValue<string>(value, "AutoRecruitmentText");
				}
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x06001BF1 RID: 7153 RVA: 0x00067A1E File Offset: 0x00065C1E
		// (set) Token: 0x06001BF2 RID: 7154 RVA: 0x00067A26 File Offset: 0x00065C26
		[DataSourceProperty]
		public HintViewModel AutoRecruitmentHint
		{
			get
			{
				return this._autoRecruitmentHint;
			}
			set
			{
				if (value != this._autoRecruitmentHint)
				{
					this._autoRecruitmentHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AutoRecruitmentHint");
				}
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x06001BF3 RID: 7155 RVA: 0x00067A44 File Offset: 0x00065C44
		// (set) Token: 0x06001BF4 RID: 7156 RVA: 0x00067A4C File Offset: 0x00065C4C
		[DataSourceProperty]
		public HintViewModel InArmyHint
		{
			get
			{
				return this._inArmyHint;
			}
			set
			{
				if (value != this._inArmyHint)
				{
					this._inArmyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "InArmyHint");
				}
			}
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x06001BF5 RID: 7157 RVA: 0x00067A6A File Offset: 0x00065C6A
		// (set) Token: 0x06001BF6 RID: 7158 RVA: 0x00067A72 File Offset: 0x00065C72
		[DataSourceProperty]
		public HintViewModel ChangeLeaderHint
		{
			get
			{
				return this._changeLeaderHint;
			}
			set
			{
				if (value != this._changeLeaderHint)
				{
					this._changeLeaderHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ChangeLeaderHint");
				}
			}
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06001BF7 RID: 7159 RVA: 0x00067A90 File Offset: 0x00065C90
		// (set) Token: 0x06001BF8 RID: 7160 RVA: 0x00067A98 File Offset: 0x00065C98
		[DataSourceProperty]
		public BasicTooltipViewModel InfantryHint
		{
			get
			{
				return this._infantryHint;
			}
			set
			{
				if (value != this._infantryHint)
				{
					this._infantryHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "InfantryHint");
				}
			}
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06001BF9 RID: 7161 RVA: 0x00067AB6 File Offset: 0x00065CB6
		// (set) Token: 0x06001BFA RID: 7162 RVA: 0x00067ABE File Offset: 0x00065CBE
		[DataSourceProperty]
		public BasicTooltipViewModel RangedHint
		{
			get
			{
				return this._rangedHint;
			}
			set
			{
				if (value != this._rangedHint)
				{
					this._rangedHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "RangedHint");
				}
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06001BFB RID: 7163 RVA: 0x00067ADC File Offset: 0x00065CDC
		// (set) Token: 0x06001BFC RID: 7164 RVA: 0x00067AE4 File Offset: 0x00065CE4
		[DataSourceProperty]
		public BasicTooltipViewModel CavalryHint
		{
			get
			{
				return this._cavalryHint;
			}
			set
			{
				if (value != this._cavalryHint)
				{
					this._cavalryHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "CavalryHint");
				}
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06001BFD RID: 7165 RVA: 0x00067B02 File Offset: 0x00065D02
		// (set) Token: 0x06001BFE RID: 7166 RVA: 0x00067B0A File Offset: 0x00065D0A
		[DataSourceProperty]
		public BasicTooltipViewModel HorseArcherHint
		{
			get
			{
				return this._horseArcherHint;
			}
			set
			{
				if (value != this._horseArcherHint)
				{
					this._horseArcherHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "HorseArcherHint");
				}
			}
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06001BFF RID: 7167 RVA: 0x00067B28 File Offset: 0x00065D28
		// (set) Token: 0x06001C00 RID: 7168 RVA: 0x00067B30 File Offset: 0x00065D30
		[DataSourceProperty]
		public MBBindingList<ClanPartyMemberItemVM> HeroMembers
		{
			get
			{
				return this._heroMembers;
			}
			set
			{
				if (value != this._heroMembers)
				{
					this._heroMembers = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanPartyMemberItemVM>>(value, "HeroMembers");
				}
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06001C01 RID: 7169 RVA: 0x00067B4E File Offset: 0x00065D4E
		// (set) Token: 0x06001C02 RID: 7170 RVA: 0x00067B56 File Offset: 0x00065D56
		[DataSourceProperty]
		public MBBindingList<ClanRoleItemVM> Roles
		{
			get
			{
				return this._roles;
			}
			set
			{
				if (value != this._roles)
				{
					this._roles = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanRoleItemVM>>(value, "Roles");
				}
			}
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x00067B74 File Offset: 0x00065D74
		private List<TooltipProperty> GetPartyTroopInfo(PartyBase party, FormationClass formationClass)
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			list.Add(new TooltipProperty("", GameTexts.FindText("str_formation_class_string", formationClass.GetName()).ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.Title));
			foreach (TroopRosterElement troopRosterElement in this.Party.MemberRoster.GetTroopRoster())
			{
				if (!troopRosterElement.Character.IsHero && troopRosterElement.Character.DefaultFormationClass.Equals(formationClass))
				{
					list.Add(new TooltipProperty(troopRosterElement.Character.Name.ToString(), troopRosterElement.Number.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				}
			}
			return list;
		}

		// Token: 0x04000CD7 RID: 3287
		private readonly Action<ClanPartyItemVM> _onAssignment;

		// Token: 0x04000CD8 RID: 3288
		private readonly Action _onExpenseChange;

		// Token: 0x04000CD9 RID: 3289
		private readonly Action _onShowChangeLeaderPopup;

		// Token: 0x04000CDA RID: 3290
		private readonly ClanPartyItemVM.ClanPartyType _type;

		// Token: 0x04000CDB RID: 3291
		private readonly TextObject _changeLeaderHintText = GameTexts.FindText("str_change_party_leader", null);

		// Token: 0x04000CDC RID: 3292
		private readonly IDisbandPartyCampaignBehavior _disbandBehavior;

		// Token: 0x04000CDD RID: 3293
		private readonly bool _isLeaderTeleporting;

		// Token: 0x04000CDF RID: 3295
		private readonly CharacterObject _leader;

		// Token: 0x04000CE0 RID: 3296
		private ClanPartyBehaviorSelectorVM _partyBehaviorSelector;

		// Token: 0x04000CE1 RID: 3297
		private ClanFinanceExpenseItemVM _expenseItem;

		// Token: 0x04000CE2 RID: 3298
		private ClanRoleItemVM _lastOpenedRoleSelection;

		// Token: 0x04000CE3 RID: 3299
		private ClanPartyMemberItemVM _leaderMember;

		// Token: 0x04000CE4 RID: 3300
		private CharacterImageIdentifierVM _leaderVisual;

		// Token: 0x04000CE5 RID: 3301
		private bool _isMainHeroParty;

		// Token: 0x04000CE6 RID: 3302
		private bool _isSelected;

		// Token: 0x04000CE7 RID: 3303
		private bool _hasHeroMembers;

		// Token: 0x04000CE8 RID: 3304
		private string _partyLocationText;

		// Token: 0x04000CE9 RID: 3305
		private string _partySizeText;

		// Token: 0x04000CEA RID: 3306
		private string _shipCountText;

		// Token: 0x04000CEB RID: 3307
		private string _membersText;

		// Token: 0x04000CEC RID: 3308
		private string _assigneesText;

		// Token: 0x04000CED RID: 3309
		private string _rolesText;

		// Token: 0x04000CEE RID: 3310
		private string _partyLeaderRoleEffectsText;

		// Token: 0x04000CEF RID: 3311
		private string _name;

		// Token: 0x04000CF0 RID: 3312
		private string _partySizeSubTitleText;

		// Token: 0x04000CF1 RID: 3313
		private string _partyWageSubTitleText;

		// Token: 0x04000CF2 RID: 3314
		private string _partyBehaviorText;

		// Token: 0x04000CF3 RID: 3315
		private int _infantryCount;

		// Token: 0x04000CF4 RID: 3316
		private int _rangedCount;

		// Token: 0x04000CF5 RID: 3317
		private int _cavalryCount;

		// Token: 0x04000CF6 RID: 3318
		private int _horseArcherCount;

		// Token: 0x04000CF7 RID: 3319
		private int _shipCount;

		// Token: 0x04000CF8 RID: 3320
		private string _inArmyText;

		// Token: 0x04000CF9 RID: 3321
		private string _disbandingText;

		// Token: 0x04000CFA RID: 3322
		private string _autoRecruitmentText;

		// Token: 0x04000CFB RID: 3323
		private bool _autoRecruitmentValue;

		// Token: 0x04000CFC RID: 3324
		private bool _isAutoRecruitmentVisible;

		// Token: 0x04000CFD RID: 3325
		private bool _shouldPartyHaveExpense;

		// Token: 0x04000CFE RID: 3326
		private bool _hasCompanion;

		// Token: 0x04000CFF RID: 3327
		private bool _isPartyBehaviorEnabled;

		// Token: 0x04000D00 RID: 3328
		private bool _isMembersAndRolesVisible;

		// Token: 0x04000D01 RID: 3329
		private bool _isCaravan;

		// Token: 0x04000D02 RID: 3330
		private bool _isDisbanding;

		// Token: 0x04000D03 RID: 3331
		private bool _isInArmy;

		// Token: 0x04000D04 RID: 3332
		private bool _canUseActions;

		// Token: 0x04000D05 RID: 3333
		private bool _isChangeLeaderVisible;

		// Token: 0x04000D06 RID: 3334
		private bool _isChangeLeaderEnabled;

		// Token: 0x04000D07 RID: 3335
		private bool _isClanRoleSelectionHighlightEnabled;

		// Token: 0x04000D08 RID: 3336
		private bool _isRoleSelectionPopupVisible;

		// Token: 0x04000D09 RID: 3337
		private HintViewModel _actionsDisabledHint;

		// Token: 0x04000D0A RID: 3338
		private CharacterViewModel _characterModel;

		// Token: 0x04000D0B RID: 3339
		private HintViewModel _autoRecruitmentHint;

		// Token: 0x04000D0C RID: 3340
		private HintViewModel _inArmyHint;

		// Token: 0x04000D0D RID: 3341
		private HintViewModel _changeLeaderHint;

		// Token: 0x04000D0E RID: 3342
		private BasicTooltipViewModel _infantryHint;

		// Token: 0x04000D0F RID: 3343
		private BasicTooltipViewModel _rangedHint;

		// Token: 0x04000D10 RID: 3344
		private BasicTooltipViewModel _cavalryHint;

		// Token: 0x04000D11 RID: 3345
		private BasicTooltipViewModel _horseArcherHint;

		// Token: 0x04000D12 RID: 3346
		private MBBindingList<ClanPartyMemberItemVM> _heroMembers;

		// Token: 0x04000D13 RID: 3347
		private MBBindingList<ClanRoleItemVM> _roles;

		// Token: 0x02000288 RID: 648
		public enum ClanPartyType
		{
			// Token: 0x040012F2 RID: 4850
			Main,
			// Token: 0x040012F3 RID: 4851
			Member,
			// Token: 0x040012F4 RID: 4852
			Caravan,
			// Token: 0x040012F5 RID: 4853
			Garrison
		}
	}
}
