using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map
{
	// Token: 0x02000119 RID: 281
	public class MobilePartyTrackerItemWidget : Widget
	{
		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x00028DE4 File Offset: 0x00026FE4
		// (set) Token: 0x06000EDA RID: 3802 RVA: 0x00028DEC File Offset: 0x00026FEC
		public Widget FrameVisualWidget { get; set; }

		// Token: 0x06000EDB RID: 3803 RVA: 0x00028DF5 File Offset: 0x00026FF5
		public MobilePartyTrackerItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x00028DFE File Offset: 0x00026FFE
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.UpdateScreenPosition();
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x00028E10 File Offset: 0x00027010
		private void UpdateScreenPosition()
		{
			this._screenWidth = base.Context.EventManager.PageSize.X;
			this._screenHeight = base.Context.EventManager.PageSize.Y;
			if (!this.IsActive)
			{
				base.IsHidden = true;
				return;
			}
			Vec2 vec = new Vec2(this.Position);
			if (this.IsTracked)
			{
				if (!this.IsBehind && vec.X - base.Size.X / 2f > 0f && vec.x + base.Size.X / 2f < base.Context.EventManager.PageSize.X && vec.y > 0f && vec.y + base.Size.Y < base.Context.EventManager.PageSize.Y)
				{
					base.ScaledPositionXOffset = vec.x - base.Size.X / 2f;
					base.ScaledPositionYOffset = vec.y;
				}
				else
				{
					Vec2 vec2 = new Vec2(base.Context.EventManager.PageSize.X / 2f, base.Context.EventManager.PageSize.Y / 2f);
					vec -= vec2;
					if (this.IsBehind)
					{
						vec *= -1f;
					}
					float num = Mathf.Atan2(vec.y, vec.x) - 1.5707964f;
					float num2 = Mathf.Cos(num);
					float num3 = Mathf.Sin(num);
					float num4 = num2 / num3;
					Vec2 vec3 = vec2 * 1f;
					vec = ((num2 > 0f) ? new Vec2(-vec3.y / num4, vec2.y) : new Vec2(vec3.y / num4, -vec2.y));
					if (vec.x > vec3.x)
					{
						vec = new Vec2(vec3.x, -vec3.x * num4);
					}
					else if (vec.x < -vec3.x)
					{
						vec = new Vec2(-vec3.x, vec3.x * num4);
					}
					vec += vec2;
					base.ScaledPositionXOffset = Mathf.Clamp(vec.x - base.Size.X / 2f, 0f, this._screenWidth - base.Size.X);
					base.ScaledPositionYOffset = Mathf.Clamp(vec.y, 0f, this._screenHeight - (base.Size.Y + 55f));
				}
			}
			else
			{
				base.ScaledPositionXOffset = this.Position.x - base.Size.X / 2f;
				base.ScaledPositionYOffset = this.Position.y;
			}
			base.IsHidden = (!this.IsTracked && this.IsBehind) || base.ScaledPositionXOffset > base.Context.TwoDimensionContext.Width || base.ScaledPositionYOffset > base.Context.TwoDimensionContext.Height || base.ScaledPositionXOffset + base.Size.X < 0f || base.ScaledPositionYOffset + base.Size.Y < 0f;
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x00029178 File Offset: 0x00027378
		private void UpdateTrackerVisual()
		{
			if (this.FrameVisualWidget != null && this.TrackerImageBrush != null && !string.IsNullOrEmpty(this.TrackerType))
			{
				Widget frameVisualWidget = this.FrameVisualWidget;
				BrushLayer layer = this.TrackerImageBrush.GetLayer(this.TrackerType);
				frameVisualWidget.Sprite = ((layer != null) ? layer.Sprite : null);
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06000EDF RID: 3807 RVA: 0x000291CA File Offset: 0x000273CA
		// (set) Token: 0x06000EE0 RID: 3808 RVA: 0x000291D2 File Offset: 0x000273D2
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

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06000EE1 RID: 3809 RVA: 0x000291F0 File Offset: 0x000273F0
		// (set) Token: 0x06000EE2 RID: 3810 RVA: 0x000291F8 File Offset: 0x000273F8
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

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x00029216 File Offset: 0x00027416
		// (set) Token: 0x06000EE4 RID: 3812 RVA: 0x0002921E File Offset: 0x0002741E
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

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x0002923C File Offset: 0x0002743C
		// (set) Token: 0x06000EE6 RID: 3814 RVA: 0x00029244 File Offset: 0x00027444
		public string TrackerType
		{
			get
			{
				return this._trackerType;
			}
			set
			{
				if (this._trackerType != value)
				{
					this._trackerType = value;
					base.OnPropertyChanged<string>(value, "TrackerType");
					this.UpdateTrackerVisual();
				}
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06000EE7 RID: 3815 RVA: 0x0002926D File Offset: 0x0002746D
		// (set) Token: 0x06000EE8 RID: 3816 RVA: 0x00029275 File Offset: 0x00027475
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

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06000EE9 RID: 3817 RVA: 0x00029298 File Offset: 0x00027498
		// (set) Token: 0x06000EEA RID: 3818 RVA: 0x000292A0 File Offset: 0x000274A0
		public Brush TrackerImageBrush
		{
			get
			{
				return this._trackerImageBrush;
			}
			set
			{
				if (this._trackerImageBrush != value)
				{
					this._trackerImageBrush = value;
					base.OnPropertyChanged<Brush>(value, "TrackerImageBrush");
					this.UpdateTrackerVisual();
				}
			}
		}

		// Token: 0x040006C3 RID: 1731
		private float _screenWidth;

		// Token: 0x040006C4 RID: 1732
		private float _screenHeight;

		// Token: 0x040006C5 RID: 1733
		private bool _isActive;

		// Token: 0x040006C6 RID: 1734
		private bool _isBehind;

		// Token: 0x040006C7 RID: 1735
		private bool _isTracked;

		// Token: 0x040006C8 RID: 1736
		private string _trackerType;

		// Token: 0x040006C9 RID: 1737
		private Vec2 _position;

		// Token: 0x040006CA RID: 1738
		private Brush _trackerImageBrush;
	}
}
