using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Siege
{
	// Token: 0x0200011E RID: 286
	public class MapSiegeScreenWidget : Widget
	{
		// Token: 0x06000F1C RID: 3868 RVA: 0x0002993B File Offset: 0x00027B3B
		public MapSiegeScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x00029944 File Offset: 0x00027B44
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			Widget latestMouseUpWidget = base.EventManager.LatestMouseUpWidget;
			if (this._currentSelectedButton != null && latestMouseUpWidget != null && !(latestMouseUpWidget is MapSiegeMachineButtonWidget) && !this._currentSelectedButton.CheckIsMyChildRecursive(latestMouseUpWidget) && this.IsWidgetChildOfType<MapSiegeMachineButtonWidget>(latestMouseUpWidget) == null)
			{
				this.SetCurrentButton(null);
			}
			if (base.EventManager.LatestMouseUpWidget == null)
			{
				this.SetCurrentButton(null);
			}
			if (this.DeployableSiegeMachinesPopup != null)
			{
				this.DeployableSiegeMachinesPopup.IsVisible = this._currentSelectedButton != null;
			}
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x000299C8 File Offset: 0x00027BC8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._currentSelectedButton != null && this.DeployableSiegeMachinesPopup != null)
			{
				this.DeployableSiegeMachinesPopup.ScaledPositionXOffset = Mathf.Clamp(this._currentSelectedButton.GlobalPosition.X - this.DeployableSiegeMachinesPopup.Size.X / 2f + this._currentSelectedButton.Size.X / 2f, 0f, base.EventManager.PageSize.X - this.DeployableSiegeMachinesPopup.Size.X);
				this.DeployableSiegeMachinesPopup.ScaledPositionYOffset = Mathf.Clamp(this._currentSelectedButton.GlobalPosition.Y + this._currentSelectedButton.Size.Y + 10f * base._inverseScaleToUse, 0f, base.EventManager.PageSize.Y - this.DeployableSiegeMachinesPopup.Size.Y);
			}
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x00029ACA File Offset: 0x00027CCA
		public void SetCurrentButton(MapSiegeMachineButtonWidget button)
		{
			if (button == null)
			{
				this._currentSelectedButton = null;
				return;
			}
			if (this._currentSelectedButton == button || !button.IsDeploymentTarget)
			{
				this.SetCurrentButton(null);
				return;
			}
			this._currentSelectedButton = button;
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x00029AF7 File Offset: 0x00027CF7
		protected override bool OnPreviewMousePressed()
		{
			this.SetCurrentButton(null);
			return false;
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x00029B01 File Offset: 0x00027D01
		protected override bool OnPreviewDragEnd()
		{
			return false;
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x00029B04 File Offset: 0x00027D04
		protected override bool OnPreviewDragBegin()
		{
			return false;
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x00029B07 File Offset: 0x00027D07
		protected override bool OnPreviewDrop()
		{
			return false;
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x00029B0A File Offset: 0x00027D0A
		protected override bool OnPreviewDragHover()
		{
			return false;
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x00029B0D File Offset: 0x00027D0D
		protected override bool OnPreviewMouseMove()
		{
			return false;
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x00029B10 File Offset: 0x00027D10
		protected override bool OnPreviewMouseReleased()
		{
			return false;
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x00029B13 File Offset: 0x00027D13
		protected override bool OnPreviewMouseScroll()
		{
			return false;
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x00029B16 File Offset: 0x00027D16
		protected override bool OnPreviewMouseAlternatePressed()
		{
			return false;
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x00029B19 File Offset: 0x00027D19
		protected override bool OnPreviewMouseAlternateReleased()
		{
			return false;
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00029B1C File Offset: 0x00027D1C
		private T IsWidgetChildOfType<T>(Widget currentWidget) where T : Widget
		{
			while (currentWidget != null)
			{
				if (currentWidget is T)
				{
					return (T)((object)currentWidget);
				}
				currentWidget = currentWidget.ParentWidget;
			}
			return default(T);
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x00029B4E File Offset: 0x00027D4E
		// (set) Token: 0x06000F2C RID: 3884 RVA: 0x00029B56 File Offset: 0x00027D56
		[Editor(false)]
		public Widget DeployableSiegeMachinesPopup
		{
			get
			{
				return this._deployableSiegeMachinesPopup;
			}
			set
			{
				if (value != this._deployableSiegeMachinesPopup)
				{
					this._deployableSiegeMachinesPopup = value;
					base.OnPropertyChanged<Widget>(value, "DeployableSiegeMachinesPopup");
				}
			}
		}

		// Token: 0x040006E1 RID: 1761
		private Widget _deployableSiegeMachinesPopup;

		// Token: 0x040006E2 RID: 1762
		private MapSiegeMachineButtonWidget _currentSelectedButton;
	}
}
