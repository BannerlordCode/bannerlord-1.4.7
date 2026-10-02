using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GamepadNavigation;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000022 RID: 34
	public class GamepadCursorWidget : BrushWidget
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00006C5F File Offset: 0x00004E5F
		// (set) Token: 0x060001BA RID: 442 RVA: 0x00006C67 File Offset: 0x00004E67
		private protected float TransitionTimer { protected get; private set; }

		// Token: 0x060001BB RID: 443 RVA: 0x00006C70 File Offset: 0x00004E70
		public GamepadCursorWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00006C7C File Offset: 0x00004E7C
		protected override void OnLateUpdate(float dt)
		{
			if (base.IsVisible)
			{
				this.RefreshTarget();
				bool flag = Input.IsKeyDown(InputKey.ControllerRDown);
				if (flag != this._isPressing)
				{
					this._animationRatioTimer = 0f;
					this.TransitionTimer = 0f;
					this._additionalOffsetBeforeStateChange = this._additionalOffset;
				}
				this._isPressing = flag;
				if (this._animationRatioTimer < 1.4f)
				{
					this._animationRatioTimer = MathF.Min(this._animationRatioTimer + dt, 1.4f);
				}
				bool flag2 = !this._targetChangedThisFrame && this._targetPositionChangedThisFrame;
				this._animationRatio = ((this.HasTarget && !this._isPressing) ? MathF.Clamp(17f * dt, 0f, 1f) : MathF.Lerp(this._animationRatio, 1f, this._animationRatioTimer / 1.4f, 1E-05f));
				this.UpdateAdditionalOffsets();
				this.AnimateToTarget(this._animationRatio);
				if (!flag2)
				{
					this.TransitionTimer += dt;
				}
			}
			this._targetChangedThisFrame = false;
			this._targetPositionChangedThisFrame = false;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00006D8C File Offset: 0x00004F8C
		private void AnimateToTarget(float ratio)
		{
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			if (this.HasTarget)
			{
				Rectangle2D gamepadCursorAreaRect = this._targetWidget.GamepadCursorAreaRect;
				num = gamepadCursorAreaRect.LocalScale.X;
				num2 = gamepadCursorAreaRect.LocalScale.Y;
				num3 = gamepadCursorAreaRect.GetCachedOrigin().X - base.EventManager.LeftUsableAreaStart;
				num4 = gamepadCursorAreaRect.GetCachedOrigin().Y - base.EventManager.TopUsableAreaStart;
				num5 = this._targetWidget.GlobalRotation;
			}
			else
			{
				num = this.DefaultSizeX * base._scaleToUse;
				num2 = this.DefaultSizeY * base._scaleToUse;
				num3 = this.CursorParentWidget.XOffset - num * 0.5f;
				num4 = this.CursorParentWidget.YOffset - num2 * 0.5f;
				num5 = 0f;
			}
			num3 -= this._additionalOffset;
			num4 -= this._additionalOffset;
			num += this._additionalOffset * 2f;
			num2 += this._additionalOffset * 2f;
			base.ScaledPositionXOffset = Mathf.Lerp(base.ScaledPositionXOffset, num3, ratio);
			base.ScaledPositionYOffset = Mathf.Lerp(base.ScaledPositionYOffset, num4, ratio);
			base.ScaledSuggestedWidth = Mathf.Lerp(base.ScaledSuggestedWidth, num, ratio);
			base.ScaledSuggestedHeight = Mathf.Lerp(base.ScaledSuggestedHeight, num2, ratio);
			base.Rotation = Mathf.Lerp(base.Rotation, num5, ratio);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00006EE4 File Offset: 0x000050E4
		private void RefreshTarget()
		{
			GauntletGamepadNavigationManager instance = GauntletGamepadNavigationManager.Instance;
			Widget widget = ((instance != null) ? instance.LastTargetedWidget : null);
			this._targetChangedThisFrame = this._targetWidget != widget;
			this._targetWidget = widget;
			this.TargetHasAction = GauntletGamepadNavigationManager.Instance.TargetedWidgetHasAction;
			this.HasTarget = this._targetWidget != null;
			this.CursorParentWidget.HasTarget = this.HasTarget;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00006F4C File Offset: 0x0000514C
		private void UpdateAdditionalOffsets()
		{
			float num2;
			if (this.TargetHasAction && !this._isPressing)
			{
				float num = (MathF.Sin(this.TransitionTimer / this.ActionAnimationTime * 1.6f) + 1f) / 2f;
				num2 = MathF.Lerp(this.DefaultOffset, this.HoverOffset, num, 1E-05f) - this.DefaultOffset;
			}
			else
			{
				num2 = 0f;
			}
			float num3;
			if (this._isPressing)
			{
				num3 = (this.HasTarget ? this.PressOffset : (this.DefaultTargetlessOffset * 0.7f));
			}
			else if (this.HasTarget)
			{
				num3 = this.DefaultOffset;
			}
			else
			{
				num3 = this.DefaultTargetlessOffset;
			}
			this._additionalOffset = (num3 + num2) * base._scaleToUse;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00007004 File Offset: 0x00005204
		private void ResetAnimations()
		{
			if (!this._isPressing)
			{
				this.TransitionTimer = 0f;
				this._additionalOffsetBeforeStateChange = this._additionalOffset;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00007025 File Offset: 0x00005225
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x0000702D File Offset: 0x0000522D
		public GamepadCursorParentWidget CursorParentWidget
		{
			get
			{
				return this._cursorParentWidget;
			}
			set
			{
				if (value != this._cursorParentWidget)
				{
					this._cursorParentWidget = value;
					base.OnPropertyChanged<GamepadCursorParentWidget>(value, "CursorParentWidget");
				}
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000704B File Offset: 0x0000524B
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x00007053 File Offset: 0x00005253
		public GamepadCursorMarkerWidget TopLeftMarker
		{
			get
			{
				return this._topLeftMarker;
			}
			set
			{
				if (value != this._topLeftMarker)
				{
					this._topLeftMarker = value;
					base.OnPropertyChanged<GamepadCursorMarkerWidget>(value, "TopLeftMarker");
				}
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00007071 File Offset: 0x00005271
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00007079 File Offset: 0x00005279
		public GamepadCursorMarkerWidget TopRightMarker
		{
			get
			{
				return this._topRightMarker;
			}
			set
			{
				if (value != this._topRightMarker)
				{
					this._topRightMarker = value;
					base.OnPropertyChanged<GamepadCursorMarkerWidget>(value, "TopRightMarker");
				}
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00007097 File Offset: 0x00005297
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x0000709F File Offset: 0x0000529F
		public GamepadCursorMarkerWidget BottomLeftMarker
		{
			get
			{
				return this._bottomLeftMarker;
			}
			set
			{
				if (value != this._bottomLeftMarker)
				{
					this._bottomLeftMarker = value;
					base.OnPropertyChanged<GamepadCursorMarkerWidget>(value, "BottomLeftMarker");
				}
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x000070BD File Offset: 0x000052BD
		// (set) Token: 0x060001CA RID: 458 RVA: 0x000070C5 File Offset: 0x000052C5
		public GamepadCursorMarkerWidget BottomRightMarker
		{
			get
			{
				return this._bottomRightMarker;
			}
			set
			{
				if (value != this._bottomRightMarker)
				{
					this._bottomRightMarker = value;
					base.OnPropertyChanged<GamepadCursorMarkerWidget>(value, "BottomRightMarker");
				}
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001CB RID: 459 RVA: 0x000070E3 File Offset: 0x000052E3
		// (set) Token: 0x060001CC RID: 460 RVA: 0x000070EB File Offset: 0x000052EB
		public bool HasTarget
		{
			get
			{
				return this._hasTarget;
			}
			set
			{
				if (value != this._hasTarget)
				{
					this._hasTarget = value;
					base.OnPropertyChanged(value, "HasTarget");
					this.ResetAnimations();
					this._animationRatioTimer = 0f;
				}
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001CD RID: 461 RVA: 0x0000711A File Offset: 0x0000531A
		// (set) Token: 0x060001CE RID: 462 RVA: 0x00007122 File Offset: 0x00005322
		public bool TargetHasAction
		{
			get
			{
				return this._targetHasAction;
			}
			set
			{
				if (value != this._targetHasAction)
				{
					this._targetHasAction = value;
					base.OnPropertyChanged(value, "TargetHasAction");
					this.ResetAnimations();
				}
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00007146 File Offset: 0x00005346
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x0000714E File Offset: 0x0000534E
		public float DefaultOffset
		{
			get
			{
				return this._defaultOffset;
			}
			set
			{
				if (value != this._defaultOffset)
				{
					this._defaultOffset = value;
					base.OnPropertyChanged(value, "DefaultOffset");
					this.ResetAnimations();
				}
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00007172 File Offset: 0x00005372
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x0000717A File Offset: 0x0000537A
		public float HoverOffset
		{
			get
			{
				return this._hoverOffset;
			}
			set
			{
				if (value != this._hoverOffset)
				{
					this._hoverOffset = value;
					base.OnPropertyChanged(value, "HoverOffset");
					this.ResetAnimations();
				}
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000719E File Offset: 0x0000539E
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x000071A6 File Offset: 0x000053A6
		public float DefaultTargetlessOffset
		{
			get
			{
				return this._defaultTargetlessOffset;
			}
			set
			{
				if (value != this._defaultTargetlessOffset)
				{
					this._defaultTargetlessOffset = value;
					base.OnPropertyChanged(value, "DefaultTargetlessOffset");
					this.ResetAnimations();
				}
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x000071CA File Offset: 0x000053CA
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x000071D2 File Offset: 0x000053D2
		public float PressOffset
		{
			get
			{
				return this._pressOffset;
			}
			set
			{
				if (value != this._pressOffset)
				{
					this._pressOffset = value;
					base.OnPropertyChanged(value, "PressOffset");
					this.ResetAnimations();
				}
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x000071F6 File Offset: 0x000053F6
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x000071FE File Offset: 0x000053FE
		public float DefaultSizeX
		{
			get
			{
				return this._defaultSizeX;
			}
			set
			{
				if (value != this._defaultSizeX)
				{
					this._defaultSizeX = value;
					base.OnPropertyChanged(value, "DefaultSizeX");
					this.ResetAnimations();
				}
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00007222 File Offset: 0x00005422
		// (set) Token: 0x060001DA RID: 474 RVA: 0x0000722A File Offset: 0x0000542A
		public float DefaultSizeY
		{
			get
			{
				return this._defaultSizeY;
			}
			set
			{
				if (value != this._defaultSizeY)
				{
					this._defaultSizeY = value;
					base.OnPropertyChanged(value, "DefaultSizeY");
					this.ResetAnimations();
				}
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001DB RID: 475 RVA: 0x0000724E File Offset: 0x0000544E
		// (set) Token: 0x060001DC RID: 476 RVA: 0x00007256 File Offset: 0x00005456
		public float ActionAnimationTime
		{
			get
			{
				return this._actionAnimationTime;
			}
			set
			{
				if (value != this._actionAnimationTime)
				{
					this._actionAnimationTime = value;
					base.OnPropertyChanged(value, "ActionAnimationTime");
					this.ResetAnimations();
				}
			}
		}

		// Token: 0x040000CC RID: 204
		private Widget _targetWidget;

		// Token: 0x040000CD RID: 205
		private bool _targetChangedThisFrame;

		// Token: 0x040000CE RID: 206
		private bool _targetPositionChangedThisFrame;

		// Token: 0x040000CF RID: 207
		private float _animationRatio;

		// Token: 0x040000D0 RID: 208
		private float _animationRatioTimer;

		// Token: 0x040000D1 RID: 209
		protected bool _isPressing;

		// Token: 0x040000D2 RID: 210
		protected bool _areBrushesValidated;

		// Token: 0x040000D4 RID: 212
		protected float _additionalOffset;

		// Token: 0x040000D5 RID: 213
		protected float _additionalOffsetBeforeStateChange;

		// Token: 0x040000D6 RID: 214
		protected float _leftOffset;

		// Token: 0x040000D7 RID: 215
		protected float _rightOffset;

		// Token: 0x040000D8 RID: 216
		protected float _topOffset;

		// Token: 0x040000D9 RID: 217
		protected float _bottomOffset;

		// Token: 0x040000DA RID: 218
		private GamepadCursorParentWidget _cursorParentWidget;

		// Token: 0x040000DB RID: 219
		private GamepadCursorMarkerWidget _topLeftMarker;

		// Token: 0x040000DC RID: 220
		private GamepadCursorMarkerWidget _topRightMarker;

		// Token: 0x040000DD RID: 221
		private GamepadCursorMarkerWidget _bottomLeftMarker;

		// Token: 0x040000DE RID: 222
		private GamepadCursorMarkerWidget _bottomRightMarker;

		// Token: 0x040000DF RID: 223
		private bool _hasTarget;

		// Token: 0x040000E0 RID: 224
		private bool _targetHasAction;

		// Token: 0x040000E1 RID: 225
		private float _defaultOffset;

		// Token: 0x040000E2 RID: 226
		private float _hoverOffset;

		// Token: 0x040000E3 RID: 227
		private float _defaultTargetlessOffset;

		// Token: 0x040000E4 RID: 228
		private float _pressOffset;

		// Token: 0x040000E5 RID: 229
		private float _defaultSizeX;

		// Token: 0x040000E6 RID: 230
		private float _defaultSizeY;

		// Token: 0x040000E7 RID: 231
		private float _actionAnimationTime;
	}
}
