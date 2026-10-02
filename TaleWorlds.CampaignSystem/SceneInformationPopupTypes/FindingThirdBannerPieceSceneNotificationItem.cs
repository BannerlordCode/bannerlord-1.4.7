using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000BD RID: 189
	public class FindingThirdBannerPieceSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x0005C8F1 File Offset: 0x0005AAF1
		public override string SceneID
		{
			get
			{
				return "scn_third_banner_piece_notification";
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x060013E1 RID: 5089 RVA: 0x0005C8F8 File Offset: 0x0005AAF8
		public override bool IsAffirmativeOptionShown { get; }

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x060013E2 RID: 5090 RVA: 0x0005C900 File Offset: 0x0005AB00
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_third_banner_piece_found", null);
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x060013E3 RID: 5091 RVA: 0x0005C945 File Offset: 0x0005AB45
		public override TextObject AffirmativeTitleText
		{
			get
			{
				return GameTexts.FindText("str_third_banner_piece_found_assembled", null);
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x060013E4 RID: 5092 RVA: 0x0005C952 File Offset: 0x0005AB52
		public override TextObject AffirmativeText
		{
			get
			{
				return new TextObject("{=6mgapvxb}Assemble", null);
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x060013E5 RID: 5093 RVA: 0x0005C95F File Offset: 0x0005AB5F
		public override TextObject AffirmativeDescriptionText
		{
			get
			{
				return new TextObject("{=IRLB42FY}Assemble the dragon banner!", null);
			}
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x0005C96C File Offset: 0x0005AB6C
		public override Banner[] GetBanners()
		{
			return new Banner[] { Hero.MainHero.ClanBanner };
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x0005C981 File Offset: 0x0005AB81
		public FindingThirdBannerPieceSceneNotificationItem()
		{
			this.IsAffirmativeOptionShown = true;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x0400068F RID: 1679
		private readonly CampaignTime _creationCampaignTime;
	}
}
