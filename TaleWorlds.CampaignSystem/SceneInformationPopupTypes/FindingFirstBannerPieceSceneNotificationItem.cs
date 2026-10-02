using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000BB RID: 187
	public class FindingFirstBannerPieceSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x060013D6 RID: 5078 RVA: 0x0005C7DC File Offset: 0x0005A9DC
		public Hero PlayerHero { get; }

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x060013D7 RID: 5079 RVA: 0x0005C7E4 File Offset: 0x0005A9E4
		public override string SceneID
		{
			get
			{
				return "scn_first_banner_piece_notification";
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060013D8 RID: 5080 RVA: 0x0005C7EC File Offset: 0x0005A9EC
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_first_banner_piece_found", null);
			}
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x0005C831 File Offset: 0x0005AA31
		public override void OnCloseAction()
		{
			base.OnCloseAction();
			Action onCloseAction = this._onCloseAction;
			if (onCloseAction == null)
			{
				return;
			}
			onCloseAction();
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x0005C849 File Offset: 0x0005AA49
		public FindingFirstBannerPieceSceneNotificationItem(Hero playerHero, Action onCloseAction = null)
		{
			this.PlayerHero = playerHero;
			this._creationCampaignTime = CampaignTime.Now;
			this._onCloseAction = onCloseAction;
		}

		// Token: 0x0400068A RID: 1674
		private readonly Action _onCloseAction;

		// Token: 0x0400068B RID: 1675
		private readonly CampaignTime _creationCampaignTime;
	}
}
