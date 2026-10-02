using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper
{
	// Token: 0x02000144 RID: 324
	public class PerkVM : ViewModel
	{
		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06001EFF RID: 7935 RVA: 0x000723F1 File Offset: 0x000705F1
		private bool _hasAlternativeAndSelected
		{
			get
			{
				return this.AlternativeType != 0 && this._getIsPerkSelected(this.Perk.AlternativePerk);
			}
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06001F00 RID: 7936 RVA: 0x00072413 File Offset: 0x00070613
		// (set) Token: 0x06001F01 RID: 7937 RVA: 0x0007241B File Offset: 0x0007061B
		public PerkVM.PerkStates CurrentState
		{
			get
			{
				return this._currentState;
			}
			private set
			{
				if (value != this._currentState)
				{
					this._currentState = value;
					this.PerkState = (int)value;
				}
			}
		}

		// Token: 0x06001F02 RID: 7938 RVA: 0x00072434 File Offset: 0x00070634
		public PerkVM(PerkObject perk, bool isAvailable, PerkVM.PerkAlternativeType alternativeType, Action<PerkVM> onStartSelection, Action<PerkVM> onSelectionOver, Func<PerkObject, bool> getIsPerkSelected, Func<PerkObject, bool> getIsPreviousPerkSelected)
		{
			PerkVM <>4__this = this;
			this.AlternativeType = (int)alternativeType;
			this.Perk = perk;
			this._onStartSelection = onStartSelection;
			this._onSelectionOver = onSelectionOver;
			this._getIsPerkSelected = getIsPerkSelected;
			this._getIsPreviousPerkSelected = getIsPreviousPerkSelected;
			this._isAvailable = isAvailable;
			this.PerkId = "SPPerks\\" + perk.StringId;
			this.Level = (int)perk.RequiredSkillValue;
			this.LevelText = ((int)perk.RequiredSkillValue).ToString();
			this.Hint = new BasicTooltipViewModel(() => CampaignUIHelper.GetPerkEffectText(perk, <>4__this._getIsPerkSelected(<>4__this.Perk)));
			this._perkConceptObj = Concept.All.SingleOrDefault<Concept>((Concept c) => c.StringId == "str_game_objects_perks");
			this.RefreshState();
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x00072538 File Offset: 0x00070738
		public void RefreshState()
		{
			bool flag = this._getIsPerkSelected(this.Perk);
			if (!this._isAvailable)
			{
				this.CurrentState = PerkVM.PerkStates.NotEarned;
				return;
			}
			if (flag)
			{
				this.CurrentState = PerkVM.PerkStates.EarnedAndActive;
				return;
			}
			if (this.Perk.AlternativePerk != null && this._getIsPerkSelected(this.Perk.AlternativePerk))
			{
				this.CurrentState = PerkVM.PerkStates.EarnedAndNotActive;
				return;
			}
			if (this._getIsPreviousPerkSelected(this.Perk))
			{
				this.CurrentState = PerkVM.PerkStates.EarnedButNotSelected;
				return;
			}
			this.CurrentState = PerkVM.PerkStates.EarnedPreviousPerkNotSelected;
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x000725C1 File Offset: 0x000707C1
		public void ExecuteShowPerkConcept()
		{
			if (this._perkConceptObj != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this._perkConceptObj.EncyclopediaLink);
				return;
			}
			Debug.FailedAssert("Couldn't find Perks encyclopedia page", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\CharacterDeveloper\\PerkVM.cs", "ExecuteShowPerkConcept", 127);
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x000725FC File Offset: 0x000707FC
		public void ExecuteStartSelection()
		{
			if (this._isAvailable && !this._getIsPerkSelected(this.Perk) && !this._hasAlternativeAndSelected && this._getIsPreviousPerkSelected(this.Perk))
			{
				this._onStartSelection(this);
			}
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06001F06 RID: 7942 RVA: 0x0007264B File Offset: 0x0007084B
		// (set) Token: 0x06001F07 RID: 7943 RVA: 0x00072653 File Offset: 0x00070853
		[DataSourceProperty]
		public bool IsTutorialHighlightEnabled
		{
			get
			{
				return this._isTutorialHighlightEnabled;
			}
			set
			{
				if (value != this._isTutorialHighlightEnabled)
				{
					this._isTutorialHighlightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsTutorialHighlightEnabled");
				}
			}
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06001F08 RID: 7944 RVA: 0x00072671 File Offset: 0x00070871
		// (set) Token: 0x06001F09 RID: 7945 RVA: 0x00072679 File Offset: 0x00070879
		[DataSourceProperty]
		public BasicTooltipViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06001F0A RID: 7946 RVA: 0x00072697 File Offset: 0x00070897
		// (set) Token: 0x06001F0B RID: 7947 RVA: 0x0007269F File Offset: 0x0007089F
		[DataSourceProperty]
		public int Level
		{
			get
			{
				return this._level;
			}
			set
			{
				if (value != this._level)
				{
					this._level = value;
					base.OnPropertyChangedWithValue(value, "Level");
				}
			}
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06001F0C RID: 7948 RVA: 0x000726BD File Offset: 0x000708BD
		// (set) Token: 0x06001F0D RID: 7949 RVA: 0x000726C5 File Offset: 0x000708C5
		[DataSourceProperty]
		public int PerkState
		{
			get
			{
				return this._perkState;
			}
			set
			{
				if (value != this._perkState)
				{
					this._perkState = value;
					base.OnPropertyChangedWithValue(value, "PerkState");
				}
			}
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06001F0E RID: 7950 RVA: 0x000726E3 File Offset: 0x000708E3
		// (set) Token: 0x06001F0F RID: 7951 RVA: 0x000726EB File Offset: 0x000708EB
		[DataSourceProperty]
		public int AlternativeType
		{
			get
			{
				return this._alternativeType;
			}
			set
			{
				if (value != this._alternativeType)
				{
					this._alternativeType = value;
					base.OnPropertyChangedWithValue(value, "AlternativeType");
				}
			}
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06001F10 RID: 7952 RVA: 0x00072709 File Offset: 0x00070909
		// (set) Token: 0x06001F11 RID: 7953 RVA: 0x00072711 File Offset: 0x00070911
		[DataSourceProperty]
		public string LevelText
		{
			get
			{
				return this._levelText;
			}
			set
			{
				if (value != this._levelText)
				{
					this._levelText = value;
					base.OnPropertyChangedWithValue<string>(value, "LevelText");
				}
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06001F12 RID: 7954 RVA: 0x00072734 File Offset: 0x00070934
		// (set) Token: 0x06001F13 RID: 7955 RVA: 0x0007273C File Offset: 0x0007093C
		[DataSourceProperty]
		public string BackgroundImage
		{
			get
			{
				return this._backgroundImage;
			}
			set
			{
				if (value != this._backgroundImage)
				{
					this._backgroundImage = value;
					base.OnPropertyChangedWithValue<string>(value, "BackgroundImage");
				}
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06001F14 RID: 7956 RVA: 0x0007275F File Offset: 0x0007095F
		// (set) Token: 0x06001F15 RID: 7957 RVA: 0x00072767 File Offset: 0x00070967
		[DataSourceProperty]
		public string PerkId
		{
			get
			{
				return this._perkId;
			}
			set
			{
				if (value != this._perkId)
				{
					this._perkId = value;
					base.OnPropertyChangedWithValue<string>(value, "PerkId");
				}
			}
		}

		// Token: 0x04000E74 RID: 3700
		public readonly PerkObject Perk;

		// Token: 0x04000E75 RID: 3701
		private readonly Action<PerkVM> _onStartSelection;

		// Token: 0x04000E76 RID: 3702
		private readonly Action<PerkVM> _onSelectionOver;

		// Token: 0x04000E77 RID: 3703
		private readonly Func<PerkObject, bool> _getIsPerkSelected;

		// Token: 0x04000E78 RID: 3704
		private readonly Func<PerkObject, bool> _getIsPreviousPerkSelected;

		// Token: 0x04000E79 RID: 3705
		private readonly bool _isAvailable;

		// Token: 0x04000E7A RID: 3706
		private readonly Concept _perkConceptObj;

		// Token: 0x04000E7B RID: 3707
		private PerkVM.PerkStates _currentState = PerkVM.PerkStates.None;

		// Token: 0x04000E7C RID: 3708
		private string _levelText;

		// Token: 0x04000E7D RID: 3709
		private string _perkId;

		// Token: 0x04000E7E RID: 3710
		private string _backgroundImage;

		// Token: 0x04000E7F RID: 3711
		private BasicTooltipViewModel _hint;

		// Token: 0x04000E80 RID: 3712
		private int _level;

		// Token: 0x04000E81 RID: 3713
		private int _alternativeType;

		// Token: 0x04000E82 RID: 3714
		private int _perkState = -1;

		// Token: 0x04000E83 RID: 3715
		private bool _isTutorialHighlightEnabled;

		// Token: 0x020002CF RID: 719
		public enum PerkStates
		{
			// Token: 0x040013AD RID: 5037
			None = -1,
			// Token: 0x040013AE RID: 5038
			NotEarned,
			// Token: 0x040013AF RID: 5039
			EarnedButNotSelected,
			// Token: 0x040013B0 RID: 5040
			EarnedAndActive,
			// Token: 0x040013B1 RID: 5041
			EarnedAndNotActive,
			// Token: 0x040013B2 RID: 5042
			EarnedPreviousPerkNotSelected
		}

		// Token: 0x020002D0 RID: 720
		public enum PerkAlternativeType
		{
			// Token: 0x040013B4 RID: 5044
			NoAlternative,
			// Token: 0x040013B5 RID: 5045
			FirstAlternative,
			// Token: 0x040013B6 RID: 5046
			SecondAlternative
		}
	}
}
