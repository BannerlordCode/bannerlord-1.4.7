using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x0200013A RID: 314
	public class InventoryEquippedItemControlsBrushWidget : BrushWidget
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06001051 RID: 4177 RVA: 0x0002C920 File Offset: 0x0002AB20
		// (remove) Token: 0x06001052 RID: 4178 RVA: 0x0002C958 File Offset: 0x0002AB58
		public event Action OnHidePanel;

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001053 RID: 4179 RVA: 0x0002C98D File Offset: 0x0002AB8D
		// (set) Token: 0x06001054 RID: 4180 RVA: 0x0002C995 File Offset: 0x0002AB95
		public NavigationForcedScopeCollectionTargeter ForcedScopeCollection { get; set; }

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001055 RID: 4181 RVA: 0x0002C99E File Offset: 0x0002AB9E
		// (set) Token: 0x06001056 RID: 4182 RVA: 0x0002C9A6 File Offset: 0x0002ABA6
		public NavigationScopeTargeter NavigationScope { get; set; }

		// Token: 0x06001057 RID: 4183 RVA: 0x0002C9AF File Offset: 0x0002ABAF
		public InventoryEquippedItemControlsBrushWidget(UIContext context)
			: base(context)
		{
			base.AddState("LeftHidden");
			base.AddState("LeftVisible");
			base.AddState("RightHidden");
			base.AddState("RightVisible");
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x0002C9E4 File Offset: 0x0002ABE4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isScopeDirty && base.EventManager.Time - this._lastTransitionStartTime > base.VisualDefinition.TransitionDuration)
			{
				this.ForcedScopeCollection.IsCollectionDisabled = base.CurrentState == "RightHidden" || base.CurrentState == "LeftHidden";
				this.NavigationScope.IsScopeDisabled = this.ForcedScopeCollection.IsCollectionDisabled;
				this._isScopeDirty = false;
			}
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x0002CA6C File Offset: 0x0002AC6C
		public void ShowPanel()
		{
			if (this._panelVisible)
			{
				return;
			}
			if (this.ItemWidget.IsRightSide)
			{
				base.HorizontalAlignment = HorizontalAlignment.Right;
				base.Brush.HorizontalFlip = false;
				this.SetState("RightHidden");
				base.PositionXOffset = base.VisualDefinition.VisualStates["RightHidden"].PositionXOffset;
				this.SetState("RightVisible");
			}
			else
			{
				base.HorizontalAlignment = HorizontalAlignment.Left;
				base.Brush.HorizontalFlip = true;
				this.SetState("LeftHidden");
				base.PositionXOffset = base.VisualDefinition.VisualStates["LeftHidden"].PositionXOffset;
				this.SetState("LeftVisible");
			}
			base.IsVisible = true;
			this._panelVisible = true;
			this._isScopeDirty = true;
			this._lastTransitionStartTime = base.Context.EventManager.Time;
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x0002CB50 File Offset: 0x0002AD50
		public void HidePanel()
		{
			if (!this._panelVisible)
			{
				return;
			}
			if (this.ItemWidget.IsRightSide)
			{
				this.SetState("RightHidden");
			}
			else
			{
				this.SetState("LeftHidden");
			}
			Action onHidePanel = this.OnHidePanel;
			if (onHidePanel != null)
			{
				onHidePanel();
			}
			this._panelVisible = false;
			this._isScopeDirty = true;
			this._lastTransitionStartTime = base.Context.EventManager.Time;
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x0002CBC0 File Offset: 0x0002ADC0
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._panelVisible && this.ItemWidget.IsSelected)
			{
				this.ShowPanel();
				return;
			}
			if (this._panelVisible && !this.ItemWidget.IsSelected)
			{
				this.HidePanel();
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x0600105C RID: 4188 RVA: 0x0002CC00 File Offset: 0x0002AE00
		// (set) Token: 0x0600105D RID: 4189 RVA: 0x0002CC08 File Offset: 0x0002AE08
		[Editor(false)]
		public InventoryItemButtonWidget ItemWidget
		{
			get
			{
				return this._itemWidget;
			}
			set
			{
				if (this._itemWidget != value)
				{
					this._itemWidget = value;
					base.OnPropertyChanged<InventoryItemButtonWidget>(value, "ItemWidget");
				}
			}
		}

		// Token: 0x04000762 RID: 1890
		private float _lastTransitionStartTime;

		// Token: 0x04000763 RID: 1891
		private bool _isScopeDirty;

		// Token: 0x04000764 RID: 1892
		private bool _panelVisible;

		// Token: 0x04000767 RID: 1895
		private InventoryItemButtonWidget _itemWidget;

		// Token: 0x020001CC RID: 460
		// (Invoke) Token: 0x0600155F RID: 5471
		public delegate void ButtonClickEventHandler(Widget itemWidget);
	}
}
