using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Conversation
{
	// Token: 0x02000118 RID: 280
	public class MissionConversationVM : ViewModel
	{
		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06001999 RID: 6553 RVA: 0x0006123F File Offset: 0x0005F43F
		// (set) Token: 0x0600199A RID: 6554 RVA: 0x00061247 File Offset: 0x0005F447
		public bool SelectedAnOptionOrLinkThisFrame { get; set; }

		// Token: 0x0600199B RID: 6555 RVA: 0x00061250 File Offset: 0x0005F450
		public MissionConversationVM(Func<string> getContinueInputText, bool isLinksDisabled = false)
		{
			this.AnswerList = new MBBindingList<ConversationItemVM>();
			this.AttackerParties = new MBBindingList<ConversationAggressivePartyItemVM>();
			this.DefenderParties = new MBBindingList<ConversationAggressivePartyItemVM>();
			this._conversationManager = Campaign.Current.ConversationManager;
			this._getContinueInputText = getContinueInputText;
			this._isLinksDisabled = isLinksDisabled;
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(Input.OnGamepadActiveStateChanged, new Action(this.RefreshValues));
			CampaignEvents.PersuasionProgressCommittedEvent.AddNonSerializedListener(this, new Action<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>(this.OnPersuasionProgress));
			this.Persuasion = new PersuasionVM(this._conversationManager);
			if (this._conversationManager.SpeakerAgent != null && (CharacterObject)this._conversationManager.SpeakerAgent.Character != null && ((CharacterObject)this._conversationManager.SpeakerAgent.Character).IsHero && this._conversationManager.SpeakerAgent.Character != CharacterObject.PlayerCharacter)
			{
				Hero heroObject = ((CharacterObject)this._conversationManager.SpeakerAgent.Character).HeroObject;
				this.Relation = (int)heroObject.GetRelationWithPlayer();
			}
			this.IsAggressive = Campaign.Current.CurrentConversationContext == ConversationContext.PartyEncounter && this._conversationManager.ConversationParty != null && FactionManager.IsAtWarAgainstFaction(this._conversationManager.ConversationParty.MapFaction, Hero.MainHero.MapFaction);
			this.OnAgressiveStateUpdated();
			this.ExecuteSetCurrentAnswer(null);
			this.RefreshValues();
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x000613C0 File Offset: 0x0005F5C0
		private void OnPersuasionProgress(Tuple<PersuasionOptionArgs, PersuasionOptionResult> result)
		{
			PersuasionVM persuasion = this.Persuasion;
			if (persuasion != null)
			{
				persuasion.OnPersuasionProgress(result);
			}
			this.AnswerList.ApplyActionOnAllItems(delegate(ConversationItemVM a)
			{
				a.OnPersuasionProgress(result);
			});
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x00061408 File Offset: 0x0005F608
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ContinueText = this._getContinueInputText();
			this.MoreOptionText = GameTexts.FindText("str_more_brackets", null).ToString();
			this.PersuasionText = GameTexts.FindText("str_persuasion", null).ToString();
			this.RelationHint = new HintViewModel(GameTexts.FindText("str_tooltip_label_relation", null), null);
			this.GoldHint = new HintViewModel(new TextObject("{=o5G8A8ZH}Your Denars", null), null);
			this._answerList.ApplyActionOnAllItems(delegate(ConversationItemVM x)
			{
				x.RefreshValues();
			});
			this._defenderParties.ApplyActionOnAllItems(delegate(ConversationAggressivePartyItemVM x)
			{
				x.RefreshValues();
			});
			this._attackerParties.ApplyActionOnAllItems(delegate(ConversationAggressivePartyItemVM x)
			{
				x.RefreshValues();
			});
			this._defenderLeader.RefreshValues();
			this._attackerLeader.RefreshValues();
			this._currentSelectedAnswer.RefreshValues();
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x00061528 File Offset: 0x0005F728
		public void Tick(float dt)
		{
			this.IsAggressive = Campaign.Current.CurrentConversationContext == ConversationContext.PartyEncounter && this._conversationManager.ConversationParty != null && FactionManager.IsAtWarAgainstFaction(this._conversationManager.ConversationParty.MapFaction, Hero.MainHero.MapFaction);
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x00061578 File Offset: 0x0005F778
		private void OnAgressiveStateUpdated()
		{
			if (this.IsAggressive)
			{
				List<MobileParty> list = new List<MobileParty>();
				List<MobileParty> list2 = new List<MobileParty>();
				this.DefenderParties.Clear();
				this.AttackerParties.Clear();
				MobileParty conversationParty = this._conversationManager.ConversationParty;
				MobileParty mainParty = MobileParty.MainParty;
				if (PlayerEncounter.PlayerIsAttacker)
				{
					list2.Add(mainParty);
					list.Add(conversationParty);
					PlayerEncounter.Current.FindAllNpcPartiesWhoWillJoinEvent(list2, list);
				}
				else
				{
					list2.Add(conversationParty);
					list.Add(mainParty);
					PlayerEncounter.Current.FindAllNpcPartiesWhoWillJoinEvent(list, list2);
				}
				this.AttackerLeader = new ConversationAggressivePartyItemVM(PlayerEncounter.PlayerIsAttacker ? mainParty : conversationParty, null);
				this.DefenderLeader = new ConversationAggressivePartyItemVM(PlayerEncounter.PlayerIsAttacker ? conversationParty : mainParty, null);
				double num = 0.0;
				double num2 = 0.0;
				num += (double)this.DefenderLeader.Party.Party.CalculateCurrentStrength();
				num2 += (double)this.AttackerLeader.Party.Party.CalculateCurrentStrength();
				foreach (MobileParty mobileParty in list)
				{
					if (mobileParty != conversationParty && mobileParty != mainParty)
					{
						num += (double)mobileParty.Party.CalculateCurrentStrength();
						this.DefenderParties.Add(new ConversationAggressivePartyItemVM(mobileParty, null));
					}
				}
				foreach (MobileParty mobileParty2 in list2)
				{
					if (mobileParty2 != conversationParty && mobileParty2 != mainParty)
					{
						num2 += (double)mobileParty2.Party.CalculateCurrentStrength();
						this.AttackerParties.Add(new ConversationAggressivePartyItemVM(mobileParty2, null));
					}
				}
				string text;
				if (this.DefenderLeader.Party.MapFaction != null && this.DefenderLeader.Party.MapFaction is Kingdom)
				{
					text = Color.FromUint(((Kingdom)this.DefenderLeader.Party.MapFaction).PrimaryBannerColor).ToString();
				}
				else
				{
					text = Color.FromUint(this.DefenderLeader.Party.MapFaction.Banner.GetPrimaryColor()).ToString();
				}
				string text2;
				if (this.AttackerLeader.Party.MapFaction != null && this.AttackerLeader.Party.MapFaction is Kingdom)
				{
					text2 = Color.FromUint(((Kingdom)this.AttackerLeader.Party.MapFaction).PrimaryBannerColor).ToString();
				}
				else
				{
					text2 = Color.FromUint(this.AttackerLeader.Party.MapFaction.Banner.GetPrimaryColor()).ToString();
				}
				if (!list2.AnyQ<MobileParty>((MobileParty p) => p.IsInfoHidden))
				{
					if (!list.AnyQ<MobileParty>((MobileParty p) => p.IsInfoHidden))
					{
						goto IL_0345;
					}
				}
				if (PlayerEncounter.PlayerIsAttacker)
				{
					num2 = 0.0;
					num = 1.0;
				}
				else
				{
					num2 = 1.0;
					num = 0.0;
				}
				IL_0345:
				this.PowerComparer = new PowerLevelComparer(num, num2);
				this.PowerComparer.SetColors(text, text2);
				return;
			}
			this.DefenderLeader = new ConversationAggressivePartyItemVM(null, null);
			this.AttackerLeader = new ConversationAggressivePartyItemVM(null, null);
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x00061920 File Offset: 0x0005FB20
		public void OnConversationContinue()
		{
			if (ConversationManager.GetPersuasionIsActive() && (!ConversationManager.GetPersuasionIsActive() || this.IsPersuading))
			{
				List<ConversationSentenceOption> curOptions = this._conversationManager.CurOptions;
				if (((curOptions != null) ? curOptions.Count : 0) > 1)
				{
					return;
				}
			}
			this.Refresh();
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x00061958 File Offset: 0x0005FB58
		public void ExecuteLink(string link)
		{
			if (!this._isLinksDisabled)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(link);
			}
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x00061974 File Offset: 0x0005FB74
		public void ExecuteConversedHeroLink()
		{
			CharacterObject characterObject;
			if (!this._isLinksDisabled && (characterObject = this._currentDialogCharacter as CharacterObject) != null)
			{
				EncyclopediaManager encyclopediaManager = Campaign.Current.EncyclopediaManager;
				Hero heroObject = characterObject.HeroObject;
				encyclopediaManager.GoToLink(((heroObject != null) ? heroObject.EncyclopediaLink : null) ?? characterObject.EncyclopediaLink);
				this.SelectedAnOptionOrLinkThisFrame = true;
			}
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x000619CC File Offset: 0x0005FBCC
		public void Refresh()
		{
			this.ExecuteCloseTooltip();
			this._isProcessingOption = false;
			this.IsLoadingOver = false;
			IReadOnlyList<IAgent> conversationAgents = this._conversationManager.ConversationAgents;
			if (conversationAgents != null && conversationAgents.Count > 0)
			{
				this._currentDialogCharacter = this._conversationManager.SpeakerAgent.Character;
				this.CurrentCharacterNameLbl = this._currentDialogCharacter.Name.ToString();
				this.IsCurrentCharacterValidInEncyclopedia = false;
				if (((CharacterObject)this._currentDialogCharacter).IsHero && this._currentDialogCharacter != CharacterObject.PlayerCharacter)
				{
					this.MinRelation = Campaign.Current.Models.DiplomacyModel.MinRelationLimit;
					this.MaxRelation = Campaign.Current.Models.DiplomacyModel.MaxRelationLimit;
					Hero heroObject = ((CharacterObject)this._currentDialogCharacter).HeroObject;
					if (heroObject.IsLord && !heroObject.IsMinorFactionHero)
					{
						Clan clan = heroObject.Clan;
						if (((clan != null) ? clan.Leader : null) == heroObject)
						{
							Clan clan2 = heroObject.Clan;
							if (((clan2 != null) ? clan2.Kingdom : null) != null)
							{
								string stringId = heroObject.MapFaction.Culture.StringId;
								TextObject textObject;
								if (GameTexts.TryGetText("str_faction_noble_name_with_title", out textObject, stringId))
								{
									if (heroObject.Clan.Kingdom.Leader == heroObject)
									{
										textObject = GameTexts.FindText("str_faction_ruler_name_with_title", stringId);
									}
									StringHelpers.SetCharacterProperties("RULER", (CharacterObject)this._currentDialogCharacter, null, false);
									this.CurrentCharacterNameLbl = textObject.ToString();
								}
							}
						}
					}
					this.IsRelationEnabled = true;
					this.Relation = Hero.MainHero.GetRelation(heroObject);
					GameTexts.SetVariable("NUM", this.Relation.ToString());
					if (this.Relation > 0)
					{
						this.RelationText = "+" + this.Relation;
					}
					else if (this.Relation < 0)
					{
						this.RelationText = "-" + MathF.Abs(this.Relation);
					}
					else
					{
						this.RelationText = this.Relation.ToString();
					}
					if (heroObject.Clan == null)
					{
						this.ConversedHeroBanner = new BannerImageIdentifierVM(null, false);
						this.IsRelationEnabled = false;
						this.IsBannerEnabled = false;
					}
					else
					{
						this.ConversedHeroBanner = ((heroObject != null) ? new BannerImageIdentifierVM(heroObject.ClanBanner, false) : new BannerImageIdentifierVM(null, false));
						TextObject textObject2 = ((heroObject != null) ? heroObject.Clan.Name : TextObject.GetEmpty());
						this.FactionHint = new HintViewModel(textObject2, null);
						this.IsBannerEnabled = true;
					}
					this.IsCurrentCharacterValidInEncyclopedia = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Hero)).IsValidEncyclopediaItem(heroObject);
				}
				else
				{
					this.ConversedHeroBanner = new BannerImageIdentifierVM(null, false);
					this.IsRelationEnabled = false;
					this.IsBannerEnabled = false;
					this.IsCurrentCharacterValidInEncyclopedia = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(CharacterObject)).IsValidEncyclopediaItem((CharacterObject)this._conversationManager.SpeakerAgent.Character);
				}
			}
			this.DialogText = this._conversationManager.CurrentSentenceText;
			this.AnswerList.Clear();
			MissionConversationVM._isCurrentlyPlayerSpeaking = this._currentDialogCharacter == Hero.MainHero.CharacterObject;
			this._conversationManager.GetPlayerSentenceOptions();
			List<ConversationSentenceOption> curOptions = this._conversationManager.CurOptions;
			int num = ((curOptions != null) ? curOptions.Count : 0);
			if (num > 0 && !MissionConversationVM._isCurrentlyPlayerSpeaking)
			{
				for (int i = 0; i < num; i++)
				{
					this.AnswerList.Add(new ConversationItemVM(new Action<int>(this.OnSelectOption), new Action(this.OnReadyToContinue), new Action<ConversationItemVM>(this.ExecuteSetCurrentAnswer), i));
				}
			}
			this.GoldText = CampaignUIHelper.GetAbbreviatedValueTextFromValue(Hero.MainHero.Gold);
			this.IsPersuading = ConversationManager.GetPersuasionIsActive();
			if (this.IsPersuading)
			{
				this.CurrentSelectedAnswer = new ConversationItemVM();
			}
			this.IsLoadingOver = true;
			PersuasionVM persuasion = this.Persuasion;
			if (persuasion == null)
			{
				return;
			}
			persuasion.RefreshPersusasion();
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x00061DBD File Offset: 0x0005FFBD
		private void OnReadyToContinue()
		{
			this.Refresh();
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x00061DC8 File Offset: 0x0005FFC8
		private void ExecuteDefenderTooltip()
		{
			if (PlayerEncounter.PlayerIsDefender)
			{
				InformationManager.ShowTooltip(typeof(List<MobileParty>), new object[] { 0 });
				return;
			}
			InformationManager.ShowTooltip(typeof(List<MobileParty>), new object[] { 1 });
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x00061E19 File Offset: 0x00060019
		public void ExecuteCloseTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x00061E20 File Offset: 0x00060020
		public void ExecuteHeroTooltip()
		{
			CharacterObject characterObject = (CharacterObject)this._currentDialogCharacter;
			if (characterObject != null && characterObject.IsHero)
			{
				InformationManager.ShowTooltip(typeof(Hero), new object[] { characterObject.HeroObject, true });
			}
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x00061E6C File Offset: 0x0006006C
		private void ExecuteAttackerTooltip()
		{
			if (PlayerEncounter.PlayerIsAttacker)
			{
				InformationManager.ShowTooltip(typeof(List<MobileParty>), new object[] { 0 });
				return;
			}
			InformationManager.ShowTooltip(typeof(List<MobileParty>), new object[] { 1 });
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x00061EC0 File Offset: 0x000600C0
		private void ExecuteHeroInfo()
		{
			if (this._conversationManager.ListenerAgent.Character == Hero.MainHero.CharacterObject)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(Hero.MainHero.EncyclopediaLink);
				return;
			}
			if (CharacterObject.OneToOneConversationCharacter.IsHero)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(CharacterObject.OneToOneConversationCharacter.HeroObject.EncyclopediaLink);
				return;
			}
			Campaign.Current.EncyclopediaManager.GoToLink(CharacterObject.OneToOneConversationCharacter.EncyclopediaLink);
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x00061F47 File Offset: 0x00060147
		private void OnSelectOption(int optionIndex)
		{
			if (!this._isProcessingOption)
			{
				this._isProcessingOption = true;
				this._conversationManager.DoOption(optionIndex);
				PersuasionVM persuasion = this.Persuasion;
				if (persuasion != null)
				{
					persuasion.RefreshPersusasion();
				}
				this.SelectedAnOptionOrLinkThisFrame = true;
			}
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x00061F7C File Offset: 0x0006017C
		public void ExecuteFinalizeSelection()
		{
			this.Refresh();
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x00061F84 File Offset: 0x00060184
		public void ExecuteContinue()
		{
			Debug.Print("ExecuteContinue", 0, Debug.DebugColor.White, 17592186044416UL);
			this._conversationManager.ContinueConversation();
			this._isProcessingOption = false;
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x00061FAE File Offset: 0x000601AE
		private void ExecuteSetCurrentAnswer(ConversationItemVM _answer)
		{
			this.Persuasion.SetCurrentOption((_answer != null) ? _answer.PersuasionItem : null);
			if (_answer != null)
			{
				this.CurrentSelectedAnswer = _answer;
				return;
			}
			this.CurrentSelectedAnswer = new ConversationItemVM();
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x00061FE0 File Offset: 0x000601E0
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.PersuasionProgressCommittedEvent.ClearListeners(this);
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(Input.OnGamepadActiveStateChanged, new Action(this.RefreshValues));
			PersuasionVM persuasion = this.Persuasion;
			if (persuasion == null)
			{
				return;
			}
			persuasion.OnFinalize();
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x060019AF RID: 6575 RVA: 0x0006202F File Offset: 0x0006022F
		// (set) Token: 0x060019B0 RID: 6576 RVA: 0x00062037 File Offset: 0x00060237
		[DataSourceProperty]
		public PersuasionVM Persuasion
		{
			get
			{
				return this._persuasion;
			}
			set
			{
				if (value != this._persuasion)
				{
					this._persuasion = value;
					base.OnPropertyChangedWithValue<PersuasionVM>(value, "Persuasion");
				}
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x060019B1 RID: 6577 RVA: 0x00062055 File Offset: 0x00060255
		// (set) Token: 0x060019B2 RID: 6578 RVA: 0x0006205D File Offset: 0x0006025D
		[DataSourceProperty]
		public PowerLevelComparer PowerComparer
		{
			get
			{
				return this._powerComparer;
			}
			set
			{
				if (value != this._powerComparer)
				{
					this._powerComparer = value;
					base.OnPropertyChangedWithValue<PowerLevelComparer>(value, "PowerComparer");
				}
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x060019B3 RID: 6579 RVA: 0x0006207B File Offset: 0x0006027B
		// (set) Token: 0x060019B4 RID: 6580 RVA: 0x00062083 File Offset: 0x00060283
		[DataSourceProperty]
		public int Relation
		{
			get
			{
				return this._relation;
			}
			set
			{
				if (this._relation != value)
				{
					this._relation = value;
					base.OnPropertyChangedWithValue(value, "Relation");
				}
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x060019B5 RID: 6581 RVA: 0x000620A1 File Offset: 0x000602A1
		// (set) Token: 0x060019B6 RID: 6582 RVA: 0x000620A9 File Offset: 0x000602A9
		[DataSourceProperty]
		public int MinRelation
		{
			get
			{
				return this._minRelation;
			}
			set
			{
				if (this._minRelation != value)
				{
					this._minRelation = value;
					base.OnPropertyChangedWithValue(value, "MinRelation");
				}
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x060019B7 RID: 6583 RVA: 0x000620C7 File Offset: 0x000602C7
		// (set) Token: 0x060019B8 RID: 6584 RVA: 0x000620CF File Offset: 0x000602CF
		[DataSourceProperty]
		public int MaxRelation
		{
			get
			{
				return this._maxRelation;
			}
			set
			{
				if (this._maxRelation != value)
				{
					this._maxRelation = value;
					base.OnPropertyChangedWithValue(value, "MaxRelation");
				}
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x060019B9 RID: 6585 RVA: 0x000620ED File Offset: 0x000602ED
		// (set) Token: 0x060019BA RID: 6586 RVA: 0x000620F5 File Offset: 0x000602F5
		[DataSourceProperty]
		public ConversationAggressivePartyItemVM DefenderLeader
		{
			get
			{
				return this._defenderLeader;
			}
			set
			{
				if (value != this._defenderLeader)
				{
					this._defenderLeader = value;
					base.OnPropertyChangedWithValue<ConversationAggressivePartyItemVM>(value, "DefenderLeader");
				}
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x060019BB RID: 6587 RVA: 0x00062113 File Offset: 0x00060313
		// (set) Token: 0x060019BC RID: 6588 RVA: 0x0006211B File Offset: 0x0006031B
		[DataSourceProperty]
		public ConversationAggressivePartyItemVM AttackerLeader
		{
			get
			{
				return this._attackerLeader;
			}
			set
			{
				if (value != this._attackerLeader)
				{
					this._attackerLeader = value;
					base.OnPropertyChangedWithValue<ConversationAggressivePartyItemVM>(value, "AttackerLeader");
				}
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x060019BD RID: 6589 RVA: 0x00062139 File Offset: 0x00060339
		// (set) Token: 0x060019BE RID: 6590 RVA: 0x00062141 File Offset: 0x00060341
		[DataSourceProperty]
		public MBBindingList<ConversationAggressivePartyItemVM> AttackerParties
		{
			get
			{
				return this._attackerParties;
			}
			set
			{
				if (value != this._attackerParties)
				{
					this._attackerParties = value;
					base.OnPropertyChangedWithValue<MBBindingList<ConversationAggressivePartyItemVM>>(value, "AttackerParties");
				}
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x060019BF RID: 6591 RVA: 0x0006215F File Offset: 0x0006035F
		// (set) Token: 0x060019C0 RID: 6592 RVA: 0x00062167 File Offset: 0x00060367
		[DataSourceProperty]
		public MBBindingList<ConversationAggressivePartyItemVM> DefenderParties
		{
			get
			{
				return this._defenderParties;
			}
			set
			{
				if (value != this._defenderParties)
				{
					this._defenderParties = value;
					base.OnPropertyChangedWithValue<MBBindingList<ConversationAggressivePartyItemVM>>(value, "DefenderParties");
				}
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x060019C1 RID: 6593 RVA: 0x00062185 File Offset: 0x00060385
		// (set) Token: 0x060019C2 RID: 6594 RVA: 0x0006218D File Offset: 0x0006038D
		[DataSourceProperty]
		public string MoreOptionText
		{
			get
			{
				return this._moreOptionText;
			}
			set
			{
				if (this._moreOptionText != value)
				{
					this._moreOptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "MoreOptionText");
				}
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x060019C3 RID: 6595 RVA: 0x000621B0 File Offset: 0x000603B0
		// (set) Token: 0x060019C4 RID: 6596 RVA: 0x000621B8 File Offset: 0x000603B8
		[DataSourceProperty]
		public string GoldText
		{
			get
			{
				return this._goldText;
			}
			set
			{
				if (this._goldText != value)
				{
					this._goldText = value;
					base.OnPropertyChangedWithValue<string>(value, "GoldText");
				}
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x060019C5 RID: 6597 RVA: 0x000621DB File Offset: 0x000603DB
		// (set) Token: 0x060019C6 RID: 6598 RVA: 0x000621E3 File Offset: 0x000603E3
		[DataSourceProperty]
		public string PersuasionText
		{
			get
			{
				return this._persuasionText;
			}
			set
			{
				if (this._persuasionText != value)
				{
					this._persuasionText = value;
					base.OnPropertyChangedWithValue<string>(value, "PersuasionText");
				}
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x060019C7 RID: 6599 RVA: 0x00062206 File Offset: 0x00060406
		// (set) Token: 0x060019C8 RID: 6600 RVA: 0x0006220E File Offset: 0x0006040E
		[DataSourceProperty]
		public bool IsCurrentCharacterValidInEncyclopedia
		{
			get
			{
				return this._isCurrentCharacterValidInEncyclopedia;
			}
			set
			{
				if (this._isCurrentCharacterValidInEncyclopedia != value)
				{
					this._isCurrentCharacterValidInEncyclopedia = value;
					base.OnPropertyChangedWithValue(value, "IsCurrentCharacterValidInEncyclopedia");
				}
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x060019C9 RID: 6601 RVA: 0x0006222C File Offset: 0x0006042C
		// (set) Token: 0x060019CA RID: 6602 RVA: 0x00062234 File Offset: 0x00060434
		[DataSourceProperty]
		public bool IsLoadingOver
		{
			get
			{
				return this._isLoadingOver;
			}
			set
			{
				if (this._isLoadingOver != value)
				{
					this._isLoadingOver = value;
					base.OnPropertyChangedWithValue(value, "IsLoadingOver");
				}
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x060019CB RID: 6603 RVA: 0x00062252 File Offset: 0x00060452
		// (set) Token: 0x060019CC RID: 6604 RVA: 0x0006225A File Offset: 0x0006045A
		[DataSourceProperty]
		public bool IsPersuading
		{
			get
			{
				return this._isPersuading;
			}
			set
			{
				if (this._isPersuading != value)
				{
					this._isPersuading = value;
					base.OnPropertyChangedWithValue(value, "IsPersuading");
				}
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x060019CD RID: 6605 RVA: 0x00062278 File Offset: 0x00060478
		// (set) Token: 0x060019CE RID: 6606 RVA: 0x00062280 File Offset: 0x00060480
		[DataSourceProperty]
		public string ContinueText
		{
			get
			{
				return this._continueText;
			}
			set
			{
				if (this._continueText != value)
				{
					this._continueText = value;
					base.OnPropertyChangedWithValue<string>(value, "ContinueText");
				}
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x060019CF RID: 6607 RVA: 0x000622A3 File Offset: 0x000604A3
		// (set) Token: 0x060019D0 RID: 6608 RVA: 0x000622AB File Offset: 0x000604AB
		[DataSourceProperty]
		public string CurrentCharacterNameLbl
		{
			get
			{
				return this._currentCharacterNameLbl;
			}
			set
			{
				if (this._currentCharacterNameLbl != value)
				{
					this._currentCharacterNameLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentCharacterNameLbl");
				}
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x060019D1 RID: 6609 RVA: 0x000622CE File Offset: 0x000604CE
		// (set) Token: 0x060019D2 RID: 6610 RVA: 0x000622D6 File Offset: 0x000604D6
		[DataSourceProperty]
		public MBBindingList<ConversationItemVM> AnswerList
		{
			get
			{
				return this._answerList;
			}
			set
			{
				if (this._answerList != value)
				{
					this._answerList = value;
					base.OnPropertyChangedWithValue<MBBindingList<ConversationItemVM>>(value, "AnswerList");
				}
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x060019D3 RID: 6611 RVA: 0x000622F4 File Offset: 0x000604F4
		// (set) Token: 0x060019D4 RID: 6612 RVA: 0x000622FC File Offset: 0x000604FC
		[DataSourceProperty]
		public string DialogText
		{
			get
			{
				return this._dialogText;
			}
			set
			{
				if (this._dialogText != value)
				{
					this._dialogText = value;
					base.OnPropertyChangedWithValue<string>(value, "DialogText");
				}
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x060019D5 RID: 6613 RVA: 0x0006231F File Offset: 0x0006051F
		// (set) Token: 0x060019D6 RID: 6614 RVA: 0x00062327 File Offset: 0x00060527
		[DataSourceProperty]
		public bool IsAggressive
		{
			get
			{
				return this._isAggressive;
			}
			set
			{
				if (value != this._isAggressive)
				{
					this._isAggressive = value;
					base.OnPropertyChangedWithValue(value, "IsAggressive");
					this.OnAgressiveStateUpdated();
				}
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x060019D7 RID: 6615 RVA: 0x0006234B File Offset: 0x0006054B
		// (set) Token: 0x060019D8 RID: 6616 RVA: 0x00062353 File Offset: 0x00060553
		[DataSourceProperty]
		public int SelectedSide
		{
			get
			{
				return this._selectedSide;
			}
			set
			{
				if (value != this._selectedSide)
				{
					this._selectedSide = value;
					base.OnPropertyChangedWithValue(value, "SelectedSide");
				}
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x060019D9 RID: 6617 RVA: 0x00062371 File Offset: 0x00060571
		// (set) Token: 0x060019DA RID: 6618 RVA: 0x00062379 File Offset: 0x00060579
		[DataSourceProperty]
		public string RelationText
		{
			get
			{
				return this._relationText;
			}
			set
			{
				if (this._relationText != value)
				{
					this._relationText = value;
					base.OnPropertyChangedWithValue<string>(value, "RelationText");
				}
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x060019DB RID: 6619 RVA: 0x0006239C File Offset: 0x0006059C
		// (set) Token: 0x060019DC RID: 6620 RVA: 0x000623A4 File Offset: 0x000605A4
		[DataSourceProperty]
		public bool IsRelationEnabled
		{
			get
			{
				return this._isRelationEnabled;
			}
			set
			{
				if (value != this._isRelationEnabled)
				{
					this._isRelationEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsRelationEnabled");
				}
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x060019DD RID: 6621 RVA: 0x000623C2 File Offset: 0x000605C2
		// (set) Token: 0x060019DE RID: 6622 RVA: 0x000623CA File Offset: 0x000605CA
		[DataSourceProperty]
		public bool IsBannerEnabled
		{
			get
			{
				return this._isBannerEnabled;
			}
			set
			{
				if (value != this._isBannerEnabled)
				{
					this._isBannerEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsBannerEnabled");
				}
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x060019DF RID: 6623 RVA: 0x000623E8 File Offset: 0x000605E8
		// (set) Token: 0x060019E0 RID: 6624 RVA: 0x000623F0 File Offset: 0x000605F0
		[DataSourceProperty]
		public ConversationItemVM CurrentSelectedAnswer
		{
			get
			{
				return this._currentSelectedAnswer;
			}
			set
			{
				if (this._currentSelectedAnswer != value)
				{
					this._currentSelectedAnswer = value;
					base.OnPropertyChangedWithValue<ConversationItemVM>(value, "CurrentSelectedAnswer");
				}
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x060019E1 RID: 6625 RVA: 0x0006240E File Offset: 0x0006060E
		// (set) Token: 0x060019E2 RID: 6626 RVA: 0x00062416 File Offset: 0x00060616
		[DataSourceProperty]
		public BannerImageIdentifierVM ConversedHeroBanner
		{
			get
			{
				return this._conversedHeroBanner;
			}
			set
			{
				if (this._conversedHeroBanner != value)
				{
					this._conversedHeroBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "ConversedHeroBanner");
				}
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x060019E3 RID: 6627 RVA: 0x00062434 File Offset: 0x00060634
		// (set) Token: 0x060019E4 RID: 6628 RVA: 0x0006243C File Offset: 0x0006063C
		[DataSourceProperty]
		public HintViewModel RelationHint
		{
			get
			{
				return this._relationHint;
			}
			set
			{
				if (this._relationHint != value)
				{
					this._relationHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RelationHint");
				}
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x060019E5 RID: 6629 RVA: 0x0006245A File Offset: 0x0006065A
		// (set) Token: 0x060019E6 RID: 6630 RVA: 0x00062462 File Offset: 0x00060662
		[DataSourceProperty]
		public HintViewModel FactionHint
		{
			get
			{
				return this._factionHint;
			}
			set
			{
				if (this._factionHint != value)
				{
					this._factionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "FactionHint");
				}
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x060019E7 RID: 6631 RVA: 0x00062480 File Offset: 0x00060680
		// (set) Token: 0x060019E8 RID: 6632 RVA: 0x00062488 File Offset: 0x00060688
		[DataSourceProperty]
		public HintViewModel GoldHint
		{
			get
			{
				return this._goldHint;
			}
			set
			{
				if (this._goldHint != value)
				{
					this._goldHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "GoldHint");
				}
			}
		}

		// Token: 0x04000BC3 RID: 3011
		private readonly ConversationManager _conversationManager;

		// Token: 0x04000BC4 RID: 3012
		private readonly bool _isLinksDisabled;

		// Token: 0x04000BC5 RID: 3013
		private static bool _isCurrentlyPlayerSpeaking;

		// Token: 0x04000BC6 RID: 3014
		private bool _isProcessingOption;

		// Token: 0x04000BC7 RID: 3015
		private BasicCharacterObject _currentDialogCharacter;

		// Token: 0x04000BC8 RID: 3016
		private Func<string> _getContinueInputText;

		// Token: 0x04000BC9 RID: 3017
		private MBBindingList<ConversationItemVM> _answerList;

		// Token: 0x04000BCA RID: 3018
		private string _dialogText;

		// Token: 0x04000BCB RID: 3019
		private string _currentCharacterNameLbl;

		// Token: 0x04000BCC RID: 3020
		private string _continueText;

		// Token: 0x04000BCD RID: 3021
		private string _relationText;

		// Token: 0x04000BCE RID: 3022
		private string _persuasionText;

		// Token: 0x04000BCF RID: 3023
		private bool _isLoadingOver;

		// Token: 0x04000BD0 RID: 3024
		private string _moreOptionText;

		// Token: 0x04000BD1 RID: 3025
		private string _goldText;

		// Token: 0x04000BD2 RID: 3026
		private ConversationAggressivePartyItemVM _defenderLeader;

		// Token: 0x04000BD3 RID: 3027
		private ConversationAggressivePartyItemVM _attackerLeader;

		// Token: 0x04000BD4 RID: 3028
		private MBBindingList<ConversationAggressivePartyItemVM> _defenderParties;

		// Token: 0x04000BD5 RID: 3029
		private MBBindingList<ConversationAggressivePartyItemVM> _attackerParties;

		// Token: 0x04000BD6 RID: 3030
		private BannerImageIdentifierVM _conversedHeroBanner;

		// Token: 0x04000BD7 RID: 3031
		private bool _isAggressive;

		// Token: 0x04000BD8 RID: 3032
		private bool _isRelationEnabled;

		// Token: 0x04000BD9 RID: 3033
		private bool _isBannerEnabled;

		// Token: 0x04000BDA RID: 3034
		private bool _isPersuading;

		// Token: 0x04000BDB RID: 3035
		private bool _isCurrentCharacterValidInEncyclopedia;

		// Token: 0x04000BDC RID: 3036
		private int _selectedSide;

		// Token: 0x04000BDD RID: 3037
		private int _relation;

		// Token: 0x04000BDE RID: 3038
		private int _minRelation;

		// Token: 0x04000BDF RID: 3039
		private int _maxRelation;

		// Token: 0x04000BE0 RID: 3040
		private PowerLevelComparer _powerComparer;

		// Token: 0x04000BE1 RID: 3041
		private ConversationItemVM _currentSelectedAnswer;

		// Token: 0x04000BE2 RID: 3042
		private PersuasionVM _persuasion;

		// Token: 0x04000BE3 RID: 3043
		private HintViewModel _relationHint;

		// Token: 0x04000BE4 RID: 3044
		private HintViewModel _factionHint;

		// Token: 0x04000BE5 RID: 3045
		private HintViewModel _goldHint;
	}
}
