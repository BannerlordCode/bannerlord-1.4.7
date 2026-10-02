using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B5 RID: 181
	public class DeathOldAgeSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x060013B6 RID: 5046 RVA: 0x0005BF2A File Offset: 0x0005A12A
		public Hero DeadHero { get; }

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x060013B7 RID: 5047 RVA: 0x0005BF32 File Offset: 0x0005A132
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_death_old_age";
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x060013B8 RID: 5048 RVA: 0x0005BF3C File Offset: 0x0005A13C
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("NAME", this.DeadHero.Name);
				return GameTexts.FindText("str_died_of_old_age", null);
			}
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x0005BF96 File Offset: 0x0005A196
		public override Banner[] GetBanners()
		{
			return new Banner[] { this.DeadHero.ClanBanner };
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x0005BFAC File Offset: 0x0005A1AC
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment equipment = this.DeadHero.CivilianEquipment.Clone(false);
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

		// Token: 0x060013BB RID: 5051 RVA: 0x0005C07C File Offset: 0x0005A27C
		public DeathOldAgeSceneNotificationItem(Hero deadHero)
		{
			this.DeadHero = deadHero;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x0400067B RID: 1659
		private const int NumberOfAudienceHeroes = 5;

		// Token: 0x0400067D RID: 1661
		private readonly CampaignTime _creationCampaignTime;
	}
}
