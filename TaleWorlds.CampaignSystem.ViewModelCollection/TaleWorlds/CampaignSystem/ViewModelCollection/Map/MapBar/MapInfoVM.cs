using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapBar
{
	// Token: 0x0200005C RID: 92
	public class MapInfoVM : ViewModel
	{
		// Token: 0x060006A0 RID: 1696 RVA: 0x000212AC File Offset: 0x0001F4AC
		public MapInfoVM()
		{
			this.ExtendHint = new HintViewModel(GameTexts.FindText("str_map_extend_bar_hint", null), null);
			this._viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			this.IsInfoBarExtended = this._viewDataTracker.GetMapBarExtendedState();
			this.PrimaryInfoItems = new MBBindingList<MapInfoItemVM>();
			this.SecondaryInfoItems = new MBBindingList<MapInfoItemVM>();
			this.CreateItems();
			this.RefreshValues();
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0002131C File Offset: 0x0001F51C
		protected virtual void CreateItems()
		{
			this.PrimaryInfoItems.ApplyActionOnAllItems(delegate(MapInfoItemVM i)
			{
				i.OnFinalize();
			});
			this.PrimaryInfoItems.Clear();
			this.SecondaryInfoItems.ApplyActionOnAllItems(delegate(MapInfoItemVM i)
			{
				i.OnFinalize();
			});
			this.SecondaryInfoItems.Clear();
			this._goldInfo = new MapInfoItemVM("gold", CampaignUIHelper.GetDenarTooltip());
			this._influenceInfo = new MapInfoItemVM("influence", () => CampaignUIHelper.GetInfluenceTooltip(Clan.PlayerClan));
			this._hitPointsInfo = new MapInfoItemVM("hit_points", () => CampaignUIHelper.GetPlayerHitpointsTooltip());
			this._troopsInfo = new MapInfoItemVM("troops", () => CampaignUIHelper.GetMainPartyHealthTooltip());
			this._foodInfo = new MapInfoItemVM("food", () => CampaignUIHelper.GetPartyFoodTooltip(MobileParty.MainParty));
			this._moraleInfo = new MapInfoItemVM("morale", () => CampaignUIHelper.GetPartyMoraleTooltip(MobileParty.MainParty));
			this._speedInfo = new MapInfoItemVM("speed", () => CampaignUIHelper.GetPartySpeedTooltip(true));
			this._viewDistanceInfo = new MapInfoItemVM("view_distance", () => CampaignUIHelper.GetViewDistanceTooltip());
			this._troopWageInfo = new MapInfoItemVM("troop_wage", () => CampaignUIHelper.GetPartyWageTooltip(MobileParty.MainParty));
			this.PrimaryInfoItems.Add(this._goldInfo);
			this.PrimaryInfoItems.Add(this._speedInfo);
			this.PrimaryInfoItems.Add(this._hitPointsInfo);
			this.PrimaryInfoItems.Add(this._troopsInfo);
			this.PrimaryInfoItems.Add(this._foodInfo);
			this.PrimaryInfoItems.Add(this._moraleInfo);
			this.SecondaryInfoItems.Add(this._influenceInfo);
			this.SecondaryInfoItems.Add(this._viewDistanceInfo);
			this.SecondaryInfoItems.Add(this._troopWageInfo);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x000215B9 File Offset: 0x0001F7B9
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.UpdatePlayerInfo(true);
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x000215C8 File Offset: 0x0001F7C8
		public void Tick()
		{
			bool flag = Hero.MainHero != null && Hero.IsMainHeroIll;
			if (this._isMainHeroSick != flag)
			{
				this._isMainHeroSick = flag;
				this._hitPointsInfo.SetOverriddenVisualId(this._isMainHeroSick ? "hit_points_sick" : null);
			}
			bool isCurrentlyAtSea = MobileParty.MainParty.IsCurrentlyAtSea;
			if (this._isMainPartyAtSea != isCurrentlyAtSea)
			{
				this._isMainPartyAtSea = isCurrentlyAtSea;
				this._speedInfo.SetOverriddenVisualId(this._isMainPartyAtSea ? "speed_at_sea" : null);
			}
			Hero mainHero = Hero.MainHero;
			this.IsInfoBarEnabled = mainHero != null && mainHero.IsAlive;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0002165D File Offset: 0x0001F85D
		public void Refresh()
		{
			this.UpdatePlayerInfo(false);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00021668 File Offset: 0x0001F868
		protected virtual void UpdatePlayerInfo(bool updateForced)
		{
			ExplainedNumber explainedNumber = Campaign.Current.Models.ClanFinanceModel.CalculateClanGoldChange(Clan.PlayerClan, true, false, true);
			this._goldInfo.HasWarning = (float)Hero.MainHero.Gold + explainedNumber.ResultNumber < 0f;
			if (this._goldInfo.IntValue != Hero.MainHero.Gold || updateForced)
			{
				this._goldInfo.IntValue = Hero.MainHero.Gold;
				this._goldInfo.Value = CampaignUIHelper.GetAbbreviatedValueTextFromValue(this._goldInfo.IntValue);
			}
			this._influenceInfo.HasWarning = Hero.MainHero.Clan.Influence < -100f;
			if (this._influenceInfo.IntValue != (int)Hero.MainHero.Clan.Influence || updateForced)
			{
				this._influenceInfo.IntValue = (int)Hero.MainHero.Clan.Influence;
				this._influenceInfo.Value = CampaignUIHelper.GetAbbreviatedValueTextFromValue(this._influenceInfo.IntValue);
			}
			float num = MathF.Round(MobileParty.MainParty.Morale, 1);
			this._moraleInfo.HasWarning = MobileParty.MainParty.Morale < (float)Campaign.Current.Models.PartyDesertionModel.GetMoraleThresholdForTroopDesertion();
			if (this._moraleInfo.FloatValue != num || updateForced)
			{
				this._moraleInfo.Value = num.ToString();
				this._moraleInfo.FloatValue = num;
				MBTextManager.SetTextVariable("BASE_EFFECT", num.ToString("0.0"), false);
			}
			int numDaysForFoodToLast = MobileParty.MainParty.GetNumDaysForFoodToLast();
			this._foodInfo.HasWarning = numDaysForFoodToLast < 1;
			this._foodInfo.IntValue = (int)((MobileParty.MainParty.Food > 0f) ? MobileParty.MainParty.Food : 0f);
			this._foodInfo.Value = this._foodInfo.IntValue.ToString();
			this._troopsInfo.HasWarning = PartyBase.MainParty.PartySizeLimit < PartyBase.MainParty.NumberOfAllMembers || PartyBase.MainParty.PrisonerSizeLimit < PartyBase.MainParty.NumberOfPrisoners;
			this._troopsInfo.IntValue = PartyBase.MainParty.MemberRoster.TotalManCount;
			this._troopsInfo.Value = CampaignUIHelper.GetPartyNameplateText(PartyBase.MainParty);
			int num2 = (int)MathF.Clamp((float)(Hero.MainHero.HitPoints * 100 / CharacterObject.PlayerCharacter.MaxHitPoints()), 1f, 100f);
			this._hitPointsInfo.HasWarning = Hero.MainHero.IsWounded;
			if (this._hitPointsInfo.IntValue != num2 || updateForced)
			{
				this._hitPointsInfo.IntValue = num2;
				GameTexts.SetVariable("NUMBER", this._hitPointsInfo.IntValue);
				this._hitPointsInfo.Value = GameTexts.FindText("str_NUMBER_percent", null).ToString();
			}
			Army army = MobileParty.MainParty.Army;
			MobileParty mobileParty = ((army != null) ? army.LeaderParty : null) ?? MobileParty.MainParty;
			float num3 = ((mobileParty.IsActive && mobileParty.CurrentNavigationFace.IsValid()) ? mobileParty.Speed : 0f);
			if (this._speedInfo.FloatValue != num3 || updateForced)
			{
				this._speedInfo.FloatValue = num3;
				this._speedInfo.Value = CampaignUIHelper.FloatToString(num3);
			}
			float seeingRange = MobileParty.MainParty.SeeingRange;
			if (this._viewDistanceInfo.FloatValue != seeingRange || updateForced)
			{
				this._viewDistanceInfo.FloatValue = seeingRange;
				this._viewDistanceInfo.Value = CampaignUIHelper.FloatToString(seeingRange);
			}
			int totalWage = MobileParty.MainParty.TotalWage;
			if (this._troopWageInfo.IntValue != totalWage || updateForced)
			{
				this._troopWageInfo.IntValue = totalWage;
				this._troopWageInfo.Value = totalWage.ToString();
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x00021A6A File Offset: 0x0001FC6A
		// (set) Token: 0x060006A7 RID: 1703 RVA: 0x00021A72 File Offset: 0x0001FC72
		[DataSourceProperty]
		public bool IsInfoBarExtended
		{
			get
			{
				return this._isInfoBarExtended;
			}
			set
			{
				if (value != this._isInfoBarExtended)
				{
					this._isInfoBarExtended = value;
					this._viewDataTracker.SetMapBarExtendedState(value);
					base.OnPropertyChangedWithValue(value, "IsInfoBarExtended");
				}
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060006A8 RID: 1704 RVA: 0x00021A9C File Offset: 0x0001FC9C
		// (set) Token: 0x060006A9 RID: 1705 RVA: 0x00021AA4 File Offset: 0x0001FCA4
		[DataSourceProperty]
		public bool IsInfoBarEnabled
		{
			get
			{
				return this._isInfoBarEnabled;
			}
			set
			{
				if (value != this._isInfoBarEnabled)
				{
					this._isInfoBarEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsInfoBarEnabled");
				}
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x00021AC2 File Offset: 0x0001FCC2
		// (set) Token: 0x060006AB RID: 1707 RVA: 0x00021ACA File Offset: 0x0001FCCA
		[DataSourceProperty]
		public HintViewModel ExtendHint
		{
			get
			{
				return this._extendHint;
			}
			set
			{
				if (value != this._extendHint)
				{
					this._extendHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ExtendHint");
				}
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x00021AE8 File Offset: 0x0001FCE8
		// (set) Token: 0x060006AD RID: 1709 RVA: 0x00021AF0 File Offset: 0x0001FCF0
		[DataSourceProperty]
		public MBBindingList<MapInfoItemVM> PrimaryInfoItems
		{
			get
			{
				return this._primaryInfoItems;
			}
			set
			{
				if (value != this._primaryInfoItems)
				{
					this._primaryInfoItems = value;
					base.OnPropertyChangedWithValue<MBBindingList<MapInfoItemVM>>(value, "PrimaryInfoItems");
				}
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x00021B0E File Offset: 0x0001FD0E
		// (set) Token: 0x060006AF RID: 1711 RVA: 0x00021B16 File Offset: 0x0001FD16
		[DataSourceProperty]
		public MBBindingList<MapInfoItemVM> SecondaryInfoItems
		{
			get
			{
				return this._secondaryInfoItems;
			}
			set
			{
				if (value != this._secondaryInfoItems)
				{
					this._secondaryInfoItems = value;
					base.OnPropertyChangedWithValue<MBBindingList<MapInfoItemVM>>(value, "SecondaryInfoItems");
				}
			}
		}

		// Token: 0x040002DB RID: 731
		private IViewDataTracker _viewDataTracker;

		// Token: 0x040002DC RID: 732
		private MapInfoItemVM _goldInfo;

		// Token: 0x040002DD RID: 733
		private MapInfoItemVM _influenceInfo;

		// Token: 0x040002DE RID: 734
		private MapInfoItemVM _hitPointsInfo;

		// Token: 0x040002DF RID: 735
		private MapInfoItemVM _troopsInfo;

		// Token: 0x040002E0 RID: 736
		private MapInfoItemVM _foodInfo;

		// Token: 0x040002E1 RID: 737
		private MapInfoItemVM _moraleInfo;

		// Token: 0x040002E2 RID: 738
		private MapInfoItemVM _speedInfo;

		// Token: 0x040002E3 RID: 739
		private MapInfoItemVM _viewDistanceInfo;

		// Token: 0x040002E4 RID: 740
		private MapInfoItemVM _troopWageInfo;

		// Token: 0x040002E5 RID: 741
		private bool _isMainHeroSick;

		// Token: 0x040002E6 RID: 742
		private bool _isMainPartyAtSea;

		// Token: 0x040002E7 RID: 743
		private bool _isInfoBarExtended;

		// Token: 0x040002E8 RID: 744
		private bool _isInfoBarEnabled;

		// Token: 0x040002E9 RID: 745
		private HintViewModel _extendHint;

		// Token: 0x040002EA RID: 746
		private MBBindingList<MapInfoItemVM> _primaryInfoItems;

		// Token: 0x040002EB RID: 747
		private MBBindingList<MapInfoItemVM> _secondaryInfoItems;
	}
}
