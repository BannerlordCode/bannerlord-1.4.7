using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B6 RID: 182
	public class DeclareDragonBannerSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x060013BC RID: 5052 RVA: 0x0005C096 File Offset: 0x0005A296
		public bool PlayerWantsToRestore { get; }

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x060013BD RID: 5053 RVA: 0x0005C09E File Offset: 0x0005A29E
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_declare_dragon_banner";
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x060013BE RID: 5054 RVA: 0x0005C0A8 File Offset: 0x0005A2A8
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("PLAYER_WANTS_RESTORE", this.PlayerWantsToRestore ? 1 : 0);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_declare_dragon_banner", null);
			}
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x0005C103 File Offset: 0x0005A303
		public override Banner[] GetBanners()
		{
			return new Banner[] { Hero.MainHero.ClanBanner };
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x0005C118 File Offset: 0x0005A318
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			IOrderedEnumerable<Hero> orderedEnumerable = from h in Hero.MainHero.Clan.Heroes
				where !h.IsChild && h.IsAlive && h != Hero.MainHero
				orderby h.Level
				select h;
			for (int i = 0; i < 17; i++)
			{
				SceneNotificationData.SceneNotificationCharacter characterAtIndex = this.GetCharacterAtIndex(i, orderedEnumerable);
				list.Add(characterAtIndex);
			}
			return list.ToArray();
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x0005C1A6 File Offset: 0x0005A3A6
		public DeclareDragonBannerSceneNotificationItem(bool playerWantsToRestore)
		{
			this.PlayerWantsToRestore = playerWantsToRestore;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x0005C1C0 File Offset: 0x0005A3C0
		private SceneNotificationData.SceneNotificationCharacter GetCharacterAtIndex(int index, IOrderedEnumerable<Hero> clanHeroesPool)
		{
			bool flag = false;
			int num = -1;
			string text = string.Empty;
			switch (index)
			{
			case 0:
				text = "battanian_picked_warrior";
				num = 0;
				break;
			case 1:
				text = "imperial_infantryman";
				break;
			case 2:
				text = "imperial_veteran_infantryman";
				break;
			case 3:
				text = "sturgian_warrior";
				num = 1;
				break;
			case 4:
				text = "imperial_menavliaton";
				break;
			case 5:
				text = "sturgian_ulfhednar";
				num = 2;
				break;
			case 6:
				text = "aserai_recruit";
				break;
			case 7:
				text = "aserai_skirmisher";
				break;
			case 8:
				text = "aserai_veteran_faris";
				break;
			case 9:
				text = "imperial_legionary";
				num = 3;
				break;
			case 10:
				text = "mountain_bandits_bandit";
				break;
			case 11:
				text = "mountain_bandits_chief";
				break;
			case 12:
				text = "forest_people_tier_3";
				num = 4;
				break;
			case 13:
				text = "mountain_bandits_raider";
				break;
			case 14:
				flag = true;
				break;
			case 15:
				text = "vlandian_pikeman";
				break;
			case 16:
				text = "vlandian_voulgier";
				break;
			}
			uint num2 = uint.MaxValue;
			uint num3 = uint.MaxValue;
			CharacterObject characterObject;
			if (flag)
			{
				characterObject = CharacterObject.PlayerCharacter;
				num2 = Hero.MainHero.MapFaction.Color;
				num3 = Hero.MainHero.MapFaction.Color2;
			}
			else if (num != -1 && clanHeroesPool.ElementAtOrDefault<Hero>(num) != null)
			{
				Hero hero = clanHeroesPool.ElementAtOrDefault<Hero>(num);
				characterObject = hero.CharacterObject;
				num2 = hero.MapFaction.Color;
				num3 = hero.MapFaction.Color2;
			}
			else
			{
				characterObject = MBObjectManager.Instance.GetObject<CharacterObject>(text);
			}
			Equipment equipment = characterObject.FirstBattleEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, true, false);
			return new SceneNotificationData.SceneNotificationCharacter(characterObject, equipment, default(BodyProperties), false, num2, num3, false);
		}

		// Token: 0x0400067E RID: 1662
		private const int NumberOfCharacters = 17;

		// Token: 0x04000680 RID: 1664
		private readonly CampaignTime _creationCampaignTime;
	}
}
