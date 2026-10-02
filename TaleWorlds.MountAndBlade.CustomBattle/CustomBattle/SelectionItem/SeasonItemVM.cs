using System;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem
{
	// Token: 0x02000026 RID: 38
	public class SeasonItemVM : SelectorItemVM
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000A8EF File Offset: 0x00008AEF
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x0000A8F7 File Offset: 0x00008AF7
		public string SeasonId { get; private set; }

		// Token: 0x060001D9 RID: 473 RVA: 0x0000A900 File Offset: 0x00008B00
		public SeasonItemVM(string seasonName, string seasonId)
			: base(seasonName)
		{
			this.SeasonId = seasonId;
		}
	}
}
