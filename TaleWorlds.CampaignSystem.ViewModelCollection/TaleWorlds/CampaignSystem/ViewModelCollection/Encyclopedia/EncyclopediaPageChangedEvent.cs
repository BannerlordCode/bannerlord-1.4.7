using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia
{
	// Token: 0x020000C4 RID: 196
	public class EncyclopediaPageChangedEvent : EventBase
	{
		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x0600132B RID: 4907 RVA: 0x0004DBBB File Offset: 0x0004BDBB
		// (set) Token: 0x0600132C RID: 4908 RVA: 0x0004DBC3 File Offset: 0x0004BDC3
		public EncyclopediaPages NewPage { get; private set; }

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x0600132D RID: 4909 RVA: 0x0004DBCC File Offset: 0x0004BDCC
		// (set) Token: 0x0600132E RID: 4910 RVA: 0x0004DBD4 File Offset: 0x0004BDD4
		public bool NewPageHasHiddenInformation { get; private set; }

		// Token: 0x0600132F RID: 4911 RVA: 0x0004DBDD File Offset: 0x0004BDDD
		public EncyclopediaPageChangedEvent(EncyclopediaPages newPage, bool hasHiddenInformation = false)
		{
			this.NewPage = newPage;
			this.NewPageHasHiddenInformation = hasHiddenInformation;
		}
	}
}
