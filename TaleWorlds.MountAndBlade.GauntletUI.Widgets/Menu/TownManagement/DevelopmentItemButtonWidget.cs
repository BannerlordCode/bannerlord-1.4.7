using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.TownManagement
{
	// Token: 0x02000108 RID: 264
	public class DevelopmentItemButtonWidget : ButtonWidget
	{
		// Token: 0x06000E00 RID: 3584 RVA: 0x000265B9 File Offset: 0x000247B9
		public DevelopmentItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x000265C4 File Offset: 0x000247C4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			ButtonWidget buttonWidget;
			if (!this._isParentInitialized && (buttonWidget = base.ParentWidget as ButtonWidget) != null)
			{
				buttonWidget.ClickEventHandlers.Add(new Action<Widget>(this.OnParentClick));
				this._isParentInitialized = true;
			}
			if (!this.IsDaily)
			{
				this.HandleFocus();
				this.HandleEnabledStates();
				this.DevelopmentFrontVisualWidget.HeightSizePolicy = SizePolicy.Fixed;
				this.DevelopmentFrontVisualWidget.WidthSizePolicy = SizePolicy.Fixed;
				this.DevelopmentFrontVisualWidget.ScaledSuggestedHeight = this.DevelopmentBackVisualWidget.Size.Y;
				this.DevelopmentFrontVisualWidget.ScaledSuggestedWidth = this.DevelopmentBackVisualWidget.Size.X;
				if (this.IsProgressShown)
				{
					if (this.Progress > 0 || this.Level == 0)
					{
						this.ProgressClipWidget.HeightSizePolicy = SizePolicy.Fixed;
						this.ProgressClipWidget.ScaledSuggestedHeight = this.DevelopmentBackVisualWidget.Size.Y * ((float)this.Progress / 100f);
					}
					if (this.Level == 0)
					{
						this.DevelopmentBackVisualWidget.AlphaFactor = 0.8f;
						this.DevelopmentBackVisualWidget.SaturationFactor = -80f;
					}
					else
					{
						this.DevelopmentBackVisualWidget.AlphaFactor = 0.2f;
					}
				}
				else
				{
					this.ProgressClipWidget.HeightSizePolicy = SizePolicy.StretchToParent;
				}
				this.HandleChildrenVisibilities();
			}
			this.DevelopmentBackVisualWidget.CircularClipEnabled = true;
			this.DevelopmentBackVisualWidget.CircularClipRadius = this.DevelopmentBackVisualWidget.Size.X / 2f * base._inverseScaleToUse - 10f * base._scaleToUse;
			this.DevelopmentBackVisualWidget.CircularClipSmoothingRadius = 3f;
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x00026764 File Offset: 0x00024964
		private void HandleFocus()
		{
			if (this.IsSelectedItem)
			{
				if (base.EventManager.LatestMouseUpWidget != null && base.EventManager.LatestMouseUpWidget != base.ParentWidget)
				{
					DevelopmentItemVisualButtonWidget developmentItemVisualButtonWidget;
					if ((developmentItemVisualButtonWidget = base.EventManager.LatestMouseUpWidget as DevelopmentItemVisualButtonWidget) != null)
					{
						string spriteCode = developmentItemVisualButtonWidget.SpriteCode;
						DevelopmentItemVisualButtonWidget developmentItemVisualButtonWidget2 = this.DevelopmentBackVisualWidget as DevelopmentItemVisualButtonWidget;
						if (spriteCode == ((developmentItemVisualButtonWidget2 != null) ? developmentItemVisualButtonWidget2.SpriteCode : null))
						{
							goto IL_0067;
						}
					}
					this.IsSelectedItem = false;
				}
				IL_0067:
				if (base.EventManager.DraggedWidget != null)
				{
					this.IsSelectedItem = false;
				}
			}
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x000267EC File Offset: 0x000249EC
		private void HandleChildrenVisibilities()
		{
			this.SetAsActiveButtonWidget.IsVisible = this.IsSelectedItem;
			this.AddToQueueButtonWidget.IsVisible = this.IsSelectedItem;
			this.SelectedBlackOverlayWidget.IsVisible = this.IsSelectedItem;
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x00026821 File Offset: 0x00024A21
		private void HandleEnabledStates()
		{
			base.ParentWidget.DoNotPassEventsToChildren = !this.IsSelectedItem;
			base.DoNotPassEventsToChildren = !this.IsSelectedItem;
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x00026846 File Offset: 0x00024A46
		private void OnParentClick(Widget widget)
		{
			if (!this.IsSelectedItem && this.CanBuild)
			{
				this.IsSelectedItem = true;
			}
			if (!this.CanBuild)
			{
				DevelopmentNameTextWidget nameTextWidget = this.NameTextWidget;
				if (nameTextWidget == null)
				{
					return;
				}
				nameTextWidget.StartMaxTextAnimation();
			}
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x00026877 File Offset: 0x00024A77
		private void OnAddToQueueClick(Widget widget)
		{
			this.IsSelectedItem = false;
			base.EventFired("OnAddToQueue", Array.Empty<object>());
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x00026890 File Offset: 0x00024A90
		private void OnSetAsActiveDevelopmentClick(Widget widget)
		{
			this.IsSelectedItem = false;
			base.EventFired("SetAsActive", Array.Empty<object>());
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x000268A9 File Offset: 0x00024AA9
		private void UpdateDevelopmentLevelVisual(int level)
		{
			if (!this.IsDaily)
			{
				this.DevelopmentLevelVisualWidget.SetState(level.ToString());
				this.DevelopmentLevelVisualWidget.IsVisible = level >= 0;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06000E09 RID: 3593 RVA: 0x000268D7 File Offset: 0x00024AD7
		// (set) Token: 0x06000E0A RID: 3594 RVA: 0x000268DF File Offset: 0x00024ADF
		[Editor(false)]
		public bool IsSelectedItem
		{
			get
			{
				return this._isSelectedItem;
			}
			set
			{
				if (this._isSelectedItem != value)
				{
					this._isSelectedItem = value;
					base.OnPropertyChanged(value, "IsSelectedItem");
				}
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06000E0B RID: 3595 RVA: 0x000268FD File Offset: 0x00024AFD
		// (set) Token: 0x06000E0C RID: 3596 RVA: 0x00026905 File Offset: 0x00024B05
		[Editor(false)]
		public Widget SelectedBlackOverlayWidget
		{
			get
			{
				return this._selectedBlackOverlayWidget;
			}
			set
			{
				if (this._selectedBlackOverlayWidget != value)
				{
					this._selectedBlackOverlayWidget = value;
					base.OnPropertyChanged<Widget>(value, "SelectedBlackOverlayWidget");
				}
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06000E0D RID: 3597 RVA: 0x00026923 File Offset: 0x00024B23
		// (set) Token: 0x06000E0E RID: 3598 RVA: 0x0002692B File Offset: 0x00024B2B
		[Editor(false)]
		public DevelopmentNameTextWidget NameTextWidget
		{
			get
			{
				return this._nameTextWidget;
			}
			set
			{
				if (this._nameTextWidget != value)
				{
					this._nameTextWidget = value;
					base.OnPropertyChanged<DevelopmentNameTextWidget>(value, "NameTextWidget");
				}
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06000E0F RID: 3599 RVA: 0x00026949 File Offset: 0x00024B49
		// (set) Token: 0x06000E10 RID: 3600 RVA: 0x00026951 File Offset: 0x00024B51
		[Editor(false)]
		public ButtonWidget AddToQueueButtonWidget
		{
			get
			{
				return this._addToQueueButtonWidget;
			}
			set
			{
				if (this._addToQueueButtonWidget != value)
				{
					this._addToQueueButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "AddToQueueButtonWidget");
					value.ClickEventHandlers.Add(new Action<Widget>(this.OnAddToQueueClick));
				}
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06000E11 RID: 3601 RVA: 0x00026986 File Offset: 0x00024B86
		// (set) Token: 0x06000E12 RID: 3602 RVA: 0x0002698E File Offset: 0x00024B8E
		[Editor(false)]
		public ButtonWidget SetAsActiveButtonWidget
		{
			get
			{
				return this._setAsActiveButtonWidget;
			}
			set
			{
				if (this._setAsActiveButtonWidget != value)
				{
					this._setAsActiveButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "SetAsActiveButtonWidget");
					value.ClickEventHandlers.Add(new Action<Widget>(this.OnSetAsActiveDevelopmentClick));
				}
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06000E13 RID: 3603 RVA: 0x000269C3 File Offset: 0x00024BC3
		// (set) Token: 0x06000E14 RID: 3604 RVA: 0x000269CB File Offset: 0x00024BCB
		[Editor(false)]
		public Widget DevelopmentLevelVisualWidget
		{
			get
			{
				return this._developmentLevelVisualWidget;
			}
			set
			{
				if (this._developmentLevelVisualWidget != value)
				{
					this._developmentLevelVisualWidget = value;
					base.OnPropertyChanged<Widget>(value, "DevelopmentLevelVisualWidget");
					this.UpdateDevelopmentLevelVisual(this.Level);
				}
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06000E15 RID: 3605 RVA: 0x000269F5 File Offset: 0x00024BF5
		// (set) Token: 0x06000E16 RID: 3606 RVA: 0x000269FD File Offset: 0x00024BFD
		[Editor(false)]
		public Widget ProgressClipWidget
		{
			get
			{
				return this._progressClipWidget;
			}
			set
			{
				if (this._progressClipWidget != value)
				{
					this._progressClipWidget = value;
					base.OnPropertyChanged<Widget>(value, "ProgressClipWidget");
				}
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06000E17 RID: 3607 RVA: 0x00026A1B File Offset: 0x00024C1B
		// (set) Token: 0x06000E18 RID: 3608 RVA: 0x00026A23 File Offset: 0x00024C23
		[Editor(false)]
		public bool IsProgressShown
		{
			get
			{
				return this._isProgressShown;
			}
			set
			{
				if (this._isProgressShown != value)
				{
					this._isProgressShown = value;
					base.OnPropertyChanged(value, "IsProgressShown");
				}
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06000E19 RID: 3609 RVA: 0x00026A41 File Offset: 0x00024C41
		// (set) Token: 0x06000E1A RID: 3610 RVA: 0x00026A49 File Offset: 0x00024C49
		[Editor(false)]
		public bool CanBuild
		{
			get
			{
				return this._canBuild;
			}
			set
			{
				if (this._canBuild != value)
				{
					this._canBuild = value;
					base.OnPropertyChanged(value, "CanBuild");
				}
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06000E1B RID: 3611 RVA: 0x00026A67 File Offset: 0x00024C67
		// (set) Token: 0x06000E1C RID: 3612 RVA: 0x00026A6F File Offset: 0x00024C6F
		[Editor(false)]
		public Widget DevelopmentBackVisualWidget
		{
			get
			{
				return this._developmentBackVisualWidget;
			}
			set
			{
				if (this._developmentBackVisualWidget != value)
				{
					this._developmentBackVisualWidget = value;
					base.OnPropertyChanged<Widget>(value, "DevelopmentBackVisualWidget");
				}
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06000E1D RID: 3613 RVA: 0x00026A8D File Offset: 0x00024C8D
		// (set) Token: 0x06000E1E RID: 3614 RVA: 0x00026A95 File Offset: 0x00024C95
		[Editor(false)]
		public Widget DevelopmentFrontVisualWidget
		{
			get
			{
				return this._developmentFrontVisualWidget;
			}
			set
			{
				if (this._developmentFrontVisualWidget != value)
				{
					this._developmentFrontVisualWidget = value;
					base.OnPropertyChanged<Widget>(value, "DevelopmentFrontVisualWidget");
				}
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06000E1F RID: 3615 RVA: 0x00026AB3 File Offset: 0x00024CB3
		// (set) Token: 0x06000E20 RID: 3616 RVA: 0x00026ABB File Offset: 0x00024CBB
		[Editor(false)]
		public bool IsProgressIndicatorsEnabled
		{
			get
			{
				return this._isProgressIndicatorsEnabled;
			}
			set
			{
				if (this._isProgressIndicatorsEnabled != value)
				{
					this._isProgressIndicatorsEnabled = value;
					base.OnPropertyChanged(value, "IsProgressIndicatorsEnabled");
				}
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06000E21 RID: 3617 RVA: 0x00026AD9 File Offset: 0x00024CD9
		// (set) Token: 0x06000E22 RID: 3618 RVA: 0x00026AE1 File Offset: 0x00024CE1
		[Editor(false)]
		public bool IsDaily
		{
			get
			{
				return this._isDaily;
			}
			set
			{
				if (this._isDaily != value)
				{
					this._isDaily = value;
					base.OnPropertyChanged(value, "IsDaily");
				}
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06000E23 RID: 3619 RVA: 0x00026AFF File Offset: 0x00024CFF
		// (set) Token: 0x06000E24 RID: 3620 RVA: 0x00026B07 File Offset: 0x00024D07
		[Editor(false)]
		public int Level
		{
			get
			{
				return this._level;
			}
			set
			{
				if (this._level != value)
				{
					this._level = value;
					base.OnPropertyChanged(value, "Level");
					this.UpdateDevelopmentLevelVisual(value);
				}
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06000E25 RID: 3621 RVA: 0x00026B2C File Offset: 0x00024D2C
		// (set) Token: 0x06000E26 RID: 3622 RVA: 0x00026B34 File Offset: 0x00024D34
		[Editor(false)]
		public int Progress
		{
			get
			{
				return this._progress;
			}
			set
			{
				if (this._progress != value)
				{
					this._progress = value;
					base.OnPropertyChanged(value, "Progress");
				}
			}
		}

		// Token: 0x04000659 RID: 1625
		private bool _isParentInitialized;

		// Token: 0x0400065A RID: 1626
		private bool _isSelectedItem;

		// Token: 0x0400065B RID: 1627
		private int _level;

		// Token: 0x0400065C RID: 1628
		private int _progress;

		// Token: 0x0400065D RID: 1629
		private bool _isDaily;

		// Token: 0x0400065E RID: 1630
		private bool _isProgressIndicatorsEnabled;

		// Token: 0x0400065F RID: 1631
		private Widget _developmentLevelVisualWidget;

		// Token: 0x04000660 RID: 1632
		private Widget _developmentBackVisualWidget;

		// Token: 0x04000661 RID: 1633
		private Widget _developmentFrontVisualWidget;

		// Token: 0x04000662 RID: 1634
		private Widget _selectedBlackOverlayWidget;

		// Token: 0x04000663 RID: 1635
		private ButtonWidget _addToQueueButtonWidget;

		// Token: 0x04000664 RID: 1636
		private ButtonWidget _setAsActiveButtonWidget;

		// Token: 0x04000665 RID: 1637
		private DevelopmentNameTextWidget _nameTextWidget;

		// Token: 0x04000666 RID: 1638
		private Widget _progressClipWidget;

		// Token: 0x04000667 RID: 1639
		private bool _isProgressShown;

		// Token: 0x04000668 RID: 1640
		private bool _canBuild;
	}
}
