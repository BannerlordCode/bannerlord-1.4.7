using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C3 RID: 195
	public class KingdomDestroyedSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x0005D7B6 File Offset: 0x0005B9B6
		public Kingdom DestroyedKingdom { get; }

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x0005D7BE File Offset: 0x0005B9BE
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_enemykingdom_destroyed";
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x0600141C RID: 5148 RVA: 0x0005D7C8 File Offset: 0x0005B9C8
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("FORMAL_NAME", CampaignSceneNotificationHelper.GetFormalNameForKingdom(this.DestroyedKingdom));
				return GameTexts.FindText("str_kingdom_destroyed_scene_notification", null);
			}
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0005D822 File Offset: 0x0005BA22
		public override Banner[] GetBanners()
		{
			return new Banner[] { this.DestroyedKingdom.Banner };
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x0005D838 File Offset: 0x0005BA38
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			for (int i = 0; i < 2; i++)
			{
				CharacterObject randomTroopForCulture = CampaignSceneNotificationHelper.GetRandomTroopForCulture(this.DestroyedKingdom.Culture);
				Equipment equipment = randomTroopForCulture.FirstBattleEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, false, false);
				BodyProperties bodyProperties = randomTroopForCulture.GetBodyProperties(equipment, MBRandom.RandomInt(100));
				list.Add(new SceneNotificationData.SceneNotificationCharacter(randomTroopForCulture, equipment, bodyProperties, false, uint.MaxValue, uint.MaxValue, false));
			}
			return list.ToArray();
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x0005D8A7 File Offset: 0x0005BAA7
		public KingdomDestroyedSceneNotificationItem(Kingdom destroyedKingdom, CampaignTime creationTime)
		{
			this.DestroyedKingdom = destroyedKingdom;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x040006AA RID: 1706
		private const int NumberOfDeadTroops = 2;

		// Token: 0x040006AC RID: 1708
		private readonly CampaignTime _creationCampaignTime;
	}
}
