using System;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x0200006B RID: 107
	public interface IAdminPanelOption
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600034D RID: 845
		string UniqueId { get; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600034E RID: 846
		string Name { get; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600034F RID: 847
		string Description { get; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000350 RID: 848
		bool RequiresMissionRestart { get; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000351 RID: 849
		bool IsRequired { get; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000352 RID: 850
		bool IsDirty { get; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000353 RID: 851
		bool CanRevertToDefaultValue { get; }

		// Token: 0x06000354 RID: 852
		bool GetIsDisabled(out string reason);

		// Token: 0x06000355 RID: 853
		bool GetIsAvailable();

		// Token: 0x06000356 RID: 854
		void RevertChanges();

		// Token: 0x06000357 RID: 855
		void RestoreDefaults();

		// Token: 0x06000358 RID: 856
		void SetOnRefreshCallback(Action callback);
	}
}
