using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000CA RID: 202
	public class NewBornSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x0600144A RID: 5194 RVA: 0x0005E809 File Offset: 0x0005CA09
		public Hero MaleHero { get; }

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x0600144B RID: 5195 RVA: 0x0005E811 File Offset: 0x0005CA11
		public Hero FemaleHero { get; }

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x0600144C RID: 5196 RVA: 0x0005E819 File Offset: 0x0005CA19
		public override string SceneID
		{
			get
			{
				return "scn_born_baby";
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x0600144D RID: 5197 RVA: 0x0005E820 File Offset: 0x0005CA20
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("FATHER_NAME", this.MaleHero.Name);
				GameTexts.SetVariable("MOTHER_NAME", this.FemaleHero.Name);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_baby_born", null);
			}
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0005E890 File Offset: 0x0005CA90
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

		// Token: 0x0600144F RID: 5199 RVA: 0x0005E978 File Offset: 0x0005CB78
		public NewBornSceneNotificationItem(Hero maleHero, Hero femaleHero, CampaignTime creationTime)
		{
			this.MaleHero = maleHero;
			this.FemaleHero = femaleHero;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x040006C6 RID: 1734
		private readonly CampaignTime _creationCampaignTime;
	}
}
