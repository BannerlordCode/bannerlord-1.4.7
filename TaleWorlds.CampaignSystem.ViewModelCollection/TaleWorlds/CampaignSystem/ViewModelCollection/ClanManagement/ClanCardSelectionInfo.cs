using System;
using System.Collections.Generic;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x02000120 RID: 288
	public readonly struct ClanCardSelectionInfo
	{
		// Token: 0x06001A5D RID: 6749 RVA: 0x00063809 File Offset: 0x00061A09
		public ClanCardSelectionInfo(TextObject title, IEnumerable<ClanCardSelectionItemInfo> items, Action<List<object>, Action> onClosedAction, bool isMultiSelection, int minimumSelection = 1, int maximumSelection = 0)
		{
			this.Title = title;
			this.Items = items;
			this.OnClosedAction = onClosedAction;
			this.IsMultiSelection = isMultiSelection;
			this.MinimumSelection = minimumSelection;
			this.MaximumSelection = maximumSelection;
		}

		// Token: 0x04000C33 RID: 3123
		public readonly TextObject Title;

		// Token: 0x04000C34 RID: 3124
		public readonly IEnumerable<ClanCardSelectionItemInfo> Items;

		// Token: 0x04000C35 RID: 3125
		public readonly Action<List<object>, Action> OnClosedAction;

		// Token: 0x04000C36 RID: 3126
		public readonly bool IsMultiSelection;

		// Token: 0x04000C37 RID: 3127
		public readonly int MinimumSelection;

		// Token: 0x04000C38 RID: 3128
		public readonly int MaximumSelection;
	}
}
