using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ArmyManagement
{
	// Token: 0x0200015E RID: 350
	public class ArmyManagementVM : ViewModel
	{
		// Token: 0x0600217E RID: 8574 RVA: 0x0007966C File Offset: 0x0007786C
		public ArmyManagementVM(Action onClose)
		{
			this._onClose = onClose;
			this._itemComparer = new ArmyManagementVM.ManagementItemComparer();
			this.PartyList = new MBBindingList<ArmyManagementItemVM>();
			this.PartiesInCart = new MBBindingList<ArmyManagementItemVM>();
			this._partiesToRemove = new MBBindingList<ArmyManagementItemVM>();
			this._currentParties = new List<MobileParty>();
			this.CohesionHint = new BasicTooltipViewModel();
			this.FoodHint = new HintViewModel();
			this.MoraleHint = new HintViewModel();
			this.BoostCohesionHint = new HintViewModel();
			this.DisbandArmyHint = new HintViewModel();
			this.DoneHint = new HintViewModel();
			this.TutorialNotification = new ElementNotificationVM();
			this.CanConfirm = false;
			this.CanAffordInfluenceCost = true;
			this.PlayerHasArmy = MobileParty.MainParty.Army != null;
			foreach (MobileParty mobileParty in MobileParty.All)
			{
				if (mobileParty.LeaderHero != null && mobileParty.MapFaction == Hero.MainHero.MapFaction && mobileParty.LeaderHero != Hero.MainHero && !mobileParty.IsCaravan)
				{
					this.PartyList.Add(new ArmyManagementItemVM(new Action<ArmyManagementItemVM>(this.OnAddToCart), new Action<ArmyManagementItemVM>(this.OnRemove), new Action<ArmyManagementItemVM>(this.OnFocus), mobileParty));
				}
			}
			this._mainPartyItem = new ArmyManagementItemVM(null, null, null, Hero.MainHero.PartyBelongedTo)
			{
				IsAlreadyWithPlayer = true,
				IsMainHero = true,
				IsInCart = true
			};
			this.PartiesInCart.Add(this._mainPartyItem);
			foreach (ArmyManagementItemVM armyManagementItemVM in this.PartyList)
			{
				if (MobileParty.MainParty.Army != null && armyManagementItemVM.Party.Army == MobileParty.MainParty.Army && armyManagementItemVM.Party != MobileParty.MainParty)
				{
					armyManagementItemVM.Cost = 0;
					armyManagementItemVM.IsAlreadyWithPlayer = true;
					armyManagementItemVM.IsInCart = true;
					this.PartiesInCart.Add(armyManagementItemVM);
				}
			}
			if (MobileParty.MainParty.Army != null)
			{
				this.CohesionBoostCost = Campaign.Current.Models.ArmyManagementCalculationModel.GetCohesionBoostInfluenceCost(MobileParty.MainParty.Army, 10);
			}
			this._initialInfluence = Hero.MainHero.Clan.Influence;
			this.OnRefresh();
			Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.ArmyManagement));
			this.SortControllerVM = new ArmyManagementSortControllerVM(this._partyList);
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
			this.RefreshValues();
		}

		// Token: 0x0600217F RID: 8575 RVA: 0x00079928 File Offset: 0x00077B28
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = GameTexts.FindText("str_army_management", null).ToString();
			this.BoostTitleText = GameTexts.FindText("str_boost_cohesion", null).ToString();
			this.CancelText = GameTexts.FindText("str_cancel", null).ToString();
			this.DoneText = GameTexts.FindText("str_done", null).ToString();
			this.DistanceText = GameTexts.FindText("str_distance", null).ToString();
			this.CostText = GameTexts.FindText("str_cost", null).ToString();
			this.StrengthText = GameTexts.FindText("str_men", null).ToString();
			this.LordsText = GameTexts.FindText("str_leader", null).ToString();
			this.ClanText = GameTexts.FindText("str_clan", null).ToString();
			this.NameText = GameTexts.FindText("str_sort_by_name_label", null).ToString();
			this.OwnerText = GameTexts.FindText("str_party", null).ToString();
			this.DisbandArmyText = GameTexts.FindText("str_disband_army", null).ToString();
			this.ShipCountText = new TextObject("{=7Q8ufo5X}Ships", null).ToString();
			this._playerDoesntHaveEnoughInfluenceStr = GameTexts.FindText("str_warning_you_dont_have_enough_influence", null).ToString();
			GameTexts.SetVariable("TOTAL_INFLUENCE", MathF.Round(Hero.MainHero.Clan.Influence));
			this.TotalInfluence = GameTexts.FindText("str_total_influence", null).ToString();
			GameTexts.SetVariable("NUMBER", 10);
			this.CohesionBoostAmountText = GameTexts.FindText("str_plus_with_number", null).ToString();
			this.PartyList.ApplyActionOnAllItems(delegate(ArmyManagementItemVM x)
			{
				x.RefreshValues();
			});
			this.PartiesInCart.ApplyActionOnAllItems(delegate(ArmyManagementItemVM x)
			{
				x.RefreshValues();
			});
			this.TutorialNotification.RefreshValues();
		}

		// Token: 0x06002180 RID: 8576 RVA: 0x00079B24 File Offset: 0x00077D24
		private void CalculateCohesion()
		{
			if (MobileParty.MainParty.Army != null)
			{
				this.Cohesion = (int)MobileParty.MainParty.Army.Cohesion;
				this.NewCohesion = MathF.Min(this.Cohesion + this._boostedCohesion, 100);
				ArmyManagementCalculationModel armyManagementCalculationModel = Campaign.Current.Models.ArmyManagementCalculationModel;
				this._currentParties.Clear();
				foreach (ArmyManagementItemVM armyManagementItemVM in this.PartiesInCart)
				{
					if (!armyManagementItemVM.Party.IsMainParty)
					{
						this._currentParties.Add(armyManagementItemVM.Party);
						if (!armyManagementItemVM.IsAlreadyWithPlayer)
						{
							this.NewCohesion = armyManagementCalculationModel.CalculateNewCohesion(MobileParty.MainParty.Army, armyManagementItemVM.Party.Party, this.NewCohesion, 1);
						}
					}
				}
			}
		}

		// Token: 0x06002181 RID: 8577 RVA: 0x00079C14 File Offset: 0x00077E14
		private void OnFocus(ArmyManagementItemVM focusedItem)
		{
			this.FocusedItem = focusedItem;
		}

		// Token: 0x06002182 RID: 8578 RVA: 0x00079C20 File Offset: 0x00077E20
		private void OnAddToCart(ArmyManagementItemVM armyItem)
		{
			if (!this.PartiesInCart.Contains(armyItem))
			{
				this.PartiesInCart.Add(armyItem);
				armyItem.IsInCart = true;
				Game.Current.EventManager.TriggerEvent<PartyAddedToArmyByPlayerEvent>(new PartyAddedToArmyByPlayerEvent(armyItem.Party));
				if (this._partiesToRemove.Contains(armyItem))
				{
					this._partiesToRemove.Remove(armyItem);
				}
				if (armyItem.IsAlreadyWithPlayer)
				{
					armyItem.CanJoinBackWithoutCost = false;
				}
				this.TotalCost += armyItem.Cost;
			}
			this.OnRefresh();
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x00079CAC File Offset: 0x00077EAC
		private void OnRemove(ArmyManagementItemVM armyItem)
		{
			if (this.PartiesInCart.Contains(armyItem))
			{
				this.PartiesInCart.Remove(armyItem);
				armyItem.IsInCart = false;
				this._partiesToRemove.Add(armyItem);
				if (armyItem.IsAlreadyWithPlayer)
				{
					armyItem.CanJoinBackWithoutCost = true;
				}
				this.TotalCost -= armyItem.Cost;
			}
			this.OnRefresh();
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x00079D10 File Offset: 0x00077F10
		private void UpdateCanConfirm()
		{
			if (!this.CanAffordInfluenceCost)
			{
				this.CanConfirm = false;
				this.DoneHint.HintText = GameTexts.FindText("str_warning_you_dont_have_enough_influence", null).CopyTextObject();
			}
			else if (this.PartiesInCart.Count == 1 && this.PartiesInCart[0].IsMainHero)
			{
				this.CanConfirm = this.CanDisbandArmy;
				if (!this.CanConfirm)
				{
					this.DoneHint.HintText = new TextObject("{=aUq1M6Wa}You need more than 1 party to create an army", null);
				}
			}
			else
			{
				this.CanConfirm = true;
			}
			if (this.CanConfirm)
			{
				this.DoneHint.HintText = TextObject.GetEmpty();
			}
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x00079DB8 File Offset: 0x00077FB8
		private void ApplyCohesionChange()
		{
			if (MobileParty.MainParty.Army != null)
			{
				int num = this.NewCohesion - this.Cohesion;
				MobileParty.MainParty.Army.BoostCohesionWithInfluence((float)num, this._influenceSpentForCohesionBoosting);
			}
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x00079DF8 File Offset: 0x00077FF8
		private void OnBoostCohesion()
		{
			if (this.CanBoostCohesion)
			{
				this.TotalCost += this.CohesionBoostCost;
				this._boostedCohesion += 10;
				this._influenceSpentForCohesionBoosting += this.CohesionBoostCost;
				this.OnRefresh();
			}
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x00079E48 File Offset: 0x00078048
		private void OnRefresh()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			float num4 = 0f;
			foreach (ArmyManagementItemVM armyManagementItemVM in this.PartiesInCart)
			{
				num2++;
				num += (int)armyManagementItemVM.Party.Party.EstimatedStrength;
				if (armyManagementItemVM.IsAlreadyWithPlayer)
				{
					num4 += armyManagementItemVM.Party.Food;
					num3 += (int)armyManagementItemVM.Party.Morale;
				}
			}
			this.TotalStrength = num;
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_total_cost", null).ToString());
			this.TotalCostText = GameTexts.FindText("str_LEFT_colon", null).ToString();
			GameTexts.SetVariable("LEFT", this.TotalCost.ToString());
			GameTexts.SetVariable("RIGHT", ((int)Hero.MainHero.Clan.Influence).ToString());
			this.TotalCostNumbersText = GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString();
			GameTexts.SetVariable("NUM", num2);
			this.TotalLords = GameTexts.FindText("str_NUM_lords", null).ToString();
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_strength", null).ToString());
			this.TotalStrengthText = GameTexts.FindText("str_LEFT_colon", null).ToString();
			this.CanCreateArmy = (float)this.TotalCost <= Hero.MainHero.Clan.Influence && num2 > 1;
			bool flag;
			if (MobileParty.MainParty.Army != null)
			{
				if (this._partiesToRemove.Count > 0)
				{
					flag = this.PartiesInCart.Count<ArmyManagementItemVM>((ArmyManagementItemVM p) => p.IsAlreadyWithPlayer) >= 1;
				}
				else
				{
					flag = true;
				}
			}
			else
			{
				flag = false;
			}
			this.PlayerHasArmy = flag;
			this.CalculateCohesion();
			this.CanBoostCohesion = this.PlayerHasArmy && this.NewCohesion + 10 <= 100;
			if (this.CanBoostCohesion)
			{
				TextObject textObject = new TextObject("{=nNZ1ZtTE}Add {BOOSTAMOUNT} cohesion to your army", null);
				textObject.SetTextVariable("BOOSTAMOUNT", 10);
				this.BoostCohesionHint.HintText = textObject;
			}
			else if (this.NewCohesion + 10 > 100)
			{
				TextObject textObject2 = new TextObject("{=rsHPaaYZ}Cohesion needs to be lower than {MINAMOUNT} to boost", null);
				textObject2.SetTextVariable("MINAMOUNT", 90);
				this.BoostCohesionHint.HintText = textObject2;
			}
			else
			{
				this.BoostCohesionHint.HintText = new TextObject("{=Ioiqzz4E}You need to be in an army to boost cohesion", null);
			}
			if (MobileParty.MainParty.Army != null)
			{
				this.CohesionText = GameTexts.FindText("str_cohesion", null).ToString();
				num3 += (int)MobileParty.MainParty.Morale;
				num4 += MobileParty.MainParty.Food;
			}
			this.MoraleText = num3.ToString();
			this.FoodText = MathF.Round(num4, 1).ToString();
			this.PartiesInCart.Sort(this._itemComparer);
			TextObject textObject3;
			this.CanDisbandArmy = this.GetCanDisbandArmyWithReason(out textObject3);
			this.DisbandArmyHint.HintText = textObject3;
			this.UpdateCanConfirm();
			this.UpdateTooltips();
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x0007A178 File Offset: 0x00078378
		private bool GetCanDisbandArmyWithReason(out TextObject disabledReason)
		{
			if (MobileParty.MainParty.Army == null)
			{
				disabledReason = new TextObject("{=iSZTOeYH}No army to disband.", null);
				return false;
			}
			if (MobileParty.MainParty.MapEvent != null)
			{
				disabledReason = new TextObject("{=uipNpzVw}Cannot disband the army right now.", null);
				return false;
			}
			if (PlayerSiege.PlayerSiegeEvent != null)
			{
				disabledReason = GameTexts.FindText("str_action_disabled_reason_siege", null);
				return false;
			}
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x0007A1E8 File Offset: 0x000783E8
		private void UpdateTooltips()
		{
			if (this.PlayerHasArmy)
			{
				this.CohesionHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetArmyCohesionTooltip(PartyBase.MainParty.MobileParty.Army));
				PartyBase.MainParty.MobileParty.Army.RecalculateArmyMorale();
				MathF.Round(PartyBase.MainParty.MobileParty.Army.Morale, 1).ToString("0.0");
				MBTextManager.SetTextVariable("BASE_EFFECT", MathF.Round(MobileParty.MainParty.Morale, 1).ToString("0.0"), false);
				MBTextManager.SetTextVariable("STR1", "", false);
				MBTextManager.SetTextVariable("STR2", "", false);
				MBTextManager.SetTextVariable("ARMY_MORALE", MobileParty.MainParty.Army.Morale, 2);
				foreach (MobileParty mobileParty in MobileParty.MainParty.Army.Parties)
				{
					MBTextManager.SetTextVariable("STR1", GameTexts.FindText("str_STR1_STR2", null).ToString(), false);
					MBTextManager.SetTextVariable("PARTY_NAME", mobileParty.Name, false);
					MBTextManager.SetTextVariable("PARTY_MORALE", (int)mobileParty.Morale);
					MBTextManager.SetTextVariable("STR2", GameTexts.FindText("str_new_morale_item_line", null), false);
				}
				MBTextManager.SetTextVariable("ARMY_MORALE_ITEMS", GameTexts.FindText("str_STR1_STR2", null).ToString(), false);
				this.MoraleHint.HintText = GameTexts.FindText("str_army_morale_tooltip", null);
			}
			else
			{
				GameTexts.SetVariable("reg1", (int)MobileParty.MainParty.Morale);
				this.MoraleHint.HintText = GameTexts.FindText("str_morale_reg1", null);
			}
			MBTextManager.SetTextVariable("newline", "\n", false);
			MBTextManager.SetTextVariable("DAILY_FOOD_CONSUMPTION", MobileParty.MainParty.FoodChange, 2);
			this.FoodHint.HintText = GameTexts.FindText("str_food_consumption_tooltip", null);
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x0007A3FC File Offset: 0x000785FC
		public void ExecuteDone()
		{
			if (!this.CanAffordInfluenceCost)
			{
				return;
			}
			if (this.PartiesInCart.Count == 1 && this.PartiesInCart[0].IsMainHero)
			{
				this.ExecuteDisbandArmy();
				return;
			}
			if (this.NewCohesion > this.Cohesion)
			{
				this.ApplyCohesionChange();
			}
			if (this.PartiesInCart.Count > 1 && MobileParty.MainParty.MapFaction.IsKingdomFaction)
			{
				if (MobileParty.MainParty.Army == null)
				{
					((Kingdom)MobileParty.MainParty.MapFaction).CreateArmy(Hero.MainHero, Hero.MainHero.HomeSettlement, Army.ArmyTypes.Defender, null);
				}
				foreach (ArmyManagementItemVM armyManagementItemVM in this.PartiesInCart)
				{
					if (armyManagementItemVM.Party != MobileParty.MainParty)
					{
						armyManagementItemVM.Party.Army = MobileParty.MainParty.Army;
					}
				}
				ChangeClanInfluenceAction.Apply(Clan.PlayerClan, (float)(-(float)(this.TotalCost - this._influenceSpentForCohesionBoosting)));
			}
			if (this._partiesToRemove.Count > 0)
			{
				bool flag = false;
				foreach (ArmyManagementItemVM armyManagementItemVM2 in this._partiesToRemove)
				{
					if (armyManagementItemVM2.Party == MobileParty.MainParty)
					{
						armyManagementItemVM2.Party.Army = null;
						flag = true;
					}
				}
				if (!flag)
				{
					foreach (ArmyManagementItemVM armyManagementItemVM3 in this._partiesToRemove)
					{
						Army army = MobileParty.MainParty.Army;
						if (army != null && army.Parties.Contains(armyManagementItemVM3.Party))
						{
							armyManagementItemVM3.Party.Army = null;
						}
					}
				}
				this._partiesToRemove.Clear();
			}
			this._onClose();
			CampaignEventDispatcher.Instance.OnArmyOverlaySetDirty();
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x0007A608 File Offset: 0x00078808
		public void ExecuteCancel()
		{
			ChangeClanInfluenceAction.Apply(Clan.PlayerClan, this._initialInfluence - Clan.PlayerClan.Influence);
			this._onClose();
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x0007A630 File Offset: 0x00078830
		public void ExecuteReset()
		{
			foreach (ArmyManagementItemVM armyManagementItemVM in this.PartiesInCart.ToList<ArmyManagementItemVM>())
			{
				this.OnRemove(armyManagementItemVM);
				armyManagementItemVM.UpdateEligibility();
			}
			this.PartiesInCart.Add(this._mainPartyItem);
			foreach (ArmyManagementItemVM armyManagementItemVM2 in this.PartyList)
			{
				if (armyManagementItemVM2.IsAlreadyWithPlayer)
				{
					this.PartiesInCart.Add(armyManagementItemVM2);
					armyManagementItemVM2.IsInCart = true;
					armyManagementItemVM2.CanJoinBackWithoutCost = false;
				}
			}
			this.NewCohesion = this.Cohesion;
			ChangeClanInfluenceAction.Apply(Clan.PlayerClan, this._initialInfluence - Clan.PlayerClan.Influence);
			this.TotalCost = 0;
			this._boostedCohesion = 0;
			this._influenceSpentForCohesionBoosting = 0;
			this._partiesToRemove.Clear();
			this.OnRefresh();
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x0007A744 File Offset: 0x00078944
		public void ExecuteDisbandArmy()
		{
			if (this.CanDisbandArmy)
			{
				InformationManager.ShowInquiry(new InquiryData(new TextObject("{=ViYdZUbQ}Disband Army", null).ToString(), new TextObject("{=kqeA8rjL}Are you sure you want to disband your army?", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
				{
					this.DisbandArmy();
				}, null, "", 0f, null, null, null), false, false);
			}
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x0007A7C1 File Offset: 0x000789C1
		public void ExecuteBoostCohesionManual()
		{
			this.OnBoostCohesion();
			Game.Current.EventManager.TriggerEvent<ArmyCohesionBoostedByPlayerEvent>(new ArmyCohesionBoostedByPlayerEvent());
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x0007A7E0 File Offset: 0x000789E0
		private void DisbandArmy()
		{
			foreach (ArmyManagementItemVM armyManagementItemVM in this.PartiesInCart.ToList<ArmyManagementItemVM>())
			{
				this.OnRemove(armyManagementItemVM);
			}
			this.ExecuteDone();
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x0007A840 File Offset: 0x00078A40
		private void OnCloseBoost()
		{
			Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.ArmyManagement));
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x0007A858 File Offset: 0x00078A58
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent obj)
		{
			if (obj.NewNotificationElementID != this._latestTutorialElementID)
			{
				if (this._latestTutorialElementID != null)
				{
					this.TutorialNotification.ElementID = string.Empty;
				}
				this._latestTutorialElementID = obj.NewNotificationElementID;
				if (this._latestTutorialElementID != null)
				{
					this.TutorialNotification.ElementID = this._latestTutorialElementID;
				}
			}
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x0007A8B8 File Offset: 0x00078AB8
		public override void OnFinalize()
		{
			base.OnFinalize();
			Game.Current.EventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			InputKeyItemVM resetInputKey = this.ResetInputKey;
			if (resetInputKey != null)
			{
				resetInputKey.OnFinalize();
			}
			InputKeyItemVM removeInputKey = this.RemoveInputKey;
			if (removeInputKey == null)
			{
				return;
			}
			removeInputKey.OnFinalize();
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06002193 RID: 8595 RVA: 0x0007A929 File Offset: 0x00078B29
		// (set) Token: 0x06002194 RID: 8596 RVA: 0x0007A931 File Offset: 0x00078B31
		[DataSourceProperty]
		public ElementNotificationVM TutorialNotification
		{
			get
			{
				return this._tutorialNotification;
			}
			set
			{
				if (value != this._tutorialNotification)
				{
					this._tutorialNotification = value;
					base.OnPropertyChangedWithValue<ElementNotificationVM>(value, "TutorialNotification");
				}
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06002195 RID: 8597 RVA: 0x0007A94F File Offset: 0x00078B4F
		// (set) Token: 0x06002196 RID: 8598 RVA: 0x0007A957 File Offset: 0x00078B57
		[DataSourceProperty]
		public ArmyManagementSortControllerVM SortControllerVM
		{
			get
			{
				return this._sortControllerVM;
			}
			set
			{
				if (value != this._sortControllerVM)
				{
					this._sortControllerVM = value;
					base.OnPropertyChangedWithValue<ArmyManagementSortControllerVM>(value, "SortControllerVM");
				}
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x06002197 RID: 8599 RVA: 0x0007A975 File Offset: 0x00078B75
		// (set) Token: 0x06002198 RID: 8600 RVA: 0x0007A97D File Offset: 0x00078B7D
		[DataSourceProperty]
		public string BoostTitleText
		{
			get
			{
				return this._boostTitleText;
			}
			set
			{
				if (value != this._boostTitleText)
				{
					this._boostTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "BoostTitleText");
				}
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x06002199 RID: 8601 RVA: 0x0007A9A0 File Offset: 0x00078BA0
		// (set) Token: 0x0600219A RID: 8602 RVA: 0x0007A9A8 File Offset: 0x00078BA8
		[DataSourceProperty]
		public string DisbandArmyText
		{
			get
			{
				return this._disbandArmyText;
			}
			set
			{
				if (value != this._disbandArmyText)
				{
					this._disbandArmyText = value;
					base.OnPropertyChangedWithValue<string>(value, "DisbandArmyText");
				}
			}
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x0600219B RID: 8603 RVA: 0x0007A9CB File Offset: 0x00078BCB
		// (set) Token: 0x0600219C RID: 8604 RVA: 0x0007A9D3 File Offset: 0x00078BD3
		[DataSourceProperty]
		public string CohesionBoostAmountText
		{
			get
			{
				return this._cohesionBoostAmountText;
			}
			set
			{
				if (value != this._cohesionBoostAmountText)
				{
					this._cohesionBoostAmountText = value;
					base.OnPropertyChangedWithValue<string>(value, "CohesionBoostAmountText");
				}
			}
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x0600219D RID: 8605 RVA: 0x0007A9F6 File Offset: 0x00078BF6
		// (set) Token: 0x0600219E RID: 8606 RVA: 0x0007A9FE File Offset: 0x00078BFE
		[DataSourceProperty]
		public string DistanceText
		{
			get
			{
				return this._distanceText;
			}
			set
			{
				if (value != this._distanceText)
				{
					this._distanceText = value;
					base.OnPropertyChangedWithValue<string>(value, "DistanceText");
				}
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x0600219F RID: 8607 RVA: 0x0007AA21 File Offset: 0x00078C21
		// (set) Token: 0x060021A0 RID: 8608 RVA: 0x0007AA29 File Offset: 0x00078C29
		[DataSourceProperty]
		public string CostText
		{
			get
			{
				return this._costText;
			}
			set
			{
				if (value != this._costText)
				{
					this._costText = value;
					base.OnPropertyChangedWithValue<string>(value, "CostText");
				}
			}
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x060021A1 RID: 8609 RVA: 0x0007AA4C File Offset: 0x00078C4C
		// (set) Token: 0x060021A2 RID: 8610 RVA: 0x0007AA54 File Offset: 0x00078C54
		[DataSourceProperty]
		public string OwnerText
		{
			get
			{
				return this._ownerText;
			}
			set
			{
				if (value != this._ownerText)
				{
					this._ownerText = value;
					base.OnPropertyChangedWithValue<string>(value, "OwnerText");
				}
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x060021A3 RID: 8611 RVA: 0x0007AA77 File Offset: 0x00078C77
		// (set) Token: 0x060021A4 RID: 8612 RVA: 0x0007AA7F File Offset: 0x00078C7F
		[DataSourceProperty]
		public string StrengthText
		{
			get
			{
				return this._strengthText;
			}
			set
			{
				if (value != this._strengthText)
				{
					this._strengthText = value;
					base.OnPropertyChangedWithValue<string>(value, "StrengthText");
				}
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x060021A5 RID: 8613 RVA: 0x0007AAA2 File Offset: 0x00078CA2
		// (set) Token: 0x060021A6 RID: 8614 RVA: 0x0007AAAA File Offset: 0x00078CAA
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

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x060021A7 RID: 8615 RVA: 0x0007AACD File Offset: 0x00078CCD
		// (set) Token: 0x060021A8 RID: 8616 RVA: 0x0007AAD5 File Offset: 0x00078CD5
		[DataSourceProperty]
		public string LordsText
		{
			get
			{
				return this._lordsText;
			}
			set
			{
				if (value != this._lordsText)
				{
					this._lordsText = value;
					base.OnPropertyChangedWithValue<string>(value, "LordsText");
				}
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x060021A9 RID: 8617 RVA: 0x0007AAF8 File Offset: 0x00078CF8
		// (set) Token: 0x060021AA RID: 8618 RVA: 0x0007AB00 File Offset: 0x00078D00
		[DataSourceProperty]
		public string TotalInfluence
		{
			get
			{
				return this._totalInfluence;
			}
			set
			{
				if (value != this._totalInfluence)
				{
					this._totalInfluence = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalInfluence");
				}
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x060021AB RID: 8619 RVA: 0x0007AB23 File Offset: 0x00078D23
		// (set) Token: 0x060021AC RID: 8620 RVA: 0x0007AB2B File Offset: 0x00078D2B
		[DataSourceProperty]
		public int TotalStrength
		{
			get
			{
				return this._totalStrength;
			}
			set
			{
				if (value != this._totalStrength)
				{
					this._totalStrength = value;
					base.OnPropertyChangedWithValue(value, "TotalStrength");
				}
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x060021AD RID: 8621 RVA: 0x0007AB49 File Offset: 0x00078D49
		// (set) Token: 0x060021AE RID: 8622 RVA: 0x0007AB54 File Offset: 0x00078D54
		[DataSourceProperty]
		public int TotalCost
		{
			get
			{
				return this._totalCost;
			}
			set
			{
				if (value != this._totalCost)
				{
					this._totalCost = value;
					this.CanAffordInfluenceCost = this.TotalCost <= 0 || (float)this.TotalCost <= Hero.MainHero.Clan.Influence;
					base.OnPropertyChangedWithValue(value, "TotalCost");
				}
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x060021AF RID: 8623 RVA: 0x0007ABAA File Offset: 0x00078DAA
		// (set) Token: 0x060021B0 RID: 8624 RVA: 0x0007ABB2 File Offset: 0x00078DB2
		[DataSourceProperty]
		public string TotalLords
		{
			get
			{
				return this._totalLords;
			}
			set
			{
				if (value != this._totalLords)
				{
					this._totalLords = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalLords");
				}
			}
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x060021B1 RID: 8625 RVA: 0x0007ABD5 File Offset: 0x00078DD5
		// (set) Token: 0x060021B2 RID: 8626 RVA: 0x0007ABDD File Offset: 0x00078DDD
		[DataSourceProperty]
		public bool CanCreateArmy
		{
			get
			{
				return this._canCreateArmy;
			}
			set
			{
				if (value != this._canCreateArmy)
				{
					this._canCreateArmy = value;
					base.OnPropertyChangedWithValue(value, "CanCreateArmy");
				}
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x060021B3 RID: 8627 RVA: 0x0007ABFB File Offset: 0x00078DFB
		// (set) Token: 0x060021B4 RID: 8628 RVA: 0x0007AC03 File Offset: 0x00078E03
		[DataSourceProperty]
		public bool CanBoostCohesion
		{
			get
			{
				return this._canBoostCohesion;
			}
			set
			{
				if (value != this._canBoostCohesion)
				{
					this._canBoostCohesion = value;
					base.OnPropertyChangedWithValue(value, "CanBoostCohesion");
				}
			}
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x060021B5 RID: 8629 RVA: 0x0007AC21 File Offset: 0x00078E21
		// (set) Token: 0x060021B6 RID: 8630 RVA: 0x0007AC29 File Offset: 0x00078E29
		[DataSourceProperty]
		public bool CanDisbandArmy
		{
			get
			{
				return this._canDisbandArmy;
			}
			set
			{
				if (value != this._canDisbandArmy)
				{
					this._canDisbandArmy = value;
					base.OnPropertyChangedWithValue(value, "CanDisbandArmy");
				}
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x060021B7 RID: 8631 RVA: 0x0007AC47 File Offset: 0x00078E47
		// (set) Token: 0x060021B8 RID: 8632 RVA: 0x0007AC4F File Offset: 0x00078E4F
		[DataSourceProperty]
		public bool CanConfirm
		{
			get
			{
				return this._canConfirm;
			}
			set
			{
				if (value != this._canConfirm)
				{
					this._canConfirm = value;
					base.OnPropertyChangedWithValue(value, "CanConfirm");
				}
			}
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x060021B9 RID: 8633 RVA: 0x0007AC6D File Offset: 0x00078E6D
		// (set) Token: 0x060021BA RID: 8634 RVA: 0x0007AC75 File Offset: 0x00078E75
		[DataSourceProperty]
		public bool CanAffordInfluenceCost
		{
			get
			{
				return this._canAffordInfluenceCost;
			}
			set
			{
				if (value != this._canAffordInfluenceCost)
				{
					this._canAffordInfluenceCost = value;
					base.OnPropertyChangedWithValue(value, "CanAffordInfluenceCost");
				}
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x060021BB RID: 8635 RVA: 0x0007AC93 File Offset: 0x00078E93
		// (set) Token: 0x060021BC RID: 8636 RVA: 0x0007AC9B File Offset: 0x00078E9B
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x060021BD RID: 8637 RVA: 0x0007ACBE File Offset: 0x00078EBE
		// (set) Token: 0x060021BE RID: 8638 RVA: 0x0007ACC6 File Offset: 0x00078EC6
		[DataSourceProperty]
		public string ClanText
		{
			get
			{
				return this._clanText;
			}
			set
			{
				if (value != this._clanText)
				{
					this._clanText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClanText");
				}
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x060021BF RID: 8639 RVA: 0x0007ACE9 File Offset: 0x00078EE9
		// (set) Token: 0x060021C0 RID: 8640 RVA: 0x0007ACF1 File Offset: 0x00078EF1
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

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x060021C1 RID: 8641 RVA: 0x0007AD14 File Offset: 0x00078F14
		// (set) Token: 0x060021C2 RID: 8642 RVA: 0x0007AD1C File Offset: 0x00078F1C
		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
			set
			{
				if (value != this._cancelText)
				{
					this._cancelText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelText");
				}
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x060021C3 RID: 8643 RVA: 0x0007AD3F File Offset: 0x00078F3F
		// (set) Token: 0x060021C4 RID: 8644 RVA: 0x0007AD47 File Offset: 0x00078F47
		[DataSourceProperty]
		public string DoneText
		{
			get
			{
				return this._doneText;
			}
			set
			{
				if (value != this._doneText)
				{
					this._doneText = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneText");
				}
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x060021C5 RID: 8645 RVA: 0x0007AD6A File Offset: 0x00078F6A
		// (set) Token: 0x060021C6 RID: 8646 RVA: 0x0007AD72 File Offset: 0x00078F72
		[DataSourceProperty]
		public ArmyManagementItemVM FocusedItem
		{
			get
			{
				return this._focusedItem;
			}
			set
			{
				if (value != this._focusedItem)
				{
					this._focusedItem = value;
					base.OnPropertyChangedWithValue<ArmyManagementItemVM>(value, "FocusedItem");
				}
			}
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x060021C7 RID: 8647 RVA: 0x0007AD90 File Offset: 0x00078F90
		// (set) Token: 0x060021C8 RID: 8648 RVA: 0x0007AD98 File Offset: 0x00078F98
		[DataSourceProperty]
		public MBBindingList<ArmyManagementItemVM> PartyList
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
					base.OnPropertyChangedWithValue<MBBindingList<ArmyManagementItemVM>>(value, "PartyList");
				}
			}
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x060021C9 RID: 8649 RVA: 0x0007ADB6 File Offset: 0x00078FB6
		// (set) Token: 0x060021CA RID: 8650 RVA: 0x0007ADBE File Offset: 0x00078FBE
		[DataSourceProperty]
		public MBBindingList<ArmyManagementItemVM> PartiesInCart
		{
			get
			{
				return this._partiesInCart;
			}
			set
			{
				if (value != this._partiesInCart)
				{
					this._partiesInCart = value;
					base.OnPropertyChangedWithValue<MBBindingList<ArmyManagementItemVM>>(value, "PartiesInCart");
				}
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x060021CB RID: 8651 RVA: 0x0007ADDC File Offset: 0x00078FDC
		// (set) Token: 0x060021CC RID: 8652 RVA: 0x0007ADE4 File Offset: 0x00078FE4
		[DataSourceProperty]
		public string TotalStrengthText
		{
			get
			{
				return this._totalStrengthText;
			}
			set
			{
				if (value != this._totalStrengthText)
				{
					this._totalStrengthText = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalStrengthText");
				}
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x060021CD RID: 8653 RVA: 0x0007AE07 File Offset: 0x00079007
		// (set) Token: 0x060021CE RID: 8654 RVA: 0x0007AE0F File Offset: 0x0007900F
		[DataSourceProperty]
		public string TotalCostText
		{
			get
			{
				return this._totalCostText;
			}
			set
			{
				if (value != this._totalCostText)
				{
					this._totalCostText = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalCostText");
				}
			}
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x060021CF RID: 8655 RVA: 0x0007AE32 File Offset: 0x00079032
		// (set) Token: 0x060021D0 RID: 8656 RVA: 0x0007AE3A File Offset: 0x0007903A
		[DataSourceProperty]
		public string TotalCostNumbersText
		{
			get
			{
				return this._totalCostNumbersText;
			}
			set
			{
				if (value != this._totalCostNumbersText)
				{
					this._totalCostNumbersText = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalCostNumbersText");
				}
			}
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x060021D1 RID: 8657 RVA: 0x0007AE5D File Offset: 0x0007905D
		// (set) Token: 0x060021D2 RID: 8658 RVA: 0x0007AE65 File Offset: 0x00079065
		[DataSourceProperty]
		public string CohesionText
		{
			get
			{
				return this._cohesionText;
			}
			set
			{
				if (value != this._cohesionText)
				{
					this._cohesionText = value;
					base.OnPropertyChangedWithValue<string>(value, "CohesionText");
				}
			}
		}

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x060021D3 RID: 8659 RVA: 0x0007AE88 File Offset: 0x00079088
		// (set) Token: 0x060021D4 RID: 8660 RVA: 0x0007AE90 File Offset: 0x00079090
		[DataSourceProperty]
		public int Cohesion
		{
			get
			{
				return this._cohesion;
			}
			set
			{
				if (value != this._cohesion)
				{
					this._cohesion = value;
					base.OnPropertyChangedWithValue(value, "Cohesion");
				}
			}
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x060021D5 RID: 8661 RVA: 0x0007AEAE File Offset: 0x000790AE
		// (set) Token: 0x060021D6 RID: 8662 RVA: 0x0007AEB6 File Offset: 0x000790B6
		[DataSourceProperty]
		public int CohesionBoostCost
		{
			get
			{
				return this._cohesionBoostCost;
			}
			set
			{
				if (value != this._cohesionBoostCost)
				{
					this._cohesionBoostCost = value;
					base.OnPropertyChangedWithValue(value, "CohesionBoostCost");
				}
			}
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x060021D7 RID: 8663 RVA: 0x0007AED4 File Offset: 0x000790D4
		// (set) Token: 0x060021D8 RID: 8664 RVA: 0x0007AEDC File Offset: 0x000790DC
		[DataSourceProperty]
		public bool PlayerHasArmy
		{
			get
			{
				return this._playerHasArmy;
			}
			set
			{
				if (value != this._playerHasArmy)
				{
					this._playerHasArmy = value;
					base.OnPropertyChangedWithValue(value, "PlayerHasArmy");
				}
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x060021D9 RID: 8665 RVA: 0x0007AEFA File Offset: 0x000790FA
		// (set) Token: 0x060021DA RID: 8666 RVA: 0x0007AF02 File Offset: 0x00079102
		[DataSourceProperty]
		public string MoraleText
		{
			get
			{
				return this._moraleText;
			}
			set
			{
				if (value != this._moraleText)
				{
					this._moraleText = value;
					base.OnPropertyChangedWithValue<string>(value, "MoraleText");
				}
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x060021DB RID: 8667 RVA: 0x0007AF25 File Offset: 0x00079125
		// (set) Token: 0x060021DC RID: 8668 RVA: 0x0007AF2D File Offset: 0x0007912D
		[DataSourceProperty]
		public string FoodText
		{
			get
			{
				return this._foodText;
			}
			set
			{
				if (value != this._foodText)
				{
					this._foodText = value;
					base.OnPropertyChangedWithValue<string>(value, "FoodText");
				}
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x060021DD RID: 8669 RVA: 0x0007AF50 File Offset: 0x00079150
		// (set) Token: 0x060021DE RID: 8670 RVA: 0x0007AF58 File Offset: 0x00079158
		[DataSourceProperty]
		public int NewCohesion
		{
			get
			{
				return this._newCohesion;
			}
			set
			{
				if (value != this._newCohesion)
				{
					this._newCohesion = value;
					base.OnPropertyChangedWithValue(value, "NewCohesion");
				}
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x060021DF RID: 8671 RVA: 0x0007AF76 File Offset: 0x00079176
		// (set) Token: 0x060021E0 RID: 8672 RVA: 0x0007AF7E File Offset: 0x0007917E
		[DataSourceProperty]
		public BasicTooltipViewModel CohesionHint
		{
			get
			{
				return this._cohesionHint;
			}
			set
			{
				if (value != this._cohesionHint)
				{
					this._cohesionHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "CohesionHint");
				}
			}
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x060021E1 RID: 8673 RVA: 0x0007AF9C File Offset: 0x0007919C
		// (set) Token: 0x060021E2 RID: 8674 RVA: 0x0007AFA4 File Offset: 0x000791A4
		[DataSourceProperty]
		public HintViewModel MoraleHint
		{
			get
			{
				return this._moraleHint;
			}
			set
			{
				if (value != this._moraleHint)
				{
					this._moraleHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "MoraleHint");
				}
			}
		}

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x060021E3 RID: 8675 RVA: 0x0007AFC2 File Offset: 0x000791C2
		// (set) Token: 0x060021E4 RID: 8676 RVA: 0x0007AFCA File Offset: 0x000791CA
		[DataSourceProperty]
		public HintViewModel BoostCohesionHint
		{
			get
			{
				return this._boostCohesionHint;
			}
			set
			{
				if (value != this._boostCohesionHint)
				{
					this._boostCohesionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "BoostCohesionHint");
				}
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x060021E5 RID: 8677 RVA: 0x0007AFE8 File Offset: 0x000791E8
		// (set) Token: 0x060021E6 RID: 8678 RVA: 0x0007AFF0 File Offset: 0x000791F0
		[DataSourceProperty]
		public HintViewModel DisbandArmyHint
		{
			get
			{
				return this._disbandArmyHint;
			}
			set
			{
				if (value != this._disbandArmyHint)
				{
					this._disbandArmyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DisbandArmyHint");
				}
			}
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x060021E7 RID: 8679 RVA: 0x0007B00E File Offset: 0x0007920E
		// (set) Token: 0x060021E8 RID: 8680 RVA: 0x0007B016 File Offset: 0x00079216
		[DataSourceProperty]
		public HintViewModel DoneHint
		{
			get
			{
				return this._doneHint;
			}
			set
			{
				if (value != this._doneHint)
				{
					this._doneHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DoneHint");
				}
			}
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x060021E9 RID: 8681 RVA: 0x0007B034 File Offset: 0x00079234
		// (set) Token: 0x060021EA RID: 8682 RVA: 0x0007B03C File Offset: 0x0007923C
		[DataSourceProperty]
		public HintViewModel FoodHint
		{
			get
			{
				return this._foodHint;
			}
			set
			{
				if (value != this._foodHint)
				{
					this._foodHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "FoodHint");
				}
			}
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x0007B05A File Offset: 0x0007925A
		public void SetResetInputKey(HotKey hotKey)
		{
			this.ResetInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x0007B069 File Offset: 0x00079269
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x0007B078 File Offset: 0x00079278
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x0007B087 File Offset: 0x00079287
		public void SetRemoveInputKey(HotKey hotKey)
		{
			this.RemoveInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x060021EF RID: 8687 RVA: 0x0007B096 File Offset: 0x00079296
		// (set) Token: 0x060021F0 RID: 8688 RVA: 0x0007B09E File Offset: 0x0007929E
		[DataSourceProperty]
		public InputKeyItemVM ResetInputKey
		{
			get
			{
				return this._resetInputKey;
			}
			set
			{
				if (value != this._resetInputKey)
				{
					this._resetInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ResetInputKey");
				}
			}
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x060021F1 RID: 8689 RVA: 0x0007B0BC File Offset: 0x000792BC
		// (set) Token: 0x060021F2 RID: 8690 RVA: 0x0007B0C4 File Offset: 0x000792C4
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x060021F3 RID: 8691 RVA: 0x0007B0E2 File Offset: 0x000792E2
		// (set) Token: 0x060021F4 RID: 8692 RVA: 0x0007B0EA File Offset: 0x000792EA
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x060021F5 RID: 8693 RVA: 0x0007B108 File Offset: 0x00079308
		// (set) Token: 0x060021F6 RID: 8694 RVA: 0x0007B110 File Offset: 0x00079310
		[DataSourceProperty]
		public InputKeyItemVM RemoveInputKey
		{
			get
			{
				return this._removeInputKey;
			}
			set
			{
				if (value != this._removeInputKey)
				{
					this._removeInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "RemoveInputKey");
					foreach (ArmyManagementItemVM armyManagementItemVM in this.PartyList)
					{
						armyManagementItemVM.RemoveInputKey = value;
					}
				}
			}
		}

		// Token: 0x04000F95 RID: 3989
		private readonly Action _onClose;

		// Token: 0x04000F96 RID: 3990
		private readonly ArmyManagementItemVM _mainPartyItem;

		// Token: 0x04000F97 RID: 3991
		private readonly ArmyManagementVM.ManagementItemComparer _itemComparer;

		// Token: 0x04000F98 RID: 3992
		private readonly float _initialInfluence;

		// Token: 0x04000F99 RID: 3993
		private string _latestTutorialElementID;

		// Token: 0x04000F9A RID: 3994
		private string _playerDoesntHaveEnoughInfluenceStr;

		// Token: 0x04000F9B RID: 3995
		private const int _cohesionBoostAmount = 10;

		// Token: 0x04000F9C RID: 3996
		private int _influenceSpentForCohesionBoosting;

		// Token: 0x04000F9D RID: 3997
		private int _boostedCohesion;

		// Token: 0x04000F9E RID: 3998
		private string _titleText;

		// Token: 0x04000F9F RID: 3999
		private string _boostTitleText;

		// Token: 0x04000FA0 RID: 4000
		private string _cancelText;

		// Token: 0x04000FA1 RID: 4001
		private string _doneText;

		// Token: 0x04000FA2 RID: 4002
		private bool _canCreateArmy;

		// Token: 0x04000FA3 RID: 4003
		private bool _canBoostCohesion;

		// Token: 0x04000FA4 RID: 4004
		private List<MobileParty> _currentParties;

		// Token: 0x04000FA5 RID: 4005
		private ArmyManagementItemVM _focusedItem;

		// Token: 0x04000FA6 RID: 4006
		private MBBindingList<ArmyManagementItemVM> _partyList;

		// Token: 0x04000FA7 RID: 4007
		private MBBindingList<ArmyManagementItemVM> _partiesInCart;

		// Token: 0x04000FA8 RID: 4008
		private MBBindingList<ArmyManagementItemVM> _partiesToRemove;

		// Token: 0x04000FA9 RID: 4009
		private ArmyManagementSortControllerVM _sortControllerVM;

		// Token: 0x04000FAA RID: 4010
		private int _totalStrength;

		// Token: 0x04000FAB RID: 4011
		private int _totalCost;

		// Token: 0x04000FAC RID: 4012
		private int _cohesion;

		// Token: 0x04000FAD RID: 4013
		private int _cohesionBoostCost;

		// Token: 0x04000FAE RID: 4014
		private string _cohesionText;

		// Token: 0x04000FAF RID: 4015
		private int _newCohesion;

		// Token: 0x04000FB0 RID: 4016
		private string _totalStrengthText;

		// Token: 0x04000FB1 RID: 4017
		private string _totalCostText;

		// Token: 0x04000FB2 RID: 4018
		private string _totalCostNumbersText;

		// Token: 0x04000FB3 RID: 4019
		private string _totalInfluence;

		// Token: 0x04000FB4 RID: 4020
		private string _totalLords;

		// Token: 0x04000FB5 RID: 4021
		private string _costText;

		// Token: 0x04000FB6 RID: 4022
		private string _strengthText;

		// Token: 0x04000FB7 RID: 4023
		private string _shipCountText;

		// Token: 0x04000FB8 RID: 4024
		private string _lordsText;

		// Token: 0x04000FB9 RID: 4025
		private string _distanceText;

		// Token: 0x04000FBA RID: 4026
		private string _clanText;

		// Token: 0x04000FBB RID: 4027
		private string _ownerText;

		// Token: 0x04000FBC RID: 4028
		private string _nameText;

		// Token: 0x04000FBD RID: 4029
		private string _disbandArmyText;

		// Token: 0x04000FBE RID: 4030
		private string _cohesionBoostAmountText;

		// Token: 0x04000FBF RID: 4031
		private bool _playerHasArmy;

		// Token: 0x04000FC0 RID: 4032
		private bool _canDisbandArmy;

		// Token: 0x04000FC1 RID: 4033
		private bool _canConfirm;

		// Token: 0x04000FC2 RID: 4034
		private bool _canAffordInfluenceCost;

		// Token: 0x04000FC3 RID: 4035
		private string _moraleText;

		// Token: 0x04000FC4 RID: 4036
		private string _foodText;

		// Token: 0x04000FC5 RID: 4037
		private BasicTooltipViewModel _cohesionHint;

		// Token: 0x04000FC6 RID: 4038
		private HintViewModel _moraleHint;

		// Token: 0x04000FC7 RID: 4039
		private HintViewModel _foodHint;

		// Token: 0x04000FC8 RID: 4040
		private HintViewModel _boostCohesionHint;

		// Token: 0x04000FC9 RID: 4041
		private HintViewModel _disbandArmyHint;

		// Token: 0x04000FCA RID: 4042
		private HintViewModel _doneHint;

		// Token: 0x04000FCB RID: 4043
		public ElementNotificationVM _tutorialNotification;

		// Token: 0x04000FCC RID: 4044
		private InputKeyItemVM _resetInputKey;

		// Token: 0x04000FCD RID: 4045
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000FCE RID: 4046
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000FCF RID: 4047
		private InputKeyItemVM _removeInputKey;

		// Token: 0x020002F1 RID: 753
		public class ManagementItemComparer : IComparer<ArmyManagementItemVM>
		{
			// Token: 0x06002763 RID: 10083 RVA: 0x00085364 File Offset: 0x00083564
			public int Compare(ArmyManagementItemVM x, ArmyManagementItemVM y)
			{
				if (x.IsMainHero)
				{
					return -1;
				}
				return y.IsAlreadyWithPlayer.CompareTo(x.IsAlreadyWithPlayer);
			}
		}
	}
}
