using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Order
{
	// Token: 0x02000072 RID: 114
	public class OrderSiegeDeploymentItemButtonWidget : ButtonWidget
	{
		// Token: 0x06000616 RID: 1558 RVA: 0x00011F23 File Offset: 0x00010123
		public OrderSiegeDeploymentItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00011F34 File Offset: 0x00010134
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			base.IsVisible = this.IsInsideWindow && this.IsInFront;
			base.IsEnabled = this.IsPlayerGeneral && this.PointType != 2;
			if (this.preSelectedState != base.IsSelected)
			{
				if (base.IsSelected)
				{
					this.ScreenWidget.SetSelectedDeploymentItem(this);
				}
				this.preSelectedState = base.IsSelected;
			}
			if (this._isVisualsDirty)
			{
				this.UpdateTypeVisuals();
				this._isVisualsDirty = false;
			}
			this.UpdatePosition();
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00011FC8 File Offset: 0x000101C8
		private void UpdatePosition()
		{
			if (this.IsInsideWindow)
			{
				base.ScaledPositionXOffset = this.Position.x - base.Size.X / 2f;
				base.ScaledPositionYOffset = this.Position.y - base.Size.Y;
			}
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00012020 File Offset: 0x00010220
		private void UpdateTypeVisuals()
		{
			this.TypeIconWidget.RegisterBrushStatesOfWidget();
			this.BreachedTextWidget.IsVisible = this.PointType == 2;
			this.TypeIconWidget.IsVisible = this.PointType != 2;
			if (this.PointType == 0)
			{
				this.TypeIconWidget.SetState("BatteringRam");
				return;
			}
			if (this.PointType == 1)
			{
				this.TypeIconWidget.SetState("TowerLadder");
				return;
			}
			if (this.PointType == 2)
			{
				this.TypeIconWidget.SetState("Breach");
				return;
			}
			if (this.PointType == 3)
			{
				this.TypeIconWidget.SetState("Ranged");
				return;
			}
			this.TypeIconWidget.SetState("Default");
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x000120DA File Offset: 0x000102DA
		// (set) Token: 0x0600061B RID: 1563 RVA: 0x000120E2 File Offset: 0x000102E2
		[Editor(false)]
		public TextWidget BreachedTextWidget
		{
			get
			{
				return this._breachedTextWidget;
			}
			set
			{
				if (this._breachedTextWidget != value)
				{
					this._breachedTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "BreachedTextWidget");
					this._isVisualsDirty = true;
				}
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x00012107 File Offset: 0x00010307
		// (set) Token: 0x0600061D RID: 1565 RVA: 0x0001210F File Offset: 0x0001030F
		[Editor(false)]
		public Widget TypeIconWidget
		{
			get
			{
				return this._typeIconWidget;
			}
			set
			{
				if (this._typeIconWidget != value)
				{
					this._typeIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "TypeIconWidget");
					this._isVisualsDirty = true;
				}
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x00012134 File Offset: 0x00010334
		// (set) Token: 0x0600061F RID: 1567 RVA: 0x0001213C File Offset: 0x0001033C
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
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x0001215F File Offset: 0x0001035F
		// (set) Token: 0x06000621 RID: 1569 RVA: 0x00012167 File Offset: 0x00010367
		public int PointType
		{
			get
			{
				return this._pointType;
			}
			set
			{
				if (this._pointType != value)
				{
					this._pointType = value;
					base.OnPropertyChanged(value, "PointType");
				}
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x00012185 File Offset: 0x00010385
		// (set) Token: 0x06000623 RID: 1571 RVA: 0x0001218D File Offset: 0x0001038D
		public bool IsInsideWindow
		{
			get
			{
				return this._isInsideWindow;
			}
			set
			{
				if (this._isInsideWindow != value)
				{
					this._isInsideWindow = value;
					base.OnPropertyChanged(value, "IsInsideWindow");
				}
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x000121AB File Offset: 0x000103AB
		// (set) Token: 0x06000625 RID: 1573 RVA: 0x000121B3 File Offset: 0x000103B3
		public bool IsInFront
		{
			get
			{
				return this._isInFront;
			}
			set
			{
				if (this._isInFront != value)
				{
					this._isInFront = value;
					base.OnPropertyChanged(value, "IsInFront");
				}
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x000121D1 File Offset: 0x000103D1
		// (set) Token: 0x06000627 RID: 1575 RVA: 0x000121D9 File Offset: 0x000103D9
		public bool IsPlayerGeneral
		{
			get
			{
				return this._isPlayerGeneral;
			}
			set
			{
				if (this._isPlayerGeneral != value)
				{
					this._isPlayerGeneral = value;
					base.OnPropertyChanged(value, "IsPlayerGeneral");
				}
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x000121F7 File Offset: 0x000103F7
		// (set) Token: 0x06000629 RID: 1577 RVA: 0x000121FF File Offset: 0x000103FF
		public OrderSiegeDeploymentScreenWidget ScreenWidget
		{
			get
			{
				return this._screenWidget;
			}
			set
			{
				if (this._screenWidget != value)
				{
					this._screenWidget = value;
					base.OnPropertyChanged<OrderSiegeDeploymentScreenWidget>(value, "ScreenWidget");
				}
			}
		}

		// Token: 0x0400029D RID: 669
		private bool preSelectedState;

		// Token: 0x0400029E RID: 670
		private bool _isVisualsDirty = true;

		// Token: 0x0400029F RID: 671
		private Vec2 _position;

		// Token: 0x040002A0 RID: 672
		private bool _isInsideWindow;

		// Token: 0x040002A1 RID: 673
		private bool _isInFront;

		// Token: 0x040002A2 RID: 674
		private bool _isPlayerGeneral;

		// Token: 0x040002A3 RID: 675
		private OrderSiegeDeploymentScreenWidget _screenWidget;

		// Token: 0x040002A4 RID: 676
		private int _pointType;

		// Token: 0x040002A5 RID: 677
		private Widget _typeIconWidget;

		// Token: 0x040002A6 RID: 678
		private TextWidget _breachedTextWidget;
	}
}
