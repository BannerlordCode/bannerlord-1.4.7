using System;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Events
{
	// Token: 0x020000BF RID: 191
	public class SettlementOverlayTalkPermissionEvent : EventBase
	{
		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06001321 RID: 4897 RVA: 0x0004DB45 File Offset: 0x0004BD45
		// (set) Token: 0x06001322 RID: 4898 RVA: 0x0004DB4D File Offset: 0x0004BD4D
		public Action<bool, TextObject> IsTalkAvailable { get; private set; }

		// Token: 0x06001323 RID: 4899 RVA: 0x0004DB56 File Offset: 0x0004BD56
		public SettlementOverlayTalkPermissionEvent(Hero heroToTalkTo, Action<bool, TextObject> isTalkAvailable)
		{
			this.HeroToTalkTo = heroToTalkTo;
			this.IsTalkAvailable = isTalkAvailable;
		}

		// Token: 0x040008B4 RID: 2228
		public Hero HeroToTalkTo;
	}
}
