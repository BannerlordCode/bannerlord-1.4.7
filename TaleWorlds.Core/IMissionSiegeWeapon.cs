using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200008D RID: 141
	public interface IMissionSiegeWeapon
	{
		// Token: 0x170002EE RID: 750
		// (get) Token: 0x060008AB RID: 2219
		int Index { get; }

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x060008AC RID: 2220
		SiegeEngineType Type { get; }

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x060008AD RID: 2221
		float Health { get; }

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x060008AE RID: 2222
		float InitialHealth { get; }

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x060008AF RID: 2223
		float MaxHealth { get; }
	}
}
