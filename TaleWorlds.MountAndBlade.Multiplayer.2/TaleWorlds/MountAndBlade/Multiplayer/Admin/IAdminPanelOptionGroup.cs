using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x02000071 RID: 113
	public interface IAdminPanelOptionGroup
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000364 RID: 868
		string UniqueId { get; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000365 RID: 869
		bool RequiresRestart { get; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000366 RID: 870
		TextObject Name { get; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000367 RID: 871
		MBReadOnlyList<IAdminPanelOption> Options { get; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000368 RID: 872
		MBReadOnlyList<IAdminPanelAction> Actions { get; }

		// Token: 0x06000369 RID: 873
		void OnFinalize();
	}
}
