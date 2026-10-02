using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x02000018 RID: 24
	public class DeploymentSiegeMachineVM : ViewModel
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00006B57 File Offset: 0x00004D57
		public DeploymentPoint DeploymentPoint { get; }

		// Token: 0x060001CE RID: 462 RVA: 0x00006B60 File Offset: 0x00004D60
		public DeploymentSiegeMachineVM(DeploymentPoint selectedDeploymentPoint, SiegeWeapon siegeMachine, Camera deploymentCamera, Action<DeploymentSiegeMachineVM> onSelectSiegeMachine, Action<DeploymentPoint> onHoverSiegeMachine, bool isSelected)
		{
			this._deploymentCamera = deploymentCamera;
			this.DeploymentPoint = selectedDeploymentPoint;
			this._onSelect = onSelectSiegeMachine;
			this._onHover = onHoverSiegeMachine;
			this.SiegeWeapon = siegeMachine;
			this.IsSelected = isSelected;
			if (siegeMachine != null)
			{
				this.MachineType = siegeMachine.GetType();
				this.Machine = OrderSiegeMachineVM.GetSiegeType(this.MachineType, siegeMachine.Side);
				this.MachineClass = siegeMachine.GetSiegeEngineType().StringId;
			}
			else
			{
				this.MachineType = null;
				this.MachineClass = "Empty";
			}
			this.Type = (int)selectedDeploymentPoint.GetDeploymentPointType();
			this._worldPos = selectedDeploymentPoint.GameEntity.GlobalPosition;
			this.IsPlayerGeneral = Mission.Current.PlayerTeam.IsPlayerGeneral;
			this.RefreshValues();
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00006C38 File Offset: 0x00004E38
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.BreachedText = new TextObject("{=D0TbQm4r}BREACHED", null).ToString();
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00006C56 File Offset: 0x00004E56
		public void Update()
		{
			this.CalculatePosition();
			this.RefreshPosition();
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00006C64 File Offset: 0x00004E64
		public void CalculatePosition()
		{
			this._latestX = 0f;
			this._latestY = 0f;
			MatrixFrame identity = MatrixFrame.Identity;
			this._deploymentCamera.GetViewProjMatrix(ref identity);
			Vec3 worldPos = this._worldPos;
			worldPos.z += 8f;
			worldPos.w = 1f;
			Vec3 vec = worldPos * identity;
			this.IsInFront = vec.w > 0f;
			vec.x /= vec.w;
			vec.y /= vec.w;
			vec.z /= vec.w;
			vec.w /= vec.w;
			vec *= 0.5f;
			vec.x += 0.5f;
			vec.y += 0.5f;
			vec.y = 1f - vec.y;
			int num = (int)Screen.RealScreenResolutionWidth;
			int num2 = (int)Screen.RealScreenResolutionHeight;
			this._latestX = vec.x * (float)num;
			this._latestY = vec.y * (float)num2;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00006D89 File Offset: 0x00004F89
		public void RefreshPosition()
		{
			this.IsInside = this.IsInsideWindow();
			this.Position = new Vec2(this._latestX, this._latestY);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00006DB0 File Offset: 0x00004FB0
		private bool IsInsideWindow()
		{
			return this._latestX <= Screen.RealScreenResolutionWidth && this._latestY <= Screen.RealScreenResolutionHeight && this._latestX + 200f >= 0f && this._latestY + 100f >= 0f;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00006E02 File Offset: 0x00005002
		public void ExecuteAction()
		{
			Action<DeploymentSiegeMachineVM> onSelect = this._onSelect;
			if (onSelect == null)
			{
				return;
			}
			onSelect(this);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00006E15 File Offset: 0x00005015
		public void ExecuteFocusBegin()
		{
			Action<DeploymentPoint> onHover = this._onHover;
			if (onHover == null)
			{
				return;
			}
			onHover(this.DeploymentPoint);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00006E2D File Offset: 0x0000502D
		public void ExecuteFocusEnd()
		{
			Action<DeploymentPoint> onHover = this._onHover;
			if (onHover == null)
			{
				return;
			}
			onHover(null);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00006E40 File Offset: 0x00005040
		public void RefreshWithDeployedWeapon()
		{
			SiegeWeapon siegeWeapon = this.DeploymentPoint.DeployedWeapon as SiegeWeapon;
			this.SiegeWeapon = siegeWeapon;
			if (siegeWeapon != null)
			{
				this.MachineType = siegeWeapon.GetType();
				this.MachineClass = siegeWeapon.GetSiegeEngineType().StringId;
				return;
			}
			this.MachineType = null;
			this.MachineClass = "none";
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x00006E98 File Offset: 0x00005098
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x00006EA0 File Offset: 0x000050A0
		[DataSourceProperty]
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (value != this._type)
				{
					this._type = value;
					base.OnPropertyChangedWithValue(value, "Type");
				}
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001DA RID: 474 RVA: 0x00006EBE File Offset: 0x000050BE
		// (set) Token: 0x060001DB RID: 475 RVA: 0x00006EC6 File Offset: 0x000050C6
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

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001DC RID: 476 RVA: 0x00006EE4 File Offset: 0x000050E4
		// (set) Token: 0x060001DD RID: 477 RVA: 0x00006EEC File Offset: 0x000050EC
		[DataSourceProperty]
		public bool IsPlayerGeneral
		{
			get
			{
				return this._isPlayerGeneral;
			}
			set
			{
				if (value != this._isPlayerGeneral)
				{
					this._isPlayerGeneral = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerGeneral");
				}
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001DE RID: 478 RVA: 0x00006F0A File Offset: 0x0000510A
		// (set) Token: 0x060001DF RID: 479 RVA: 0x00006F12 File Offset: 0x00005112
		[DataSourceProperty]
		public string MachineClass
		{
			get
			{
				return this._machineClass;
			}
			set
			{
				if (value != this._machineClass)
				{
					this._machineClass = value;
					base.OnPropertyChangedWithValue<string>(value, "MachineClass");
				}
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x00006F35 File Offset: 0x00005135
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x00006F3D File Offset: 0x0000513D
		[DataSourceProperty]
		public string BreachedText
		{
			get
			{
				return this._breachedText;
			}
			set
			{
				if (value != this._breachedText)
				{
					this._breachedText = value;
					base.OnPropertyChangedWithValue<string>(value, "BreachedText");
				}
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x00006F60 File Offset: 0x00005160
		// (set) Token: 0x060001E3 RID: 483 RVA: 0x00006F68 File Offset: 0x00005168
		[DataSourceProperty]
		public int RemainingCount
		{
			get
			{
				return this._remainingCount;
			}
			set
			{
				if (value != this._remainingCount)
				{
					this._remainingCount = value;
					base.OnPropertyChangedWithValue(value, "RemainingCount");
				}
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x00006F86 File Offset: 0x00005186
		// (set) Token: 0x060001E5 RID: 485 RVA: 0x00006F8E File Offset: 0x0000518E
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

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00006FAC File Offset: 0x000051AC
		// (set) Token: 0x060001E7 RID: 487 RVA: 0x00006FB4 File Offset: 0x000051B4
		public bool IsInFront
		{
			get
			{
				return this._isInFront;
			}
			set
			{
				if (value != this._isInFront)
				{
					this._isInFront = value;
					base.OnPropertyChangedWithValue(value, "IsInFront");
				}
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x00006FD2 File Offset: 0x000051D2
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x00006FDA File Offset: 0x000051DA
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

		// Token: 0x040000D8 RID: 216
		public Type MachineType;

		// Token: 0x040000D9 RID: 217
		public SiegeEngineType Machine;

		// Token: 0x040000DA RID: 218
		public SiegeWeapon SiegeWeapon;

		// Token: 0x040000DB RID: 219
		private readonly Camera _deploymentCamera;

		// Token: 0x040000DC RID: 220
		private Vec3 _worldPos;

		// Token: 0x040000DD RID: 221
		private float _latestX;

		// Token: 0x040000DE RID: 222
		private float _latestY;

		// Token: 0x040000DF RID: 223
		private readonly Action<DeploymentSiegeMachineVM> _onSelect;

		// Token: 0x040000E0 RID: 224
		private readonly Action<DeploymentPoint> _onHover;

		// Token: 0x040000E1 RID: 225
		private string _machineClass = "";

		// Token: 0x040000E2 RID: 226
		private int _remainingCount = -1;

		// Token: 0x040000E3 RID: 227
		private bool _isSelected;

		// Token: 0x040000E4 RID: 228
		private bool _isPlayerGeneral;

		// Token: 0x040000E5 RID: 229
		private int _type;

		// Token: 0x040000E6 RID: 230
		private bool _isInside;

		// Token: 0x040000E7 RID: 231
		private bool _isInFront;

		// Token: 0x040000E8 RID: 232
		private string _breachedText;

		// Token: 0x040000E9 RID: 233
		private Vec2 _position;
	}
}
