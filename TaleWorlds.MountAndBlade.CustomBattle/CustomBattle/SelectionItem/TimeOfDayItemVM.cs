using System;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem
{
	// Token: 0x02000027 RID: 39
	public class TimeOfDayItemVM : SelectorItemVM
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000A910 File Offset: 0x00008B10
		// (set) Token: 0x060001DB RID: 475 RVA: 0x0000A918 File Offset: 0x00008B18
		public int TimeOfDay { get; private set; }

		// Token: 0x060001DC RID: 476 RVA: 0x0000A921 File Offset: 0x00008B21
		public TimeOfDayItemVM(string timeOfDayName, int timeOfDay)
			: base(timeOfDayName)
		{
			this.TimeOfDay = timeOfDay;
		}
	}
}
