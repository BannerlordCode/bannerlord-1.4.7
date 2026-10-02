using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000D9 RID: 217
	[EncyclopediaViewModel(typeof(ShipHull))]
	public class EncyclopediaShipPageVM : EncyclopediaContentPageVM
	{
		// Token: 0x060014E3 RID: 5347 RVA: 0x00052DCC File Offset: 0x00050FCC
		public EncyclopediaShipPageVM(EncyclopediaPageArgs args)
			: base(args)
		{
			this._shipHull = base.Obj as ShipHull;
			this._missionShip = MBObjectManager.Instance.GetObject<MissionShipObject>(this._shipHull.MissionShipObjectId);
			this.StatList = new MBBindingList<EncyclopediaShipStatVM>();
			this.AllShipSlots = new MBBindingList<EncyclopediaShipSlotVM>();
			base.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(this._shipHull);
			this.SailType = this.GetSailType();
			this.RefreshValues();
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x00052E54 File Offset: 0x00051054
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NameText = this.GetName();
			this.PrefabId = EncyclopediaShipPageVM.GetPrefabIdOfShipHull(this._shipHull);
			TextObject description = this._shipHull.Description;
			this.DescriptionText = ((description != null) ? description.ToString() : null) ?? "";
			this.AvailableUpgradesText = new TextObject("{=0xN2FaYa}Available Upgrades", null).ToString();
			this.RefreshShipSlots();
			this.RefreshStats();
			base.UpdateBookmarkHintText();
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x00052ED4 File Offset: 0x000510D4
		private void RefreshShipSlots()
		{
			this.AllShipSlots.Clear();
			using (List<ShipSlot>.Enumerator enumerator = MBObjectManager.Instance.GetObjectTypeList<ShipSlot>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ShipSlot shipSlot = enumerator.Current;
					this.AllShipSlots.Add(new EncyclopediaShipSlotVM(shipSlot.TypeId, this._shipHull.AvailableSlots.Values.Any<ShipSlot>((ShipSlot x) => x.TypeId == shipSlot.TypeId)));
				}
			}
			this.AllShipSlots.Add(new EncyclopediaShipSlotVM("figurehead", this._shipHull.CanEquipFigurehead));
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x00052F98 File Offset: 0x00051198
		private void RefreshStats()
		{
			this.StatList.Clear();
			this.StatsText = new TextObject("{=ffjTMejn}Stats", null).ToString();
			this.StatList.Add(new EncyclopediaShipStatVM("hull", new TextObject("{=wEmx6fZi}Hull", null), this._shipHull.Name.ToString(), null));
			this.StatList.Add(new EncyclopediaShipStatVM("class", new TextObject("{=sqdzHOPe}Class", null), GameTexts.FindText("str_ship_type", this._shipHull.Type.ToString().ToLowerInvariant()).ToString(), null));
			this.StatList.Add(new EncyclopediaShipStatVM("crew", new TextObject("{=wXCM8BnW}Crew", null), this.GetCrewCapacityStr(), new Func<List<TooltipProperty>>(this.GetCrewCapacityTooltip)));
			this.StatList.Add(new EncyclopediaShipStatVM("cargo_capacity", new TextObject("{=IE1KbkaH}Cargo Capacity", null), this._shipHull.InventoryCapacity.ToString(), null));
			this.StatList.Add(new EncyclopediaShipStatVM("weight", new TextObject("{=4Dd2xgPm}Weight", null), this._missionShip.Mass.ToString("0"), null));
			this.StatList.Add(new EncyclopediaShipStatVM("travel_speed", new TextObject("{=DbERaPfF}Travel Speed", null), this._shipHull.BaseSpeed.ToString("0.##"), null));
			this.SailTypeStat = new EncyclopediaShipStatVM("sail_type", new TextObject("{=PJyFY05L}Sail", null), this.GetSailTypeDescription(), null);
			this.StatList.Add(this.SailTypeStat);
			this.StatList.Add(new EncyclopediaShipStatVM("draft_type", new TextObject("{=I4bu7cLr}Draft", null), this.GetDraftTypeStr(), null));
			this.StatList.Add(new EncyclopediaShipStatVM("sea_worthiness", new TextObject("{=yCzuXN3O}Seaworthiness", null), this._shipHull.SeaWorthiness.ToString(), null));
			this.StatList.Add(new EncyclopediaShipStatVM("hit_points", new TextObject("{=oBbiVeKE}Hit Points", null), this._shipHull.MaxHitPoints.ToString(), null));
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x000531E0 File Offset: 0x000513E0
		private string GetSailType()
		{
			if (this._missionShip.HasSails)
			{
				bool flag = this._missionShip.Sails.Any<ShipSail>((ShipSail x) => x.Type == TaleWorlds.Core.SailType.Lateen);
				bool flag2 = this._missionShip.Sails.Any<ShipSail>((ShipSail x) => x.Type == TaleWorlds.Core.SailType.Square);
				if (flag && flag2)
				{
					return "Hybrid";
				}
				if (flag)
				{
					return "Lateen";
				}
				if (flag2)
				{
					return "Square";
				}
			}
			return "None";
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x0005327C File Offset: 0x0005147C
		private string GetSailTypeDescription()
		{
			if (this._missionShip.HasSails)
			{
				bool flag = this._missionShip.Sails.Any<ShipSail>((ShipSail x) => x.Type == TaleWorlds.Core.SailType.Lateen);
				bool flag2 = this._missionShip.Sails.Any<ShipSail>((ShipSail x) => x.Type == TaleWorlds.Core.SailType.Square);
				if (flag && flag2)
				{
					return new TextObject("{=bXJLb0BE}Hybrid", null).ToString();
				}
				if (flag)
				{
					return new TextObject("{=kNxD2oer}Lateen", null).ToString();
				}
				if (flag2)
				{
					return new TextObject("{=squareSail}Square", null).ToString();
				}
			}
			return new TextObject("{=koX9okuG}None", null).ToString();
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x00053347 File Offset: 0x00051547
		private string GetDraftTypeStr()
		{
			if (this._shipHull.CanNavigateShallowWater)
			{
				return new TextObject("{=ShipDraftTypeShallow}Shallow", null).ToString();
			}
			return new TextObject("{=ShipDraftTypeDeep}Deep", null).ToString();
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x00053378 File Offset: 0x00051578
		private string GetCrewCapacityStr()
		{
			int skeletalCrewCapacity = this._shipHull.SkeletalCrewCapacity;
			int mainDeckCrewCapacity = this._shipHull.MainDeckCrewCapacity;
			int num = this._shipHull.TotalCrewCapacity - this._shipHull.MainDeckCrewCapacity;
			TextObject textObject;
			if (num > 0)
			{
				textObject = new TextObject("{=!}{SKELETAL} • {DECK} + {RESERVE}", null);
			}
			else
			{
				textObject = new TextObject("{=!}{SKELETAL} • {DECK}", null);
			}
			return textObject.SetTextVariable("SKELETAL", skeletalCrewCapacity).SetTextVariable("DECK", mainDeckCrewCapacity).SetTextVariable("RESERVE", num)
				.ToString();
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x000533FC File Offset: 0x000515FC
		private List<TooltipProperty> GetCrewCapacityTooltip()
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			int skeletalCrewCapacity = this._shipHull.SkeletalCrewCapacity;
			int mainDeckCrewCapacity = this._shipHull.MainDeckCrewCapacity;
			int totalCrewCapacity = this._shipHull.TotalCrewCapacity;
			int num = totalCrewCapacity - mainDeckCrewCapacity;
			list.Add(new TooltipProperty(new TextObject("{=kalMphFt}Skeletal Capacity", null).ToString(), skeletalCrewCapacity.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			list.Add(new TooltipProperty(string.Empty, GameTexts.FindText("str_ship_stat_explanation", "crewskeletal").ToString(), -1, false, TooltipProperty.TooltipPropertyFlags.MultiLine));
			list.Add(new TooltipProperty(string.Empty, string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.DefaultSeperator));
			list.Add(new TooltipProperty(new TextObject("{=Bt82dbKu}Deck Capacity", null).ToString(), mainDeckCrewCapacity.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			list.Add(new TooltipProperty(string.Empty, GameTexts.FindText("str_ship_stat_explanation", "crewdeck").ToString(), -1, false, TooltipProperty.TooltipPropertyFlags.MultiLine));
			list.Add(new TooltipProperty(string.Empty, string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.None));
			list.Add(new TooltipProperty(new TextObject("{=HThruy9f}Reserve Capacity", null).ToString(), num.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			list.Add(new TooltipProperty(string.Empty, GameTexts.FindText("str_ship_stat_explanation", "crewreserve").ToString(), -1, false, TooltipProperty.TooltipPropertyFlags.MultiLine));
			list.Add(new TooltipProperty(string.Empty, string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.RundownSeperator));
			list.Add(new TooltipProperty(new TextObject("{=kLvWPxIK}Total Capacity", null).ToString(), totalCrewCapacity.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			list.Add(new TooltipProperty(string.Empty, GameTexts.FindText("str_ship_stat_explanation", "crewtotal").ToString(), -1, false, TooltipProperty.TooltipPropertyFlags.MultiLine));
			return list;
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x000535B6 File Offset: 0x000517B6
		public override string GetName()
		{
			return this._shipHull.Name.ToString();
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x000535C8 File Offset: 0x000517C8
		private static string GetPrefabIdOfShipHull(ShipHull shipHull)
		{
			MissionShipObject @object = MBObjectManager.Instance.GetObject<MissionShipObject>(shipHull.MissionShipObjectId);
			return ((@object != null) ? @object.Prefab : null) ?? string.Empty;
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x000535F0 File Offset: 0x000517F0
		public override string GetNavigationBarURL()
		{
			return HyperlinkTexts.GetGenericHyperlinkText("Home", GameTexts.FindText("str_encyclopedia_home", null).ToString()) + " \\ " + HyperlinkTexts.GetGenericHyperlinkText("ListPage-Ships", GameTexts.FindText("str_encyclopedia_ships", null).ToString()) + " \\ " + this.GetName();
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x00053655 File Offset: 0x00051855
		public void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x00053668 File Offset: 0x00051868
		public override void ExecuteSwitchBookmarkedState()
		{
			base.ExecuteSwitchBookmarkedState();
			if (base.IsBookmarked)
			{
				Campaign.Current.EncyclopediaManager.ViewDataTracker.AddEncyclopediaBookmarkToItem(this._shipHull);
				return;
			}
			Campaign.Current.EncyclopediaManager.ViewDataTracker.RemoveEncyclopediaBookmarkFromItem(this._shipHull);
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x060014F1 RID: 5361 RVA: 0x000536B8 File Offset: 0x000518B8
		// (set) Token: 0x060014F2 RID: 5362 RVA: 0x000536C0 File Offset: 0x000518C0
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x060014F3 RID: 5363 RVA: 0x000536E3 File Offset: 0x000518E3
		// (set) Token: 0x060014F4 RID: 5364 RVA: 0x000536EB File Offset: 0x000518EB
		[DataSourceProperty]
		public string AvailableUpgradesText
		{
			get
			{
				return this._availableUpgradesText;
			}
			set
			{
				if (value != this._availableUpgradesText)
				{
					this._availableUpgradesText = value;
					base.OnPropertyChangedWithValue<string>(value, "AvailableUpgradesText");
				}
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x060014F5 RID: 5365 RVA: 0x0005370E File Offset: 0x0005190E
		// (set) Token: 0x060014F6 RID: 5366 RVA: 0x00053716 File Offset: 0x00051916
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x060014F7 RID: 5367 RVA: 0x00053739 File Offset: 0x00051939
		// (set) Token: 0x060014F8 RID: 5368 RVA: 0x00053741 File Offset: 0x00051941
		[DataSourceProperty]
		public string PrefabId
		{
			get
			{
				return this._prefabId;
			}
			set
			{
				if (value != this._prefabId)
				{
					this._prefabId = value;
					base.OnPropertyChangedWithValue<string>(value, "PrefabId");
				}
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x060014F9 RID: 5369 RVA: 0x00053764 File Offset: 0x00051964
		// (set) Token: 0x060014FA RID: 5370 RVA: 0x0005376C File Offset: 0x0005196C
		[DataSourceProperty]
		public string StatsText
		{
			get
			{
				return this._statsText;
			}
			set
			{
				if (value != this._statsText)
				{
					this._statsText = value;
					base.OnPropertyChangedWithValue<string>(value, "StatsText");
				}
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x060014FB RID: 5371 RVA: 0x0005378F File Offset: 0x0005198F
		// (set) Token: 0x060014FC RID: 5372 RVA: 0x00053797 File Offset: 0x00051997
		[DataSourceProperty]
		public string SailType
		{
			get
			{
				return this._sailType;
			}
			set
			{
				if (value != this._sailType)
				{
					this._sailType = value;
					base.OnPropertyChangedWithValue<string>(value, "SailType");
				}
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x060014FD RID: 5373 RVA: 0x000537BA File Offset: 0x000519BA
		// (set) Token: 0x060014FE RID: 5374 RVA: 0x000537C2 File Offset: 0x000519C2
		[DataSourceProperty]
		public EncyclopediaShipStatVM SailTypeStat
		{
			get
			{
				return this._sailTypeStat;
			}
			set
			{
				if (value != this._sailTypeStat)
				{
					this._sailTypeStat = value;
					base.OnPropertyChangedWithValue<EncyclopediaShipStatVM>(value, "SailTypeStat");
				}
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x060014FF RID: 5375 RVA: 0x000537E0 File Offset: 0x000519E0
		// (set) Token: 0x06001500 RID: 5376 RVA: 0x000537E8 File Offset: 0x000519E8
		[DataSourceProperty]
		public MBBindingList<EncyclopediaShipStatVM> StatList
		{
			get
			{
				return this._statList;
			}
			set
			{
				if (value != this._statList)
				{
					this._statList = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaShipStatVM>>(value, "StatList");
				}
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06001501 RID: 5377 RVA: 0x00053806 File Offset: 0x00051A06
		// (set) Token: 0x06001502 RID: 5378 RVA: 0x0005380E File Offset: 0x00051A0E
		[DataSourceProperty]
		public MBBindingList<EncyclopediaShipSlotVM> AllShipSlots
		{
			get
			{
				return this._allShipSlots;
			}
			set
			{
				if (value != this._allShipSlots)
				{
					this._allShipSlots = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaShipSlotVM>>(value, "AllShipSlots");
				}
			}
		}

		// Token: 0x04000989 RID: 2441
		private readonly ShipHull _shipHull;

		// Token: 0x0400098A RID: 2442
		private readonly MissionShipObject _missionShip;

		// Token: 0x0400098B RID: 2443
		private string _descriptionText;

		// Token: 0x0400098C RID: 2444
		private string _prefabId;

		// Token: 0x0400098D RID: 2445
		private string _nameText;

		// Token: 0x0400098E RID: 2446
		private string _availableUpgradesText;

		// Token: 0x0400098F RID: 2447
		private string _statsText;

		// Token: 0x04000990 RID: 2448
		private string _sailType;

		// Token: 0x04000991 RID: 2449
		private EncyclopediaShipStatVM _sailTypeStat;

		// Token: 0x04000992 RID: 2450
		private MBBindingList<EncyclopediaShipStatVM> _statList;

		// Token: 0x04000993 RID: 2451
		private MBBindingList<EncyclopediaShipSlotVM> _allShipSlots;
	}
}
