using System;

namespace TaleWorlds.CampaignSystem.GameMenus
{
	// Token: 0x020000E7 RID: 231
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class GameMenuInitializationHandler : Attribute
	{
		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001580 RID: 5504 RVA: 0x00061C51 File Offset: 0x0005FE51
		// (set) Token: 0x06001581 RID: 5505 RVA: 0x00061C59 File Offset: 0x0005FE59
		public string MenuId { get; private set; }

		// Token: 0x06001582 RID: 5506 RVA: 0x00061C62 File Offset: 0x0005FE62
		public GameMenuInitializationHandler(string menuId)
		{
			this.MenuId = menuId;
		}
	}
}
