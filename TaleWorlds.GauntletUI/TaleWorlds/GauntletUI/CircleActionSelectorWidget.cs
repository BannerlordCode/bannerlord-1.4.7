using System;
using System.Numerics;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200001C RID: 28
	public class CircleActionSelectorWidget : Widget
	{
		// Token: 0x0600021F RID: 543 RVA: 0x0000B584 File Offset: 0x00009784
		public CircleActionSelectorWidget(UIContext context)
			: base(context)
		{
			this._activateOnlyWithController = false;
			this._distanceFromCenterModifier = 300f;
			this._directionWidgetDistanceMultiplier = 0.5f;
			this._centerDistanceAnimationTimer = -1f;
			this._centerDistanceAnimationDuration = -1f;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000B5C0 File Offset: 0x000097C0
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.boolPropertyChanged += this.OnChildPropertyChanged;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000B5DB File Offset: 0x000097DB
		private void OnChildPropertyChanged(PropertyOwnerObject widget, string propertyName, bool value)
		{
			if (propertyName == "IsSelected" && base.EventManager.IsControllerActive && !this._isRefreshingSelection)
			{
				this._mouseDirection = Vec2.Zero;
				this._mouseMoveAccumulated = Vec2.Zero;
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000B618 File Offset: 0x00009818
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this.AllowInvalidSelection)
			{
				this._currentSelectedIndex = -1;
			}
			if (base.IsRecursivelyVisible())
			{
				this.UpdateItemPlacement();
				this.AnimateDistanceFromCenter(dt);
				bool flag = this.IsCircularInputEnabled && (!this.ActivateOnlyWithController || base.EventManager.IsControllerActive);
				if (this.DirectionWidget != null)
				{
					this.DirectionWidget.IsVisible = flag;
				}
				if (flag)
				{
					this.UpdateAverageMouseDirection();
					this.UpdateCircularInput();
					return;
				}
			}
			else
			{
				if (this._mouseDirection.X != 0f || this._mouseDirection.Y != 0f)
				{
					this._mouseDirection = default(Vec2);
				}
				if (this.DirectionWidget != null)
				{
					this.DirectionWidget.IsVisible = false;
				}
				this._mouseMoveAccumulated = Vec2.Zero;
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000B6E8 File Offset: 0x000098E8
		private void AnimateDistanceFromCenter(float dt)
		{
			if (this._centerDistanceAnimationTimer == -1f || this._centerDistanceAnimationDuration == -1f || this._centerDistanceAnimationTarget == -1f)
			{
				return;
			}
			if (this._centerDistanceAnimationTimer < this._centerDistanceAnimationDuration)
			{
				this.DistanceFromCenterModifier = MathF.Lerp(this._centerDistanceAnimationInitialValue, this._centerDistanceAnimationTarget, this._centerDistanceAnimationTimer / this._centerDistanceAnimationDuration, 1E-05f);
				this._centerDistanceAnimationTimer += dt;
				return;
			}
			this.DistanceFromCenterModifier = this._centerDistanceAnimationTarget;
			this._centerDistanceAnimationTimer = -1f;
			this._centerDistanceAnimationDuration = -1f;
			this._centerDistanceAnimationTarget = -1f;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000B790 File Offset: 0x00009990
		public void AnimateDistanceFromCenterTo(float distanceFromCenter, float animationDuration)
		{
			this._centerDistanceAnimationTimer = 0f;
			this._centerDistanceAnimationInitialValue = this.DistanceFromCenterModifier;
			this._centerDistanceAnimationDuration = animationDuration;
			this._centerDistanceAnimationTarget = distanceFromCenter;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000B7B8 File Offset: 0x000099B8
		private void UpdateAverageMouseDirection()
		{
			bool isMouseActive = base.Context.InputContext.GetIsMouseActive();
			Vector2 vector = (isMouseActive ? base.Context.InputContext.GetMouseMovement() : base.Context.InputContext.GetControllerRightStickState());
			if (isMouseActive)
			{
				this._mouseMoveAccumulated += vector;
				if (this._mouseMoveAccumulated.LengthSquared > 15625f)
				{
					this._mouseMoveAccumulated.Normalize();
					this._mouseMoveAccumulated *= 125f;
				}
				this._mouseDirection = new Vec2(this._mouseMoveAccumulated.X, -this._mouseMoveAccumulated.Y);
				return;
			}
			this._mouseDirection = new Vec2(vector.X, vector.Y);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000B884 File Offset: 0x00009A84
		private void UpdateItemPlacement()
		{
			if (base.ChildCount > 0)
			{
				int childCount = base.ChildCount;
				float num = 360f / (float)childCount;
				float num2 = -(num / 2f);
				if (num2 < 0f)
				{
					num2 += 360f;
				}
				for (int i = 0; i < base.ChildCount; i++)
				{
					float num3 = num * (float)i;
					float num4 = this.AddAngle(num2, num3);
					num4 = this.AddAngle(num4, num / 2f);
					Vec2 vec = this.DirFromAngle(num4 * 0.017453292f);
					Widget child = base.GetChild(i);
					child.PositionXOffset = vec.X * this.DistanceFromCenterModifier;
					child.PositionYOffset = vec.Y * this.DistanceFromCenterModifier * -1f;
				}
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000B93D File Offset: 0x00009B3D
		public bool TrySetSelectedIndex(int index)
		{
			if (index >= 0 && index < base.ChildCount)
			{
				this.OnSelectedIndexChanged(index);
				return true;
			}
			return false;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000B958 File Offset: 0x00009B58
		protected virtual void OnSelectedIndexChanged(int selectedIndex)
		{
			for (int i = 0; i < base.ChildCount; i++)
			{
				Widget child = base.GetChild(i);
				ButtonWidget buttonWidget;
				if ((buttonWidget = child as ButtonWidget) != null)
				{
					buttonWidget.IsSelected = !child.IsDisabled && i == selectedIndex;
				}
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000B9A0 File Offset: 0x00009BA0
		private void UpdateCircularInput()
		{
			int currentSelectedIndex = this._currentSelectedIndex;
			if (this._mouseDirection.Length > 0.391f)
			{
				if (base.ChildCount > 0)
				{
					float num = this.AngleFromDir(this._mouseDirection);
					this._currentSelectedIndex = this.GetIndexOfSelectedItemByAngle(num);
				}
			}
			else if (this.AllowInvalidSelection)
			{
				this._currentSelectedIndex = -1;
			}
			if (currentSelectedIndex != this._currentSelectedIndex)
			{
				this._isRefreshingSelection = true;
				this.OnSelectedIndexChanged(this._currentSelectedIndex);
				this._isRefreshingSelection = false;
			}
			if (this.DirectionWidget != null)
			{
				if (this._mouseDirection.LengthSquared > 0f)
				{
					Vec2 vec = this._mouseDirection.Normalized();
					this.DirectionWidget.PositionXOffset = vec.X * (this.DistanceFromCenterModifier * this.DirectionWidgetDistanceMultiplier);
					this.DirectionWidget.PositionYOffset = -vec.Y * (this.DistanceFromCenterModifier * this.DirectionWidgetDistanceMultiplier);
					return;
				}
				this.DirectionWidget.PositionXOffset = 0f;
				this.DirectionWidget.PositionYOffset = 0f;
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000BAA8 File Offset: 0x00009CA8
		private int GetIndexOfSelectedItemByAngle(float mouseDirectionAngle)
		{
			int childCount = base.ChildCount;
			float num = 360f / (float)childCount;
			float num2 = -(num / 2f);
			if (num2 < 0f)
			{
				num2 += 360f;
			}
			for (int i = 0; i < childCount; i++)
			{
				float num3 = num * (float)i;
				float num4 = num * (float)(i + 1);
				float num5 = this.AddAngle(num2, num3) * 0.017453292f;
				float num6 = this.AddAngle(num2, num4) * 0.017453292f;
				if (this.IsAngleBetweenAngles(mouseDirectionAngle * 0.017453292f, num5, num6))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000BB30 File Offset: 0x00009D30
		private float AddAngle(float angle1, float angle2)
		{
			float num = angle1 + angle2;
			if (num < 0f)
			{
				num += 360f;
			}
			return num % 360f;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000BB5C File Offset: 0x00009D5C
		private bool IsAngleBetweenAngles(float angle, float minAngle, float maxAngle)
		{
			float num = angle - 3.1415927f;
			float num2 = minAngle - 3.1415927f;
			float num3 = maxAngle - 3.1415927f;
			if (num2 == num3)
			{
				return true;
			}
			float num4 = MathF.Abs(MBMath.GetSmallestDifferenceBetweenTwoAngles(num3, num2));
			if (num4.ApproximatelyEqualsTo(3.1415927f, 1E-05f))
			{
				return num < num3;
			}
			float num5 = MathF.Abs(MBMath.GetSmallestDifferenceBetweenTwoAngles(num, num2));
			float num6 = MathF.Abs(MBMath.GetSmallestDifferenceBetweenTwoAngles(num, num3));
			return num5 < num4 && num6 < num4;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000BBD4 File Offset: 0x00009DD4
		private float AngleFromDir(Vec2 directionVector)
		{
			if (directionVector.X < 0f)
			{
				return 360f - (float)Math.Atan2((double)directionVector.X, (double)directionVector.Y) * 57.29578f * -1f;
			}
			return (float)Math.Atan2((double)directionVector.X, (double)directionVector.Y) * 57.29578f;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000BC34 File Offset: 0x00009E34
		private Vec2 DirFromAngle(float angle)
		{
			return new Vec2(MathF.Sin(angle), MathF.Cos(angle));
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000BC49 File Offset: 0x00009E49
		// (set) Token: 0x06000230 RID: 560 RVA: 0x0000BC51 File Offset: 0x00009E51
		public bool AllowInvalidSelection
		{
			get
			{
				return this._allowInvalidSelection;
			}
			set
			{
				if (value != this._allowInvalidSelection)
				{
					this._allowInvalidSelection = value;
					base.OnPropertyChanged(value, "AllowInvalidSelection");
				}
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000BC6F File Offset: 0x00009E6F
		// (set) Token: 0x06000232 RID: 562 RVA: 0x0000BC77 File Offset: 0x00009E77
		public bool ActivateOnlyWithController
		{
			get
			{
				return this._activateOnlyWithController;
			}
			set
			{
				if (value != this._activateOnlyWithController)
				{
					this._activateOnlyWithController = value;
					base.OnPropertyChanged(value, "ActivateOnlyWithController");
				}
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0000BC95 File Offset: 0x00009E95
		// (set) Token: 0x06000234 RID: 564 RVA: 0x0000BCA0 File Offset: 0x00009EA0
		public bool IsCircularInputEnabled
		{
			get
			{
				return !this.IsCircularInputDisabled;
			}
			set
			{
				if (value == this.IsCircularInputDisabled)
				{
					this.IsCircularInputDisabled = !value;
					base.OnPropertyChanged(!value, "IsCircularInputEnabled");
				}
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000BCC4 File Offset: 0x00009EC4
		// (set) Token: 0x06000236 RID: 566 RVA: 0x0000BCCC File Offset: 0x00009ECC
		public bool IsCircularInputDisabled
		{
			get
			{
				return this._isCircularInputDisabled;
			}
			set
			{
				if (value != this._isCircularInputDisabled)
				{
					this._isCircularInputDisabled = value;
					base.OnPropertyChanged(value, "IsCircularInputDisabled");
					if (value)
					{
						this.OnSelectedIndexChanged(-1);
					}
				}
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0000BCF4 File Offset: 0x00009EF4
		// (set) Token: 0x06000238 RID: 568 RVA: 0x0000BCFC File Offset: 0x00009EFC
		public float DistanceFromCenterModifier
		{
			get
			{
				return this._distanceFromCenterModifier;
			}
			set
			{
				if (value != this._distanceFromCenterModifier)
				{
					this._distanceFromCenterModifier = value;
					base.OnPropertyChanged(value, "DistanceFromCenterModifier");
				}
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0000BD1A File Offset: 0x00009F1A
		// (set) Token: 0x0600023A RID: 570 RVA: 0x0000BD22 File Offset: 0x00009F22
		public float DirectionWidgetDistanceMultiplier
		{
			get
			{
				return this._directionWidgetDistanceMultiplier;
			}
			set
			{
				if (value != this._directionWidgetDistanceMultiplier)
				{
					this._directionWidgetDistanceMultiplier = value;
					base.OnPropertyChanged(value, "DirectionWidgetDistanceMultiplier");
				}
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000BD40 File Offset: 0x00009F40
		// (set) Token: 0x0600023C RID: 572 RVA: 0x0000BD48 File Offset: 0x00009F48
		public Widget DirectionWidget
		{
			get
			{
				return this._directionWidget;
			}
			set
			{
				if (value != this._directionWidget)
				{
					this._directionWidget = value;
					base.OnPropertyChanged<Widget>(value, "DirectionWidget");
				}
			}
		}

		// Token: 0x04000117 RID: 279
		private int _currentSelectedIndex;

		// Token: 0x04000118 RID: 280
		private const float _mouseMoveMaxDistance = 125f;

		// Token: 0x04000119 RID: 281
		private const float _gamepadDeadzoneLength = 0.391f;

		// Token: 0x0400011A RID: 282
		private const float _mouseMoveMaxDistanceSquared = 15625f;

		// Token: 0x0400011B RID: 283
		private float _centerDistanceAnimationTimer;

		// Token: 0x0400011C RID: 284
		private float _centerDistanceAnimationDuration;

		// Token: 0x0400011D RID: 285
		private float _centerDistanceAnimationInitialValue;

		// Token: 0x0400011E RID: 286
		private float _centerDistanceAnimationTarget;

		// Token: 0x0400011F RID: 287
		private Vec2 _mouseDirection;

		// Token: 0x04000120 RID: 288
		private Vec2 _mouseMoveAccumulated;

		// Token: 0x04000121 RID: 289
		private bool _isRefreshingSelection;

		// Token: 0x04000122 RID: 290
		private bool _allowInvalidSelection;

		// Token: 0x04000123 RID: 291
		private bool _activateOnlyWithController;

		// Token: 0x04000124 RID: 292
		private bool _isCircularInputDisabled;

		// Token: 0x04000125 RID: 293
		private float _distanceFromCenterModifier;

		// Token: 0x04000126 RID: 294
		private float _directionWidgetDistanceMultiplier;

		// Token: 0x04000127 RID: 295
		private Widget _directionWidget;
	}
}
