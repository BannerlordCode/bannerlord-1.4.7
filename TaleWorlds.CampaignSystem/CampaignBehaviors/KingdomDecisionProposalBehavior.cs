using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000414 RID: 1044
	public class KingdomDecisionProposalBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x06004197 RID: 16791 RVA: 0x00133799 File Offset: 0x00131999
		public ITradeAgreementsCampaignBehavior TradeAgreementsCampaignBehavior
		{
			get
			{
				if (this._tradeAgreementsBehavior == null)
				{
					this._tradeAgreementsBehavior = Campaign.Current.GetCampaignBehavior<ITradeAgreementsCampaignBehavior>();
				}
				return this._tradeAgreementsBehavior;
			}
		}

		// Token: 0x17000E3D RID: 3645
		// (get) Token: 0x06004198 RID: 16792 RVA: 0x001337B9 File Offset: 0x001319B9
		private IAllianceCampaignBehavior AllianceCampaignBehavior
		{
			get
			{
				if (this._allianceCampaignBehavior == null)
				{
					this._allianceCampaignBehavior = Campaign.Current.GetCampaignBehavior<IAllianceCampaignBehavior>();
				}
				return this._allianceCampaignBehavior;
			}
		}

		// Token: 0x06004199 RID: 16793 RVA: 0x001337DC File Offset: 0x001319DC
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickClanEvent.AddNonSerializedListener(this, new Action<Clan>(this.DailyTickClan));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTick));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnPeaceMade));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomDestroyed));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.KingdomDecisionAdded.AddNonSerializedListener(this, new Action<KingdomDecision, bool>(this.OnKingdomDecisionAdded));
		}

		// Token: 0x0600419A RID: 16794 RVA: 0x001338A1 File Offset: 0x00131AA1
		private void OnKingdomDestroyed(Kingdom kingdom)
		{
			this.UpdateKingdomDecisions(kingdom);
		}

		// Token: 0x0600419B RID: 16795 RVA: 0x001338AC File Offset: 0x00131AAC
		private void DailyTickClan(Clan clan)
		{
			if ((float)((int)Campaign.Current.Models.CampaignTimeModel.CampaignStartTime.ElapsedDaysUntilNow) < 5f)
			{
				return;
			}
			if (clan.IsEliminated)
			{
				return;
			}
			if (clan == Clan.PlayerClan || clan.CurrentTotalStrength <= 0f)
			{
				return;
			}
			if (clan.IsBanditFaction)
			{
				return;
			}
			if (clan.Kingdom == null)
			{
				return;
			}
			if (clan.Influence < 100f)
			{
				return;
			}
			KingdomDecision kingdomDecision = null;
			float randomFloat = MBRandom.RandomFloat;
			int num = ((Kingdom)clan.MapFaction).Clans.Count<Clan>((Clan x) => x.Influence > 100f);
			float num2 = MathF.Min(0.33f, 1f / ((float)num + 2f));
			num2 *= ((clan.Kingdom == Hero.MainHero.MapFaction && !Hero.MainHero.Clan.IsUnderMercenaryService) ? ((clan.Kingdom.Leader == Hero.MainHero) ? 0.5f : 0.75f) : 1f);
			DiplomacyModel diplomacyModel = Campaign.Current.Models.DiplomacyModel;
			AllianceModel allianceModel = Campaign.Current.Models.AllianceModel;
			if (randomFloat < num2 && clan.Influence > (float)diplomacyModel.GetInfluenceCostOfProposingPeace(clan))
			{
				kingdomDecision = this.GetRandomPeaceDecision(clan);
			}
			else if (randomFloat < num2 * 2f && clan.Influence > (float)diplomacyModel.GetInfluenceCostOfProposingWar(clan))
			{
				kingdomDecision = this.GetRandomWarDecision(clan);
			}
			else if (randomFloat < num2 * 2.5f)
			{
				kingdomDecision = ((MBRandom.RandomFloat < 0.5f) ? this.GetRandomTradeAgreementDecision(clan) : this.GetRandomStartingAllianceDecision(clan));
			}
			else if (randomFloat < num2 * 2.75f && clan.Influence > (float)(diplomacyModel.GetInfluenceCostOfPolicyProposalAndDisavowal(clan) * 4))
			{
				kingdomDecision = this.GetRandomPolicyDecision(clan);
			}
			else if (randomFloat < num2 * 3f && clan.Influence > 700f)
			{
				kingdomDecision = this.GetRandomAnnexationDecision(clan);
			}
			if (kingdomDecision != null)
			{
				bool flag = false;
				if (kingdomDecision is MakePeaceKingdomDecision && ((MakePeaceKingdomDecision)kingdomDecision).FactionToMakePeaceWith == Hero.MainHero.MapFaction)
				{
					foreach (KingdomDecision kingdomDecision2 in this._kingdomDecisionsList)
					{
						if (kingdomDecision2 is MakePeaceKingdomDecision && kingdomDecision2.Kingdom == Hero.MainHero.MapFaction && ((MakePeaceKingdomDecision)kingdomDecision2).FactionToMakePeaceWith == clan.Kingdom && kingdomDecision2.TriggerTime.IsFuture)
						{
							flag = true;
							break;
						}
						if (kingdomDecision2 is MakePeaceKingdomDecision && kingdomDecision2.Kingdom == clan.Kingdom && ((MakePeaceKingdomDecision)kingdomDecision2).FactionToMakePeaceWith == Hero.MainHero.MapFaction && kingdomDecision2.TriggerTime.IsFuture)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					bool flag2 = false;
					foreach (KingdomDecision kingdomDecision3 in this._kingdomDecisionsList)
					{
						DeclareWarDecision declareWarDecision;
						DeclareWarDecision declareWarDecision2;
						if ((declareWarDecision = kingdomDecision3 as DeclareWarDecision) != null && (declareWarDecision2 = kingdomDecision as DeclareWarDecision) != null && declareWarDecision.FactionToDeclareWarOn == declareWarDecision2.FactionToDeclareWarOn && declareWarDecision.ProposerClan.MapFaction == declareWarDecision2.ProposerClan.MapFaction)
						{
							flag2 = true;
							break;
						}
						MakePeaceKingdomDecision makePeaceKingdomDecision;
						MakePeaceKingdomDecision makePeaceKingdomDecision2;
						if ((makePeaceKingdomDecision = kingdomDecision3 as MakePeaceKingdomDecision) != null && (makePeaceKingdomDecision2 = kingdomDecision as MakePeaceKingdomDecision) != null && makePeaceKingdomDecision.FactionToMakePeaceWith == makePeaceKingdomDecision2.FactionToMakePeaceWith && makePeaceKingdomDecision.ProposerClan.MapFaction == makePeaceKingdomDecision2.ProposerClan.MapFaction)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						clan.Kingdom.AddDecision(kingdomDecision, false);
						return;
					}
				}
			}
			else
			{
				this.UpdateKingdomDecisions(clan.Kingdom);
			}
		}

		// Token: 0x0600419C RID: 16796 RVA: 0x00133C98 File Offset: 0x00131E98
		private void HourlyTick()
		{
			if (Clan.PlayerClan.Kingdom != null)
			{
				this.UpdateKingdomDecisions(Clan.PlayerClan.Kingdom);
			}
		}

		// Token: 0x0600419D RID: 16797 RVA: 0x00133CB8 File Offset: 0x00131EB8
		private void DailyTick()
		{
			for (int i = this._kingdomDecisionsList.Count - 1; i >= 0; i--)
			{
				if (this._kingdomDecisionsList[i].TriggerTime.ElapsedDaysUntilNow > 5f)
				{
					this._kingdomDecisionsList.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600419E RID: 16798 RVA: 0x00133D0C File Offset: 0x00131F0C
		public void UpdateKingdomDecisions(Kingdom kingdom)
		{
			List<KingdomDecision> list = new List<KingdomDecision>();
			List<KingdomDecision> list2 = new List<KingdomDecision>();
			foreach (KingdomDecision kingdomDecision in kingdom.UnresolvedDecisions)
			{
				if (kingdomDecision.ShouldBeCancelled())
				{
					list.Add(kingdomDecision);
				}
				else if (!kingdomDecision.IsPlayerParticipant || (kingdomDecision.TriggerTime.IsPast && !kingdomDecision.NeedsPlayerResolution))
				{
					list2.Add(kingdomDecision);
				}
			}
			foreach (KingdomDecision kingdomDecision2 in list)
			{
				kingdom.RemoveDecision(kingdomDecision2);
				bool flag;
				if (!kingdomDecision2.DetermineChooser().Leader.IsHumanPlayerCharacter)
				{
					flag = kingdomDecision2.DetermineSupporters().Any<Supporter>((Supporter x) => x.IsPlayer);
				}
				else
				{
					flag = true;
				}
				bool flag2 = flag;
				CampaignEventDispatcher.Instance.OnKingdomDecisionCancelled(kingdomDecision2, flag2);
			}
			foreach (KingdomDecision kingdomDecision3 in list2)
			{
				new KingdomElection(kingdomDecision3).StartElectionWithoutPlayer();
			}
		}

		// Token: 0x0600419F RID: 16799 RVA: 0x00133E6C File Offset: 0x0013206C
		private void OnPeaceMade(IFaction side1Faction, IFaction side2Faction, MakePeaceAction.MakePeaceDetail detail)
		{
			this.HandleDiplomaticChangeBetweenFactions(side1Faction, side2Faction);
		}

		// Token: 0x060041A0 RID: 16800 RVA: 0x00133E76 File Offset: 0x00132076
		private void OnWarDeclared(IFaction side1Faction, IFaction side2Faction, DeclareWarAction.DeclareWarDetail detail)
		{
			this.HandleDiplomaticChangeBetweenFactions(side1Faction, side2Faction);
		}

		// Token: 0x060041A1 RID: 16801 RVA: 0x00133E80 File Offset: 0x00132080
		private void HandleDiplomaticChangeBetweenFactions(IFaction side1Faction, IFaction side2Faction)
		{
			if (side1Faction.IsKingdomFaction && side2Faction.IsKingdomFaction)
			{
				this.UpdateKingdomDecisions((Kingdom)side1Faction);
				this.UpdateKingdomDecisions((Kingdom)side2Faction);
			}
		}

		// Token: 0x060041A2 RID: 16802 RVA: 0x00133EAC File Offset: 0x001320AC
		private KingdomDecision GetRandomStartingAllianceDecision(Clan clan)
		{
			Kingdom kingdom = clan.Kingdom;
			KingdomDecision kingdomDecision = null;
			if (kingdom.UnresolvedDecisions.AnyQ<KingdomDecision>((KingdomDecision x) => x is StartAllianceDecision) || clan.Influence < (float)Campaign.Current.Models.AllianceModel.GetInfluenceCostOfProposingStartingAlliance(clan))
			{
				return null;
			}
			Kingdom randomElementWithPredicate = Kingdom.All.GetRandomElementWithPredicate<Kingdom>((Kingdom x) => !x.IsEliminated && x != kingdom);
			if (randomElementWithPredicate != null)
			{
				kingdomDecision = new StartAllianceDecision(clan, randomElementWithPredicate);
				TextObject textObject;
				if (!kingdomDecision.CanMakeDecision(out textObject, false))
				{
					kingdomDecision = null;
				}
			}
			return kingdomDecision;
		}

		// Token: 0x060041A3 RID: 16803 RVA: 0x00133F50 File Offset: 0x00132150
		private KingdomDecision GetRandomWarDecision(Clan clan)
		{
			KingdomDecision kingdomDecision = null;
			Kingdom kingdom = clan.Kingdom;
			if (kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>((KingdomDecision x) => x is DeclareWarDecision) != null)
			{
				return null;
			}
			Kingdom randomElementWithPredicate = Kingdom.All.GetRandomElementWithPredicate<Kingdom>((Kingdom x) => !x.IsEliminated && x != kingdom && !x.IsAtWarWith(kingdom) && x.GetStanceWith(kingdom).PeaceDeclarationDate.ElapsedDaysUntilNow > 20f);
			if (randomElementWithPredicate != null)
			{
				if ((float)new DeclareWarBarterable(kingdom, randomElementWithPredicate).GetValueForFaction(clan) < Campaign.Current.Models.DiplomacyModel.GetDecisionMakingThreshold(randomElementWithPredicate))
				{
					return null;
				}
				if (this.ConsiderWar(clan, kingdom, randomElementWithPredicate))
				{
					kingdomDecision = new DeclareWarDecision(clan, randomElementWithPredicate);
				}
			}
			return kingdomDecision;
		}

		// Token: 0x060041A4 RID: 16804 RVA: 0x00134004 File Offset: 0x00132204
		private KingdomDecision GetRandomPeaceDecision(Clan clan)
		{
			KingdomDecision kingdomDecision = null;
			Kingdom kingdom = clan.Kingdom;
			if (kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>((KingdomDecision x) => x is MakePeaceKingdomDecision) != null)
			{
				return null;
			}
			Kingdom randomElementWithPredicate = Kingdom.All.GetRandomElementWithPredicate<Kingdom>(delegate(Kingdom x)
			{
				if (x.IsAtWarWith(kingdom) && !x.IsAtConstantWarWith(kingdom))
				{
					IAllianceCampaignBehavior allianceCampaignBehavior = this.AllianceCampaignBehavior;
					Kingdom kingdom2;
					if (allianceCampaignBehavior == null || !allianceCampaignBehavior.IsAtWarByCallToWarAgreement(kingdom, x, out kingdom2))
					{
						IAllianceCampaignBehavior allianceCampaignBehavior2 = this.AllianceCampaignBehavior;
						return allianceCampaignBehavior2 == null || !allianceCampaignBehavior2.IsAtWarByCallToWarAgreement(x, kingdom, out kingdom2);
					}
				}
				return false;
			});
			MakePeaceKingdomDecision makePeaceKingdomDecision;
			if (randomElementWithPredicate != null && KingdomDecisionProposalBehavior.ConsiderPeace(clan, randomElementWithPredicate.RulingClan, randomElementWithPredicate, out makePeaceKingdomDecision))
			{
				kingdomDecision = makePeaceKingdomDecision;
			}
			return kingdomDecision;
		}

		// Token: 0x060041A5 RID: 16805 RVA: 0x00134090 File Offset: 0x00132290
		private bool ConsiderWar(Clan clan, Kingdom kingdom, IFaction otherFaction)
		{
			int num = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfProposingWar(clan) / 2;
			if (clan.Influence < (float)num)
			{
				return false;
			}
			DeclareWarDecision declareWarDecision = new DeclareWarDecision(clan, otherFaction);
			if (declareWarDecision.CalculateSupport(clan) > 50f)
			{
				KingdomElection kingdomElection = new KingdomElection(declareWarDecision);
				float num2 = 0f;
				using (List<DecisionOutcome>.Enumerator enumerator = kingdomElection.PossibleOutcomes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DeclareWarDecision.DeclareWarDecisionOutcome declareWarDecisionOutcome;
						if ((declareWarDecisionOutcome = enumerator.Current as DeclareWarDecision.DeclareWarDecisionOutcome) != null && declareWarDecisionOutcome.ShouldWarBeDeclared)
						{
							num2 = declareWarDecisionOutcome.Likelihood;
							break;
						}
					}
				}
				if (MBRandom.RandomFloat < 1.4f * num2 - 0.55f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060041A6 RID: 16806 RVA: 0x00134154 File Offset: 0x00132354
		private static bool ConsiderPeace(Clan clan, Clan otherClan, IFaction otherFaction, out MakePeaceKingdomDecision decision)
		{
			if (!Campaign.Current.Models.DiplomacyModel.IsPeaceSuitable(clan.MapFaction, otherFaction))
			{
				decision = null;
				return false;
			}
			if (Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringPeace(clan.MapFaction, otherFaction) < Campaign.Current.Models.DiplomacyModel.GetDecisionMakingThreshold(clan.Kingdom))
			{
				decision = null;
				return false;
			}
			int num;
			int dailyTributeToPay = Campaign.Current.Models.DiplomacyModel.GetDailyTributeToPay(clan, otherClan, out num);
			if (dailyTributeToPay < 0)
			{
				decision = null;
				return false;
			}
			MakePeaceKingdomDecision makePeaceKingdomDecision = new MakePeaceKingdomDecision(clan, otherFaction, dailyTributeToPay, num, true, false);
			DecisionOutcome decisionOutcome = makePeaceKingdomDecision.DetermineInitialCandidates().First<DecisionOutcome>(delegate(DecisionOutcome x)
			{
				MakePeaceKingdomDecision.MakePeaceDecisionOutcome makePeaceDecisionOutcome;
				return (makePeaceDecisionOutcome = x as MakePeaceKingdomDecision.MakePeaceDecisionOutcome) != null && makePeaceDecisionOutcome.ShouldPeaceBeDeclared;
			});
			if (makePeaceKingdomDecision.DetermineSupport(clan, decisionOutcome) <= 0f)
			{
				decision = null;
				return false;
			}
			decision = makePeaceKingdomDecision;
			return true;
		}

		// Token: 0x060041A7 RID: 16807 RVA: 0x00134230 File Offset: 0x00132430
		private KingdomDecision GetRandomPolicyDecision(Clan clan)
		{
			KingdomDecision kingdomDecision = null;
			Kingdom kingdom = clan.Kingdom;
			if (kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>((KingdomDecision x) => x is KingdomPolicyDecision) != null)
			{
				return null;
			}
			if (clan.Influence < 200f)
			{
				return null;
			}
			PolicyObject randomElement = PolicyObject.All.GetRandomElement<PolicyObject>();
			bool flag = kingdom.ActivePolicies.Contains(randomElement);
			if (this.ConsiderPolicy(clan, kingdom, randomElement, flag))
			{
				kingdomDecision = new KingdomPolicyDecision(clan, randomElement, flag);
			}
			return kingdomDecision;
		}

		// Token: 0x060041A8 RID: 16808 RVA: 0x001342B4 File Offset: 0x001324B4
		private bool ConsiderPolicy(Clan clan, Kingdom kingdom, PolicyObject policy, bool invert)
		{
			int influenceCostOfPolicyProposalAndDisavowal = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfPolicyProposalAndDisavowal(clan);
			if (clan.Influence < (float)influenceCostOfPolicyProposalAndDisavowal)
			{
				return false;
			}
			KingdomPolicyDecision kingdomPolicyDecision = new KingdomPolicyDecision(clan, policy, invert);
			if (kingdomPolicyDecision.CalculateSupport(clan) > 50f)
			{
				KingdomElection kingdomElection = new KingdomElection(kingdomPolicyDecision);
				float num = 0f;
				using (List<DecisionOutcome>.Enumerator enumerator = kingdomElection.PossibleOutcomes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KingdomPolicyDecision.PolicyDecisionOutcome policyDecisionOutcome;
						if ((policyDecisionOutcome = enumerator.Current as KingdomPolicyDecision.PolicyDecisionOutcome) != null && policyDecisionOutcome.ShouldDecisionBeEnforced)
						{
							num = policyDecisionOutcome.Likelihood;
							break;
						}
					}
				}
				if ((double)MBRandom.RandomFloat < (double)num - 0.55)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060041A9 RID: 16809 RVA: 0x00134378 File Offset: 0x00132578
		private float GetKingdomSupportForPolicy(Clan clan, Kingdom kingdom, PolicyObject policy, bool invert)
		{
			Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfPolicyProposalAndDisavowal(clan);
			return new KingdomElection(new KingdomPolicyDecision(clan, policy, invert)).GetLikelihoodForSponsor(clan);
		}

		// Token: 0x060041AA RID: 16810 RVA: 0x001343A4 File Offset: 0x001325A4
		private KingdomDecision GetRandomAnnexationDecision(Clan clan)
		{
			KingdomDecision kingdomDecision = null;
			Kingdom kingdom = clan.Kingdom;
			if (kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>((KingdomDecision x) => x is KingdomPolicyDecision) != null)
			{
				return null;
			}
			if (clan.Influence < 300f)
			{
				return null;
			}
			Clan randomElement = kingdom.Clans.GetRandomElement<Clan>();
			if (randomElement != null && randomElement != clan && randomElement.GetRelationWithClan(clan) < -25)
			{
				if (randomElement.Fiefs.Count == 0)
				{
					return null;
				}
				Town randomElement2 = randomElement.Fiefs.GetRandomElement<Town>();
				if (this.ConsiderAnnex(clan, randomElement2))
				{
					kingdomDecision = new SettlementClaimantPreliminaryDecision(clan, randomElement2.Settlement);
				}
			}
			return kingdomDecision;
		}

		// Token: 0x060041AB RID: 16811 RVA: 0x00134448 File Offset: 0x00132648
		private bool ConsiderAnnex(Clan clan, Town targetSettlement)
		{
			int influenceCostOfAnnexation = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfAnnexation(clan);
			if (clan.Influence < (float)influenceCostOfAnnexation)
			{
				return false;
			}
			SettlementClaimantPreliminaryDecision settlementClaimantPreliminaryDecision = new SettlementClaimantPreliminaryDecision(clan, targetSettlement.Settlement);
			if (settlementClaimantPreliminaryDecision.CalculateSupport(clan) > 50f)
			{
				float num = 0f;
				using (List<DecisionOutcome>.Enumerator enumerator = new KingdomElection(settlementClaimantPreliminaryDecision).PossibleOutcomes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SettlementClaimantPreliminaryDecision.SettlementClaimantPreliminaryOutcome settlementClaimantPreliminaryOutcome;
						if ((settlementClaimantPreliminaryOutcome = enumerator.Current as SettlementClaimantPreliminaryDecision.SettlementClaimantPreliminaryOutcome) != null && settlementClaimantPreliminaryOutcome.ShouldSettlementOwnerChange)
						{
							num = settlementClaimantPreliminaryOutcome.Likelihood;
							break;
						}
					}
				}
				if ((double)MBRandom.RandomFloat < (double)num - 0.6)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060041AC RID: 16812 RVA: 0x00134510 File Offset: 0x00132710
		private KingdomDecision GetRandomTradeAgreementDecision(Clan clan)
		{
			KingdomDecision kingdomDecision = null;
			Kingdom kingdom = clan.Kingdom;
			if (kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>((KingdomDecision x) => x is TradeAgreementDecision) != null || clan.Influence < (float)Campaign.Current.Models.TradeAgreementModel.GetInfluenceCostOfProposingTradeAgreement(clan))
			{
				return null;
			}
			Kingdom randomElementWithPredicate = Kingdom.All.GetRandomElementWithPredicate<Kingdom>((Kingdom x) => x != kingdom);
			if (randomElementWithPredicate != null && this.ConsiderTradeAgreement(clan, kingdom, randomElementWithPredicate))
			{
				kingdomDecision = new TradeAgreementDecision(clan, randomElementWithPredicate);
			}
			return kingdomDecision;
		}

		// Token: 0x060041AD RID: 16813 RVA: 0x001345B8 File Offset: 0x001327B8
		private bool ConsiderTradeAgreement(Clan clan, Kingdom kingdom, Kingdom otherKingdom)
		{
			TextObject textObject;
			if (!Campaign.Current.Models.TradeAgreementModel.CanMakeTradeAgreement(kingdom, otherKingdom, true, out textObject, false))
			{
				return false;
			}
			TradeAgreementDecision tradeAgreementDecision = new TradeAgreementDecision(clan, otherKingdom);
			if (kingdom != Clan.PlayerClan.Kingdom)
			{
				return tradeAgreementDecision.CalculateSupport(clan, out textObject) > 50f;
			}
			KingdomElection kingdomElection = new KingdomElection(tradeAgreementDecision);
			kingdomElection.SetupResultWithoutPlayerSupport();
			DecisionOutcome decisionOutcome = kingdomElection.PossibleOutcomes.FirstOrDefault<DecisionOutcome>(delegate(DecisionOutcome x)
			{
				TradeAgreementDecision.TradeAgreementDecisionOutcome tradeAgreementDecisionOutcome;
				return (tradeAgreementDecisionOutcome = x as TradeAgreementDecision.TradeAgreementDecisionOutcome) != null && tradeAgreementDecisionOutcome.ShouldTradeAgreementStart;
			});
			return kingdomElection.GetWinChanceWithPlayerSupport(decisionOutcome, Supporter.SupportWeights.FullyPush) > 0.5f;
		}

		// Token: 0x060041AE RID: 16814 RVA: 0x00134650 File Offset: 0x00132850
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<KingdomDecision>>("_kingdomDecisionsList", ref this._kingdomDecisionsList);
			if (dataStore.IsLoading && MBSaveLoad.LastLoadedGameVersion.IsOlderThan(ApplicationVersion.FromString("v1.3.0", 0)) && this._kingdomDecisionsList == null)
			{
				this._kingdomDecisionsList = new List<KingdomDecision>();
			}
		}

		// Token: 0x060041AF RID: 16815 RVA: 0x001346A4 File Offset: 0x001328A4
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (clan == Clan.PlayerClan && oldKingdom != null && detail != ChangeKingdomAction.ChangeKingdomActionDetail.LeaveByKingdomDestruction)
			{
				this.UpdateKingdomDecisions(oldKingdom);
			}
		}

		// Token: 0x060041B0 RID: 16816 RVA: 0x001346BD File Offset: 0x001328BD
		private void OnKingdomDecisionAdded(KingdomDecision decision, bool isPlayerInvolved)
		{
			this._kingdomDecisionsList.Add(decision);
		}

		// Token: 0x04001315 RID: 4885
		private const float DaysBetweenSameProposal = 5f;

		// Token: 0x04001316 RID: 4886
		private List<KingdomDecision> _kingdomDecisionsList = new List<KingdomDecision>();

		// Token: 0x04001317 RID: 4887
		private ITradeAgreementsCampaignBehavior _tradeAgreementsBehavior;

		// Token: 0x04001318 RID: 4888
		private IAllianceCampaignBehavior _allianceCampaignBehavior;

		// Token: 0x02000819 RID: 2073
		// (Invoke) Token: 0x0600667D RID: 26237
		private delegate KingdomDecision KingdomDecisionCreatorDelegate(Clan sponsorClan);
	}
}
