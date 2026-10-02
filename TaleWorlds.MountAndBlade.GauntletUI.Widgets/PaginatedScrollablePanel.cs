using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000034 RID: 52
	public class PaginatedScrollablePanel : ScrollablePanel
	{
		// Token: 0x06000315 RID: 789 RVA: 0x00009B0E File Offset: 0x00007D0E
		public PaginatedScrollablePanel(UIContext context)
			: base(context)
		{
			this.ItemsPerPage = 4;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00009B20 File Offset: 0x00007D20
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			int num = -1;
			for (int i = 0; i < this.ListWidget.ChildCount; i++)
			{
				Widget child = this.ListWidget.GetChild(i);
				object obj;
				if (child == null)
				{
					obj = null;
				}
				else
				{
					obj = child.GetFirstInChildrenAndThisRecursive((Widget x) => x is ButtonWidget);
				}
				ButtonWidget buttonWidget;
				if ((buttonWidget = obj as ButtonWidget) != null && buttonWidget.IsSelected)
				{
					num = i;
					break;
				}
			}
			if (this._selectedIndex != num)
			{
				this._selectedIndex = num;
				this.OnSelectedWidgetUpdated();
			}
			bool flag = base.IsRecursivelyVisible();
			if (this._isRecursivelyVisible != flag)
			{
				this._isRecursivelyVisible = flag;
				this.OnVisibilityUpdated();
			}
			ScrollablePanel.ScrollbarInterpolationController horizontalScrollbarInterpolationController = this._horizontalScrollbarInterpolationController;
			bool flag2;
			if (horizontalScrollbarInterpolationController == null || !horizontalScrollbarInterpolationController.IsInterpolating)
			{
				ScrollablePanel.ScrollbarInterpolationController verticalScrollbarInterpolationController = this._verticalScrollbarInterpolationController;
				flag2 = verticalScrollbarInterpolationController != null && verticalScrollbarInterpolationController.IsInterpolating;
			}
			else
			{
				flag2 = true;
			}
			bool flag3 = flag2;
			if (this._isInterpolating != flag3)
			{
				this._isInterpolating = flag3;
				if (this.NavigationScope != null)
				{
					this.NavigationScope.DoNotAutoNavigateAfterSort = this._isInterpolating;
				}
				this.UpdateChildrenNavigationStates();
			}
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00009C26 File Offset: 0x00007E26
		private void ListWidget_EventFire(Widget widget, string eventName, object[] eventArgs)
		{
			if (eventName == "ItemAdd" || eventName == "ItemRemove" || eventName == "AfterItemRemove")
			{
				this.UpdatePageInfo();
				this.UpdateChildrenNavigationStates();
			}
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00009C5B File Offset: 0x00007E5B
		private void OnPreviousButtonPressed(Widget widget)
		{
			this.UpdatePageInfo();
			this._pageIndex = Mathf.Clamp(this._pageIndex - 1, 0, this._maxPages - 1);
			this.UpdateButtonEnabledStates();
			this.UpdateChildrenNavigationStates();
			this.UpdateViewport();
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00009C91 File Offset: 0x00007E91
		private void OnNextButtonPressed(Widget widget)
		{
			this.UpdatePageInfo();
			this._pageIndex = Mathf.Clamp(this._pageIndex + 1, 0, this._maxPages - 1);
			this.UpdateButtonEnabledStates();
			this.UpdateChildrenNavigationStates();
			this.UpdateViewport();
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00009CC8 File Offset: 0x00007EC8
		private void UpdatePageInfo()
		{
			if (this.ListWidget == null || base.ClipRect == null)
			{
				this._pageIndex = 0;
				this._maxPages = 0;
				return;
			}
			int childCount = this.ListWidget.ChildCount;
			this._maxPages = (int)Mathf.Ceil((float)childCount / (float)this.ItemsPerPage);
			this._pageIndex = Mathf.Clamp(this._pageIndex, 0, this._maxPages - 1);
			this.UpdateButtonEnabledStates();
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00009D38 File Offset: 0x00007F38
		private void UpdateButtonEnabledStates()
		{
			if (this._maxPages == 1)
			{
				if (this.PreviousButtonWidget != null)
				{
					this.PreviousButtonWidget.IsDisabled = true;
					this.PreviousButtonWidget.DoNotAcceptNavigation = this.PreviousButtonWidget.IsDisabled;
				}
				if (this.NextButtonWidget != null)
				{
					this.NextButtonWidget.IsDisabled = true;
					this.NextButtonWidget.DoNotAcceptNavigation = this.NextButtonWidget.IsDisabled;
					return;
				}
			}
			else
			{
				if (this.PreviousButtonWidget != null)
				{
					this.PreviousButtonWidget.IsDisabled = this._pageIndex == 0;
					this.PreviousButtonWidget.DoNotAcceptNavigation = this.PreviousButtonWidget.IsDisabled;
				}
				if (this.NextButtonWidget != null)
				{
					this.NextButtonWidget.IsDisabled = this._pageIndex == this._maxPages - 1;
					this.NextButtonWidget.DoNotAcceptNavigation = this.NextButtonWidget.IsDisabled;
				}
			}
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00009E14 File Offset: 0x00008014
		private void UpdateViewport()
		{
			if (base.ClipRect == null || this.ListWidget == null || this.ListWidget.ChildCount == 0)
			{
				return;
			}
			Vector2 vector = Vector2.Zero;
			for (int i = 0; i < this.ListWidget.ChildCount; i++)
			{
				Widget child = this.ListWidget.GetChild(i);
				vector += child.Size + new Vector2(child.ScaledMarginLeft + child.ScaledMarginRight, child.ScaledMarginBottom + child.ScaledMarginTop);
			}
			vector /= (float)this.ListWidget.ChildCount;
			if (this.ContainerDirection == PaginatedScrollablePanel.ContainerDirections.Horizontal && base.HorizontalScrollbar != null)
			{
				float value = this._horizontalScrollbarInterpolationController.GetValue();
				float num = vector.X * (float)this.ItemsPerPage;
				float num2 = Mathf.Abs(value - vector.X * (float)this.ItemsPerPage * (float)this._pageIndex) / num * this.ScrollTime;
				this._horizontalScrollbarInterpolationController.StartInterpolation(vector.X * (float)this.ItemsPerPage * (float)this._pageIndex, num2);
				return;
			}
			if (this.ContainerDirection == PaginatedScrollablePanel.ContainerDirections.Vertical && base.VerticalScrollbar != null)
			{
				float value2 = this._horizontalScrollbarInterpolationController.GetValue();
				float num3 = vector.Y * (float)this.ItemsPerPage;
				float num4 = Mathf.Abs(value2 - vector.Y * (float)this.ItemsPerPage * (float)this._pageIndex) / num3 * this.ScrollTime;
				this._verticalScrollbarInterpolationController.StartInterpolation(vector.Y * (float)this.ItemsPerPage * (float)this._pageIndex, num4);
			}
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00009F98 File Offset: 0x00008198
		private void UpdateChildrenNavigationStates()
		{
			if (base.ClipRect == null || this.ListWidget == null || this.ListWidget.ChildCount == 0)
			{
				return;
			}
			if (this._isInterpolating)
			{
				for (int i = 0; i < this.ListWidget.ChildCount; i++)
				{
					this.ListWidget.GetChild(i).DoNotAcceptNavigation = true;
				}
				return;
			}
			if (this._pageIndex == this._maxPages - 1)
			{
				for (int j = 0; j < this.ListWidget.ChildCount; j++)
				{
					Widget child = this.ListWidget.GetChild(j);
					if (j >= this.ListWidget.ChildCount - this.ItemsPerPage)
					{
						child.GamepadNavigationIndex = j - this.ListWidget.ChildCount + this.ItemsPerPage + 1;
						child.DoNotAcceptNavigation = false;
					}
					else
					{
						child.DoNotAcceptNavigation = true;
					}
				}
				return;
			}
			for (int k = 0; k < this.ListWidget.ChildCount; k++)
			{
				Widget child2 = this.ListWidget.GetChild(k);
				if (k >= this._pageIndex * this.ItemsPerPage && k < (this._pageIndex + 1) * this.ItemsPerPage)
				{
					child2.GamepadNavigationIndex = k - this._pageIndex * this.ItemsPerPage + 1;
					child2.DoNotAcceptNavigation = false;
				}
				else
				{
					child2.DoNotAcceptNavigation = true;
				}
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000A0DC File Offset: 0x000082DC
		private void ScrollToSelectedElement()
		{
			this.UpdatePageInfo();
			if (this._pageIndex != this._maxPages - 1 || this._selectedIndex < this.ListWidget.ChildCount - this.ItemsPerPage)
			{
				this._pageIndex = Mathf.Clamp(this._selectedIndex / this.ItemsPerPage, 0, this._maxPages - 1);
			}
			this.UpdateButtonEnabledStates();
			this.UpdateChildrenNavigationStates();
			this.UpdateViewport();
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000A14C File Offset: 0x0000834C
		private void OnSelectedWidgetUpdated()
		{
			this.ScrollToSelectedElement();
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000A154 File Offset: 0x00008354
		private void OnVisibilityUpdated()
		{
			if (this._isRecursivelyVisible && this.ScrollToSelectedOnVisibilityChanged)
			{
				this.ScrollToSelectedElement();
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000321 RID: 801 RVA: 0x0000A16C File Offset: 0x0000836C
		// (set) Token: 0x06000322 RID: 802 RVA: 0x0000A174 File Offset: 0x00008374
		[Editor(false)]
		public bool ScrollToSelectedOnVisibilityChanged
		{
			get
			{
				return this._scrollToSelectedOnVisibilityChanged;
			}
			set
			{
				if (value != this._scrollToSelectedOnVisibilityChanged)
				{
					this._scrollToSelectedOnVisibilityChanged = value;
					base.OnPropertyChanged(value, "ScrollToSelectedOnVisibilityChanged");
				}
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000323 RID: 803 RVA: 0x0000A192 File Offset: 0x00008392
		// (set) Token: 0x06000324 RID: 804 RVA: 0x0000A19A File Offset: 0x0000839A
		[Editor(false)]
		public int ItemsPerPage
		{
			get
			{
				return this._itemsPerPage;
			}
			set
			{
				if (value != this._itemsPerPage)
				{
					this._itemsPerPage = value;
					base.OnPropertyChanged(value, "ItemsPerPage");
				}
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0000A1B8 File Offset: 0x000083B8
		// (set) Token: 0x06000326 RID: 806 RVA: 0x0000A1C0 File Offset: 0x000083C0
		[Editor(false)]
		public float ScrollTime
		{
			get
			{
				return this._scrollTime;
			}
			set
			{
				if (value != this._scrollTime)
				{
					this._scrollTime = value;
					base.OnPropertyChanged(value, "ScrollTime");
				}
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000327 RID: 807 RVA: 0x0000A1DE File Offset: 0x000083DE
		// (set) Token: 0x06000328 RID: 808 RVA: 0x0000A1E6 File Offset: 0x000083E6
		[Editor(false)]
		public PaginatedScrollablePanel.ContainerDirections ContainerDirection
		{
			get
			{
				return this._containerDirection;
			}
			set
			{
				if (value != this._containerDirection)
				{
					this._containerDirection = value;
					base.OnPropertyChanged((int)value, "ContainerDirection");
					this.UpdatePageInfo();
					this.UpdateChildrenNavigationStates();
					this.UpdateViewport();
				}
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000329 RID: 809 RVA: 0x0000A216 File Offset: 0x00008416
		// (set) Token: 0x0600032A RID: 810 RVA: 0x0000A220 File Offset: 0x00008420
		[Editor(false)]
		public ListPanel ListWidget
		{
			get
			{
				return this._listWidget;
			}
			set
			{
				if (value != this._listWidget)
				{
					if (this._listWidget != null)
					{
						this._listWidget.EventFire -= this.ListWidget_EventFire;
					}
					this._listWidget = value;
					if (this._listWidget != null)
					{
						this._listWidget.EventFire += this.ListWidget_EventFire;
					}
					this.UpdatePageInfo();
					this.UpdateChildrenNavigationStates();
					this.UpdateViewport();
					base.OnPropertyChanged<ListPanel>(value, "ListWidget");
				}
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600032B RID: 811 RVA: 0x0000A299 File Offset: 0x00008499
		// (set) Token: 0x0600032C RID: 812 RVA: 0x0000A2A4 File Offset: 0x000084A4
		[Editor(false)]
		public ButtonWidget PreviousButtonWidget
		{
			get
			{
				return this._previousButtonWidget;
			}
			set
			{
				if (value != this._previousButtonWidget)
				{
					ButtonWidget previousButtonWidget = this._previousButtonWidget;
					if (previousButtonWidget != null)
					{
						previousButtonWidget.ClickEventHandlers.Remove(new Action<Widget>(this.OnPreviousButtonPressed));
					}
					this._previousButtonWidget = value;
					ButtonWidget previousButtonWidget2 = this._previousButtonWidget;
					if (previousButtonWidget2 != null)
					{
						previousButtonWidget2.ClickEventHandlers.Add(new Action<Widget>(this.OnPreviousButtonPressed));
					}
					base.OnPropertyChanged<ButtonWidget>(value, "PreviousButtonWidget");
				}
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600032D RID: 813 RVA: 0x0000A312 File Offset: 0x00008512
		// (set) Token: 0x0600032E RID: 814 RVA: 0x0000A31C File Offset: 0x0000851C
		[Editor(false)]
		public ButtonWidget NextButtonWidget
		{
			get
			{
				return this._nextButtonWidget;
			}
			set
			{
				if (value != this._nextButtonWidget)
				{
					ButtonWidget nextButtonWidget = this._nextButtonWidget;
					if (nextButtonWidget != null)
					{
						nextButtonWidget.ClickEventHandlers.Remove(new Action<Widget>(this.OnNextButtonPressed));
					}
					this._nextButtonWidget = value;
					ButtonWidget nextButtonWidget2 = this._nextButtonWidget;
					if (nextButtonWidget2 != null)
					{
						nextButtonWidget2.ClickEventHandlers.Add(new Action<Widget>(this.OnNextButtonPressed));
					}
					base.OnPropertyChanged<ButtonWidget>(value, "NextButtonWidget");
				}
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000A38A File Offset: 0x0000858A
		// (set) Token: 0x06000330 RID: 816 RVA: 0x0000A392 File Offset: 0x00008592
		[Editor(false)]
		public NavigationScopeTargeter NavigationScope
		{
			get
			{
				return this._navigationScope;
			}
			set
			{
				if (value != this._navigationScope)
				{
					this._navigationScope = value;
					base.OnPropertyChanged<NavigationScopeTargeter>(value, "NavigationScope");
				}
			}
		}

		// Token: 0x04000140 RID: 320
		private int _pageIndex;

		// Token: 0x04000141 RID: 321
		private int _maxPages;

		// Token: 0x04000142 RID: 322
		private int _selectedIndex;

		// Token: 0x04000143 RID: 323
		private bool _isRecursivelyVisible;

		// Token: 0x04000144 RID: 324
		private bool _isInterpolating;

		// Token: 0x04000145 RID: 325
		private bool _scrollToSelectedOnVisibilityChanged;

		// Token: 0x04000146 RID: 326
		private int _itemsPerPage;

		// Token: 0x04000147 RID: 327
		private float _scrollTime;

		// Token: 0x04000148 RID: 328
		private PaginatedScrollablePanel.ContainerDirections _containerDirection;

		// Token: 0x04000149 RID: 329
		private ListPanel _listWidget;

		// Token: 0x0400014A RID: 330
		private ButtonWidget _previousButtonWidget;

		// Token: 0x0400014B RID: 331
		private ButtonWidget _nextButtonWidget;

		// Token: 0x0400014C RID: 332
		private NavigationScopeTargeter _navigationScope;

		// Token: 0x0200019F RID: 415
		public enum ContainerDirections
		{
			// Token: 0x040009A7 RID: 2471
			Horizontal,
			// Token: 0x040009A8 RID: 2472
			Vertical
		}
	}
}
