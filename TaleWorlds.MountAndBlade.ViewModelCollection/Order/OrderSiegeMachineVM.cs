using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects.Siege;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x02000020 RID: 32
	public class OrderSiegeMachineVM : OrderSubjectVM
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000C053 File Offset: 0x0000A253
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x0000C05B File Offset: 0x0000A25B
		public DeploymentPoint DeploymentPoint { get; private set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000C064 File Offset: 0x0000A264
		public SiegeWeapon SiegeWeapon
		{
			get
			{
				if (this.DeploymentPoint != null)
				{
					return this.DeploymentPoint.DeployedWeapon as SiegeWeapon;
				}
				return null;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0000C080 File Offset: 0x0000A280
		public bool IsPrimarySiegeMachine
		{
			get
			{
				return this.SiegeWeapon is IPrimarySiegeWeapon;
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000C090 File Offset: 0x0000A290
		public OrderSiegeMachineVM(DeploymentPoint deploymentPoint, Action<OrderSiegeMachineVM> setSelected, int keyIndex)
		{
			this.DeploymentPoint = deploymentPoint;
			this.SetSelected = setSelected;
			this.RefreshValues();
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000C0B7 File Offset: 0x0000A2B7
		private void ExecuteAction()
		{
			if (this.SiegeWeapon != null)
			{
				this.SetSelected(this);
			}
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000C0CD File Offset: 0x0000A2CD
		protected override void OnSelectionStateChanged(bool isSelected)
		{
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000C0D0 File Offset: 0x0000A2D0
		public void RefreshSiegeWeapon()
		{
			if (this.SiegeWeapon == null)
			{
				this.MachineType = null;
				this.MachineClass = "none";
				this.CurrentHP = 1.0;
				base.IsSelectable = false;
				base.IsSelected = false;
				return;
			}
			base.IsSelectable = SiegeWeaponController.IsWeaponSelectable(this.SiegeWeapon);
			this.MachineType = this.SiegeWeapon.GetType();
			this.MachineClass = this.SiegeWeapon.GetSiegeEngineType().StringId;
			if (this.SiegeWeapon.DestructionComponent != null)
			{
				this.CurrentHP = (double)(this.SiegeWeapon.DestructionComponent.HitPoint / this.SiegeWeapon.DestructionComponent.MaxHitPoint);
			}
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000C184 File Offset: 0x0000A384
		public static SiegeEngineType GetSiegeType(Type t, BattleSideEnum side)
		{
			if (t == typeof(SiegeLadder))
			{
				return DefaultSiegeEngineTypes.Ladder;
			}
			if (t == typeof(Ballista))
			{
				return DefaultSiegeEngineTypes.Ballista;
			}
			if (t == typeof(FireBallista))
			{
				return DefaultSiegeEngineTypes.FireBallista;
			}
			if (t == typeof(BatteringRam))
			{
				return DefaultSiegeEngineTypes.Ram;
			}
			if (t == typeof(SiegeTower))
			{
				return DefaultSiegeEngineTypes.SiegeTower;
			}
			if (t == typeof(Mangonel))
			{
				if (side != BattleSideEnum.Attacker)
				{
					return DefaultSiegeEngineTypes.Catapult;
				}
				return DefaultSiegeEngineTypes.Onager;
			}
			else if (t == typeof(FireMangonel))
			{
				if (side != BattleSideEnum.Attacker)
				{
					return DefaultSiegeEngineTypes.FireCatapult;
				}
				return DefaultSiegeEngineTypes.FireOnager;
			}
			else
			{
				if (t == typeof(Trebuchet))
				{
					return DefaultSiegeEngineTypes.Trebuchet;
				}
				if (t == typeof(FireTrebuchet))
				{
					return DefaultSiegeEngineTypes.FireTrebuchet;
				}
				Debug.FailedAssert("Invalid siege weapon", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Order\\OrderSiegeMachineVM.cs", "GetSiegeType", 107);
				return DefaultSiegeEngineTypes.Ladder;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002FD RID: 765 RVA: 0x0000C298 File Offset: 0x0000A498
		// (set) Token: 0x060002FE RID: 766 RVA: 0x0000C2A0 File Offset: 0x0000A4A0
		[DataSourceProperty]
		public string MachineClass
		{
			get
			{
				return this._machineClass;
			}
			set
			{
				this._machineClass = value;
				base.OnPropertyChangedWithValue<string>(value, "MachineClass");
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002FF RID: 767 RVA: 0x0000C2B5 File Offset: 0x0000A4B5
		// (set) Token: 0x06000300 RID: 768 RVA: 0x0000C2BD File Offset: 0x0000A4BD
		[DataSourceProperty]
		public double CurrentHP
		{
			get
			{
				return this._currentHP;
			}
			set
			{
				if (value != this._currentHP)
				{
					this._currentHP = value;
					base.OnPropertyChangedWithValue(value, "CurrentHP");
				}
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0000C2DB File Offset: 0x0000A4DB
		// (set) Token: 0x06000302 RID: 770 RVA: 0x0000C2E3 File Offset: 0x0000A4E3
		public bool IsInside
		{
			get
			{
				return this._isInside;
			}
			set
			{
				if (value != this._isInside)
				{
					this._isInside = value;
					base.OnPropertyChangedWithValue(value, "IsInside");
				}
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000303 RID: 771 RVA: 0x0000C301 File Offset: 0x0000A501
		// (set) Token: 0x06000304 RID: 772 RVA: 0x0000C309 File Offset: 0x0000A509
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (this._position != value)
				{
					this._position = value;
					base.OnPropertyChangedWithValue(value, "Position");
				}
			}
		}

		// Token: 0x0400014A RID: 330
		public Type MachineType;

		// Token: 0x0400014B RID: 331
		public Action<OrderSiegeMachineVM> SetSelected;

		// Token: 0x0400014C RID: 332
		private string _machineClass = "";

		// Token: 0x0400014D RID: 333
		private double _currentHP;

		// Token: 0x0400014E RID: 334
		private bool _isInside;

		// Token: 0x0400014F RID: 335
		private Vec2 _position;
	}
}
