using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000BE RID: 190
	public class HeirComingOfAgeFemaleSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x060013E8 RID: 5096 RVA: 0x0005C99B File Offset: 0x0005AB9B
		public Hero MentorHero { get; }

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x060013E9 RID: 5097 RVA: 0x0005C9A3 File Offset: 0x0005ABA3
		public Hero HeroCameOfAge { get; }

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x060013EA RID: 5098 RVA: 0x0005C9AB File Offset: 0x0005ABAB
		public override string SceneID
		{
			get
			{
				return "scn_hero_come_of_age_female";
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x060013EB RID: 5099 RVA: 0x0005C9B4 File Offset: 0x0005ABB4
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("HERO_NAME", this.HeroCameOfAge.Name);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_hero_came_of_age", null);
			}
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x0005CA10 File Offset: 0x0005AC10
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment equipment = this.MentorHero.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.MentorHero, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			string childStageEquipmentIDFromCulture = CampaignSceneNotificationHelper.GetChildStageEquipmentIDFromCulture(this.HeroCameOfAge.Culture);
			Equipment equipment2 = MBObjectManager.Instance.GetObject<MBEquipmentRoster>(childStageEquipmentIDFromCulture).DefaultEquipment.Clone(false);
			BodyProperties bodyProperties = new BodyProperties(new DynamicBodyProperties(6f, this.HeroCameOfAge.Weight, this.HeroCameOfAge.Build), this.HeroCameOfAge.StaticBodyProperties);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.HeroCameOfAge, equipment2, false, bodyProperties, uint.MaxValue, uint.MaxValue, false));
			BodyProperties bodyProperties2 = new BodyProperties(new DynamicBodyProperties(14f, this.HeroCameOfAge.Weight, this.HeroCameOfAge.Build), this.HeroCameOfAge.StaticBodyProperties);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.HeroCameOfAge, equipment2, false, bodyProperties2, uint.MaxValue, uint.MaxValue, false));
			Equipment equipment3 = this.HeroCameOfAge.BattleEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment3, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.HeroCameOfAge, equipment3, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			return list.ToArray();
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x0005CB5A File Offset: 0x0005AD5A
		public HeirComingOfAgeFemaleSceneNotificationItem(Hero mentorHero, Hero heroCameOfAge, CampaignTime creationTime)
		{
			this.MentorHero = mentorHero;
			this.HeroCameOfAge = heroCameOfAge;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x04000692 RID: 1682
		private readonly CampaignTime _creationCampaignTime;
	}
}
