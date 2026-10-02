using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000DF RID: 223
	public class FormationMarkerListPanel : ListPanel
	{
		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06000B6E RID: 2926 RVA: 0x0001FC55 File Offset: 0x0001DE55
		// (set) Token: 0x06000B6F RID: 2927 RVA: 0x0001FC5D File Offset: 0x0001DE5D
		public float FarAlphaTarget { get; set; } = 0.2f;

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06000B70 RID: 2928 RVA: 0x0001FC66 File Offset: 0x0001DE66
		// (set) Token: 0x06000B71 RID: 2929 RVA: 0x0001FC6E File Offset: 0x0001DE6E
		public float FarDistanceCutoff { get; set; } = 50f;

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x0001FC77 File Offset: 0x0001DE77
		// (set) Token: 0x06000B73 RID: 2931 RVA: 0x0001FC7F File Offset: 0x0001DE7F
		public float CloseDistanceCutoff { get; set; } = 25f;

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06000B74 RID: 2932 RVA: 0x0001FC88 File Offset: 0x0001DE88
		// (set) Token: 0x06000B75 RID: 2933 RVA: 0x0001FC90 File Offset: 0x0001DE90
		public float ClosestFadeoutRange { get; set; } = 3f;

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06000B76 RID: 2934 RVA: 0x0001FC99 File Offset: 0x0001DE99
		// (set) Token: 0x06000B77 RID: 2935 RVA: 0x0001FCA1 File Offset: 0x0001DEA1
		public float FarScaleTarget { get; set; } = 0.5f;

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06000B78 RID: 2936 RVA: 0x0001FCAA File Offset: 0x0001DEAA
		// (set) Token: 0x06000B79 RID: 2937 RVA: 0x0001FCB2 File Offset: 0x0001DEB2
		public float CloseScaleTarget { get; set; } = 1.4f;

		// Token: 0x06000B7A RID: 2938 RVA: 0x0001FCBC File Offset: 0x0001DEBC
		public FormationMarkerListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x0001FD2C File Offset: 0x0001DF2C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			float num = MathF.Clamp(dt * 12f, 0f, 1f);
			if (this._isMarkersDirty)
			{
				Sprite sprite = null;
				if (!string.IsNullOrEmpty(this.MarkerType) && this.IconBrush != null)
				{
					BrushLayer layer = this.IconBrush.GetLayer(this.MarkerType);
					sprite = ((layer != null) ? layer.Sprite : null);
				}
				if (sprite != null && this.FormationTypeMarker != null)
				{
					this.FormationTypeMarker.Sprite = sprite;
				}
				else
				{
					Debug.FailedAssert("Couldn't find formation marker type image", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Mission\\FormationMarkerListPanel.cs", "OnLateUpdate", 51);
				}
				if (this.TeamTypeMarker != null)
				{
					this.TeamTypeMarker.RegisterBrushStatesOfWidget();
					if (this.TeamType == 0)
					{
						this.TeamTypeMarker.SetState("Player");
					}
					else if (this.TeamType == 1)
					{
						this.TeamTypeMarker.SetState("Ally");
					}
					else
					{
						this.TeamTypeMarker.SetState("Enemy");
					}
				}
				this._isMarkersDirty = false;
			}
			if (this.IsMarkerEnabled)
			{
				float num2 = this.GetDistanceRelatedAlphaTarget(this.Distance);
				if (!this.IsActive)
				{
					num2 *= 0.5f;
				}
				this.SetGlobalAlphaRecursively(num2);
				if (!this._markerDefaultSize.IsValid)
				{
					this._markerDefaultSize = new Vec2(this.TeamTypeMarker.SuggestedWidth, this.TeamTypeMarker.SuggestedHeight);
				}
				float distanceRelatedScale = this.GetDistanceRelatedScale(this.Distance);
				this.TeamTypeMarker.SuggestedWidth = this._markerDefaultSize.X * distanceRelatedScale;
				this.TeamTypeMarker.SuggestedHeight = this._markerDefaultSize.Y * distanceRelatedScale;
			}
			else
			{
				float num3 = MathF.Lerp(base.AlphaFactor, 0f, num, 1E-05f);
				this.SetGlobalAlphaRecursively(num3);
			}
			if ((double)base.AlphaFactor > 0.05)
			{
				this.UpdateScreenPosition();
				return;
			}
			base.IsVisible = false;
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x0001FF04 File Offset: 0x0001E104
		private void UpdateScreenPosition()
		{
			float num = this.Position.X - base.Size.X / 2f;
			float num2 = this.Position.X + base.Size.X / 2f;
			float num3 = this.Position.Y - base.Size.Y / 2f;
			float num4 = this.Position.Y + base.Size.Y / 2f;
			bool flag = this.WSign > 0 && num > 0f && num2 < base.Context.EventManager.PageSize.X && num3 > 0f && num4 < base.Context.EventManager.PageSize.Y;
			bool flag2 = this.WSign > 0 && (num2 > 0f || num < base.Context.EventManager.PageSize.X) && (num4 > 0f || num3 < base.Context.EventManager.PageSize.Y);
			if (!flag && this.IsTargetingAFormation)
			{
				base.IsVisible = true;
				Vec2 vec = new Vec2(num, num3);
				Vector2 vector = base.Context.EventManager.PageSize - base.Size;
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
				return;
			}
			if (flag || flag2)
			{
				base.IsVisible = true;
				base.ScaledPositionXOffset = num;
				base.ScaledPositionYOffset = num3;
				return;
			}
			base.IsVisible = false;
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x000201E8 File Offset: 0x0001E3E8
		private float GetDistanceRelatedScale(float distance)
		{
			if (this.ShowDistanceTexts)
			{
				return 1f;
			}
			if (distance > this.FarDistanceCutoff)
			{
				return this.FarScaleTarget;
			}
			if (distance <= this.FarDistanceCutoff && distance >= this.CloseDistanceCutoff)
			{
				float num = (float)Math.Pow((double)((distance - this.CloseDistanceCutoff) / (this.FarDistanceCutoff - this.CloseDistanceCutoff)), 0.3333333333333333);
				return MathF.Clamp(MathF.Lerp(this.CloseScaleTarget, this.FarScaleTarget, num, 1E-05f), this.FarScaleTarget, this.CloseScaleTarget);
			}
			return this.CloseScaleTarget;
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0002027C File Offset: 0x0001E47C
		private float GetDistanceRelatedAlphaTarget(float distance)
		{
			if (distance > this.FarDistanceCutoff)
			{
				return this.FarAlphaTarget;
			}
			if (distance <= this.FarDistanceCutoff && distance >= this.CloseDistanceCutoff)
			{
				float num = (float)Math.Pow((double)((distance - this.CloseDistanceCutoff) / (this.FarDistanceCutoff - this.CloseDistanceCutoff)), 0.3333333333333333);
				return MathF.Clamp(MathF.Lerp(1f, this.FarAlphaTarget, num, 1E-05f), this.FarAlphaTarget, 1f);
			}
			if (distance < this.CloseDistanceCutoff && distance > this.CloseDistanceCutoff - this.ClosestFadeoutRange)
			{
				float num2 = (distance - (this.CloseDistanceCutoff - this.ClosestFadeoutRange)) / this.ClosestFadeoutRange;
				return MathF.Lerp(0f, 1f, num2, 1E-05f);
			}
			return 0f;
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x00020344 File Offset: 0x0001E544
		// (set) Token: 0x06000B80 RID: 2944 RVA: 0x0002034C File Offset: 0x0001E54C
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

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x0002036A File Offset: 0x0001E56A
		// (set) Token: 0x06000B82 RID: 2946 RVA: 0x00020372 File Offset: 0x0001E572
		[DataSourceProperty]
		public bool IsTargetingAFormation
		{
			get
			{
				return this._isTargetingAFormation;
			}
			set
			{
				if (this._isTargetingAFormation != value)
				{
					this._isTargetingAFormation = value;
					base.OnPropertyChanged(value, "IsTargetingAFormation");
				}
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06000B83 RID: 2947 RVA: 0x00020390 File Offset: 0x0001E590
		// (set) Token: 0x06000B84 RID: 2948 RVA: 0x00020398 File Offset: 0x0001E598
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (this._isActive != value)
				{
					this._isActive = value;
					base.OnPropertyChanged(value, "IsActive");
				}
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x000203B6 File Offset: 0x0001E5B6
		// (set) Token: 0x06000B86 RID: 2950 RVA: 0x000203BE File Offset: 0x0001E5BE
		[DataSourceProperty]
		public bool ShowDistanceTexts
		{
			get
			{
				return this._showDistanceTexts;
			}
			set
			{
				if (this._showDistanceTexts != value)
				{
					this._showDistanceTexts = value;
					base.OnPropertyChanged(value, "ShowDistanceTexts");
				}
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06000B87 RID: 2951 RVA: 0x000203DC File Offset: 0x0001E5DC
		// (set) Token: 0x06000B88 RID: 2952 RVA: 0x000203E4 File Offset: 0x0001E5E4
		[DataSourceProperty]
		public int TeamType
		{
			get
			{
				return this._teamType;
			}
			set
			{
				if (this._teamType != value)
				{
					this._teamType = value;
					base.OnPropertyChanged(value, "TeamType");
					this._isMarkersDirty = true;
				}
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06000B89 RID: 2953 RVA: 0x00020409 File Offset: 0x0001E609
		// (set) Token: 0x06000B8A RID: 2954 RVA: 0x00020411 File Offset: 0x0001E611
		[DataSourceProperty]
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

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06000B8B RID: 2955 RVA: 0x0002042F File Offset: 0x0001E62F
		// (set) Token: 0x06000B8C RID: 2956 RVA: 0x00020437 File Offset: 0x0001E637
		[DataSourceProperty]
		public float Distance
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

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06000B8D RID: 2957 RVA: 0x00020455 File Offset: 0x0001E655
		// (set) Token: 0x06000B8E RID: 2958 RVA: 0x0002045D File Offset: 0x0001E65D
		[DataSourceProperty]
		public string MarkerType
		{
			get
			{
				return this._markerType;
			}
			set
			{
				if (this._markerType != value)
				{
					this._markerType = value;
					base.OnPropertyChanged<string>(value, "MarkerType");
					this._isMarkersDirty = true;
				}
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06000B8F RID: 2959 RVA: 0x00020487 File Offset: 0x0001E687
		// (set) Token: 0x06000B90 RID: 2960 RVA: 0x0002048F File Offset: 0x0001E68F
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

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06000B91 RID: 2961 RVA: 0x000204B2 File Offset: 0x0001E6B2
		// (set) Token: 0x06000B92 RID: 2962 RVA: 0x000204BA File Offset: 0x0001E6BA
		[DataSourceProperty]
		public Brush IconBrush
		{
			get
			{
				return this._iconBrush;
			}
			set
			{
				if (this._iconBrush != value)
				{
					this._iconBrush = value;
					base.OnPropertyChanged<Brush>(value, "IconBrush");
				}
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06000B93 RID: 2963 RVA: 0x000204D8 File Offset: 0x0001E6D8
		// (set) Token: 0x06000B94 RID: 2964 RVA: 0x000204E0 File Offset: 0x0001E6E0
		[DataSourceProperty]
		public Widget FormationTypeMarker
		{
			get
			{
				return this._formationTypeMarker;
			}
			set
			{
				if (this._formationTypeMarker != value)
				{
					this._formationTypeMarker = value;
					base.OnPropertyChanged<Widget>(value, "FormationTypeMarker");
					this._isMarkersDirty = true;
				}
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x00020505 File Offset: 0x0001E705
		// (set) Token: 0x06000B96 RID: 2966 RVA: 0x0002050D File Offset: 0x0001E70D
		[DataSourceProperty]
		public Widget TeamTypeMarker
		{
			get
			{
				return this._teamTypeMarker;
			}
			set
			{
				if (this._teamTypeMarker != value)
				{
					this._teamTypeMarker = value;
					base.OnPropertyChanged<Widget>(value, "TeamTypeMarker");
					this._isMarkersDirty = true;
				}
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06000B97 RID: 2967 RVA: 0x00020532 File Offset: 0x0001E732
		// (set) Token: 0x06000B98 RID: 2968 RVA: 0x0002053A File Offset: 0x0001E73A
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
				}
			}
		}

		// Token: 0x0400052D RID: 1325
		private bool _isMarkersDirty = true;

		// Token: 0x0400052E RID: 1326
		private Vec2 _markerDefaultSize = Vec2.Invalid;

		// Token: 0x0400052F RID: 1327
		private bool _isMarkerEnabled;

		// Token: 0x04000530 RID: 1328
		private bool _isTargetingAFormation;

		// Token: 0x04000531 RID: 1329
		public bool _showDistanceTexts;

		// Token: 0x04000532 RID: 1330
		private bool _isActive = true;

		// Token: 0x04000533 RID: 1331
		private int _teamType;

		// Token: 0x04000534 RID: 1332
		private int _wSign;

		// Token: 0x04000535 RID: 1333
		private float _distance;

		// Token: 0x04000536 RID: 1334
		private string _markerType;

		// Token: 0x04000537 RID: 1335
		private Vec2 _position;

		// Token: 0x04000538 RID: 1336
		private Brush _iconBrush;

		// Token: 0x04000539 RID: 1337
		private Widget _formationTypeMarker;

		// Token: 0x0400053A RID: 1338
		private Widget _teamTypeMarker;

		// Token: 0x0400053B RID: 1339
		private TextWidget _nameTextWidget;
	}
}
