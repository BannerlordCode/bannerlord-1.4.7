using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Encyclopedia.Pages
{
	// Token: 0x02000183 RID: 387
	[EncyclopediaModel(new Type[] { typeof(ShipHull) })]
	public class DefaultEncyclopediaShipPage : EncyclopediaPage
	{
		// Token: 0x06001BBF RID: 7103 RVA: 0x0008F17C File Offset: 0x0008D37C
		public DefaultEncyclopediaShipPage()
		{
			base.HomePageOrderIndex = 200;
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x0008F190 File Offset: 0x0008D390
		public override bool IsRelevant()
		{
			MBObjectManager instance = MBObjectManager.Instance;
			if (instance == null)
			{
				return false;
			}
			MBReadOnlyList<ShipHull> objectTypeList = instance.GetObjectTypeList<ShipHull>();
			int? num = ((objectTypeList != null) ? new int?(objectTypeList.Count) : null);
			int num2 = 0;
			return (num.GetValueOrDefault() > num2) & (num != null);
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x0008F1DB File Offset: 0x0008D3DB
		protected override IEnumerable<EncyclopediaListItem> InitializeListItems()
		{
			List<ShipHull> list = new List<ShipHull>();
			foreach (CultureObject cultureObject in MBObjectManager.Instance.GetObjectTypeList<CultureObject>())
			{
				if (cultureObject.IsMainCulture && cultureObject.AvailableShipHulls.Count > 0)
				{
					list.AddRange(cultureObject.AvailableShipHulls);
				}
			}
			MBReadOnlyList<ShipHull> objectTypeList = MBObjectManager.Instance.GetObjectTypeList<ShipHull>();
			list.AddRange(objectTypeList.Where<ShipHull>((ShipHull x) => x.StringId == "fishing_ship" || x.StringId == "southern_fishing_ship"));
			using (IEnumerator<ShipHull> enumerator2 = list.Distinct<ShipHull>().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ShipHull shipHull = enumerator2.Current;
					TextObject textObject = null;
					if (this.IsValidEncyclopediaItem(shipHull))
					{
						textObject = shipHull.Name;
					}
					string text = ((textObject != null) ? textObject.ToString() : null) ?? string.Empty;
					yield return new EncyclopediaListItem(shipHull, text, "", shipHull.StringId, base.GetIdentifier(typeof(ShipHull)), DefaultEncyclopediaShipPage.CanPlayerSeeValuesOf(shipHull), delegate
					{
						InformationManager.ShowTooltip(typeof(ShipHull), new object[] { shipHull });
					});
				}
			}
			IEnumerator<ShipHull> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x0008F1EC File Offset: 0x0008D3EC
		protected override IEnumerable<EncyclopediaFilterGroup> InitializeFilterItems()
		{
			List<EncyclopediaFilterGroup> list = new List<EncyclopediaFilterGroup>();
			List<EncyclopediaFilterItem> list2 = new List<EncyclopediaFilterItem>();
			using (List<CultureObject>.Enumerator enumerator = (from x in Game.Current.ObjectManager.GetObjectTypeList<CultureObject>()
				where x.IsMainCulture
				select x into f
				orderby f.Name.ToString()
				select f).ToList<CultureObject>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CultureObject culture = enumerator.Current;
					if (culture.StringId != "neutral_culture" && culture.CanHaveSettlement)
					{
						list2.Add(new EncyclopediaFilterItem(culture.Name, (object c) => culture.AvailableShipHulls.Contains((ShipHull)c)));
					}
				}
			}
			list.Add(new EncyclopediaFilterGroup(list2, GameTexts.FindText("str_culture", null)));
			List<EncyclopediaFilterItem> list3 = new List<EncyclopediaFilterItem>();
			list3.Add(new EncyclopediaFilterItem(GameTexts.FindText("str_ship_type", "heavy"), delegate(object s)
			{
				ShipHull shipHull;
				return (shipHull = s as ShipHull) != null && shipHull.Type == ShipHull.ShipType.Heavy;
			}));
			list3.Add(new EncyclopediaFilterItem(GameTexts.FindText("str_ship_type", "medium"), delegate(object s)
			{
				ShipHull shipHull2;
				return (shipHull2 = s as ShipHull) != null && shipHull2.Type == ShipHull.ShipType.Medium;
			}));
			list3.Add(new EncyclopediaFilterItem(GameTexts.FindText("str_ship_type", "light"), delegate(object s)
			{
				ShipHull shipHull3;
				return (shipHull3 = s as ShipHull) != null && shipHull3.Type == ShipHull.ShipType.Light;
			}));
			List<EncyclopediaFilterItem> list4 = list3;
			list.Add(new EncyclopediaFilterGroup(list4, new TextObject("{=sqdzHOPe}Class", null)));
			List<EncyclopediaFilterItem> list5 = new List<EncyclopediaFilterItem>
			{
				new EncyclopediaFilterItem(new TextObject("{=bXJLb0BE}Hybrid", null), delegate(object s)
				{
					ShipHull shipHull4;
					MissionShipObject @object;
					return (shipHull4 = s as ShipHull) != null && (@object = MBObjectManager.Instance.GetObject<MissionShipObject>(shipHull4.MissionShipObjectId)) != null && this.HasSailOfType(@object, SailType.Square) && this.HasSailOfType(@object, SailType.Lateen);
				}),
				new EncyclopediaFilterItem(new TextObject("{=kNxD2oer}Lateen", null), delegate(object s)
				{
					ShipHull shipHull5;
					return (shipHull5 = s as ShipHull) != null && this.HasSailOfType(MBObjectManager.Instance.GetObject<MissionShipObject>(shipHull5.MissionShipObjectId), SailType.Lateen);
				}),
				new EncyclopediaFilterItem(new TextObject("{=squareSail}Square", null), delegate(object s)
				{
					ShipHull shipHull6;
					return (shipHull6 = s as ShipHull) != null && this.HasSailOfType(MBObjectManager.Instance.GetObject<MissionShipObject>(shipHull6.MissionShipObjectId), SailType.Square);
				})
			};
			list.Add(new EncyclopediaFilterGroup(list5, new TextObject("{=UIb3IW3f}Sail Type", null)));
			return list;
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x0008F458 File Offset: 0x0008D658
		private bool HasSailOfType(MissionShipObject ship, SailType sailType)
		{
			return ship != null && ship.HasSails && ship.Sails.Any<ShipSail>((ShipSail x) => x.Type == sailType);
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x0008F498 File Offset: 0x0008D698
		protected override IEnumerable<EncyclopediaSortController> InitializeSortControllers()
		{
			return new List<EncyclopediaSortController>
			{
				new EncyclopediaSortController(new TextObject("{=sqdzHOPe}Class", null), new DefaultEncyclopediaShipPage.EncyclopediaListShipClassComparer()),
				new EncyclopediaSortController(new TextObject("{=UbZL2BJQ}Hitpoints", null), new DefaultEncyclopediaShipPage.EncyclopediaListShipHealthComparer()),
				new EncyclopediaSortController(new TextObject("{=FQ2m5e5E}Slots", null), new DefaultEncyclopediaShipPage.EncyclopediaListShipSlotCountComparer())
			};
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x0008F4FB File Offset: 0x0008D6FB
		public override string GetViewFullyQualifiedName()
		{
			return "EncyclopediaShipPage";
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x0008F502 File Offset: 0x0008D702
		public override string GetStringID()
		{
			return "EncyclopediaShip";
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x0008F509 File Offset: 0x0008D709
		public override TextObject GetName()
		{
			return GameTexts.FindText("str_encyclopedia_ships", null);
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x0008F516 File Offset: 0x0008D716
		public override MBObjectBase GetObject(string typeName, string stringID)
		{
			return MBObjectManager.Instance.GetObject<ShipHull>(stringID);
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x0008F524 File Offset: 0x0008D724
		public override bool IsValidEncyclopediaItem(object o)
		{
			ShipHull shipHull;
			return (shipHull = o as ShipHull) != null && shipHull.IsReady && shipHull.IsInitialized;
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x0008F54D File Offset: 0x0008D74D
		private static bool CanPlayerSeeValuesOf(ShipHull shipHull)
		{
			return true;
		}

		// Token: 0x020005E5 RID: 1509
		private class EncyclopediaListShipClassComparer : DefaultEncyclopediaShipPage.EncyclopediaListShipComparer
		{
			// Token: 0x06004FCA RID: 20426 RVA: 0x00184B44 File Offset: 0x00182D44
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareShips(x, y, DefaultEncyclopediaShipPage.EncyclopediaListShipClassComparer._comparison);
			}

			// Token: 0x06004FCB RID: 20427 RVA: 0x00184B54 File Offset: 0x00182D54
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				ShipHull shipHull;
				if ((shipHull = item.Object as ShipHull) == null)
				{
					Debug.FailedAssert("Unable to get the class of a ship object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaShipPage.cs", "GetComparedValueText", 164);
					return "";
				}
				if (!DefaultEncyclopediaShipPage.CanPlayerSeeValuesOf(shipHull))
				{
					return this._missingValue.ToString();
				}
				return shipHull.Type.ToString();
			}

			// Token: 0x040018AA RID: 6314
			private static Func<ShipHull, ShipHull, int> _comparison = (ShipHull s1, ShipHull s2) => s1.Type.CompareTo(s2.Type);
		}

		// Token: 0x020005E6 RID: 1510
		private class EncyclopediaListShipSlotCountComparer : DefaultEncyclopediaShipPage.EncyclopediaListShipComparer
		{
			// Token: 0x06004FCE RID: 20430 RVA: 0x00184BD6 File Offset: 0x00182DD6
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareShips(x, y, DefaultEncyclopediaShipPage.EncyclopediaListShipSlotCountComparer._comparison);
			}

			// Token: 0x06004FCF RID: 20431 RVA: 0x00184BE8 File Offset: 0x00182DE8
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				ShipHull shipHull;
				if ((shipHull = item.Object as ShipHull) == null)
				{
					Debug.FailedAssert("Unable to get the availableSlotCount of a ship object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaShipPage.cs", "GetComparedValueText", 192);
					return "";
				}
				if (!DefaultEncyclopediaShipPage.CanPlayerSeeValuesOf(shipHull))
				{
					return this._missingValue.ToString();
				}
				return shipHull.AvailableSlots.Count.ToString();
			}

			// Token: 0x040018AB RID: 6315
			private static Func<ShipHull, ShipHull, int> _comparison = (ShipHull s1, ShipHull s2) => s1.AvailableSlots.Count.CompareTo(s2.AvailableSlots.Count);
		}

		// Token: 0x020005E7 RID: 1511
		private class EncyclopediaListShipHealthComparer : DefaultEncyclopediaShipPage.EncyclopediaListShipComparer
		{
			// Token: 0x06004FD2 RID: 20434 RVA: 0x00184C69 File Offset: 0x00182E69
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareShips(x, y, DefaultEncyclopediaShipPage.EncyclopediaListShipHealthComparer._comparison);
			}

			// Token: 0x06004FD3 RID: 20435 RVA: 0x00184C78 File Offset: 0x00182E78
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				ShipHull shipHull;
				if ((shipHull = item.Object as ShipHull) == null)
				{
					Debug.FailedAssert("Unable to get the hitPoints between a ship object and the player.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaShipPage.cs", "GetComparedValueText", 222);
					return "";
				}
				if (!DefaultEncyclopediaShipPage.CanPlayerSeeValuesOf(shipHull))
				{
					return this._missingValue.ToString();
				}
				int maxHitPoints = shipHull.MaxHitPoints;
				MBTextManager.SetTextVariable("NUMBER", maxHitPoints);
				return maxHitPoints.ToString();
			}

			// Token: 0x040018AC RID: 6316
			private static Func<ShipHull, ShipHull, int> _comparison = (ShipHull s1, ShipHull s2) => s1.MaxHitPoints.CompareTo(s2.MaxHitPoints);
		}

		// Token: 0x020005E8 RID: 1512
		public abstract class EncyclopediaListShipComparer : EncyclopediaListItemComparerBase
		{
			// Token: 0x06004FD6 RID: 20438 RVA: 0x00184D00 File Offset: 0x00182F00
			protected bool CompareVisibility(ShipHull s1, ShipHull s2, out int comparisonResult)
			{
				bool flag = DefaultEncyclopediaShipPage.CanPlayerSeeValuesOf(s1);
				bool flag2 = DefaultEncyclopediaShipPage.CanPlayerSeeValuesOf(s2);
				if (!flag && !flag2)
				{
					comparisonResult = 0;
					return true;
				}
				if (!flag)
				{
					comparisonResult = (base.IsAscending ? 1 : (-1));
					return true;
				}
				if (!flag2)
				{
					comparisonResult = (base.IsAscending ? (-1) : 1);
					return true;
				}
				comparisonResult = 0;
				return false;
			}

			// Token: 0x06004FD7 RID: 20439 RVA: 0x00184D50 File Offset: 0x00182F50
			protected int CompareShips(EncyclopediaListItem x, EncyclopediaListItem y, Func<ShipHull, ShipHull, int> comparison)
			{
				ShipHull shipHull;
				ShipHull shipHull2;
				if ((shipHull = x.Object as ShipHull) == null || (shipHull2 = y.Object as ShipHull) == null)
				{
					Debug.FailedAssert("Both objects should be shipHull.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaShipPage.cs", "CompareShips", 271);
					return 0;
				}
				int num;
				if (this.CompareVisibility(shipHull, shipHull2, out num))
				{
					if (num == 0)
					{
						return base.ResolveEquality(x, y);
					}
					return num * (base.IsAscending ? 1 : (-1));
				}
				else
				{
					int num2 = comparison(shipHull, shipHull2) * (base.IsAscending ? 1 : (-1));
					if (num2 == 0)
					{
						return base.ResolveEquality(x, y);
					}
					return num2;
				}
			}

			// Token: 0x020008C2 RID: 2242
			// (Invoke) Token: 0x06006920 RID: 26912
			protected delegate bool ShipVisibilityComparerDelegate(ShipHull s1, ShipHull s2, out int comparisonResult);
		}
	}
}
