using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.ViewModelCollection.Quests;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Overlay
{
	// Token: 0x020000BA RID: 186
	public class GameMenuPartyItemVM : ViewModel
	{
		// Token: 0x06001250 RID: 4688 RVA: 0x0004A2F4 File Offset: 0x000484F4
		public GameMenuPartyItemVM()
		{
			this.Visual = new CharacterImageIdentifierVM(null);
			this.RegisterEvents();
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x0004A320 File Offset: 0x00048520
		public GameMenuPartyItemVM(Action<GameMenuPartyItemVM> onSetAsContextMenuActiveItem, Settlement settlement)
		{
			this._onSetAsContextMenuActiveItem = onSetAsContextMenuActiveItem;
			this.Settlement = settlement;
			SettlementComponent settlementComponent = settlement.SettlementComponent;
			this.SettlementPath = ((settlementComponent == null) ? "placeholder" : (settlementComponent.BackgroundMeshName + "_t"));
			this.Visual = new CharacterImageIdentifierVM(null);
			this.NameText = settlement.Name.ToString();
			this.PartySize = -1;
			this.PartyWoundedSize = -1;
			this.PartySizeLbl = "";
			this.IsPlayer = false;
			this.IsAlly = false;
			this.IsEnemy = false;
			this.Quests = new MBBindingList<QuestMarkerVM>();
			this.RefreshProperties();
			this.RegisterEvents();
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x0004A3DC File Offset: 0x000485DC
		public GameMenuPartyItemVM(Action<GameMenuPartyItemVM> onSetAsContextMenuActiveItem, PartyBase item, bool canShowQuest)
		{
			this._onSetAsContextMenuActiveItem = onSetAsContextMenuActiveItem;
			this.Party = item;
			CharacterObject visualPartyLeader = PartyBaseHelper.GetVisualPartyLeader(this.Party);
			if (visualPartyLeader != null)
			{
				CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(visualPartyLeader, false);
				this.Visual = new CharacterImageIdentifierVM(characterCode);
			}
			else
			{
				this.Visual = new CharacterImageIdentifierVM(null);
			}
			this.Quests = new MBBindingList<QuestMarkerVM>();
			this._canShowQuest = canShowQuest;
			this.RefreshProperties();
			this.RegisterEvents();
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x0004A45C File Offset: 0x0004865C
		public GameMenuPartyItemVM(Action<GameMenuPartyItemVM> onSetAsContextMenuActiveItem, CharacterObject character, bool useCivilianEquipment)
		{
			this._onSetAsContextMenuActiveItem = onSetAsContextMenuActiveItem;
			this.Character = character;
			this._useCivilianEquipment = useCivilianEquipment;
			CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(character, useCivilianEquipment);
			this.Visual = new CharacterImageIdentifierVM(characterCode);
			Hero heroObject = this.Character.HeroObject;
			this.Banner_9 = (((heroObject != null && heroObject.IsLord) || (this.Character.IsHero && this.Character.HeroObject.Clan == Clan.PlayerClan && character.HeroObject.IsLord)) ? new BannerImageIdentifierVM(this.Character.HeroObject.ClanBanner, true) : new BannerImageIdentifierVM(null, false));
			this.NameText = this.Character.Name.ToString();
			this.PartySize = -1;
			this.PartyWoundedSize = -1;
			this.PartySizeLbl = "";
			this.IsPlayer = character.IsPlayerCharacter;
			this.Quests = new MBBindingList<QuestMarkerVM>();
			this.RefreshProperties();
			this.RegisterEvents();
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x0004A570 File Offset: 0x00048770
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RefreshProperties();
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x0004A57E File Offset: 0x0004877E
		public void ExecuteSetAsContextMenuItem()
		{
			Action<GameMenuPartyItemVM> onSetAsContextMenuActiveItem = this._onSetAsContextMenuActiveItem;
			if (onSetAsContextMenuActiveItem == null)
			{
				return;
			}
			onSetAsContextMenuActiveItem(this);
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x0004A594 File Offset: 0x00048794
		public void ExecuteOpenEncyclopedia()
		{
			string encyclopediaPageLink = this.GetEncyclopediaPageLink();
			if (!string.IsNullOrEmpty(encyclopediaPageLink))
			{
				Campaign.Current.EncyclopediaManager.GoToLink(encyclopediaPageLink);
			}
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x0004A5C0 File Offset: 0x000487C0
		public void ExecuteCloseTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x0004A5C8 File Offset: 0x000487C8
		public void ExecuteOpenTooltip()
		{
			PartyBase party = this.Party;
			if (((party != null) ? party.MobileParty : null) != null)
			{
				InformationManager.ShowTooltip(typeof(MobileParty), new object[]
				{
					this.Party.MobileParty,
					true,
					false
				});
				return;
			}
			if (this.Settlement != null)
			{
				InformationManager.ShowTooltip(typeof(Settlement), new object[] { this.Settlement });
				return;
			}
			InformationManager.ShowTooltip(typeof(Hero), new object[]
			{
				this.Character.HeroObject,
				true
			});
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x0004A674 File Offset: 0x00048874
		public void RefreshProperties()
		{
			this.EncyclopediaCursorEffect = ((!string.IsNullOrEmpty(this.GetEncyclopediaPageLink())) ? "RightClickLink" : null);
			if (this.Party != null)
			{
				this.RefreshCounts();
				this.Relation = HeroVM.GetRelation(this.Party.LeaderHero);
				this.LocationText = " ";
				TextObject textObject = this.Party.Name;
				if (this.Party.IsMobile)
				{
					textObject = this.Party.MobileParty.Name;
					float getEncounterJoiningRadius = Campaign.Current.Models.EncounterModel.GetEncounterJoiningRadius;
					if (this.Party.MobileParty.Position.DistanceSquared(MobileParty.MainParty.Position) > getEncounterJoiningRadius * getEncounterJoiningRadius)
					{
						if (this.Party.MobileParty.MapEvent == null)
						{
							GameTexts.SetVariable("LEFT", GameTexts.FindText("str_distance_to_army_leader", null));
							float num = DistanceHelper.FindClosestDistanceFromMobilePartyToMobileParty(this.Party.MobileParty, MobileParty.MainParty, this.Party.MobileParty.NavigationCapability);
							GameTexts.SetVariable("RIGHT", CampaignUIHelper.GetPartyDistanceByTimeText((float)((int)num), this.Party.MobileParty.Speed));
							this.LocationText = GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).ToString();
						}
						else
						{
							TextObject textObject2 = GameTexts.FindText("str_at_map_event", null);
							TextObject textObject3 = new TextObject("{=zawBaxl5}Distance : {DISTANCE}", null);
							textObject3.SetTextVariable("DISTANCE", textObject2);
							this.LocationText = textObject3.ToString();
						}
					}
					this.DescriptionText = this.GetPartyDescriptionTextFromValues();
					this.IsMergedWithArmy = true;
					if (this.Party.MobileParty.Army != null)
					{
						this.IsMergedWithArmy = this.Party.MobileParty.Army.DoesLeaderPartyAndAttachedPartiesContain(this.Party.MobileParty);
					}
				}
				this.NameText = textObject.ToString();
				this.ProfessionText = " ";
				this.HasShips = this.Party.Ships.Count > 0;
			}
			else if (this.Character != null)
			{
				this.Relation = HeroVM.GetRelation(this.Character.HeroObject);
				Hero heroObject = this.Character.HeroObject;
				this.IsCharacterInPrison = heroObject != null && heroObject.IsPrisoner;
				GameTexts.SetVariable("PROFESSION", HeroHelper.GetCharacterTypeName(this.Character.HeroObject));
				string text = "LOCATION";
				Hero heroObject2 = this.Character.HeroObject;
				GameTexts.SetVariable(text, (((heroObject2 != null) ? heroObject2.CurrentSettlement : null) != null) ? this.Character.HeroObject.CurrentSettlement.Name.ToString() : "");
				Hero heroObject3 = this.Character.HeroObject;
				this.DescriptionText = ((heroObject3 != null && !heroObject3.IsSpecial) ? GameTexts.FindText("str_character_in_town", null).ToString() : string.Empty);
				string text2 = "LOCATION";
				LocationComplex locationComplex = LocationComplex.Current;
				TextObject textObject4;
				if (locationComplex == null)
				{
					textObject4 = null;
				}
				else
				{
					Location locationOfCharacter = locationComplex.GetLocationOfCharacter(this.Character.HeroObject);
					textObject4 = ((locationOfCharacter != null) ? locationOfCharacter.Name : null);
				}
				GameTexts.SetVariable(text2, textObject4 ?? TextObject.GetEmpty());
				this.LocationText = GameTexts.FindText("str_location_colon", null).ToString();
				GameTexts.SetVariable("PROFESSION", HeroHelper.GetCharacterTypeName(this.Character.HeroObject));
				this.ProfessionText = GameTexts.FindText("str_profession_colon", null).ToString();
				if (this.Character.IsHero && this.Character.HeroObject.IsNotable)
				{
					GameTexts.SetVariable("POWER", Campaign.Current.Models.NotablePowerModel.GetPowerRankName(this.Character.HeroObject).ToString());
					this.PowerText = GameTexts.FindText("str_power_colon", null).ToString();
				}
				this.NameText = this.Character.Name.ToString();
				this.HasShips = false;
			}
			this.RefreshQuestStatus();
			this.RefreshRelationStatus();
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x0004AA5C File Offset: 0x00048C5C
		public void RefreshQuestStatus()
		{
			this.Quests.Clear();
			PartyBase party = this.Party;
			Hero hero;
			if ((hero = ((party != null) ? party.LeaderHero : null)) == null)
			{
				CharacterObject character = this.Character;
				hero = ((character != null) ? character.HeroObject : null);
			}
			Hero hero2 = hero;
			if (hero2 != null)
			{
				GameMenuPartyItemVM.<>c__DisplayClass16_0 CS$<>8__locals1 = new GameMenuPartyItemVM.<>c__DisplayClass16_0();
				CS$<>8__locals1.questTypes = CampaignUIHelper.GetQuestStateOfHero(hero2);
				int k;
				int i;
				for (i = 0; i < CS$<>8__locals1.questTypes.Count; i = k + 1)
				{
					if (!this.Quests.Any<QuestMarkerVM>((QuestMarkerVM q) => q.QuestMarkerType == (int)CS$<>8__locals1.questTypes[i].Item1))
					{
						this.Quests.Add(new QuestMarkerVM(CS$<>8__locals1.questTypes[i].Item1, CS$<>8__locals1.questTypes[i].Item2, CS$<>8__locals1.questTypes[i].Item3));
					}
					k = i;
				}
			}
			else
			{
				PartyBase party2 = this.Party;
				if (((party2 != null) ? party2.MobileParty : null) != null)
				{
					List<QuestBase> questsRelatedToParty = CampaignUIHelper.GetQuestsRelatedToParty(this.Party.MobileParty);
					for (int j = 0; j < questsRelatedToParty.Count; j++)
					{
						TextObject textObject = ((questsRelatedToParty[j].JournalEntries.Count > 0) ? questsRelatedToParty[j].JournalEntries[0].LogText : TextObject.GetEmpty());
						CampaignUIHelper.IssueQuestFlags issueQuestFlags;
						if (hero2 != null && questsRelatedToParty[j].QuestGiver == hero2)
						{
							issueQuestFlags = (questsRelatedToParty[j].IsSpecialQuest ? CampaignUIHelper.IssueQuestFlags.ActiveStoryQuest : CampaignUIHelper.IssueQuestFlags.ActiveIssue);
						}
						else
						{
							issueQuestFlags = (questsRelatedToParty[j].IsSpecialQuest ? CampaignUIHelper.IssueQuestFlags.TrackedStoryQuest : CampaignUIHelper.IssueQuestFlags.TrackedIssue);
						}
						this.Quests.Add(new QuestMarkerVM(issueQuestFlags, questsRelatedToParty[j].Title, textObject));
					}
				}
			}
			this.Quests.Sort(new GameMenuPartyItemVM.QuestMarkerComparer());
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x0004AC6C File Offset: 0x00048E6C
		private void RefreshRelationStatus()
		{
			this.IsEnemy = false;
			this.IsAlly = false;
			this.IsNeutral = false;
			IFaction faction = null;
			bool flag = false;
			if (this.Character != null)
			{
				this.IsPlayer = this.Character.IsPlayerCharacter;
				flag = this.Character.IsHero && this.Character.HeroObject.IsNotable;
				IFaction faction2;
				if (!this.IsPlayer)
				{
					CharacterObject character = this.Character;
					faction2 = ((character != null) ? character.HeroObject.MapFaction : null);
				}
				else
				{
					faction2 = null;
				}
				faction = faction2;
			}
			else if (this.Party != null)
			{
				bool flag2;
				if (this.Party.IsMobile)
				{
					MobileParty mobileParty = this.Party.MobileParty;
					flag2 = mobileParty != null && mobileParty.IsMainParty;
				}
				else
				{
					flag2 = false;
				}
				this.IsPlayer = flag2;
				flag = false;
				IFaction faction3;
				if (!this.IsPlayer)
				{
					PartyBase party = this.Party;
					if (party == null)
					{
						faction3 = null;
					}
					else
					{
						MobileParty mobileParty2 = party.MobileParty;
						faction3 = ((mobileParty2 != null) ? mobileParty2.MapFaction : null);
					}
				}
				else
				{
					faction3 = null;
				}
				faction = faction3;
			}
			if (this.IsPlayer || faction == null || flag)
			{
				if (!this.IsPlayer)
				{
					this.IsNeutral = true;
				}
				return;
			}
			if (FactionManager.IsAtWarAgainstFaction(faction, Hero.MainHero.MapFaction))
			{
				this.IsEnemy = true;
				return;
			}
			if (DiplomacyHelper.IsSameFactionAndNotEliminated(faction, Hero.MainHero.MapFaction))
			{
				this.IsAlly = true;
				return;
			}
			this.IsNeutral = true;
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x0004ADAC File Offset: 0x00048FAC
		public void RefreshVisual()
		{
			if (this.Visual.IsEmpty)
			{
				if (this.Character != null)
				{
					CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(this.Character, this._useCivilianEquipment);
					this.Visual = new CharacterImageIdentifierVM(characterCode);
					return;
				}
				if (this.Party != null)
				{
					CharacterObject visualPartyLeader = PartyBaseHelper.GetVisualPartyLeader(this.Party);
					if (visualPartyLeader != null)
					{
						CharacterCode characterCode2 = CampaignUIHelper.GetCharacterCode(visualPartyLeader, false);
						this.Visual = new CharacterImageIdentifierVM(characterCode2);
						return;
					}
					this.Visual = new CharacterImageIdentifierVM(null);
				}
			}
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x0004AE28 File Offset: 0x00049028
		public void RefreshCounts()
		{
			if (this.PartySize != this.Party.NumberOfHealthyMembers || this.PartyWoundedSize != this.Party.NumberOfAllMembers - this.Party.NumberOfHealthyMembers)
			{
				this.PartyWoundedSize = this.Party.NumberOfAllMembers - this.Party.NumberOfHealthyMembers;
				this.PartySize = this.Party.NumberOfHealthyMembers;
				MobileParty mobileParty = this.Party.MobileParty;
				this.PartySizeLbl = ((mobileParty != null && mobileParty.IsInfoHidden) ? "?" : this.Party.NumberOfHealthyMembers.ToString());
			}
			MBReadOnlyList<Ship> ships = this.Party.Ships;
			this.ShipCount = ((ships != null) ? ships.Count : 0);
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x0004AEEC File Offset: 0x000490EC
		public string GetPartyDescriptionTextFromValues()
		{
			GameTexts.SetVariable("newline", "\n");
			string text = ((this.Party.MobileParty.CurrentSettlement != null && this.Party.MobileParty.MapEvent == null) ? "" : CampaignUIHelper.GetMobilePartyBehaviorText(this.Party.MobileParty));
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_food", null).ToString());
			GameTexts.SetVariable("RIGHT", this.Party.MobileParty.Food);
			string text2 = GameTexts.FindText("str_LEFT_colon_RIGHT", null).ToString();
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_map_tooltip_speed", null).ToString());
			GameTexts.SetVariable("RIGHT", this.Party.MobileParty.Speed.ToString("F"));
			string text3 = GameTexts.FindText("str_LEFT_colon_RIGHT", null).ToString();
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_view_distance", null).ToString());
			GameTexts.SetVariable("RIGHT", this.Party.MobileParty.SeeingRange);
			string text4 = GameTexts.FindText("str_LEFT_colon_RIGHT", null).ToString();
			GameTexts.SetVariable("STR1", text);
			GameTexts.SetVariable("STR2", text2);
			string text5 = GameTexts.FindText("str_string_newline_string", null).ToString();
			GameTexts.SetVariable("STR1", text5);
			GameTexts.SetVariable("STR2", text3);
			text5 = GameTexts.FindText("str_string_newline_string", null).ToString();
			GameTexts.SetVariable("STR1", text5);
			GameTexts.SetVariable("STR2", text4);
			return GameTexts.FindText("str_string_newline_string", null).ToString();
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x0004B099 File Offset: 0x00049299
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.UnregisterEvents();
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x0004B0A7 File Offset: 0x000492A7
		private void RegisterEvents()
		{
			CampaignEvents.OnPlayerBodyPropertiesChangedEvent.AddNonSerializedListener(this, new Action(this.OnPlayerCharacterChangedEvent));
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x0004B0C0 File Offset: 0x000492C0
		private void OnPlayerCharacterChangedEvent()
		{
			CharacterObject characterObject = ((this.Party != null) ? PartyBaseHelper.GetVisualPartyLeader(this.Party) : this.Character);
			if (characterObject == CharacterObject.PlayerCharacter)
			{
				CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(characterObject, false);
				this.Visual = new CharacterImageIdentifierVM(characterCode);
			}
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x0004B105 File Offset: 0x00049305
		private void UnregisterEvents()
		{
			CampaignEventDispatcher.Instance.RemoveListeners(this);
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x0004B114 File Offset: 0x00049314
		private string GetEncyclopediaPageLink()
		{
			PartyBase party = this.Party;
			if (party == null || !party.MobileParty.IsCaravan)
			{
				PartyBase party2 = this.Party;
				if (party2 == null || !party2.MobileParty.IsGarrison)
				{
					PartyBase party3 = this.Party;
					if (party3 == null || !party3.MobileParty.IsMilitia)
					{
						PartyBase party4 = this.Party;
						if (party4 == null || !party4.MobileParty.IsVillager)
						{
							if (this.Character != null)
							{
								return this.Character.EncyclopediaLink;
							}
							if (this.Party != null)
							{
								if (this.Party.LeaderHero != null)
								{
									return this.Party.LeaderHero.EncyclopediaLink;
								}
								if (this.Party.Owner != null)
								{
									return this.Party.Owner.EncyclopediaLink;
								}
								CharacterObject visualPartyLeader = CampaignUIHelper.GetVisualPartyLeader(this.Party);
								if (visualPartyLeader != null)
								{
									return visualPartyLeader.EncyclopediaLink;
								}
							}
							return null;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001264 RID: 4708 RVA: 0x0004B1F6 File Offset: 0x000493F6
		// (set) Token: 0x06001265 RID: 4709 RVA: 0x0004B1FE File Offset: 0x000493FE
		[DataSourceProperty]
		public int Relation
		{
			get
			{
				return this._relation;
			}
			set
			{
				if (value != this._relation)
				{
					this._relation = value;
					base.OnPropertyChangedWithValue(value, "Relation");
				}
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001266 RID: 4710 RVA: 0x0004B21C File Offset: 0x0004941C
		// (set) Token: 0x06001267 RID: 4711 RVA: 0x0004B224 File Offset: 0x00049424
		[DataSourceProperty]
		public MBBindingList<QuestMarkerVM> Quests
		{
			get
			{
				return this._quests;
			}
			set
			{
				if (value != this._quests)
				{
					this._quests = value;
					base.OnPropertyChangedWithValue<MBBindingList<QuestMarkerVM>>(value, "Quests");
				}
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x0004B242 File Offset: 0x00049442
		// (set) Token: 0x06001269 RID: 4713 RVA: 0x0004B24A File Offset: 0x0004944A
		[DataSourceProperty]
		public bool IsHighlightEnabled
		{
			get
			{
				return this._isHighlightEnabled;
			}
			set
			{
				if (value != this._isHighlightEnabled)
				{
					this._isHighlightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsHighlightEnabled");
				}
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x0600126A RID: 4714 RVA: 0x0004B268 File Offset: 0x00049468
		// (set) Token: 0x0600126B RID: 4715 RVA: 0x0004B270 File Offset: 0x00049470
		[DataSourceProperty]
		public bool IsCharacterInPrison
		{
			get
			{
				return this._isCharacterInPrison;
			}
			set
			{
				if (value != this._isCharacterInPrison)
				{
					this._isCharacterInPrison = value;
					base.OnPropertyChangedWithValue(value, "IsCharacterInPrison");
				}
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x0600126C RID: 4716 RVA: 0x0004B28E File Offset: 0x0004948E
		// (set) Token: 0x0600126D RID: 4717 RVA: 0x0004B296 File Offset: 0x00049496
		[DataSourceProperty]
		public bool HasShips
		{
			get
			{
				return this._hasShips;
			}
			set
			{
				if (value != this._hasShips)
				{
					this._hasShips = value;
					base.OnPropertyChangedWithValue(value, "HasShips");
				}
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x0600126E RID: 4718 RVA: 0x0004B2B4 File Offset: 0x000494B4
		// (set) Token: 0x0600126F RID: 4719 RVA: 0x0004B2BC File Offset: 0x000494BC
		[DataSourceProperty]
		public bool IsIdle
		{
			get
			{
				return this._isIdle;
			}
			set
			{
				if (value != this._isIdle)
				{
					this._isIdle = value;
					base.OnPropertyChangedWithValue(value, "IsIdle");
				}
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001270 RID: 4720 RVA: 0x0004B2DA File Offset: 0x000494DA
		// (set) Token: 0x06001271 RID: 4721 RVA: 0x0004B2E2 File Offset: 0x000494E2
		[DataSourceProperty]
		public bool IsPlayer
		{
			get
			{
				return this._isPlayer;
			}
			set
			{
				if (value != this._isPlayer)
				{
					this._isPlayer = value;
					base.OnPropertyChanged("IsPlayerParty");
				}
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001272 RID: 4722 RVA: 0x0004B2FF File Offset: 0x000494FF
		// (set) Token: 0x06001273 RID: 4723 RVA: 0x0004B307 File Offset: 0x00049507
		[DataSourceProperty]
		public bool IsEnemy
		{
			get
			{
				return this._isEnemy;
			}
			set
			{
				if (value != this._isEnemy)
				{
					this._isEnemy = value;
					base.OnPropertyChangedWithValue(value, "IsEnemy");
				}
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001274 RID: 4724 RVA: 0x0004B325 File Offset: 0x00049525
		// (set) Token: 0x06001275 RID: 4725 RVA: 0x0004B32D File Offset: 0x0004952D
		[DataSourceProperty]
		public bool IsAlly
		{
			get
			{
				return this._isAlly;
			}
			set
			{
				if (value != this._isAlly)
				{
					this._isAlly = value;
					base.OnPropertyChangedWithValue(value, "IsAlly");
				}
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001276 RID: 4726 RVA: 0x0004B34B File Offset: 0x0004954B
		// (set) Token: 0x06001277 RID: 4727 RVA: 0x0004B353 File Offset: 0x00049553
		[DataSourceProperty]
		public bool IsNeutral
		{
			get
			{
				return this._isNeutral;
			}
			set
			{
				if (value != this._isNeutral)
				{
					this._isNeutral = value;
					base.OnPropertyChangedWithValue(value, "IsNeutral");
				}
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001278 RID: 4728 RVA: 0x0004B371 File Offset: 0x00049571
		// (set) Token: 0x06001279 RID: 4729 RVA: 0x0004B379 File Offset: 0x00049579
		[DataSourceProperty]
		public bool IsMergedWithArmy
		{
			get
			{
				return this._isMergedWithArmy;
			}
			set
			{
				if (value != this._isMergedWithArmy)
				{
					this._isMergedWithArmy = value;
					base.OnPropertyChangedWithValue(value, "IsMergedWithArmy");
				}
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x0600127A RID: 4730 RVA: 0x0004B397 File Offset: 0x00049597
		// (set) Token: 0x0600127B RID: 4731 RVA: 0x0004B39F File Offset: 0x0004959F
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x0600127C RID: 4732 RVA: 0x0004B3C2 File Offset: 0x000495C2
		// (set) Token: 0x0600127D RID: 4733 RVA: 0x0004B3CA File Offset: 0x000495CA
		[DataSourceProperty]
		public string SettlementPath
		{
			get
			{
				return this._settlementPath;
			}
			set
			{
				if (value != this._settlementPath)
				{
					this._settlementPath = value;
					base.OnPropertyChangedWithValue<string>(value, "SettlementPath");
				}
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x0600127E RID: 4734 RVA: 0x0004B3ED File Offset: 0x000495ED
		// (set) Token: 0x0600127F RID: 4735 RVA: 0x0004B3F5 File Offset: 0x000495F5
		[DataSourceProperty]
		public string LocationText
		{
			get
			{
				return this._locationText;
			}
			set
			{
				if (value != this._locationText)
				{
					this._locationText = value;
					base.OnPropertyChangedWithValue<string>(value, "LocationText");
				}
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001280 RID: 4736 RVA: 0x0004B418 File Offset: 0x00049618
		// (set) Token: 0x06001281 RID: 4737 RVA: 0x0004B420 File Offset: 0x00049620
		[DataSourceProperty]
		public string PowerText
		{
			get
			{
				return this._powerText;
			}
			set
			{
				if (value != this._powerText)
				{
					this._powerText = value;
					base.OnPropertyChangedWithValue<string>(value, "PowerText");
				}
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001282 RID: 4738 RVA: 0x0004B443 File Offset: 0x00049643
		// (set) Token: 0x06001283 RID: 4739 RVA: 0x0004B44B File Offset: 0x0004964B
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001284 RID: 4740 RVA: 0x0004B46E File Offset: 0x0004966E
		// (set) Token: 0x06001285 RID: 4741 RVA: 0x0004B476 File Offset: 0x00049676
		[DataSourceProperty]
		public string ProfessionText
		{
			get
			{
				return this._professionText;
			}
			set
			{
				if (value != this._professionText)
				{
					this._professionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProfessionText");
				}
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001286 RID: 4742 RVA: 0x0004B499 File Offset: 0x00049699
		// (set) Token: 0x06001287 RID: 4743 RVA: 0x0004B4A1 File Offset: 0x000496A1
		[DataSourceProperty]
		public string EncyclopediaCursorEffect
		{
			get
			{
				return this._encyclopediaCursorEffect;
			}
			set
			{
				if (value != this._encyclopediaCursorEffect)
				{
					this._encyclopediaCursorEffect = value;
					base.OnPropertyChangedWithValue<string>(value, "EncyclopediaCursorEffect");
				}
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001288 RID: 4744 RVA: 0x0004B4C4 File Offset: 0x000496C4
		// (set) Token: 0x06001289 RID: 4745 RVA: 0x0004B4CC File Offset: 0x000496CC
		[DataSourceProperty]
		public CharacterImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x0600128A RID: 4746 RVA: 0x0004B4EA File Offset: 0x000496EA
		// (set) Token: 0x0600128B RID: 4747 RVA: 0x0004B4F2 File Offset: 0x000496F2
		[DataSourceProperty]
		public BannerImageIdentifierVM Banner_9
		{
			get
			{
				return this._banner_9;
			}
			set
			{
				if (value != this._banner_9)
				{
					this._banner_9 = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Banner_9");
				}
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x0600128C RID: 4748 RVA: 0x0004B510 File Offset: 0x00049710
		// (set) Token: 0x0600128D RID: 4749 RVA: 0x0004B518 File Offset: 0x00049718
		[DataSourceProperty]
		public int PartySize
		{
			get
			{
				return this._partySize;
			}
			set
			{
				if (value != this._partySize)
				{
					this._partySize = value;
					base.OnPropertyChangedWithValue(value, "PartySize");
				}
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x0600128E RID: 4750 RVA: 0x0004B536 File Offset: 0x00049736
		// (set) Token: 0x0600128F RID: 4751 RVA: 0x0004B53E File Offset: 0x0004973E
		[DataSourceProperty]
		public int PartyWoundedSize
		{
			get
			{
				return this._partyWoundedSize;
			}
			set
			{
				if (value != this._partySize)
				{
					this._partyWoundedSize = value;
					base.OnPropertyChangedWithValue(value, "PartyWoundedSize");
				}
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001290 RID: 4752 RVA: 0x0004B55C File Offset: 0x0004975C
		// (set) Token: 0x06001291 RID: 4753 RVA: 0x0004B564 File Offset: 0x00049764
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

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001292 RID: 4754 RVA: 0x0004B582 File Offset: 0x00049782
		// (set) Token: 0x06001293 RID: 4755 RVA: 0x0004B58A File Offset: 0x0004978A
		[DataSourceProperty]
		public string PartySizeLbl
		{
			get
			{
				return this._partySizeLbl;
			}
			set
			{
				if (value != this._partySizeLbl)
				{
					this._partySizeLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "PartySizeLbl");
				}
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001294 RID: 4756 RVA: 0x0004B5AD File Offset: 0x000497AD
		// (set) Token: 0x06001295 RID: 4757 RVA: 0x0004B5B5 File Offset: 0x000497B5
		[DataSourceProperty]
		public bool IsLeader
		{
			get
			{
				return this._isLeader;
			}
			set
			{
				if (value != this._isLeader)
				{
					this._isLeader = value;
					base.OnPropertyChangedWithValue(value, "IsLeader");
				}
			}
		}

		// Token: 0x0400085B RID: 2139
		public CharacterObject Character;

		// Token: 0x0400085C RID: 2140
		public PartyBase Party;

		// Token: 0x0400085D RID: 2141
		public Settlement Settlement;

		// Token: 0x0400085E RID: 2142
		private readonly bool _canShowQuest = true;

		// Token: 0x0400085F RID: 2143
		private readonly bool _useCivilianEquipment;

		// Token: 0x04000860 RID: 2144
		private readonly Action<GameMenuPartyItemVM> _onSetAsContextMenuActiveItem;

		// Token: 0x04000861 RID: 2145
		private MBBindingList<QuestMarkerVM> _quests;

		// Token: 0x04000862 RID: 2146
		private int _partySize;

		// Token: 0x04000863 RID: 2147
		private int _partyWoundedSize;

		// Token: 0x04000864 RID: 2148
		private int _shipCount;

		// Token: 0x04000865 RID: 2149
		private int _relation = -101;

		// Token: 0x04000866 RID: 2150
		private CharacterImageIdentifierVM _visual;

		// Token: 0x04000867 RID: 2151
		private BannerImageIdentifierVM _banner_9;

		// Token: 0x04000868 RID: 2152
		private string _settlementPath;

		// Token: 0x04000869 RID: 2153
		private string _partySizeLbl;

		// Token: 0x0400086A RID: 2154
		private string _nameText;

		// Token: 0x0400086B RID: 2155
		private string _locationText;

		// Token: 0x0400086C RID: 2156
		private string _descriptionText;

		// Token: 0x0400086D RID: 2157
		private string _professionText;

		// Token: 0x0400086E RID: 2158
		private string _powerText;

		// Token: 0x0400086F RID: 2159
		private string _encyclopediaCursorEffect;

		// Token: 0x04000870 RID: 2160
		private bool _isIdle;

		// Token: 0x04000871 RID: 2161
		private bool _isPlayer;

		// Token: 0x04000872 RID: 2162
		private bool _isEnemy;

		// Token: 0x04000873 RID: 2163
		private bool _isAlly;

		// Token: 0x04000874 RID: 2164
		private bool _isNeutral;

		// Token: 0x04000875 RID: 2165
		private bool _isHighlightEnabled;

		// Token: 0x04000876 RID: 2166
		private bool _isLeader;

		// Token: 0x04000877 RID: 2167
		private bool _isMergedWithArmy;

		// Token: 0x04000878 RID: 2168
		private bool _isCharacterInPrison;

		// Token: 0x04000879 RID: 2169
		private bool _hasShips;

		// Token: 0x02000233 RID: 563
		private class QuestMarkerComparer : IComparer<QuestMarkerVM>
		{
			// Token: 0x060024DE RID: 9438 RVA: 0x00080FD4 File Offset: 0x0007F1D4
			public int Compare(QuestMarkerVM x, QuestMarkerVM y)
			{
				return x.QuestMarkerType.CompareTo(y.QuestMarkerType);
			}
		}
	}
}
