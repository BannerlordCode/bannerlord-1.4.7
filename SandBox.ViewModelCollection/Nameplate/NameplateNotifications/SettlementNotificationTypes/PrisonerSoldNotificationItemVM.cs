using System;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;

namespace SandBox.ViewModelCollection.Nameplate.NameplateNotifications.SettlementNotificationTypes
{
	// Token: 0x02000026 RID: 38
	public class PrisonerSoldNotificationItemVM : SettlementNotificationItemBaseVM
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600035A RID: 858 RVA: 0x0000E710 File Offset: 0x0000C910
		// (set) Token: 0x0600035B RID: 859 RVA: 0x0000E718 File Offset: 0x0000C918
		public MobileParty Party { get; private set; }

		// Token: 0x0600035C RID: 860 RVA: 0x0000E724 File Offset: 0x0000C924
		public PrisonerSoldNotificationItemVM(Action<SettlementNotificationItemBaseVM> onRemove, MobileParty party, TroopRoster prisoners, int createdTick)
			: base(onRemove, createdTick)
		{
			this._prisonersAmount = prisoners.TotalManCount;
			base.Text = SandBoxUIHelper.GetPrisonersSoldNotificationText(this._prisonersAmount);
			this.Party = party;
			base.CharacterName = ((party.LeaderHero != null) ? party.LeaderHero.Name.ToString() : party.Name.ToString());
			base.CharacterVisual = new CharacterImageIdentifierVM(SandBoxUIHelper.GetCharacterCode(PartyBaseHelper.GetVisualPartyLeader(party.Party), false));
			base.RelationType = 0;
			base.CreatedTick = createdTick;
			if (party.LeaderHero != null)
			{
				base.RelationType = (party.LeaderHero.Clan.IsAtWarWith(Hero.MainHero.Clan) ? (-1) : 1);
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000E7E2 File Offset: 0x0000C9E2
		public void AddNewPrisoners(TroopRoster newPrisoners)
		{
			this._prisonersAmount += newPrisoners.Count;
			base.Text = SandBoxUIHelper.GetPrisonersSoldNotificationText(this._prisonersAmount);
		}

		// Token: 0x040001B8 RID: 440
		private int _prisonersAmount;
	}
}
