using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000E9 RID: 233
	internal class OrderOfBattleFormationClassDropdownListButtonWidget : ButtonWidget
	{
		// Token: 0x06000BF8 RID: 3064 RVA: 0x00020F95 File Offset: 0x0001F195
		public OrderOfBattleFormationClassDropdownListButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x00020FA0 File Offset: 0x0001F1A0
		private void SetBaseBrush()
		{
			switch (this.FormationClass)
			{
			case 0:
				base.Brush = this.UnsetBrush;
				break;
			case 1:
				base.Brush = this.InfantryBrush;
				break;
			case 2:
				base.Brush = this.RangedBrush;
				break;
			case 3:
				base.Brush = this.CavalryBrush;
				break;
			case 4:
				base.Brush = this.HorseArcherBrush;
				break;
			case 5:
				base.Brush = this.InfantryAndRangedBrush;
				break;
			case 6:
				base.Brush = this.CavalryAndHorseArcherBrush;
				break;
			default:
				base.Brush = this.UnsetBrush;
				break;
			}
			this._hasBaseBrushSet = true;
			this.SetColor();
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x00021053 File Offset: 0x0001F253
		private void SetColor()
		{
			if (this.IsErrored)
			{
				base.Brush.Color = this.ErroredColor;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x0002106E File Offset: 0x0001F26E
		// (set) Token: 0x06000BFC RID: 3068 RVA: 0x00021076 File Offset: 0x0001F276
		[Editor(false)]
		public int FormationClass
		{
			get
			{
				return this._formationClass;
			}
			set
			{
				if (value != this._formationClass || !this._hasBaseBrushSet)
				{
					this._formationClass = value;
					base.OnPropertyChanged(value, "FormationClass");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06000BFD RID: 3069 RVA: 0x000210A2 File Offset: 0x0001F2A2
		// (set) Token: 0x06000BFE RID: 3070 RVA: 0x000210AA File Offset: 0x0001F2AA
		[Editor(false)]
		public Color ErroredColor
		{
			get
			{
				return this._erroredColor;
			}
			set
			{
				if (value != this._erroredColor)
				{
					this._erroredColor = value;
					base.OnPropertyChanged(value, "ErroredColor");
				}
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06000BFF RID: 3071 RVA: 0x000210CD File Offset: 0x0001F2CD
		// (set) Token: 0x06000C00 RID: 3072 RVA: 0x000210D5 File Offset: 0x0001F2D5
		[Editor(false)]
		public bool IsErrored
		{
			get
			{
				return this._isErrored;
			}
			set
			{
				if (value != this._isErrored)
				{
					this._isErrored = value;
					base.OnPropertyChanged(value, "IsErrored");
					this.SetColor();
				}
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06000C01 RID: 3073 RVA: 0x000210F9 File Offset: 0x0001F2F9
		// (set) Token: 0x06000C02 RID: 3074 RVA: 0x00021101 File Offset: 0x0001F301
		[Editor(false)]
		public Brush UnsetBrush
		{
			get
			{
				return this._unsetBrush;
			}
			set
			{
				if (value != this._unsetBrush)
				{
					this._unsetBrush = value;
					base.OnPropertyChanged<Brush>(value, "UnsetBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06000C03 RID: 3075 RVA: 0x00021125 File Offset: 0x0001F325
		// (set) Token: 0x06000C04 RID: 3076 RVA: 0x0002112D File Offset: 0x0001F32D
		[Editor(false)]
		public Brush InfantryBrush
		{
			get
			{
				return this._infantryBrush;
			}
			set
			{
				if (value != this._infantryBrush)
				{
					this._infantryBrush = value;
					base.OnPropertyChanged<Brush>(value, "InfantryBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06000C05 RID: 3077 RVA: 0x00021151 File Offset: 0x0001F351
		// (set) Token: 0x06000C06 RID: 3078 RVA: 0x00021159 File Offset: 0x0001F359
		[Editor(false)]
		public Brush RangedBrush
		{
			get
			{
				return this._rangedBrush;
			}
			set
			{
				if (value != this._rangedBrush)
				{
					this._rangedBrush = value;
					base.OnPropertyChanged<Brush>(value, "RangedBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06000C07 RID: 3079 RVA: 0x0002117D File Offset: 0x0001F37D
		// (set) Token: 0x06000C08 RID: 3080 RVA: 0x00021185 File Offset: 0x0001F385
		[Editor(false)]
		public Brush CavalryBrush
		{
			get
			{
				return this._cavalryBrush;
			}
			set
			{
				if (value != this._cavalryBrush)
				{
					this._cavalryBrush = value;
					base.OnPropertyChanged<Brush>(value, "CavalryBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06000C09 RID: 3081 RVA: 0x000211A9 File Offset: 0x0001F3A9
		// (set) Token: 0x06000C0A RID: 3082 RVA: 0x000211B1 File Offset: 0x0001F3B1
		[Editor(false)]
		public Brush HorseArcherBrush
		{
			get
			{
				return this._horseArcherBrush;
			}
			set
			{
				if (value != this._horseArcherBrush)
				{
					this._horseArcherBrush = value;
					base.OnPropertyChanged<Brush>(value, "HorseArcherBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06000C0B RID: 3083 RVA: 0x000211D5 File Offset: 0x0001F3D5
		// (set) Token: 0x06000C0C RID: 3084 RVA: 0x000211DD File Offset: 0x0001F3DD
		[Editor(false)]
		public Brush InfantryAndRangedBrush
		{
			get
			{
				return this._infantryAndRangedBrush;
			}
			set
			{
				if (value != this._infantryAndRangedBrush)
				{
					this._infantryAndRangedBrush = value;
					base.OnPropertyChanged<Brush>(value, "InfantryAndRangedBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06000C0D RID: 3085 RVA: 0x00021201 File Offset: 0x0001F401
		// (set) Token: 0x06000C0E RID: 3086 RVA: 0x00021209 File Offset: 0x0001F409
		[Editor(false)]
		public Brush CavalryAndHorseArcherBrush
		{
			get
			{
				return this._cavalryAndHorseArcherBrush;
			}
			set
			{
				if (value != this._cavalryAndHorseArcherBrush)
				{
					this._cavalryAndHorseArcherBrush = value;
					base.OnPropertyChanged<Brush>(value, "CavalryAndHorseArcherBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x04000566 RID: 1382
		private bool _hasBaseBrushSet;

		// Token: 0x04000567 RID: 1383
		private int _formationClass;

		// Token: 0x04000568 RID: 1384
		private Color _erroredColor;

		// Token: 0x04000569 RID: 1385
		private bool _isErrored;

		// Token: 0x0400056A RID: 1386
		private Brush _unsetBrush;

		// Token: 0x0400056B RID: 1387
		private Brush _infantryBrush;

		// Token: 0x0400056C RID: 1388
		private Brush _rangedBrush;

		// Token: 0x0400056D RID: 1389
		private Brush _cavalryBrush;

		// Token: 0x0400056E RID: 1390
		private Brush _horseArcherBrush;

		// Token: 0x0400056F RID: 1391
		private Brush _infantryAndRangedBrush;

		// Token: 0x04000570 RID: 1392
		private Brush _cavalryAndHorseArcherBrush;
	}
}
