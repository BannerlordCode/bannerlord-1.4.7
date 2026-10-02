using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Armies
{
	// Token: 0x0200008C RID: 140
	public class KingdomArmyVM : KingdomCategoryVM
	{
		// Token: 0x06000BEC RID: 3052 RVA: 0x00031880 File Offset: 0x0002FA80
		public KingdomArmyVM(Action onManageArmy, Action refreshDecision, Action<Army> showArmyOnMap)
		{
			this._onManageArmy = onManageArmy;
			this._refreshDecision = refreshDecision;
			this._showArmyOnMap = showArmyOnMap;
			this._viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			this._armies = new MBBindingList<KingdomArmyItemVM>();
			this.PlayerHasArmy = MobileParty.MainParty.Army != null;
			this.ChangeLeaderCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfChangingLeaderOfArmy();
			this.DisbandCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfDisbandingArmy();
			this.CreateArmyHint = new HintViewModel();
			this.DisbandHint = new HintViewModel();
			this.ManageArmyHint = new HintViewModel();
			base.IsAcceptableItemSelected = false;
			this.RefreshArmyList();
			this.ArmySortController = new KingdomArmySortControllerVM(ref this._armies);
			this.RefreshValues();
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00031950 File Offset: 0x0002FB50
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ArmyNameText = GameTexts.FindText("str_sort_by_army_name_label", null).ToString();
			this.LeaderText = GameTexts.FindText("str_sort_by_leader_name_label", null).ToString();
			this.StrengthText = GameTexts.FindText("str_men", null).ToString();
			this.LocationText = GameTexts.FindText("str_tooltip_label_location", null).ToString();
			base.NoItemSelectedText = GameTexts.FindText("str_kingdom_no_army_selected", null).ToString();
			this.DisbandActionExplanationText = GameTexts.FindText("str_kingdom_disband_army_explanation", null).ToString();
			this.ManageActionExplanationText = GameTexts.FindText("str_kingdom_manage_army_explanation", null).ToString();
			this.ManageText = GameTexts.FindText("str_manage", null).ToString();
			this.CreateArmyText = (this.PlayerHasArmy ? new TextObject("{=DAmdTxuC}Army Manage", null).ToString() : new TextObject("{=lc9s4rLZ}Create Army", null).ToString());
			base.CategoryNameText = new TextObject("{=j12VrGKz}Army", null).ToString();
			this.ChangeLeaderText = new TextObject("{=NcYbdiyT}Change Leader", null).ToString();
			this.PartiesText = new TextObject("{=t3tq0eoW}Parties", null).ToString();
			this.DisbandText = new TextObject("{=xXSFaGW8}Disband", null).ToString();
			this.ShowOnMapText = GameTexts.FindText("str_show_on_map", null).ToString();
			this.CreateArmyText = new TextObject("{=lc9s4rLZ}Create Army", null).ToString();
			this.Armies.ApplyActionOnAllItems(delegate(KingdomArmyItemVM x)
			{
				x.RefreshValues();
			});
			KingdomArmyItemVM currentSelectedArmy = this.CurrentSelectedArmy;
			if (currentSelectedArmy == null)
			{
				return;
			}
			currentSelectedArmy.RefreshValues();
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x00031B04 File Offset: 0x0002FD04
		public void RefreshArmyList()
		{
			base.NotificationCount = this._viewDataTracker.NumOfKingdomArmyNotifications;
			this._kingdom = Hero.MainHero.MapFaction as Kingdom;
			if (this._kingdom != null)
			{
				this.Armies.Clear();
				using (List<Army>.Enumerator enumerator = this._kingdom.Armies.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Army army = enumerator.Current;
						this.Armies.Add(new KingdomArmyItemVM(army, new Action<KingdomArmyItemVM>(this.OnSelection)));
					}
					goto IL_00A0;
				}
			}
			Debug.FailedAssert("Kingdom screen can't open if you're not in kingdom", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\KingdomManagement\\Armies\\KingdomArmyVM.cs", "RefreshArmyList", 81);
			IL_00A0:
			this.RefreshCanManageArmy();
			if (this.Armies.Count == 0 && this.CurrentSelectedArmy != null)
			{
				this.OnSelection(null);
				return;
			}
			if (this.Armies.Count > 0)
			{
				this.OnSelection(this.Armies[0]);
				this.CurrentSelectedArmy.IsSelected = true;
			}
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00031C10 File Offset: 0x0002FE10
		private void ExecuteManageArmy()
		{
			this._onManageArmy();
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00031C1D File Offset: 0x0002FE1D
		private void ExecuteShowOnMap()
		{
			if (this.CurrentSelectedArmy != null)
			{
				this._showArmyOnMap(this.CurrentSelectedArmy.Army);
			}
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00031C40 File Offset: 0x0002FE40
		private void RefreshCurrentArmyVisuals(KingdomArmyItemVM item)
		{
			if (item != null)
			{
				if (this.CurrentSelectedArmy != null)
				{
					this.CurrentSelectedArmy.IsSelected = false;
				}
				this.CanManageCurrentArmy = false;
				this.CurrentSelectedArmy = item;
				base.NotificationCount = this._viewDataTracker.NumOfKingdomArmyNotifications;
				this.DisbandCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfDisbandingArmy();
				this.ChangeLeaderCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfChangingLeaderOfArmy();
				TextObject textObject;
				this.CanDisbandCurrentArmy = this.GetCanDisbandCurrentArmyWithReason(item, this.DisbandCost, out textObject);
				this.DisbandHint.HintText = textObject;
				this.DisbandActionExplanationText = GameTexts.FindText("str_kingdom_disband_army_explanation", null).ToString();
				if (this.CurrentSelectedArmy != null)
				{
					this.CanShowLocationOfCurrentArmy = this.CurrentSelectedArmy.Army.AiBehaviorObject is Settlement || this.CurrentSelectedArmy.Army.AiBehaviorObject is MobileParty;
					TextObject textObject2;
					this.CanManageCurrentArmy = this.GetCanManageCurrentArmyWithReason(out textObject2);
					this.ManageArmyHint.HintText = textObject2;
				}
			}
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00031D4B File Offset: 0x0002FF4B
		private bool GetCanManageCurrentArmyWithReason(out TextObject disabledReason)
		{
			KingdomArmyItemVM currentSelectedArmy = this.CurrentSelectedArmy;
			if (currentSelectedArmy == null || !currentSelectedArmy.IsMainArmy)
			{
				disabledReason = TextObject.GetEmpty();
				return false;
			}
			return CampaignUIHelper.GetCanManageCurrentArmyWithReason(out disabledReason);
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00031D74 File Offset: 0x0002FF74
		private bool GetCanDisbandCurrentArmyWithReason(KingdomArmyItemVM armyItem, int disbandCost, out TextObject disabledReason)
		{
			if (Clan.PlayerClan.IsUnderMercenaryService)
			{
				disabledReason = GameTexts.FindText("str_cannot_disband_army_while_mercenary", null);
				return false;
			}
			if (Clan.PlayerClan.Influence < (float)disbandCost)
			{
				disabledReason = GameTexts.FindText("str_warning_you_dont_have_enough_influence", null);
				return false;
			}
			if (armyItem.Army.LeaderParty.MapEvent != null)
			{
				disabledReason = GameTexts.FindText("str_cannot_disband_army_while_in_event", null);
				return false;
			}
			if (armyItem.Army.Parties.Contains(MobileParty.MainParty))
			{
				disabledReason = GameTexts.FindText("str_cannot_disband_army_while_in_that_army", null);
				return false;
			}
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00031E18 File Offset: 0x00030018
		public void SelectArmy(Army army)
		{
			foreach (KingdomArmyItemVM kingdomArmyItemVM in this.Armies)
			{
				if (kingdomArmyItemVM.Army == army)
				{
					this.OnSelection(kingdomArmyItemVM);
					break;
				}
			}
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x00031E70 File Offset: 0x00030070
		private void OnSelection(KingdomArmyItemVM item)
		{
			if (this.CurrentSelectedArmy != item)
			{
				this.RefreshCurrentArmyVisuals(item);
				this.CurrentSelectedArmy = item;
				base.IsAcceptableItemSelected = item != null;
			}
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00031E94 File Offset: 0x00030094
		private void ExecuteDisbandCurrentArmy()
		{
			if (this.CurrentSelectedArmy != null && Hero.MainHero.Clan.Influence >= (float)this.DisbandCost)
			{
				InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_disband_army", null).ToString(), new TextObject("{=zrhr4rDA}Are you sure you want to disband this army? This will result in relation loss.", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.DisbandCurrentArmy), null, "", 0f, null, null, null), false, false);
			}
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x00031F2C File Offset: 0x0003012C
		private void DisbandCurrentArmy()
		{
			if (this.CurrentSelectedArmy != null && Hero.MainHero.Clan.Influence >= (float)this.DisbandCost)
			{
				DisbandArmyAction.ApplyByReleasedByPlayerAfterBattle(this.CurrentSelectedArmy.Army);
				this.RefreshArmyList();
			}
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00031F64 File Offset: 0x00030164
		private void RefreshCanManageArmy()
		{
			this.PlayerHasArmy = MobileParty.MainParty.Army != null;
			TextObject textObject;
			this.CanCreateArmy = Campaign.Current.Models.ArmyManagementCalculationModel.CanPlayerCreateArmy(out textObject);
			this.CreateArmyHint.HintText = textObject;
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000BF9 RID: 3065 RVA: 0x00031FAC File Offset: 0x000301AC
		// (set) Token: 0x06000BFA RID: 3066 RVA: 0x00031FB4 File Offset: 0x000301B4
		[DataSourceProperty]
		public KingdomArmySortControllerVM ArmySortController
		{
			get
			{
				return this._armySortController;
			}
			set
			{
				if (value != this._armySortController)
				{
					this._armySortController = value;
					base.OnPropertyChangedWithValue<KingdomArmySortControllerVM>(value, "ArmySortController");
				}
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x00031FD2 File Offset: 0x000301D2
		// (set) Token: 0x06000BFC RID: 3068 RVA: 0x00031FDA File Offset: 0x000301DA
		[DataSourceProperty]
		public string CreateArmyText
		{
			get
			{
				return this._createArmyText;
			}
			set
			{
				if (value != this._createArmyText)
				{
					this._createArmyText = value;
					base.OnPropertyChangedWithValue<string>(value, "CreateArmyText");
				}
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000BFD RID: 3069 RVA: 0x00031FFD File Offset: 0x000301FD
		// (set) Token: 0x06000BFE RID: 3070 RVA: 0x00032005 File Offset: 0x00030205
		[DataSourceProperty]
		public string DisbandActionExplanationText
		{
			get
			{
				return this._disbandActionExplanationText;
			}
			set
			{
				if (value != this._disbandActionExplanationText)
				{
					this._disbandActionExplanationText = value;
					base.OnPropertyChangedWithValue<string>(value, "DisbandActionExplanationText");
				}
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000BFF RID: 3071 RVA: 0x00032028 File Offset: 0x00030228
		// (set) Token: 0x06000C00 RID: 3072 RVA: 0x00032030 File Offset: 0x00030230
		[DataSourceProperty]
		public string ManageActionExplanationText
		{
			get
			{
				return this._manageActionExplanationText;
			}
			set
			{
				if (value != this._manageActionExplanationText)
				{
					this._manageActionExplanationText = value;
					base.OnPropertyChangedWithValue<string>(value, "ManageActionExplanationText");
				}
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000C01 RID: 3073 RVA: 0x00032053 File Offset: 0x00030253
		// (set) Token: 0x06000C02 RID: 3074 RVA: 0x0003205B File Offset: 0x0003025B
		[DataSourceProperty]
		public KingdomArmyItemVM CurrentSelectedArmy
		{
			get
			{
				return this._currentSelectedArmy;
			}
			set
			{
				if (value != this._currentSelectedArmy)
				{
					this._currentSelectedArmy = value;
					base.OnPropertyChangedWithValue<KingdomArmyItemVM>(value, "CurrentSelectedArmy");
				}
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000C03 RID: 3075 RVA: 0x00032079 File Offset: 0x00030279
		// (set) Token: 0x06000C04 RID: 3076 RVA: 0x00032081 File Offset: 0x00030281
		[DataSourceProperty]
		public HintViewModel CreateArmyHint
		{
			get
			{
				return this._createArmyHint;
			}
			set
			{
				if (value != this._createArmyHint)
				{
					this._createArmyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CreateArmyHint");
				}
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000C05 RID: 3077 RVA: 0x0003209F File Offset: 0x0003029F
		// (set) Token: 0x06000C06 RID: 3078 RVA: 0x000320A7 File Offset: 0x000302A7
		[DataSourceProperty]
		public HintViewModel ManageArmyHint
		{
			get
			{
				return this._manageArmyHint;
			}
			set
			{
				if (value != this._manageArmyHint)
				{
					this._manageArmyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ManageArmyHint");
				}
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000C07 RID: 3079 RVA: 0x000320C5 File Offset: 0x000302C5
		// (set) Token: 0x06000C08 RID: 3080 RVA: 0x000320CD File Offset: 0x000302CD
		[DataSourceProperty]
		public bool PlayerHasArmy
		{
			get
			{
				return this._playerHasArmy;
			}
			set
			{
				if (value != this._playerHasArmy)
				{
					this._playerHasArmy = value;
					base.OnPropertyChangedWithValue(value, "PlayerHasArmy");
				}
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000C09 RID: 3081 RVA: 0x000320EB File Offset: 0x000302EB
		// (set) Token: 0x06000C0A RID: 3082 RVA: 0x000320F3 File Offset: 0x000302F3
		[DataSourceProperty]
		public bool CanCreateArmy
		{
			get
			{
				return this._canCreateArmy;
			}
			set
			{
				if (value != this._canCreateArmy)
				{
					this._canCreateArmy = value;
					base.OnPropertyChangedWithValue(value, "CanCreateArmy");
				}
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000C0B RID: 3083 RVA: 0x00032111 File Offset: 0x00030311
		// (set) Token: 0x06000C0C RID: 3084 RVA: 0x00032119 File Offset: 0x00030319
		[DataSourceProperty]
		public string LeaderText
		{
			get
			{
				return this._categoryLeaderName;
			}
			set
			{
				if (value != this._categoryLeaderName)
				{
					this._categoryLeaderName = value;
					base.OnPropertyChanged("CategoryLeaderName");
				}
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000C0D RID: 3085 RVA: 0x0003213B File Offset: 0x0003033B
		// (set) Token: 0x06000C0E RID: 3086 RVA: 0x00032143 File Offset: 0x00030343
		[DataSourceProperty]
		public string ShowOnMapText
		{
			get
			{
				return this._showOnMapText;
			}
			set
			{
				if (value != this._showOnMapText)
				{
					this._showOnMapText = value;
					base.OnPropertyChangedWithValue<string>(value, "ShowOnMapText");
				}
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000C0F RID: 3087 RVA: 0x00032166 File Offset: 0x00030366
		// (set) Token: 0x06000C10 RID: 3088 RVA: 0x0003216E File Offset: 0x0003036E
		[DataSourceProperty]
		public string ArmyNameText
		{
			get
			{
				return this._categoryLordCount;
			}
			set
			{
				if (value != this._categoryLordCount)
				{
					this._categoryLordCount = value;
					base.OnPropertyChanged("CategoryLordCount");
				}
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000C11 RID: 3089 RVA: 0x00032190 File Offset: 0x00030390
		// (set) Token: 0x06000C12 RID: 3090 RVA: 0x00032198 File Offset: 0x00030398
		[DataSourceProperty]
		public string StrengthText
		{
			get
			{
				return this._categoryStrength;
			}
			set
			{
				if (value != this._categoryStrength)
				{
					this._categoryStrength = value;
					base.OnPropertyChanged("CategoryStrength");
				}
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000C13 RID: 3091 RVA: 0x000321BA File Offset: 0x000303BA
		// (set) Token: 0x06000C14 RID: 3092 RVA: 0x000321C2 File Offset: 0x000303C2
		[DataSourceProperty]
		public string PartiesText
		{
			get
			{
				return this._categoryParties;
			}
			set
			{
				if (value != this._categoryParties)
				{
					this._categoryParties = value;
					base.OnPropertyChangedWithValue<string>(value, "PartiesText");
				}
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000C15 RID: 3093 RVA: 0x000321E5 File Offset: 0x000303E5
		// (set) Token: 0x06000C16 RID: 3094 RVA: 0x000321ED File Offset: 0x000303ED
		[DataSourceProperty]
		public string LocationText
		{
			get
			{
				return this._categoryObjective;
			}
			set
			{
				if (value != this._categoryObjective)
				{
					this._categoryObjective = value;
					base.OnPropertyChanged("CategoryObjective");
				}
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000C17 RID: 3095 RVA: 0x0003220F File Offset: 0x0003040F
		// (set) Token: 0x06000C18 RID: 3096 RVA: 0x00032217 File Offset: 0x00030417
		[DataSourceProperty]
		public MBBindingList<KingdomArmyItemVM> Armies
		{
			get
			{
				return this._armies;
			}
			set
			{
				if (value != this._armies)
				{
					this._armies = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomArmyItemVM>>(value, "Armies");
				}
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000C19 RID: 3097 RVA: 0x00032235 File Offset: 0x00030435
		// (set) Token: 0x06000C1A RID: 3098 RVA: 0x0003223D File Offset: 0x0003043D
		[DataSourceProperty]
		public bool CanDisbandCurrentArmy
		{
			get
			{
				return this._canDisbandCurrentArmy;
			}
			set
			{
				if (value != this._canDisbandCurrentArmy)
				{
					this._canDisbandCurrentArmy = value;
					base.OnPropertyChangedWithValue(value, "CanDisbandCurrentArmy");
				}
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000C1B RID: 3099 RVA: 0x0003225B File Offset: 0x0003045B
		// (set) Token: 0x06000C1C RID: 3100 RVA: 0x00032263 File Offset: 0x00030463
		[DataSourceProperty]
		public bool CanManageCurrentArmy
		{
			get
			{
				return this._canManageCurrentArmy;
			}
			set
			{
				if (value != this._canManageCurrentArmy)
				{
					this._canManageCurrentArmy = value;
					base.OnPropertyChangedWithValue(value, "CanManageCurrentArmy");
				}
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000C1D RID: 3101 RVA: 0x00032281 File Offset: 0x00030481
		// (set) Token: 0x06000C1E RID: 3102 RVA: 0x00032289 File Offset: 0x00030489
		[DataSourceProperty]
		public bool CanChangeLeaderOfCurrentArmy
		{
			get
			{
				return this._canChangeLeaderOfCurrentArmy;
			}
			set
			{
				if (value != this._canChangeLeaderOfCurrentArmy)
				{
					this._canChangeLeaderOfCurrentArmy = value;
					base.OnPropertyChangedWithValue(value, "CanChangeLeaderOfCurrentArmy");
				}
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000C1F RID: 3103 RVA: 0x000322A7 File Offset: 0x000304A7
		// (set) Token: 0x06000C20 RID: 3104 RVA: 0x000322AF File Offset: 0x000304AF
		[DataSourceProperty]
		public bool CanShowLocationOfCurrentArmy
		{
			get
			{
				return this._canShowLocationOfCurrentArmy;
			}
			set
			{
				if (value != this._canShowLocationOfCurrentArmy)
				{
					this._canShowLocationOfCurrentArmy = value;
					base.OnPropertyChangedWithValue(value, "CanShowLocationOfCurrentArmy");
				}
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000C21 RID: 3105 RVA: 0x000322CD File Offset: 0x000304CD
		// (set) Token: 0x06000C22 RID: 3106 RVA: 0x000322D5 File Offset: 0x000304D5
		[DataSourceProperty]
		public string DisbandText
		{
			get
			{
				return this._disbandText;
			}
			set
			{
				if (value != this._disbandText)
				{
					this._disbandText = value;
					base.OnPropertyChangedWithValue<string>(value, "DisbandText");
				}
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000C23 RID: 3107 RVA: 0x000322F8 File Offset: 0x000304F8
		// (set) Token: 0x06000C24 RID: 3108 RVA: 0x00032300 File Offset: 0x00030500
		[DataSourceProperty]
		public string ManageText
		{
			get
			{
				return this._manageText;
			}
			set
			{
				if (value != this._manageText)
				{
					this._manageText = value;
					base.OnPropertyChangedWithValue<string>(value, "ManageText");
				}
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000C25 RID: 3109 RVA: 0x00032323 File Offset: 0x00030523
		// (set) Token: 0x06000C26 RID: 3110 RVA: 0x0003232B File Offset: 0x0003052B
		[DataSourceProperty]
		public int DisbandCost
		{
			get
			{
				return this._disbandCost;
			}
			set
			{
				if (value != this._disbandCost)
				{
					this._disbandCost = value;
					base.OnPropertyChangedWithValue(value, "DisbandCost");
				}
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000C27 RID: 3111 RVA: 0x00032349 File Offset: 0x00030549
		// (set) Token: 0x06000C28 RID: 3112 RVA: 0x00032351 File Offset: 0x00030551
		[DataSourceProperty]
		public string ChangeLeaderText
		{
			get
			{
				return this._changeLeaderText;
			}
			set
			{
				if (value != this._changeLeaderText)
				{
					this._changeLeaderText = value;
					base.OnPropertyChangedWithValue<string>(value, "ChangeLeaderText");
				}
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000C29 RID: 3113 RVA: 0x00032374 File Offset: 0x00030574
		// (set) Token: 0x06000C2A RID: 3114 RVA: 0x0003237C File Offset: 0x0003057C
		[DataSourceProperty]
		public int ChangeLeaderCost
		{
			get
			{
				return this._changeLeaderCost;
			}
			set
			{
				if (value != this._changeLeaderCost)
				{
					this._changeLeaderCost = value;
					base.OnPropertyChangedWithValue(value, "ChangeLeaderCost");
				}
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000C2B RID: 3115 RVA: 0x0003239A File Offset: 0x0003059A
		// (set) Token: 0x06000C2C RID: 3116 RVA: 0x000323A2 File Offset: 0x000305A2
		[DataSourceProperty]
		public HintViewModel DisbandHint
		{
			get
			{
				return this._disbandHint;
			}
			set
			{
				if (value != this._disbandHint)
				{
					this._disbandHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DisbandHint");
				}
			}
		}

		// Token: 0x04000554 RID: 1364
		private readonly Action _onManageArmy;

		// Token: 0x04000555 RID: 1365
		private readonly Action _refreshDecision;

		// Token: 0x04000556 RID: 1366
		private readonly Action<Army> _showArmyOnMap;

		// Token: 0x04000557 RID: 1367
		private readonly IViewDataTracker _viewDataTracker;

		// Token: 0x04000558 RID: 1368
		private Kingdom _kingdom;

		// Token: 0x04000559 RID: 1369
		private MBBindingList<KingdomArmyItemVM> _armies;

		// Token: 0x0400055A RID: 1370
		private KingdomArmyItemVM _currentSelectedArmy;

		// Token: 0x0400055B RID: 1371
		private HintViewModel _disbandHint;

		// Token: 0x0400055C RID: 1372
		private string _categoryLeaderName;

		// Token: 0x0400055D RID: 1373
		private string _categoryLordCount;

		// Token: 0x0400055E RID: 1374
		private string _categoryStrength;

		// Token: 0x0400055F RID: 1375
		private string _categoryObjective;

		// Token: 0x04000560 RID: 1376
		private string _categoryParties;

		// Token: 0x04000561 RID: 1377
		private string _createArmyText;

		// Token: 0x04000562 RID: 1378
		private string _disbandText;

		// Token: 0x04000563 RID: 1379
		private string _manageText;

		// Token: 0x04000564 RID: 1380
		private string _changeLeaderText;

		// Token: 0x04000565 RID: 1381
		private string _showOnMapText;

		// Token: 0x04000566 RID: 1382
		private string _disbandActionExplanationText;

		// Token: 0x04000567 RID: 1383
		private string _manageActionExplanationText;

		// Token: 0x04000568 RID: 1384
		private bool _canCreateArmy;

		// Token: 0x04000569 RID: 1385
		private bool _playerHasArmy;

		// Token: 0x0400056A RID: 1386
		private HintViewModel _createArmyHint;

		// Token: 0x0400056B RID: 1387
		private HintViewModel _manageArmyHint;

		// Token: 0x0400056C RID: 1388
		private bool _canChangeLeaderOfCurrentArmy;

		// Token: 0x0400056D RID: 1389
		private bool _canDisbandCurrentArmy;

		// Token: 0x0400056E RID: 1390
		private bool _canShowLocationOfCurrentArmy;

		// Token: 0x0400056F RID: 1391
		private bool _canManageCurrentArmy;

		// Token: 0x04000570 RID: 1392
		private int _disbandCost;

		// Token: 0x04000571 RID: 1393
		private int _changeLeaderCost;

		// Token: 0x04000572 RID: 1394
		private KingdomArmySortControllerVM _armySortController;
	}
}
