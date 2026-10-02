using System;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;

namespace SandBox.ViewModelCollection.Nameplate.NameplateNotifications.SettlementNotificationTypes
{
	// Token: 0x02000028 RID: 40
	public class ShipSoldNotificationItemVM : SettlementNotificationItemBaseVM
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000370 RID: 880 RVA: 0x0000EF31 File Offset: 0x0000D131
		public Ship Ship { get; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000371 RID: 881 RVA: 0x0000EF39 File Offset: 0x0000D139
		public PartyBase SettlementParty { get; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000372 RID: 882 RVA: 0x0000EF41 File Offset: 0x0000D141
		public PartyBase HeroParty { get; }

		// Token: 0x06000373 RID: 883 RVA: 0x0000EF4C File Offset: 0x0000D14C
		public ShipSoldNotificationItemVM(Action<SettlementNotificationItemBaseVM> onRemove, Ship ship, PartyBase settlementParty, PartyBase heroParty, int amount, int createdTick)
			: base(onRemove, createdTick)
		{
			this.Ship = ship;
			this.SettlementParty = settlementParty;
			this.HeroParty = heroParty;
			this._amount = amount;
			base.Text = SandBoxUIHelper.GetShipSoldNotificationText(this.Ship, Math.Abs(this._amount), this._amount < 0);
			Hero leaderHero = this.HeroParty.LeaderHero;
			base.CharacterName = ((leaderHero != null) ? leaderHero.Name.ToString() : null) ?? this.HeroParty.Name.ToString();
			CharacterObject visualPartyLeader = PartyBaseHelper.GetVisualPartyLeader(this.HeroParty);
			if (visualPartyLeader != null)
			{
				base.CharacterVisual = new CharacterImageIdentifierVM(SandBoxUIHelper.GetCharacterCode(visualPartyLeader, false));
			}
			else if (this.HeroParty.Owner != null)
			{
				base.CharacterVisual = new CharacterImageIdentifierVM(SandBoxUIHelper.GetCharacterCode(this.HeroParty.Owner.CharacterObject, false));
			}
			base.RelationType = 0;
			base.CreatedTick = createdTick;
			if (this.HeroParty.LeaderHero != null)
			{
				base.RelationType = (this.HeroParty.LeaderHero.Clan.IsAtWarWith(Hero.MainHero.Clan) ? (-1) : 1);
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000F074 File Offset: 0x0000D274
		public void AddNewTransaction(int amount)
		{
			this._amount += amount;
			if (this._amount == 0)
			{
				base.ExecuteRemove();
				return;
			}
			base.Text = SandBoxUIHelper.GetShipSoldNotificationText(this.Ship, Math.Abs(this._amount), this._amount < 0);
		}

		// Token: 0x040001C1 RID: 449
		private int _amount;
	}
}
