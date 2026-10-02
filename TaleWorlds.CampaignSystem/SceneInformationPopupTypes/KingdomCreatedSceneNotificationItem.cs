using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C2 RID: 194
	public class KingdomCreatedSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001412 RID: 5138 RVA: 0x0005D60D File Offset: 0x0005B80D
		public Kingdom NewKingdom { get; }

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001413 RID: 5139 RVA: 0x0005D615 File Offset: 0x0005B815
		public override string SceneID
		{
			get
			{
				return "scn_kingdom_made";
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x0005D61C File Offset: 0x0005B81C
		public override bool PauseActiveState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001415 RID: 5141 RVA: 0x0005D620 File Offset: 0x0005B820
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("KINGDOM_NAME", this.NewKingdom.Name);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("LEADER_NAME", this.NewKingdom.Leader.Name);
				return GameTexts.FindText("str_kingdom_created", null);
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x0005D694 File Offset: 0x0005B894
		public override TextObject AffirmativeText
		{
			get
			{
				return GameTexts.FindText("str_ok", null);
			}
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x0005D6A1 File Offset: 0x0005B8A1
		public override Banner[] GetBanners()
		{
			return new Banner[]
			{
				this.NewKingdom.Banner,
				this.NewKingdom.Banner
			};
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x0005D6C8 File Offset: 0x0005B8C8
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Hero leader = this.NewKingdom.Leader;
			Equipment equipment = leader.BattleEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(leader, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			foreach (Hero hero in CampaignSceneNotificationHelper.GetMilitaryAudienceForKingdom(this.NewKingdom, false).Take<Hero>(5))
			{
				Equipment equipment2 = hero.CivilianEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, true, false);
				list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(hero, equipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			}
			return list.ToArray();
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x0005D79C File Offset: 0x0005B99C
		public KingdomCreatedSceneNotificationItem(Kingdom newKingdom)
		{
			this.NewKingdom = newKingdom;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x040006A7 RID: 1703
		private const int NumberOfKingdomMemberAudience = 5;

		// Token: 0x040006A9 RID: 1705
		private readonly CampaignTime _creationCampaignTime;
	}
}
