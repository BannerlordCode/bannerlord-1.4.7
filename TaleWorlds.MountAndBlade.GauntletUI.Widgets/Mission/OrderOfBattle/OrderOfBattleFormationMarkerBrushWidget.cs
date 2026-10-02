using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000EE RID: 238
	public class OrderOfBattleFormationMarkerBrushWidget : BrushWidget
	{
		// Token: 0x06000C3F RID: 3135 RVA: 0x0002176E File Offset: 0x0001F96E
		public OrderOfBattleFormationMarkerBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00021778 File Offset: 0x0001F978
		protected override void OnUpdate(float dt)
		{
			base.IsVisible = this.IsAvailable && this.WSign > 0;
			if (base.IsVisible)
			{
				base.ScaledPositionXOffset = this.Position.x - base.Size.X / 2f;
				base.ScaledPositionYOffset = this.Position.y - base.Size.Y / 2f;
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06000C41 RID: 3137 RVA: 0x000217ED File Offset: 0x0001F9ED
		// (set) Token: 0x06000C42 RID: 3138 RVA: 0x000217F5 File Offset: 0x0001F9F5
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

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06000C43 RID: 3139 RVA: 0x00021818 File Offset: 0x0001FA18
		// (set) Token: 0x06000C44 RID: 3140 RVA: 0x00021820 File Offset: 0x0001FA20
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

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x0002183E File Offset: 0x0001FA3E
		// (set) Token: 0x06000C46 RID: 3142 RVA: 0x00021846 File Offset: 0x0001FA46
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

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06000C47 RID: 3143 RVA: 0x00021864 File Offset: 0x0001FA64
		// (set) Token: 0x06000C48 RID: 3144 RVA: 0x0002186C File Offset: 0x0001FA6C
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

		// Token: 0x04000587 RID: 1415
		private Vec2 _position;

		// Token: 0x04000588 RID: 1416
		private bool _isAvailable;

		// Token: 0x04000589 RID: 1417
		private bool _isTracked;

		// Token: 0x0400058A RID: 1418
		private int _wSign;
	}
}
