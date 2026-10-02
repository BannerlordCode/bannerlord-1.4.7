using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B1 RID: 177
	public class BecomeKingSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001395 RID: 5013 RVA: 0x0005B147 File Offset: 0x00059347
		public Hero NewLeaderHero { get; }

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001396 RID: 5014 RVA: 0x0005B14F File Offset: 0x0005934F
		public override string SceneID
		{
			get
			{
				return "scn_become_king_notification";
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001397 RID: 5015 RVA: 0x0005B158 File Offset: 0x00059358
		public override TextObject TitleText
		{
			get
			{
				TextObject textObject;
				if (this.NewLeaderHero.Clan.Kingdom.Culture.StringId.Equals("empire", StringComparison.InvariantCultureIgnoreCase))
				{
					textObject = GameTexts.FindText("str_become_king_empire", null);
				}
				else
				{
					TextObject textObject2 = (this.NewLeaderHero.IsFemale ? GameTexts.FindText("str_liege_title_female", this.NewLeaderHero.Clan.Kingdom.Culture.StringId) : GameTexts.FindText("str_liege_title", this.NewLeaderHero.Clan.Kingdom.Culture.StringId));
					textObject = GameTexts.FindText("str_become_king_nonempire", null);
					textObject.SetTextVariable("TITLE_NAME", textObject2);
				}
				textObject.SetTextVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				textObject.SetTextVariable("YEAR", this._creationCampaignTime.GetYear);
				textObject.SetTextVariable("KING_NAME", this.NewLeaderHero.Name);
				textObject.SetTextVariable("IS_KING_MALE", this.NewLeaderHero.IsFemale ? 0 : 1);
				return textObject;
			}
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x0005B271 File Offset: 0x00059471
		public override Banner[] GetBanners()
		{
			return new Banner[]
			{
				this.NewLeaderHero.Clan.Kingdom.Banner,
				this.NewLeaderHero.Clan.Kingdom.Banner
			};
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x0005B2AC File Offset: 0x000594AC
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			Equipment equipment = this.NewLeaderHero.CharacterObject.Equipment.Clone(true);
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			list.Add(new SceneNotificationData.SceneNotificationCharacter(this.NewLeaderHero.CharacterObject, equipment, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
			for (int i = 0; i < 14; i++)
			{
				CharacterObject characterObject = (this.IsAudienceFemale(i) ? this.NewLeaderHero.Clan.Kingdom.Culture.Townswoman : this.NewLeaderHero.Clan.Kingdom.Culture.Townsman);
				Equipment equipment2 = characterObject.FirstCivilianEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, true, false);
				uint color = BannerManager.Instance.ReadOnlyColorPalette.GetRandomElementInefficiently<KeyValuePair<int, BannerColor>>().Value.Color;
				uint color2 = BannerManager.Instance.ReadOnlyColorPalette.GetRandomElementInefficiently<KeyValuePair<int, BannerColor>>().Value.Color;
				list.Add(new SceneNotificationData.SceneNotificationCharacter(characterObject, equipment2, characterObject.GetBodyProperties(equipment2, MBRandom.RandomInt(100)), false, color, color2, false));
			}
			for (int j = 0; j < 2; j++)
			{
				list.Add(CampaignSceneNotificationHelper.GetBodyguardOfCulture(this.NewLeaderHero.Clan.Kingdom.MapFaction.Culture));
			}
			foreach (Hero hero in CampaignSceneNotificationHelper.GetMilitaryAudienceForHero(this.NewLeaderHero, false, false).Take<Hero>(4))
			{
				Equipment equipment3 = hero.CivilianEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment3, false, false);
				list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(hero, equipment3, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			}
			return list.ToArray();
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x0005B48C File Offset: 0x0005968C
		public BecomeKingSceneNotificationItem(Hero newLeaderHero)
		{
			this.NewLeaderHero = newLeaderHero;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x0005B4A6 File Offset: 0x000596A6
		private bool IsAudienceFemale(int indexOfAudience)
		{
			return indexOfAudience == 2 || indexOfAudience == 5 || indexOfAudience - 11 <= 2;
		}

		// Token: 0x0400066F RID: 1647
		private const int NumberOfAudience = 14;

		// Token: 0x04000670 RID: 1648
		private const int NumberOfGuards = 2;

		// Token: 0x04000671 RID: 1649
		private const int NumberOfCompanions = 4;

		// Token: 0x04000673 RID: 1651
		private readonly CampaignTime _creationCampaignTime;
	}
}
