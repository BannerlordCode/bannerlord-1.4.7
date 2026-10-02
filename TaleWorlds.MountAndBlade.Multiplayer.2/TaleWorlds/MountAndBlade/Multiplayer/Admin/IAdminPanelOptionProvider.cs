using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x02000072 RID: 114
	public interface IAdminPanelOptionProvider
	{
		// Token: 0x0600036A RID: 874
		MBReadOnlyList<IAdminPanelOptionGroup> GetOptionGroups();

		// Token: 0x0600036B RID: 875
		IAdminPanelOption GetOptionWithId(string id);

		// Token: 0x0600036C RID: 876
		IAdminPanelAction GetActionWithId(string id);

		// Token: 0x0600036D RID: 877
		void ApplyOptions();

		// Token: 0x0600036E RID: 878
		void OnTick(float dt);

		// Token: 0x0600036F RID: 879
		void OnFinalize();
	}
}
