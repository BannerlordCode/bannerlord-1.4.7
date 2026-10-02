using System;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000096 RID: 150
	public interface INavigationElement
	{
		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x060012AD RID: 4781
		string StringId { get; }

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x060012AE RID: 4782
		NavigationPermissionItem Permission { get; }

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x060012AF RID: 4783
		bool IsLockingNavigation { get; }

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x060012B0 RID: 4784
		bool IsActive { get; }

		// Token: 0x060012B1 RID: 4785
		void OpenView();

		// Token: 0x060012B2 RID: 4786
		void OpenView(params object[] parameters);

		// Token: 0x060012B3 RID: 4787
		void GoToLink();

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x060012B4 RID: 4788
		TextObject Tooltip { get; }

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x060012B5 RID: 4789
		bool HasAlert { get; }

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x060012B6 RID: 4790
		TextObject AlertTooltip { get; }
	}
}
