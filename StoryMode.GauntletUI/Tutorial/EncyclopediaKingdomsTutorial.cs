using System;
using SandBox.GauntletUI.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000038 RID: 56
	[Tutorial("EncyclopediaKingdomsTutorial")]
	public class EncyclopediaKingdomsTutorial : EncyclopediaPageTutorialBase
	{
		// Token: 0x0600010E RID: 270 RVA: 0x00003F41 File Offset: 0x00002141
		public EncyclopediaKingdomsTutorial()
			: base(EncyclopediaPages.Kingdom, EncyclopediaPages.ListKingdoms)
		{
		}
	}
}
