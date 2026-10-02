using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000E8 RID: 232
	public class OrderOfBattleFormationClassBrushWidget : BrushWidget
	{
		// Token: 0x06000BE1 RID: 3041 RVA: 0x00020CFD File Offset: 0x0001EEFD
		public OrderOfBattleFormationClassBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00020D08 File Offset: 0x0001EF08
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

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00020DBB File Offset: 0x0001EFBB
		private void SetColor()
		{
			if (this.IsErrored)
			{
				base.Brush.Color = this.ErroredColor;
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x00020DD6 File Offset: 0x0001EFD6
		// (set) Token: 0x06000BE5 RID: 3045 RVA: 0x00020DDE File Offset: 0x0001EFDE
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

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x00020E0A File Offset: 0x0001F00A
		// (set) Token: 0x06000BE7 RID: 3047 RVA: 0x00020E12 File Offset: 0x0001F012
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

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x00020E35 File Offset: 0x0001F035
		// (set) Token: 0x06000BE9 RID: 3049 RVA: 0x00020E3D File Offset: 0x0001F03D
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

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x00020E61 File Offset: 0x0001F061
		// (set) Token: 0x06000BEB RID: 3051 RVA: 0x00020E69 File Offset: 0x0001F069
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

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06000BEC RID: 3052 RVA: 0x00020E8D File Offset: 0x0001F08D
		// (set) Token: 0x06000BED RID: 3053 RVA: 0x00020E95 File Offset: 0x0001F095
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

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06000BEE RID: 3054 RVA: 0x00020EB9 File Offset: 0x0001F0B9
		// (set) Token: 0x06000BEF RID: 3055 RVA: 0x00020EC1 File Offset: 0x0001F0C1
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

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x00020EE5 File Offset: 0x0001F0E5
		// (set) Token: 0x06000BF1 RID: 3057 RVA: 0x00020EED File Offset: 0x0001F0ED
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

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06000BF2 RID: 3058 RVA: 0x00020F11 File Offset: 0x0001F111
		// (set) Token: 0x06000BF3 RID: 3059 RVA: 0x00020F19 File Offset: 0x0001F119
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

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x00020F3D File Offset: 0x0001F13D
		// (set) Token: 0x06000BF5 RID: 3061 RVA: 0x00020F45 File Offset: 0x0001F145
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

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x00020F69 File Offset: 0x0001F169
		// (set) Token: 0x06000BF7 RID: 3063 RVA: 0x00020F71 File Offset: 0x0001F171
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

		// Token: 0x0400055B RID: 1371
		private bool _hasBaseBrushSet;

		// Token: 0x0400055C RID: 1372
		private int _formationClass;

		// Token: 0x0400055D RID: 1373
		private Color _erroredColor;

		// Token: 0x0400055E RID: 1374
		private bool _isErrored;

		// Token: 0x0400055F RID: 1375
		private Brush _unsetBrush;

		// Token: 0x04000560 RID: 1376
		private Brush _infantryBrush;

		// Token: 0x04000561 RID: 1377
		private Brush _rangedBrush;

		// Token: 0x04000562 RID: 1378
		private Brush _cavalryBrush;

		// Token: 0x04000563 RID: 1379
		private Brush _horseArcherBrush;

		// Token: 0x04000564 RID: 1380
		private Brush _infantryAndRangedBrush;

		// Token: 0x04000565 RID: 1381
		private Brush _cavalryAndHorseArcherBrush;
	}
}
