using System;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem
{
	// Token: 0x02000024 RID: 36
	public class PlayerTypeItemVM : SelectorItemVM
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000A8A7 File Offset: 0x00008AA7
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x0000A8AF File Offset: 0x00008AAF
		public CustomBattlePlayerType PlayerType { get; private set; }

		// Token: 0x060001D3 RID: 467 RVA: 0x0000A8B8 File Offset: 0x00008AB8
		public PlayerTypeItemVM(string playerTypeName, CustomBattlePlayerType playerType)
			: base(playerTypeName)
		{
			this.PlayerType = playerType;
		}
	}
}
