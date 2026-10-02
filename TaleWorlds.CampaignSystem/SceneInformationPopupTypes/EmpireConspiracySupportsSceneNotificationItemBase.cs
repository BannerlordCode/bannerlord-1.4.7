using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B8 RID: 184
	public abstract class EmpireConspiracySupportsSceneNotificationItemBase : SceneNotificationData
	{
		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x060013CC RID: 5068 RVA: 0x0005C555 File Offset: 0x0005A755
		public Hero King { get; }

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x060013CD RID: 5069 RVA: 0x0005C55D File Offset: 0x0005A75D
		public override string SceneID
		{
			get
			{
				return "scn_empire_conspiracy_supports_notification";
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x060013CE RID: 5070 RVA: 0x0005C564 File Offset: 0x0005A764
		public override TextObject AffirmativeText
		{
			get
			{
				return GameTexts.FindText("str_ok", null);
			}
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x0005C571 File Offset: 0x0005A771
		public override Banner[] GetBanners()
		{
			return new Banner[]
			{
				this.King.MapFaction.Banner,
				this.King.MapFaction.Banner
			};
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x0005C5A0 File Offset: 0x0005A7A0
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment equipment = this.King.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.King, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			CharacterObject @object = MBObjectManager.Instance.GetObject<CharacterObject>("villager_battania");
			Equipment equipment2 = MBObjectManager.Instance.GetObject<MBEquipmentRoster>("conspirator_cutscene_template").DefaultEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, false, false);
			BodyProperties bodyProperties = @object.GetBodyProperties(equipment2, MBRandom.RandomInt(100));
			list.Add(new SceneNotificationData.SceneNotificationCharacter(@object, equipment2, bodyProperties, false, 0U, 0U, false));
			bodyProperties = @object.GetBodyProperties(equipment2, MBRandom.RandomInt(100));
			list.Add(new SceneNotificationData.SceneNotificationCharacter(@object, equipment2, bodyProperties, false, 0U, 0U, false));
			bodyProperties = @object.GetBodyProperties(equipment2, MBRandom.RandomInt(100));
			list.Add(new SceneNotificationData.SceneNotificationCharacter(@object, equipment2, bodyProperties, false, 0U, 0U, false));
			list.Add(CampaignSceneNotificationHelper.GetBodyguardOfCulture(this.King.MapFaction.Culture));
			list.Add(CampaignSceneNotificationHelper.GetBodyguardOfCulture(this.King.MapFaction.Culture));
			return list.ToArray();
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x0005C6BF File Offset: 0x0005A8BF
		protected EmpireConspiracySupportsSceneNotificationItemBase(Hero kingHero)
		{
			this.King = kingHero;
		}
	}
}
