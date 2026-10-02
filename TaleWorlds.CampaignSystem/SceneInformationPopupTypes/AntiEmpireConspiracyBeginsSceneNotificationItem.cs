using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B9 RID: 185
	public class AntiEmpireConspiracyBeginsSceneNotificationItem : EmpireConspiracySupportsSceneNotificationItemBase
	{
		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x060013D2 RID: 5074 RVA: 0x0005C6D0 File Offset: 0x0005A8D0
		public override TextObject TitleText
		{
			get
			{
				List<TextObject> list = new List<TextObject>();
				foreach (Kingdom kingdom in this._antiEmpireFactions)
				{
					list.Add(kingdom.InformalName);
				}
				TextObject textObject = GameTexts.FindText("str_empire_conspiracy_supports_antiempire", null);
				textObject.SetTextVariable("FACTION_NAMES", GameTexts.GameTextHelper.MergeTextObjectsWithComma(list, true));
				textObject.SetTextVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(CampaignTime.Now));
				textObject.SetTextVariable("YEAR", CampaignTime.Now.GetYear);
				return textObject;
			}
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0005C77C File Offset: 0x0005A97C
		public AntiEmpireConspiracyBeginsSceneNotificationItem(Hero kingHero, List<Kingdom> antiEmpireFactions)
			: base(kingHero)
		{
			this._antiEmpireFactions = antiEmpireFactions;
		}

		// Token: 0x04000688 RID: 1672
		private readonly List<Kingdom> _antiEmpireFactions;
	}
}
