using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000169 RID: 361
	public class DefaultWorkshopModel : WorkshopModel
	{
		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06001B24 RID: 6948 RVA: 0x0008D21F File Offset: 0x0008B41F
		public override int WarehouseCapacity
		{
			get
			{
				return 6000;
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06001B25 RID: 6949 RVA: 0x0008D226 File Offset: 0x0008B426
		public override int DaysForPlayerSaveWorkshopFromBankruptcy
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06001B26 RID: 6950 RVA: 0x0008D229 File Offset: 0x0008B429
		public override int CapitalLowLimit
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06001B27 RID: 6951 RVA: 0x0008D230 File Offset: 0x0008B430
		public override int InitialCapital
		{
			get
			{
				return 10000;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06001B28 RID: 6952 RVA: 0x0008D237 File Offset: 0x0008B437
		public override int DailyExpense
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06001B29 RID: 6953 RVA: 0x0008D23B File Offset: 0x0008B43B
		public override int DefaultWorkshopCountInSettlement
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06001B2A RID: 6954 RVA: 0x0008D23E File Offset: 0x0008B43E
		public override int MaximumWorkshopsPlayerCanHave
		{
			get
			{
				return this.GetMaxWorkshopCountForClanTier(Campaign.Current.Models.ClanTierModel.MaxClanTier);
			}
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x0008D25C File Offset: 0x0008B45C
		public override ExplainedNumber GetEffectiveConversionSpeedOfProduction(Workshop workshop, float speed, bool includeDescription)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(speed, includeDescription, null);
			Settlement settlement = workshop.Settlement;
			if (settlement.OwnerClan.Kingdom != null)
			{
				if (settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.ForgivenessOfDebts))
				{
					explainedNumber.AddFactor(-0.05f, DefaultPolicies.ForgivenessOfDebts.Name);
				}
				if (settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.StateMonopolies))
				{
					explainedNumber.AddFactor(-0.1f, DefaultPolicies.StateMonopolies.Name);
				}
			}
			if (settlement.IsFortification)
			{
				settlement.Town.AddEffectOfBuildings(BuildingEffectEnum.WorkshopProduction, ref explainedNumber);
			}
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Trade.MercenaryConnections, settlement.Town, ref explainedNumber);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Steward.Sweatshops, workshop.Owner.CharacterObject, true, ref explainedNumber, false);
			return explainedNumber;
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x0008D32E File Offset: 0x0008B52E
		public override int GetMaxWorkshopCountForClanTier(int tier)
		{
			return tier + 1;
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x0008D333 File Offset: 0x0008B533
		public override int GetCostForPlayer(Workshop workshop)
		{
			return workshop.WorkshopType.EquipmentCost + (int)workshop.Settlement.Town.Prosperity * 4 + this.InitialCapital / 5;
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x0008D35D File Offset: 0x0008B55D
		public override int GetCostForNotable(Workshop workshop)
		{
			return (workshop.WorkshopType.EquipmentCost + (int)workshop.Settlement.Town.Prosperity / 2 + workshop.Capital) / 2;
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x0008D388 File Offset: 0x0008B588
		public override Hero GetNotableOwnerForWorkshop(Workshop workshop)
		{
			List<ValueTuple<Hero, float>> list = new List<ValueTuple<Hero, float>>();
			foreach (Hero hero in workshop.Settlement.Notables)
			{
				if (hero.IsAlive && hero != workshop.Owner)
				{
					int count = hero.OwnedWorkshops.Count;
					float num = Math.Max(hero.Power, 0f) / MathF.Pow(10f, (float)count);
					list.Add(new ValueTuple<Hero, float>(hero, num));
				}
			}
			return MBRandom.ChooseWeighted<Hero>(list);
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x0008D430 File Offset: 0x0008B630
		public override int GetConvertProductionCost(WorkshopType workshopType)
		{
			return workshopType.EquipmentCost;
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x0008D438 File Offset: 0x0008B638
		public override bool CanPlayerSellWorkshop(Workshop workshop, out TextObject explanation)
		{
			Campaign.Current.Models.WorkshopModel.GetCostForNotable(workshop);
			Hero notableOwnerForWorkshop = Campaign.Current.Models.WorkshopModel.GetNotableOwnerForWorkshop(workshop);
			explanation = ((notableOwnerForWorkshop == null) ? new TextObject("{=oqPf2Gdp}There isn't any prospective buyer in the town.", null) : null);
			return notableOwnerForWorkshop != null;
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x0008D488 File Offset: 0x0008B688
		public override float GetTradeXpPerWarehouseProduction(EquipmentElement production)
		{
			return (float)production.GetBaseValue() * 0.1f;
		}
	}
}
