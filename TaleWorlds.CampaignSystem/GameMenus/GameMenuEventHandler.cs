using System;

namespace TaleWorlds.CampaignSystem.GameMenus
{
	// Token: 0x020000E6 RID: 230
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class GameMenuEventHandler : Attribute
	{
		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001579 RID: 5497 RVA: 0x00061C01 File Offset: 0x0005FE01
		// (set) Token: 0x0600157A RID: 5498 RVA: 0x00061C09 File Offset: 0x0005FE09
		public string MenuId { get; private set; }

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x0600157B RID: 5499 RVA: 0x00061C12 File Offset: 0x0005FE12
		// (set) Token: 0x0600157C RID: 5500 RVA: 0x00061C1A File Offset: 0x0005FE1A
		public string MenuOptionId { get; private set; }

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x0600157D RID: 5501 RVA: 0x00061C23 File Offset: 0x0005FE23
		// (set) Token: 0x0600157E RID: 5502 RVA: 0x00061C2B File Offset: 0x0005FE2B
		public GameMenuEventHandler.EventType Type { get; private set; }

		// Token: 0x0600157F RID: 5503 RVA: 0x00061C34 File Offset: 0x0005FE34
		public GameMenuEventHandler(string menuId, string menuOptionId, GameMenuEventHandler.EventType type)
		{
			this.MenuId = menuId;
			this.MenuOptionId = menuOptionId;
			this.Type = type;
		}

		// Token: 0x02000564 RID: 1380
		public enum EventType
		{
			// Token: 0x04001706 RID: 5894
			OnCondition,
			// Token: 0x04001707 RID: 5895
			OnConsequence
		}
	}
}
