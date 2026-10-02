using System;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x0200006D RID: 109
	public interface IAdminPanelNumericOption : IAdminPanelOption<int>, IAdminPanelOption
	{
		// Token: 0x0600035B RID: 859
		int? GetMinimumValue();

		// Token: 0x0600035C RID: 860
		int? GetMaximumValue();
	}
}
