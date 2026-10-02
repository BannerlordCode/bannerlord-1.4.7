using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper.PerkSelection
{
	// Token: 0x02000149 RID: 329
	public class PerkSelectedByPlayerEvent : EventBase
	{
		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06001F8B RID: 8075 RVA: 0x00073D28 File Offset: 0x00071F28
		// (set) Token: 0x06001F8C RID: 8076 RVA: 0x00073D30 File Offset: 0x00071F30
		public PerkObject SelectedPerk { get; private set; }

		// Token: 0x06001F8D RID: 8077 RVA: 0x00073D39 File Offset: 0x00071F39
		public PerkSelectedByPlayerEvent(PerkObject selectedPerk)
		{
			this.SelectedPerk = selectedPerk;
		}
	}
}
