using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000BA RID: 186
	public class ProEmpireConspiracyBeginsSceneNotificationItem : EmpireConspiracySupportsSceneNotificationItemBase
	{
		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x060013D4 RID: 5076 RVA: 0x0005C78C File Offset: 0x0005A98C
		public override TextObject TitleText
		{
			get
			{
				TextObject textObject = GameTexts.FindText("str_empire_conspiracy_supports_proempire", null);
				textObject.SetTextVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(CampaignTime.Now));
				textObject.SetTextVariable("YEAR", CampaignTime.Now.GetYear);
				return textObject;
			}
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x0005C7D3 File Offset: 0x0005A9D3
		public ProEmpireConspiracyBeginsSceneNotificationItem(Hero kingHero)
			: base(kingHero)
		{
		}
	}
}
