using System;
using System.Collections.Generic;

namespace TaleWorlds.Engine.Options
{
	// Token: 0x020000A6 RID: 166
	public interface ISelectionOptionData : IOptionData
	{
		// Token: 0x06000F46 RID: 3910
		int GetSelectableOptionsLimit();

		// Token: 0x06000F47 RID: 3911
		IEnumerable<SelectionData> GetSelectableOptionNames();
	}
}
