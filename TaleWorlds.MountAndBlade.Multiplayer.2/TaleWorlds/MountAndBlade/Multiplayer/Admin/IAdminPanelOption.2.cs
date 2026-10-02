using System;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x0200006C RID: 108
	public interface IAdminPanelOption<T> : IAdminPanelOption
	{
		// Token: 0x06000359 RID: 857
		T GetValue();

		// Token: 0x0600035A RID: 858
		void SetValue(T value);
	}
}
