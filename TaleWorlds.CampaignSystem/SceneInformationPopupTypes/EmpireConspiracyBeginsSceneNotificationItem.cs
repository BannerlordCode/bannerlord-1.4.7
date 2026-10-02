using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B7 RID: 183
	public class EmpireConspiracyBeginsSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x060013C3 RID: 5059 RVA: 0x0005C359 File Offset: 0x0005A559
		public Hero PlayerHero { get; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x060013C4 RID: 5060 RVA: 0x0005C361 File Offset: 0x0005A561
		public Kingdom Empire { get; }

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x060013C5 RID: 5061 RVA: 0x0005C369 File Offset: 0x0005A569
		public bool IsConspiracyAgainstEmpire { get; }

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x060013C6 RID: 5062 RVA: 0x0005C371 File Offset: 0x0005A571
		public override string SceneID
		{
			get
			{
				return "scn_empire_conspiracy_start_notification";
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x060013C7 RID: 5063 RVA: 0x0005C378 File Offset: 0x0005A578
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				if (this.IsConspiracyAgainstEmpire)
				{
					return GameTexts.FindText("str_empire_conspiracy_begins_antiempire", null);
				}
				return GameTexts.FindText("str_empire_conspiracy_begins_proempire", null);
			}
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x0005C3D1 File Offset: 0x0005A5D1
		public override Banner[] GetBanners()
		{
			return new Banner[] { this.Empire.Banner };
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x0005C3E8 File Offset: 0x0005A5E8
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			for (int i = 0; i < 8; i++)
			{
				Equipment equipment = MBObjectManager.Instance.GetObject<MBEquipmentRoster>("conspirator_cutscene_template").DefaultEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, false, false);
				CharacterObject facePropertiesFromAudienceIndex = this.GetFacePropertiesFromAudienceIndex(false, i);
				BodyProperties bodyProperties = facePropertiesFromAudienceIndex.GetBodyProperties(equipment, MBRandom.RandomInt(100));
				uint num = this._audienceColors[MBRandom.RandomInt(this._audienceColors.Length)];
				uint num2 = this._audienceColors[MBRandom.RandomInt(this._audienceColors.Length)];
				list.Add(new SceneNotificationData.SceneNotificationCharacter(facePropertiesFromAudienceIndex, equipment, bodyProperties, false, num, num2, false));
			}
			return list.ToArray();
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x0005C491 File Offset: 0x0005A691
		public EmpireConspiracyBeginsSceneNotificationItem(Hero playerHero, Kingdom empire, bool isConspiracyAgainstEmpire)
		{
			this.PlayerHero = playerHero;
			this.Empire = empire;
			this.IsConspiracyAgainstEmpire = isConspiracyAgainstEmpire;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x0005C4D0 File Offset: 0x0005A6D0
		private CharacterObject GetFacePropertiesFromAudienceIndex(bool playerWantsRestore, int audienceMemberIndex)
		{
			if (!playerWantsRestore)
			{
				return MBObjectManager.Instance.GetObject<CharacterObject>("villager_empire");
			}
			string text;
			switch (audienceMemberIndex % 8)
			{
			case 0:
				text = "villager_battania";
				break;
			case 1:
				text = "villager_khuzait";
				break;
			case 2:
				text = "villager_vlandia";
				break;
			case 3:
				text = "villager_aserai";
				break;
			case 4:
				text = "villager_battania";
				break;
			case 5:
				text = "villager_sturgia";
				break;
			default:
				text = "villager_battania";
				break;
			}
			return MBObjectManager.Instance.GetObject<CharacterObject>(text);
		}

		// Token: 0x04000681 RID: 1665
		private const int AudienceNumber = 8;

		// Token: 0x04000682 RID: 1666
		private readonly uint[] _audienceColors = new uint[] { 4278914065U, 4284308292U, 4281543757U, 4282199842U };

		// Token: 0x04000686 RID: 1670
		private readonly CampaignTime _creationCampaignTime;
	}
}
