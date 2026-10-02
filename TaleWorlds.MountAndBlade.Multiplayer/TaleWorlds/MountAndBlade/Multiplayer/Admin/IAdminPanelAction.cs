using System;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x02000069 RID: 105
	public interface IAdminPanelAction
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000346 RID: 838
		string UniqueId { get; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000347 RID: 839
		string Name { get; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000348 RID: 840
		string Description { get; }

		// Token: 0x06000349 RID: 841
		bool GetIsDisabled(out string reason);

		// Token: 0x0600034A RID: 842
		void OnActionExecuted();

		// Token: 0x0600034B RID: 843
		bool GetIsAvailable();
	}
}
