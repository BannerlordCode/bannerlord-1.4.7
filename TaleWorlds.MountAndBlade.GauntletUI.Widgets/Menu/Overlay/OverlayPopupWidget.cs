using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.Overlay
{
	// Token: 0x02000112 RID: 274
	public class OverlayPopupWidget : Widget
	{
		// Token: 0x06000E94 RID: 3732 RVA: 0x000281C3 File Offset: 0x000263C3
		public OverlayPopupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x000281CC File Offset: 0x000263CC
		public void SetCurrentCharacter(GameMenuPartyItemButtonWidget item)
		{
			this.NameTextWidget.Text = item.Name;
			this.DescriptionTextWidget.Text = item.Description;
			this.LocationTextWidget.Text = item.Location;
			this.PowerTextWidget.Text = item.Power;
			if (item.CurrentCharacterImageWidget != null)
			{
				this.CurrentCharacterImageWidget.ImageId = item.CurrentCharacterImageWidget.ImageId;
				this.CurrentCharacterImageWidget.TextureProviderName = item.CurrentCharacterImageWidget.TextureProviderName;
				this.CurrentCharacterImageWidget.AdditionalArgs = item.CurrentCharacterImageWidget.AdditionalArgs;
			}
			if (!base.ParentWidget.IsVisible)
			{
				this.OpenPopup();
			}
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x0002827A File Offset: 0x0002647A
		private void OpenPopup()
		{
			base.ParentWidget.IsVisible = true;
			base.EventFired("OnOpen", Array.Empty<object>());
			this._isOpen = true;
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x0002829F File Offset: 0x0002649F
		private void ClosePopup()
		{
			base.ParentWidget.IsVisible = false;
			base.EventFired("OnClose", Array.Empty<object>());
			this._isOpen = false;
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x000282C4 File Offset: 0x000264C4
		public void OnCloseButtonClick(Widget widget)
		{
			this.ClosePopup();
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x000282CC File Offset: 0x000264CC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isOpen && !base.IsRecursivelyVisible())
			{
				this.ClosePopup();
			}
			else if (!this._isOpen && base.IsRecursivelyVisible())
			{
				this.OpenPopup();
			}
			if (!(base.EventManager.LatestMouseDownWidget is GameMenuPartyItemButtonWidget) && base.EventManager.LatestMouseDownWidget != this && base.EventManager.LatestMouseDownWidget != this._closeButton && base.ParentWidget.IsVisible && (!base.CheckIsMyChildRecursive(base.EventManager.LatestMouseDownWidget) || this.ActionButtonsList.CheckIsMyChildRecursive(base.EventManager.LatestMouseUpWidget)))
			{
				this.ClosePopup();
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06000E9A RID: 3738 RVA: 0x0002837F File Offset: 0x0002657F
		// (set) Token: 0x06000E9B RID: 3739 RVA: 0x00028387 File Offset: 0x00026587
		[Editor(false)]
		public ImageIdentifierWidget CurrentCharacterImageWidget
		{
			get
			{
				return this._currentCharacterImageWidget;
			}
			set
			{
				if (this._currentCharacterImageWidget != value)
				{
					this._currentCharacterImageWidget = value;
					base.OnPropertyChanged<ImageIdentifierWidget>(value, "CurrentCharacterImageWidget");
				}
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06000E9C RID: 3740 RVA: 0x000283A5 File Offset: 0x000265A5
		// (set) Token: 0x06000E9D RID: 3741 RVA: 0x000283AD File Offset: 0x000265AD
		[Editor(false)]
		public TextWidget LocationTextWidget
		{
			get
			{
				return this._locationTextWidget;
			}
			set
			{
				if (this._locationTextWidget != value)
				{
					this._locationTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "LocationTextWidget");
				}
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06000E9E RID: 3742 RVA: 0x000283CB File Offset: 0x000265CB
		// (set) Token: 0x06000E9F RID: 3743 RVA: 0x000283D3 File Offset: 0x000265D3
		[Editor(false)]
		public TextWidget NameTextWidget
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
					base.OnPropertyChanged<TextWidget>(value, "NameTextWidget");
				}
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x000283F1 File Offset: 0x000265F1
		// (set) Token: 0x06000EA1 RID: 3745 RVA: 0x000283F9 File Offset: 0x000265F9
		[Editor(false)]
		public TextWidget PowerTextWidget
		{
			get
			{
				return this._powerTextWidget;
			}
			set
			{
				if (this._powerTextWidget != value)
				{
					this._powerTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "PowerTextWidget");
				}
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x00028417 File Offset: 0x00026617
		// (set) Token: 0x06000EA3 RID: 3747 RVA: 0x0002841F File Offset: 0x0002661F
		[Editor(false)]
		public TextWidget DescriptionTextWidget
		{
			get
			{
				return this._descriptionTextWidget;
			}
			set
			{
				if (this._descriptionTextWidget != value)
				{
					this._descriptionTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "DescriptionTextWidget");
				}
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x0002843D File Offset: 0x0002663D
		// (set) Token: 0x06000EA5 RID: 3749 RVA: 0x00028445 File Offset: 0x00026645
		[Editor(false)]
		public Widget RelationBackgroundWidget
		{
			get
			{
				return this._relationBackgroundWidget;
			}
			set
			{
				if (this._relationBackgroundWidget != value)
				{
					this._relationBackgroundWidget = value;
					base.OnPropertyChanged<Widget>(value, "RelationBackgroundWidget");
				}
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x00028463 File Offset: 0x00026663
		// (set) Token: 0x06000EA7 RID: 3751 RVA: 0x0002846B File Offset: 0x0002666B
		[Editor(false)]
		public ListPanel ActionButtonsList
		{
			get
			{
				return this._actionButtonsList;
			}
			set
			{
				if (this._actionButtonsList != value)
				{
					this._actionButtonsList = value;
					base.OnPropertyChanged<ListPanel>(value, "ActionButtonsList");
				}
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x00028489 File Offset: 0x00026689
		// (set) Token: 0x06000EA9 RID: 3753 RVA: 0x00028494 File Offset: 0x00026694
		[Editor(false)]
		public ButtonWidget CloseButton
		{
			get
			{
				return this._closeButton;
			}
			set
			{
				if (this._closeButton != value)
				{
					ButtonWidget closeButton = this._closeButton;
					if (closeButton != null)
					{
						closeButton.ClickEventHandlers.Remove(new Action<Widget>(this.OnCloseButtonClick));
					}
					this._closeButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "CloseButton");
					ButtonWidget closeButton2 = this._closeButton;
					if (closeButton2 == null)
					{
						return;
					}
					closeButton2.ClickEventHandlers.Add(new Action<Widget>(this.OnCloseButtonClick));
				}
			}
		}

		// Token: 0x0400069E RID: 1694
		private bool _isOpen;

		// Token: 0x0400069F RID: 1695
		private ImageIdentifierWidget _currentCharacterImageWidget;

		// Token: 0x040006A0 RID: 1696
		private TextWidget _locationTextWidget;

		// Token: 0x040006A1 RID: 1697
		private TextWidget _descriptionTextWidget;

		// Token: 0x040006A2 RID: 1698
		private TextWidget _powerTextWidget;

		// Token: 0x040006A3 RID: 1699
		private TextWidget _nameTextWidget;

		// Token: 0x040006A4 RID: 1700
		private Widget _relationBackgroundWidget;

		// Token: 0x040006A5 RID: 1701
		private ButtonWidget _closeButton;

		// Token: 0x040006A6 RID: 1702
		private ListPanel _actionButtonsList;
	}
}
