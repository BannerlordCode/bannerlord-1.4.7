using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000EC RID: 236
	public class OrderOfBattleFormationFilterVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000C1A RID: 3098 RVA: 0x00021310 File Offset: 0x0001F510
		public OrderOfBattleFormationFilterVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x0002131C File Offset: 0x0001F51C
		private void SetBaseBrush()
		{
			switch (this.FormationFilter)
			{
			case 0:
				base.Brush = this.UnsetBrush;
				break;
			case 1:
				base.Brush = this.ShieldBrush;
				break;
			case 2:
				base.Brush = this.SpearBrush;
				break;
			case 3:
				base.Brush = this.ThrownBrush;
				break;
			case 4:
				base.Brush = this.HeavyBrush;
				break;
			case 5:
				base.Brush = this.HighTierBrush;
				break;
			case 6:
				base.Brush = this.LowTierBrush;
				break;
			default:
				base.Brush = this.UnsetBrush;
				break;
			}
			this._hasBaseBrushSet = true;
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06000C1C RID: 3100 RVA: 0x000213C9 File Offset: 0x0001F5C9
		// (set) Token: 0x06000C1D RID: 3101 RVA: 0x000213D1 File Offset: 0x0001F5D1
		[Editor(false)]
		public int FormationFilter
		{
			get
			{
				return this._formationFilter;
			}
			set
			{
				if (value != this._formationFilter || !this._hasBaseBrushSet)
				{
					this._formationFilter = value;
					base.OnPropertyChanged(value, "FormationFilter");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x000213FD File Offset: 0x0001F5FD
		// (set) Token: 0x06000C1F RID: 3103 RVA: 0x00021405 File Offset: 0x0001F605
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

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x00021429 File Offset: 0x0001F629
		// (set) Token: 0x06000C21 RID: 3105 RVA: 0x00021431 File Offset: 0x0001F631
		[Editor(false)]
		public Brush SpearBrush
		{
			get
			{
				return this._spearBrush;
			}
			set
			{
				if (value != this._spearBrush)
				{
					this._spearBrush = value;
					base.OnPropertyChanged<Brush>(value, "SpearBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06000C22 RID: 3106 RVA: 0x00021455 File Offset: 0x0001F655
		// (set) Token: 0x06000C23 RID: 3107 RVA: 0x0002145D File Offset: 0x0001F65D
		[Editor(false)]
		public Brush ShieldBrush
		{
			get
			{
				return this._shieldBrush;
			}
			set
			{
				if (value != this._shieldBrush)
				{
					this._shieldBrush = value;
					base.OnPropertyChanged<Brush>(value, "ShieldBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06000C24 RID: 3108 RVA: 0x00021481 File Offset: 0x0001F681
		// (set) Token: 0x06000C25 RID: 3109 RVA: 0x00021489 File Offset: 0x0001F689
		[Editor(false)]
		public Brush ThrownBrush
		{
			get
			{
				return this._thrownBrush;
			}
			set
			{
				if (value != this._thrownBrush)
				{
					this._thrownBrush = value;
					base.OnPropertyChanged<Brush>(value, "ThrownBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x000214AD File Offset: 0x0001F6AD
		// (set) Token: 0x06000C27 RID: 3111 RVA: 0x000214B5 File Offset: 0x0001F6B5
		[Editor(false)]
		public Brush HeavyBrush
		{
			get
			{
				return this._heavyBrush;
			}
			set
			{
				if (value != this._heavyBrush)
				{
					this._heavyBrush = value;
					base.OnPropertyChanged<Brush>(value, "HeavyBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06000C28 RID: 3112 RVA: 0x000214D9 File Offset: 0x0001F6D9
		// (set) Token: 0x06000C29 RID: 3113 RVA: 0x000214E1 File Offset: 0x0001F6E1
		[Editor(false)]
		public Brush HighTierBrush
		{
			get
			{
				return this._highTierBrush;
			}
			set
			{
				if (value != this._highTierBrush)
				{
					this._highTierBrush = value;
					base.OnPropertyChanged<Brush>(value, "HighTierBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06000C2A RID: 3114 RVA: 0x00021505 File Offset: 0x0001F705
		// (set) Token: 0x06000C2B RID: 3115 RVA: 0x0002150D File Offset: 0x0001F70D
		[Editor(false)]
		public Brush LowTierBrush
		{
			get
			{
				return this._lowTierBrush;
			}
			set
			{
				if (value != this._lowTierBrush)
				{
					this._lowTierBrush = value;
					base.OnPropertyChanged<Brush>(value, "LowTierBrush");
					this.SetBaseBrush();
				}
			}
		}

		// Token: 0x04000576 RID: 1398
		private bool _hasBaseBrushSet;

		// Token: 0x04000577 RID: 1399
		private int _formationFilter;

		// Token: 0x04000578 RID: 1400
		private Brush _unsetBrush;

		// Token: 0x04000579 RID: 1401
		private Brush _spearBrush;

		// Token: 0x0400057A RID: 1402
		private Brush _shieldBrush;

		// Token: 0x0400057B RID: 1403
		private Brush _thrownBrush;

		// Token: 0x0400057C RID: 1404
		private Brush _heavyBrush;

		// Token: 0x0400057D RID: 1405
		private Brush _highTierBrush;

		// Token: 0x0400057E RID: 1406
		private Brush _lowTierBrush;
	}
}
