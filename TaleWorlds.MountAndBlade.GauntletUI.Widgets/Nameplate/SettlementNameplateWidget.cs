using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Nameplate
{
	// Token: 0x02000083 RID: 131
	public class SettlementNameplateWidget : Widget, IComparable<SettlementNameplateWidget>
	{
		// Token: 0x06000740 RID: 1856 RVA: 0x0001511A File Offset: 0x0001331A
		public SettlementNameplateWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x00015140 File Offset: 0x00013340
		private float _screenEdgeAlphaTarget
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x00015147 File Offset: 0x00013347
		private float _normalNeutralAlphaTarget
		{
			get
			{
				return 0.35f;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x0001514E File Offset: 0x0001334E
		private float _normalAllyAlphaTarget
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x00015155 File Offset: 0x00013355
		private float _normalEnemyAlphaTarget
		{
			get
			{
				return 0.35f;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x0001515C File Offset: 0x0001335C
		private float _trackedAlphaTarget
		{
			get
			{
				return 0.8f;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x00015163 File Offset: 0x00013363
		private float _trackedColorFactorTarget
		{
			get
			{
				return 1.3f;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x0001516A File Offset: 0x0001336A
		private float _normalColorFactorTarget
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00015174 File Offset: 0x00013374
		protected override void OnParallelUpdate(float dt)
		{
			base.OnParallelUpdate(dt);
			SettlementNameplateItemWidget nameplateItem = this.NameplateItem;
			if (nameplateItem != null)
			{
				nameplateItem.ParallelUpdate(dt);
			}
			if (nameplateItem != null && this._cachedItemSize != nameplateItem.Size)
			{
				this._cachedItemSize = nameplateItem.Size;
				ListPanel eventsListPanel = this._eventsListPanel;
				ListPanel notificationListPanel = this._notificationListPanel;
				if (eventsListPanel != null)
				{
					eventsListPanel.ScaledPositionXOffset = this._cachedItemSize.X;
				}
				if (notificationListPanel != null)
				{
					notificationListPanel.ScaledPositionYOffset = -this._cachedItemSize.Y;
				}
				base.SuggestedWidth = this._cachedItemSize.X * base._inverseScaleToUse;
				base.SuggestedHeight = this._cachedItemSize.Y * base._inverseScaleToUse;
				base.ScaledSuggestedWidth = this._cachedItemSize.X;
				base.ScaledSuggestedHeight = this._cachedItemSize.Y;
			}
			base.IsEnabled = this.IsVisibleOnMap;
			this.UpdateNameplateTransparencyAndBrightness(dt);
			this.UpdatePosition(dt);
			this.UpdateTutorialState();
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0001526C File Offset: 0x0001346C
		private void UpdatePosition(float dt)
		{
			SettlementNameplateItemWidget nameplateItem = this.NameplateItem;
			MapEventVisualBrushWidget mapEventVisualBrushWidget = ((nameplateItem != null) ? nameplateItem.MapEventVisualWidget : null);
			if (nameplateItem == null || mapEventVisualBrushWidget == null)
			{
				Debug.FailedAssert("Related widget null on UpdatePosition!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Nameplate\\SettlementNameplateWidget.cs", "UpdatePosition", 104);
				return;
			}
			bool flag = false;
			this._positionTimer += dt;
			if (this.IsVisibleOnMap || this._positionTimer < 2f)
			{
				float num = this.Position.X - base.Size.X / 2f - base.ScaledMarginLeft;
				float num2 = this.Position.X + base.Size.X / 2f + base.ScaledMarginRight;
				float num3 = this.Position.Y - base.Size.Y - base.ScaledMarginTop;
				float num4 = this.Position.Y + base.ScaledMarginBottom;
				bool flag2 = this.WSign > 0 && num > 0f && num2 < base.Context.EventManager.PageSize.X && num3 > 0f && num4 < base.Context.EventManager.PageSize.Y;
				if (this.IsTracked && !flag2)
				{
					Vec2 vec = new Vec2(num, num3);
					Vector2 vector = base.Context.EventManager.PageSize - base.Size;
					vector.X -= base.ScaledMarginLeft + base.ScaledMarginRight;
					vector.Y -= base.ScaledMarginTop + base.ScaledMarginBottom;
					Vec2 vec2 = vector / 2f;
					vec -= vec2;
					if (this.WSign < 0)
					{
						vec *= -1f;
					}
					float num5 = Mathf.Atan2(vec.y, vec.x) - 1.5707964f;
					float num6 = Mathf.Cos(num5);
					float num7 = Mathf.Sin(num5);
					float num8 = num6 / num7;
					Vec2 vec3 = vec2 * 1f;
					vec = ((num6 > 0f) ? new Vec2(-vec3.y / num8, vec2.y) : new Vec2(vec3.y / num8, -vec2.y));
					if (vec.x > vec3.x)
					{
						vec = new Vec2(vec3.x, -vec3.x * num8);
					}
					else if (vec.x < -vec3.x)
					{
						vec = new Vec2(-vec3.x, vec3.x * num8);
					}
					vec += vec2;
					base.ScaledPositionXOffset = Mathf.Clamp(vec.x, 0f, vector.X);
					base.ScaledPositionYOffset = Mathf.Clamp(vec.y, 0f, vector.Y);
				}
				else
				{
					base.ScaledPositionXOffset = num;
					base.ScaledPositionYOffset = num3;
				}
				flag = base.ScaledPositionYOffset - mapEventVisualBrushWidget.Size.Y < 0f;
			}
			if (flag)
			{
				mapEventVisualBrushWidget.VerticalAlignment = VerticalAlignment.Bottom;
				mapEventVisualBrushWidget.ScaledPositionYOffset = mapEventVisualBrushWidget.Size.Y;
				return;
			}
			mapEventVisualBrushWidget.VerticalAlignment = VerticalAlignment.Top;
			mapEventVisualBrushWidget.ScaledPositionYOffset = -mapEventVisualBrushWidget.Size.Y;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x000155C1 File Offset: 0x000137C1
		private void OnNotificationListUpdated(Widget widget)
		{
			this._updatePositionNextFrame = true;
			this.AddLateUpdateAction();
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x000155D0 File Offset: 0x000137D0
		private void OnNotificationListUpdated(Widget parentWidget, Widget addedWidget)
		{
			this._updatePositionNextFrame = true;
			this.AddLateUpdateAction();
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x000155DF File Offset: 0x000137DF
		private void AddLateUpdateAction()
		{
			if (!this._lateUpdateActionAdded)
			{
				base.EventManager.AddLateUpdateAction(this, new Action<float>(this.CustomLateUpdate), 1);
				this._lateUpdateActionAdded = true;
			}
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00015609 File Offset: 0x00013809
		private void CustomLateUpdate(float dt)
		{
			if (this._updatePositionNextFrame)
			{
				this.UpdatePosition(dt);
				this._updatePositionNextFrame = false;
			}
			this._lateUpdateActionAdded = false;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00015628 File Offset: 0x00013828
		private void UpdateTutorialState()
		{
			if (this._tutorialAnimState == SettlementNameplateWidget.TutorialAnimState.Start)
			{
				this._tutorialAnimState = SettlementNameplateWidget.TutorialAnimState.FirstFrame;
			}
			else
			{
				SettlementNameplateWidget.TutorialAnimState tutorialAnimState = this._tutorialAnimState;
			}
			if (this.IsTargetedByTutorial)
			{
				this.SetState("Default");
				return;
			}
			this.SetState("Disabled");
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x00015664 File Offset: 0x00013864
		private void SetNameplateRelationType(int type)
		{
			if (this.NameplateItem != null)
			{
				switch (type)
				{
				case 0:
					this.NameplateItem.Color = Color.Black;
					return;
				case 1:
					this.NameplateItem.Color = Color.ConvertStringToColor("#245E05FF");
					return;
				case 2:
					this.NameplateItem.Color = Color.ConvertStringToColor("#870707FF");
					return;
				case 3:
					this.NameplateItem.Color = Color.ConvertStringToColor("#2986CCFF");
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x000156E4 File Offset: 0x000138E4
		private void UpdateNameplateTransparencyAndBrightness(float dt)
		{
			SettlementNameplateItemWidget nameplateItem = this.NameplateItem;
			TextWidget textWidget = ((nameplateItem != null) ? nameplateItem.SettlementNameTextWidget : null);
			MaskedTextureWidget maskedTextureWidget = ((nameplateItem != null) ? nameplateItem.SettlementBannerWidget : null);
			GridWidget gridWidget = ((nameplateItem != null) ? nameplateItem.SettlementPartiesGridWidget : null);
			Widget widget = ((nameplateItem != null) ? nameplateItem.InspectedIconWidget : null);
			Widget widget2 = ((nameplateItem != null) ? nameplateItem.PortIconWidget : null);
			Widget widget3 = ((nameplateItem != null) ? nameplateItem.ParleyIconWidget : null);
			ListPanel eventsListPanel = this._eventsListPanel;
			if (nameplateItem == null || textWidget == null || maskedTextureWidget == null || gridWidget == null || widget == null || widget2 == null || widget3 == null || eventsListPanel == null)
			{
				Debug.FailedAssert("Related widget null on UpdateNameplateTransparencyAndBrightness!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Nameplate\\SettlementNameplateWidget.cs", "UpdateNameplateTransparencyAndBrightness", 302);
				return;
			}
			widget2.IsVisible = this.HasPort;
			float num = dt * this._lerpModifier;
			if (this.IsVisibleOnMap)
			{
				base.IsVisible = true;
				float num2 = this.DetermineTargetAlphaValue();
				float num3 = this.DetermineTargetColorFactor();
				float num4 = MathF.Lerp(nameplateItem.AlphaFactor, num2, num, 1E-05f);
				float num5 = MathF.Lerp(nameplateItem.ColorFactor, num3, num, 1E-05f);
				float num6 = MathF.Lerp(textWidget.ReadOnlyBrush.GlobalAlphaFactor, 1f, num, 1E-05f);
				nameplateItem.AlphaFactor = num4;
				nameplateItem.ColorFactor = num5;
				textWidget.Brush.GlobalAlphaFactor = num6;
				maskedTextureWidget.Brush.GlobalAlphaFactor = num6;
				gridWidget.SetGlobalAlphaRecursively(num6);
				widget3.AlphaFactor = MathF.Lerp(widget3.AlphaFactor, (float)(this.CanParley ? 1 : 0), num, 1E-05f);
				eventsListPanel.SetGlobalAlphaRecursively(num6);
			}
			else if (nameplateItem.AlphaFactor > this._lerpThreshold)
			{
				float num7 = MathF.Lerp(nameplateItem.AlphaFactor, 0f, num, 1E-05f);
				nameplateItem.AlphaFactor = num7;
				textWidget.Brush.GlobalAlphaFactor = num7;
				maskedTextureWidget.Brush.GlobalAlphaFactor = num7;
				gridWidget.SetGlobalAlphaRecursively(num7);
				widget3.AlphaFactor = num7;
				eventsListPanel.SetGlobalAlphaRecursively(num7);
			}
			else
			{
				base.IsVisible = false;
			}
			if (this.IsInRange && this.IsVisibleOnMap)
			{
				if (Math.Abs(widget.AlphaFactor - 1f) > this._lerpThreshold)
				{
					widget.AlphaFactor = MathF.Lerp(widget.AlphaFactor, 1f, num, 1E-05f);
					return;
				}
			}
			else if (nameplateItem.AlphaFactor - 0f > this._lerpThreshold)
			{
				widget.AlphaFactor = MathF.Lerp(widget.AlphaFactor, 0f, num, 1E-05f);
			}
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00015958 File Offset: 0x00013B58
		private float DetermineTargetAlphaValue()
		{
			if (this.IsInsideWindow)
			{
				if (this.IsTracked)
				{
					return this._trackedAlphaTarget;
				}
				if (this.RelationType == 0)
				{
					return this._normalNeutralAlphaTarget;
				}
				if (this.RelationType == 1)
				{
					return this._normalAllyAlphaTarget;
				}
				return this._normalEnemyAlphaTarget;
			}
			else
			{
				if (this.IsTracked)
				{
					return this._screenEdgeAlphaTarget;
				}
				return 0f;
			}
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x000159B6 File Offset: 0x00013BB6
		private float DetermineTargetColorFactor()
		{
			if (this.IsTracked)
			{
				return this._trackedColorFactorTarget;
			}
			return this._normalColorFactorTarget;
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x000159D0 File Offset: 0x00013BD0
		public int CompareTo(SettlementNameplateWidget other)
		{
			return other.DistanceToCamera.CompareTo(this.DistanceToCamera);
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x000159F1 File Offset: 0x00013BF1
		// (set) Token: 0x06000755 RID: 1877 RVA: 0x000159F9 File Offset: 0x00013BF9
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

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x00015A1C File Offset: 0x00013C1C
		// (set) Token: 0x06000757 RID: 1879 RVA: 0x00015A24 File Offset: 0x00013C24
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
					if (this._isVisibleOnMap && !value)
					{
						this._positionTimer = 0f;
					}
					this._isVisibleOnMap = value;
					base.OnPropertyChanged(value, "IsVisibleOnMap");
				}
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x00015A58 File Offset: 0x00013C58
		// (set) Token: 0x06000759 RID: 1881 RVA: 0x00015A60 File Offset: 0x00013C60
		public bool IsTracked
		{
			get
			{
				return this._isTracked;
			}
			set
			{
				if (this._isTracked != value)
				{
					this._isTracked = value;
					base.OnPropertyChanged(value, "IsTracked");
				}
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x00015A7E File Offset: 0x00013C7E
		// (set) Token: 0x0600075B RID: 1883 RVA: 0x00015A86 File Offset: 0x00013C86
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
					if (value)
					{
						this._tutorialAnimState = SettlementNameplateWidget.TutorialAnimState.Start;
					}
				}
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x00015AAE File Offset: 0x00013CAE
		// (set) Token: 0x0600075D RID: 1885 RVA: 0x00015AB6 File Offset: 0x00013CB6
		public bool IsInsideWindow
		{
			get
			{
				return this._isInsideWindow;
			}
			set
			{
				if (this._isInsideWindow != value)
				{
					this._isInsideWindow = value;
					base.OnPropertyChanged(value, "IsInsideWindow");
				}
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x00015AD4 File Offset: 0x00013CD4
		// (set) Token: 0x0600075F RID: 1887 RVA: 0x00015ADC File Offset: 0x00013CDC
		public bool IsInRange
		{
			get
			{
				return this._isInRange;
			}
			set
			{
				if (this._isInRange != value)
				{
					this._isInRange = value;
				}
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x00015AEE File Offset: 0x00013CEE
		// (set) Token: 0x06000761 RID: 1889 RVA: 0x00015AF6 File Offset: 0x00013CF6
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

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x00015B14 File Offset: 0x00013D14
		// (set) Token: 0x06000763 RID: 1891 RVA: 0x00015B1C File Offset: 0x00013D1C
		public bool HasPort
		{
			get
			{
				return this._hasPort;
			}
			set
			{
				if (value != this._hasPort)
				{
					this._hasPort = value;
					base.OnPropertyChanged(value, "HasPort");
				}
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x00015B3A File Offset: 0x00013D3A
		// (set) Token: 0x06000765 RID: 1893 RVA: 0x00015B42 File Offset: 0x00013D42
		public int RelationType
		{
			get
			{
				return this._relationType;
			}
			set
			{
				if (this._relationType != value)
				{
					this._relationType = value;
					base.OnPropertyChanged(value, "RelationType");
					this.SetNameplateRelationType(value);
				}
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x00015B67 File Offset: 0x00013D67
		// (set) Token: 0x06000767 RID: 1895 RVA: 0x00015B6F File Offset: 0x00013D6F
		public int WSign
		{
			get
			{
				return this._wSign;
			}
			set
			{
				if (this._wSign != value)
				{
					this._wSign = value;
					base.OnPropertyChanged(value, "WSign");
				}
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x00015B8D File Offset: 0x00013D8D
		// (set) Token: 0x06000769 RID: 1897 RVA: 0x00015B95 File Offset: 0x00013D95
		public float WPos
		{
			get
			{
				return this._wPos;
			}
			set
			{
				if (this._wPos != value)
				{
					this._wPos = value;
					base.OnPropertyChanged(value, "WPos");
				}
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x00015BB3 File Offset: 0x00013DB3
		// (set) Token: 0x0600076B RID: 1899 RVA: 0x00015BBB File Offset: 0x00013DBB
		public float DistanceToCamera
		{
			get
			{
				return this._distanceToCamera;
			}
			set
			{
				if (this._distanceToCamera != value)
				{
					this._distanceToCamera = value;
					base.OnPropertyChanged(value, "DistanceToCamera");
				}
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x00015BD9 File Offset: 0x00013DD9
		// (set) Token: 0x0600076D RID: 1901 RVA: 0x00015BE1 File Offset: 0x00013DE1
		public SettlementNameplateItemWidget NameplateItem
		{
			get
			{
				return this._nameplateItem;
			}
			set
			{
				if (this._nameplateItem != value)
				{
					this._nameplateItem = value;
					base.OnPropertyChanged<SettlementNameplateItemWidget>(value, "NameplateItem");
				}
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x00015BFF File Offset: 0x00013DFF
		// (set) Token: 0x0600076F RID: 1903 RVA: 0x00015C08 File Offset: 0x00013E08
		public ListPanel NotificationListPanel
		{
			get
			{
				return this._notificationListPanel;
			}
			set
			{
				if (this._notificationListPanel != value)
				{
					this._notificationListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "NotificationListPanel");
					this._notificationListPanel.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.OnNotificationListUpdated));
					this._notificationListPanel.ItemAfterRemoveEventHandlers.Add(new Action<Widget>(this.OnNotificationListUpdated));
				}
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x00015C69 File Offset: 0x00013E69
		// (set) Token: 0x06000771 RID: 1905 RVA: 0x00015C71 File Offset: 0x00013E71
		public ListPanel EventsListPanel
		{
			get
			{
				return this._eventsListPanel;
			}
			set
			{
				if (value != this._eventsListPanel)
				{
					this._eventsListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "EventsListPanel");
				}
			}
		}

		// Token: 0x04000327 RID: 807
		private float _positionTimer;

		// Token: 0x04000328 RID: 808
		private bool _updatePositionNextFrame;

		// Token: 0x04000329 RID: 809
		private SettlementNameplateWidget.TutorialAnimState _tutorialAnimState;

		// Token: 0x0400032A RID: 810
		private float _lerpThreshold = 5E-05f;

		// Token: 0x0400032B RID: 811
		private float _lerpModifier = 10f;

		// Token: 0x0400032C RID: 812
		private Vector2 _cachedItemSize;

		// Token: 0x0400032D RID: 813
		private bool _lateUpdateActionAdded;

		// Token: 0x0400032E RID: 814
		private Vec2 _position;

		// Token: 0x0400032F RID: 815
		private bool _isVisibleOnMap;

		// Token: 0x04000330 RID: 816
		private bool _isTracked;

		// Token: 0x04000331 RID: 817
		private bool _isInsideWindow;

		// Token: 0x04000332 RID: 818
		private bool _isTargetedByTutorial;

		// Token: 0x04000333 RID: 819
		private int _relationType = -1;

		// Token: 0x04000334 RID: 820
		private int _wSign;

		// Token: 0x04000335 RID: 821
		private float _wPos;

		// Token: 0x04000336 RID: 822
		private float _distanceToCamera;

		// Token: 0x04000337 RID: 823
		private bool _isInRange;

		// Token: 0x04000338 RID: 824
		private bool _canParley;

		// Token: 0x04000339 RID: 825
		private bool _hasPort;

		// Token: 0x0400033A RID: 826
		private SettlementNameplateItemWidget _nameplateItem;

		// Token: 0x0400033B RID: 827
		private ListPanel _notificationListPanel;

		// Token: 0x0400033C RID: 828
		private ListPanel _eventsListPanel;

		// Token: 0x020001B5 RID: 437
		public enum TutorialAnimState
		{
			// Token: 0x040009ED RID: 2541
			Idle,
			// Token: 0x040009EE RID: 2542
			Start,
			// Token: 0x040009EF RID: 2543
			FirstFrame,
			// Token: 0x040009F0 RID: 2544
			Playing
		}
	}
}
