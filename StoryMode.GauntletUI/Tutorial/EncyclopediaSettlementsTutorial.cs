using System;
using SandBox.GauntletUI.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000036 RID: 54
	[Tutorial("EncyclopediaSettlementsTutorial")]
	public class EncyclopediaSettlementsTutorial : EncyclopediaPageTutorialBase
	{
		// Token: 0x0600010C RID: 268 RVA: 0x00003F2B File Offset: 0x0000212B
		public EncyclopediaSettlementsTutorial()
			: base(EncyclopediaPages.Settlement, EncyclopediaPages.ListSettlements)
		{
		}
	}
}
