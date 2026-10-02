using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ArmyManagement
{
	// Token: 0x0200015F RID: 351
	public class PartyAddedToArmyByPlayerEvent : EventBase
	{
		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x060021F8 RID: 8696 RVA: 0x0007B180 File Offset: 0x00079380
		// (set) Token: 0x060021F9 RID: 8697 RVA: 0x0007B188 File Offset: 0x00079388
		public MobileParty AddedParty { get; private set; }

		// Token: 0x060021FA RID: 8698 RVA: 0x0007B191 File Offset: 0x00079391
		public PartyAddedToArmyByPlayerEvent(MobileParty addedParty)
		{
			this.AddedParty = addedParty;
		}
	}
}
