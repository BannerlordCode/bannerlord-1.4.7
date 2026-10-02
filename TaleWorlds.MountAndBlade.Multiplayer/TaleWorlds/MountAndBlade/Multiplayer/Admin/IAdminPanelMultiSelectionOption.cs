using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x0200006E RID: 110
	public interface IAdminPanelMultiSelectionOption : IAdminPanelOption<IAdminPanelMultiSelectionItem>, IAdminPanelOption
	{
		// Token: 0x0600035D RID: 861
		MBReadOnlyList<IAdminPanelMultiSelectionItem> GetAvailableOptions();
	}
}
