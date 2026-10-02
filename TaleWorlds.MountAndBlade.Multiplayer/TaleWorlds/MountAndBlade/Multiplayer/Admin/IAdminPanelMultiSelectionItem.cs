using System;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x02000068 RID: 104
	public interface IAdminPanelMultiSelectionItem
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000341 RID: 833
		string Value { get; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000342 RID: 834
		string DisplayName { get; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000343 RID: 835
		bool IsFallbackValue { get; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000344 RID: 836
		bool IsDisabled { get; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000345 RID: 837
		bool CanBeApplied { get; }
	}
}
