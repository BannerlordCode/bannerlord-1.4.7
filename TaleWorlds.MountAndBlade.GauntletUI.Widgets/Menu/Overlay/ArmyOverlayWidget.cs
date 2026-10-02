using System;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.Overlay
{
	// Token: 0x0200010F RID: 271
	public class ArmyOverlayWidget : OverlayBaseWidget
	{
		// Token: 0x06000E63 RID: 3683 RVA: 0x00027A6F File Offset: 0x00025C6F
		public ArmyOverlayWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x00027A80 File Offset: 0x00025C80
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			int num = this._armyListGridWidget.Children.Count<Widget>((Widget c) => c.IsVisible);
			if (num != this._armyItemCount)
			{
				this.Overlay.SetState("Reset");
				this._armyItemCount = num;
			}
			this.RefreshOverlayExtendState(!this._initialized);
			this.UpdateExtendButtonVisual();
			this._initialized = true;
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x00027B00 File Offset: 0x00025D00
		private void RefreshOverlayExtendState(bool forceSetPosition)
		{
			string text = (this._isInfoBarExtended ? "MapExtended" : "MapNormal");
			string text2 = (this._isOverlayExtended ? "OverlayExtended" : "OverlayNormal");
			if (text + text2 != this.Overlay.CurrentState)
			{
				if (!this._isOverlayExtended)
				{
					if (forceSetPosition)
					{
						VisualState visualState;
						this.Overlay.VisualDefinition.VisualStates.TryGetValue(text + text2, out visualState);
						this.Overlay.PositionYOffset = visualState.PositionYOffset;
					}
				}
				else
				{
					float y = this.ArmyListGridWidget.Size.Y;
					float num;
					if (this._isInfoBarExtended)
					{
						VisualState visualState2;
						this.Overlay.VisualDefinition.VisualStates.TryGetValue("MapExtendedOverlayNormal", out visualState2);
						num = visualState2.PositionYOffset - y * base._inverseScaleToUse;
					}
					else
					{
						VisualState visualState3;
						this.Overlay.VisualDefinition.VisualStates.TryGetValue("MapNormalOverlayNormal", out visualState3);
						num = visualState3.PositionYOffset - y * base._inverseScaleToUse;
					}
					if (forceSetPosition)
					{
						this.Overlay.PositionYOffset = num;
					}
					VisualState visualState4;
					this.Overlay.VisualDefinition.VisualStates.TryGetValue(text + text2, out visualState4);
					visualState4.PositionYOffset = num;
				}
				this.Overlay.SetState(text + text2);
			}
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00027C5C File Offset: 0x00025E5C
		private void UpdateExtendButtonVisual()
		{
			foreach (Style style in this.ExtendButton.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].VerticalFlip = this._isOverlayExtended;
				}
			}
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x00027CD4 File Offset: 0x00025ED4
		private void OnExtendButtonClick(Widget button)
		{
			this._isOverlayExtended = !this._isOverlayExtended;
			this.UpdateExtendButtonVisual();
			this.RefreshOverlayExtendState(false);
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x00027CF4 File Offset: 0x00025EF4
		private void OnArmyListPageCountChanged()
		{
			if (this.PageControlWidget.PageCount == 1)
			{
				this.Overlay.PositionXOffset = 40f;
				this.ExtendButton.PositionXOffset = -40f;
				return;
			}
			this.Overlay.PositionXOffset = 0f;
			this.ExtendButton.PositionXOffset = 0f;
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06000E69 RID: 3689 RVA: 0x00027D50 File Offset: 0x00025F50
		// (set) Token: 0x06000E6A RID: 3690 RVA: 0x00027D58 File Offset: 0x00025F58
		[Editor(false)]
		public Widget Overlay
		{
			get
			{
				return this._overlay;
			}
			set
			{
				if (this._overlay != value)
				{
					this._overlay = value;
					base.OnPropertyChanged<Widget>(value, "Overlay");
				}
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06000E6B RID: 3691 RVA: 0x00027D76 File Offset: 0x00025F76
		// (set) Token: 0x06000E6C RID: 3692 RVA: 0x00027D7E File Offset: 0x00025F7E
		[Editor(false)]
		public GridWidget ArmyListGridWidget
		{
			get
			{
				return this._armyListGridWidget;
			}
			set
			{
				if (this._armyListGridWidget != value)
				{
					this._armyListGridWidget = value;
					base.OnPropertyChanged<GridWidget>(value, "ArmyListGridWidget");
				}
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06000E6D RID: 3693 RVA: 0x00027D9C File Offset: 0x00025F9C
		// (set) Token: 0x06000E6E RID: 3694 RVA: 0x00027DA4 File Offset: 0x00025FA4
		[Editor(false)]
		public ButtonWidget ExtendButton
		{
			get
			{
				return this._extendButton;
			}
			set
			{
				if (this._extendButton != value)
				{
					this._extendButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ExtendButton");
					if (this._extendButton != null)
					{
						this._extendButton.ClickEventHandlers.Add(new Action<Widget>(this.OnExtendButtonClick));
					}
				}
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06000E6F RID: 3695 RVA: 0x00027DF1 File Offset: 0x00025FF1
		// (set) Token: 0x06000E70 RID: 3696 RVA: 0x00027DF9 File Offset: 0x00025FF9
		[Editor(false)]
		public bool IsInfoBarExtended
		{
			get
			{
				return this._isInfoBarExtended;
			}
			set
			{
				if (this._isInfoBarExtended != value)
				{
					this._isInfoBarExtended = value;
					base.OnPropertyChanged(value, "IsInfoBarExtended");
				}
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06000E71 RID: 3697 RVA: 0x00027E17 File Offset: 0x00026017
		// (set) Token: 0x06000E72 RID: 3698 RVA: 0x00027E20 File Offset: 0x00026020
		[Editor(false)]
		public ContainerPageControlWidget PageControlWidget
		{
			get
			{
				return this._pageControlWidget;
			}
			set
			{
				if (value != this._pageControlWidget)
				{
					if (this._pageControlWidget != null)
					{
						this._pageControlWidget.OnPageCountChanged -= this.OnArmyListPageCountChanged;
					}
					this._pageControlWidget = value;
					this._pageControlWidget.OnPageCountChanged += this.OnArmyListPageCountChanged;
					base.OnPropertyChanged<ContainerPageControlWidget>(value, "PageControlWidget");
				}
			}
		}

		// Token: 0x04000685 RID: 1669
		private bool _isOverlayExtended = true;

		// Token: 0x04000686 RID: 1670
		private int _armyItemCount;

		// Token: 0x04000687 RID: 1671
		private bool _initialized;

		// Token: 0x04000688 RID: 1672
		private Widget _overlay;

		// Token: 0x04000689 RID: 1673
		private bool _isInfoBarExtended;

		// Token: 0x0400068A RID: 1674
		private ButtonWidget _extendButton;

		// Token: 0x0400068B RID: 1675
		private GridWidget _armyListGridWidget;

		// Token: 0x0400068C RID: 1676
		private ContainerPageControlWidget _pageControlWidget;
	}
}
