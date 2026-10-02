using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B3 RID: 179
	public class ClanMemberPeaceDeathSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x060013A8 RID: 5032 RVA: 0x0005BBFF File Offset: 0x00059DFF
		public Hero DeadHero { get; }

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x060013A9 RID: 5033 RVA: 0x0005BC07 File Offset: 0x00059E07
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_family_member_death";
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x060013AA RID: 5034 RVA: 0x0005BC0E File Offset: 0x00059E0E
		// (set) Token: 0x060013AB RID: 5035 RVA: 0x0005BC16 File Offset: 0x00059E16
		public KillCharacterAction.KillCharacterActionDetail KillDetail { get; private set; }

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x060013AC RID: 5036 RVA: 0x0005BC20 File Offset: 0x00059E20
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("NAME", this.DeadHero.Name);
				if (this.KillDetail == KillCharacterAction.KillCharacterActionDetail.DiedInLabor)
				{
					return GameTexts.FindText("str_main_hero_battle_death_in_labor", null);
				}
				if (this.KillDetail == KillCharacterAction.KillCharacterActionDetail.Executed || this.KillDetail == KillCharacterAction.KillCharacterActionDetail.ExecutionAfterMapEvent)
				{
					return GameTexts.FindText("str_main_hero_battle_executed", null);
				}
				if (this.KillDetail == KillCharacterAction.KillCharacterActionDetail.Murdered)
				{
					return GameTexts.FindText("str_main_hero_battle_murdered", null);
				}
				return GameTexts.FindText("str_family_member_death", null);
			}
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x0005BCC2 File Offset: 0x00059EC2
		public override Banner[] GetBanners()
		{
			return new Banner[] { this.DeadHero.ClanBanner };
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x0005BCD8 File Offset: 0x00059ED8
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			Equipment equipment = this.DeadHero.CivilianEquipment.Clone(false);
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.DeadHero, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			foreach (Hero hero in CampaignSceneNotificationHelper.GetMilitaryAudienceForHero(this.DeadHero, true, false).Take<Hero>(5))
			{
				Equipment equipment2 = hero.CivilianEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, false, false);
				list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(hero, equipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			}
			return list.ToArray();
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x0005BDA8 File Offset: 0x00059FA8
		public ClanMemberPeaceDeathSceneNotificationItem(Hero deadHero, CampaignTime creationTime, KillCharacterAction.KillCharacterActionDetail killDetail)
		{
			this.DeadHero = deadHero;
			this._creationCampaignTime = creationTime;
			this.KillDetail = killDetail;
		}

		// Token: 0x04000674 RID: 1652
		private const int NumberOfAudienceHeroes = 5;

		// Token: 0x04000677 RID: 1655
		private readonly CampaignTime _creationCampaignTime;
	}
}
