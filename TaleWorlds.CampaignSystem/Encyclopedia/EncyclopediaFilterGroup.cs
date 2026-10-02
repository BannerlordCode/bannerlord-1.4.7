using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x02000173 RID: 371
	public class EncyclopediaFilterGroup : ViewModel
	{
		// Token: 0x06001B54 RID: 6996 RVA: 0x0008D8F1 File Offset: 0x0008BAF1
		public EncyclopediaFilterGroup(List<EncyclopediaFilterItem> filters, TextObject name)
		{
			this.Filters = filters;
			this.Name = name;
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06001B55 RID: 6997 RVA: 0x0008D907 File Offset: 0x0008BB07
		public Predicate<object> Predicate
		{
			get
			{
				return delegate(object item)
				{
					if (!this.Filters.Any<EncyclopediaFilterItem>((EncyclopediaFilterItem f) => f.IsActive))
					{
						return true;
					}
					foreach (EncyclopediaFilterItem encyclopediaFilterItem in this.Filters)
					{
						if (encyclopediaFilterItem.IsActive && encyclopediaFilterItem.Predicate(item))
						{
							return true;
						}
					}
					return false;
				};
			}
		}

		// Token: 0x04000931 RID: 2353
		public readonly List<EncyclopediaFilterItem> Filters;

		// Token: 0x04000932 RID: 2354
		public readonly TextObject Name;
	}
}
