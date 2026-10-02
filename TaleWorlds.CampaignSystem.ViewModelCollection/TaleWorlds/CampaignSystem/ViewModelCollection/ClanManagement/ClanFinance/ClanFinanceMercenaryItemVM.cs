using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanFinance
{
	// Token: 0x02000134 RID: 308
	public class ClanFinanceMercenaryItemVM : ClanFinanceIncomeItemBaseVM
	{
		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06001CC0 RID: 7360 RVA: 0x0006A817 File Offset: 0x00068A17
		// (set) Token: 0x06001CC1 RID: 7361 RVA: 0x0006A81F File Offset: 0x00068A1F
		public Clan Clan { get; private set; }

		// Token: 0x06001CC2 RID: 7362 RVA: 0x0006A828 File Offset: 0x00068A28
		public ClanFinanceMercenaryItemVM(Action<ClanFinanceIncomeItemBaseVM> onSelection, Action onRefresh)
			: base(onSelection, onRefresh)
		{
			base.IncomeTypeAsEnum = IncomeTypes.MercenaryService;
			this.Clan = Clan.PlayerClan;
			if (this.Clan.IsUnderMercenaryService)
			{
				base.Name = GameTexts.FindText("str_mercenary_service", null).ToString();
				base.Income = (int)(this.Clan.Influence * (float)this.Clan.MercenaryAwardMultiplier);
				base.Visual = new BannerImageIdentifierVM(this.Clan.Banner, false);
				base.IncomeValueText = base.DetermineIncomeText(base.Income);
			}
		}

		// Token: 0x06001CC3 RID: 7363 RVA: 0x0006A8BA File Offset: 0x00068ABA
		protected override void PopulateStatsList()
		{
			base.ItemProperties.Add(new SelectableItemPropertyVM("TEST", "TEST", false, null));
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x0006A8D8 File Offset: 0x00068AD8
		protected override void PopulateActionList()
		{
		}
	}
}
