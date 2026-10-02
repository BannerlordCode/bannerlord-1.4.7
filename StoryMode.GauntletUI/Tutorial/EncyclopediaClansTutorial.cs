using System;
using SandBox.GauntletUI.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000039 RID: 57
	[Tutorial("EncyclopediaClansTutorial")]
	public class EncyclopediaClansTutorial : EncyclopediaPageTutorialBase
	{
		// Token: 0x0600010F RID: 271 RVA: 0x00003F4C File Offset: 0x0000214C
		public EncyclopediaClansTutorial()
			: base(EncyclopediaPages.Clan, EncyclopediaPages.ListClans)
		{
		}
	}
}
