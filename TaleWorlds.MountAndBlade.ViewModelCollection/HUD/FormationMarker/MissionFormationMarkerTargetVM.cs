using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.FormationMarker
{
	// Token: 0x02000061 RID: 97
	public class MissionFormationMarkerTargetVM : ViewModel
	{
		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x0001B4AA File Offset: 0x000196AA
		// (set) Token: 0x060007B5 RID: 1973 RVA: 0x0001B4B2 File Offset: 0x000196B2
		public Formation Formation { get; private set; }

		// Token: 0x060007B6 RID: 1974 RVA: 0x0001B4BC File Offset: 0x000196BC
		public MissionFormationMarkerTargetVM(Formation formation)
		{
			this.Formation = formation;
			this.FormationType = MissionFormationMarkerTargetVM.GetFormationType(this.Formation.RepresentativeClass);
			if (this.Formation.Team.IsPlayerTeam)
			{
				this.TeamType = 0;
				return;
			}
			if (this.Formation.Team.IsPlayerAlly)
			{
				this.TeamType = 1;
				return;
			}
			this.TeamType = 2;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0001B527 File Offset: 0x00019727
		public void Refresh()
		{
			this.Size = this.Formation.CountOfUnits;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0001B53A File Offset: 0x0001973A
		public void SetTargetedState(bool isFocused, bool isTargetingAFormation)
		{
			this.IsCenterOfFocus = isFocused;
			this.IsTargetingAFormation = isTargetingAFormation;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0001B54C File Offset: 0x0001974C
		public static string GetFormationType(FormationClass formationType)
		{
			switch (formationType)
			{
			case FormationClass.Infantry:
				return "Infantry_Light";
			case FormationClass.Ranged:
				return "Archer_Light";
			case FormationClass.Cavalry:
				return "Cavalry_Light";
			case FormationClass.HorseArcher:
				return "HorseArcher_Light";
			case FormationClass.NumberOfDefaultFormations:
			case FormationClass.HeavyInfantry:
			case FormationClass.NumberOfRegularFormations:
			case FormationClass.Bodyguard:
			case FormationClass.NumberOfAllFormations:
				return "Infantry_Heavy";
			case FormationClass.LightCavalry:
				return "Cavalry_Light";
			case FormationClass.HeavyCavalry:
				return "Cavalry_Heavy";
			default:
				return "None";
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x0001B5BC File Offset: 0x000197BC
		// (set) Token: 0x060007BB RID: 1979 RVA: 0x0001B5C4 File Offset: 0x000197C4
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (this._isEnabled != value)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x0001B5E2 File Offset: 0x000197E2
		// (set) Token: 0x060007BD RID: 1981 RVA: 0x0001B5EA File Offset: 0x000197EA
		[DataSourceProperty]
		public bool IsCenterOfFocus
		{
			get
			{
				return this._isCenterOfFocus;
			}
			set
			{
				if (this._isCenterOfFocus != value)
				{
					this._isCenterOfFocus = value;
					base.OnPropertyChangedWithValue(value, "IsCenterOfFocus");
				}
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x0001B608 File Offset: 0x00019808
		// (set) Token: 0x060007BF RID: 1983 RVA: 0x0001B610 File Offset: 0x00019810
		[DataSourceProperty]
		public bool IsFormationTargetRelevant
		{
			get
			{
				return this._isFormationTargetRelevant;
			}
			set
			{
				if (this._isFormationTargetRelevant != value)
				{
					this._isFormationTargetRelevant = value;
					base.OnPropertyChangedWithValue(value, "IsFormationTargetRelevant");
				}
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x0001B62E File Offset: 0x0001982E
		// (set) Token: 0x060007C1 RID: 1985 RVA: 0x0001B636 File Offset: 0x00019836
		[DataSourceProperty]
		public bool IsTargetingAFormation
		{
			get
			{
				return this._isTargetingAFormation;
			}
			set
			{
				if (this._isTargetingAFormation != value)
				{
					this._isTargetingAFormation = value;
					base.OnPropertyChangedWithValue(value, "IsTargetingAFormation");
				}
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x0001B654 File Offset: 0x00019854
		// (set) Token: 0x060007C3 RID: 1987 RVA: 0x0001B65C File Offset: 0x0001985C
		[DataSourceProperty]
		public bool ShowDistanceTexts
		{
			get
			{
				return this._showDistanceTexts;
			}
			set
			{
				if (this._showDistanceTexts != value)
				{
					this._showDistanceTexts = value;
					base.OnPropertyChangedWithValue(value, "ShowDistanceTexts");
				}
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x0001B67A File Offset: 0x0001987A
		// (set) Token: 0x060007C5 RID: 1989 RVA: 0x0001B682 File Offset: 0x00019882
		[DataSourceProperty]
		public string FormationType
		{
			get
			{
				return this._formationType;
			}
			set
			{
				if (this._formationType != value)
				{
					this._formationType = value;
					base.OnPropertyChangedWithValue<string>(value, "FormationType");
				}
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x0001B6A5 File Offset: 0x000198A5
		// (set) Token: 0x060007C7 RID: 1991 RVA: 0x0001B6AD File Offset: 0x000198AD
		[DataSourceProperty]
		public int TeamType
		{
			get
			{
				return this._teamType;
			}
			set
			{
				if (this._teamType != value)
				{
					this._teamType = value;
					base.OnPropertyChangedWithValue(value, "TeamType");
				}
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x0001B6CB File Offset: 0x000198CB
		// (set) Token: 0x060007C9 RID: 1993 RVA: 0x0001B6D3 File Offset: 0x000198D3
		[DataSourceProperty]
		public Vec2 ScreenPosition
		{
			get
			{
				return this._screenPosition;
			}
			set
			{
				if (value.x != this._screenPosition.x || value.y != this._screenPosition.y)
				{
					this._screenPosition = value;
					base.OnPropertyChangedWithValue(value, "ScreenPosition");
				}
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060007CA RID: 1994 RVA: 0x0001B70E File Offset: 0x0001990E
		// (set) Token: 0x060007CB RID: 1995 RVA: 0x0001B716 File Offset: 0x00019916
		[DataSourceProperty]
		public float Distance
		{
			get
			{
				return this._distance;
			}
			set
			{
				if (this._distance != value && !float.IsNaN(value))
				{
					this._distance = value;
					base.OnPropertyChangedWithValue(value, "Distance");
				}
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060007CC RID: 1996 RVA: 0x0001B73C File Offset: 0x0001993C
		// (set) Token: 0x060007CD RID: 1997 RVA: 0x0001B744 File Offset: 0x00019944
		[DataSourceProperty]
		public string DistanceText
		{
			get
			{
				return this._distanceText;
			}
			set
			{
				if (this._distanceText != value)
				{
					this._distanceText = value;
					base.OnPropertyChangedWithValue<string>(value, "DistanceText");
				}
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x0001B767 File Offset: 0x00019967
		// (set) Token: 0x060007CF RID: 1999 RVA: 0x0001B76F File Offset: 0x0001996F
		[DataSourceProperty]
		public int Size
		{
			get
			{
				return this._size;
			}
			set
			{
				if (this._size != value)
				{
					this._size = value;
					base.OnPropertyChangedWithValue(value, "Size");
				}
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x0001B78D File Offset: 0x0001998D
		// (set) Token: 0x060007D1 RID: 2001 RVA: 0x0001B795 File Offset: 0x00019995
		[DataSourceProperty]
		public int WSign
		{
			get
			{
				return this._wSign;
			}
			set
			{
				if (this._wSign != value)
				{
					this._wSign = value;
					base.OnPropertyChangedWithValue(value, "WSign");
				}
			}
		}

		// Token: 0x0400036D RID: 877
		private Vec2 _screenPosition;

		// Token: 0x0400036E RID: 878
		private float _distance;

		// Token: 0x0400036F RID: 879
		private string _distanceText;

		// Token: 0x04000370 RID: 880
		private bool _isEnabled;

		// Token: 0x04000371 RID: 881
		private bool _isCenterOfFocus;

		// Token: 0x04000372 RID: 882
		private bool _isFormationTargetRelevant;

		// Token: 0x04000373 RID: 883
		private bool _isTargetingAFormation;

		// Token: 0x04000374 RID: 884
		private bool _showDistanceTexts;

		// Token: 0x04000375 RID: 885
		private int _teamType;

		// Token: 0x04000376 RID: 886
		private int _size;

		// Token: 0x04000377 RID: 887
		private int _wSign;

		// Token: 0x04000378 RID: 888
		private string _formationType;

		// Token: 0x020000F7 RID: 247
		public enum TeamTypes
		{
			// Token: 0x04000685 RID: 1669
			PlayerTeam,
			// Token: 0x04000686 RID: 1670
			PlayerAllyTeam,
			// Token: 0x04000687 RID: 1671
			EnemyTeam
		}
	}
}
