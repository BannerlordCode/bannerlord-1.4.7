using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GamepadNavigation;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000021 RID: 33
	public class GamepadCursorParentWidget : Widget
	{
		// Token: 0x060001AF RID: 431 RVA: 0x00006B12 File Offset: 0x00004D12
		public GamepadCursorParentWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00006B1C File Offset: 0x00004D1C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.CenterWidget.SetGlobalAlphaRecursively(MathF.Lerp(this.CenterWidget.AlphaFactor, this.HasTarget ? 0.67f : 1f, 0.16f, 1E-05f));
			GauntletGamepadNavigationManager instance = GauntletGamepadNavigationManager.Instance;
			Widget widget = ((instance != null) ? instance.LastTargetedWidget : null);
			if (widget != null)
			{
				this.CenterWidget.PivotX = 0.5f;
				this.CenterWidget.PivotY = 0.5f;
				this.CenterWidget.Rotation = widget.GlobalRotation;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00006BAF File Offset: 0x00004DAF
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x00006BB7 File Offset: 0x00004DB7
		public float XOffset
		{
			get
			{
				return this._xOffset;
			}
			set
			{
				if (value != this._xOffset)
				{
					this._xOffset = value;
					base.OnPropertyChanged(value, "XOffset");
					this.CenterWidget.ScaledPositionXOffset = value;
				}
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00006BE1 File Offset: 0x00004DE1
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x00006BE9 File Offset: 0x00004DE9
		public float YOffset
		{
			get
			{
				return this._yOffset;
			}
			set
			{
				if (value != this._yOffset)
				{
					this._yOffset = value;
					base.OnPropertyChanged(value, "YOffset");
					this.CenterWidget.ScaledPositionYOffset = value;
				}
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00006C13 File Offset: 0x00004E13
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00006C1B File Offset: 0x00004E1B
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
				}
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00006C39 File Offset: 0x00004E39
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x00006C41 File Offset: 0x00004E41
		public BrushWidget CenterWidget
		{
			get
			{
				return this._centerWidget;
			}
			set
			{
				if (value != this._centerWidget)
				{
					this._centerWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "CenterWidget");
				}
			}
		}

		// Token: 0x040000C8 RID: 200
		private float _xOffset;

		// Token: 0x040000C9 RID: 201
		private float _yOffset;

		// Token: 0x040000CA RID: 202
		private bool _hasTarget;

		// Token: 0x040000CB RID: 203
		private BrushWidget _centerWidget;
	}
}
