using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Nameplate
{
	// Token: 0x0200007E RID: 126
	public class PartyNameplateWidget : Widget
	{
		// Token: 0x060006DC RID: 1756 RVA: 0x00013D6D File Offset: 0x00011F6D
		public PartyNameplateWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x00013D88 File Offset: 0x00011F88
		protected float _animSpeedModifier
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x00013D8F File Offset: 0x00011F8F
		protected int _armyFontSizeOffset
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x00013D93 File Offset: 0x00011F93
		// (set) Token: 0x060006E0 RID: 1760 RVA: 0x00013D9B File Offset: 0x00011F9B
		public Widget HeadGroupWidget { get; set; }

		// Token: 0x060006E1 RID: 1761 RVA: 0x00013DA4 File Offset: 0x00011FA4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isFirstFrame)
			{
				this.NameplateFullNameTextWidget.Brush.GlobalAlphaFactor = 0f;
				this.NameplateTextWidget.Brush.GlobalAlphaFactor = 0f;
				this.NameplateExtraInfoTextWidget.Brush.GlobalAlphaFactor = 0f;
				this.PartyBannerWidget.Brush.GlobalAlphaFactor = 0f;
				this.SpeedTextWidget.Brush.GlobalAlphaFactor = 0f;
				this.ParleyIconWidget.AlphaFactor = 0f;
				this._defaultNameplateFontSize = this.NameplateTextWidget.ReadOnlyBrush.FontSize;
				this._isFirstFrame = false;
			}
			int num = (this.IsArmy ? (this._defaultNameplateFontSize + this._armyFontSizeOffset) : this._defaultNameplateFontSize);
			if (this.NameplateTextWidget.Brush.FontSize != num)
			{
				this.NameplateTextWidget.Brush.FontSize = num;
			}
			this.UpdateNameplatesScreenPosition();
			this.UpdateNameplatesVisibility(dt);
			this.UpdateTutorialStatus();
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00013EB0 File Offset: 0x000120B0
		protected virtual void UpdateNameplatesVisibility(float dt)
		{
			float num = 0f;
			this.PartyBannerWidget.IsVisible = true;
			this.NameplateTextWidget.IsVisible = this.IsVisibleOnMap;
			this.NameplateFullNameTextWidget.IsVisible = this.IsVisibleOnMap;
			this.SpeedTextWidget.IsVisible = this.IsVisibleOnMap;
			this.SpeedIconWidget.IsVisible = this.IsVisibleOnMap;
			this.DisorganizedWidget.IsVisible = this.IsVisibleOnMap && this.IsDisorganized;
			float num2 = (float)(this.IsVisibleOnMap ? 1 : 0);
			this.TrackerFrame.IsVisible = false;
			base.IsEnabled = false;
			if (this.IsVisibleOnMap)
			{
				if (this._initialDelayAmount <= 0f)
				{
					num = (float)(this.ShouldShowFullName ? 1 : 0);
				}
				else
				{
					this._initialDelayAmount -= dt;
					num = 1f;
				}
			}
			this.NameplateTextWidget.Brush.GlobalAlphaFactor = MathF.Lerp(this.NameplateTextWidget.ReadOnlyBrush.GlobalAlphaFactor, num2, dt * this._animSpeedModifier, 1E-05f);
			this.NameplateFullNameTextWidget.Brush.GlobalAlphaFactor = MathF.Lerp(this.NameplateFullNameTextWidget.ReadOnlyBrush.GlobalAlphaFactor, num, dt * this._animSpeedModifier, 1E-05f);
			this.SpeedTextWidget.Brush.GlobalAlphaFactor = MathF.Lerp(this.SpeedTextWidget.ReadOnlyBrush.GlobalAlphaFactor, num, dt * this._animSpeedModifier, 1E-05f);
			float num3 = MathF.Lerp(this.SpeedIconWidget.AlphaFactor, num, dt * this._animSpeedModifier, 1E-05f);
			this.SpeedIconWidget.SetGlobalAlphaRecursively(num3);
			this.NameplateExtraInfoTextWidget.Brush.GlobalAlphaFactor = MathF.Lerp(this.NameplateExtraInfoTextWidget.ReadOnlyBrush.GlobalAlphaFactor, (float)(this.ShouldShowFullName ? 1 : 0), dt * this._animSpeedModifier, 1E-05f);
			this.PartyBannerWidget.Brush.GlobalAlphaFactor = MathF.Lerp(this.PartyBannerWidget.ReadOnlyBrush.GlobalAlphaFactor, num2, dt * this._animSpeedModifier, 1E-05f);
			this.ParleyIconWidget.AlphaFactor = MathF.Lerp(this.ParleyIconWidget.AlphaFactor, (float)(this.CanParley ? 1 : 0), dt * this._animSpeedModifier, 1E-05f);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x000140FC File Offset: 0x000122FC
		protected virtual void UpdateNameplatesScreenPosition()
		{
			this._screenWidth = base.Context.EventManager.PageSize.X;
			this._screenHeight = base.Context.EventManager.PageSize.Y;
			base.IsVisible = this.IsVisibleOnMap && !this.IsPositionOutsideScreen();
			if (!base.IsVisible)
			{
				return;
			}
			Widget headGroupWidget = this.HeadGroupWidget;
			float num = ((headGroupWidget != null) ? headGroupWidget.Size.Y : 0f);
			this.NameplateLayoutListPanel.ScaledPositionXOffset = base.Size.X / 2f - this.PartyBannerWidget.Size.X;
			this.NameplateLayoutListPanel.ScaledPositionYOffset = this.Position.y - this.HeadPosition.y + num;
			base.ScaledPositionXOffset = this.HeadPosition.x - base.Size.X / 2f;
			base.ScaledPositionYOffset = this.HeadPosition.y - num;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00014205 File Offset: 0x00012405
		private void UpdateTutorialStatus()
		{
			if (this._tutorialAnimState == PartyNameplateWidget.TutorialAnimState.Start)
			{
				this._tutorialAnimState = PartyNameplateWidget.TutorialAnimState.FirstFrame;
			}
			else
			{
				PartyNameplateWidget.TutorialAnimState tutorialAnimState = this._tutorialAnimState;
			}
			if (this.IsTargetedByTutorial)
			{
				this.SetState("Default");
				return;
			}
			this.SetState("Disabled");
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00014244 File Offset: 0x00012444
		protected bool IsPositionOutsideScreen()
		{
			return this.Position.X > this._screenWidth || this.HeadPosition.X > this._screenWidth || this.Position.X < 0f || this.HeadPosition.X < 0f || this.Position.Y > this._screenHeight || this.HeadPosition.Y > this._screenHeight || this.Position.Y < 0f || this.HeadPosition.Y < 0f;
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x00014305 File Offset: 0x00012505
		// (set) Token: 0x060006E7 RID: 1767 RVA: 0x0001430D File Offset: 0x0001250D
		public ListPanel NameplateLayoutListPanel
		{
			get
			{
				return this._nameplateLayoutListPanel;
			}
			set
			{
				if (this._nameplateLayoutListPanel != value)
				{
					this._nameplateLayoutListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "NameplateLayoutListPanel");
				}
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x0001432B File Offset: 0x0001252B
		// (set) Token: 0x060006E9 RID: 1769 RVA: 0x00014333 File Offset: 0x00012533
		public MaskedTextureWidget PartyBannerWidget
		{
			get
			{
				return this._partyBannerWidget;
			}
			set
			{
				if (this._partyBannerWidget != value)
				{
					this._partyBannerWidget = value;
					base.OnPropertyChanged<MaskedTextureWidget>(value, "PartyBannerWidget");
				}
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x00014351 File Offset: 0x00012551
		// (set) Token: 0x060006EB RID: 1771 RVA: 0x00014359 File Offset: 0x00012559
		public Widget TrackerFrame
		{
			get
			{
				return this._trackerFrame;
			}
			set
			{
				if (this._trackerFrame != value)
				{
					this._trackerFrame = value;
					base.OnPropertyChanged<Widget>(value, "TrackerFrame");
				}
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x00014377 File Offset: 0x00012577
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x0001437F File Offset: 0x0001257F
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (this._position != value)
				{
					this._position = value;
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x000143A2 File Offset: 0x000125A2
		// (set) Token: 0x060006EF RID: 1775 RVA: 0x000143AA File Offset: 0x000125AA
		public Vec2 HeadPosition
		{
			get
			{
				return this._headPosition;
			}
			set
			{
				if (this._headPosition != value)
				{
					this._headPosition = value;
					base.OnPropertyChanged(value, "HeadPosition");
				}
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x000143CD File Offset: 0x000125CD
		// (set) Token: 0x060006F1 RID: 1777 RVA: 0x000143D5 File Offset: 0x000125D5
		public bool ShouldShowFullName
		{
			get
			{
				return this._shouldShowFullName;
			}
			set
			{
				if (this._shouldShowFullName != value)
				{
					this._shouldShowFullName = value;
					base.OnPropertyChanged(value, "ShouldShowFullName");
				}
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x000143F3 File Offset: 0x000125F3
		// (set) Token: 0x060006F3 RID: 1779 RVA: 0x000143FB File Offset: 0x000125FB
		public bool CanParley
		{
			get
			{
				return this._canParley;
			}
			set
			{
				if (this._canParley != value)
				{
					this._canParley = value;
					base.OnPropertyChanged(value, "CanParley");
				}
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x00014419 File Offset: 0x00012619
		// (set) Token: 0x060006F5 RID: 1781 RVA: 0x00014421 File Offset: 0x00012621
		public bool IsTargetedByTutorial
		{
			get
			{
				return this._isTargetedByTutorial;
			}
			set
			{
				if (this._isTargetedByTutorial != value)
				{
					this._isTargetedByTutorial = value;
					base.OnPropertyChanged(value, "IsTargetedByTutorial");
					this._tutorialAnimState = PartyNameplateWidget.TutorialAnimState.Start;
				}
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x00014446 File Offset: 0x00012646
		// (set) Token: 0x060006F7 RID: 1783 RVA: 0x0001444E File Offset: 0x0001264E
		public bool IsInArmy
		{
			get
			{
				return this._isInArmy;
			}
			set
			{
				if (this._isInArmy != value)
				{
					this._isInArmy = value;
					base.OnPropertyChanged(value, "IsInArmy");
				}
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x0001446C File Offset: 0x0001266C
		// (set) Token: 0x060006F9 RID: 1785 RVA: 0x00014474 File Offset: 0x00012674
		public bool IsInSettlement
		{
			get
			{
				return this._isInSettlement;
			}
			set
			{
				if (this._isInSettlement != value)
				{
					this._isInSettlement = value;
					base.OnPropertyChanged(value, "IsInSettlement");
				}
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x00014492 File Offset: 0x00012692
		// (set) Token: 0x060006FB RID: 1787 RVA: 0x0001449A File Offset: 0x0001269A
		public bool IsArmy
		{
			get
			{
				return this._isArmy;
			}
			set
			{
				if (this._isArmy != value)
				{
					this._isArmy = value;
					base.OnPropertyChanged(value, "IsArmy");
				}
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x000144B8 File Offset: 0x000126B8
		// (set) Token: 0x060006FD RID: 1789 RVA: 0x000144C0 File Offset: 0x000126C0
		public bool IsVisibleOnMap
		{
			get
			{
				return this._isVisibleOnMap;
			}
			set
			{
				if (this._isVisibleOnMap != value)
				{
					this._isVisibleOnMap = value;
					base.OnPropertyChanged(value, "IsVisibleOnMap");
				}
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x000144DE File Offset: 0x000126DE
		// (set) Token: 0x060006FF RID: 1791 RVA: 0x000144E6 File Offset: 0x000126E6
		public bool IsInside
		{
			get
			{
				return this._isInside;
			}
			set
			{
				if (this._isInside != value)
				{
					this._isInside = value;
					base.OnPropertyChanged(value, "IsInside");
				}
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x00014504 File Offset: 0x00012704
		// (set) Token: 0x06000701 RID: 1793 RVA: 0x0001450C File Offset: 0x0001270C
		public bool IsHigh
		{
			get
			{
				return this._isHigh;
			}
			set
			{
				if (this._isHigh != value)
				{
					this._isHigh = value;
					base.OnPropertyChanged(value, "IsHigh");
				}
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x0001452A File Offset: 0x0001272A
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x00014532 File Offset: 0x00012732
		public bool IsBehind
		{
			get
			{
				return this._isBehind;
			}
			set
			{
				if (this._isBehind != value)
				{
					this._isBehind = value;
					base.OnPropertyChanged(value, "IsBehind");
				}
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x00014550 File Offset: 0x00012750
		// (set) Token: 0x06000705 RID: 1797 RVA: 0x00014558 File Offset: 0x00012758
		public bool IsDisorganized
		{
			get
			{
				return this._isDisorganized;
			}
			set
			{
				if (this._isDisorganized != value)
				{
					this._isDisorganized = value;
					base.OnPropertyChanged(value, "IsDisorganized");
				}
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x00014576 File Offset: 0x00012776
		// (set) Token: 0x06000707 RID: 1799 RVA: 0x0001457E File Offset: 0x0001277E
		public TextWidget NameplateTextWidget
		{
			get
			{
				return this._nameplateTextWidget;
			}
			set
			{
				if (this._nameplateTextWidget != value)
				{
					this._nameplateTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "NameplateTextWidget");
				}
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x0001459C File Offset: 0x0001279C
		// (set) Token: 0x06000709 RID: 1801 RVA: 0x000145A4 File Offset: 0x000127A4
		public TextWidget NameplateExtraInfoTextWidget
		{
			get
			{
				return this._nameplateExtraInfoTextWidget;
			}
			set
			{
				if (this._nameplateExtraInfoTextWidget != value)
				{
					this._nameplateExtraInfoTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "NameplateExtraInfoTextWidget");
				}
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x000145C2 File Offset: 0x000127C2
		// (set) Token: 0x0600070B RID: 1803 RVA: 0x000145CA File Offset: 0x000127CA
		public TextWidget NameplateFullNameTextWidget
		{
			get
			{
				return this._nameplateFullNameTextWidget;
			}
			set
			{
				if (this._nameplateFullNameTextWidget != value)
				{
					this._nameplateFullNameTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "NameplateFullNameTextWidget");
				}
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x000145E8 File Offset: 0x000127E8
		// (set) Token: 0x0600070D RID: 1805 RVA: 0x000145F0 File Offset: 0x000127F0
		public TextWidget SpeedTextWidget
		{
			get
			{
				return this._speedTextWidget;
			}
			set
			{
				if (this._speedTextWidget != value)
				{
					this._speedTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "SpeedTextWidget");
				}
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x0001460E File Offset: 0x0001280E
		// (set) Token: 0x0600070F RID: 1807 RVA: 0x00014616 File Offset: 0x00012816
		public Widget SpeedIconWidget
		{
			get
			{
				return this._speedIconWidget;
			}
			set
			{
				if (value != this._speedIconWidget)
				{
					this._speedIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "SpeedIconWidget");
				}
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x00014634 File Offset: 0x00012834
		// (set) Token: 0x06000711 RID: 1809 RVA: 0x0001463C File Offset: 0x0001283C
		public Widget ParleyIconWidget
		{
			get
			{
				return this._parleyIconWidget;
			}
			set
			{
				if (value != this._parleyIconWidget)
				{
					this._parleyIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "ParleyIconWidget");
				}
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x0001465A File Offset: 0x0001285A
		// (set) Token: 0x06000713 RID: 1811 RVA: 0x00014662 File Offset: 0x00012862
		public Widget DisorganizedWidget
		{
			get
			{
				return this._disorganizedWidget;
			}
			set
			{
				if (this._disorganizedWidget != value)
				{
					this._disorganizedWidget = value;
					base.OnPropertyChanged<Widget>(value, "DisorganizedWidget");
				}
			}
		}

		// Token: 0x040002F6 RID: 758
		protected bool _isFirstFrame = true;

		// Token: 0x040002F7 RID: 759
		protected float _screenWidth;

		// Token: 0x040002F8 RID: 760
		protected float _screenHeight;

		// Token: 0x040002F9 RID: 761
		protected float _initialDelayAmount = 2f;

		// Token: 0x040002FA RID: 762
		protected int _defaultNameplateFontSize;

		// Token: 0x040002FB RID: 763
		protected PartyNameplateWidget.TutorialAnimState _tutorialAnimState;

		// Token: 0x040002FD RID: 765
		private Vec2 _position;

		// Token: 0x040002FE RID: 766
		private Vec2 _headPosition;

		// Token: 0x040002FF RID: 767
		private TextWidget _nameplateTextWidget;

		// Token: 0x04000300 RID: 768
		private TextWidget _nameplateFullNameTextWidget;

		// Token: 0x04000301 RID: 769
		private TextWidget _speedTextWidget;

		// Token: 0x04000302 RID: 770
		private Widget _speedIconWidget;

		// Token: 0x04000303 RID: 771
		private Widget _parleyIconWidget;

		// Token: 0x04000304 RID: 772
		private TextWidget _nameplateExtraInfoTextWidget;

		// Token: 0x04000305 RID: 773
		private Widget _trackerFrame;

		// Token: 0x04000306 RID: 774
		private Widget _disorganizedWidget;

		// Token: 0x04000307 RID: 775
		private ListPanel _nameplateLayoutListPanel;

		// Token: 0x04000308 RID: 776
		private MaskedTextureWidget _partyBannerWidget;

		// Token: 0x04000309 RID: 777
		private bool _isVisibleOnMap;

		// Token: 0x0400030A RID: 778
		private bool _isInside;

		// Token: 0x0400030B RID: 779
		private bool _isBehind;

		// Token: 0x0400030C RID: 780
		private bool _isHigh;

		// Token: 0x0400030D RID: 781
		private bool _isInArmy;

		// Token: 0x0400030E RID: 782
		private bool _isInSettlement;

		// Token: 0x0400030F RID: 783
		private bool _isArmy;

		// Token: 0x04000310 RID: 784
		private bool _isTargetedByTutorial;

		// Token: 0x04000311 RID: 785
		private bool _shouldShowFullName;

		// Token: 0x04000312 RID: 786
		private bool _canParley;

		// Token: 0x04000313 RID: 787
		private bool _isDisorganized;

		// Token: 0x020001B4 RID: 436
		public enum TutorialAnimState
		{
			// Token: 0x040009E8 RID: 2536
			Idle,
			// Token: 0x040009E9 RID: 2537
			Start,
			// Token: 0x040009EA RID: 2538
			FirstFrame,
			// Token: 0x040009EB RID: 2539
			Playing
		}
	}
}
