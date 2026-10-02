using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;

namespace SandBox.ViewModelCollection.Nameplate.NameplateNotifications.SettlementNotificationTypes
{
	// Token: 0x02000029 RID: 41
	public class TroopGivenToSettlementNotificationItemVM : SettlementNotificationItemBaseVM
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000375 RID: 885 RVA: 0x0000F0C3 File Offset: 0x0000D2C3
		// (set) Token: 0x06000376 RID: 886 RVA: 0x0000F0CB File Offset: 0x0000D2CB
		public Hero GiverHero { get; private set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000377 RID: 887 RVA: 0x0000F0D4 File Offset: 0x0000D2D4
		// (set) Token: 0x06000378 RID: 888 RVA: 0x0000F0DC File Offset: 0x0000D2DC
		public TroopRoster Troops { get; private set; }

		// Token: 0x06000379 RID: 889 RVA: 0x0000F0E8 File Offset: 0x0000D2E8
		public TroopGivenToSettlementNotificationItemVM(Action<SettlementNotificationItemBaseVM> onRemove, Hero giverHero, TroopRoster troops, int createdTick)
			: base(onRemove, createdTick)
		{
			this.GiverHero = giverHero;
			this.Troops = troops;
			base.Text = SandBoxUIHelper.GetTroopGivenToSettlementNotificationText(this.Troops.TotalManCount);
			base.CharacterName = ((this.GiverHero != null) ? this.GiverHero.Name.ToString() : "null hero");
			base.CharacterVisual = ((this.GiverHero != null) ? new CharacterImageIdentifierVM(SandBoxUIHelper.GetCharacterCode(this.GiverHero.CharacterObject, false)) : new CharacterImageIdentifierVM(null));
			base.RelationType = 0;
			base.CreatedTick = createdTick;
			if (this.GiverHero != null)
			{
				base.RelationType = (this.GiverHero.Clan.IsAtWarWith(Hero.MainHero.Clan) ? (-1) : 1);
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000F1B0 File Offset: 0x0000D3B0
		public void AddNewAction(TroopRoster newTroops)
		{
			this.Troops.Add(newTroops);
			base.Text = SandBoxUIHelper.GetTroopGivenToSettlementNotificationText(this.Troops.TotalManCount);
		}
	}
}
