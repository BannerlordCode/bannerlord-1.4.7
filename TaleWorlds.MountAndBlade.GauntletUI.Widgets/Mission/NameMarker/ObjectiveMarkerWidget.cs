using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.NameMarker
{
	// Token: 0x020000F9 RID: 249
	public class ObjectiveMarkerWidget : Widget
	{
		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x00023A8F File Offset: 0x00021C8F
		public bool IsCombinedWithOtherMarkers
		{
			get
			{
				return this.CombinedSiblingsCount > 0;
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x00023A9A File Offset: 0x00021C9A
		// (set) Token: 0x06000CFF RID: 3327 RVA: 0x00023AA2 File Offset: 0x00021CA2
		public float FarAlphaTarget { get; set; } = 0.2f;

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x00023AAB File Offset: 0x00021CAB
		// (set) Token: 0x06000D01 RID: 3329 RVA: 0x00023AB3 File Offset: 0x00021CB3
		public float FarDistanceCutoff { get; set; } = 50f;

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00023ABC File Offset: 0x00021CBC
		// (set) Token: 0x06000D03 RID: 3331 RVA: 0x00023AC4 File Offset: 0x00021CC4
		public float CloseDistanceCutoff { get; set; } = 25f;

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x00023ACD File Offset: 0x00021CCD
		// (set) Token: 0x06000D05 RID: 3333 RVA: 0x00023AD5 File Offset: 0x00021CD5
		public MarkerRect Rect { get; private set; }

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x00023ADE File Offset: 0x00021CDE
		// (set) Token: 0x06000D07 RID: 3335 RVA: 0x00023AE6 File Offset: 0x00021CE6
		public bool IsInScreenBoundaries { get; private set; }

		// Token: 0x06000D08 RID: 3336 RVA: 0x00023AEF File Offset: 0x00021CEF
		public ObjectiveMarkerWidget(UIContext context)
			: base(context)
		{
			this.Rect = new MarkerRect();
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x00023B24 File Offset: 0x00021D24
		public void Update(float dt)
		{
			float transitionRatio = MathF.Clamp(dt * 12f, 0f, 1f);
			float num = ((this.IsMarkerEnabled && this.IsMarkerActive) ? this.GetDistanceRelatedAlphaTarget(this.Distance) : 0f);
			float distanceAlpha = ((this.IsFocused && (!this.IsCombinedWithOtherMarkers || this.IsMainCombinationMarker)) ? num : 0f);
			float num2 = ((this.IsFocused && !this.IsCombinedWithOtherMarkers) ? num : 0f);
			this.DistanceContainerWidget.ApplyActionForThisAndAllChildren(delegate(Widget w)
			{
				ObjectiveMarkerWidget.UpdateAlpha(w, distanceAlpha, transitionRatio);
			});
			ObjectiveMarkerWidget.UpdateAlpha(this.NameTextWidget, num2, transitionRatio);
			ObjectiveMarkerWidget.UpdateAlpha(this.QuestIconWidget, num, transitionRatio);
			ObjectiveMarkerWidget.UpdateAlpha(this.CombinationCountWidget, num, transitionRatio);
			base.ScaledPositionYOffset = this.Position.Y - base.Size.Y / 2f;
			base.ScaledPositionXOffset = this.Position.X - base.Size.X / 2f;
			this.CombinationCountWidget.Text = (this.IsMainCombinationMarker ? (this.CombinedSiblingsCount + 1).ToString() : string.Empty);
			Vec2 vec = (this.IsCombinedWithOtherMarkers ? (this.CombinedAveragePosition - this.Position) : Vec2.Zero);
			this.MainContainer.ScaledPositionXOffset = MathF.Lerp(this.MainContainer.ScaledPositionXOffset, vec.x, transitionRatio, 1E-05f);
			this.MainContainer.ScaledPositionYOffset = MathF.Lerp(this.MainContainer.ScaledPositionYOffset, vec.y, transitionRatio, 1E-05f);
			this.UpdateRectangle();
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x00023CFC File Offset: 0x00021EFC
		private static void UpdateAlpha(Widget item, float targetAlpha, float transitionRatio)
		{
			if (item == null)
			{
				return;
			}
			float num = ObjectiveMarkerWidget.LocalLerp(item.AlphaFactor, targetAlpha, transitionRatio);
			item.SetAlpha(num);
			item.IsVisible = num > 1E-05f;
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00023D30 File Offset: 0x00021F30
		public void UpdateRectangle()
		{
			this.Rect.Reset();
			this.Rect.UpdatePoints(base.ScaledPositionXOffset, base.ScaledPositionXOffset + base.Size.X, base.ScaledPositionYOffset, base.ScaledPositionYOffset + base.Size.Y);
			this.IsInScreenBoundaries = this.Rect.Left > -50f && this.Rect.Right < base.EventManager.PageSize.X + 50f && this.Rect.Top > -50f && this.Rect.Bottom < base.EventManager.PageSize.Y + 50f;
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x00023DF8 File Offset: 0x00021FF8
		private float GetDistanceRelatedAlphaTarget(int distance)
		{
			if (this.IsCombinedWithOtherMarkers)
			{
				if (this.IsMainCombinationMarker)
				{
					return 1f;
				}
				if ((this.CombinedAveragePosition - this.Position).Distance(new Vec2(this.MainContainer.ScaledPositionXOffset, this.MainContainer.ScaledPositionYOffset)) <= 5f)
				{
					return 0f;
				}
				return 1f;
			}
			else
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
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x00023EEB File Offset: 0x000220EB
		private static float LocalLerp(float start, float end, float delta)
		{
			if (Math.Abs(start - end) > 1E-45f)
			{
				return (end - start) * delta + start;
			}
			return end;
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x00023F05 File Offset: 0x00022105
		private void OnStateChanged()
		{
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06000D0F RID: 3343 RVA: 0x00023F07 File Offset: 0x00022107
		// (set) Token: 0x06000D10 RID: 3344 RVA: 0x00023F0F File Offset: 0x0002210F
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

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06000D11 RID: 3345 RVA: 0x00023F33 File Offset: 0x00022133
		// (set) Token: 0x06000D12 RID: 3346 RVA: 0x00023F3B File Offset: 0x0002213B
		[DataSourceProperty]
		public TextWidget CombinationCountWidget
		{
			get
			{
				return this._combinationCountWidget;
			}
			set
			{
				if (this._combinationCountWidget != value)
				{
					this._combinationCountWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "CombinationCountWidget");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06000D13 RID: 3347 RVA: 0x00023F5F File Offset: 0x0002215F
		// (set) Token: 0x06000D14 RID: 3348 RVA: 0x00023F67 File Offset: 0x00022167
		[DataSourceProperty]
		public Widget QuestIconWidget
		{
			get
			{
				return this._questIconWidget;
			}
			set
			{
				if (this._questIconWidget != value)
				{
					this._questIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "QuestIconWidget");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x00023F8B File Offset: 0x0002218B
		// (set) Token: 0x06000D16 RID: 3350 RVA: 0x00023F93 File Offset: 0x00022193
		[DataSourceProperty]
		public Widget MainContainer
		{
			get
			{
				return this._mainContainer;
			}
			set
			{
				if (this._mainContainer != value)
				{
					this._mainContainer = value;
					base.OnPropertyChanged<Widget>(value, "MainContainer");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06000D17 RID: 3351 RVA: 0x00023FB7 File Offset: 0x000221B7
		// (set) Token: 0x06000D18 RID: 3352 RVA: 0x00023FBF File Offset: 0x000221BF
		[DataSourceProperty]
		public Widget DistanceContainerWidget
		{
			get
			{
				return this._distanceContainerWidget;
			}
			set
			{
				if (this._distanceContainerWidget != value)
				{
					this._distanceContainerWidget = value;
					base.OnPropertyChanged<Widget>(value, "DistanceContainerWidget");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x00023FE3 File Offset: 0x000221E3
		// (set) Token: 0x06000D1A RID: 3354 RVA: 0x00023FEB File Offset: 0x000221EB
		[DataSourceProperty]
		public Widget DistanceIconWidget
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
					base.OnPropertyChanged<Widget>(value, "DistanceIconWidget");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x0002400F File Offset: 0x0002220F
		// (set) Token: 0x06000D1C RID: 3356 RVA: 0x00024017 File Offset: 0x00022217
		[DataSourceProperty]
		public Widget DistanceTextWidget
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
					base.OnPropertyChanged<Widget>(value, "DistanceTextWidget");
					this.OnStateChanged();
				}
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x0002403B File Offset: 0x0002223B
		// (set) Token: 0x06000D1E RID: 3358 RVA: 0x00024043 File Offset: 0x00022243
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

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x00024066 File Offset: 0x00022266
		// (set) Token: 0x06000D20 RID: 3360 RVA: 0x0002406E File Offset: 0x0002226E
		[DataSourceProperty]
		public Vec2 CombinedAveragePosition
		{
			get
			{
				return this._combinedAveragePosition;
			}
			set
			{
				if (this._combinedAveragePosition != value)
				{
					this._combinedAveragePosition = value;
					base.OnPropertyChanged(value, "CombinedAveragePosition");
				}
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06000D21 RID: 3361 RVA: 0x00024091 File Offset: 0x00022291
		// (set) Token: 0x06000D22 RID: 3362 RVA: 0x00024099 File Offset: 0x00022299
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

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x000240B7 File Offset: 0x000222B7
		// (set) Token: 0x06000D24 RID: 3364 RVA: 0x000240BF File Offset: 0x000222BF
		[DataSourceProperty]
		public int CombinedSiblingsCount
		{
			get
			{
				return this._combinedSiblingsCount;
			}
			set
			{
				if (this._combinedSiblingsCount != value)
				{
					this._combinedSiblingsCount = value;
					base.OnPropertyChanged(value, "CombinedSiblingsCount");
				}
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06000D25 RID: 3365 RVA: 0x000240DD File Offset: 0x000222DD
		// (set) Token: 0x06000D26 RID: 3366 RVA: 0x000240E5 File Offset: 0x000222E5
		[DataSourceProperty]
		public bool IsMainCombinationMarker
		{
			get
			{
				return this._isMainCombinationMarker;
			}
			set
			{
				if (this._isMainCombinationMarker != value)
				{
					this._isMainCombinationMarker = value;
					base.OnPropertyChanged(value, "IsMainCombinationMarker");
				}
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x00024103 File Offset: 0x00022303
		// (set) Token: 0x06000D28 RID: 3368 RVA: 0x0002410B File Offset: 0x0002230B
		[DataSourceProperty]
		public bool IsDistanceRelevant
		{
			get
			{
				return this._isDistanceRelevant;
			}
			set
			{
				if (this._isDistanceRelevant != value)
				{
					this._isDistanceRelevant = value;
					base.OnPropertyChanged(value, "IsDistanceRelevant");
				}
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06000D29 RID: 3369 RVA: 0x00024129 File Offset: 0x00022329
		// (set) Token: 0x06000D2A RID: 3370 RVA: 0x00024131 File Offset: 0x00022331
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

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06000D2B RID: 3371 RVA: 0x0002414F File Offset: 0x0002234F
		// (set) Token: 0x06000D2C RID: 3372 RVA: 0x00024157 File Offset: 0x00022357
		[DataSourceProperty]
		public bool IsMarkerActive
		{
			get
			{
				return this._isMarkerActive;
			}
			set
			{
				if (this._isMarkerActive != value)
				{
					this._isMarkerActive = value;
					base.OnPropertyChanged(value, "IsMarkerActive");
				}
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06000D2D RID: 3373 RVA: 0x00024175 File Offset: 0x00022375
		// (set) Token: 0x06000D2E RID: 3374 RVA: 0x0002417D File Offset: 0x0002237D
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
					base.RenderLate = value;
				}
			}
		}

		// Token: 0x040005E6 RID: 1510
		private const float BoundaryOffset = 50f;

		// Token: 0x040005E7 RID: 1511
		private int _distance;

		// Token: 0x040005E8 RID: 1512
		private int _combinedSiblingsCount;

		// Token: 0x040005E9 RID: 1513
		private TextWidget _nameTextWidget;

		// Token: 0x040005EA RID: 1514
		private TextWidget _combinationCountWidget;

		// Token: 0x040005EB RID: 1515
		private Widget _mainContainer;

		// Token: 0x040005EC RID: 1516
		private Widget _questIconWidget;

		// Token: 0x040005ED RID: 1517
		private Widget _distanceContainerWidget;

		// Token: 0x040005EE RID: 1518
		private Widget _distanceIconWidget;

		// Token: 0x040005EF RID: 1519
		private Widget _distanceTextWidget;

		// Token: 0x040005F0 RID: 1520
		private Vec2 _position;

		// Token: 0x040005F1 RID: 1521
		private Vec2 _combinedAveragePosition;

		// Token: 0x040005F2 RID: 1522
		private bool _isMainCombinationMarker;

		// Token: 0x040005F3 RID: 1523
		private bool _isDistanceRelevant;

		// Token: 0x040005F4 RID: 1524
		private bool _isMarkerEnabled;

		// Token: 0x040005F5 RID: 1525
		private bool _isMarkerActive;

		// Token: 0x040005F6 RID: 1526
		private bool _isFocused;
	}
}
