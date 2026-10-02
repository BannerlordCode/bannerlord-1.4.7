using System;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem
{
	// Token: 0x02000028 RID: 40
	public class WallHitpointItemVM : SelectorItemVM
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001DD RID: 477 RVA: 0x0000A931 File Offset: 0x00008B31
		// (set) Token: 0x060001DE RID: 478 RVA: 0x0000A939 File Offset: 0x00008B39
		public string WallState { get; private set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000A942 File Offset: 0x00008B42
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x0000A94A File Offset: 0x00008B4A
		public int BreachedWallCount { get; private set; }

		// Token: 0x060001E1 RID: 481 RVA: 0x0000A953 File Offset: 0x00008B53
		public WallHitpointItemVM(string wallStateName, int breachedWallCount)
			: base(wallStateName)
		{
			this.WallState = wallStateName;
			this.BreachedWallCount = breachedWallCount;
		}
	}
}
