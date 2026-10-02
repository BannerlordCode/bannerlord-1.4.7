using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party
{
	// Token: 0x0200002E RID: 46
	public class PlayerToggledUpgradePopupEvent : EventBase
	{
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x0001BADA File Offset: 0x00019CDA
		// (set) Token: 0x060004AC RID: 1196 RVA: 0x0001BAE2 File Offset: 0x00019CE2
		public bool IsOpened { get; private set; }

		// Token: 0x060004AD RID: 1197 RVA: 0x0001BAEB File Offset: 0x00019CEB
		public PlayerToggledUpgradePopupEvent(bool isOpened)
		{
			this.IsOpened = isOpened;
		}
	}
}
