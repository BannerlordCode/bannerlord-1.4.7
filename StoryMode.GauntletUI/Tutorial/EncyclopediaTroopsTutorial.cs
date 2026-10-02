using System;
using SandBox.GauntletUI.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000037 RID: 55
	[Tutorial("EncyclopediaTroopsTutorial")]
	public class EncyclopediaTroopsTutorial : EncyclopediaPageTutorialBase
	{
		// Token: 0x0600010D RID: 269 RVA: 0x00003F36 File Offset: 0x00002136
		public EncyclopediaTroopsTutorial()
			: base(EncyclopediaPages.Unit, EncyclopediaPages.ListUnits)
		{
		}
	}
}
