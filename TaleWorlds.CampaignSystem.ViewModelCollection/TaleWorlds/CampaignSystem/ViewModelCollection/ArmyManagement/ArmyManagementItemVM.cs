using System;
using Helpers;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ArmyManagement
{
	// Token: 0x0200015C RID: 348
	public class ArmyManagementItemVM : ViewModel
	{
		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06002127 RID: 8487 RVA: 0x000789BF File Offset: 0x00076BBF
		public float DistInTime { get; }

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x06002128 RID: 8488 RVA: 0x000789C7 File Offset: 0x00076BC7
		public float _distance { get; }

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06002129 RID: 8489 RVA: 0x000789CF File Offset: 0x00076BCF
		public Clan Clan { get; }

		// Token: 0x0600212A RID: 8490 RVA: 0x000789D8 File Offset: 0x00076BD8
		public ArmyManagementItemVM(Action<ArmyManagementItemVM> onAddToCart, Action<ArmyManagementItemVM> onRemove, Action<ArmyManagementItemVM> onFocus, MobileParty mobileParty)
		{
			ArmyManagementCalculationModel armyManagementCalculationModel = Campaign.Current.Models.ArmyManagementCalculationModel;
			this._onAddToCart = onAddToCart;
			this._onRemove = onRemove;
			this._onFocus = onFocus;
			this.Party = mobileParty;
			this._eligibilityReason = TextObject.GetEmpty();
			this.ClanBanner = new BannerImageIdentifierVM(mobileParty.LeaderHero.ClanBanner, true);
			CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(mobileParty.LeaderHero.CharacterObject, false);
			this.LordFace = new CharacterImageIdentifierVM(characterCode);
			this.Relation = armyManagementCalculationModel.GetPartyRelation(mobileParty.LeaderHero);
			this.Strength = this.Party.Party.NumberOfHealthyMembers;
			this.ShipCount = this.Party.Ships.Count;
			this._distance = DistanceHelper.FindClosestDistanceFromMobilePartyToMobileParty(this.Party, MobileParty.MainParty, this.Party.NavigationCapability);
			if (MobileParty.MainParty.IsCurrentlyAtSea && !this.Party.HasNavalNavigationCapability)
			{
				this.DistInTime = 2.1474836E+09f;
			}
			else
			{
				this.DistInTime = (float)MathF.Ceiling(this._distance / this.Party.Speed);
				this.Cost = armyManagementCalculationModel.CalculatePartyInfluenceCost(MobileParty.MainParty, mobileParty);
			}
			this.Clan = mobileParty.LeaderHero.Clan;
			this.IsMainHero = mobileParty.IsMainParty;
			this.UpdateEligibility();
			this.IsTransferDisabled = this.IsMainHero || PlayerSiege.PlayerSiegeEvent != null;
			this.RefreshValues();
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x00078B74 File Offset: 0x00076D74
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.InArmyText = GameTexts.FindText("str_in_army", null).ToString();
			this.LeaderNameText = this.Party.LeaderHero.Name.ToString();
			this.NameText = this.Party.Name.ToString();
			if (!this.Party.IsMainParty)
			{
				this.DistanceText = (((int)this._distance < 5) ? GameTexts.FindText("str_nearby", null).ToString() : CampaignUIHelper.GetPartyDistanceByTimeTextAbbreviated((float)((int)this._distance), this.Party.Speed));
			}
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x00078C15 File Offset: 0x00076E15
		public void ExecuteAction()
		{
			if (this.IsInCart)
			{
				this.OnRemove();
				return;
			}
			this.OnAddToCart();
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x00078C2C File Offset: 0x00076E2C
		private void OnRemove()
		{
			if (!this.IsMainHero)
			{
				this._onRemove(this);
				this.UpdateEligibility();
			}
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x00078C48 File Offset: 0x00076E48
		private void OnAddToCart()
		{
			this.UpdateEligibility();
			if (this.IsEligible)
			{
				this._onAddToCart(this);
			}
			this.UpdateEligibility();
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x00078C6A File Offset: 0x00076E6A
		public void ExecuteSetFocused()
		{
			this.IsFocused = true;
			Action<ArmyManagementItemVM> onFocus = this._onFocus;
			if (onFocus == null)
			{
				return;
			}
			onFocus(this);
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x00078C84 File Offset: 0x00076E84
		public void ExecuteSetUnfocused()
		{
			this.IsFocused = false;
			Action<ArmyManagementItemVM> onFocus = this._onFocus;
			if (onFocus == null)
			{
				return;
			}
			onFocus(null);
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x00078CA0 File Offset: 0x00076EA0
		public void UpdateEligibility()
		{
			GameModels models = Campaign.Current.Models;
			ArmyManagementCalculationModel armyManagementCalculationModel = ((models != null) ? models.ArmyManagementCalculationModel : null);
			bool flag = true;
			this._eligibilityReason = TextObject.GetEmpty();
			if (!this.CanJoinBackWithoutCost)
			{
				if (this.IsInCart && !this.IsAlreadyWithPlayer)
				{
					flag = false;
					this._eligibilityReason = new TextObject("{=idRXFzQ6}Already added to the army.", null);
				}
				else
				{
					flag = armyManagementCalculationModel.CheckPartyEligibility(this.Party, out this._eligibilityReason);
					if (flag)
					{
						flag = CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out this._eligibilityReason);
					}
				}
			}
			this.IsEligible = flag;
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x00078D27 File Offset: 0x00076F27
		private void UpdateIsCostRelevant()
		{
			if (this.Cost == 0 && this.IsAlreadyWithPlayer && this.IsInCart)
			{
				this.IsCostRelevant = false;
				return;
			}
			this.IsCostRelevant = true;
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x00078D50 File Offset: 0x00076F50
		public void ExecuteBeginHint()
		{
			if (!this.IsEligible)
			{
				MBInformationManager.ShowHint(this._eligibilityReason.ToString());
				return;
			}
			InformationManager.ShowTooltip(typeof(MobileParty), new object[] { this.Party, true, true });
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x00078DA6 File Offset: 0x00076FA6
		public void ExecuteBeginClanHint()
		{
			Type typeFromHandle = typeof(Clan);
			object[] array = new object[3];
			int num = 0;
			MobileParty party = this.Party;
			array[num] = ((party != null) ? party.ActualClan : null);
			array[1] = true;
			array[2] = true;
			InformationManager.ShowTooltip(typeFromHandle, array);
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x00078DE4 File Offset: 0x00076FE4
		public void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x00078DEB File Offset: 0x00076FEB
		public void ExecuteOpenEncyclopedia()
		{
			MobileParty party = this.Party;
			if (((party != null) ? party.LeaderHero : null) != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this.Party.LeaderHero.EncyclopediaLink);
			}
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x00078E20 File Offset: 0x00077020
		public void ExecuteOpenClanEncyclopedia()
		{
			MobileParty party = this.Party;
			if (((party != null) ? party.ActualClan : null) != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this.Party.ActualClan.EncyclopediaLink);
			}
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06002138 RID: 8504 RVA: 0x00078E55 File Offset: 0x00077055
		// (set) Token: 0x06002139 RID: 8505 RVA: 0x00078E5D File Offset: 0x0007705D
		[DataSourceProperty]
		public InputKeyItemVM RemoveInputKey
		{
			get
			{
				return this._removeInputKey;
			}
			set
			{
				if (value != this._removeInputKey)
				{
					this._removeInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "RemoveInputKey");
				}
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x0600213A RID: 8506 RVA: 0x00078E7B File Offset: 0x0007707B
		// (set) Token: 0x0600213B RID: 8507 RVA: 0x00078E83 File Offset: 0x00077083
		[DataSourceProperty]
		public bool IsEligible
		{
			get
			{
				return this._isEligible;
			}
			set
			{
				if (value != this._isEligible)
				{
					this._isEligible = value;
					base.OnPropertyChangedWithValue(value, "IsEligible");
				}
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x0600213C RID: 8508 RVA: 0x00078EA1 File Offset: 0x000770A1
		// (set) Token: 0x0600213D RID: 8509 RVA: 0x00078EA9 File Offset: 0x000770A9
		[DataSourceProperty]
		public bool IsInCart
		{
			get
			{
				return this._isInCart;
			}
			set
			{
				if (value != this._isInCart)
				{
					this._isInCart = value;
					base.OnPropertyChangedWithValue(value, "IsInCart");
					this.UpdateIsCostRelevant();
				}
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x0600213E RID: 8510 RVA: 0x00078ECD File Offset: 0x000770CD
		// (set) Token: 0x0600213F RID: 8511 RVA: 0x00078ED5 File Offset: 0x000770D5
		[DataSourceProperty]
		public bool IsMainHero
		{
			get
			{
				return this._isMainHero;
			}
			set
			{
				if (value != this._isMainHero)
				{
					this._isMainHero = value;
					base.OnPropertyChangedWithValue(value, "IsMainHero");
				}
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x06002140 RID: 8512 RVA: 0x00078EF3 File Offset: 0x000770F3
		// (set) Token: 0x06002141 RID: 8513 RVA: 0x00078EFB File Offset: 0x000770FB
		[DataSourceProperty]
		public int Strength
		{
			get
			{
				return this._strength;
			}
			set
			{
				if (value != this._strength)
				{
					this._strength = value;
					base.OnPropertyChangedWithValue(value, "Strength");
				}
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x06002142 RID: 8514 RVA: 0x00078F19 File Offset: 0x00077119
		// (set) Token: 0x06002143 RID: 8515 RVA: 0x00078F21 File Offset: 0x00077121
		[DataSourceProperty]
		public int ShipCount
		{
			get
			{
				return this._shipCount;
			}
			set
			{
				if (value != this._shipCount)
				{
					this._shipCount = value;
					base.OnPropertyChangedWithValue(value, "ShipCount");
					this.HasShip = this._shipCount > 0;
				}
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x06002144 RID: 8516 RVA: 0x00078F4E File Offset: 0x0007714E
		// (set) Token: 0x06002145 RID: 8517 RVA: 0x00078F56 File Offset: 0x00077156
		[DataSourceProperty]
		public bool HasShip
		{
			get
			{
				return this._hasShip;
			}
			set
			{
				if (value != this._hasShip)
				{
					this._hasShip = value;
					base.OnPropertyChangedWithValue(value, "HasShip");
				}
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x06002146 RID: 8518 RVA: 0x00078F74 File Offset: 0x00077174
		// (set) Token: 0x06002147 RID: 8519 RVA: 0x00078F7C File Offset: 0x0007717C
		[DataSourceProperty]
		public string DistanceText
		{
			get
			{
				return this._distanceText;
			}
			set
			{
				if (value != this._distanceText)
				{
					this._distanceText = value;
					base.OnPropertyChangedWithValue<string>(value, "DistanceText");
				}
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06002148 RID: 8520 RVA: 0x00078F9F File Offset: 0x0007719F
		// (set) Token: 0x06002149 RID: 8521 RVA: 0x00078FA7 File Offset: 0x000771A7
		[DataSourceProperty]
		public string InArmyText
		{
			get
			{
				return this._inArmyText;
			}
			set
			{
				if (value != this._inArmyText)
				{
					this._inArmyText = value;
					base.OnPropertyChangedWithValue<string>(value, "InArmyText");
				}
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x0600214A RID: 8522 RVA: 0x00078FCA File Offset: 0x000771CA
		// (set) Token: 0x0600214B RID: 8523 RVA: 0x00078FD2 File Offset: 0x000771D2
		[DataSourceProperty]
		public int Cost
		{
			get
			{
				return this._cost;
			}
			set
			{
				if (value != this._cost)
				{
					this._cost = value;
					base.OnPropertyChangedWithValue(value, "Cost");
					this.UpdateIsCostRelevant();
				}
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x0600214C RID: 8524 RVA: 0x00078FF6 File Offset: 0x000771F6
		// (set) Token: 0x0600214D RID: 8525 RVA: 0x00078FFE File Offset: 0x000771FE
		[DataSourceProperty]
		public bool IsCostRelevant
		{
			get
			{
				return this._isCostRelevant;
			}
			set
			{
				if (value != this._isCostRelevant)
				{
					this._isCostRelevant = value;
					base.OnPropertyChangedWithValue(value, "IsCostRelevant");
				}
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x0600214E RID: 8526 RVA: 0x0007901C File Offset: 0x0007721C
		// (set) Token: 0x0600214F RID: 8527 RVA: 0x00079024 File Offset: 0x00077224
		[DataSourceProperty]
		public int Relation
		{
			get
			{
				return this._relation;
			}
			set
			{
				if (value != this._relation)
				{
					this._relation = value;
					base.OnPropertyChangedWithValue(value, "Relation");
				}
			}
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06002150 RID: 8528 RVA: 0x00079042 File Offset: 0x00077242
		// (set) Token: 0x06002151 RID: 8529 RVA: 0x0007904A File Offset: 0x0007724A
		[DataSourceProperty]
		public BannerImageIdentifierVM ClanBanner
		{
			get
			{
				return this._clanBanner;
			}
			set
			{
				if (value != this._clanBanner)
				{
					this._clanBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "ClanBanner");
				}
			}
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x06002152 RID: 8530 RVA: 0x00079068 File Offset: 0x00077268
		// (set) Token: 0x06002153 RID: 8531 RVA: 0x00079070 File Offset: 0x00077270
		[DataSourceProperty]
		public CharacterImageIdentifierVM LordFace
		{
			get
			{
				return this._lordFace;
			}
			set
			{
				if (value != this._lordFace)
				{
					this._lordFace = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "LordFace");
				}
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x06002154 RID: 8532 RVA: 0x0007908E File Offset: 0x0007728E
		// (set) Token: 0x06002155 RID: 8533 RVA: 0x00079096 File Offset: 0x00077296
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

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x06002156 RID: 8534 RVA: 0x000790B9 File Offset: 0x000772B9
		// (set) Token: 0x06002157 RID: 8535 RVA: 0x000790C1 File Offset: 0x000772C1
		[DataSourceProperty]
		public bool IsAlreadyWithPlayer
		{
			get
			{
				return this._isAlreadyWithPlayer;
			}
			set
			{
				if (value != this._isAlreadyWithPlayer)
				{
					this._isAlreadyWithPlayer = value;
					base.OnPropertyChangedWithValue(value, "IsAlreadyWithPlayer");
					this.UpdateIsCostRelevant();
				}
			}
		}

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x06002158 RID: 8536 RVA: 0x000790E5 File Offset: 0x000772E5
		// (set) Token: 0x06002159 RID: 8537 RVA: 0x000790ED File Offset: 0x000772ED
		[DataSourceProperty]
		public bool IsTransferDisabled
		{
			get
			{
				return this._isTransferDisabled;
			}
			set
			{
				if (value != this._isTransferDisabled)
				{
					this._isTransferDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsTransferDisabled");
				}
			}
		}

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x0600215A RID: 8538 RVA: 0x0007910B File Offset: 0x0007730B
		// (set) Token: 0x0600215B RID: 8539 RVA: 0x00079113 File Offset: 0x00077313
		[DataSourceProperty]
		public string LeaderNameText
		{
			get
			{
				return this._leaderNameText;
			}
			set
			{
				if (value != this._leaderNameText)
				{
					this._leaderNameText = value;
					base.OnPropertyChangedWithValue<string>(value, "LeaderNameText");
				}
			}
		}

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x0600215C RID: 8540 RVA: 0x00079136 File Offset: 0x00077336
		// (set) Token: 0x0600215D RID: 8541 RVA: 0x0007913E File Offset: 0x0007733E
		[DataSourceProperty]
		public bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
			set
			{
				if (value != this._isFocused)
				{
					this._isFocused = value;
					base.OnPropertyChangedWithValue(value, "IsFocused");
				}
			}
		}

		// Token: 0x04000F68 RID: 3944
		private readonly Action<ArmyManagementItemVM> _onAddToCart;

		// Token: 0x04000F69 RID: 3945
		private readonly Action<ArmyManagementItemVM> _onRemove;

		// Token: 0x04000F6A RID: 3946
		private readonly Action<ArmyManagementItemVM> _onFocus;

		// Token: 0x04000F6B RID: 3947
		public readonly MobileParty Party;

		// Token: 0x04000F6C RID: 3948
		private const float _minimumPartySizeScoreNeeded = 0.4f;

		// Token: 0x04000F6D RID: 3949
		public bool CanJoinBackWithoutCost;

		// Token: 0x04000F6E RID: 3950
		private TextObject _eligibilityReason;

		// Token: 0x04000F6F RID: 3951
		private InputKeyItemVM _removeInputKey;

		// Token: 0x04000F70 RID: 3952
		private BannerImageIdentifierVM _clanBanner;

		// Token: 0x04000F71 RID: 3953
		private CharacterImageIdentifierVM _lordFace;

		// Token: 0x04000F72 RID: 3954
		private string _nameText;

		// Token: 0x04000F73 RID: 3955
		private string _inArmyText;

		// Token: 0x04000F74 RID: 3956
		private string _leaderNameText;

		// Token: 0x04000F75 RID: 3957
		private int _relation = -102;

		// Token: 0x04000F76 RID: 3958
		private int _strength = -1;

		// Token: 0x04000F77 RID: 3959
		private int _shipCount = -1;

		// Token: 0x04000F78 RID: 3960
		private bool _hasShip;

		// Token: 0x04000F79 RID: 3961
		private string _distanceText;

		// Token: 0x04000F7A RID: 3962
		private int _cost = -1;

		// Token: 0x04000F7B RID: 3963
		private bool _isCostRelevant;

		// Token: 0x04000F7C RID: 3964
		private bool _isEligible;

		// Token: 0x04000F7D RID: 3965
		private bool _isMainHero;

		// Token: 0x04000F7E RID: 3966
		private bool _isInCart;

		// Token: 0x04000F7F RID: 3967
		private bool _isAlreadyWithPlayer;

		// Token: 0x04000F80 RID: 3968
		private bool _isTransferDisabled;

		// Token: 0x04000F81 RID: 3969
		private bool _isFocused;
	}
}
