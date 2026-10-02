using System;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem
{
	// Token: 0x02000021 RID: 33
	public class GameTypeItemVM : SelectorItemVM
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000A754 File Offset: 0x00008954
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x0000A75C File Offset: 0x0000895C
		public string GameTypeStringId { get; private set; }

		// Token: 0x060001C3 RID: 451 RVA: 0x0000A765 File Offset: 0x00008965
		public GameTypeItemVM(string gameTypeName, string gameType)
			: base(gameTypeName)
		{
			this.GameTypeStringId = gameType;
		}
	}
}
