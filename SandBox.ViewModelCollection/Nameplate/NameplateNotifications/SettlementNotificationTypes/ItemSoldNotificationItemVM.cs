using System;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;

namespace SandBox.ViewModelCollection.Nameplate.NameplateNotifications.SettlementNotificationTypes
{
	// Token: 0x02000025 RID: 37
	public class ItemSoldNotificationItemVM : SettlementNotificationItemBaseVM
	{
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0000E5B4 File Offset: 0x0000C7B4
		public ItemRosterElement Item { get; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0000E5BC File Offset: 0x0000C7BC
		public PartyBase ReceiverParty { get; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000357 RID: 855 RVA: 0x0000E5C4 File Offset: 0x0000C7C4
		public PartyBase PayerParty { get; }

		// Token: 0x06000358 RID: 856 RVA: 0x0000E5CC File Offset: 0x0000C7CC
		public ItemSoldNotificationItemVM(Action<SettlementNotificationItemBaseVM> onRemove, PartyBase receiverParty, PartyBase payerParty, ItemRosterElement item, int number, int createdTick)
			: base(onRemove, createdTick)
		{
			this.Item = item;
			this.ReceiverParty = receiverParty;
			this.PayerParty = payerParty;
			this._number = number;
			this._heroParty = (receiverParty.IsSettlement ? payerParty : receiverParty);
			base.Text = SandBoxUIHelper.GetItemSoldNotificationText(this.Item, this._number, this._number < 0);
			base.CharacterName = ((this._heroParty.LeaderHero != null) ? this._heroParty.LeaderHero.Name.ToString() : this._heroParty.Name.ToString());
			CharacterObject visualPartyLeader = PartyBaseHelper.GetVisualPartyLeader(this._heroParty);
			base.CharacterVisual = new CharacterImageIdentifierVM(SandBoxUIHelper.GetCharacterCode(visualPartyLeader, false));
			base.RelationType = 0;
			base.CreatedTick = createdTick;
			if (this._heroParty.LeaderHero != null)
			{
				base.RelationType = (this._heroParty.LeaderHero.Clan.IsAtWarWith(Hero.MainHero.Clan) ? (-1) : 1);
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000E6D1 File Offset: 0x0000C8D1
		public void AddNewTransaction(int amount)
		{
			this._number += amount;
			if (this._number == 0)
			{
				base.ExecuteRemove();
				return;
			}
			base.Text = SandBoxUIHelper.GetItemSoldNotificationText(this.Item, this._number, this._number < 0);
		}

		// Token: 0x040001B5 RID: 437
		private int _number;

		// Token: 0x040001B6 RID: 438
		private PartyBase _heroParty;
	}
}
