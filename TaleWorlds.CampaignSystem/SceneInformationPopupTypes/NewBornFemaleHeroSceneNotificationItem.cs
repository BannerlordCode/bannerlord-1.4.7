using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C9 RID: 201
	public class NewBornFemaleHeroSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001444 RID: 5188 RVA: 0x0005E68E File Offset: 0x0005C88E
		public Hero MaleHero { get; }

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001445 RID: 5189 RVA: 0x0005E696 File Offset: 0x0005C896
		public Hero FemaleHero { get; }

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001446 RID: 5190 RVA: 0x0005E69E File Offset: 0x0005C89E
		public override string SceneID
		{
			get
			{
				return "scn_born_baby_female_hero";
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001447 RID: 5191 RVA: 0x0005E6A8 File Offset: 0x0005C8A8
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("MOTHER_NAME", this.FemaleHero.Name);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_baby_born_only_mother", null);
			}
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0005E704 File Offset: 0x0005C904
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			CharacterObject characterObject = CharacterObject.All.First<CharacterObject>((CharacterObject h) => h.StringId == "cutscene_midwife");
			Equipment equipment = this.MaleHero.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, true, false);
			Equipment equipment2 = this.FemaleHero.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, true, true);
			Equipment equipment3 = characterObject.FirstCivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment3, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.MaleHero, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.FemaleHero, equipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			list.Add(new SceneNotificationData.SceneNotificationCharacter(characterObject, equipment3, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
			return list.ToArray();
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x0005E7EC File Offset: 0x0005C9EC
		public NewBornFemaleHeroSceneNotificationItem(Hero maleHero, Hero femaleHero, CampaignTime creationTime)
		{
			this.MaleHero = maleHero;
			this.FemaleHero = femaleHero;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x040006C3 RID: 1731
		private readonly CampaignTime _creationCampaignTime;
	}
}
