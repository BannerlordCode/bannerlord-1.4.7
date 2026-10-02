using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200002D RID: 45
	public class NavigatableGridWidget : GridWidget
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000832F File Offset: 0x0000652F
		// (set) Token: 0x06000245 RID: 581 RVA: 0x00008337 File Offset: 0x00006537
		public ScrollablePanel ParentPanel { get; set; }

		// Token: 0x06000246 RID: 582 RVA: 0x00008340 File Offset: 0x00006540
		public NavigatableGridWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000835C File Offset: 0x0000655C
		protected override void OnLateUpdate(float dt)
		{
			if (this._areIndicesDirty)
			{
				for (int i = 0; i < base.ChildCount; i++)
				{
					base.Children[i].GamepadNavigationIndex = -1;
				}
				this.RefreshChildNavigationIndices();
				this._areIndicesDirty = false;
			}
		}

		// Token: 0x06000248 RID: 584 RVA: 0x000083A1 File Offset: 0x000065A1
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			if (this.ParentPanel == null)
			{
				this.ParentPanel = base.FindParentPanel();
			}
		}

		// Token: 0x06000249 RID: 585 RVA: 0x000083C0 File Offset: 0x000065C0
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.OnGamepadNavigationFocusGained = new Action<Widget>(this.OnWidgetGainedGamepadFocus);
			child.EventFire += this.OnChildSiblingIndexChanged;
			child.boolPropertyChanged += this.OnChildVisibilityChanged;
			this._areIndicesDirty = true;
			this.UpdateEmptyNavigationWidget();
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00008418 File Offset: 0x00006618
		protected override void OnAfterChildRemoved(Widget child, int previousIndexOfChild)
		{
			base.OnAfterChildRemoved(child, previousIndexOfChild);
			this._areIndicesDirty = true;
			child.OnGamepadNavigationFocusGained = null;
			child.EventFire -= this.OnChildSiblingIndexChanged;
			child.boolPropertyChanged -= this.OnChildVisibilityChanged;
			child.GamepadNavigationIndex = -1;
			this.UpdateEmptyNavigationWidget();
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000846C File Offset: 0x0000666C
		protected override void OnDisconnectedFromRoot()
		{
			base.OnDisconnectedFromRoot();
			for (int i = 0; i < base.Children.Count; i++)
			{
				base.Children[i].OnGamepadNavigationFocusGained = null;
				base.Children[i].EventFire -= this.OnChildSiblingIndexChanged;
				base.Children[i].boolPropertyChanged -= this.OnChildVisibilityChanged;
			}
		}

		// Token: 0x0600024C RID: 588 RVA: 0x000084E1 File Offset: 0x000066E1
		private void OnChildVisibilityChanged(PropertyOwnerObject child, string propertyName, bool value)
		{
			if (propertyName == "IsVisible")
			{
				this._areIndicesDirty = true;
			}
		}

		// Token: 0x0600024D RID: 589 RVA: 0x000084F8 File Offset: 0x000066F8
		private void OnWidgetGainedGamepadFocus(Widget widget)
		{
			if (this.ParentPanel != null)
			{
				ScrollablePanel.AutoScrollParameters autoScrollParameters = new ScrollablePanel.AutoScrollParameters((float)this.AutoScrollTopOffset, (float)this.AutoScrollBottomOffset, (float)this.AutoScrollLeftOffset, (float)this.AutoScrollRightOffset, -1f, -1f, 0f);
				this.ParentPanel.ScrollToChild(widget, autoScrollParameters);
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000854B File Offset: 0x0000674B
		private void OnChildSiblingIndexChanged(Widget widget, string eventName, object[] parameters)
		{
			if (eventName == "SiblingIndexChanged")
			{
				this._areIndicesDirty = true;
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00008564 File Offset: 0x00006764
		private void SetNavigationIndexForChild(Widget widget)
		{
			if (!widget.IsVisible)
			{
				widget.GamepadNavigationIndex = -1;
				return;
			}
			int num = this.MinIndex + widget.GetVisibleSiblingIndex() * this.StepSize;
			if (num <= this.MaxIndex)
			{
				widget.GamepadNavigationIndex = num;
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x000085A6 File Offset: 0x000067A6
		protected override void OnGamepadNavigationIndexUpdated(int newIndex)
		{
			if (newIndex != -1 && this.UseSelfIndexForMinimum)
			{
				this.SetNavigationIndicesFromSelf();
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x000085BA File Offset: 0x000067BA
		private void SetNavigationIndicesFromSelf()
		{
			this.MinIndex = base.GamepadNavigationIndex;
			base.GamepadNavigationIndex = -1;
			this._areIndicesDirty = true;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x000085D6 File Offset: 0x000067D6
		private void UpdateEmptyNavigationWidget()
		{
			if (this._emptyNavigationWidget != null)
			{
				if (base.Children.Count == 0)
				{
					this.EmptyNavigationWidget.GamepadNavigationIndex = this.MinIndex;
					return;
				}
				this.EmptyNavigationWidget.GamepadNavigationIndex = -1;
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000860C File Offset: 0x0000680C
		protected void RefreshChildNavigationIndices()
		{
			for (int i = 0; i < base.Children.Count; i++)
			{
				this.SetNavigationIndexForChild(base.Children[i]);
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00008641 File Offset: 0x00006841
		// (set) Token: 0x06000255 RID: 597 RVA: 0x00008649 File Offset: 0x00006849
		public int AutoScrollTopOffset { get; set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000256 RID: 598 RVA: 0x00008652 File Offset: 0x00006852
		// (set) Token: 0x06000257 RID: 599 RVA: 0x0000865A File Offset: 0x0000685A
		public int AutoScrollBottomOffset { get; set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000258 RID: 600 RVA: 0x00008663 File Offset: 0x00006863
		// (set) Token: 0x06000259 RID: 601 RVA: 0x0000866B File Offset: 0x0000686B
		public int AutoScrollLeftOffset { get; set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00008674 File Offset: 0x00006874
		// (set) Token: 0x0600025B RID: 603 RVA: 0x0000867C File Offset: 0x0000687C
		public int AutoScrollRightOffset { get; set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00008685 File Offset: 0x00006885
		// (set) Token: 0x0600025D RID: 605 RVA: 0x0000868D File Offset: 0x0000688D
		public int MinIndex
		{
			get
			{
				return this._minIndex;
			}
			set
			{
				if (value != this._minIndex)
				{
					this._minIndex = value;
					this.RefreshChildNavigationIndices();
				}
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600025E RID: 606 RVA: 0x000086A5 File Offset: 0x000068A5
		// (set) Token: 0x0600025F RID: 607 RVA: 0x000086AD File Offset: 0x000068AD
		public int MaxIndex
		{
			get
			{
				return this._maxIndex;
			}
			set
			{
				if (value != this._maxIndex)
				{
					this._maxIndex = value;
					this.RefreshChildNavigationIndices();
				}
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000260 RID: 608 RVA: 0x000086C5 File Offset: 0x000068C5
		// (set) Token: 0x06000261 RID: 609 RVA: 0x000086CD File Offset: 0x000068CD
		public int StepSize
		{
			get
			{
				return this._stepSize;
			}
			set
			{
				if (value != this._stepSize)
				{
					this._stepSize = value;
					this.RefreshChildNavigationIndices();
				}
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000262 RID: 610 RVA: 0x000086E5 File Offset: 0x000068E5
		// (set) Token: 0x06000263 RID: 611 RVA: 0x000086ED File Offset: 0x000068ED
		public bool UseSelfIndexForMinimum
		{
			get
			{
				return this._useSelfIndexForMinimum;
			}
			set
			{
				if (value != this._useSelfIndexForMinimum)
				{
					this._useSelfIndexForMinimum = value;
					if (this._useSelfIndexForMinimum && base.GamepadNavigationIndex != -1)
					{
						this.SetNavigationIndicesFromSelf();
					}
				}
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000264 RID: 612 RVA: 0x00008716 File Offset: 0x00006916
		// (set) Token: 0x06000265 RID: 613 RVA: 0x0000871E File Offset: 0x0000691E
		public Widget EmptyNavigationWidget
		{
			get
			{
				return this._emptyNavigationWidget;
			}
			set
			{
				if (value != this._emptyNavigationWidget)
				{
					if (this._emptyNavigationWidget != null)
					{
						this._emptyNavigationWidget.GamepadNavigationIndex = -1;
					}
					this._emptyNavigationWidget = value;
					this.UpdateEmptyNavigationWidget();
				}
			}
		}

		// Token: 0x04000111 RID: 273
		private bool _areIndicesDirty;

		// Token: 0x04000116 RID: 278
		private int _minIndex;

		// Token: 0x04000117 RID: 279
		private int _maxIndex = int.MaxValue;

		// Token: 0x04000118 RID: 280
		private int _stepSize = 1;

		// Token: 0x04000119 RID: 281
		private bool _useSelfIndexForMinimum;

		// Token: 0x0400011A RID: 282
		private Widget _emptyNavigationWidget;
	}
}
