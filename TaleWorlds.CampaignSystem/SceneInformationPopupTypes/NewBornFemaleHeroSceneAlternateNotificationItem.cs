using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C8 RID: 200
	public class NewBornFemaleHeroSceneAlternateNotificationItem : SceneNotificationData
	{
		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x0005E536 File Offset: 0x0005C736
		public Hero MaleHero { get; }

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x0600143F RID: 5183 RVA: 0x0005E53E File Offset: 0x0005C73E
		public Hero FemaleHero { get; }

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x0005E546 File Offset: 0x0005C746
		public override string SceneID
		{
			get
			{
				return "scn_born_baby_female_hero2";
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001441 RID: 5185 RVA: 0x0005E550 File Offset: 0x0005C750
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("MOTHER_NAME", this.FemaleHero.Name);
				return GameTexts.FindText("str_baby_born_only_mother", null);
			}
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x0005E5AC File Offset: 0x0005C7AC
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment equipment = this.FemaleHero.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, true, true);
			CharacterObject characterObject = CharacterObject.All.First<CharacterObject>((CharacterObject h) => h.StringId == "cutscene_midwife");
			Equipment equipment2 = characterObject.FirstCivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, false, false);
			list.Add(new SceneNotificationData.SceneNotificationCharacter(null, null, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.FemaleHero, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			list.Add(new SceneNotificationData.SceneNotificationCharacter(characterObject, equipment2, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
			return list.ToArray();
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x0005E671 File Offset: 0x0005C871
		public NewBornFemaleHeroSceneAlternateNotificationItem(Hero maleHero, Hero femaleHero, CampaignTime creationTime)
		{
			this.MaleHero = maleHero;
			this.FemaleHero = femaleHero;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x040006C0 RID: 1728
		private readonly CampaignTime _creationCampaignTime;
	}
}
