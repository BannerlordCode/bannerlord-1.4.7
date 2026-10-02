using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000017 RID: 23
	public class DiscreteScrollablePanel : ScrollablePanel
	{
		// Token: 0x0600012F RID: 303 RVA: 0x00005299 File Offset: 0x00003499
		public DiscreteScrollablePanel(UIContext context)
			: base(context)
		{
			this.ItemsPerPage = 6;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000052AC File Offset: 0x000034AC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
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
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005357 File Offset: 0x00003557
		private void UpdateAll()
		{
			this.UpdatePageInfo();
			this.UpdateButtonEnabledStates();
			this.UpdateViewport();
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000536C File Offset: 0x0000356C
		private void UpdatePageInfo()
		{
			if (this.ListWidget == null || base.ClipRect == null)
			{
				this._pageIndex = 0;
				this._maxPages = 0;
				return;
			}
			this._maxPages = MathF.Max(0, this.ListWidget.ChildCount - this.ItemsPerPage) + 1;
			this._pageIndex = (int)MathF.Clamp((float)this._pageIndex, 0f, (float)(this._maxPages - 1));
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000053DC File Offset: 0x000035DC
		private void UpdateButtonEnabledStates()
		{
			if (this._maxPages == 1)
			{
				if (this.PreviousButtonWidget != null)
				{
					this.PreviousButtonWidget.IsEnabled = false;
				}
				if (this.NextButtonWidget != null)
				{
					this.NextButtonWidget.IsEnabled = false;
					return;
				}
			}
			else if (this.IsLooping)
			{
				if (this.PreviousButtonWidget != null)
				{
					this.PreviousButtonWidget.IsEnabled = true;
				}
				if (this.NextButtonWidget != null)
				{
					this.NextButtonWidget.IsEnabled = true;
					return;
				}
			}
			else
			{
				if (this.PreviousButtonWidget != null)
				{
					this.PreviousButtonWidget.IsEnabled = this._pageIndex > 0;
				}
				if (this.NextButtonWidget != null)
				{
					this.NextButtonWidget.IsEnabled = this._pageIndex < this._maxPages - 1;
				}
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000548C File Offset: 0x0000368C
		private void UpdateViewport()
		{
			if (base.ClipRect == null || this.ListWidget == null || this.ListWidget.ChildCount == 0)
			{
				return;
			}
			Vec2 vec = Vec2.Zero;
			for (int i = 0; i < this.ListWidget.ChildCount; i++)
			{
				Widget child = this.ListWidget.GetChild(i);
				vec += child.Size + new Vec2(child.ScaledMarginLeft + child.ScaledMarginRight, child.ScaledMarginBottom + child.ScaledMarginTop);
			}
			vec /= (float)this.ListWidget.ChildCount;
			if (this.ListWidget.GetChild(this._pageIndex) != null)
			{
				if (base.HorizontalScrollbar != null)
				{
					ScrollablePanel.ScrollbarInterpolationController horizontalScrollbarInterpolationController = this._horizontalScrollbarInterpolationController;
					if (horizontalScrollbarInterpolationController != null)
					{
						horizontalScrollbarInterpolationController.StartInterpolation(vec.X * (float)this._pageIndex, this.ScrollTime);
					}
				}
				if (base.VerticalScrollbar != null)
				{
					ScrollablePanel.ScrollbarInterpolationController verticalScrollbarInterpolationController = this._verticalScrollbarInterpolationController;
					if (verticalScrollbarInterpolationController == null)
					{
						return;
					}
					verticalScrollbarInterpolationController.StartInterpolation(vec.Y * (float)this._pageIndex, this.ScrollTime);
				}
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005597 File Offset: 0x00003797
		private void ListWidget_EventFire(Widget widget, string eventName, object[] eventArgs)
		{
			if (eventName == "ItemAdd" || eventName == "ItemRemove" || eventName == "AfterItemRemove")
			{
				this.UpdateAll();
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000055C6 File Offset: 0x000037C6
		private void OnPreviousButtonPressed(Widget widget)
		{
			this._pageIndex--;
			if (this.IsLooping && this._pageIndex < 0)
			{
				this._pageIndex = this.ListWidget.ChildCount - 1;
			}
			this.UpdateAll();
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005600 File Offset: 0x00003800
		private void OnNextButtonPressed(Widget widget)
		{
			this._pageIndex++;
			if (this.IsLooping && this._pageIndex >= this._maxPages)
			{
				this._pageIndex = 0;
			}
			this.UpdateAll();
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005634 File Offset: 0x00003834
		private void ScrollToSelectedElement()
		{
			this._pageIndex = MathF.Min(this._pageIndex, this._selectedIndex - 1);
			this._pageIndex = MathF.Max(this._pageIndex, this._selectedIndex - this.ItemsPerPage + 2);
			this.UpdateAll();
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00005680 File Offset: 0x00003880
		private void OnSelectedWidgetUpdated()
		{
			this.ScrollToSelectedElement();
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00005688 File Offset: 0x00003888
		private void OnVisibilityUpdated()
		{
			if (this._isRecursivelyVisible && this.ScrollToSelectedOnVisibilityChanged)
			{
				this.ScrollToSelectedElement();
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600013B RID: 315 RVA: 0x000056A0 File Offset: 0x000038A0
		// (set) Token: 0x0600013C RID: 316 RVA: 0x000056A8 File Offset: 0x000038A8
		[Editor(false)]
		public bool IsLooping
		{
			get
			{
				return this._isLooping;
			}
			set
			{
				if (value != this._isLooping)
				{
					this._isLooping = value;
					base.OnPropertyChanged(value, "IsLooping");
					this.UpdateAll();
				}
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600013D RID: 317 RVA: 0x000056CC File Offset: 0x000038CC
		// (set) Token: 0x0600013E RID: 318 RVA: 0x000056D4 File Offset: 0x000038D4
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

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600013F RID: 319 RVA: 0x000056F2 File Offset: 0x000038F2
		// (set) Token: 0x06000140 RID: 320 RVA: 0x000056FA File Offset: 0x000038FA
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
					this.UpdateAll();
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000141 RID: 321 RVA: 0x0000571E File Offset: 0x0000391E
		// (set) Token: 0x06000142 RID: 322 RVA: 0x00005726 File Offset: 0x00003926
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

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00005744 File Offset: 0x00003944
		// (set) Token: 0x06000144 RID: 324 RVA: 0x0000574C File Offset: 0x0000394C
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
					base.OnPropertyChanged<ListPanel>(value, "ListWidget");
					if (this._listWidget != null)
					{
						this._listWidget.EventFire += this.ListWidget_EventFire;
					}
					this.UpdateAll();
				}
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000145 RID: 325 RVA: 0x000057B9 File Offset: 0x000039B9
		// (set) Token: 0x06000146 RID: 326 RVA: 0x000057C4 File Offset: 0x000039C4
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

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00005832 File Offset: 0x00003A32
		// (set) Token: 0x06000148 RID: 328 RVA: 0x0000583C File Offset: 0x00003A3C
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

		// Token: 0x04000092 RID: 146
		private int _pageIndex;

		// Token: 0x04000093 RID: 147
		private int _maxPages;

		// Token: 0x04000094 RID: 148
		private int _selectedIndex;

		// Token: 0x04000095 RID: 149
		private bool _isRecursivelyVisible;

		// Token: 0x04000096 RID: 150
		private bool _isLooping;

		// Token: 0x04000097 RID: 151
		private bool _scrollToSelectedOnVisibilityChanged;

		// Token: 0x04000098 RID: 152
		private int _itemsPerPage;

		// Token: 0x04000099 RID: 153
		private float _scrollTime;

		// Token: 0x0400009A RID: 154
		private ListPanel _listWidget;

		// Token: 0x0400009B RID: 155
		private ButtonWidget _previousButtonWidget;

		// Token: 0x0400009C RID: 156
		private ButtonWidget _nextButtonWidget;
	}
}
