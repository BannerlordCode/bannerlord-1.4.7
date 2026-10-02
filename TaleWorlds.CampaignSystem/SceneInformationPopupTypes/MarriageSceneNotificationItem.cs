using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C6 RID: 198
	public class MarriageSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x0600142C RID: 5164 RVA: 0x0005DC39 File Offset: 0x0005BE39
		public Hero GroomHero { get; }

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x0600142D RID: 5165 RVA: 0x0005DC41 File Offset: 0x0005BE41
		public Hero BrideHero { get; }

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x0600142E RID: 5166 RVA: 0x0005DC49 File Offset: 0x0005BE49
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_wedding";
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x0600142F RID: 5167 RVA: 0x0005DC50 File Offset: 0x0005BE50
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				Hero hero = ((this.GroomHero == Hero.MainHero) ? this.GroomHero : this.BrideHero);
				Hero hero2 = ((hero == this.GroomHero) ? this.BrideHero : this.GroomHero);
				GameTexts.SetVariable("FIRST_HERO", hero.Name);
				GameTexts.SetVariable("SECOND_HERO", hero2.Name);
				return GameTexts.FindText("str_marriage_notification", null);
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x0005DCE9 File Offset: 0x0005BEE9
		public override SceneNotificationData.RelevantContextType RelevantContext { get; }

		// Token: 0x06001431 RID: 5169 RVA: 0x0005DCF4 File Offset: 0x0005BEF4
		public override Banner[] GetBanners()
		{
			return new Banner[]
			{
				(this.GroomHero.Father != null) ? this.GroomHero.Father.ClanBanner : this.GroomHero.ClanBanner,
				(this.BrideHero.Father != null) ? this.BrideHero.Father.ClanBanner : this.BrideHero.ClanBanner,
				(this.GroomHero.Father != null) ? this.GroomHero.Father.ClanBanner : this.GroomHero.ClanBanner,
				(this.BrideHero.Father != null) ? this.BrideHero.Father.ClanBanner : this.BrideHero.ClanBanner
			};
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x0005DDBC File Offset: 0x0005BFBC
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment equipment = this.GroomHero.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.GroomHero, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			Equipment equipment2;
			if (this.BrideHero.Culture.MarriageBrideEquipmentRoster != null)
			{
				equipment2 = this.BrideHero.Culture.MarriageBrideEquipmentRoster.DefaultEquipment.Clone(false);
			}
			else
			{
				equipment2 = MBEquipmentRoster.EmptyEquipment.Clone(false);
				Debug.FailedAssert("Could not find marriage equipment for culture: " + this.BrideHero.Culture.StringId + ".", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\SceneInformationPopupTypes\\MarriageSceneNotificationItem.cs", "GetSceneNotificationCharacters", 61);
			}
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.BrideHero, equipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			CharacterObject @object = MBObjectManager.Instance.GetObject<CharacterObject>("cutscene_monk");
			Equipment equipment3 = @object.Equipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment3, false, false);
			list.Add(new SceneNotificationData.SceneNotificationCharacter(@object, equipment3, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
			List<Hero> audienceMembers = this.GetAudienceMembers(this.BrideHero, this.GroomHero);
			for (int i = 0; i < audienceMembers.Count; i++)
			{
				Hero hero = audienceMembers[i];
				if (hero != null)
				{
					Equipment equipment4 = hero.CivilianEquipment.Clone(false);
					CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment4, false, false);
					list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(hero, equipment4, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
				}
				else
				{
					list.Add(new SceneNotificationData.SceneNotificationCharacter(null, null, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
				}
			}
			return list.ToArray();
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0005DF73 File Offset: 0x0005C173
		public MarriageSceneNotificationItem(Hero groomHero, Hero brideHero, CampaignTime creationTime, SceneNotificationData.RelevantContextType relevantContextType = SceneNotificationData.RelevantContextType.Any)
		{
			this.GroomHero = groomHero;
			this.BrideHero = brideHero;
			this.RelevantContext = relevantContextType;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x0005DF98 File Offset: 0x0005C198
		private List<Hero> GetAudienceMembers(Hero brideHero, Hero groomHero)
		{
			Queue<Hero> groomSide = new Queue<Hero>();
			Queue<Hero> brideSide = new Queue<Hero>();
			List<Hero> list = new List<Hero>();
			Hero mother = groomHero.Mother;
			if (mother != null && mother.IsAlive)
			{
				groomSide.Enqueue(groomHero.Mother);
			}
			Hero father = groomHero.Father;
			if (father != null && father.IsAlive)
			{
				groomSide.Enqueue(groomHero.Father);
			}
			if (groomHero.Siblings != null)
			{
				foreach (Hero hero in groomHero.Siblings.Where<Hero>((Hero s) => s.IsAlive && !s.IsChild))
				{
					groomSide.Enqueue(hero);
				}
			}
			if (groomHero.Children != null)
			{
				foreach (Hero hero2 in groomHero.Children.Where<Hero>((Hero s) => s.IsAlive && !s.IsChild))
				{
					groomSide.Enqueue(hero2);
				}
			}
			Hero mother2 = brideHero.Mother;
			if (mother2 != null && mother2.IsAlive)
			{
				brideSide.Enqueue(brideHero.Mother);
			}
			Hero father2 = brideHero.Father;
			if (father2 != null && father2.IsAlive)
			{
				brideSide.Enqueue(brideHero.Father);
			}
			if (brideHero.Siblings != null)
			{
				foreach (Hero hero3 in brideHero.Siblings.Where<Hero>((Hero s) => s.IsAlive && !s.IsChild))
				{
					brideSide.Enqueue(hero3);
				}
			}
			if (brideHero.Children != null)
			{
				foreach (Hero hero4 in brideHero.Children.Where<Hero>((Hero s) => s.IsAlive && !s.IsChild))
				{
					brideSide.Enqueue(hero4);
				}
			}
			if (groomSide.Count < 3)
			{
				IEnumerable<Hero> allAliveHeroes = Hero.AllAliveHeroes;
				Func<Hero, bool> <>9__4;
				Func<Hero, bool> func;
				if ((func = <>9__4) == null)
				{
					func = (<>9__4 = (Hero h) => h.IsLord && !h.IsChild && h != groomHero && h != brideHero && h.IsFriend(groomHero) && !brideSide.Contains(h));
				}
				foreach (Hero hero5 in allAliveHeroes.Where<Hero>(func).Take<Hero>(MathF.Ceiling(3f - (float)groomSide.Count)))
				{
					groomSide.Enqueue(hero5);
				}
			}
			if (brideSide.Count < 3)
			{
				IEnumerable<Hero> allAliveHeroes2 = Hero.AllAliveHeroes;
				Func<Hero, bool> <>9__5;
				Func<Hero, bool> func2;
				if ((func2 = <>9__5) == null)
				{
					func2 = (<>9__5 = (Hero h) => h.IsLord && !h.IsChild && h != brideHero && h != groomHero && h.IsFriend(brideHero) && !groomSide.Contains(h));
				}
				foreach (Hero hero6 in allAliveHeroes2.Where<Hero>(func2).Take<Hero>(MathF.Ceiling(3f - (float)brideSide.Count)))
				{
					brideSide.Enqueue(hero6);
				}
			}
			for (int i = 0; i < 6; i++)
			{
				bool flag = i <= 1 || i == 4;
				Queue<Hero> queue = (flag ? brideSide : groomSide);
				if (queue.Count > 0 && queue.Peek() != null)
				{
					list.Add(queue.Dequeue());
				}
				else
				{
					list.Add(null);
				}
			}
			return list;
		}

		// Token: 0x040006B6 RID: 1718
		private const int NumberOfAudienceHeroes = 6;

		// Token: 0x040006BA RID: 1722
		private readonly CampaignTime _creationCampaignTime;
	}
}
