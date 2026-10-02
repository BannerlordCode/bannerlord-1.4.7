using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x0200012E RID: 302
	public class ClanRoleAssignedThroughClanScreenEvent : EventBase
	{
		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06001C58 RID: 7256 RVA: 0x00068E79 File Offset: 0x00067079
		// (set) Token: 0x06001C59 RID: 7257 RVA: 0x00068E81 File Offset: 0x00067081
		public PartyRole Role { get; private set; }

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06001C5A RID: 7258 RVA: 0x00068E8A File Offset: 0x0006708A
		// (set) Token: 0x06001C5B RID: 7259 RVA: 0x00068E92 File Offset: 0x00067092
		public Hero HeroObject { get; private set; }

		// Token: 0x06001C5C RID: 7260 RVA: 0x00068E9B File Offset: 0x0006709B
		public ClanRoleAssignedThroughClanScreenEvent(PartyRole role, Hero heroObject)
		{
			this.Role = role;
			this.HeroObject = heroObject;
		}
	}
}
