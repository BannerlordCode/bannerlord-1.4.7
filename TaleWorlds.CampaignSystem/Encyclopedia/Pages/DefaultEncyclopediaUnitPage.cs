using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Encyclopedia.Pages
{
	// Token: 0x02000184 RID: 388
	[EncyclopediaModel(new Type[] { typeof(CharacterObject) })]
	public class DefaultEncyclopediaUnitPage : EncyclopediaPage
	{
		// Token: 0x06001BCE RID: 7118 RVA: 0x0008F5F0 File Offset: 0x0008D7F0
		public DefaultEncyclopediaUnitPage()
		{
			base.HomePageOrderIndex = 300;
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x0008F603 File Offset: 0x0008D803
		protected override IEnumerable<EncyclopediaListItem> InitializeListItems()
		{
			using (List<CharacterObject>.Enumerator enumerator = CharacterObject.All.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CharacterObject character = enumerator.Current;
					if (this.IsValidEncyclopediaItem(character))
					{
						yield return new EncyclopediaListItem(character, character.Name.ToString(), "", character.StringId, base.GetIdentifier(typeof(CharacterObject)), true, delegate
						{
							InformationManager.ShowTooltip(typeof(CharacterObject), new object[] { character });
						});
					}
				}
			}
			List<CharacterObject>.Enumerator enumerator = default(List<CharacterObject>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x0008F614 File Offset: 0x0008D814
		protected override IEnumerable<EncyclopediaFilterGroup> InitializeFilterItems()
		{
			List<EncyclopediaFilterGroup> list = new List<EncyclopediaFilterGroup>();
			List<EncyclopediaFilterItem> typeFilterItems = this.GetTypeFilterItems();
			list.Add(new EncyclopediaFilterGroup(typeFilterItems, new TextObject("{=zMMqgxb1}Type", null)));
			List<EncyclopediaFilterItem> occupationFilterItems = this.GetOccupationFilterItems();
			list.Add(new EncyclopediaFilterGroup(occupationFilterItems, new TextObject("{=GZxFIeiJ}Occupation", null)));
			List<EncyclopediaFilterItem> cultureFilterItems = this.GetCultureFilterItems();
			list.Add(new EncyclopediaFilterGroup(cultureFilterItems, GameTexts.FindText("str_culture", null)));
			List<EncyclopediaFilterItem> outlawFilterItems = this.GetOutlawFilterItems();
			list.Add(new EncyclopediaFilterGroup(outlawFilterItems, GameTexts.FindText("str_outlaw", null)));
			return list;
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x0008F6A0 File Offset: 0x0008D8A0
		protected virtual List<EncyclopediaFilterItem> GetTypeFilterItems()
		{
			List<EncyclopediaFilterItem> list = new List<EncyclopediaFilterItem>();
			list.Add(new EncyclopediaFilterItem(new TextObject("{=1Bm1Wk1v}Infantry", null), (object s) => ((CharacterObject)s).IsInfantry));
			list.Add(new EncyclopediaFilterItem(new TextObject("{=bIiBytSB}Archers", null), (object s) => ((CharacterObject)s).IsRanged && !((CharacterObject)s).IsMounted));
			list.Add(new EncyclopediaFilterItem(new TextObject("{=YVGtcLHF}Cavalry", null), (object s) => ((CharacterObject)s).IsMounted && !((CharacterObject)s).IsRanged));
			list.Add(new EncyclopediaFilterItem(new TextObject("{=I1CMeL9R}Mounted Archers", null), (object s) => ((CharacterObject)s).IsRanged && ((CharacterObject)s).IsMounted));
			return list;
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x0008F788 File Offset: 0x0008D988
		protected virtual List<EncyclopediaFilterItem> GetOccupationFilterItems()
		{
			List<EncyclopediaFilterItem> list = new List<EncyclopediaFilterItem>();
			list.Add(new EncyclopediaFilterItem(GameTexts.FindText("str_occupation", "Soldier"), (object s) => ((CharacterObject)s).Occupation == Occupation.Soldier));
			list.Add(new EncyclopediaFilterItem(GameTexts.FindText("str_occupation", "Mercenary"), (object s) => ((CharacterObject)s).Occupation == Occupation.Mercenary));
			list.Add(new EncyclopediaFilterItem(GameTexts.FindText("str_occupation", "Bandit"), (object s) => ((CharacterObject)s).Occupation == Occupation.Bandit));
			return list;
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x0008F848 File Offset: 0x0008DA48
		protected virtual List<EncyclopediaFilterItem> GetCultureFilterItems()
		{
			List<EncyclopediaFilterItem> list = new List<EncyclopediaFilterItem>();
			using (List<CultureObject>.Enumerator enumerator = (from x in Game.Current.ObjectManager.GetObjectTypeList<CultureObject>()
				where x.IsMainCulture
				select x into f
				orderby f.Name.ToString()
				select f).ToList<CultureObject>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CultureObject culture = enumerator.Current;
					if (!culture.IsBandit && culture.StringId != "neutral_culture")
					{
						list.Add(new EncyclopediaFilterItem(culture.Name, (object c) => ((CharacterObject)c).Culture == culture));
					}
				}
			}
			return list;
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x0008F944 File Offset: 0x0008DB44
		protected virtual List<EncyclopediaFilterItem> GetOutlawFilterItems()
		{
			List<EncyclopediaFilterItem> list = new List<EncyclopediaFilterItem>();
			using (List<CultureObject>.Enumerator enumerator = (from x in Game.Current.ObjectManager.GetObjectTypeList<CultureObject>()
				orderby !x.IsMainCulture descending
				select x).ThenBy<CultureObject, string>((CultureObject f) => f.Name.ToString()).ToList<CultureObject>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CultureObject culture = enumerator.Current;
					if (culture.IsBandit)
					{
						list.Add(new EncyclopediaFilterItem(culture.Name, (object c) => ((CharacterObject)c).Culture == culture));
					}
				}
			}
			return list;
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x0008FA28 File Offset: 0x0008DC28
		protected override IEnumerable<EncyclopediaSortController> InitializeSortControllers()
		{
			return new List<EncyclopediaSortController>
			{
				new EncyclopediaSortController(new TextObject("{=cc1d7mkq}Tier", null), new DefaultEncyclopediaUnitPage.EncyclopediaListUnitTierComparer()),
				new EncyclopediaSortController(GameTexts.FindText("str_level_tag", null), new DefaultEncyclopediaUnitPage.EncyclopediaListUnitLevelComparer())
			};
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x0008FA65 File Offset: 0x0008DC65
		public override string GetViewFullyQualifiedName()
		{
			return "EncyclopediaUnitPage";
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x0008FA6C File Offset: 0x0008DC6C
		public override TextObject GetName()
		{
			return GameTexts.FindText("str_encyclopedia_troops", null);
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x0008FA79 File Offset: 0x0008DC79
		public override TextObject GetDescriptionText()
		{
			return GameTexts.FindText("str_unit_description", null);
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x0008FA86 File Offset: 0x0008DC86
		public override string GetStringID()
		{
			return "EncyclopediaUnit";
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x0008FA90 File Offset: 0x0008DC90
		public override bool IsValidEncyclopediaItem(object o)
		{
			CharacterObject characterObject = o as CharacterObject;
			return characterObject != null && !characterObject.IsTemplate && characterObject != null && !characterObject.HiddenInEncyclopedia && ((characterObject != null) ? characterObject.HeroObject : null) == null && (characterObject.Occupation == Occupation.Soldier || characterObject.Occupation == Occupation.Mercenary || characterObject.Occupation == Occupation.Bandit || characterObject.Occupation == Occupation.Gangster || characterObject.Occupation == Occupation.CaravanGuard || (characterObject.Occupation == Occupation.Villager && characterObject.UpgradeTargets.Length != 0));
		}

		// Token: 0x020005EE RID: 1518
		private class EncyclopediaListUnitTierComparer : DefaultEncyclopediaUnitPage.EncyclopediaListUnitComparer
		{
			// Token: 0x06004FF0 RID: 20464 RVA: 0x001851D3 File Offset: 0x001833D3
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareUnits(x, y, DefaultEncyclopediaUnitPage.EncyclopediaListUnitTierComparer._comparison);
			}

			// Token: 0x06004FF1 RID: 20465 RVA: 0x001851E4 File Offset: 0x001833E4
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				CharacterObject characterObject;
				if ((characterObject = item.Object as CharacterObject) != null)
				{
					return characterObject.Tier.ToString();
				}
				Debug.FailedAssert("Unable to get the tier of a non-character object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaUnitPage.cs", "GetComparedValueText", 175);
				return "";
			}

			// Token: 0x040018BC RID: 6332
			private static Func<CharacterObject, CharacterObject, int> _comparison = (CharacterObject c1, CharacterObject c2) => c1.Tier.CompareTo(c2.Tier);
		}

		// Token: 0x020005EF RID: 1519
		private class EncyclopediaListUnitLevelComparer : DefaultEncyclopediaUnitPage.EncyclopediaListUnitComparer
		{
			// Token: 0x06004FF4 RID: 20468 RVA: 0x0018524C File Offset: 0x0018344C
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareUnits(x, y, DefaultEncyclopediaUnitPage.EncyclopediaListUnitLevelComparer._comparison);
			}

			// Token: 0x06004FF5 RID: 20469 RVA: 0x0018525C File Offset: 0x0018345C
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				CharacterObject characterObject;
				if ((characterObject = item.Object as CharacterObject) != null)
				{
					return characterObject.Level.ToString();
				}
				Debug.FailedAssert("Unable to get the level of a non-character object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaUnitPage.cs", "GetComparedValueText", 196);
				return "";
			}

			// Token: 0x040018BD RID: 6333
			private static Func<CharacterObject, CharacterObject, int> _comparison = (CharacterObject c1, CharacterObject c2) => c1.Level.CompareTo(c2.Level);
		}

		// Token: 0x020005F0 RID: 1520
		public abstract class EncyclopediaListUnitComparer : EncyclopediaListItemComparerBase
		{
			// Token: 0x06004FF8 RID: 20472 RVA: 0x001852C4 File Offset: 0x001834C4
			public int CompareUnits(EncyclopediaListItem x, EncyclopediaListItem y, Func<CharacterObject, CharacterObject, int> comparison)
			{
				CharacterObject characterObject;
				CharacterObject characterObject2;
				if ((characterObject = x.Object as CharacterObject) == null || (characterObject2 = y.Object as CharacterObject) == null)
				{
					Debug.FailedAssert("Both objects should be character objects.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaUnitPage.cs", "CompareUnits", 211);
					return 0;
				}
				int num = comparison(characterObject, characterObject2) * (base.IsAscending ? 1 : (-1));
				if (num == 0)
				{
					return base.ResolveEquality(x, y);
				}
				return num;
			}
		}
	}
}
