using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.NameMarker
{
	// Token: 0x020000F4 RID: 244
	public class DuelTargetMarkerListPanel : ListPanel
	{
		// Token: 0x06000C7F RID: 3199 RVA: 0x00022182 File Offset: 0x00020382
		public DuelTargetMarkerListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0002218C File Offset: 0x0002038C
		protected override void OnLateUpdate(float dt)
		{
			if (!this.IsAvailable)
			{
				base.IsVisible = false;
				return;
			}
			float x = base.Context.EventManager.PageSize.X;
			float y = base.Context.EventManager.PageSize.Y;
			Vec2 vec = this.Position;
			if (this.WSign > 0 && vec.x - base.Size.X / 2f > 0f && vec.x + base.Size.X / 2f < base.Context.EventManager.PageSize.X && vec.y > 0f && vec.y + base.Size.Y < base.Context.EventManager.PageSize.Y)
			{
				base.ScaledPositionXOffset = vec.x - base.Size.X / 2f;
				base.ScaledPositionYOffset = vec.y - base.Size.Y - 20f;
				this._actionText.ScaledPositionXOffset = base.ScaledPositionXOffset;
				this._actionText.ScaledPositionYOffset = base.ScaledPositionYOffset + base.Size.Y;
				base.IsVisible = true;
				return;
			}
			if (this.IsTracked)
			{
				Vec2 vec2 = new Vec2(base.Context.EventManager.PageSize.X / 2f, base.Context.EventManager.PageSize.Y / 2f);
				vec -= vec2;
				if (this.WSign < 0)
				{
					vec *= -1f;
				}
				float num = Mathf.Atan2(vec.y, vec.x) - 1.5707964f;
				float num2 = Mathf.Cos(num);
				float num3 = Mathf.Sin(num);
				vec = vec2 + new Vec2(num3 * 150f, num2 * 150f);
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
				base.ScaledPositionXOffset = Mathf.Clamp(vec.x - base.Size.X / 2f, 0f, x - base.Size.X);
				base.ScaledPositionYOffset = Mathf.Clamp(vec.y - base.Size.Y, 0f, y - base.Size.Y);
				base.IsVisible = true;
				return;
			}
			base.IsVisible = false;
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x000224A8 File Offset: 0x000206A8
		private void UpdateChildrenFocusStates()
		{
			string text = (this.HasTargetSentDuelRequest ? "Tracked" : ((this.HasPlayerSentDuelRequest || this.IsAgentFocused) ? "Focused" : "Default"));
			this.Background.SetState(text);
			this.Border.SetState(text);
			BrushWidget troopClassBorder = this.TroopClassBorder;
			if (troopClassBorder == null)
			{
				return;
			}
			troopClassBorder.SetState(text);
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x0002250A File Offset: 0x0002070A
		// (set) Token: 0x06000C83 RID: 3203 RVA: 0x00022512 File Offset: 0x00020712
		[Editor(false)]
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (value != this._position)
				{
					this._position = value;
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x00022535 File Offset: 0x00020735
		// (set) Token: 0x06000C85 RID: 3205 RVA: 0x0002253D File Offset: 0x0002073D
		[Editor(false)]
		public bool IsAgentInScreenBoundaries
		{
			get
			{
				return this._isAgentInScreenBoundaries;
			}
			set
			{
				if (value != this._isAgentInScreenBoundaries)
				{
					this._isAgentInScreenBoundaries = value;
					base.OnPropertyChanged(value, "IsAgentInScreenBoundaries");
				}
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x0002255B File Offset: 0x0002075B
		// (set) Token: 0x06000C87 RID: 3207 RVA: 0x00022563 File Offset: 0x00020763
		[Editor(false)]
		public bool IsAvailable
		{
			get
			{
				return this._isAvailable;
			}
			set
			{
				if (value != this._isAvailable)
				{
					this._isAvailable = value;
					base.OnPropertyChanged(value, "IsAvailable");
				}
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x00022581 File Offset: 0x00020781
		// (set) Token: 0x06000C89 RID: 3209 RVA: 0x00022589 File Offset: 0x00020789
		[Editor(false)]
		public bool IsTracked
		{
			get
			{
				return this._isTracked;
			}
			set
			{
				if (value != this._isTracked)
				{
					this._isTracked = value;
					base.OnPropertyChanged(value, "IsTracked");
				}
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06000C8A RID: 3210 RVA: 0x000225A7 File Offset: 0x000207A7
		// (set) Token: 0x06000C8B RID: 3211 RVA: 0x000225AF File Offset: 0x000207AF
		[Editor(false)]
		public bool IsAgentFocused
		{
			get
			{
				return this._isAgentFocused;
			}
			set
			{
				if (value != this._isAgentFocused)
				{
					this._isAgentFocused = value;
					base.OnPropertyChanged(value, "IsAgentFocused");
					this.UpdateChildrenFocusStates();
				}
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06000C8C RID: 3212 RVA: 0x000225D3 File Offset: 0x000207D3
		// (set) Token: 0x06000C8D RID: 3213 RVA: 0x000225DB File Offset: 0x000207DB
		[Editor(false)]
		public bool HasTargetSentDuelRequest
		{
			get
			{
				return this._hasTargetSentDuelRequest;
			}
			set
			{
				if (value != this._hasTargetSentDuelRequest)
				{
					this._hasTargetSentDuelRequest = value;
					base.OnPropertyChanged(value, "HasTargetSentDuelRequest");
					this.UpdateChildrenFocusStates();
				}
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x000225FF File Offset: 0x000207FF
		// (set) Token: 0x06000C8F RID: 3215 RVA: 0x00022607 File Offset: 0x00020807
		[Editor(false)]
		public bool HasPlayerSentDuelRequest
		{
			get
			{
				return this._hasPlayerSentDuelRequest;
			}
			set
			{
				if (value != this._hasPlayerSentDuelRequest)
				{
					this._hasPlayerSentDuelRequest = value;
					base.OnPropertyChanged(value, "HasPlayerSentDuelRequest");
					this.UpdateChildrenFocusStates();
				}
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06000C90 RID: 3216 RVA: 0x0002262B File Offset: 0x0002082B
		// (set) Token: 0x06000C91 RID: 3217 RVA: 0x00022633 File Offset: 0x00020833
		[Editor(false)]
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

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x00022651 File Offset: 0x00020851
		// (set) Token: 0x06000C93 RID: 3219 RVA: 0x00022659 File Offset: 0x00020859
		[Editor(false)]
		public RichTextWidget ActionText
		{
			get
			{
				return this._actionText;
			}
			set
			{
				if (value != this._actionText)
				{
					this._actionText = value;
					base.OnPropertyChanged<RichTextWidget>(value, "ActionText");
				}
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x00022677 File Offset: 0x00020877
		// (set) Token: 0x06000C95 RID: 3221 RVA: 0x0002267F File Offset: 0x0002087F
		[Editor(false)]
		public BrushWidget Background
		{
			get
			{
				return this._background;
			}
			set
			{
				if (value != this._background)
				{
					this._background = value;
					base.OnPropertyChanged<BrushWidget>(value, "Background");
				}
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x0002269D File Offset: 0x0002089D
		// (set) Token: 0x06000C97 RID: 3223 RVA: 0x000226A5 File Offset: 0x000208A5
		[Editor(false)]
		public BrushWidget Border
		{
			get
			{
				return this._border;
			}
			set
			{
				if (value != this._border)
				{
					this._border = value;
					base.OnPropertyChanged<BrushWidget>(value, "Border");
				}
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x000226C3 File Offset: 0x000208C3
		// (set) Token: 0x06000C99 RID: 3225 RVA: 0x000226CB File Offset: 0x000208CB
		[Editor(false)]
		public BrushWidget TroopClassBorder
		{
			get
			{
				return this._troopClassBorder;
			}
			set
			{
				if (value != this._troopClassBorder)
				{
					this._troopClassBorder = value;
					base.OnPropertyChanged<BrushWidget>(value, "TroopClassBorder");
				}
			}
		}

		// Token: 0x040005A4 RID: 1444
		private const string DefaultState = "Default";

		// Token: 0x040005A5 RID: 1445
		private const string FocusedState = "Focused";

		// Token: 0x040005A6 RID: 1446
		private const string TrackedState = "Tracked";

		// Token: 0x040005A7 RID: 1447
		private Vec2 _position;

		// Token: 0x040005A8 RID: 1448
		private bool _isAgentInScreenBoundaries;

		// Token: 0x040005A9 RID: 1449
		private bool _isAvailable;

		// Token: 0x040005AA RID: 1450
		private bool _isTracked;

		// Token: 0x040005AB RID: 1451
		private bool _isAgentFocused;

		// Token: 0x040005AC RID: 1452
		private bool _hasTargetSentDuelRequest;

		// Token: 0x040005AD RID: 1453
		private bool _hasPlayerSentDuelRequest;

		// Token: 0x040005AE RID: 1454
		private int _wSign;

		// Token: 0x040005AF RID: 1455
		private RichTextWidget _actionText;

		// Token: 0x040005B0 RID: 1456
		private BrushWidget _background;

		// Token: 0x040005B1 RID: 1457
		private BrushWidget _border;

		// Token: 0x040005B2 RID: 1458
		private BrushWidget _troopClassBorder;
	}
}
