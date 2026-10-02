using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Order
{
	// Token: 0x02000073 RID: 115
	public class OrderSiegeDeploymentScreenWidget : Widget
	{
		// Token: 0x0600062A RID: 1578 RVA: 0x0001221D File Offset: 0x0001041D
		public OrderSiegeDeploymentScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00012228 File Offset: 0x00010428
		public void SetSelectedDeploymentItem(OrderSiegeDeploymentItemButtonWidget deploymentItem)
		{
			this.DeploymentListPanel.ParentWidget.IsVisible = deploymentItem != null;
			if (deploymentItem == null)
			{
				return;
			}
			this.DeploymentListPanel.MarginLeft = (deploymentItem.GlobalPosition.X + deploymentItem.Size.Y + 20f) / base._scaleToUse;
			this.DeploymentListPanel.MarginTop = (deploymentItem.GlobalPosition.Y + (deploymentItem.Size.Y / 2f - this.DeploymentListPanel.Size.Y / 2f)) / base._scaleToUse;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x000122C2 File Offset: 0x000104C2
		private void UpdateEnabledState(bool isEnabled)
		{
			this.SetGlobalAlphaRecursively(isEnabled ? 1f : 0.5f);
			base.DoNotPassEventsToChildren = !isEnabled;
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x000122E3 File Offset: 0x000104E3
		// (set) Token: 0x0600062E RID: 1582 RVA: 0x000122EB File Offset: 0x000104EB
		public bool IsSiegeDeploymentDisabled
		{
			get
			{
				return this._isSiegeDeploymentDisabled;
			}
			set
			{
				if (value != this._isSiegeDeploymentDisabled)
				{
					this._isSiegeDeploymentDisabled = value;
					base.OnPropertyChanged(value, "IsSiegeDeploymentDisabled");
					this.UpdateEnabledState(!value);
				}
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x00012313 File Offset: 0x00010513
		// (set) Token: 0x06000630 RID: 1584 RVA: 0x0001231B File Offset: 0x0001051B
		public Widget DeploymentTargetsParent
		{
			get
			{
				return this._deploymentTargetsParent;
			}
			set
			{
				if (this._deploymentTargetsParent != value)
				{
					this._deploymentTargetsParent = value;
					base.OnPropertyChanged<Widget>(value, "DeploymentTargetsParent");
				}
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x00012339 File Offset: 0x00010539
		// (set) Token: 0x06000632 RID: 1586 RVA: 0x00012341 File Offset: 0x00010541
		public ListPanel DeploymentListPanel
		{
			get
			{
				return this._deploymentListPanel;
			}
			set
			{
				if (this._deploymentListPanel != value)
				{
					this._deploymentListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "DeploymentListPanel");
				}
			}
		}

		// Token: 0x040002A7 RID: 679
		private bool _isSiegeDeploymentDisabled;

		// Token: 0x040002A8 RID: 680
		private Widget _deploymentTargetsParent;

		// Token: 0x040002A9 RID: 681
		private ListPanel _deploymentListPanel;
	}
}
