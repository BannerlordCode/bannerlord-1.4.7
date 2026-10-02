using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories
{
	// Token: 0x02000139 RID: 313
	public class ClanFiefsVM : ViewModel
	{
		// Token: 0x06001D2B RID: 7467 RVA: 0x0006BEF8 File Offset: 0x0006A0F8
		public ClanFiefsVM(Action onRefresh, Action<ClanCardSelectionInfo> openCardSelectionPopup)
		{
			this._onRefresh = onRefresh;
			this._clan = Hero.MainHero.Clan;
			this._openCardSelectionPopup = openCardSelectionPopup;
			this._teleportationBehavior = Campaign.Current.GetCampaignBehavior<ITeleportationCampaignBehavior>();
			this.Settlements = new MBBindingList<ClanSettlementItemVM>();
			this.Castles = new MBBindingList<ClanSettlementItemVM>();
			List<MBBindingList<ClanSettlementItemVM>> list = new List<MBBindingList<ClanSettlementItemVM>> { this.Settlements, this.Castles };
			this.SortController = new ClanFiefsSortControllerVM(list);
			this.RefreshAllLists();
			this.RefreshValues();
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x0006BF96 File Offset: 0x0006A196
		protected virtual ClanSettlementItemVM CreateSettlementItem(Settlement settlement, Action<ClanSettlementItemVM> onSelection, Action onShowSendMembers, ITeleportationCampaignBehavior teleportationBehavior)
		{
			return new ClanSettlementItemVM(settlement, onSelection, onShowSendMembers, teleportationBehavior);
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x0006BFA4 File Offset: 0x0006A1A4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TaxText = GameTexts.FindText("str_tax", null).ToString();
			this.GovernorText = GameTexts.FindText("str_notable_governor", null).ToString();
			this.ProfitText = GameTexts.FindText("str_profit", null).ToString();
			this.NameText = GameTexts.FindText("str_sort_by_name_label", null).ToString();
			this.NoFiefsText = GameTexts.FindText("str_clan_no_fiefs", null).ToString();
			this.NoGovernorText = this._noGovernorTextSource.ToString();
			this.Settlements.ApplyActionOnAllItems(delegate(ClanSettlementItemVM x)
			{
				x.RefreshValues();
			});
			this.Castles.ApplyActionOnAllItems(delegate(ClanSettlementItemVM x)
			{
				x.RefreshValues();
			});
			ClanSettlementItemVM currentSelectedFief = this.CurrentSelectedFief;
			if (currentSelectedFief != null)
			{
				currentSelectedFief.RefreshValues();
			}
			this.SortController.RefreshValues();
		}

		// Token: 0x06001D2E RID: 7470 RVA: 0x0006C0A6 File Offset: 0x0006A2A6
		public override void OnFinalize()
		{
			base.OnFinalize();
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x0006C0B0 File Offset: 0x0006A2B0
		public void RefreshAllLists()
		{
			this.Settlements.Clear();
			this.Castles.Clear();
			this.SortController.ResetAllStates();
			foreach (Settlement settlement in this._clan.Settlements)
			{
				if (settlement.IsTown)
				{
					this.Settlements.Add(this.CreateSettlementItem(settlement, new Action<ClanSettlementItemVM>(this.OnFiefSelection), new Action(this.OnShowSendMembers), this._teleportationBehavior));
				}
				else if (settlement.IsCastle)
				{
					this.Castles.Add(this.CreateSettlementItem(settlement, new Action<ClanSettlementItemVM>(this.OnFiefSelection), new Action(this.OnShowSendMembers), this._teleportationBehavior));
				}
			}
			GameTexts.SetVariable("RANK", GameTexts.FindText("str_towns", null));
			GameTexts.SetVariable("NUMBER", this.Settlements.Count);
			this.TownsText = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null).ToString();
			GameTexts.SetVariable("RANK", GameTexts.FindText("str_castles", null));
			GameTexts.SetVariable("NUMBER", this.Castles.Count);
			this.CastlesText = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null).ToString();
			this.OnFiefSelection(this.GetDefaultMember());
		}

		// Token: 0x06001D30 RID: 7472 RVA: 0x0006C224 File Offset: 0x0006A424
		private ClanSettlementItemVM GetDefaultMember()
		{
			if (!this.Settlements.IsEmpty<ClanSettlementItemVM>())
			{
				return this.Settlements.FirstOrDefault<ClanSettlementItemVM>();
			}
			return this.Castles.FirstOrDefault<ClanSettlementItemVM>();
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x0006C24C File Offset: 0x0006A44C
		public void SelectFief(Settlement settlement)
		{
			foreach (ClanSettlementItemVM clanSettlementItemVM in this.Settlements)
			{
				if (clanSettlementItemVM.Settlement == settlement)
				{
					this.OnFiefSelection(clanSettlementItemVM);
					break;
				}
			}
		}

		// Token: 0x06001D32 RID: 7474 RVA: 0x0006C2A4 File Offset: 0x0006A4A4
		private void OnFiefSelection(ClanSettlementItemVM fief)
		{
			if (this.CurrentSelectedFief != null)
			{
				this.CurrentSelectedFief.IsSelected = false;
			}
			this.CurrentSelectedFief = fief;
			TextObject textObject;
			this.CanChangeGovernorOfCurrentFief = this.GetCanChangeGovernor(out textObject);
			this.GovernorActionHint = new HintViewModel(textObject, null);
			if (fief != null)
			{
				fief.IsSelected = true;
				this.GovernorActionText = (fief.HasGovernor ? GameTexts.FindText("str_clan_change_governor", null).ToString() : GameTexts.FindText("str_clan_assign_governor", null).ToString());
			}
		}

		// Token: 0x06001D33 RID: 7475 RVA: 0x0006C324 File Offset: 0x0006A524
		private bool GetCanChangeGovernor(out TextObject disabledReason)
		{
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			ClanSettlementItemVM currentSelectedFief = this.CurrentSelectedFief;
			bool flag;
			if (currentSelectedFief == null)
			{
				flag = false;
			}
			else
			{
				HeroVM governor = currentSelectedFief.Governor;
				bool? flag2;
				if (governor == null)
				{
					flag2 = null;
				}
				else
				{
					Hero hero = governor.Hero;
					flag2 = ((hero != null) ? new bool?(hero.IsTraveling) : null);
				}
				bool? flag3 = flag2;
				bool flag4 = true;
				flag = (flag3.GetValueOrDefault() == flag4) & (flag3 != null);
			}
			if (flag)
			{
				disabledReason = new TextObject("{=qbqimqMb}{GOVERNOR.NAME} is on the way to be the new governor of {SETTLEMENT_NAME}", null);
				if (this.CurrentSelectedFief.Governor.Hero.CharacterObject != null)
				{
					StringHelpers.SetCharacterProperties("GOVERNOR", this.CurrentSelectedFief.Governor.Hero.CharacterObject, disabledReason, false);
				}
				TextObject textObject2 = disabledReason;
				string text = "SETTLEMENT_NAME";
				Settlement settlement = this.CurrentSelectedFief.Settlement;
				string text2;
				if (settlement == null)
				{
					text2 = null;
				}
				else
				{
					TextObject name = settlement.Name;
					text2 = ((name != null) ? name.ToString() : null);
				}
				textObject2.SetTextVariable(text, text2 ?? string.Empty);
				return false;
			}
			ClanSettlementItemVM currentSelectedFief2 = this.CurrentSelectedFief;
			if (((currentSelectedFief2 != null) ? currentSelectedFief2.Settlement.Town : null) == null)
			{
				disabledReason = TextObject.GetEmpty();
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x06001D34 RID: 7476 RVA: 0x0006C444 File Offset: 0x0006A644
		public void ExecuteAssignGovernor()
		{
			ClanSettlementItemVM currentSelectedFief = this.CurrentSelectedFief;
			bool flag;
			if (currentSelectedFief == null)
			{
				flag = null != null;
			}
			else
			{
				Settlement settlement = currentSelectedFief.Settlement;
				flag = ((settlement != null) ? settlement.Town : null) != null;
			}
			if (flag)
			{
				ClanCardSelectionInfo clanCardSelectionInfo = new ClanCardSelectionInfo(GameTexts.FindText("str_clan_assign_governor", null).CopyTextObject(), this.GetGovernorCandidates(), new Action<List<object>, Action>(this.OnGovernorSelectionOver), false, 1, 0);
				Action<ClanCardSelectionInfo> openCardSelectionPopup = this._openCardSelectionPopup;
				if (openCardSelectionPopup == null)
				{
					return;
				}
				openCardSelectionPopup(clanCardSelectionInfo);
			}
		}

		// Token: 0x06001D35 RID: 7477 RVA: 0x0006C4AE File Offset: 0x0006A6AE
		private IEnumerable<ClanCardSelectionItemInfo> GetGovernorCandidates()
		{
			yield return new ClanCardSelectionItemInfo(this._noGovernorTextSource.CopyTextObject(), false, null, null);
			foreach (Hero hero in this._clan.Heroes.Where<Hero>((Hero h) => !h.IsDisabled).Union<Hero>(this._clan.Companions))
			{
				if ((hero.IsActive || hero.IsTraveling) && !hero.IsChild && hero != Hero.MainHero)
				{
					Hero hero2 = hero;
					HeroVM governor = this.CurrentSelectedFief.Governor;
					if (hero2 != ((governor != null) ? governor.Hero : null) && hero.CanBeGovernorOrHavePartyRole())
					{
						TextObject textObject;
						bool flag = FactionHelper.IsMainClanMemberAvailableForSendingSettlementAsGovernor(hero, this.GetSettlementOfGovernor(hero), out textObject);
						SkillObject charm = DefaultSkills.Charm;
						int skillValue = hero.GetSkillValue(charm);
						CharacterImageIdentifier characterImageIdentifier = new CharacterImageIdentifier(CampaignUIHelper.GetCharacterCode(hero.CharacterObject, false));
						yield return new ClanCardSelectionItemInfo(hero, hero.Name, characterImageIdentifier, CardSelectionItemSpriteType.Skill, charm.StringId.ToLower(), skillValue.ToString(), this.GetGovernorCandidateProperties(hero), !flag, textObject, null);
					}
				}
			}
			IEnumerator<Hero> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001D36 RID: 7478 RVA: 0x0006C4BE File Offset: 0x0006A6BE
		private IEnumerable<ClanCardSelectionItemPropertyInfo> GetGovernorCandidateProperties(Hero hero)
		{
			GameTexts.SetVariable("newline", "\n");
			TextObject teleportationDelayText = CampaignUIHelper.GetTeleportationDelayText(hero, this.CurrentSelectedFief.Settlement.Party);
			yield return new ClanCardSelectionItemPropertyInfo(teleportationDelayText);
			ValueTuple<TextObject, TextObject> governorEngineeringSkillEffectForHero = PerkHelper.GetGovernorEngineeringSkillEffectForHero(hero);
			yield return new ClanCardSelectionItemPropertyInfo(new TextObject("{=J8ddrAOf}Governor Effects", null), governorEngineeringSkillEffectForHero.Item2);
			List<PerkObject> governorPerksForHero = PerkHelper.GetGovernorPerksForHero(hero);
			TextObject textObject = new TextObject("{=oSfsqBwJ}No perks", null);
			int num = 0;
			foreach (PerkObject perkObject in governorPerksForHero)
			{
				bool flag = perkObject.PrimaryRole == PartyRole.Governor;
				bool flag2 = perkObject.SecondaryRole == PartyRole.Governor;
				if (flag)
				{
					TextObject textObject2 = ClanCardSelectionItemPropertyInfo.CreateLabeledValueText(perkObject.Name, perkObject.PrimaryDescription);
					this.SetPerksPropertyText(textObject2, ref textObject, ref num);
				}
				if (flag2)
				{
					TextObject textObject3 = ClanCardSelectionItemPropertyInfo.CreateLabeledValueText(perkObject.Name, perkObject.SecondaryDescription);
					this.SetPerksPropertyText(textObject3, ref textObject, ref num);
				}
			}
			yield return new ClanCardSelectionItemPropertyInfo(GameTexts.FindText("str_clan_governor_perks", null), textObject);
			yield break;
		}

		// Token: 0x06001D37 RID: 7479 RVA: 0x0006C4D8 File Offset: 0x0006A6D8
		private void SetPerksPropertyText(TextObject perkText, ref TextObject perksPropertyText, ref int addedPerkCount)
		{
			if (addedPerkCount == 0)
			{
				perksPropertyText = perkText;
			}
			else
			{
				TextObject textObject = GameTexts.FindText("str_string_newline_newline_string", null);
				textObject.SetTextVariable("STR1", perksPropertyText);
				textObject.SetTextVariable("STR2", perkText);
				perksPropertyText = textObject;
			}
			addedPerkCount++;
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x0006C520 File Offset: 0x0006A720
		private void OnGovernorSelectionOver(List<object> selectedItems, Action closePopup)
		{
			if (selectedItems.Count == 1)
			{
				ClanSettlementItemVM currentSelectedFief = this.CurrentSelectedFief;
				Hero hero;
				if (currentSelectedFief == null)
				{
					hero = null;
				}
				else
				{
					HeroVM governor = currentSelectedFief.Governor;
					hero = ((governor != null) ? governor.Hero : null);
				}
				Hero hero2 = hero;
				Hero newGovernor = selectedItems.FirstOrDefault<object>() as Hero;
				bool isRemoveGovernor = newGovernor == null;
				if (!isRemoveGovernor || hero2 != null)
				{
					ValueTuple<TextObject, TextObject> governorSelectionConfirmationPopupTexts = CampaignUIHelper.GetGovernorSelectionConfirmationPopupTexts(hero2, newGovernor, this.CurrentSelectedFief.Settlement);
					InformationManager.ShowInquiry(new InquiryData(governorSelectionConfirmationPopupTexts.Item1.ToString(), governorSelectionConfirmationPopupTexts.Item2.ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
					{
						Action closePopup4 = closePopup;
						if (closePopup4 != null)
						{
							closePopup4();
						}
						if (isRemoveGovernor)
						{
							ChangeGovernorAction.RemoveGovernorOfIfExists(this.CurrentSelectedFief.Settlement.Town);
						}
						else
						{
							ChangeGovernorAction.Apply(this.CurrentSelectedFief.Settlement.Town, newGovernor);
						}
						Action onRefresh = this._onRefresh;
						if (onRefresh == null)
						{
							return;
						}
						onRefresh();
					}, null, "", 0f, null, null, null), false, false);
					return;
				}
				Action closePopup2 = closePopup;
				if (closePopup2 == null)
				{
					return;
				}
				closePopup2();
				return;
			}
			else
			{
				Action closePopup3 = closePopup;
				if (closePopup3 == null)
				{
					return;
				}
				closePopup3();
				return;
			}
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x0006C644 File Offset: 0x0006A844
		private Settlement GetSettlementOfGovernor(Hero hero)
		{
			foreach (ClanSettlementItemVM clanSettlementItemVM in this.Settlements)
			{
				Hero hero2;
				if (clanSettlementItemVM == null)
				{
					hero2 = null;
				}
				else
				{
					HeroVM governor = clanSettlementItemVM.Governor;
					hero2 = ((governor != null) ? governor.Hero : null);
				}
				if (hero2 == hero)
				{
					return clanSettlementItemVM.Settlement;
				}
			}
			foreach (ClanSettlementItemVM clanSettlementItemVM2 in this.Castles)
			{
				Hero hero3;
				if (clanSettlementItemVM2 == null)
				{
					hero3 = null;
				}
				else
				{
					HeroVM governor2 = clanSettlementItemVM2.Governor;
					hero3 = ((governor2 != null) ? governor2.Hero : null);
				}
				if (hero3 == hero)
				{
					return clanSettlementItemVM2.Settlement;
				}
			}
			return null;
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x0006C70C File Offset: 0x0006A90C
		private void OnShowSendMembers()
		{
			ClanSettlementItemVM currentSelectedFief = this.CurrentSelectedFief;
			Settlement settlement = ((currentSelectedFief != null) ? currentSelectedFief.Settlement : null);
			if (settlement != null)
			{
				TextObject textObject = GameTexts.FindText("str_send_members", null);
				textObject.SetTextVariable("SETTLEMENT_NAME", settlement.Name);
				ClanCardSelectionInfo clanCardSelectionInfo = new ClanCardSelectionInfo(textObject, this.GetSendMembersCandidates(), new Action<List<object>, Action>(this.OnSendMembersSelectionOver), true, 1, 0);
				Action<ClanCardSelectionInfo> openCardSelectionPopup = this._openCardSelectionPopup;
				if (openCardSelectionPopup == null)
				{
					return;
				}
				openCardSelectionPopup(clanCardSelectionInfo);
			}
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x0006C77B File Offset: 0x0006A97B
		private IEnumerable<ClanCardSelectionItemInfo> GetSendMembersCandidates()
		{
			foreach (Hero hero in this._clan.Heroes.Where<Hero>((Hero h) => !h.IsDisabled).Union<Hero>(this._clan.Companions))
			{
				if ((hero.IsActive || hero.IsTraveling) && (hero.CurrentSettlement != this.CurrentSelectedFief.Settlement || hero.PartyBelongedTo != null) && !hero.IsChild && hero != Hero.MainHero)
				{
					TextObject textObject;
					bool flag = FactionHelper.IsMainClanMemberAvailableForSendingSettlement(hero, this.CurrentSelectedFief.Settlement, out textObject);
					SkillObject charm = DefaultSkills.Charm;
					int skillValue = hero.GetSkillValue(charm);
					CharacterImageIdentifier characterImageIdentifier = new CharacterImageIdentifier(CampaignUIHelper.GetCharacterCode(hero.CharacterObject, false));
					yield return new ClanCardSelectionItemInfo(hero, hero.Name, characterImageIdentifier, CardSelectionItemSpriteType.Skill, charm.StringId.ToLower(), skillValue.ToString(), this.GetSendMembersCandidateProperties(hero), !flag, textObject, null);
				}
			}
			IEnumerator<Hero> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x0006C78B File Offset: 0x0006A98B
		private IEnumerable<ClanCardSelectionItemPropertyInfo> GetSendMembersCandidateProperties(Hero hero)
		{
			TextObject teleportationDelayText = CampaignUIHelper.GetTeleportationDelayText(hero, this.CurrentSelectedFief.Settlement.Party);
			yield return new ClanCardSelectionItemPropertyInfo(teleportationDelayText);
			TextObject textObject = new TextObject("{=otaUtXMX}+{AMOUNT} relation chance with notables per day.", null);
			int emissaryRelationBonusForMainClan = Campaign.Current.Models.EmissaryModel.EmissaryRelationBonusForMainClan;
			textObject.SetTextVariable("AMOUNT", emissaryRelationBonusForMainClan);
			yield return new ClanCardSelectionItemPropertyInfo(textObject);
			yield break;
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x0006C7A4 File Offset: 0x0006A9A4
		private void OnSendMembersSelectionOver(List<object> selectedItems, Action closePopup)
		{
			if (selectedItems.Count > 0)
			{
				string text = "SETTLEMENT_NAME";
				ClanSettlementItemVM currentSelectedFief = this.CurrentSelectedFief;
				string text2;
				if (currentSelectedFief == null)
				{
					text2 = null;
				}
				else
				{
					Settlement settlement = currentSelectedFief.Settlement;
					if (settlement == null)
					{
						text2 = null;
					}
					else
					{
						TextObject name = settlement.Name;
						text2 = ((name != null) ? name.ToString() : null);
					}
				}
				MBTextManager.SetTextVariable(text, text2 ?? string.Empty, false);
				InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_send_members", null).ToString(), GameTexts.FindText("str_send_members_inquiry", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
				{
					Action closePopup3 = closePopup;
					if (closePopup3 != null)
					{
						closePopup3();
					}
					using (List<object>.Enumerator enumerator = selectedItems.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Hero hero;
							if ((hero = enumerator.Current as Hero) != null)
							{
								TeleportHeroAction.ApplyDelayedTeleportToSettlement(hero, this.CurrentSelectedFief.Settlement);
							}
						}
					}
					Action onRefresh = this._onRefresh;
					if (onRefresh == null)
					{
						return;
					}
					onRefresh();
				}, null, "", 0f, null, null, null), false, false);
				return;
			}
			Action closePopup2 = closePopup;
			if (closePopup2 == null)
			{
				return;
			}
			closePopup2();
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06001D3E RID: 7486 RVA: 0x0006C894 File Offset: 0x0006AA94
		// (set) Token: 0x06001D3F RID: 7487 RVA: 0x0006C89C File Offset: 0x0006AA9C
		[DataSourceProperty]
		public string GovernorActionText
		{
			get
			{
				return this._governorActionText;
			}
			set
			{
				if (value != this._governorActionText)
				{
					this._governorActionText = value;
					base.OnPropertyChangedWithValue<string>(value, "GovernorActionText");
				}
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06001D40 RID: 7488 RVA: 0x0006C8BF File Offset: 0x0006AABF
		// (set) Token: 0x06001D41 RID: 7489 RVA: 0x0006C8C7 File Offset: 0x0006AAC7
		[DataSourceProperty]
		public bool CanChangeGovernorOfCurrentFief
		{
			get
			{
				return this._canChangeGovernorOfCurrentFief;
			}
			set
			{
				if (value != this._canChangeGovernorOfCurrentFief)
				{
					this._canChangeGovernorOfCurrentFief = value;
					base.OnPropertyChangedWithValue(value, "CanChangeGovernorOfCurrentFief");
				}
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x06001D42 RID: 7490 RVA: 0x0006C8E5 File Offset: 0x0006AAE5
		// (set) Token: 0x06001D43 RID: 7491 RVA: 0x0006C8ED File Offset: 0x0006AAED
		[DataSourceProperty]
		public HintViewModel GovernorActionHint
		{
			get
			{
				return this._governorActionHint;
			}
			set
			{
				if (value != this._governorActionHint)
				{
					this._governorActionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "GovernorActionHint");
				}
			}
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06001D44 RID: 7492 RVA: 0x0006C90B File Offset: 0x0006AB0B
		// (set) Token: 0x06001D45 RID: 7493 RVA: 0x0006C913 File Offset: 0x0006AB13
		[DataSourceProperty]
		public bool IsAnyValidFiefSelected
		{
			get
			{
				return this._isAnyValidFiefSelected;
			}
			set
			{
				if (value != this._isAnyValidFiefSelected)
				{
					this._isAnyValidFiefSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyValidFiefSelected");
				}
			}
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06001D46 RID: 7494 RVA: 0x0006C931 File Offset: 0x0006AB31
		// (set) Token: 0x06001D47 RID: 7495 RVA: 0x0006C939 File Offset: 0x0006AB39
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

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06001D48 RID: 7496 RVA: 0x0006C95C File Offset: 0x0006AB5C
		// (set) Token: 0x06001D49 RID: 7497 RVA: 0x0006C964 File Offset: 0x0006AB64
		[DataSourceProperty]
		public string TaxText
		{
			get
			{
				return this._taxText;
			}
			set
			{
				if (value != this._taxText)
				{
					this._taxText = value;
					base.OnPropertyChangedWithValue<string>(value, "TaxText");
				}
			}
		}

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06001D4A RID: 7498 RVA: 0x0006C987 File Offset: 0x0006AB87
		// (set) Token: 0x06001D4B RID: 7499 RVA: 0x0006C98F File Offset: 0x0006AB8F
		[DataSourceProperty]
		public string GovernorText
		{
			get
			{
				return this._governorText;
			}
			set
			{
				if (value != this._governorText)
				{
					this._governorText = value;
					base.OnPropertyChangedWithValue<string>(value, "GovernorText");
				}
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06001D4C RID: 7500 RVA: 0x0006C9B2 File Offset: 0x0006ABB2
		// (set) Token: 0x06001D4D RID: 7501 RVA: 0x0006C9BA File Offset: 0x0006ABBA
		[DataSourceProperty]
		public string ProfitText
		{
			get
			{
				return this._profitText;
			}
			set
			{
				if (value != this._profitText)
				{
					this._profitText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProfitText");
				}
			}
		}

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06001D4E RID: 7502 RVA: 0x0006C9DD File Offset: 0x0006ABDD
		// (set) Token: 0x06001D4F RID: 7503 RVA: 0x0006C9E5 File Offset: 0x0006ABE5
		[DataSourceProperty]
		public string TownsText
		{
			get
			{
				return this._townsText;
			}
			set
			{
				if (value != this._townsText)
				{
					this._townsText = value;
					base.OnPropertyChangedWithValue<string>(value, "TownsText");
				}
			}
		}

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x06001D50 RID: 7504 RVA: 0x0006CA08 File Offset: 0x0006AC08
		// (set) Token: 0x06001D51 RID: 7505 RVA: 0x0006CA10 File Offset: 0x0006AC10
		[DataSourceProperty]
		public string CastlesText
		{
			get
			{
				return this._castlesText;
			}
			set
			{
				if (value != this._castlesText)
				{
					this._castlesText = value;
					base.OnPropertyChangedWithValue<string>(value, "CastlesText");
				}
			}
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x06001D52 RID: 7506 RVA: 0x0006CA33 File Offset: 0x0006AC33
		// (set) Token: 0x06001D53 RID: 7507 RVA: 0x0006CA3B File Offset: 0x0006AC3B
		[DataSourceProperty]
		public string NoFiefsText
		{
			get
			{
				return this._noFiefsText;
			}
			set
			{
				if (value != this._noFiefsText)
				{
					this._noFiefsText = value;
					base.OnPropertyChangedWithValue<string>(value, "NoFiefsText");
				}
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x06001D54 RID: 7508 RVA: 0x0006CA5E File Offset: 0x0006AC5E
		// (set) Token: 0x06001D55 RID: 7509 RVA: 0x0006CA66 File Offset: 0x0006AC66
		[DataSourceProperty]
		public string NoGovernorText
		{
			get
			{
				return this._noGovernorText;
			}
			set
			{
				if (value != this._noGovernorText)
				{
					this._noGovernorText = value;
					base.OnPropertyChangedWithValue<string>(value, "NoGovernorText");
				}
			}
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06001D56 RID: 7510 RVA: 0x0006CA89 File Offset: 0x0006AC89
		// (set) Token: 0x06001D57 RID: 7511 RVA: 0x0006CA91 File Offset: 0x0006AC91
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

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x06001D58 RID: 7512 RVA: 0x0006CAAF File Offset: 0x0006ACAF
		// (set) Token: 0x06001D59 RID: 7513 RVA: 0x0006CAB7 File Offset: 0x0006ACB7
		[DataSourceProperty]
		public MBBindingList<ClanSettlementItemVM> Settlements
		{
			get
			{
				return this._settlements;
			}
			set
			{
				if (value != this._settlements)
				{
					this._settlements = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanSettlementItemVM>>(value, "Settlements");
				}
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x06001D5A RID: 7514 RVA: 0x0006CAD5 File Offset: 0x0006ACD5
		// (set) Token: 0x06001D5B RID: 7515 RVA: 0x0006CADD File Offset: 0x0006ACDD
		[DataSourceProperty]
		public MBBindingList<ClanSettlementItemVM> Castles
		{
			get
			{
				return this._castles;
			}
			set
			{
				if (value != this._castles)
				{
					this._castles = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanSettlementItemVM>>(value, "Castles");
				}
			}
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x06001D5C RID: 7516 RVA: 0x0006CAFB File Offset: 0x0006ACFB
		// (set) Token: 0x06001D5D RID: 7517 RVA: 0x0006CB03 File Offset: 0x0006AD03
		[DataSourceProperty]
		public ClanSettlementItemVM CurrentSelectedFief
		{
			get
			{
				return this._currentSelectedFief;
			}
			set
			{
				if (value != this._currentSelectedFief)
				{
					this._currentSelectedFief = value;
					base.OnPropertyChangedWithValue<ClanSettlementItemVM>(value, "CurrentSelectedFief");
					this.IsAnyValidFiefSelected = value != null;
				}
			}
		}

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x06001D5E RID: 7518 RVA: 0x0006CB2B File Offset: 0x0006AD2B
		// (set) Token: 0x06001D5F RID: 7519 RVA: 0x0006CB33 File Offset: 0x0006AD33
		[DataSourceProperty]
		public ClanFiefsSortControllerVM SortController
		{
			get
			{
				return this._sortController;
			}
			set
			{
				if (value != this._sortController)
				{
					this._sortController = value;
					base.OnPropertyChangedWithValue<ClanFiefsSortControllerVM>(value, "SortController");
				}
			}
		}

		// Token: 0x04000D9C RID: 3484
		private readonly Clan _clan;

		// Token: 0x04000D9D RID: 3485
		private readonly Action _onRefresh;

		// Token: 0x04000D9E RID: 3486
		private readonly Action<ClanCardSelectionInfo> _openCardSelectionPopup;

		// Token: 0x04000D9F RID: 3487
		private readonly ITeleportationCampaignBehavior _teleportationBehavior;

		// Token: 0x04000DA0 RID: 3488
		private readonly TextObject _noGovernorTextSource = new TextObject("{=zLFsnaqR}No Governor", null);

		// Token: 0x04000DA1 RID: 3489
		private MBBindingList<ClanSettlementItemVM> _settlements;

		// Token: 0x04000DA2 RID: 3490
		private MBBindingList<ClanSettlementItemVM> _castles;

		// Token: 0x04000DA3 RID: 3491
		private ClanSettlementItemVM _currentSelectedFief;

		// Token: 0x04000DA4 RID: 3492
		private bool _isSelected;

		// Token: 0x04000DA5 RID: 3493
		private string _nameText;

		// Token: 0x04000DA6 RID: 3494
		private string _taxText;

		// Token: 0x04000DA7 RID: 3495
		private string _governorText;

		// Token: 0x04000DA8 RID: 3496
		private string _profitText;

		// Token: 0x04000DA9 RID: 3497
		private string _townsText;

		// Token: 0x04000DAA RID: 3498
		private string _castlesText;

		// Token: 0x04000DAB RID: 3499
		private string _noFiefsText;

		// Token: 0x04000DAC RID: 3500
		private string _noGovernorText;

		// Token: 0x04000DAD RID: 3501
		private bool _isAnyValidFiefSelected;

		// Token: 0x04000DAE RID: 3502
		private bool _canChangeGovernorOfCurrentFief;

		// Token: 0x04000DAF RID: 3503
		private HintViewModel _governorActionHint;

		// Token: 0x04000DB0 RID: 3504
		private string _governorActionText;

		// Token: 0x04000DB1 RID: 3505
		private ClanFiefsSortControllerVM _sortController;
	}
}
