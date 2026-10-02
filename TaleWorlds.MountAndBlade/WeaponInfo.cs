using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000392 RID: 914
	public struct WeaponInfo
	{
		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x0600347D RID: 13437 RVA: 0x000D8D67 File Offset: 0x000D6F67
		// (set) Token: 0x0600347E RID: 13438 RVA: 0x000D8D6F File Offset: 0x000D6F6F
		public bool IsValid { get; private set; }

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x0600347F RID: 13439 RVA: 0x000D8D78 File Offset: 0x000D6F78
		// (set) Token: 0x06003480 RID: 13440 RVA: 0x000D8D80 File Offset: 0x000D6F80
		public bool IsMeleeWeapon { get; private set; }

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06003481 RID: 13441 RVA: 0x000D8D89 File Offset: 0x000D6F89
		// (set) Token: 0x06003482 RID: 13442 RVA: 0x000D8D91 File Offset: 0x000D6F91
		public bool IsRangedWeapon { get; private set; }

		// Token: 0x06003483 RID: 13443 RVA: 0x000D8D9A File Offset: 0x000D6F9A
		public WeaponInfo(bool isValid, bool isMeleeWeapon, bool isRangedWeapon)
		{
			this.IsValid = isValid;
			this.IsMeleeWeapon = isMeleeWeapon;
			this.IsRangedWeapon = isRangedWeapon;
		}
	}
}
