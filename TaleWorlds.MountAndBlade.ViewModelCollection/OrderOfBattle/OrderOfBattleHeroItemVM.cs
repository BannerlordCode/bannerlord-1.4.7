using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x02000034 RID: 52
	public class OrderOfBattleHeroItemVM : ViewModel
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x0000F5AF File Offset: 0x0000D7AF
		// (set) Token: 0x0600041D RID: 1053 RVA: 0x0000F5B7 File Offset: 0x0000D7B7
		public ItemObject BannerOfHero { get; private set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x0000F5C0 File Offset: 0x0000D7C0
		// (set) Token: 0x0600041F RID: 1055 RVA: 0x0000F5C8 File Offset: 0x0000D7C8
		public bool IsAssignedBeforePlayer { get; private set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x0000F5D1 File Offset: 0x0000D7D1
		// (set) Token: 0x06000421 RID: 1057 RVA: 0x0000F5D9 File Offset: 0x0000D7D9
		public Formation InitialFormation { get; private set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x0000F5E2 File Offset: 0x0000D7E2
		// (set) Token: 0x06000423 RID: 1059 RVA: 0x0000F5EA File Offset: 0x0000D7EA
		public OrderOfBattleFormationItemVM InitialFormationItem { get; private set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x0000F5F3 File Offset: 0x0000D7F3
		// (set) Token: 0x06000425 RID: 1061 RVA: 0x0000F5FC File Offset: 0x0000D7FC
		public OrderOfBattleFormationItemVM CurrentAssignedFormationItem
		{
			get
			{
				return this._currentAssignedFormationItem;
			}
			set
			{
				if (value != this._currentAssignedFormationItem)
				{
					this._currentAssignedFormationItem = value;
					if (this._currentAssignedFormationItem == null)
					{
						this.OnAssignmentRemoved();
					}
					this.IsAssignedToAFormation = this._currentAssignedFormationItem != null;
					this.IsLeadingAFormation = this._currentAssignedFormationItem != null && this._currentAssignedFormationItem.Formation.Captain == this.Agent;
					this.OnAssignedFormationChanged();
				}
			}
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000F665 File Offset: 0x0000D865
		public OrderOfBattleHeroItemVM()
		{
			this.IsDisabled = true;
			this.RefreshValues();
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000F69C File Offset: 0x0000D89C
		public OrderOfBattleHeroItemVM(Agent agent)
		{
			this.Agent = agent;
			this.BannerOfHero = agent.FormationBanner;
			this.IsDisabled = !Mission.Current.PlayerTeam.IsPlayerGeneral && !agent.IsMainAgent;
			this.IsShown = true;
			this.IsMainHero = this.Agent.IsMainAgent;
			this.ImageIdentifier = new CharacterImageIdentifierVM(CharacterCode.CreateFrom(this.Agent.Character));
			this.Tooltip = new BasicTooltipViewModel(() => this.GetCaptainTooltip());
			this.RefreshValues();
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000F757 File Offset: 0x0000D957
		public void SetInitialFormation(OrderOfBattleFormationItemVM formation)
		{
			if (this.InitialFormationItem != null)
			{
				Debug.FailedAssert("Initial formation for hero is already set", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\OrderOfBattle\\OrderOfBattleHeroItemVM.cs", "SetInitialFormation", 77);
			}
			if (formation != null)
			{
				this.InitialFormationItem = formation;
				this.InitialFormation = formation.Formation;
			}
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000F78D File Offset: 0x0000D98D
		public override void RefreshValues()
		{
			Func<Agent, List<TooltipProperty>> getAgentTooltip = OrderOfBattleHeroItemVM.GetAgentTooltip;
			this._cachedTooltipProperties = ((getAgentTooltip != null) ? getAgentTooltip(this.Agent) : null);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000F7AC File Offset: 0x0000D9AC
		private List<TooltipProperty> GetCaptainTooltip()
		{
			return this._cachedTooltipProperties;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000F7B4 File Offset: 0x0000D9B4
		public void OnAssignmentRemoved()
		{
			if (this.CurrentAssignedFormationItem != null)
			{
				this.CurrentAssignedFormationItem.Formation.Refresh();
			}
			if (this.InitialFormation != null)
			{
				this.Agent.Formation = this.InitialFormation;
				this.InitialFormation.Refresh();
				if (this.Agent.IsDetachableFromFormation)
				{
					this.Agent.Team.DetachmentManager.RemoveScoresOfAgentFromDetachments(this.Agent);
				}
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000F825 File Offset: 0x0000DA25
		public void RefreshInformation()
		{
			if (this.Agent != null)
			{
				this.ImageIdentifier = new CharacterImageIdentifierVM(CharacterCode.CreateFrom(this.Agent.Character));
				return;
			}
			this.ImageIdentifier = new CharacterImageIdentifierVM(CharacterCode.CreateEmpty());
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000F85B File Offset: 0x0000DA5B
		private void OnAssignedFormationChanged()
		{
			Action<OrderOfBattleHeroItemVM> onHeroAssignedFormationChanged = OrderOfBattleHeroItemVM.OnHeroAssignedFormationChanged;
			if (onHeroAssignedFormationChanged != null)
			{
				onHeroAssignedFormationChanged(this);
			}
			this.RefreshAssignmentInfo();
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000F874 File Offset: 0x0000DA74
		public void RefreshAssignmentInfo()
		{
			if (!this.IsLeadingAFormation)
			{
				this.HasMismatchedAssignment = false;
				return;
			}
			DeploymentFormationClass orderOfBattleClass = this.CurrentAssignedFormationItem.GetOrderOfBattleClass();
			if (this.Agent.HasMount)
			{
				if (orderOfBattleClass == DeploymentFormationClass.Infantry || orderOfBattleClass == DeploymentFormationClass.Ranged || orderOfBattleClass == DeploymentFormationClass.InfantryAndRanged)
				{
					this.HasMismatchedAssignment = true;
					this.MismatchedAssignmentDescriptionText = this._mismatchMountedText.ToString();
					return;
				}
			}
			else if (orderOfBattleClass == DeploymentFormationClass.Cavalry || orderOfBattleClass == DeploymentFormationClass.HorseArcher || orderOfBattleClass == DeploymentFormationClass.CavalryAndHorseArcher)
			{
				this.HasMismatchedAssignment = true;
				this.MismatchedAssignmentDescriptionText = this._mismatchDismountedText.ToString();
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000F8F3 File Offset: 0x0000DAF3
		public void SetIsPreAssigned(bool isPreAssigned)
		{
			this.IsAssignedBeforePlayer = isPreAssigned;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000F8FC File Offset: 0x0000DAFC
		private void ExecuteSelection()
		{
			Action<OrderOfBattleHeroItemVM> onHeroSelection = OrderOfBattleHeroItemVM.OnHeroSelection;
			if (onHeroSelection == null)
			{
				return;
			}
			onHeroSelection(this);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000F90E File Offset: 0x0000DB0E
		private void ExecuteBeginAssignment()
		{
			Action<OrderOfBattleHeroItemVM> onHeroAssignmentBegin = OrderOfBattleHeroItemVM.OnHeroAssignmentBegin;
			if (onHeroAssignmentBegin == null)
			{
				return;
			}
			onHeroAssignmentBegin(this);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000F920 File Offset: 0x0000DB20
		private void ExecuteEndAssignment()
		{
			Action<OrderOfBattleHeroItemVM> onHeroAssignmentEnd = OrderOfBattleHeroItemVM.OnHeroAssignmentEnd;
			if (onHeroAssignmentEnd == null)
			{
				return;
			}
			onHeroAssignmentEnd(this);
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x0000F932 File Offset: 0x0000DB32
		// (set) Token: 0x06000434 RID: 1076 RVA: 0x0000F93A File Offset: 0x0000DB3A
		[DataSourceProperty]
		public string MismatchedAssignmentDescriptionText
		{
			get
			{
				return this._mismatchedAssignmentDescriptionText;
			}
			set
			{
				if (value != this._mismatchedAssignmentDescriptionText)
				{
					this._mismatchedAssignmentDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "MismatchedAssignmentDescriptionText");
				}
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x0000F95D File Offset: 0x0000DB5D
		// (set) Token: 0x06000436 RID: 1078 RVA: 0x0000F965 File Offset: 0x0000DB65
		[DataSourceProperty]
		public bool IsAssignedToAFormation
		{
			get
			{
				return this._isAssignedToAFormation;
			}
			set
			{
				if (value != this._isAssignedToAFormation)
				{
					this._isAssignedToAFormation = value;
					base.OnPropertyChangedWithValue(value, "IsAssignedToAFormation");
				}
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x0000F983 File Offset: 0x0000DB83
		// (set) Token: 0x06000438 RID: 1080 RVA: 0x0000F98B File Offset: 0x0000DB8B
		[DataSourceProperty]
		public bool IsLeadingAFormation
		{
			get
			{
				return this._isLeadingAFormation;
			}
			set
			{
				if (value != this._isLeadingAFormation)
				{
					this._isLeadingAFormation = value;
					base.OnPropertyChangedWithValue(value, "IsLeadingAFormation");
				}
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x0000F9A9 File Offset: 0x0000DBA9
		// (set) Token: 0x0600043A RID: 1082 RVA: 0x0000F9B1 File Offset: 0x0000DBB1
		[DataSourceProperty]
		public bool HasMismatchedAssignment
		{
			get
			{
				return this._hasMismatchedAssignment;
			}
			set
			{
				if (value != this._hasMismatchedAssignment)
				{
					this._hasMismatchedAssignment = value;
					base.OnPropertyChangedWithValue(value, "HasMismatchedAssignment");
				}
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x0000F9CF File Offset: 0x0000DBCF
		// (set) Token: 0x0600043C RID: 1084 RVA: 0x0000F9D7 File Offset: 0x0000DBD7
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x0000F9F5 File Offset: 0x0000DBF5
		// (set) Token: 0x0600043E RID: 1086 RVA: 0x0000F9FD File Offset: 0x0000DBFD
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				if (value != this._isDisabled)
				{
					this._isDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsDisabled");
				}
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x0000FA1B File Offset: 0x0000DC1B
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x0000FA23 File Offset: 0x0000DC23
		public bool IsShown
		{
			get
			{
				return this._isShown;
			}
			set
			{
				if (value != this._isShown)
				{
					this._isShown = value;
					base.OnPropertyChangedWithValue(value, "IsShown");
				}
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x0000FA41 File Offset: 0x0000DC41
		// (set) Token: 0x06000442 RID: 1090 RVA: 0x0000FA49 File Offset: 0x0000DC49
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

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x0000FA67 File Offset: 0x0000DC67
		// (set) Token: 0x06000444 RID: 1092 RVA: 0x0000FA6F File Offset: 0x0000DC6F
		[DataSourceProperty]
		public CharacterImageIdentifierVM ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (value != this._imageIdentifier)
				{
					this._imageIdentifier = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "ImageIdentifier");
				}
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x0000FA8D File Offset: 0x0000DC8D
		// (set) Token: 0x06000446 RID: 1094 RVA: 0x0000FA95 File Offset: 0x0000DC95
		[DataSourceProperty]
		public BasicTooltipViewModel Tooltip
		{
			get
			{
				return this._tooltip;
			}
			set
			{
				if (value != this._tooltip)
				{
					this._tooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Tooltip");
				}
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x0000FAB3 File Offset: 0x0000DCB3
		// (set) Token: 0x06000448 RID: 1096 RVA: 0x0000FABB File Offset: 0x0000DCBB
		[DataSourceProperty]
		public bool IsHighlightActive
		{
			get
			{
				return this._isHighlightActive;
			}
			set
			{
				if (value != this._isHighlightActive)
				{
					this._isHighlightActive = value;
					base.OnPropertyChangedWithValue(value, "IsHighlightActive");
				}
			}
		}

		// Token: 0x040001E6 RID: 486
		private readonly TextObject _mismatchMountedText = new TextObject("{=J9V9YhkY}Captain is mounted!", null);

		// Token: 0x040001E7 RID: 487
		private readonly TextObject _mismatchDismountedText = new TextObject("{=ufjypmaX}Captain is not mounted!", null);

		// Token: 0x040001E8 RID: 488
		public static Action<OrderOfBattleHeroItemVM> OnHeroSelection;

		// Token: 0x040001E9 RID: 489
		public static Action<OrderOfBattleHeroItemVM> OnHeroAssignedFormationChanged;

		// Token: 0x040001EA RID: 490
		public static Func<Agent, List<TooltipProperty>> GetAgentTooltip;

		// Token: 0x040001EB RID: 491
		public static Action<OrderOfBattleHeroItemVM> OnHeroAssignmentBegin;

		// Token: 0x040001EC RID: 492
		public static Action<OrderOfBattleHeroItemVM> OnHeroAssignmentEnd;

		// Token: 0x040001ED RID: 493
		public readonly Agent Agent;

		// Token: 0x040001F2 RID: 498
		private List<TooltipProperty> _cachedTooltipProperties;

		// Token: 0x040001F3 RID: 499
		private OrderOfBattleFormationItemVM _currentAssignedFormationItem;

		// Token: 0x040001F4 RID: 500
		private string _mismatchedAssignmentDescriptionText;

		// Token: 0x040001F5 RID: 501
		private bool _isAssignedToAFormation;

		// Token: 0x040001F6 RID: 502
		private bool _isLeadingAFormation;

		// Token: 0x040001F7 RID: 503
		private bool _hasMismatchedAssignment;

		// Token: 0x040001F8 RID: 504
		private bool _isSelected;

		// Token: 0x040001F9 RID: 505
		private bool _isDisabled;

		// Token: 0x040001FA RID: 506
		private bool _isShown;

		// Token: 0x040001FB RID: 507
		private bool _isMainHero;

		// Token: 0x040001FC RID: 508
		private CharacterImageIdentifierVM _imageIdentifier;

		// Token: 0x040001FD RID: 509
		private BasicTooltipViewModel _tooltip;

		// Token: 0x040001FE RID: 510
		private bool _isHighlightActive;
	}
}
