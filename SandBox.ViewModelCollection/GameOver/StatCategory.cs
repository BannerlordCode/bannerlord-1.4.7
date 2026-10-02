using System;
using System.Collections.Generic;

namespace SandBox.ViewModelCollection.GameOver
{
	// Token: 0x0200005B RID: 91
	public class StatCategory
	{
		// Token: 0x0600059D RID: 1437 RVA: 0x00014EDA File Offset: 0x000130DA
		public StatCategory(string id, IEnumerable<StatItem> items)
		{
			this.ID = id;
			this.Items = items;
		}

		// Token: 0x040002C2 RID: 706
		public readonly IEnumerable<StatItem> Items;

		// Token: 0x040002C3 RID: 707
		public readonly string ID;
	}
}
