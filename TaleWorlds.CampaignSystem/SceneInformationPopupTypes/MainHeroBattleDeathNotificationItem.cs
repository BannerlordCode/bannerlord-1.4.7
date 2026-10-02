using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C4 RID: 196
	public class MainHeroBattleDeathNotificationItem : SceneNotificationData
	{
		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001420 RID: 5152 RVA: 0x0005D8BD File Offset: 0x0005BABD
		public Hero DeadHero { get; }

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001421 RID: 5153 RVA: 0x0005D8C5 File Offset: 0x0005BAC5
		public CultureObject KillerCulture { get; }

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x0005D8CD File Offset: 0x0005BACD
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_main_hero_battle_death";
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001423 RID: 5155 RVA: 0x0005D8D4 File Offset: 0x0005BAD4
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("NAME", this.DeadHero.Name);
				return GameTexts.FindText("str_main_hero_battle_death", null);
			}
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x0005D930 File Offset: 0x0005BB30
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment equipment = this.DeadHero.BattleEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.DeadHero, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			for (int i = 0; i < 23; i++)
			{
				CharacterObject randomTroopForCulture = CampaignSceneNotificationHelper.GetRandomTroopForCulture((this.KillerCulture != null && (float)i > 11.5f) ? this.KillerCulture : this.DeadHero.MapFaction.Culture);
				Equipment equipment2 = randomTroopForCulture.FirstBattleEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, false, false);
				BodyProperties bodyProperties = randomTroopForCulture.GetBodyProperties(equipment2, MBRandom.RandomInt(100));
				list.Add(new SceneNotificationData.SceneNotificationCharacter(randomTroopForCulture, equipment2, bodyProperties, false, uint.MaxValue, uint.MaxValue, false));
			}
			return list.ToArray();
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x0005D9FF File Offset: 0x0005BBFF
		public MainHeroBattleDeathNotificationItem(Hero deadHero, CultureObject killerCulture = null)
		{
			this.DeadHero = deadHero;
			this.KillerCulture = killerCulture;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x040006AD RID: 1709
		private const int NumberOfCorpses = 23;

		// Token: 0x040006B0 RID: 1712
		private readonly CampaignTime _creationCampaignTime;
	}
}
