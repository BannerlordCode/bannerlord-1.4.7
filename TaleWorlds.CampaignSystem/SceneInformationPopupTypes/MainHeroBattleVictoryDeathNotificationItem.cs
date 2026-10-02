using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C5 RID: 197
	public class MainHeroBattleVictoryDeathNotificationItem : SceneNotificationData
	{
		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x0005DA20 File Offset: 0x0005BC20
		public Hero DeadHero { get; }

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001427 RID: 5159 RVA: 0x0005DA28 File Offset: 0x0005BC28
		public List<CharacterObject> EncounterAllyCharacters { get; }

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001428 RID: 5160 RVA: 0x0005DA30 File Offset: 0x0005BC30
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_main_hero_battle_victory_death";
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001429 RID: 5161 RVA: 0x0005DA38 File Offset: 0x0005BC38
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

		// Token: 0x0600142A RID: 5162 RVA: 0x0005DA94 File Offset: 0x0005BC94
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment equipment = this.DeadHero.BattleEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.DeadHero, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			for (int i = 0; i < 2; i++)
			{
				CharacterObject randomTroopForCulture = CampaignSceneNotificationHelper.GetRandomTroopForCulture(this.DeadHero.MapFaction.Culture);
				Equipment equipment2 = randomTroopForCulture.FirstBattleEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, false, false);
				BodyProperties bodyProperties = randomTroopForCulture.GetBodyProperties(equipment2, MBRandom.RandomInt(100));
				list.Add(new SceneNotificationData.SceneNotificationCharacter(randomTroopForCulture, equipment2, bodyProperties, false, uint.MaxValue, uint.MaxValue, false));
			}
			List<CharacterObject> encounterAllyCharacters = this.EncounterAllyCharacters;
			foreach (CharacterObject characterObject in ((encounterAllyCharacters != null) ? encounterAllyCharacters.Take<CharacterObject>(3) : null))
			{
				if (characterObject.IsHero)
				{
					Equipment equipment3 = characterObject.HeroObject.BattleEquipment.Clone(false);
					CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment3, false, false);
					list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(characterObject.HeroObject, equipment3, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
				}
				else
				{
					Equipment equipment4 = characterObject.FirstBattleEquipment.Clone(false);
					CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment4, false, false);
					list.Add(new SceneNotificationData.SceneNotificationCharacter(characterObject, equipment4, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0005DC18 File Offset: 0x0005BE18
		public MainHeroBattleVictoryDeathNotificationItem(Hero deadHero, List<CharacterObject> encounterAllyCharacters)
		{
			this.DeadHero = deadHero;
			this.EncounterAllyCharacters = encounterAllyCharacters;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x040006B1 RID: 1713
		private const int NumberOfCorpses = 2;

		// Token: 0x040006B2 RID: 1714
		private const int NumberOfCompanions = 3;

		// Token: 0x040006B5 RID: 1717
		private readonly CampaignTime _creationCampaignTime;
	}
}
