using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.Layout;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200002E RID: 46
	public class NavigatableListPanel : ListPanel
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000874A File Offset: 0x0000694A
		// (set) Token: 0x06000267 RID: 615 RVA: 0x00008752 File Offset: 0x00006952
		public ScrollablePanel ParentPanel { get; set; }

		// Token: 0x06000268 RID: 616 RVA: 0x0000875B File Offset: 0x0000695B
		public NavigatableListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00008776 File Offset: 0x00006976
		protected override void OnLateUpdate(float dt)
		{
			if (this._areIndicesDirty)
			{
				this.RefreshChildNavigationIndices();
				this._areIndicesDirty = false;
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000878D File Offset: 0x0000698D
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			if (this.ParentPanel == null)
			{
				this.ParentPanel = base.FindParentPanel();
			}
		}

		// Token: 0x0600026B RID: 619 RVA: 0x000087AC File Offset: 0x000069AC
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.OnGamepadNavigationFocusGained = new Action<Widget>(this.OnWidgetGainedGamepadFocus);
			child.EventFire += this.OnChildSiblingIndexChanged;
			child.boolPropertyChanged += this.OnChildVisibilityChanged;
			this._areIndicesDirty = true;
			this.UpdateEmptyNavigationWidget();
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00008804 File Offset: 0x00006A04
		protected override void OnAfterChildRemoved(Widget child, int previousIndexOfChild)
		{
			base.OnAfterChildRemoved(child, previousIndexOfChild);
			child.OnGamepadNavigationFocusGained = null;
			child.EventFire -= this.OnChildSiblingIndexChanged;
			child.boolPropertyChanged -= this.OnChildVisibilityChanged;
			child.GamepadNavigationIndex = -1;
			this.UpdateEmptyNavigationWidget();
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00008854 File Offset: 0x00006A54
		protected override void OnDisconnectedFromRoot()
		{
			base.OnDisconnectedFromRoot();
			for (int i = 0; i < base.Children.Count; i++)
			{
				base.Children[i].OnGamepadNavigationFocusGained = null;
				base.Children[i].EventFire -= this.OnChildSiblingIndexChanged;
				base.Children[i].boolPropertyChanged -= this.OnChildVisibilityChanged;
				base.Children[i].GamepadNavigationIndex = -1;
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x000088DC File Offset: 0x00006ADC
		private void OnChildVisibilityChanged(PropertyOwnerObject child, string propertyName, bool value)
		{
			if (propertyName == "IsVisible")
			{
				Widget widget = (Widget)child;
				if (!value)
				{
					widget.GamepadNavigationIndex = -1;
					return;
				}
				this.SetNavigationIndexForChild(widget);
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00008910 File Offset: 0x00006B10
		private void OnWidgetGainedGamepadFocus(Widget widget)
		{
			if (this.ParentPanel != null)
			{
				ScrollablePanel.AutoScrollParameters autoScrollParameters = new ScrollablePanel.AutoScrollParameters((float)this.AutoScrollTopOffset, (float)this.AutoScrollBottomOffset, (float)this.AutoScrollLeftOffset, (float)this.AutoScrollRightOffset, -1f, -1f, 0f);
				this.ParentPanel.ScrollToChild(widget, autoScrollParameters);
			}
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00008963 File Offset: 0x00006B63
		private void OnChildSiblingIndexChanged(Widget widget, string eventName, object[] parameters)
		{
			if (eventName == "SiblingIndexChanged")
			{
				this._areIndicesDirty = true;
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000897C File Offset: 0x00006B7C
		private void SetNavigationIndexForChild(Widget widget)
		{
			int num;
			if (base.StackLayout.LayoutMethod == LayoutMethod.VerticalBottomToTop || base.StackLayout.LayoutMethod == LayoutMethod.HorizontalRightToLeft)
			{
				num = this.MaxIndex - widget.GetSiblingIndex() * this.StepSize;
			}
			else
			{
				num = this.MinIndex + widget.GetSiblingIndex() * this.StepSize;
			}
			if (num <= this.MaxIndex)
			{
				widget.GamepadNavigationIndex = num;
			}
		}

		// Token: 0x06000272 RID: 626 RVA: 0x000089E1 File Offset: 0x00006BE1
		protected override void OnGamepadNavigationIndexUpdated(int newIndex)
		{
			if (newIndex != -1 && this.UseSelfIndexForMinimum)
			{
				this.SetNavigationIndicesFromSelf();
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x000089F5 File Offset: 0x00006BF5
		private void SetNavigationIndicesFromSelf()
		{
			this.MinIndex = base.GamepadNavigationIndex;
			base.GamepadNavigationIndex = -1;
			this._areIndicesDirty = true;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00008A14 File Offset: 0x00006C14
		protected void RefreshChildNavigationIndices()
		{
			for (int i = 0; i < base.Children.Count; i++)
			{
				this.SetNavigationIndexForChild(base.Children[i]);
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00008A49 File Offset: 0x00006C49
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

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000276 RID: 630 RVA: 0x00008A7E File Offset: 0x00006C7E
		// (set) Token: 0x06000277 RID: 631 RVA: 0x00008A86 File Offset: 0x00006C86
		public int AutoScrollTopOffset { get; set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000278 RID: 632 RVA: 0x00008A8F File Offset: 0x00006C8F
		// (set) Token: 0x06000279 RID: 633 RVA: 0x00008A97 File Offset: 0x00006C97
		public int AutoScrollBottomOffset { get; set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600027A RID: 634 RVA: 0x00008AA0 File Offset: 0x00006CA0
		// (set) Token: 0x0600027B RID: 635 RVA: 0x00008AA8 File Offset: 0x00006CA8
		public int AutoScrollLeftOffset { get; set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600027C RID: 636 RVA: 0x00008AB1 File Offset: 0x00006CB1
		// (set) Token: 0x0600027D RID: 637 RVA: 0x00008AB9 File Offset: 0x00006CB9
		public int AutoScrollRightOffset { get; set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600027E RID: 638 RVA: 0x00008AC2 File Offset: 0x00006CC2
		// (set) Token: 0x0600027F RID: 639 RVA: 0x00008ACA File Offset: 0x00006CCA
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

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000280 RID: 640 RVA: 0x00008AE2 File Offset: 0x00006CE2
		// (set) Token: 0x06000281 RID: 641 RVA: 0x00008AEA File Offset: 0x00006CEA
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

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000282 RID: 642 RVA: 0x00008B02 File Offset: 0x00006D02
		// (set) Token: 0x06000283 RID: 643 RVA: 0x00008B0A File Offset: 0x00006D0A
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

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000284 RID: 644 RVA: 0x00008B22 File Offset: 0x00006D22
		// (set) Token: 0x06000285 RID: 645 RVA: 0x00008B2A File Offset: 0x00006D2A
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

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000286 RID: 646 RVA: 0x00008B53 File Offset: 0x00006D53
		// (set) Token: 0x06000287 RID: 647 RVA: 0x00008B5B File Offset: 0x00006D5B
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

		// Token: 0x0400011B RID: 283
		private bool _areIndicesDirty;

		// Token: 0x0400011D RID: 285
		private int _minIndex;

		// Token: 0x0400011E RID: 286
		private int _maxIndex = int.MaxValue;

		// Token: 0x0400011F RID: 287
		private int _stepSize = 1;

		// Token: 0x04000120 RID: 288
		private bool _useSelfIndexForMinimum;

		// Token: 0x04000121 RID: 289
		private Widget _emptyNavigationWidget;
	}
}
