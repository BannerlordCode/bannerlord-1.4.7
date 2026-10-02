using System;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Events
{
	// Token: 0x020000C0 RID: 192
	public class SettlementOverylayQuickTalkPermissionEvent : EventBase
	{
		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06001324 RID: 4900 RVA: 0x0004DB6C File Offset: 0x0004BD6C
		// (set) Token: 0x06001325 RID: 4901 RVA: 0x0004DB74 File Offset: 0x0004BD74
		public Action<bool, TextObject> IsTalkAvailable { get; private set; }

		// Token: 0x06001326 RID: 4902 RVA: 0x0004DB7D File Offset: 0x0004BD7D
		public SettlementOverylayQuickTalkPermissionEvent(Hero heroToTalkTo, Action<bool, TextObject> isTalkAvailable)
		{
			this.HeroToTalkTo = heroToTalkTo;
			this.IsTalkAvailable = isTalkAvailable;
		}

		// Token: 0x040008B6 RID: 2230
		public Hero HeroToTalkTo;
	}
}
