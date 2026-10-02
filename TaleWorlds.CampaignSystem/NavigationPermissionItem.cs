using System;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000097 RID: 151
	public struct NavigationPermissionItem
	{
		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x060012B7 RID: 4791 RVA: 0x00054C74 File Offset: 0x00052E74
		// (set) Token: 0x060012B8 RID: 4792 RVA: 0x00054C7C File Offset: 0x00052E7C
		public bool IsAuthorized { get; private set; }

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x060012B9 RID: 4793 RVA: 0x00054C85 File Offset: 0x00052E85
		// (set) Token: 0x060012BA RID: 4794 RVA: 0x00054C8D File Offset: 0x00052E8D
		public TextObject ReasonString { get; private set; }

		// Token: 0x060012BB RID: 4795 RVA: 0x00054C96 File Offset: 0x00052E96
		public NavigationPermissionItem(bool isAuthorized, TextObject reasonString)
		{
			this.IsAuthorized = isAuthorized;
			this.ReasonString = reasonString;
		}
	}
}
