using System;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Events
{
	// Token: 0x020000BE RID: 190
	public class PartyScreenCharacterTalkPermissionEvent : EventBase
	{
		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x0600131E RID: 4894 RVA: 0x0004DB1E File Offset: 0x0004BD1E
		// (set) Token: 0x0600131F RID: 4895 RVA: 0x0004DB26 File Offset: 0x0004BD26
		public Action<bool, TextObject> IsTalkAvailable { get; private set; }

		// Token: 0x06001320 RID: 4896 RVA: 0x0004DB2F File Offset: 0x0004BD2F
		public PartyScreenCharacterTalkPermissionEvent(Hero heroToTalkTo, Action<bool, TextObject> isTalkAvailable)
		{
			this.HeroToTalkTo = heroToTalkTo;
			this.IsTalkAvailable = isTalkAvailable;
		}

		// Token: 0x040008B2 RID: 2226
		public Hero HeroToTalkTo;
	}
}
