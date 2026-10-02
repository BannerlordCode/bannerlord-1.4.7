using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000103 RID: 259
	public class DefaultClanFinanceModel : ClanFinanceModel
	{
		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x060016ED RID: 5869 RVA: 0x00069BC0 File Offset: 0x00067DC0
		private ITradeAgreementsCampaignBehavior TradeAgreementsBehavior
		{
			get
			{
				if (this._tradeAgreementsCampaignBehavior == null)
				{
					this._tradeAgreementsCampaignBehavior = Campaign.Current.GetCampaignBehavior<ITradeAgreementsCampaignBehavior>();
				}
				return this._tradeAgreementsCampaignBehavior;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x060016EE RID: 5870 RVA: 0x00069BE0 File Offset: 0x00067DE0
		public override int PartyGoldLowerThreshold
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x00069BE8 File Offset: 0x00067DE8
		public override ExplainedNumber CalculateClanGoldChange(Clan clan, bool includeDescriptions = false, bool applyWithdrawals = false, bool includeDetails = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			this.CalculateClanIncomeInternal(clan, ref explainedNumber, applyWithdrawals, includeDetails);
			this.CalculateClanExpensesInternal(clan, ref explainedNumber, applyWithdrawals, includeDetails);
			return explainedNumber;
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x00069C1C File Offset: 0x00067E1C
		public override ExplainedNumber CalculateClanIncome(Clan clan, bool includeDescriptions = false, bool applyWithdrawals = false, bool includeDetails = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			this.CalculateClanIncomeInternal(clan, ref explainedNumber, applyWithdrawals, includeDetails);
			return explainedNumber;
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x00069C44 File Offset: 0x00067E44
		private void CalculateClanIncomeInternal(Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals = false, bool includeDetails = false)
		{
			if (clan.IsEliminated)
			{
				return;
			}
			Kingdom kingdom = clan.Kingdom;
			if (((kingdom != null) ? kingdom.RulingClan : null) == clan)
			{
				this.AddRulingClanIncome(clan, ref goldChange, applyWithdrawals, includeDetails);
			}
			if (clan != Clan.PlayerClan && (!clan.MapFaction.IsKingdomFaction || clan.IsUnderMercenaryService) && clan.Fiefs.Count == 0)
			{
				int num = clan.Tier * (80 + (clan.IsUnderMercenaryService ? 40 : 0));
				goldChange.Add((float)num, null, null);
			}
			this.AddMercenaryIncome(clan, ref goldChange, applyWithdrawals);
			this.AddSettlementIncome(clan, ref goldChange, applyWithdrawals, includeDetails);
			this.CalculateHeroIncomeFromWorkshops(clan.Leader, ref goldChange, applyWithdrawals);
			this.AddIncomeFromParties(clan, ref goldChange, applyWithdrawals, includeDetails);
			if (clan == Clan.PlayerClan)
			{
				this.AddPlayerClanIncomeFromOwnedAlleys(ref goldChange);
			}
			if (!clan.IsUnderMercenaryService)
			{
				this.AddIncomeFromTribute(clan, ref goldChange, applyWithdrawals, includeDetails);
				this.AddIncomeFromCallToWarAgrements(clan, ref goldChange, applyWithdrawals);
				if (clan.Kingdom != null && this.TradeAgreementsBehavior != null)
				{
					this.AddIncomeFromTradeAgreements(clan, ref goldChange, applyWithdrawals, includeDetails);
				}
			}
			if (clan.Gold < 30000 && clan.Kingdom != null && clan.Leader != Hero.MainHero && !clan.IsUnderMercenaryService)
			{
				this.AddIncomeFromKingdomBudget(clan, ref goldChange, applyWithdrawals);
			}
			Hero leader = clan.Leader;
			if (leader != null && leader.GetPerkValue(DefaultPerks.Trade.SpringOfGold))
			{
				int num2 = MathF.Min(1000, MathF.Round((float)clan.Leader.Gold * DefaultPerks.Trade.SpringOfGold.PrimaryBonus));
				goldChange.Add((float)num2, DefaultPerks.Trade.SpringOfGold.Name, null);
			}
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x00069DC0 File Offset: 0x00067FC0
		public void CalculateClanExpensesInternal(Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals = false, bool includeDetails = false)
		{
			this.AddExpensesFromPartiesAndGarrisons(clan, ref goldChange, applyWithdrawals, includeDetails);
			if (!clan.IsUnderMercenaryService)
			{
				this.AddExpensesForHiredMercenaries(clan, ref goldChange, applyWithdrawals);
				this.AddExpensesForTributes(clan, ref goldChange, applyWithdrawals);
			}
			this.AddExpensesForAutoRecruitment(clan, ref goldChange, applyWithdrawals);
			if (clan.Gold > 100000 && clan.Kingdom != null && clan.Leader != Hero.MainHero && !clan.IsUnderMercenaryService)
			{
				int num = (int)(((float)clan.Gold - 100000f) * 0.01f);
				if (applyWithdrawals)
				{
					clan.Kingdom.KingdomBudgetWallet += num;
				}
				goldChange.Add((float)(-(float)num), DefaultClanFinanceModel._kingdomBudgetText, null);
			}
			if (clan.DebtToKingdom > 0)
			{
				this.AddPaymentForDebts(clan, ref goldChange, applyWithdrawals);
			}
			if (Clan.PlayerClan == clan)
			{
				this.AddPlayerExpenseForWorkshops(ref goldChange);
			}
			if (!clan.IsUnderMercenaryService)
			{
				this.AddExpensesForCallToWarAgreements(clan, ref goldChange, applyWithdrawals);
			}
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x00069E94 File Offset: 0x00068094
		private void AddPlayerExpenseForWorkshops(ref ExplainedNumber goldChange)
		{
			int num = 0;
			foreach (Workshop workshop in Hero.MainHero.OwnedWorkshops)
			{
				if (workshop.Capital < Campaign.Current.Models.WorkshopModel.CapitalLowLimit)
				{
					num -= workshop.Expense;
				}
			}
			goldChange.Add((float)num, DefaultClanFinanceModel._shopExpenseText, null);
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x00069F1C File Offset: 0x0006811C
		public override ExplainedNumber CalculateClanExpenses(Clan clan, bool includeDescriptions = false, bool applyWithdrawals = false, bool includeDetails = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescriptions, null);
			this.CalculateClanExpensesInternal(clan, ref explainedNumber, applyWithdrawals, includeDetails);
			return explainedNumber;
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x00069F44 File Offset: 0x00068144
		private void AddPaymentForDebts(Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals)
		{
			if (clan.Kingdom != null && clan.DebtToKingdom > 0)
			{
				int num = clan.DebtToKingdom;
				if (applyWithdrawals)
				{
					num = MathF.Min(num, (int)((float)clan.Gold + goldChange.ResultNumber));
					clan.DebtToKingdom -= num;
				}
				goldChange.Add((float)(-(float)num), DefaultClanFinanceModel._debtText, null);
			}
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x00069FA0 File Offset: 0x000681A0
		private void AddRulingClanIncome(Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals, bool includeDetails)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, goldChange.IncludeDescriptions, null);
			Kingdom kingdom = clan.Kingdom;
			if (kingdom.ActivePolicies.Contains(DefaultPolicies.LandTax))
			{
				float num = 0f;
				foreach (Village village in kingdom.Villages)
				{
					if (!village.IsOwnerUnassigned && village.Settlement.OwnerClan != clan && village.VillageState != Village.VillageStates.Looted && village.VillageState != Village.VillageStates.BeingRaided)
					{
						int num2 = (int)((float)village.TradeTaxAccumulated / this.RevenueSmoothenFraction());
						num += (float)num2 * 0.05f;
					}
				}
				if (num > 1E-05f)
				{
					explainedNumber.Add((float)((int)num), DefaultPolicies.LandTax.Name, null);
				}
			}
			if (kingdom.ActivePolicies.Contains(DefaultPolicies.WarTax))
			{
				float num3 = 0f;
				foreach (Town town in kingdom.Fiefs)
				{
					num3 += Campaign.Current.Models.SettlementTaxModel.CalculateTownTax(town, false).ResultNumber;
				}
				int num4 = (int)(num3 * 0.05f);
				explainedNumber.Add((float)num4, DefaultPolicies.WarTax.Name, null);
			}
			if (kingdom.ActivePolicies.Contains(DefaultPolicies.CrownDuty))
			{
				foreach (Town town2 in kingdom.Fiefs)
				{
					int num5 = (int)((float)town2.TradeTaxAccumulated * 0.05f);
					explainedNumber.Add((float)num5, DefaultPolicies.CrownDuty.Name, null);
					if (applyWithdrawals)
					{
						town2.TradeTaxAccumulated -= num5;
					}
				}
			}
			if (kingdom.ActivePolicies.Contains(DefaultPolicies.DebasementOfTheCurrency))
			{
				int count = kingdom.Fiefs.Count;
				explainedNumber.Add((float)(count * 100), DefaultPolicies.DebasementOfTheCurrency.Name, null);
			}
			int num6 = 0;
			int num7 = 0;
			foreach (Settlement settlement in clan.Settlements)
			{
				if (settlement.IsTown)
				{
					if (kingdom.ActivePolicies.Contains(DefaultPolicies.RoadTolls))
					{
						int num8 = settlement.Town.TradeTaxAccumulated / 30;
						if (applyWithdrawals)
						{
							settlement.Town.TradeTaxAccumulated -= num8;
						}
						num6 += num8;
					}
					if (kingdom.ActivePolicies.Contains(DefaultPolicies.StateMonopolies))
					{
						num7 += (int)((float)settlement.Town.Workshops.Sum<Workshop>((Workshop t) => t.ProfitMade) * 0.05f);
					}
					if (num6 > 0)
					{
						explainedNumber.Add((float)num6, DefaultPolicies.RoadTolls.Name, null);
					}
					if (num7 > 0)
					{
						explainedNumber.Add((float)num7, DefaultPolicies.StateMonopolies.Name, null);
					}
				}
			}
			if (!explainedNumber.ResultNumber.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				if (!includeDetails)
				{
					goldChange.Add(explainedNumber.ResultNumber, GameTexts.FindText("str_policies", null), null);
					return;
				}
				goldChange.AddFromExplainedNumber(explainedNumber, GameTexts.FindText("str_policies", null));
			}
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x0006A344 File Offset: 0x00068544
		private void AddExpensesForHiredMercenaries(Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals)
		{
			Kingdom kingdom = clan.Kingdom;
			if (kingdom != null)
			{
				float num = DefaultClanFinanceModel.CalculateShareFactor(clan);
				if (kingdom.MercenaryWallet < 0)
				{
					int num2 = (int)((float)(-(float)kingdom.MercenaryWallet) * num);
					DefaultClanFinanceModel.ApplyShareForExpenses(clan, ref goldChange, applyWithdrawals, num2, DefaultClanFinanceModel._mercenaryExpensesText);
					if (applyWithdrawals)
					{
						kingdom.MercenaryWallet += num2;
					}
				}
			}
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x0006A398 File Offset: 0x00068598
		private void AddExpensesForTributes(Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals)
		{
			Kingdom kingdom = clan.Kingdom;
			if (kingdom != null)
			{
				float num = DefaultClanFinanceModel.CalculateShareFactor(clan);
				if (kingdom.TributeWallet < 0)
				{
					int num2 = (int)((float)(-(float)kingdom.TributeWallet) * num);
					DefaultClanFinanceModel.ApplyShareForExpenses(clan, ref goldChange, applyWithdrawals, num2, DefaultClanFinanceModel._tributeExpensesText);
					if (applyWithdrawals)
					{
						kingdom.TributeWallet += num2;
					}
				}
			}
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x0006A3EC File Offset: 0x000685EC
		private void AddExpensesForCallToWarAgreements(Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals)
		{
			Kingdom kingdom = clan.Kingdom;
			if (kingdom != null && kingdom.CallToWarWallet < 0)
			{
				float num = DefaultClanFinanceModel.CalculateShareFactor(clan);
				int num2 = (int)((float)(-(float)kingdom.CallToWarWallet) * num);
				int num3 = num2;
				int num4 = (int)((float)clan.Gold + goldChange.ResultNumber);
				if (applyWithdrawals && num4 - num3 < 5000)
				{
					num3 = MathF.Max(0, num4 - 5000);
					clan.DebtToKingdom += num2 - num3;
				}
				DefaultClanFinanceModel.ApplyShareForExpenses(clan, ref goldChange, applyWithdrawals, num3, DefaultClanFinanceModel._callToWarExpenses);
				if (applyWithdrawals)
				{
					kingdom.CallToWarWallet += num2;
				}
			}
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x0006A480 File Offset: 0x00068680
		private static void ApplyShareForExpenses(Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals, int expenseShare, TextObject mercenaryExpensesText)
		{
			if (applyWithdrawals)
			{
				int num = (int)((float)clan.Gold + goldChange.ResultNumber);
				if (expenseShare > num)
				{
					int num2 = expenseShare - num;
					expenseShare = num;
					clan.DebtToKingdom += num2;
				}
			}
			goldChange.Add((float)(-(float)expenseShare), mercenaryExpensesText, null);
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x0006A4C8 File Offset: 0x000686C8
		private void AddSettlementIncome(Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals, bool includeDetails)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, goldChange.IncludeDescriptions, null);
			foreach (Town town in clan.Fiefs)
			{
				ExplainedNumber explainedNumber2 = Campaign.Current.Models.SettlementTaxModel.CalculateTownTax(town, false);
				ExplainedNumber explainedNumber3 = Campaign.Current.Models.ClanFinanceModel.CalculateTownIncomeFromTariffs(clan, town, applyWithdrawals);
				int num = Campaign.Current.Models.ClanFinanceModel.CalculateTownIncomeFromProjects(town);
				if (explainedNumber.IncludeDescriptions)
				{
					explainedNumber.Add((float)((int)explainedNumber2.ResultNumber), Game.Current.GameTextManager.FindText(DefaultClanFinanceModel._townTaxStr, null), town.Name);
					explainedNumber.Add((float)((int)explainedNumber3.ResultNumber), Game.Current.GameTextManager.FindText(DefaultClanFinanceModel._tariffTaxStr, null), town.Name);
					explainedNumber.Add((float)num, DefaultClanFinanceModel._projectsIncomeText, null);
				}
				else
				{
					explainedNumber.Add((float)((int)explainedNumber2.ResultNumber), null, null);
					explainedNumber.Add((float)((int)explainedNumber3.ResultNumber), null, null);
					explainedNumber.Add((float)num, null, null);
				}
				foreach (Village village in town.Villages)
				{
					int num2 = this.CalculateVillageIncome(clan, village, applyWithdrawals);
					explainedNumber.Add((float)num2, village.Name, null);
				}
			}
			if (!includeDetails)
			{
				goldChange.Add(explainedNumber.ResultNumber, DefaultClanFinanceModel._settlementIncome, null);
				return;
			}
			goldChange.AddFromExplainedNumber(explainedNumber, DefaultClanFinanceModel._settlementIncome);
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x0006A6AC File Offset: 0x000688AC
		public override ExplainedNumber CalculateTownIncomeFromTariffs(Clan clan, Town town, bool applyWithdrawals = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber((float)((int)((float)town.TradeTaxAccumulated / this.RevenueSmoothenFraction())), false, null);
			int num = MathF.Round(explainedNumber.ResultNumber);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Trade.ContentTrades, town, ref explainedNumber);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Crossbow.Steady, town, ref explainedNumber);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Roguery.SaltTheEarth, town, ref explainedNumber);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.GivingHands, town, ref explainedNumber);
			this.CalculateSettlementProjectTariffBonuses(town, ref explainedNumber);
			if (applyWithdrawals)
			{
				town.TradeTaxAccumulated -= num;
				if (clan == Clan.PlayerClan)
				{
					CampaignEventDispatcher.Instance.OnPlayerEarnedGoldFromAsset(DefaultClanFinanceModel.AssetIncomeType.Taxes, (int)explainedNumber.ResultNumber);
				}
			}
			return explainedNumber;
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x0006A749 File Offset: 0x00068949
		private void CalculateSettlementProjectTariffBonuses(Town town, ref ExplainedNumber result)
		{
			town.AddEffectOfBuildings(BuildingEffectEnum.TariffIncome, ref result);
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x0006A754 File Offset: 0x00068954
		public override int CalculateTownIncomeFromProjects(Town town)
		{
			ExplainedNumber explainedNumber = default(ExplainedNumber);
			if (town.CurrentDefaultBuilding != null && town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Engineering.ArchitecturalCommisions))
			{
				explainedNumber.Add((float)((int)DefaultPerks.Engineering.ArchitecturalCommisions.SecondaryBonus), null, null);
			}
			town.AddEffectOfBuildings(BuildingEffectEnum.DenarByBoundVillageHeartPerDay, ref explainedNumber);
			return (int)explainedNumber.ResultNumber;
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x0006A7B4 File Offset: 0x000689B4
		public override int CalculateVillageIncome(Clan clan, Village village, bool applyWithdrawals = false)
		{
			int num = ((village.VillageState == Village.VillageStates.Looted || village.VillageState == Village.VillageStates.BeingRaided) ? 0 : ((int)((float)village.TradeTaxAccumulated / this.RevenueSmoothenFraction())));
			int num2 = num;
			if (clan.Kingdom != null && clan.Kingdom.RulingClan != clan && clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.LandTax))
			{
				num -= (int)(0.05f * (float)num);
			}
			if (village.Bound.Town != null && village.Bound.Town.Governor != null && village.Bound.Town.Governor.GetPerkValue(DefaultPerks.Scouting.ForestKin))
			{
				num += MathF.Round((float)num * DefaultPerks.Scouting.ForestKin.SecondaryBonus);
			}
			Settlement bound = village.Bound;
			bool flag;
			if (bound == null)
			{
				flag = null != null;
			}
			else
			{
				Town town = bound.Town;
				flag = ((town != null) ? town.Governor : null) != null;
			}
			if (flag && village.Bound.Town.Governor.GetPerkValue(DefaultPerks.Steward.Logistician))
			{
				num += MathF.Round((float)num * DefaultPerks.Steward.Logistician.SecondaryBonus);
			}
			if (applyWithdrawals)
			{
				village.TradeTaxAccumulated -= num2;
				if (clan == Clan.PlayerClan)
				{
					CampaignEventDispatcher.Instance.OnPlayerEarnedGoldFromAsset(DefaultClanFinanceModel.AssetIncomeType.Taxes, num);
				}
			}
			return num;
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x0006A8E8 File Offset: 0x00068AE8
		private static float CalculateShareFactor(Clan clan)
		{
			Kingdom kingdom = clan.Kingdom;
			int num = kingdom.Fiefs.Sum<Town>(delegate(Town x)
			{
				if (!x.IsCastle)
				{
					return 3;
				}
				return 1;
			}) + 1 + kingdom.Clans.Count;
			return (float)(clan.Fiefs.Sum<Town>(delegate(Town x)
			{
				if (!x.IsCastle)
				{
					return 3;
				}
				return 1;
			}) + ((clan == kingdom.RulingClan) ? 1 : 0) + 1) / (float)num;
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x0006A974 File Offset: 0x00068B74
		private void AddMercenaryIncome(Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals)
		{
			if (clan.IsUnderMercenaryService && clan.Leader != null && clan.Kingdom != null)
			{
				int num = MathF.Ceiling(clan.Influence * (1f / Campaign.Current.Models.ClanFinanceModel.RevenueSmoothenFraction())) * clan.MercenaryAwardMultiplier;
				if (applyWithdrawals)
				{
					clan.Kingdom.MercenaryWallet -= num;
				}
				goldChange.Add((float)num, DefaultClanFinanceModel._mercenaryText, null);
			}
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x0006A9EC File Offset: 0x00068BEC
		private void AddIncomeFromKingdomBudget(Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals)
		{
			int num = ((clan.Gold < 5000) ? 2000 : ((clan.Gold < 10000) ? 1500 : ((clan.Gold < 20000) ? 1000 : 500)));
			num *= ((clan.Kingdom.KingdomBudgetWallet > 1000000) ? 2 : 1);
			num *= ((clan.Leader == clan.Kingdom.Leader) ? 2 : 1);
			int num2 = MathF.Min(clan.Kingdom.KingdomBudgetWallet, num);
			if (applyWithdrawals)
			{
				clan.Kingdom.KingdomBudgetWallet -= num2;
			}
			goldChange.Add((float)num2, DefaultClanFinanceModel._kingdomSupportText, null);
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x0006AAA4 File Offset: 0x00068CA4
		private void AddPlayerClanIncomeFromOwnedAlleys(ref ExplainedNumber goldChange)
		{
			int num = 0;
			foreach (Alley alley in Hero.MainHero.OwnedAlleys)
			{
				num += Campaign.Current.Models.AlleyModel.GetDailyIncomeOfAlley(alley);
			}
			goldChange.Add((float)num, DefaultClanFinanceModel._alleyText, null);
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x0006AB1C File Offset: 0x00068D1C
		private void AddIncomeFromTribute(Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals, bool includeDetails)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, goldChange.IncludeDescriptions, null);
			IFaction mapFaction = clan.MapFaction;
			float num = 1f;
			if (clan.Kingdom != null)
			{
				num = DefaultClanFinanceModel.CalculateShareFactor(clan);
			}
			foreach (StanceLink stanceLink in FactionHelper.GetStances(mapFaction))
			{
				IFaction faction = ((stanceLink.Faction1 == mapFaction) ? stanceLink.Faction2 : stanceLink.Faction1);
				int dailyTributeToPay = stanceLink.GetDailyTributeToPay(mapFaction);
				if (!mapFaction.IsAtWarWith(faction) && dailyTributeToPay < 0)
				{
					int num2 = (int)((float)dailyTributeToPay * num);
					if (applyWithdrawals)
					{
						faction.TributeWallet += num2;
						if (stanceLink.Faction1 == mapFaction)
						{
							stanceLink.TotalTributePaidFrom2To1 += -num2;
						}
						if (stanceLink.Faction2 == mapFaction)
						{
							stanceLink.TotalTributePaidFrom1To2 += -num2;
						}
						CampaignEventDispatcher.Instance.OnClanEarnedGoldFromTribute(clan, faction);
						if (clan == Clan.PlayerClan)
						{
							CampaignEventDispatcher.Instance.OnPlayerEarnedGoldFromAsset(DefaultClanFinanceModel.AssetIncomeType.TributesEarned, -num2);
						}
					}
					explainedNumber.Add((float)(-(float)num2), Game.Current.GameTextManager.FindText(DefaultClanFinanceModel._tributeIncomeStr, null), faction.InformalName);
				}
			}
			if (!includeDetails)
			{
				goldChange.Add(explainedNumber.ResultNumber, DefaultClanFinanceModel._tributeIncomes, null);
				return;
			}
			goldChange.AddFromExplainedNumber(explainedNumber, DefaultClanFinanceModel._tributeIncomes);
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x0006AC94 File Offset: 0x00068E94
		private void AddIncomeFromTradeAgreements(Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals, bool includeDetails)
		{
			foreach (Kingdom kingdom in Kingdom.All)
			{
				TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement;
				if (kingdom != clan.Kingdom && !kingdom.IsEliminated && this.TradeAgreementsBehavior.HasTradeAgreement(kingdom, clan.Kingdom, out tradeAgreement))
				{
					this.AddIncomeFromTradeAgreements(clan, ref tradeAgreement, ref goldChange, applyWithdrawals, includeDetails);
				}
			}
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x0006AD14 File Offset: 0x00068F14
		private void AddIncomeFromTradeAgreements(Clan clan, ref TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement, ref ExplainedNumber goldChange, bool applyWithdrawals, bool includeDetails)
		{
			bool flag = clan.Kingdom == tradeAgreement.Kingdom1;
			int num = (flag ? tradeAgreement.Kingdom1GoldGained : tradeAgreement.Kingdom2GoldGained);
			if (num > 0)
			{
				ExplainedNumber explainedNumber = new ExplainedNumber(0f, goldChange.IncludeDescriptions, null);
				float num2 = 1f / (float)clan.Kingdom.Clans.Count<Clan>((Clan x) => !x.IsUnderMercenaryService);
				int num3 = (int)((float)num * num2);
				if (num3 > 0)
				{
					TextObject textObject = Game.Current.GameTextManager.FindText("str_finance_trade_agreement_income", null).SetTextVariable("KINGDOM", flag ? tradeAgreement.Kingdom2.Name : tradeAgreement.Kingdom1.Name);
					explainedNumber.Add((float)num3, textObject, null);
					if (!includeDetails)
					{
						goldChange.Add(explainedNumber.ResultNumber, textObject, null);
					}
					else
					{
						goldChange.AddFromExplainedNumber(explainedNumber, textObject);
					}
					if (applyWithdrawals)
					{
						this.TradeAgreementsBehavior.OnTradeGoldDistributedInKingdom(tradeAgreement.Kingdom1, tradeAgreement.Kingdom2, clan, num3);
					}
				}
			}
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x0006AE28 File Offset: 0x00069028
		private void AddIncomeFromCallToWarAgrements(Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals)
		{
			if (clan.Kingdom != null && clan.Kingdom.CallToWarWallet > 0)
			{
				float num = DefaultClanFinanceModel.CalculateShareFactor(clan);
				int num2 = (int)((float)clan.Kingdom.CallToWarWallet * num);
				if (applyWithdrawals)
				{
					clan.Kingdom.CallToWarWallet -= num2;
					if (clan == Clan.PlayerClan)
					{
						CampaignEventDispatcher.Instance.OnPlayerEarnedGoldFromAsset(DefaultClanFinanceModel.AssetIncomeType.TributesEarned, num2);
					}
				}
				goldChange.Add((float)num2, DefaultClanFinanceModel._callToWarIncomes, null);
			}
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x0006AE9C File Offset: 0x0006909C
		private void AddIncomeFromParties(Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals, bool includeDetails)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, goldChange.IncludeDescriptions, null);
			foreach (Hero hero in clan.AliveLords)
			{
				foreach (CaravanPartyComponent caravanPartyComponent in hero.OwnedCaravans)
				{
					if (caravanPartyComponent.MobileParty.IsActive && caravanPartyComponent.MobileParty.LeaderHero != clan.Leader && (caravanPartyComponent.MobileParty.IsLordParty || caravanPartyComponent.MobileParty.IsGarrison || caravanPartyComponent.MobileParty.IsCaravan))
					{
						int num = this.AddIncomeFromParty(caravanPartyComponent.MobileParty, clan, ref goldChange, applyWithdrawals);
						explainedNumber.Add((float)num, Game.Current.GameTextManager.FindText(caravanPartyComponent.MobileParty.CaravanPartyComponent.CanHaveNavalNavigationCapability ? DefaultClanFinanceModel._convoyIncomeStr : DefaultClanFinanceModel._caravanIncomeStr, null), (caravanPartyComponent.Leader != null) ? caravanPartyComponent.Leader.Name : caravanPartyComponent.Name);
					}
				}
			}
			foreach (Hero hero2 in clan.Companions)
			{
				foreach (CaravanPartyComponent caravanPartyComponent2 in hero2.OwnedCaravans)
				{
					if (caravanPartyComponent2.MobileParty.IsActive && caravanPartyComponent2.MobileParty.LeaderHero != clan.Leader && (caravanPartyComponent2.MobileParty.IsLordParty || caravanPartyComponent2.MobileParty.IsGarrison || caravanPartyComponent2.MobileParty.IsCaravan))
					{
						int num2 = this.AddIncomeFromParty(caravanPartyComponent2.MobileParty, clan, ref goldChange, applyWithdrawals);
						explainedNumber.Add((float)num2, Game.Current.GameTextManager.FindText(caravanPartyComponent2.MobileParty.CaravanPartyComponent.CanHaveNavalNavigationCapability ? DefaultClanFinanceModel._convoyIncomeStr : DefaultClanFinanceModel._caravanIncomeStr, null), (caravanPartyComponent2.Leader != null) ? caravanPartyComponent2.Leader.Name : caravanPartyComponent2.Name);
					}
				}
			}
			foreach (WarPartyComponent warPartyComponent in clan.WarPartyComponents)
			{
				if (warPartyComponent.MobileParty.IsActive && warPartyComponent.MobileParty.LeaderHero != clan.Leader && (warPartyComponent.MobileParty.IsLordParty || warPartyComponent.MobileParty.IsGarrison || warPartyComponent.MobileParty.IsCaravan))
				{
					int num3 = this.AddIncomeFromParty(warPartyComponent.MobileParty, clan, ref goldChange, applyWithdrawals);
					explainedNumber.Add((float)num3, Game.Current.GameTextManager.FindText(DefaultClanFinanceModel._partyIncomeStr, null), warPartyComponent.MobileParty.Name);
				}
			}
			if (!includeDetails)
			{
				goldChange.Add(explainedNumber.ResultNumber, DefaultClanFinanceModel._caravanAndPartyIncome, null);
				return;
			}
			goldChange.AddFromExplainedNumber(explainedNumber, DefaultClanFinanceModel._caravanAndPartyIncome);
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x0006B25C File Offset: 0x0006945C
		private int AddIncomeFromParty(MobileParty party, Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals)
		{
			int num = 0;
			if (party.IsActive && party.LeaderHero != clan.Leader && (party.IsLordParty || party.IsGarrison || party.IsCaravan))
			{
				int partyTradeGold = party.PartyTradeGold;
				if (partyTradeGold > 10000)
				{
					num = (partyTradeGold - 10000) / 10;
					if (applyWithdrawals)
					{
						party.PartyTradeGold -= num;
						if (party.LeaderHero != null && num > 0)
						{
							SkillLevelingManager.OnTradeProfitMade(party.LeaderHero, num);
						}
						Hero owner = party.Party.Owner;
						bool flag;
						if (owner == null)
						{
							flag = null != null;
						}
						else
						{
							Clan clan2 = owner.Clan;
							flag = ((clan2 != null) ? clan2.Leader : null) != null;
						}
						if (flag && party.IsCaravan && party.Party.Owner.Clan.Leader.GetPerkValue(DefaultPerks.Trade.GreatInvestor) && num > 0)
						{
							party.Party.Owner.Clan.AddRenown(DefaultPerks.Trade.GreatInvestor.PrimaryBonus, true);
						}
						if (clan == Clan.PlayerClan && party.IsCaravan)
						{
							CampaignEventDispatcher.Instance.OnPlayerEarnedGoldFromAsset(DefaultClanFinanceModel.AssetIncomeType.Caravan, num);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x0006B37C File Offset: 0x0006957C
		private void AddExpensesFromPartiesAndGarrisons(Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals, bool includeDetails)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, goldChange.IncludeDescriptions, null);
			int num = this.AddExpenseFromLeaderParty(clan, goldChange, applyWithdrawals);
			explainedNumber.Add((float)num, DefaultClanFinanceModel._mainPartywageText, null);
			foreach (Hero hero in clan.AliveLords)
			{
				foreach (CaravanPartyComponent caravanPartyComponent in hero.OwnedCaravans)
				{
					if (caravanPartyComponent.MobileParty.IsActive && caravanPartyComponent.MobileParty.LeaderHero != clan.Leader)
					{
						int num2 = this.AddPartyExpense(caravanPartyComponent.MobileParty, clan, goldChange, applyWithdrawals);
						if (explainedNumber.IncludeDescriptions)
						{
							explainedNumber.Add((float)num2, Game.Current.GameTextManager.FindText(DefaultClanFinanceModel._partyExpensesStr, null), caravanPartyComponent.Name);
						}
						else
						{
							explainedNumber.Add((float)num2, null, null);
						}
					}
				}
			}
			foreach (Hero hero2 in clan.Companions)
			{
				foreach (CaravanPartyComponent caravanPartyComponent2 in hero2.OwnedCaravans)
				{
					int num3 = this.AddPartyExpense(caravanPartyComponent2.MobileParty, clan, goldChange, applyWithdrawals);
					if (explainedNumber.IncludeDescriptions)
					{
						explainedNumber.Add((float)num3, Game.Current.GameTextManager.FindText(DefaultClanFinanceModel._partyExpensesStr, null), caravanPartyComponent2.Name);
					}
					else
					{
						explainedNumber.Add((float)num3, null, null);
					}
				}
			}
			foreach (WarPartyComponent warPartyComponent in clan.WarPartyComponents)
			{
				if (warPartyComponent.MobileParty.IsActive && warPartyComponent.MobileParty.LeaderHero != clan.Leader)
				{
					int num4 = this.AddPartyExpense(warPartyComponent.MobileParty, clan, goldChange, applyWithdrawals);
					if (explainedNumber.IncludeDescriptions)
					{
						explainedNumber.Add((float)num4, Game.Current.GameTextManager.FindText(DefaultClanFinanceModel._partyExpensesStr, null), warPartyComponent.Name);
					}
					else
					{
						explainedNumber.Add((float)num4, null, null);
					}
				}
			}
			foreach (Town town in clan.Fiefs)
			{
				if (town.GarrisonParty != null && town.GarrisonParty.IsActive)
				{
					int num5 = this.AddPartyExpense(town.GarrisonParty, clan, goldChange, applyWithdrawals);
					if (explainedNumber.IncludeDescriptions)
					{
						TextObject textObject = new TextObject("{=fsTBcLvA}{SETTLEMENT} Garrison", null);
						textObject.SetTextVariable("SETTLEMENT", town.Name);
						explainedNumber.Add((float)num5, Game.Current.GameTextManager.FindText(DefaultClanFinanceModel._partyExpensesStr, null), textObject);
					}
					else
					{
						explainedNumber.Add((float)num5, null, null);
					}
				}
			}
			if (!includeDetails)
			{
				goldChange.Add(explainedNumber.ResultNumber, DefaultClanFinanceModel._garrisonAndPartyExpenses, null);
				return;
			}
			goldChange.AddFromExplainedNumber(explainedNumber, DefaultClanFinanceModel._garrisonAndPartyExpenses);
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x0006B728 File Offset: 0x00069928
		private void AddExpensesForAutoRecruitment(Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals = false)
		{
			int num = clan.AutoRecruitmentExpenses / 5;
			if (applyWithdrawals)
			{
				clan.AutoRecruitmentExpenses -= num;
			}
			goldChange.Add((float)(-(float)num), DefaultClanFinanceModel._autoRecruitmentText, null);
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x0006B760 File Offset: 0x00069960
		private int AddExpenseFromLeaderParty(Clan clan, ExplainedNumber goldChange, bool applyWithdrawals)
		{
			Hero leader = clan.Leader;
			MobileParty mobileParty = ((leader != null) ? leader.PartyBelongedTo : null);
			if (mobileParty != null)
			{
				int num = clan.Gold + (int)goldChange.ResultNumber;
				if (num < 2000 && applyWithdrawals && clan != Clan.PlayerClan)
				{
					num = 0;
				}
				return -this.CalculatePartyWage(mobileParty, num, applyWithdrawals);
			}
			return 0;
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x0006B7B8 File Offset: 0x000699B8
		private int AddPartyExpense(MobileParty party, Clan clan, ExplainedNumber goldChange, bool applyWithdrawals)
		{
			int num = clan.Gold + (int)goldChange.ResultNumber;
			int num2 = num;
			if (num < (party.IsGarrison ? 8000 : 4000) && applyWithdrawals && clan != Clan.PlayerClan)
			{
				num2 = ((party.LeaderHero != null && party.PartyTradeGold < 500) ? MathF.Min(num, 250) : 0);
			}
			int num3 = this.CalculatePartyWage(party, num2, applyWithdrawals);
			int num4 = party.PartyTradeGold;
			if (applyWithdrawals)
			{
				if (party.IsLordParty && party.LeaderHero == null)
				{
					party.ActualClan.Leader.Gold -= num3;
				}
				else
				{
					party.PartyTradeGold -= num3;
				}
			}
			num4 -= num3;
			if (num4 < this.PartyGoldLowerThreshold)
			{
				int num5 = this.PartyGoldLowerThreshold - num4;
				if (party.IsLordParty && party.LeaderHero == null)
				{
					num5 = num3;
				}
				if (applyWithdrawals)
				{
					num5 = MathF.Min(num5, num2);
					party.PartyTradeGold += num5;
				}
				return -num5;
			}
			return 0;
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x0006B8B9 File Offset: 0x00069AB9
		public override int CalculateOwnerIncomeFromCaravan(MobileParty caravan)
		{
			return (int)((float)MathF.Max(0, caravan.PartyTradeGold - Campaign.Current.Models.CaravanModel.GetInitialTradeGold(caravan.Owner, caravan.CaravanPartyComponent.CanHaveNavalNavigationCapability, false)) / this.RevenueSmoothenFraction());
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x0006B8F7 File Offset: 0x00069AF7
		public override int CalculateOwnerIncomeFromWorkshop(Workshop workshop)
		{
			return (int)((float)MathF.Max(0, workshop.ProfitMade) / this.RevenueSmoothenFraction());
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x0006B910 File Offset: 0x00069B10
		private void CalculateHeroIncomeFromAssets(Hero hero, ref ExplainedNumber goldChange, bool applyWithdrawals)
		{
			int num = 0;
			foreach (CaravanPartyComponent caravanPartyComponent in hero.OwnedCaravans)
			{
				if (caravanPartyComponent.MobileParty.PartyTradeGold > Campaign.Current.Models.CaravanModel.GetInitialTradeGold(caravanPartyComponent.Owner, caravanPartyComponent.CanHaveNavalNavigationCapability, false))
				{
					int num2 = Campaign.Current.Models.ClanFinanceModel.CalculateOwnerIncomeFromCaravan(caravanPartyComponent.MobileParty);
					if (applyWithdrawals)
					{
						caravanPartyComponent.MobileParty.PartyTradeGold -= num2;
						SkillLevelingManager.OnTradeProfitMade(hero, num2);
					}
					if (num2 > 0)
					{
						num += num2;
					}
				}
			}
			goldChange.Add((float)num, goldChange.IncludeDescriptions ? Game.Current.GameTextManager.FindText(DefaultClanFinanceModel._caravanIncomeStr, null) : null, null);
			this.CalculateHeroIncomeFromWorkshops(hero, ref goldChange, applyWithdrawals);
			if (hero.CurrentSettlement != null)
			{
				foreach (Alley alley in hero.CurrentSettlement.Alleys)
				{
					if (alley.Owner == hero)
					{
						goldChange.Add(30f, alley.Name, null);
					}
				}
			}
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x0006BA68 File Offset: 0x00069C68
		private void CalculateHeroIncomeFromWorkshops(Hero hero, ref ExplainedNumber goldChange, bool applyWithdrawals)
		{
			int num = 0;
			int num2 = 0;
			foreach (Workshop workshop in hero.OwnedWorkshops)
			{
				int num3 = Campaign.Current.Models.ClanFinanceModel.CalculateOwnerIncomeFromWorkshop(workshop);
				num += num3;
				if (applyWithdrawals && num3 > 0)
				{
					workshop.ChangeGold(-num3);
					if (hero == Hero.MainHero)
					{
						CampaignEventDispatcher.Instance.OnPlayerEarnedGoldFromAsset(DefaultClanFinanceModel.AssetIncomeType.Workshop, num3);
					}
				}
				if (num3 > 0)
				{
					num2++;
				}
			}
			goldChange.Add((float)num, DefaultClanFinanceModel._shopIncomeText, null);
			bool flag;
			if (hero.Clan != null)
			{
				Hero leader = hero.Clan.Leader;
				flag = leader != null && leader.GetPerkValue(DefaultPerks.Trade.ArtisanCommunity);
			}
			else
			{
				flag = false;
			}
			if (flag && applyWithdrawals && num2 > 0)
			{
				hero.Clan.AddRenown((float)num2 * DefaultPerks.Trade.ArtisanCommunity.PrimaryBonus, true);
			}
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x0006BB5C File Offset: 0x00069D5C
		public override float RevenueSmoothenFraction()
		{
			return 5f;
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x0006BB64 File Offset: 0x00069D64
		private int CalculatePartyWage(MobileParty mobileParty, int budget, bool applyWithdrawals)
		{
			int totalWage = mobileParty.TotalWage;
			int num = totalWage;
			if (applyWithdrawals)
			{
				num = MathF.Min(totalWage, budget);
				DefaultClanFinanceModel.ApplyMoraleEffect(mobileParty, totalWage, num);
			}
			return num;
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x0006BB90 File Offset: 0x00069D90
		public override int CalculateNotableDailyGoldChange(Hero hero, bool applyWithdrawals)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			this.CalculateHeroIncomeFromAssets(hero, ref explainedNumber, applyWithdrawals);
			return (int)explainedNumber.ResultNumber;
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x0006BBC0 File Offset: 0x00069DC0
		private static void ApplyMoraleEffect(MobileParty mobileParty, int wage, int paymentAmount)
		{
			if (paymentAmount < wage && wage > 0)
			{
				float num = 1f - (float)paymentAmount / (float)wage;
				float num2 = (float)Campaign.Current.Models.PartyMoraleModel.GetDailyNoWageMoralePenalty(mobileParty) * num;
				if (mobileParty.HasUnpaidWages < num)
				{
					num2 += (float)Campaign.Current.Models.PartyMoraleModel.GetDailyNoWageMoralePenalty(mobileParty) * (num - mobileParty.HasUnpaidWages);
				}
				mobileParty.RecentEventsMorale += num2;
				mobileParty.HasUnpaidWages = num;
				MBTextManager.SetTextVariable("reg1", MathF.Round(MathF.Abs(num2), 1), 2);
				if (mobileParty == MobileParty.MainParty)
				{
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_party_loses_moral_due_to_insufficent_funds", null), 0, null, null, "");
					return;
				}
			}
			else
			{
				mobileParty.HasUnpaidWages = 0f;
			}
		}

		// Token: 0x040007AC RID: 1964
		private static readonly string _townTaxStr = "str_finance_town_tax";

		// Token: 0x040007AD RID: 1965
		private static readonly string _partyIncomeStr = "str_finance_party_income";

		// Token: 0x040007AE RID: 1966
		private static readonly string _partyExpensesStr = "str_finance_party_expenses";

		// Token: 0x040007AF RID: 1967
		private static readonly string _tariffTaxStr = "str_finance_tariff_tax";

		// Token: 0x040007B0 RID: 1968
		private static readonly TextObject _projectsIncomeText = Game.Current.GameTextManager.FindText("str_finance_projects_income", null);

		// Token: 0x040007B1 RID: 1969
		private static readonly string _caravanIncomeStr = "str_finance_caravan_income";

		// Token: 0x040007B2 RID: 1970
		private static readonly string _convoyIncomeStr = "str_finance_convoy_income";

		// Token: 0x040007B3 RID: 1971
		private static readonly TextObject _shopExpenseText = Game.Current.GameTextManager.FindText("str_finance_shop_expense", null);

		// Token: 0x040007B4 RID: 1972
		private static readonly TextObject _mercenaryText = Game.Current.GameTextManager.FindText("str_finance_mercenary", null);

		// Token: 0x040007B5 RID: 1973
		private static readonly TextObject _mercenaryExpensesText = Game.Current.GameTextManager.FindText("str_finance_mercenary_expenses", null);

		// Token: 0x040007B6 RID: 1974
		private static readonly string _tributeIncomeStr = "str_finance_tribute_income";

		// Token: 0x040007B7 RID: 1975
		private static readonly TextObject _tributeExpensesText = Game.Current.GameTextManager.FindText("str_finance_tribute_expenses", null);

		// Token: 0x040007B8 RID: 1976
		private static readonly TextObject _tributeIncomes = Game.Current.GameTextManager.FindText("str_finance_tribute_incomes", null);

		// Token: 0x040007B9 RID: 1977
		private static readonly TextObject _callToWarExpenses = Game.Current.GameTextManager.FindText("str_finance_call_to_war_expenses", null);

		// Token: 0x040007BA RID: 1978
		private static readonly TextObject _callToWarIncomes = Game.Current.GameTextManager.FindText("str_finance_call_to_war_incomes", null);

		// Token: 0x040007BB RID: 1979
		private static readonly TextObject _settlementIncome = Game.Current.GameTextManager.FindText("str_finance_settlement_income", null);

		// Token: 0x040007BC RID: 1980
		private static readonly TextObject _mainPartywageText = Game.Current.GameTextManager.FindText("str_finance_main_party_wage", null);

		// Token: 0x040007BD RID: 1981
		private static readonly TextObject _caravanAndPartyIncome = Game.Current.GameTextManager.FindText("str_finance_caravan_and_party_income", null);

		// Token: 0x040007BE RID: 1982
		private static readonly TextObject _garrisonAndPartyExpenses = Game.Current.GameTextManager.FindText("str_finance_garrison_and_party_expenses", null);

		// Token: 0x040007BF RID: 1983
		private static readonly TextObject _debtText = Game.Current.GameTextManager.FindText("str_finance_debt", null);

		// Token: 0x040007C0 RID: 1984
		private static readonly TextObject _kingdomSupportText = Game.Current.GameTextManager.FindText("str_finance_kingdom_support", null);

		// Token: 0x040007C1 RID: 1985
		private static readonly TextObject _kingdomBudgetText = Game.Current.GameTextManager.FindText("str_finance_kingdom_budget", null);

		// Token: 0x040007C2 RID: 1986
		private static readonly TextObject _autoRecruitmentText = Game.Current.GameTextManager.FindText("str_finance_auto_recruitment", null);

		// Token: 0x040007C3 RID: 1987
		private static readonly TextObject _alleyText = Game.Current.GameTextManager.FindText("str_finance_alley", null);

		// Token: 0x040007C4 RID: 1988
		private static readonly TextObject _shopIncomeText = Game.Current.GameTextManager.FindText("str_finance_shop_income", null);

		// Token: 0x040007C5 RID: 1989
		private ITradeAgreementsCampaignBehavior _tradeAgreementsCampaignBehavior;

		// Token: 0x040007C6 RID: 1990
		private const int PartyGoldIncomeThreshold = 10000;

		// Token: 0x040007C7 RID: 1991
		private const int payGarrisonWagesTreshold = 8000;

		// Token: 0x040007C8 RID: 1992
		private const int payClanPartiesTreshold = 4000;

		// Token: 0x040007C9 RID: 1993
		private const int payLeaderPartyWageTreshold = 2000;

		// Token: 0x0200057E RID: 1406
		private enum TransactionType
		{
			// Token: 0x0400177D RID: 6013
			Income = 1,
			// Token: 0x0400177E RID: 6014
			Both = 0,
			// Token: 0x0400177F RID: 6015
			Expense = -1
		}

		// Token: 0x0200057F RID: 1407
		public enum AssetIncomeType
		{
			// Token: 0x04001781 RID: 6017
			Workshop,
			// Token: 0x04001782 RID: 6018
			Caravan,
			// Token: 0x04001783 RID: 6019
			Taxes,
			// Token: 0x04001784 RID: 6020
			TributesEarned
		}
	}
}
