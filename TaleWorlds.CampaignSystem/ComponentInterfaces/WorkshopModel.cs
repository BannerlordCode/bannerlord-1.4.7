using System;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001EE RID: 494
	public abstract class WorkshopModel : MBGameModel<WorkshopModel>
	{
		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06001F2A RID: 7978
		public abstract int DaysForPlayerSaveWorkshopFromBankruptcy { get; }

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06001F2B RID: 7979
		public abstract int CapitalLowLimit { get; }

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06001F2C RID: 7980
		public abstract int InitialCapital { get; }

		// Token: 0x06001F2D RID: 7981
		public abstract int GetMaxWorkshopCountForClanTier(int tier);

		// Token: 0x06001F2E RID: 7982
		public abstract int GetCostForPlayer(Workshop workshop);

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06001F2F RID: 7983
		public abstract int DailyExpense { get; }

		// Token: 0x06001F30 RID: 7984
		public abstract int GetCostForNotable(Workshop workshop);

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06001F31 RID: 7985
		public abstract int WarehouseCapacity { get; }

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06001F32 RID: 7986
		public abstract int DefaultWorkshopCountInSettlement { get; }

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06001F33 RID: 7987
		public abstract int MaximumWorkshopsPlayerCanHave { get; }

		// Token: 0x06001F34 RID: 7988
		public abstract Hero GetNotableOwnerForWorkshop(Workshop workshop);

		// Token: 0x06001F35 RID: 7989
		public abstract ExplainedNumber GetEffectiveConversionSpeedOfProduction(Workshop workshop, float speed, bool includeDescriptions);

		// Token: 0x06001F36 RID: 7990
		public abstract int GetConvertProductionCost(WorkshopType workshopType);

		// Token: 0x06001F37 RID: 7991
		public abstract bool CanPlayerSellWorkshop(Workshop workshop, out TextObject explanation);

		// Token: 0x06001F38 RID: 7992
		public abstract float GetTradeXpPerWarehouseProduction(EquipmentElement production);
	}
}
