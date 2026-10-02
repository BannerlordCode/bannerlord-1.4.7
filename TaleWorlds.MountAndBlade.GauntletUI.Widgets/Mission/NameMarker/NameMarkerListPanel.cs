using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.NameMarker
{
	// Token: 0x020000F5 RID: 245
	public class NameMarkerListPanel : ListPanel
	{
		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x000226E9 File Offset: 0x000208E9
		// (set) Token: 0x06000C9B RID: 3227 RVA: 0x000226F1 File Offset: 0x000208F1
		public float FarAlphaTarget { get; set; } = 0.2f;

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06000C9C RID: 3228 RVA: 0x000226FA File Offset: 0x000208FA
		// (set) Token: 0x06000C9D RID: 3229 RVA: 0x00022702 File Offset: 0x00020902
		public float FarDistanceCutoff { get; set; } = 50f;

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x0002270B File Offset: 0x0002090B
		// (set) Token: 0x06000C9F RID: 3231 RVA: 0x00022713 File Offset: 0x00020913
		public float CloseDistanceCutoff { get; set; } = 25f;

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x0002271C File Offset: 0x0002091C
		// (set) Token: 0x06000CA1 RID: 3233 RVA: 0x00022724 File Offset: 0x00020924
		public bool HasTypeMarker { get; private set; }

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x0002272D File Offset: 0x0002092D
		// (set) Token: 0x06000CA3 RID: 3235 RVA: 0x00022735 File Offset: 0x00020935
		public MarkerRect Rect { get; private set; }

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x0002273E File Offset: 0x0002093E
		// (set) Token: 0x06000CA5 RID: 3237 RVA: 0x00022746 File Offset: 0x00020946
		public bool IsInScreenBoundaries { get; private set; }

		// Token: 0x06000CA6 RID: 3238 RVA: 0x00022750 File Offset: 0x00020950
		public NameMarkerListPanel(UIContext context)
			: base(context)
		{
			this._parentScreenWidget = base.EventManager.Root.GetChild(0).GetChild(0);
			this.Rect = new MarkerRect();
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x000227C4 File Offset: 0x000209C4
		public void Update(float dt)
		{
			this._transitionDT = MathF.Clamp(dt * 12f, 0f, 1f);
			this._targetAlpha = ((this.IsMarkerEnabled || this.IsMarkerPersistent) ? this.GetDistanceRelatedAlphaTarget(this.Distance) : 0f);
			this.ApplyActionForThisAndAllChildren(new Action<Widget>(this.UpdateAlpha));
			TextWidget nameTextWidget = this.NameTextWidget;
			if ((nameTextWidget != null && nameTextWidget.IsVisible) || this.TypeVisualWidget.IsVisible)
			{
				base.ScaledPositionYOffset = this.Position.y - base.Size.Y / 2f;
				base.ScaledPositionXOffset = this.Position.x - base.Size.X / 2f;
			}
			this.UpdateRectangle();
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x00022898 File Offset: 0x00020A98
		private void UpdateAlpha(Widget item)
		{
			if ((item == this.NameTextWidget || item == this.DistanceTextWidget || item == this.DistanceIconWidget) && this.HasTypeMarker && !this.IsFocused)
			{
				return;
			}
			float num = this.LocalLerp(item.AlphaFactor, this._targetAlpha, this._transitionDT);
			item.SetAlpha(num);
			item.IsVisible = (double)item.AlphaFactor > 0.05;
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0002290C File Offset: 0x00020B0C
		public void UpdateRectangle()
		{
			this.Rect.Reset();
			this.Rect.UpdatePoints(base.ScaledPositionXOffset, base.ScaledPositionXOffset + base.Size.X, base.ScaledPositionYOffset, base.ScaledPositionYOffset + base.Size.Y);
			this.IsInScreenBoundaries = this.Rect.Left > -50f && this.Rect.Right < base.EventManager.PageSize.X + 50f && this.Rect.Top > -50f && this.Rect.Bottom < base.EventManager.PageSize.Y + 50f;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x000229D4 File Offset: 0x00020BD4
		private float GetDistanceRelatedAlphaTarget(int distance)
		{
			if (this.IsFocused)
			{
				return 1f;
			}
			if ((float)distance > this.FarDistanceCutoff)
			{
				return this.FarAlphaTarget;
			}
			if ((float)distance <= this.FarDistanceCutoff && (float)distance >= this.CloseDistanceCutoff)
			{
				float num = (float)Math.Pow((double)(((float)distance - this.CloseDistanceCutoff) / (this.FarDistanceCutoff - this.CloseDistanceCutoff)), 0.3333333333333333);
				return MathF.Clamp(MathF.Lerp(1f, this.FarAlphaTarget, num, 1E-05f), this.FarAlphaTarget, 1f);
			}
			return 1f;
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x00022A68 File Offset: 0x00020C68
		private float LocalLerp(float start, float end, float delta)
		{
			if (Math.Abs(start - end) > 1E-45f)
			{
				return (end - start) * delta + start;
			}
			return end;
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00022A84 File Offset: 0x00020C84
		private void OnStateChanged()
		{
			if (this.NameTextWidget != null)
			{
				this.NameTextWidget.SetState(this.NameType);
			}
			if (this.TypeVisualWidget != null)
			{
				this.TypeVisualWidget.SetState(this.IconType);
			}
			this.HasTypeMarker = this.IconType != string.Empty;
			if (this.HasTypeMarker && this.IsFocused)
			{
				TextWidget nameTextWidget = this.NameTextWidget;
				if (nameTextWidget != null)
				{
					nameTextWidget.SetAlpha(1f);
				}
				TextWidget distanceTextWidget = this.DistanceTextWidget;
				if (distanceTextWidget != null)
				{
					distanceTextWidget.SetAlpha(1f);
				}
				BrushWidget distanceIconWidget = this.DistanceIconWidget;
				if (distanceIconWidget != null)
				{
					distanceIconWidget.SetAlpha(1f);
				}
			}
			else if (this.HasTypeMarker && !this.IsFocused)
			{
				TextWidget nameTextWidget2 = this.NameTextWidget;
				if (nameTextWidget2 != null)
				{
					nameTextWidget2.SetAlpha(0f);
				}
				TextWidget distanceTextWidget2 = this.DistanceTextWidget;
				if (distanceTextWidget2 != null)
				{
					distanceTextWidget2.SetAlpha(0f);
				}
				BrushWidget distanceIconWidget2 = this.DistanceIconWidget;
				if (distanceIconWidget2 != null)
				{
					distanceIconWidget2.SetAlpha(0f);
				}
			}
			if (this.IsEnemy)
			{
				this.TypeVisualWidget.Brush.GlobalColor = this.EnemyColor;
			}
			else if (this.IsFriendly)
			{
				this.TypeVisualWidget.Brush.GlobalColor = this.FriendlyColor;
			}
			else if (this.HasMainQuest)
			{
				this.TypeVisualWidget.Brush.GlobalColor = this.MainQuestNotificationColor;
			}
			else if (this.HasIssue)
			{
				this.TypeVisualWidget.Brush.GlobalColor = this.IssueNotificationColor;
			}
			BrushWidget typeVisualWidget = this.TypeVisualWidget;
			Sprite sprite;
			if (typeVisualWidget == null)
			{
				sprite = null;
			}
			else
			{
				Style style = typeVisualWidget.Brush.GetStyle(this.IconType);
				if (style == null)
				{
					sprite = null;
				}
				else
				{
					StyleLayer layer = style.GetLayer(0);
					sprite = ((layer != null) ? layer.Sprite : null);
				}
			}
			Sprite sprite2 = sprite;
			if (sprite2 != null)
			{
				base.SuggestedWidth = base.SuggestedHeight / (float)sprite2.Height * (float)sprite2.Width;
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06000CAD RID: 3245 RVA: 0x00022C53 File Offset: 0x00020E53
		// (set) Token: 0x06000CAE RID: 3246 RVA: 0x00022C5B File Offset: 0x00020E5B
		[DataSourceProperty]
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
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x00022C7F File Offset: 0x00020E7F
		// (set) Token: 0x06000CB0 RID: 3248 RVA: 0x00022C87 File Offset: 0x00020E87
		[DataSourceProperty]
		public BrushWidget TypeVisualWidget
		{
			get
			{
				return this._typeVisualWidget;
			}
			set
			{
				if (this._typeVisualWidget != value)
				{
					this._typeVisualWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "TypeVisualWidget");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x00022CAB File Offset: 0x00020EAB
		// (set) Token: 0x06000CB2 RID: 3250 RVA: 0x00022CB3 File Offset: 0x00020EB3
		[DataSourceProperty]
		public BrushWidget DistanceIconWidget
		{
			get
			{
				return this._distanceIconWidget;
			}
			set
			{
				if (this._distanceIconWidget != value)
				{
					this._distanceIconWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "DistanceIconWidget");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x00022CD7 File Offset: 0x00020ED7
		// (set) Token: 0x06000CB4 RID: 3252 RVA: 0x00022CDF File Offset: 0x00020EDF
		[DataSourceProperty]
		public TextWidget DistanceTextWidget
		{
			get
			{
				return this._distanceTextWidget;
			}
			set
			{
				if (this._distanceTextWidget != value)
				{
					this._distanceTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "DistanceTextWidget");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x00022D03 File Offset: 0x00020F03
		// (set) Token: 0x06000CB6 RID: 3254 RVA: 0x00022D0B File Offset: 0x00020F0B
		[DataSourceProperty]
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

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x00022D2E File Offset: 0x00020F2E
		// (set) Token: 0x06000CB8 RID: 3256 RVA: 0x00022D36 File Offset: 0x00020F36
		[Editor(false)]
		public Color IssueNotificationColor
		{
			get
			{
				return this._issueNotificationColor;
			}
			set
			{
				if (value != this._issueNotificationColor)
				{
					this._issueNotificationColor = value;
					base.OnPropertyChanged(value, "IssueNotificationColor");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x00022D5F File Offset: 0x00020F5F
		// (set) Token: 0x06000CBA RID: 3258 RVA: 0x00022D67 File Offset: 0x00020F67
		[Editor(false)]
		public Color MainQuestNotificationColor
		{
			get
			{
				return this._mainQuestNotificationColor;
			}
			set
			{
				if (value != this._mainQuestNotificationColor)
				{
					this._mainQuestNotificationColor = value;
					base.OnPropertyChanged(value, "MainQuestNotificationColor");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x00022D90 File Offset: 0x00020F90
		// (set) Token: 0x06000CBC RID: 3260 RVA: 0x00022D98 File Offset: 0x00020F98
		[Editor(false)]
		public Color EnemyColor
		{
			get
			{
				return this._enemyColor;
			}
			set
			{
				if (value != this._enemyColor)
				{
					this._enemyColor = value;
					base.OnPropertyChanged(value, "EnemyColor");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x00022DC1 File Offset: 0x00020FC1
		// (set) Token: 0x06000CBE RID: 3262 RVA: 0x00022DC9 File Offset: 0x00020FC9
		[Editor(false)]
		public Color FriendlyColor
		{
			get
			{
				return this._friendlyColor;
			}
			set
			{
				if (value != this._friendlyColor)
				{
					this._friendlyColor = value;
					base.OnPropertyChanged(value, "FriendlyColor");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x00022DF2 File Offset: 0x00020FF2
		// (set) Token: 0x06000CC0 RID: 3264 RVA: 0x00022DFA File Offset: 0x00020FFA
		[Editor(false)]
		public string IconType
		{
			get
			{
				return this._iconType;
			}
			set
			{
				if (value != this._iconType)
				{
					this._iconType = value;
					base.OnPropertyChanged<string>(value, "IconType");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x00022E23 File Offset: 0x00021023
		// (set) Token: 0x06000CC2 RID: 3266 RVA: 0x00022E2B File Offset: 0x0002102B
		[Editor(false)]
		public string NameType
		{
			get
			{
				return this._nameType;
			}
			set
			{
				if (value != this._nameType)
				{
					this._nameType = value;
					base.OnPropertyChanged<string>(value, "NameType");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x00022E54 File Offset: 0x00021054
		// (set) Token: 0x06000CC4 RID: 3268 RVA: 0x00022E5C File Offset: 0x0002105C
		[DataSourceProperty]
		public int Distance
		{
			get
			{
				return this._distance;
			}
			set
			{
				if (this._distance != value)
				{
					this._distance = value;
					base.OnPropertyChanged(value, "Distance");
				}
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x00022E7A File Offset: 0x0002107A
		// (set) Token: 0x06000CC6 RID: 3270 RVA: 0x00022E82 File Offset: 0x00021082
		[DataSourceProperty]
		public bool IsMarkerEnabled
		{
			get
			{
				return this._isMarkerEnabled;
			}
			set
			{
				if (this._isMarkerEnabled != value)
				{
					this._isMarkerEnabled = value;
					base.OnPropertyChanged(value, "IsMarkerEnabled");
				}
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x00022EA0 File Offset: 0x000210A0
		// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x00022EA8 File Offset: 0x000210A8
		[DataSourceProperty]
		public bool IsMarkerPersistent
		{
			get
			{
				return this._isMarkerPersistent;
			}
			set
			{
				if (this._isMarkerPersistent != value)
				{
					this._isMarkerPersistent = value;
					base.OnPropertyChanged(value, "IsMarkerPersistent");
				}
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x00022EC6 File Offset: 0x000210C6
		// (set) Token: 0x06000CCA RID: 3274 RVA: 0x00022ECE File Offset: 0x000210CE
		[DataSourceProperty]
		public bool HasIssue
		{
			get
			{
				return this._hasIssue;
			}
			set
			{
				if (this._hasIssue != value)
				{
					this._hasIssue = value;
					base.OnPropertyChanged(value, "HasIssue");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x00022EF2 File Offset: 0x000210F2
		// (set) Token: 0x06000CCC RID: 3276 RVA: 0x00022EFA File Offset: 0x000210FA
		[DataSourceProperty]
		public bool HasMainQuest
		{
			get
			{
				return this._hasMainQuest;
			}
			set
			{
				if (this._hasMainQuest != value)
				{
					this._hasMainQuest = value;
					base.OnPropertyChanged(value, "HasMainQuest");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x00022F1E File Offset: 0x0002111E
		// (set) Token: 0x06000CCE RID: 3278 RVA: 0x00022F26 File Offset: 0x00021126
		[DataSourceProperty]
		public bool IsEnemy
		{
			get
			{
				return this._isEnemy;
			}
			set
			{
				if (this._isEnemy != value)
				{
					this._isEnemy = value;
					base.OnPropertyChanged(value, "IsEnemy");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x00022F4A File Offset: 0x0002114A
		// (set) Token: 0x06000CD0 RID: 3280 RVA: 0x00022F52 File Offset: 0x00021152
		[DataSourceProperty]
		public bool IsFriendly
		{
			get
			{
				return this._isFriendly;
			}
			set
			{
				if (this._isFriendly != value)
				{
					this._isFriendly = value;
					base.OnPropertyChanged(value, "IsFriendly");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x00022F76 File Offset: 0x00021176
		// (set) Token: 0x06000CD2 RID: 3282 RVA: 0x00022F80 File Offset: 0x00021180
		[Editor(false)]
		public new bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
			set
			{
				if (value != this._isFocused)
				{
					this._isFocused = value;
					base.OnPropertyChanged(value, "IsFocused");
					if (!value && (this.IsMarkerEnabled || this.IsMarkerPersistent))
					{
						TextWidget nameTextWidget = this.NameTextWidget;
						if (nameTextWidget != null)
						{
							nameTextWidget.SetAlpha(0f);
						}
						TextWidget distanceTextWidget = this.DistanceTextWidget;
						if (distanceTextWidget != null)
						{
							distanceTextWidget.SetAlpha(0f);
						}
						BrushWidget distanceIconWidget = this.DistanceIconWidget;
						if (distanceIconWidget != null)
						{
							distanceIconWidget.SetAlpha(0f);
						}
					}
					else if (value && (this.IsMarkerEnabled || this.IsMarkerPersistent))
					{
						TextWidget nameTextWidget2 = this.NameTextWidget;
						if (nameTextWidget2 != null)
						{
							nameTextWidget2.SetAlpha(1f);
						}
						TextWidget distanceTextWidget2 = this.DistanceTextWidget;
						if (distanceTextWidget2 != null)
						{
							distanceTextWidget2.SetAlpha(1f);
						}
						BrushWidget distanceIconWidget2 = this.DistanceIconWidget;
						if (distanceIconWidget2 != null)
						{
							distanceIconWidget2.SetAlpha(1f);
						}
					}
					base.RenderLate = value;
				}
			}
		}

		// Token: 0x040005B6 RID: 1462
		private Widget _parentScreenWidget;

		// Token: 0x040005BA RID: 1466
		private const float BoundaryOffset = 50f;

		// Token: 0x040005BB RID: 1467
		private float _transitionDT;

		// Token: 0x040005BC RID: 1468
		private float _targetAlpha;

		// Token: 0x040005BD RID: 1469
		private string _iconType = string.Empty;

		// Token: 0x040005BE RID: 1470
		private string _nameType = string.Empty;

		// Token: 0x040005BF RID: 1471
		private int _distance;

		// Token: 0x040005C0 RID: 1472
		private TextWidget _nameTextWidget;

		// Token: 0x040005C1 RID: 1473
		private BrushWidget _typeVisualWidget;

		// Token: 0x040005C2 RID: 1474
		private BrushWidget _distanceIconWidget;

		// Token: 0x040005C3 RID: 1475
		private TextWidget _distanceTextWidget;

		// Token: 0x040005C4 RID: 1476
		private Vec2 _position;

		// Token: 0x040005C5 RID: 1477
		private Color _issueNotificationColor;

		// Token: 0x040005C6 RID: 1478
		private Color _mainQuestNotificationColor;

		// Token: 0x040005C7 RID: 1479
		private Color _enemyColor;

		// Token: 0x040005C8 RID: 1480
		private Color _friendlyColor;

		// Token: 0x040005C9 RID: 1481
		private bool _isMarkerEnabled;

		// Token: 0x040005CA RID: 1482
		private bool _isMarkerPersistent;

		// Token: 0x040005CB RID: 1483
		private bool _hasIssue;

		// Token: 0x040005CC RID: 1484
		private bool _hasMainQuest;

		// Token: 0x040005CD RID: 1485
		private bool _isEnemy;

		// Token: 0x040005CE RID: 1486
		private bool _isFriendly;

		// Token: 0x040005CF RID: 1487
		private bool _isFocused;
	}
}
