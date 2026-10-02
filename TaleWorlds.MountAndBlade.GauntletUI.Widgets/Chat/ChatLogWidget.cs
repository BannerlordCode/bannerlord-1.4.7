using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Chat
{
	// Token: 0x0200017C RID: 380
	public class ChatLogWidget : Widget
	{
		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x060013B2 RID: 5042 RVA: 0x00035826 File Offset: 0x00033A26
		private float _resizeTransitionTime
		{
			get
			{
				return 0.14f;
			}
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x0003582D File Offset: 0x00033A2D
		public ChatLogWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x00035844 File Offset: 0x00033A44
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this.IsChatDisabled && this.TextInputWidget != null && this.FullyShowChatWithTyping && this._focusOnNextUpdate)
			{
				base.EventManager.FocusedWidget = this.TextInputWidget;
				this._focusOnNextUpdate = false;
			}
			if (!this.FullyShowChat)
			{
				this.ScrollablePanel.ResetTweenSpeed();
				this.Scrollbar.ValueFloat = this.Scrollbar.MaxValue;
			}
			base.ParentWidget.DoNotPassEventsToChildren = !this.FullyShowChat;
			if (this.ResizerWidget != null && this.ResizeFrameWidget != null)
			{
				this.UpdateResize(dt);
			}
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x000358E4 File Offset: 0x00033AE4
		private void UpdateResize(float dt)
		{
			if (Input.IsKeyPressed(InputKey.LeftMouseButton) && base.EventManager.HoveredWidget == this.ResizerWidget)
			{
				this._isResizing = true;
				this._resizeStartMousePosition = Input.MousePositionPixel;
				this._resizeOriginalSize = new Vec2(this.SizeX, this.SizeY);
				this.ResizeFrameWidget.IsVisible = true;
				this.ResizeFrameWidget.WidthSizePolicy = SizePolicy.Fixed;
				this.ResizeFrameWidget.HeightSizePolicy = SizePolicy.Fixed;
				this.ResizeFrameWidget.SuggestedHeight = this.SizeY;
				this.ResizeFrameWidget.SuggestedWidth = this.SizeX;
				this._innerPanelDefaultSizePolicies = new ValueTuple<SizePolicy, SizePolicy>(this.ScrollablePanel.InnerPanel.WidthSizePolicy, this.ScrollablePanel.InnerPanel.HeightSizePolicy);
				this.ScrollablePanel.InnerPanel.WidthSizePolicy = SizePolicy.Fixed;
				this.ScrollablePanel.InnerPanel.HeightSizePolicy = SizePolicy.Fixed;
				this.ScrollablePanel.InnerPanel.SuggestedWidth = this.ScrollablePanel.InnerPanel.Size.X;
				this.ScrollablePanel.InnerPanel.SuggestedHeight = this.ScrollablePanel.InnerPanel.Size.Y;
			}
			else if (Input.IsKeyReleased(InputKey.LeftMouseButton))
			{
				if (this._isResizing)
				{
					this.ResizeFrameWidget.IsVisible = false;
					this._resizeActualPanel = true;
					this._lerpRatio = 0f;
				}
				this._isResizing = false;
			}
			if (this._isResizing)
			{
				Vec2 vec = this._resizeOriginalSize + new Vec2((Input.MousePositionPixel - this._resizeStartMousePosition).X, -(Input.MousePositionPixel - this._resizeStartMousePosition).Y);
				this.ResizeFrameWidget.SuggestedWidth = Mathf.Clamp(vec.X, base.MinWidth, base.MaxWidth);
				this.ResizeFrameWidget.SuggestedHeight = Mathf.Clamp(vec.Y, base.MinHeight, base.MaxHeight) - this.ResizeFrameWidget.MarginBottom;
				return;
			}
			if (this._resizeActualPanel)
			{
				this._lerpRatio = Mathf.Clamp(this._lerpRatio + dt / this._resizeTransitionTime, 0f, 1f);
				this.SizeX = Mathf.Lerp(this._resizeOriginalSize.x, this.ResizeFrameWidget.SuggestedWidth, this._lerpRatio);
				this.SizeY = Mathf.Lerp(this._resizeOriginalSize.y, this.ResizeFrameWidget.SuggestedHeight + this.ResizeFrameWidget.MarginBottom, this._lerpRatio);
				if (this.SizeX.ApproximatelyEqualsTo(this.ResizeFrameWidget.SuggestedWidth, 0.01f) && this.SizeY.ApproximatelyEqualsTo(this.ResizeFrameWidget.SuggestedHeight + this.ResizeFrameWidget.MarginBottom, 0.01f))
				{
					this.SizeX = this.ResizeFrameWidget.SuggestedWidth;
					this.SizeY = this.ResizeFrameWidget.SuggestedHeight + this.ResizeFrameWidget.MarginBottom;
					this.ResizeFrameWidget.WidthSizePolicy = SizePolicy.StretchToParent;
					this.ResizeFrameWidget.HeightSizePolicy = SizePolicy.StretchToParent;
					this.ScrollablePanel.InnerPanel.WidthSizePolicy = this._innerPanelDefaultSizePolicies.Item1;
					this.ScrollablePanel.InnerPanel.HeightSizePolicy = this._innerPanelDefaultSizePolicies.Item2;
					this._resizeActualPanel = false;
					base.EventFired("FinishResize", Array.Empty<object>());
					return;
				}
			}
			else if (!this._isInitialized)
			{
				this.SizeX = base.SuggestedWidth;
				this.SizeY = base.SuggestedHeight;
				this._isInitialized = true;
			}
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x00035C87 File Offset: 0x00033E87
		public void RegisterMultiLineElement(ChatCollapsableListPanel element)
		{
			if (!this._registeredMultilineWidgets.Contains(element))
			{
				this._registeredMultilineWidgets.Add(element);
			}
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x00035CA3 File Offset: 0x00033EA3
		public void RemoveMultiLineElement(ChatCollapsableListPanel element)
		{
			if (this._registeredMultilineWidgets.Contains(element))
			{
				this._registeredMultilineWidgets.Remove(element);
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x060013B8 RID: 5048 RVA: 0x00035CC0 File Offset: 0x00033EC0
		// (set) Token: 0x060013B9 RID: 5049 RVA: 0x00035CC8 File Offset: 0x00033EC8
		[DataSourceProperty]
		public bool IsChatDisabled
		{
			get
			{
				return this._isChatDisabled;
			}
			set
			{
				if (value != this._isChatDisabled)
				{
					this._isChatDisabled = value;
					base.OnPropertyChanged(value, "IsChatDisabled");
				}
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x060013BA RID: 5050 RVA: 0x00035CE6 File Offset: 0x00033EE6
		// (set) Token: 0x060013BB RID: 5051 RVA: 0x00035CEE File Offset: 0x00033EEE
		[DataSourceProperty]
		public bool FinishedResizing
		{
			get
			{
				return this._finishedResizing;
			}
			set
			{
				if (value != this._finishedResizing)
				{
					this._finishedResizing = value;
					base.OnPropertyChanged(value, "FinishedResizing");
				}
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x060013BC RID: 5052 RVA: 0x00035D0C File Offset: 0x00033F0C
		// (set) Token: 0x060013BD RID: 5053 RVA: 0x00035D14 File Offset: 0x00033F14
		[DataSourceProperty]
		public bool FullyShowChat
		{
			get
			{
				return this._fullyShowChat;
			}
			set
			{
				if (value != this._fullyShowChat)
				{
					this._fullyShowChat = value;
				}
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x060013BE RID: 5054 RVA: 0x00035D26 File Offset: 0x00033F26
		// (set) Token: 0x060013BF RID: 5055 RVA: 0x00035D30 File Offset: 0x00033F30
		[DataSourceProperty]
		public bool FullyShowChatWithTyping
		{
			get
			{
				return this._fullyShowChatWithTyping;
			}
			set
			{
				if (value != this._fullyShowChatWithTyping)
				{
					this._fullyShowChatWithTyping = value;
					if (!this.IsChatDisabled && this.TextInputWidget != null && this._fullyShowChatWithTyping)
					{
						this._focusOnNextUpdate = true;
					}
					base.EventManager.FocusedWidget = null;
					base.OnPropertyChanged(value, "FullyShowChatWithTyping");
				}
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x060013C0 RID: 5056 RVA: 0x00035D84 File Offset: 0x00033F84
		// (set) Token: 0x060013C1 RID: 5057 RVA: 0x00035D8C File Offset: 0x00033F8C
		[DataSourceProperty]
		public EditableTextWidget TextInputWidget
		{
			get
			{
				return this._textInputWidget;
			}
			set
			{
				if (value != this._textInputWidget)
				{
					this._textInputWidget = value;
					base.OnPropertyChanged<EditableTextWidget>(value, "TextInputWidget");
				}
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x060013C2 RID: 5058 RVA: 0x00035DAA File Offset: 0x00033FAA
		// (set) Token: 0x060013C3 RID: 5059 RVA: 0x00035DB2 File Offset: 0x00033FB2
		[DataSourceProperty]
		public ScrollbarWidget Scrollbar
		{
			get
			{
				return this._scrollbar;
			}
			set
			{
				if (value != this._scrollbar)
				{
					this._scrollbar = value;
					base.OnPropertyChanged<ScrollbarWidget>(value, "Scrollbar");
				}
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x060013C4 RID: 5060 RVA: 0x00035DD0 File Offset: 0x00033FD0
		// (set) Token: 0x060013C5 RID: 5061 RVA: 0x00035DD8 File Offset: 0x00033FD8
		[DataSourceProperty]
		public ScrollablePanel ScrollablePanel
		{
			get
			{
				return this._scrollablePanel;
			}
			set
			{
				if (value != this._scrollablePanel)
				{
					this._scrollablePanel = value;
					base.OnPropertyChanged<ScrollablePanel>(value, "ScrollablePanel");
				}
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x060013C6 RID: 5062 RVA: 0x00035DF6 File Offset: 0x00033FF6
		// (set) Token: 0x060013C7 RID: 5063 RVA: 0x00035DFE File Offset: 0x00033FFE
		[DataSourceProperty]
		public Widget ResizerWidget
		{
			get
			{
				return this._resizerWidget;
			}
			set
			{
				if (value != this._resizerWidget)
				{
					this._resizerWidget = value;
					base.OnPropertyChanged<Widget>(value, "ResizerWidget");
				}
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x060013C8 RID: 5064 RVA: 0x00035E1C File Offset: 0x0003401C
		// (set) Token: 0x060013C9 RID: 5065 RVA: 0x00035E24 File Offset: 0x00034024
		[DataSourceProperty]
		public Widget ResizeFrameWidget
		{
			get
			{
				return this._resizeFrameWidget;
			}
			set
			{
				if (value != this._resizeFrameWidget)
				{
					this._resizeFrameWidget = value;
					base.OnPropertyChanged<Widget>(value, "ResizeFrameWidget");
				}
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x060013CA RID: 5066 RVA: 0x00035E42 File Offset: 0x00034042
		// (set) Token: 0x060013CB RID: 5067 RVA: 0x00035E4A File Offset: 0x0003404A
		[DataSourceProperty]
		public float SizeX
		{
			get
			{
				return this._sizeX;
			}
			set
			{
				if (value != this._sizeX)
				{
					this._sizeX = value;
					base.SuggestedWidth = value;
					base.OnPropertyChanged(value, "SizeX");
				}
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x060013CC RID: 5068 RVA: 0x00035E6F File Offset: 0x0003406F
		// (set) Token: 0x060013CD RID: 5069 RVA: 0x00035E77 File Offset: 0x00034077
		[DataSourceProperty]
		public float SizeY
		{
			get
			{
				return this._sizeY;
			}
			set
			{
				if (value != this._sizeY)
				{
					this._sizeY = value;
					base.SuggestedHeight = value;
					base.OnPropertyChanged(value, "SizeY");
				}
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x060013CE RID: 5070 RVA: 0x00035E9C File Offset: 0x0003409C
		// (set) Token: 0x060013CF RID: 5071 RVA: 0x00035EA4 File Offset: 0x000340A4
		[DataSourceProperty]
		public ListPanel MessageHistoryList
		{
			get
			{
				return this._messageHistoryList;
			}
			set
			{
				if (value != this._messageHistoryList)
				{
					this._messageHistoryList = value;
					base.OnPropertyChanged<ListPanel>(value, "MessageHistoryList");
				}
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x060013D0 RID: 5072 RVA: 0x00035EC2 File Offset: 0x000340C2
		// (set) Token: 0x060013D1 RID: 5073 RVA: 0x00035ECA File Offset: 0x000340CA
		[DataSourceProperty]
		public bool IsMPChatLog
		{
			get
			{
				return this._isMPChatLog;
			}
			set
			{
				if (value != this._isMPChatLog)
				{
					this._isMPChatLog = value;
					base.OnPropertyChanged(value, "IsMPChatLog");
				}
			}
		}

		// Token: 0x040008E7 RID: 2279
		private List<ChatCollapsableListPanel> _registeredMultilineWidgets = new List<ChatCollapsableListPanel>();

		// Token: 0x040008E8 RID: 2280
		private bool _isInitialized;

		// Token: 0x040008E9 RID: 2281
		private float _lerpRatio;

		// Token: 0x040008EA RID: 2282
		private bool _isResizing;

		// Token: 0x040008EB RID: 2283
		private bool _resizeActualPanel;

		// Token: 0x040008EC RID: 2284
		private Vec2 _resizeStartMousePosition;

		// Token: 0x040008ED RID: 2285
		private Vec2 _resizeOriginalSize;

		// Token: 0x040008EE RID: 2286
		private ValueTuple<SizePolicy, SizePolicy> _innerPanelDefaultSizePolicies;

		// Token: 0x040008EF RID: 2287
		private bool _focusOnNextUpdate;

		// Token: 0x040008F0 RID: 2288
		private bool _isChatDisabled;

		// Token: 0x040008F1 RID: 2289
		private bool _isMPChatLog;

		// Token: 0x040008F2 RID: 2290
		private bool _finishedResizing;

		// Token: 0x040008F3 RID: 2291
		private bool _fullyShowChat;

		// Token: 0x040008F4 RID: 2292
		private bool _fullyShowChatWithTyping;

		// Token: 0x040008F5 RID: 2293
		private EditableTextWidget _textInputWidget;

		// Token: 0x040008F6 RID: 2294
		private ScrollbarWidget _scrollbar;

		// Token: 0x040008F7 RID: 2295
		private ScrollablePanel _scrollablePanel;

		// Token: 0x040008F8 RID: 2296
		private Widget _resizerWidget;

		// Token: 0x040008F9 RID: 2297
		private Widget _resizeFrameWidget;

		// Token: 0x040008FA RID: 2298
		private float _sizeX;

		// Token: 0x040008FB RID: 2299
		private float _sizeY;

		// Token: 0x040008FC RID: 2300
		private ListPanel _messageHistoryList;
	}
}
