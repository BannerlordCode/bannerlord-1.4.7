using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper.PerkSelection
{
	// Token: 0x0200014A RID: 330
	public class PerkSelectionToggleEvent : EventBase
	{
		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06001F8E RID: 8078 RVA: 0x00073D48 File Offset: 0x00071F48
		// (set) Token: 0x06001F8F RID: 8079 RVA: 0x00073D50 File Offset: 0x00071F50
		public bool IsCurrentlyActive { get; private set; }

		// Token: 0x06001F90 RID: 8080 RVA: 0x00073D59 File Offset: 0x00071F59
		public PerkSelectionToggleEvent(bool isCurrentlyActive)
		{
			this.IsCurrentlyActive = isCurrentlyActive;
		}
	}
}
