using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.MainAgentControlMode
{
	// Token: 0x020000FA RID: 250
	public class MainAgentControlModeParentWidget : Widget
	{
		// Token: 0x06000D2F RID: 3375 RVA: 0x000241A2 File Offset: 0x000223A2
		public MainAgentControlModeParentWidget(UIContext context)
			: base(context)
		{
			this._selectionItems = new List<ButtonWidget>();
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x000241C4 File Offset: 0x000223C4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isSelectionItemsDirty && this._selectionItems != null && !string.IsNullOrEmpty(this.ChildItemId))
			{
				this.UnregisterChildEvents();
				this._selectionItems = this._controlModesList.FindChildrenWithId<ButtonWidget>(this.ChildItemId, true);
				this.RegisterChildEvents();
				this._isSelectionItemsDirty = false;
			}
			this.AnimationTick(dt);
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x00024227 File Offset: 0x00022427
		private void OnControlModesListUpdated(Widget widget, string eventName, object[] args)
		{
			if (eventName == "ItemAdd" || eventName == "ItemRemove")
			{
				this._isSelectionItemsDirty = true;
			}
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x0002424A File Offset: 0x0002244A
		private void OnControlItemUpdated(PropertyOwnerObject widget, string propertyName, bool value)
		{
			if (propertyName == "IsSelected" && !this.IsActive)
			{
				this.StartIndicatorAnimation();
			}
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00024267 File Offset: 0x00022467
		private void StartIndicatorAnimation()
		{
			this._animationTimer = 0f;
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00024274 File Offset: 0x00022474
		private void AnimationTick(float dt)
		{
			if (this.SelectionIndicatorWidget == null)
			{
				return;
			}
			if (this._animationTimer < this.AnimationFirstStepDuration + this.AnimationSecondStepDuration)
			{
				float num = ((this._animationTimer < this.AnimationFirstStepDuration) ? (this._animationTimer / this.AnimationFirstStepDuration) : ((this._animationTimer - this.AnimationFirstStepDuration) / this.AnimationSecondStepDuration));
				num = Mathf.Clamp(num, 0f, 1f);
				float num2 = ((this._animationTimer < this.AnimationFirstStepDuration) ? 1f : 0f);
				float num3 = Mathf.Lerp((this._animationTimer < this.AnimationFirstStepDuration) ? 0f : 1f, num2, num);
				this.SelectionIndicatorWidget.SetGlobalAlphaRecursively(num3);
				this._animationTimer += dt;
				return;
			}
			if (this.SelectionIndicatorWidget.AlphaFactor > 0f)
			{
				this.SelectionIndicatorWidget.SetGlobalAlphaRecursively(0f);
			}
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x00024360 File Offset: 0x00022560
		private void RegisterChildEvents()
		{
			for (int i = 0; i < this._selectionItems.Count; i++)
			{
				this._selectionItems[i].boolPropertyChanged += this.OnControlItemUpdated;
			}
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x000243A0 File Offset: 0x000225A0
		private void UnregisterChildEvents()
		{
			for (int i = 0; i < this._selectionItems.Count; i++)
			{
				this._selectionItems[i].boolPropertyChanged -= this.OnControlItemUpdated;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06000D37 RID: 3383 RVA: 0x000243E0 File Offset: 0x000225E0
		// (set) Token: 0x06000D38 RID: 3384 RVA: 0x000243E8 File Offset: 0x000225E8
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChanged(value, "IsActive");
				}
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06000D39 RID: 3385 RVA: 0x00024406 File Offset: 0x00022606
		// (set) Token: 0x06000D3A RID: 3386 RVA: 0x0002440E File Offset: 0x0002260E
		public float AnimationFirstStepDuration
		{
			get
			{
				return this._animationFirstStepDuration;
			}
			set
			{
				if (value != this._animationFirstStepDuration)
				{
					this._animationFirstStepDuration = value;
					base.OnPropertyChanged(value, "AnimationFirstStepDuration");
				}
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06000D3B RID: 3387 RVA: 0x0002442C File Offset: 0x0002262C
		// (set) Token: 0x06000D3C RID: 3388 RVA: 0x00024434 File Offset: 0x00022634
		public float AnimationSecondStepDuration
		{
			get
			{
				return this._animationSecondStepDuration;
			}
			set
			{
				if (value != this._animationSecondStepDuration)
				{
					this._animationSecondStepDuration = value;
					base.OnPropertyChanged(value, "AnimationSecondStepDuration");
				}
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06000D3D RID: 3389 RVA: 0x00024452 File Offset: 0x00022652
		// (set) Token: 0x06000D3E RID: 3390 RVA: 0x0002445A File Offset: 0x0002265A
		public string ChildItemId
		{
			get
			{
				return this._childItemId;
			}
			set
			{
				if (value != this._childItemId)
				{
					this._childItemId = value;
					base.OnPropertyChanged<string>(value, "ChildItemId");
				}
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06000D3F RID: 3391 RVA: 0x0002447D File Offset: 0x0002267D
		// (set) Token: 0x06000D40 RID: 3392 RVA: 0x00024488 File Offset: 0x00022688
		public ListPanel ControlModesList
		{
			get
			{
				return this._controlModesList;
			}
			set
			{
				if (value != this._controlModesList)
				{
					if (this._controlModesList != null)
					{
						this._controlModesList.EventFire -= this.OnControlModesListUpdated;
					}
					this._controlModesList = value;
					base.OnPropertyChanged<ListPanel>(value, "ControlModesList");
					if (this._controlModesList != null)
					{
						this._controlModesList.EventFire += this.OnControlModesListUpdated;
					}
					this._isSelectionItemsDirty = true;
				}
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06000D41 RID: 3393 RVA: 0x000244F6 File Offset: 0x000226F6
		// (set) Token: 0x06000D42 RID: 3394 RVA: 0x000244FE File Offset: 0x000226FE
		public Widget SelectionIndicatorWidget
		{
			get
			{
				return this._selectionIndicatorWidget;
			}
			set
			{
				if (value != this._selectionIndicatorWidget)
				{
					this._selectionIndicatorWidget = value;
					base.OnPropertyChanged<Widget>(value, "SelectionIndicatorWidget");
				}
			}
		}

		// Token: 0x040005F7 RID: 1527
		private List<ButtonWidget> _selectionItems;

		// Token: 0x040005F8 RID: 1528
		private float _animationTimer = float.MaxValue;

		// Token: 0x040005F9 RID: 1529
		private bool _isSelectionItemsDirty;

		// Token: 0x040005FA RID: 1530
		private bool _isActive;

		// Token: 0x040005FB RID: 1531
		private float _animationFirstStepDuration;

		// Token: 0x040005FC RID: 1532
		private float _animationSecondStepDuration;

		// Token: 0x040005FD RID: 1533
		private string _childItemId;

		// Token: 0x040005FE RID: 1534
		private ListPanel _controlModesList;

		// Token: 0x040005FF RID: 1535
		private Widget _selectionIndicatorWidget;
	}
}
