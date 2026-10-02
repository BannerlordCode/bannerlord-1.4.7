using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.FormationMarker
{
	// Token: 0x02000064 RID: 100
	public class MissionSiegeEngineMarkerTargetVM : ViewModel
	{
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x0001BF5B File Offset: 0x0001A15B
		// (set) Token: 0x060007EC RID: 2028 RVA: 0x0001BF63 File Offset: 0x0001A163
		public SiegeWeapon Engine { get; private set; }

		// Token: 0x060007ED RID: 2029 RVA: 0x0001BF6C File Offset: 0x0001A16C
		public MissionSiegeEngineMarkerTargetVM(SiegeWeapon engine, bool isEnemy)
		{
			this.Engine = engine;
			this.EngineType = this.Engine.GetSiegeEngineType().StringId;
			this.IsEnemy = isEnemy;
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0001BF98 File Offset: 0x0001A198
		public void Refresh()
		{
			this.HitPoints = MathF.Ceiling(this.Engine.DestructionComponent.HitPoint / this.Engine.DestructionComponent.MaxHitPoint * 100f);
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0001BFCC File Offset: 0x0001A1CC
		// (set) Token: 0x060007F0 RID: 2032 RVA: 0x0001BFD4 File Offset: 0x0001A1D4
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

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0001BFF2 File Offset: 0x0001A1F2
		// (set) Token: 0x060007F2 RID: 2034 RVA: 0x0001BFFA File Offset: 0x0001A1FA
		[DataSourceProperty]
		public bool IsEnemy
		{
			get
			{
				return this._isEnemy;
			}
			set
			{
				if (this._isEnemy != value)
				{
					this._isEnemy = value;
					base.OnPropertyChangedWithValue(value, "IsEnemy");
				}
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x0001C018 File Offset: 0x0001A218
		// (set) Token: 0x060007F4 RID: 2036 RVA: 0x0001C020 File Offset: 0x0001A220
		[DataSourceProperty]
		public string EngineType
		{
			get
			{
				return this._engineType;
			}
			set
			{
				if (this._engineType != value)
				{
					this._engineType = value;
					base.OnPropertyChangedWithValue<string>(value, "EngineType");
				}
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x0001C043 File Offset: 0x0001A243
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x0001C04B File Offset: 0x0001A24B
		[DataSourceProperty]
		public bool IsBehind
		{
			get
			{
				return this._isBehind;
			}
			set
			{
				if (this._isBehind != value)
				{
					this._isBehind = value;
					base.OnPropertyChangedWithValue(value, "IsBehind");
				}
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x0001C069 File Offset: 0x0001A269
		// (set) Token: 0x060007F8 RID: 2040 RVA: 0x0001C071 File Offset: 0x0001A271
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

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x0001C0AC File Offset: 0x0001A2AC
		// (set) Token: 0x060007FA RID: 2042 RVA: 0x0001C0B4 File Offset: 0x0001A2B4
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

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0001C0DA File Offset: 0x0001A2DA
		// (set) Token: 0x060007FC RID: 2044 RVA: 0x0001C0E2 File Offset: 0x0001A2E2
		[DataSourceProperty]
		public int HitPoints
		{
			get
			{
				return this._hitPoints;
			}
			set
			{
				if (this._hitPoints != value)
				{
					this._hitPoints = value;
					base.OnPropertyChangedWithValue(value, "HitPoints");
				}
			}
		}

		// Token: 0x0400038B RID: 907
		private Vec2 _screenPosition;

		// Token: 0x0400038C RID: 908
		private float _distance;

		// Token: 0x0400038D RID: 909
		private bool _isEnabled;

		// Token: 0x0400038E RID: 910
		private bool _isBehind;

		// Token: 0x0400038F RID: 911
		private bool _isEnemy;

		// Token: 0x04000390 RID: 912
		private string _engineType;

		// Token: 0x04000391 RID: 913
		private int _hitPoints;
	}
}
