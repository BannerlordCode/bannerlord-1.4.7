using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C1 RID: 193
	public class JoinKingdomSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x0600140A RID: 5130 RVA: 0x0005D468 File Offset: 0x0005B668
		public Clan NewMemberClan { get; }

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x0600140B RID: 5131 RVA: 0x0005D470 File Offset: 0x0005B670
		public Kingdom KingdomToUse { get; }

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x0600140C RID: 5132 RVA: 0x0005D478 File Offset: 0x0005B678
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_factionjoin";
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x0600140D RID: 5133 RVA: 0x0005D47F File Offset: 0x0005B67F
		public override SceneNotificationData.RelevantContextType RelevantContext
		{
			get
			{
				return SceneNotificationData.RelevantContextType.Any;
			}
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x0600140E RID: 5134 RVA: 0x0005D484 File Offset: 0x0005B684
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("CLAN_NAME", this.NewMemberClan.Name);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("KINGDOM_FORMALNAME", CampaignSceneNotificationHelper.GetFormalNameForKingdom(this.KingdomToUse));
				return GameTexts.FindText("str_new_faction_member", null);
			}
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0005D4F3 File Offset: 0x0005B6F3
		public override Banner[] GetBanners()
		{
			return new Banner[]
			{
				this.KingdomToUse.Banner,
				this.KingdomToUse.Banner
			};
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x0005D518 File Offset: 0x0005B718
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Hero leader = this.NewMemberClan.Leader;
			Equipment equipment = leader.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(leader, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			foreach (Hero hero in CampaignSceneNotificationHelper.GetMilitaryAudienceForKingdom(this.KingdomToUse, true).Take<Hero>(5))
			{
				Equipment equipment2 = hero.CivilianEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, true, false);
				list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(hero, equipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			}
			return list.ToArray();
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x0005D5EC File Offset: 0x0005B7EC
		public JoinKingdomSceneNotificationItem(Clan newMember, Kingdom kingdom)
		{
			this.NewMemberClan = newMember;
			this.KingdomToUse = kingdom;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x040006A3 RID: 1699
		private const int NumberOfKingdomMembers = 5;

		// Token: 0x040006A6 RID: 1702
		private readonly CampaignTime _creationCampaignTime;
	}
}
