using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy
{
	// Token: 0x0200006F RID: 111
	public class KingdomDiplomacyVM : KingdomCategoryVM
	{
		// Token: 0x060008F2 RID: 2290 RVA: 0x00027DB8 File Offset: 0x00025FB8
		public KingdomDiplomacyVM(Action<KingdomDecision> forceDecision)
		{
			this._forceDecision = forceDecision;
			this._playerKingdom = Hero.MainHero.MapFaction as Kingdom;
			this.PlayerWars = new MBBindingList<KingdomWarItemVM>();
			this.PlayerTruces = new MBBindingList<KingdomTruceItemVM>();
			this.WarsSortController = new KingdomWarSortControllerVM(ref this._playerWars);
			this.Actions = new MBBindingList<KingdomDiplomacyProposalActionItemVM>();
			this.ExecuteShowStatComparisons();
			this.RefreshValues();
			this.SetDefaultSelectedItem();
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00027E2C File Offset: 0x0002602C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.BehaviorSelection = new SelectorVM<SelectorItemVM>(0, new Action<SelectorVM<SelectorItemVM>>(this.OnBehaviorSelectionChanged));
			this.BehaviorSelection.AddItem(new SelectorItemVM(GameTexts.FindText("str_kingdom_war_strategy_balanced", null), GameTexts.FindText("str_kingdom_war_strategy_balanced_desc", null)));
			this.BehaviorSelection.AddItem(new SelectorItemVM(GameTexts.FindText("str_kingdom_war_strategy_defensive", null), GameTexts.FindText("str_kingdom_war_strategy_defensive_desc", null)));
			this.BehaviorSelection.AddItem(new SelectorItemVM(GameTexts.FindText("str_kingdom_war_strategy_offensive", null), GameTexts.FindText("str_kingdom_war_strategy_offensive_desc", null)));
			this.RefreshDiplomacyList();
			this.BehaviorSelectionTitle = GameTexts.FindText("str_kingdom_war_strategy", null).ToString();
			base.NoItemSelectedText = GameTexts.FindText("str_kingdom_no_war_selected", null).ToString();
			this.PlayerWarsText = GameTexts.FindText("str_kingdom_at_war", null).ToString();
			this.PlayerTrucesText = GameTexts.FindText("str_kingdom_at_peace", null).ToString();
			this.WarsText = GameTexts.FindText("str_diplomatic_group", null).ToString();
			this.ShowStatBarsHint = new HintViewModel(GameTexts.FindText("str_kingdom_war_show_comparison_bars", null), null);
			this.ShowWarLogsHint = new HintViewModel(GameTexts.FindText("str_kingdom_war_show_war_logs", null), null);
			this.PlayerWars.ApplyActionOnAllItems(delegate(KingdomWarItemVM x)
			{
				x.RefreshValues();
			});
			this.PlayerTruces.ApplyActionOnAllItems(delegate(KingdomTruceItemVM x)
			{
				x.RefreshValues();
			});
			KingdomDiplomacyItemVM currentSelectedDiplomacyItem = this.CurrentSelectedDiplomacyItem;
			if (currentSelectedDiplomacyItem != null)
			{
				currentSelectedDiplomacyItem.RefreshValues();
			}
			this.Actions.ApplyActionOnAllItems(delegate(KingdomDiplomacyProposalActionItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x00027FFC File Offset: 0x000261FC
		public void RefreshDiplomacyList()
		{
			Kingdom kingdom = Clan.PlayerClan.Kingdom;
			int num;
			if (kingdom == null)
			{
				num = 0;
			}
			else
			{
				num = kingdom.UnresolvedDecisions.Count<KingdomDecision>((KingdomDecision d) => !d.ShouldBeCancelled());
			}
			base.NotificationCount = num;
			this.PlayerWars.Clear();
			this.PlayerTruces.Clear();
			foreach (StanceLink stanceLink in from x in this._playerKingdom.FactionsAtWarWith
				select this._playerKingdom.GetStanceWith(x) into w
				orderby w.Faction1.Name.ToString() + w.Faction2.Name.ToString()
				select w)
			{
				if (stanceLink.Faction1.IsKingdomFaction && stanceLink.Faction2.IsKingdomFaction)
				{
					this.PlayerWars.Add(new KingdomWarItemVM(stanceLink, new Action<KingdomWarItemVM>(this.OnDiplomacyItemSelection)));
				}
			}
			foreach (Kingdom kingdom2 in Kingdom.All)
			{
				if (kingdom2 != this._playerKingdom && !kingdom2.IsEliminated && (DiplomacyHelper.IsSameFactionAndNotEliminated(kingdom2, this._playerKingdom) || FactionManager.IsNeutralWithFaction(kingdom2, this._playerKingdom)))
				{
					this.PlayerTruces.Add(new KingdomTruceItemVM(this._playerKingdom, kingdom2, new Action<KingdomDiplomacyItemVM>(this.OnDiplomacyItemSelection)));
				}
			}
			GameTexts.SetVariable("STR", this.PlayerWars.Count);
			this.NumOfPlayerWarsText = GameTexts.FindText("str_STR_in_parentheses", null).ToString();
			GameTexts.SetVariable("STR", this.PlayerTruces.Count);
			this.NumOfPlayerTrucesText = GameTexts.FindText("str_STR_in_parentheses", null).ToString();
			this.SetDefaultSelectedItem();
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x000281F0 File Offset: 0x000263F0
		public void SelectKingdom(Kingdom kingdom)
		{
			bool flag = false;
			foreach (KingdomWarItemVM kingdomWarItemVM in this.PlayerWars)
			{
				if (kingdomWarItemVM.Faction1 == kingdom || kingdomWarItemVM.Faction2 == kingdom)
				{
					this.OnSetCurrentDiplomacyItem(kingdomWarItemVM);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				foreach (KingdomTruceItemVM kingdomTruceItemVM in this.PlayerTruces)
				{
					if (kingdomTruceItemVM.Faction1 == kingdom || kingdomTruceItemVM.Faction2 == kingdom)
					{
						this.OnSetCurrentDiplomacyItem(kingdomTruceItemVM);
						flag = true;
						break;
					}
				}
			}
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x000282B0 File Offset: 0x000264B0
		private void OnSetCurrentDiplomacyItem(KingdomDiplomacyItemVM item)
		{
			this.Actions.Clear();
			if (item is KingdomWarItemVM)
			{
				this.OnSetWarItem(item as KingdomWarItemVM);
			}
			else if (item is KingdomTruceItemVM)
			{
				this.OnSetPeaceItem(item as KingdomTruceItemVM);
			}
			this.RefreshCurrentWarVisuals(item);
			this.UpdateBehaviorSelection();
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x00028300 File Offset: 0x00026500
		private void OnSetWarItem(KingdomWarItemVM item)
		{
			KingdomDiplomacyVM.<>c__DisplayClass9_0 CS$<>8__locals1 = new KingdomDiplomacyVM.<>c__DisplayClass9_0();
			CS$<>8__locals1.item = item;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.unresolvedPeaceDecision = Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision d)
			{
				MakePeaceKingdomDecision makePeaceKingdomDecision;
				return (makePeaceKingdomDecision = d as MakePeaceKingdomDecision) != null && makePeaceKingdomDecision.FactionToMakePeaceWith == CS$<>8__locals1.item.Faction2 && !d.ShouldBeCancelled();
			});
			if (CS$<>8__locals1.unresolvedPeaceDecision != null)
			{
				TextObject textObject;
				this.Actions.Add(new KingdomDiplomacyProposalActionItemVM(GameTexts.FindText("str_resolve", null), GameTexts.FindText("str_resolve_explanation", null), 0, this.GetAreProposalActionsEnabledWithReason(KingdomDiplomacyVM.DiplomacyItemType.Peace, 0f, out textObject), textObject, delegate
				{
					CS$<>8__locals1.<>4__this._forceDecision(CS$<>8__locals1.unresolvedPeaceDecision);
				}));
				return;
			}
			int durationInDays;
			int dailyPeaceTributeToPay = Campaign.Current.Models.DiplomacyModel.GetDailyTributeToPay(Clan.PlayerClan, CS$<>8__locals1.item.Faction2.Leader.Clan, out durationInDays);
			dailyPeaceTributeToPay = 10 * (dailyPeaceTributeToPay / 10);
			TextObject textObject2 = ((dailyPeaceTributeToPay == 0) ? GameTexts.FindText("str_propose_peace_explanation", null) : ((dailyPeaceTributeToPay > 0) ? GameTexts.FindText("str_propose_peace_explanation_pay_tribute", null) : GameTexts.FindText("str_propose_peace_explanation_get_tribute", null)));
			textObject2.SetTextVariable("SUPPORT", this.CalculatePeaceSupport(CS$<>8__locals1.item.Faction2, dailyPeaceTributeToPay, durationInDays)).SetTextVariable("TRIBUTE_AMOUNT", MathF.Abs(dailyPeaceTributeToPay)).SetTextVariable("TRIBUTE_DURATION", durationInDays);
			int influenceCostOfProposingPeace = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfProposingPeace(Clan.PlayerClan);
			TextObject textObject3;
			this.Actions.Add(new KingdomDiplomacyProposalActionItemVM((this._playerKingdom.Clans.Count > 1) ? GameTexts.FindText("str_policy_propose", null) : GameTexts.FindText("str_policy_enact", null), textObject2, influenceCostOfProposingPeace, this.GetIsProposingPeaceEnabledWithReason(CS$<>8__locals1.item, (float)influenceCostOfProposingPeace, out textObject3), textObject3, delegate
			{
				CS$<>8__locals1.<>4__this.OnDeclarePeace(CS$<>8__locals1.item, dailyPeaceTributeToPay, durationInDays);
			}));
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x000284F8 File Offset: 0x000266F8
		private void OnSetPeaceItem(KingdomTruceItemVM item)
		{
			KingdomDecision unresolvedAllianceDecision = Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision d)
			{
				StartAllianceDecision startAllianceDecision;
				return (startAllianceDecision = d as StartAllianceDecision) != null && startAllianceDecision.KingdomToStartAllianceWith == item.Faction2 && !d.ShouldBeCancelled();
			});
			if (unresolvedAllianceDecision != null)
			{
				TextObject textObject;
				this.Actions.Add(new KingdomDiplomacyProposalActionItemVM(GameTexts.FindText("str_resolve", null), GameTexts.FindText("str_resolve_explanation", null), 0, this.GetAreProposalActionsEnabledWithReason(KingdomDiplomacyVM.DiplomacyItemType.Alliance, 0f, out textObject), textObject, delegate
				{
					this._forceDecision(unresolvedAllianceDecision);
				}));
			}
			else if (!DiplomacyHelper.HasAllianceWithFaction(item.Faction1, item.Faction2))
			{
				int influenceCostOfProposingStartingAlliance = Campaign.Current.Models.AllianceModel.GetInfluenceCostOfProposingStartingAlliance(Clan.PlayerClan);
				TextObject textObject2;
				this.Actions.Add(new KingdomDiplomacyProposalActionItemVM((this._playerKingdom.Clans.Count > 1) ? GameTexts.FindText("str_policy_propose", null) : GameTexts.FindText("str_policy_enact", null), GameTexts.FindText("str_propose_alliance_explanation", null).SetTextVariable("SUPPORT", this.CalculateAllianceSupport(item.Faction2)), influenceCostOfProposingStartingAlliance, this.GetIsProposingAllianceEnabledWithReason(item, (float)influenceCostOfProposingStartingAlliance, out textObject2), textObject2, delegate
				{
					this.OnStartAlliance(item);
				}));
			}
			KingdomDecision unresolvedWarDecision = Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision d)
			{
				DeclareWarDecision declareWarDecision;
				return (declareWarDecision = d as DeclareWarDecision) != null && declareWarDecision.FactionToDeclareWarOn == item.Faction2 && !d.ShouldBeCancelled();
			});
			if (unresolvedWarDecision != null)
			{
				TextObject textObject3;
				this.Actions.Add(new KingdomDiplomacyProposalActionItemVM(GameTexts.FindText("str_resolve", null), GameTexts.FindText("str_resolve_explanation", null), 0, this.GetAreProposalActionsEnabledWithReason(KingdomDiplomacyVM.DiplomacyItemType.War, 0f, out textObject3), textObject3, delegate
				{
					this._forceDecision(unresolvedWarDecision);
				}));
			}
			else
			{
				int influenceCostOfProposingWar = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfProposingWar(Clan.PlayerClan);
				TextObject textObject4;
				this.Actions.Add(new KingdomDiplomacyProposalActionItemVM((this._playerKingdom.Clans.Count > 1) ? GameTexts.FindText("str_policy_propose", null) : GameTexts.FindText("str_policy_enact", null), GameTexts.FindText("str_propose_war_explanation", null).SetTextVariable("SUPPORT", this.CalculateWarSupport(item.Faction2)), influenceCostOfProposingWar, this.GetIsProposingWarEnabledWithReason(item, (float)influenceCostOfProposingWar, out textObject4), textObject4, delegate
				{
					this.OnDeclareWar(item);
				}));
			}
			KingdomDecision unresolvedTradeAgreementDecision = Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision d)
			{
				TradeAgreementDecision tradeAgreementDecision;
				return (tradeAgreementDecision = d as TradeAgreementDecision) != null && tradeAgreementDecision.TargetKingdom == item.Faction2 && !d.ShouldBeCancelled();
			});
			if (unresolvedTradeAgreementDecision != null)
			{
				TextObject textObject5;
				this.Actions.Add(new KingdomDiplomacyProposalActionItemVM(GameTexts.FindText("str_resolve", null), GameTexts.FindText("str_resolve_explanation", null), 0, this.GetAreProposalActionsEnabledWithReason(KingdomDiplomacyVM.DiplomacyItemType.TradeAgreement, 0f, out textObject5), textObject5, delegate
				{
					this._forceDecision(unresolvedTradeAgreementDecision);
				}));
				return;
			}
			ITradeAgreementsCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<ITradeAgreementsCampaignBehavior>();
			TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement;
			if (campaignBehavior != null && !campaignBehavior.HasTradeAgreement(item.Faction1 as Kingdom, item.Faction2 as Kingdom, out tradeAgreement))
			{
				int influenceCostOfProposingTradeAgreement = Campaign.Current.Models.TradeAgreementModel.GetInfluenceCostOfProposingTradeAgreement(Clan.PlayerClan);
				TextObject textObject6;
				this.Actions.Add(new KingdomDiplomacyProposalActionItemVM((this._playerKingdom.Clans.Count > 1) ? GameTexts.FindText("str_policy_propose", null) : GameTexts.FindText("str_policy_enact", null), GameTexts.FindText("str_propose_trade_agreement_explanation", null).SetTextVariable("SUPPORT", this.CalculateTradeAgreementSupport(item.Faction2)), influenceCostOfProposingTradeAgreement, this.GetIsProposingTradeAgreementEnabledWithReason(item, (float)influenceCostOfProposingTradeAgreement, out textObject6), textObject6, delegate
				{
					this.OnStartTradeAgreement(item);
				}));
			}
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x000288AC File Offset: 0x00026AAC
		private bool GetIsProposingWarEnabledWithReason(KingdomTruceItemVM item, float actionInfluenceCost, out TextObject disabledReason)
		{
			TextObject textObject;
			if (!this.GetAreProposalActionsEnabledWithReason(KingdomDiplomacyVM.DiplomacyItemType.War, actionInfluenceCost, out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			TextObject textObject2;
			if (!Campaign.Current.Models.KingdomDecisionPermissionModel.IsWarDecisionAllowedBetweenKingdoms(item.Faction1 as Kingdom, item.Faction2 as Kingdom, out textObject2))
			{
				disabledReason = textObject2;
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x00028908 File Offset: 0x00026B08
		private bool GetIsProposingPeaceEnabledWithReason(KingdomWarItemVM item, float actionInfluenceCost, out TextObject disabledReason)
		{
			TextObject textObject;
			if (!this.GetAreProposalActionsEnabledWithReason(KingdomDiplomacyVM.DiplomacyItemType.Peace, actionInfluenceCost, out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			TextObject textObject2;
			if (!Campaign.Current.Models.KingdomDecisionPermissionModel.IsPeaceDecisionAllowedBetweenKingdoms(item.Faction1 as Kingdom, item.Faction2 as Kingdom, out textObject2))
			{
				disabledReason = textObject2;
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00028964 File Offset: 0x00026B64
		private bool GetIsProposingAllianceEnabledWithReason(KingdomTruceItemVM item, float actionInfluenceCost, out TextObject disabledReason)
		{
			TextObject textObject;
			if (!this.GetAreProposalActionsEnabledWithReason(KingdomDiplomacyVM.DiplomacyItemType.Alliance, actionInfluenceCost, out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			TextObject textObject2;
			if (!new StartAllianceDecision(Clan.PlayerClan, item.Faction2 as Kingdom).CanMakeDecision(out textObject2, true))
			{
				disabledReason = textObject2;
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x000289B0 File Offset: 0x00026BB0
		private bool GetIsProposingTradeAgreementEnabledWithReason(KingdomTruceItemVM item, float actionInfluenceCost, out TextObject disabledReason)
		{
			TextObject textObject;
			if (!this.GetAreProposalActionsEnabledWithReason(KingdomDiplomacyVM.DiplomacyItemType.TradeAgreement, actionInfluenceCost, out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			TextObject textObject2;
			if (!new TradeAgreementDecision(Clan.PlayerClan, item.Faction2 as Kingdom).CanMakeDecision(out textObject2, true))
			{
				disabledReason = textObject2;
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x000289FC File Offset: 0x00026BFC
		private bool GetAreProposalActionsEnabledWithReason(KingdomDiplomacyVM.DiplomacyItemType diplomacyItemType, float actionInfluenceCost, out TextObject disabledReason)
		{
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			if (Clan.PlayerClan.IsUnderMercenaryService)
			{
				switch (diplomacyItemType)
				{
				case KingdomDiplomacyVM.DiplomacyItemType.War:
				case KingdomDiplomacyVM.DiplomacyItemType.Peace:
					disabledReason = GameTexts.FindText("str_cannot_propose_war_truce_while_mercenary", null);
					return false;
				case KingdomDiplomacyVM.DiplomacyItemType.Alliance:
					disabledReason = GameTexts.FindText("str_cannot_propose_alliance_while_mercenary", null);
					return false;
				case KingdomDiplomacyVM.DiplomacyItemType.TradeAgreement:
					disabledReason = GameTexts.FindText("str_cannot_propose_trade_agreement_while_mercenary", null);
					return false;
				}
				disabledReason = TextObject.GetEmpty();
				return false;
			}
			if (actionInfluenceCost > 0f && Clan.PlayerClan.Influence < actionInfluenceCost)
			{
				disabledReason = GameTexts.FindText("str_warning_you_dont_have_enough_influence", null);
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00028AA1 File Offset: 0x00026CA1
		private void RefreshCurrentWarVisuals(KingdomDiplomacyItemVM item)
		{
			if (item != null)
			{
				if (this.CurrentSelectedDiplomacyItem != null)
				{
					this.CurrentSelectedDiplomacyItem.IsSelected = false;
				}
				this.CurrentSelectedDiplomacyItem = item;
				if (this.CurrentSelectedDiplomacyItem != null)
				{
					this.CurrentSelectedDiplomacyItem.IsSelected = true;
				}
			}
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x00028AD5 File Offset: 0x00026CD5
		private void OnDiplomacyItemSelection(KingdomDiplomacyItemVM item)
		{
			if (this.CurrentSelectedDiplomacyItem != item)
			{
				if (this.CurrentSelectedDiplomacyItem != null)
				{
					this.CurrentSelectedDiplomacyItem.IsSelected = false;
				}
				this.CurrentSelectedDiplomacyItem = item;
				base.IsAcceptableItemSelected = item != null;
				this.OnSetCurrentDiplomacyItem(item);
			}
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00028B0C File Offset: 0x00026D0C
		private void OnDeclareWar(KingdomTruceItemVM item)
		{
			DeclareWarDecision declareWarDecision = new DeclareWarDecision(Clan.PlayerClan, item.Faction2);
			Clan.PlayerClan.Kingdom.AddDecision(declareWarDecision, false);
			this._forceDecision(declareWarDecision);
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x00028B48 File Offset: 0x00026D48
		private void OnDeclarePeace(KingdomWarItemVM item, int tributeToPay, int tributeDurationInDays)
		{
			MakePeaceKingdomDecision makePeaceKingdomDecision = new MakePeaceKingdomDecision(Clan.PlayerClan, item.Faction2 as Kingdom, tributeToPay, tributeDurationInDays, true, false);
			Clan.PlayerClan.Kingdom.AddDecision(makePeaceKingdomDecision, false);
			this._forceDecision(makePeaceKingdomDecision);
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00028B8C File Offset: 0x00026D8C
		private void OnStartAlliance(KingdomTruceItemVM item)
		{
			if (item.Faction2.IsKingdomFaction)
			{
				StartAllianceDecision startAllianceDecision = new StartAllianceDecision(Clan.PlayerClan, (Kingdom)item.Faction2);
				Clan.PlayerClan.Kingdom.AddDecision(startAllianceDecision, false);
				this._forceDecision(startAllianceDecision);
			}
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x00028BDC File Offset: 0x00026DDC
		private void OnStartTradeAgreement(KingdomTruceItemVM item)
		{
			if (item.Faction2.IsKingdomFaction)
			{
				TradeAgreementDecision tradeAgreementDecision = new TradeAgreementDecision(Clan.PlayerClan, (Kingdom)item.Faction2);
				Clan.PlayerClan.Kingdom.AddDecision(tradeAgreementDecision, false);
				this._forceDecision(tradeAgreementDecision);
			}
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x00028C29 File Offset: 0x00026E29
		private void ExecuteShowWarLogs()
		{
			this.IsDisplayingWarLogs = true;
			this.IsDisplayingStatComparisons = false;
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x00028C39 File Offset: 0x00026E39
		private void ExecuteShowStatComparisons()
		{
			this.IsDisplayingWarLogs = false;
			this.IsDisplayingStatComparisons = true;
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00028C4C File Offset: 0x00026E4C
		private void SetDefaultSelectedItem()
		{
			KingdomDiplomacyItemVM kingdomDiplomacyItemVM = this.PlayerWars.FirstOrDefault<KingdomWarItemVM>();
			KingdomDiplomacyItemVM kingdomDiplomacyItemVM2 = this.PlayerTruces.FirstOrDefault<KingdomTruceItemVM>();
			this.OnDiplomacyItemSelection(kingdomDiplomacyItemVM ?? kingdomDiplomacyItemVM2);
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00028C80 File Offset: 0x00026E80
		private void UpdateBehaviorSelection()
		{
			if (Hero.MainHero.MapFaction.IsKingdomFaction && Hero.MainHero.MapFaction.Leader == Hero.MainHero && this.CurrentSelectedDiplomacyItem != null)
			{
				StanceLink stanceWith = Hero.MainHero.MapFaction.GetStanceWith(this.CurrentSelectedDiplomacyItem.Faction2);
				this.BehaviorSelection.SelectedIndex = stanceWith.BehaviorPriority;
			}
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00028CE8 File Offset: 0x00026EE8
		private void OnBehaviorSelectionChanged(SelectorVM<SelectorItemVM> s)
		{
			if (!this._isChangingDiplomacyItem && Hero.MainHero.MapFaction.IsKingdomFaction && Hero.MainHero.MapFaction.Leader == Hero.MainHero && this.CurrentSelectedDiplomacyItem != null)
			{
				Hero.MainHero.MapFaction.GetStanceWith(this.CurrentSelectedDiplomacyItem.Faction2).BehaviorPriority = s.SelectedIndex;
			}
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x00028D54 File Offset: 0x00026F54
		private TextObject CalculateWarSupport(IFaction faction)
		{
			DeclareWarDecision declareWarDecision = new DeclareWarDecision(Clan.PlayerClan, faction);
			return GameTexts.FindText("str_decision_outcome_support_status", KingdomElection.GetElectionOutcomeSupport(declareWarDecision, Clan.PlayerClan).ToString());
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00028D90 File Offset: 0x00026F90
		private TextObject CalculateAllianceSupport(IFaction faction)
		{
			StartAllianceDecision startAllianceDecision = new StartAllianceDecision(Clan.PlayerClan, faction as Kingdom);
			return GameTexts.FindText("str_decision_outcome_support_status", KingdomElection.GetElectionOutcomeSupport(startAllianceDecision, Clan.PlayerClan).ToString());
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00028DD4 File Offset: 0x00026FD4
		private TextObject CalculatePeaceSupport(IFaction faction, int dailyTributeToBePaid, int durationInDays)
		{
			MakePeaceKingdomDecision makePeaceKingdomDecision = new MakePeaceKingdomDecision(Clan.PlayerClan, faction, dailyTributeToBePaid, durationInDays, true, false);
			return GameTexts.FindText("str_decision_outcome_support_status", KingdomElection.GetElectionOutcomeSupport(makePeaceKingdomDecision, Clan.PlayerClan).ToString());
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00028E14 File Offset: 0x00027014
		private TextObject CalculateTradeAgreementSupport(IFaction faction)
		{
			TradeAgreementDecision tradeAgreementDecision = new TradeAgreementDecision(Clan.PlayerClan, faction as Kingdom);
			return GameTexts.FindText("str_decision_outcome_support_status", KingdomElection.GetElectionOutcomeSupport(tradeAgreementDecision, Clan.PlayerClan).ToString());
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x0600090D RID: 2317 RVA: 0x00028E55 File Offset: 0x00027055
		// (set) Token: 0x0600090E RID: 2318 RVA: 0x00028E5D File Offset: 0x0002705D
		[DataSourceProperty]
		public MBBindingList<KingdomWarItemVM> PlayerWars
		{
			get
			{
				return this._playerWars;
			}
			set
			{
				if (value != this._playerWars)
				{
					this._playerWars = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomWarItemVM>>(value, "PlayerWars");
				}
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x00028E7B File Offset: 0x0002707B
		// (set) Token: 0x06000910 RID: 2320 RVA: 0x00028E83 File Offset: 0x00027083
		[DataSourceProperty]
		public bool IsDisplayingWarLogs
		{
			get
			{
				return this._isDisplayingWarLogs;
			}
			set
			{
				if (value != this._isDisplayingWarLogs)
				{
					this._isDisplayingWarLogs = value;
					base.OnPropertyChangedWithValue(value, "IsDisplayingWarLogs");
				}
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000911 RID: 2321 RVA: 0x00028EA1 File Offset: 0x000270A1
		// (set) Token: 0x06000912 RID: 2322 RVA: 0x00028EA9 File Offset: 0x000270A9
		[DataSourceProperty]
		public bool IsDisplayingStatComparisons
		{
			get
			{
				return this._isDisplayingStatComparisons;
			}
			set
			{
				if (value != this._isDisplayingStatComparisons)
				{
					this._isDisplayingStatComparisons = value;
					base.OnPropertyChangedWithValue(value, "IsDisplayingStatComparisons");
				}
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000913 RID: 2323 RVA: 0x00028EC7 File Offset: 0x000270C7
		// (set) Token: 0x06000914 RID: 2324 RVA: 0x00028ECF File Offset: 0x000270CF
		[DataSourceProperty]
		public bool IsWar
		{
			get
			{
				return this._isWar;
			}
			set
			{
				if (value != this._isWar)
				{
					this._isWar = value;
					if (!value)
					{
						this.ExecuteShowStatComparisons();
					}
					base.OnPropertyChangedWithValue(value, "IsWar");
				}
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x00028EF6 File Offset: 0x000270F6
		// (set) Token: 0x06000916 RID: 2326 RVA: 0x00028EFE File Offset: 0x000270FE
		[DataSourceProperty]
		public string BehaviorSelectionTitle
		{
			get
			{
				return this._behaviorSelectionTitle;
			}
			set
			{
				if (value != this._behaviorSelectionTitle)
				{
					this._behaviorSelectionTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "BehaviorSelectionTitle");
				}
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000917 RID: 2327 RVA: 0x00028F21 File Offset: 0x00027121
		// (set) Token: 0x06000918 RID: 2328 RVA: 0x00028F29 File Offset: 0x00027129
		[DataSourceProperty]
		public MBBindingList<KingdomTruceItemVM> PlayerTruces
		{
			get
			{
				return this._playerTruces;
			}
			set
			{
				if (value != this._playerTruces)
				{
					this._playerTruces = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomTruceItemVM>>(value, "PlayerTruces");
				}
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000919 RID: 2329 RVA: 0x00028F47 File Offset: 0x00027147
		// (set) Token: 0x0600091A RID: 2330 RVA: 0x00028F4F File Offset: 0x0002714F
		[DataSourceProperty]
		public KingdomDiplomacyItemVM CurrentSelectedDiplomacyItem
		{
			get
			{
				return this._currentSelectedItem;
			}
			set
			{
				if (value != this._currentSelectedItem)
				{
					this._isChangingDiplomacyItem = true;
					this._currentSelectedItem = value;
					this.IsWar = value is KingdomWarItemVM;
					base.OnPropertyChangedWithValue<KingdomDiplomacyItemVM>(value, "CurrentSelectedDiplomacyItem");
					this._isChangingDiplomacyItem = false;
				}
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x00028F8A File Offset: 0x0002718A
		// (set) Token: 0x0600091C RID: 2332 RVA: 0x00028F92 File Offset: 0x00027192
		[DataSourceProperty]
		public KingdomWarSortControllerVM WarsSortController
		{
			get
			{
				return this._warsSortController;
			}
			set
			{
				if (value != this._warsSortController)
				{
					this._warsSortController = value;
					base.OnPropertyChangedWithValue<KingdomWarSortControllerVM>(value, "WarsSortController");
				}
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x00028FB0 File Offset: 0x000271B0
		// (set) Token: 0x0600091E RID: 2334 RVA: 0x00028FB8 File Offset: 0x000271B8
		[DataSourceProperty]
		public string PlayerWarsText
		{
			get
			{
				return this._playerWarsText;
			}
			set
			{
				if (value != this._playerWarsText)
				{
					this._playerWarsText = value;
					base.OnPropertyChangedWithValue<string>(value, "PlayerWarsText");
				}
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x0600091F RID: 2335 RVA: 0x00028FDB File Offset: 0x000271DB
		// (set) Token: 0x06000920 RID: 2336 RVA: 0x00028FE3 File Offset: 0x000271E3
		[DataSourceProperty]
		public string WarsText
		{
			get
			{
				return this._warsText;
			}
			set
			{
				if (value != this._warsText)
				{
					this._warsText = value;
					base.OnPropertyChangedWithValue<string>(value, "WarsText");
				}
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000921 RID: 2337 RVA: 0x00029006 File Offset: 0x00027206
		// (set) Token: 0x06000922 RID: 2338 RVA: 0x0002900E File Offset: 0x0002720E
		[DataSourceProperty]
		public string NumOfPlayerWarsText
		{
			get
			{
				return this._numOfPlayerWarsText;
			}
			set
			{
				if (value != this._numOfPlayerWarsText)
				{
					this._numOfPlayerWarsText = value;
					base.OnPropertyChangedWithValue<string>(value, "NumOfPlayerWarsText");
				}
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x00029031 File Offset: 0x00027231
		// (set) Token: 0x06000924 RID: 2340 RVA: 0x00029039 File Offset: 0x00027239
		[DataSourceProperty]
		public string PlayerTrucesText
		{
			get
			{
				return this._otherWarsText;
			}
			set
			{
				if (value != this._otherWarsText)
				{
					this._otherWarsText = value;
					base.OnPropertyChangedWithValue<string>(value, "PlayerTrucesText");
				}
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x0002905C File Offset: 0x0002725C
		// (set) Token: 0x06000926 RID: 2342 RVA: 0x00029064 File Offset: 0x00027264
		[DataSourceProperty]
		public string NumOfPlayerTrucesText
		{
			get
			{
				return this._numOfOtherWarsText;
			}
			set
			{
				if (value != this._numOfOtherWarsText)
				{
					this._numOfOtherWarsText = value;
					base.OnPropertyChangedWithValue<string>(value, "NumOfPlayerTrucesText");
				}
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x00029087 File Offset: 0x00027287
		// (set) Token: 0x06000928 RID: 2344 RVA: 0x0002908F File Offset: 0x0002728F
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> BehaviorSelection
		{
			get
			{
				return this._behaviorSelection;
			}
			set
			{
				if (value != this._behaviorSelection)
				{
					this._behaviorSelection = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "BehaviorSelection");
				}
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x000290AD File Offset: 0x000272AD
		// (set) Token: 0x0600092A RID: 2346 RVA: 0x000290B5 File Offset: 0x000272B5
		[DataSourceProperty]
		public HintViewModel ShowStatBarsHint
		{
			get
			{
				return this._showStatBarsHint;
			}
			set
			{
				if (value != this._showStatBarsHint)
				{
					this._showStatBarsHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ShowStatBarsHint");
				}
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x000290D3 File Offset: 0x000272D3
		// (set) Token: 0x0600092C RID: 2348 RVA: 0x000290DB File Offset: 0x000272DB
		[DataSourceProperty]
		public HintViewModel ShowWarLogsHint
		{
			get
			{
				return this._showWarLogsHint;
			}
			set
			{
				if (value != this._showWarLogsHint)
				{
					this._showWarLogsHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ShowWarLogsHint");
				}
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x000290F9 File Offset: 0x000272F9
		// (set) Token: 0x0600092E RID: 2350 RVA: 0x00029101 File Offset: 0x00027301
		[DataSourceProperty]
		public MBBindingList<KingdomDiplomacyProposalActionItemVM> Actions
		{
			get
			{
				return this._actions;
			}
			set
			{
				if (value != this._actions)
				{
					this._actions = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomDiplomacyProposalActionItemVM>>(value, "Actions");
				}
			}
		}

		// Token: 0x040003F4 RID: 1012
		private readonly Action<KingdomDecision> _forceDecision;

		// Token: 0x040003F5 RID: 1013
		private readonly Kingdom _playerKingdom;

		// Token: 0x040003F6 RID: 1014
		private bool _isChangingDiplomacyItem;

		// Token: 0x040003F7 RID: 1015
		private MBBindingList<KingdomWarItemVM> _playerWars;

		// Token: 0x040003F8 RID: 1016
		private MBBindingList<KingdomTruceItemVM> _playerTruces;

		// Token: 0x040003F9 RID: 1017
		private KingdomWarSortControllerVM _warsSortController;

		// Token: 0x040003FA RID: 1018
		private KingdomDiplomacyItemVM _currentSelectedItem;

		// Token: 0x040003FB RID: 1019
		private SelectorVM<SelectorItemVM> _behaviorSelection;

		// Token: 0x040003FC RID: 1020
		private HintViewModel _showStatBarsHint;

		// Token: 0x040003FD RID: 1021
		private HintViewModel _showWarLogsHint;

		// Token: 0x040003FE RID: 1022
		private string _playerWarsText;

		// Token: 0x040003FF RID: 1023
		private string _numOfPlayerWarsText;

		// Token: 0x04000400 RID: 1024
		private string _otherWarsText;

		// Token: 0x04000401 RID: 1025
		private string _numOfOtherWarsText;

		// Token: 0x04000402 RID: 1026
		private string _warsText;

		// Token: 0x04000403 RID: 1027
		private string _behaviorSelectionTitle;

		// Token: 0x04000404 RID: 1028
		private bool _isDisplayingWarLogs;

		// Token: 0x04000405 RID: 1029
		private bool _isDisplayingStatComparisons;

		// Token: 0x04000406 RID: 1030
		private bool _isWar;

		// Token: 0x04000407 RID: 1031
		private MBBindingList<KingdomDiplomacyProposalActionItemVM> _actions;

		// Token: 0x020001D2 RID: 466
		private enum DiplomacyItemType
		{
			// Token: 0x0400110F RID: 4367
			None,
			// Token: 0x04001110 RID: 4368
			War,
			// Token: 0x04001111 RID: 4369
			Peace,
			// Token: 0x04001112 RID: 4370
			Alliance,
			// Token: 0x04001113 RID: 4371
			TradeAgreement
		}
	}
}
