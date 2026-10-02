using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes
{
	// Token: 0x0200003F RID: 63
	public class ArmyCreationNotificationItemVM : MapNotificationItemBaseVM
	{
		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0001EF93 File Offset: 0x0001D193
		public Army Army { get; }

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001EF9C File Offset: 0x0001D19C
		public ArmyCreationNotificationItemVM(ArmyCreationMapNotification data)
			: base(data)
		{
			this.Army = data.CreatedArmy;
			base.NotificationIdentifier = "armycreation";
			this._onInspect = delegate
			{
				Army army = this.Army;
				CampaignVec2? campaignVec;
				if (army == null)
				{
					campaignVec = null;
				}
				else
				{
					MobileParty leaderParty = army.LeaderParty;
					campaignVec = ((leaderParty != null) ? new CampaignVec2?(leaderParty.Position) : null);
				}
				base.GoToMapPosition(campaignVec ?? MobileParty.MainParty.Position);
			};
			CampaignEvents.OnPartyJoinedArmyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyJoinedArmy));
			CampaignEvents.ArmyDispersed.AddNonSerializedListener(this, new Action<Army, Army.ArmyDispersionReason, bool>(this.OnArmyDispersed));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001F01E File Offset: 0x0001D21E
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (clan == MobileParty.MainParty.ActualClan && oldKingdom != newKingdom)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001F037 File Offset: 0x0001D237
		private void OnArmyDispersed(Army arg1, Army.ArmyDispersionReason arg2, bool isPlayersArmy)
		{
			if (arg1 == this.Army)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001F048 File Offset: 0x0001D248
		private void OnPartyJoinedArmy(MobileParty party)
		{
			if (party == MobileParty.MainParty && party.Army == this.Army)
			{
				base.ExecuteRemove();
			}
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0001F066 File Offset: 0x0001D266
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.OnPartyJoinedArmyEvent.ClearListeners(this);
			CampaignEvents.ArmyDispersed.ClearListeners(this);
			CampaignEvents.OnClanChangedKingdomEvent.ClearListeners(this);
		}
	}
}
