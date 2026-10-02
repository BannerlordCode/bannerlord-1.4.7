using System;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem
{
	// Token: 0x02000023 RID: 35
	public class PlayerSideItemVM : SelectorItemVM
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001CE RID: 462 RVA: 0x0000A886 File Offset: 0x00008A86
		// (set) Token: 0x060001CF RID: 463 RVA: 0x0000A88E File Offset: 0x00008A8E
		public CustomBattlePlayerSide PlayerSide { get; private set; }

		// Token: 0x060001D0 RID: 464 RVA: 0x0000A897 File Offset: 0x00008A97
		public PlayerSideItemVM(string playerSideName, CustomBattlePlayerSide playerSide)
			: base(playerSideName)
		{
			this.PlayerSide = playerSide;
		}
	}
}
